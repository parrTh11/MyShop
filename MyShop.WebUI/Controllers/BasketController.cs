using MyShop.Core.Contracts;
using MyShop.Core.Models;
using MyShop.Core.ViewModels;
using MyShop.Services;
using System.Web.Mvc;

namespace MyShop.WebUI.Controllers
{
    public class BasketController : Controller
    {
        IBasketService basketService;
        IOrderService orderService;

        public BasketController(IBasketService BasketService, IOrderService OrderService)
        {
            this.basketService = BasketService;
            this.orderService = OrderService;
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

        public ActionResult Checkout()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Checkout(Order order)
        {
            var basketItems = basketService.GetBasketItems(this.HttpContext);
            order.OrderStatus = "Order Created.";

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