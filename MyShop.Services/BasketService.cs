using MyShop.Core;
using MyShop.Core.Models;
using MyShop.Core.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MyShop.Services
{
    public class BasketService : IBasketService
    {
        IRepository<Product> productContext;
        IRepository<Basket> basketContext;
        IRepository<BasketItem> basketItemContext;
        IRepository<WishList> wishListContext;

        public const string BasketSessionName = "eCommerceBasket";

        public BasketService(IRepository<Product> ProductContext, IRepository<Basket> BasketContext, IRepository<BasketItem> basketItemContext, IRepository<WishList> wishListContext)
        {
            this.productContext = ProductContext;
            this.basketContext = BasketContext;
            this.basketItemContext = basketItemContext;
            this.wishListContext = wishListContext;
        }

        private Basket GetBasket(HttpContextBase httpContext, bool createIfNull)
        {
            HttpCookie cookie = httpContext.Request.Cookies.Get(BasketSessionName);

            Basket basket = new Basket();

            if (cookie != null)
            {
                string basketId = cookie.Value;
                if (!string.IsNullOrEmpty(basketId))
                {
                    basket = basketContext.Find(basketId);
                }
                else
                {
                    if (createIfNull)
                    {
                        basket = CreateNewBasket(httpContext);
                    }
                }
            }
            else
            {
                if (createIfNull)
                {
                    basket = CreateNewBasket(httpContext);
                }
            }

            return basket;
        }

        private Basket CreateNewBasket(HttpContextBase httpContext)
        {
            Basket basket = new Basket();

            basketContext.Insert(basket);
            basketContext.Commit();

            HttpCookie cookie = new HttpCookie(BasketSessionName);
            cookie.Value = basket.Id;
            cookie.Expires = DateTime.Now.AddDays(1);
            httpContext.Response.Cookies.Add(cookie);

            return basket;
        }

        public void AddToBasket(HttpContextBase httpContext, string productId)
        {
            Basket basket = GetBasket(httpContext, true);
            BasketItem item = basket.BasketItems.FirstOrDefault(i => i.ProductId == productId);

            if (item == null)
            {
                item = new BasketItem()
                {
                    BasketId = basket.Id,
                    ProductId = productId,
                    Quantity = 1
                };

                basket.BasketItems.Add(item);
            }
            else
            {
                item.Quantity = item.Quantity + 1;
            }

            basketContext.Commit();
        }

        public void RemoveFromBasket(HttpContextBase httpContext, string itemId)
        {
            Basket basket = GetBasket(httpContext, true);
            BasketItem item = basket.BasketItems.FirstOrDefault(i => i.Id == itemId);

            if (item != null)
            {
                basket.BasketItems.Remove(item);
                basketContext.Commit();
            }
        }

        public List<BasketItemViewModel> GetBasketItems(HttpContextBase httpContext)
        {
            Basket basket = GetBasket(httpContext, true);

            if (basket != null)
            {
                var results = (from b in basket.BasketItems
                               join p in productContext.Collection() on b.ProductId equals p.Id
                               select new BasketItemViewModel()
                               {
                                   Id = b.Id,
                                   Quantity = b.Quantity,
                                   ProductName = p.Name,
                                   Image = p.Image,
                                   Price = p.Price
                               }).ToList();

                return results;
            }
            else
            {
                return new List<BasketItemViewModel>();
            }
        }

        public BasketSummaryViewModel GetBasketSummary(HttpContextBase httpcontext)
        {
            Basket basket = GetBasket(httpcontext, false);
            BasketSummaryViewModel model = new BasketSummaryViewModel(0, 0);

            if (basket != null)
            {
                int? basketCount = (from item in basket.BasketItems
                                    select item.Quantity).Sum();

                decimal? basketTotal = (from item in basket.BasketItems
                                        join p in productContext.Collection() on item.ProductId equals p.Id
                                        select item.Quantity * p.Price).Sum();

                model.BasketCount = basketCount ?? 0;
                model.BasketTotal = basketTotal ?? decimal.Zero;

                return model;
            }
            else
            {
                return model;
            }
        }

        public List<ProductViewModel> AddToWishList(Product product)
        {
            WishList wishList = new WishList();
            var productToAdd = wishListContext.Collection().FirstOrDefault(x => x.ProductId == product.Id);
            if(productToAdd == null)
            {
                wishList.ProductId = product.Id;
                wishList.ProductName = product.Name;
            }
            wishListContext.Insert(wishList);
            wishListContext.Commit();

            var listOfProductId = wishListContext.Collection().Select(x => x.ProductId);

            List<ProductViewModel> listOfProductViewModel = new List<ProductViewModel>();
            foreach (var productId in listOfProductId)
            {
                var productFromDb = productContext.Find(productId);
                if(productFromDb != null)
                {
                    ProductViewModel viewModel = new ProductViewModel();
                    viewModel.Image = productFromDb.Image ?? "";
                    viewModel.Name = productFromDb.Name;
                    viewModel.Category = productFromDb.Category;
                    viewModel.Description = productFromDb.Description;
                    viewModel.Price = productFromDb.Price;
                    viewModel.Id = productFromDb.Id;
                    listOfProductViewModel.Add(viewModel);
                }
            }
                return listOfProductViewModel;
        }

        public void RemoveFromWishList(string Id)
        {
            WishList itemToDelete = wishListContext.Collection().FirstOrDefault(x => x.Id == Id);
            if(itemToDelete != null)
            {
                wishListContext.Delete(Id);
                wishListContext.Commit();
            }
        }

        //public void AddToBasketPlus(string basketItemId)
        //{
        //    BasketItem basketItem = basketItemContext.Find(basketItemId);

        //    basketItem.Quantity += 1;

        //    basketItemContext.Commit();
        //}

        //public void AddToBasketMinus(string basketItemId)
        //{
        //    BasketItem basketItem = basketItemContext.Find(basketItemId);

        //    if(basketItem.Quantity == 1)
        //    {
        //        basketItemContext.Delete(basketItemId);
        //    }
        //    basketItem.Quantity -= 1;

        //    basketItemContext.Commit();
        //}

        public void AddToBasketPlusOrMinus(string basketItemId, bool isAdded)
        {
            BasketItem basketItem = basketItemContext.Find(basketItemId);

            if (isAdded)
            {
                basketItem.Quantity += 1;
            }
            else
            {
                if (basketItem.Quantity == 1)
                {
                    basketItemContext.Delete(basketItemId);
                }
                else
                {
                    basketItem.Quantity -= 1;
                }
            }
            basketItemContext.Commit();
        }

        public void ClearBasket(HttpContextBase httpContext)
        {
            Basket basket = GetBasket(httpContext, false);
            basket.BasketItems.Clear();
            basketContext.Commit();
        }
    }
}
