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
        ICustomerService customerService;
        IRepository<Customer> customers;

        public BasketController(IBasketService BasketService, IOrderService OrderService, IRepository<Customer> Customers, ICustomerService CustomerService)
        {
            this.basketService = BasketService;
            this.orderService = OrderService;
            this.customers = Customers;
            this.customerService = CustomerService;
        }

        // GET: Basket
        public ActionResult Index()
        {
            var userEmail = User.Identity.Name;
            var model = basketService.GetBasketItems(this.HttpContext, userEmail);
            return View(model);
        }

        public ActionResult AddToBasket(string Id)
        {
            var userEmail = User.Identity.Name;
            basketService.AddToBasket(this.HttpContext, Id, userEmail);
            return RedirectToAction("Index");
        }

        public ActionResult RemoveFromBasket(string Id)
        {
            var userEmail = User.Identity.Name;
            basketService.RemoveFromBasket(this.HttpContext, Id, userEmail);
            return RedirectToAction("Index");
        }
        public PartialViewResult BasketSummary()
        {
            var userEmail = User.Identity.Name;
            var basketSummary = basketService.GetBasketSummary(this.HttpContext, userEmail);

            return PartialView(basketSummary);
        }

        public PartialViewResult SearchBox()
        {
            SearchBoxViewModel searchBox = new SearchBoxViewModel();
            return PartialView(searchBox);
        }

        public ActionResult WishListIndex()
        {
            var wishList = basketService.GetWishList();
            return View(wishList);
        }

        public ActionResult AddToWishList(Product product)
        {
            basketService.AddToWishList(product);
            return RedirectToAction("WishListIndex");
        }

        public ActionResult RemoveFromWishList(string Id)
        {
            basketService.RemoveFromWishList(Id);
            return RedirectToAction("WishListIndex");
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
            var userEmail = User.Identity.Name;
            var basketItems = basketService.GetBasketItems(this.HttpContext, userEmail);
            order.OrderStatus = "Order Created.";
            order.Email = User.Identity.Name;
            //process payment

            order.OrderStatus = "Payment Processed";
            orderService.CreateOrder(order, basketItems);
            basketService.ClearBasket(this.HttpContext, userEmail);

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