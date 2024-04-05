using MyShop.Core;
using MyShop.Core.Contracts;
using MyShop.Core.Models;
using MyShop.Core.ViewModels;
using MyShop.Services;
using System.Linq;
using System.Web.Mvc;

namespace MyShop.WebUI.Controllers
{
    public class BasketController : Controller
    {
        IBasketService basketService;
        IOrderService orderService;
        IRepository<Customer> customers;

        public BasketController(IBasketService BasketService, IOrderService OrderService, IRepository<Customer> Customers)
        {
            this.basketService = BasketService;
            this.orderService = OrderService;
            this.customers = Customers;
        }

        // GET: Basket
        public ActionResult Index()
        {
            var model = basketService.GetBasketItems(this.HttpContext);
            return View(model);
        }

        public ActionResult AddToBasket(string Id)
        {
            basketService.AddToBasket(this.HttpContext, Id);
            return RedirectToAction("Index");
        }

        public ActionResult RemoveFromBasket(string Id)
        {
            basketService.RemoveFromBasket(this.HttpContext, Id);
            return RedirectToAction("Index");
        }
        public PartialViewResult BasketSummary()
        {
            var basketSummary = basketService.GetBasketSummary(this.HttpContext);

            return PartialView(basketSummary);
        }

        public PartialViewResult SearchBox()
        {
            SearchBoxViewModel searchBox = new SearchBoxViewModel();
            return PartialView(searchBox);
        }

        public ActionResult AddToWishList(Product product)
        {
            var wishList = basketService.AddToWishList(product);
            return View(wishList);
        }

        public ActionResult RemoveFromWishList(string Id)
        {
            basketService.RemoveFromWishList(Id);
            return RedirectToAction("AddToWishList");
        }

        public ActionResult AddOrRemoveProductToBasket(string basketItemId, bool isAdded)
        {
            if (isAdded)
            {
                basketService.AddToBasketPlusOrMinus(basketItemId, true);
            }
            else
            {
                basketService.AddToBasketPlusOrMinus(basketItemId, false);
            }
            return RedirectToAction("Index");
        }

        [Authorize]
        public ActionResult Checkout()
        {
            Customer customer = customers.Collection().FirstOrDefault(x=>x.Email == User.Identity.Name);

            if(customer != null)
            {
                Order order = new Order()
                {
                    FirstName = customer.FirstName,
                    Street = customer.Street,
                    City = customer.City,
                    Email = customer.Email,
                    State = customer.State,
                    SurName = customer.LastName,
                    ZipCode = customer.ZipCode
                };

                return View(order);
            }
            else
            {
                return RedirectToAction("Error");
            }
            
        }

        [HttpPost]
        [Authorize]
        public ActionResult Checkout(Order order)
        {
            var basketItems = basketService.GetBasketItems(this.HttpContext);
            order.OrderStatus = "Order Created.";
            order.Email = User.Identity.Name;
            //process payment

            order.OrderStatus = "Payment Processed";
            orderService.CreateOrder(order, basketItems);
            basketService.ClearBasket(this.HttpContext);

            return RedirectToAction("Thankyou", new { OrderId = order.Id });
        }

        public ActionResult Thankyou(string OrderId)
        {
            ViewBag.OrderId = OrderId;
            return View();
        }

        //public ActionResult RemoveProductToBasket(string basketItemId)
        //{
        //    basketService.AddToBasketMinus(basketItemId);

        //    return RedirectToAction("Index");
        //}
    }
}