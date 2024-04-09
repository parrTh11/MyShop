using MyShop.Core.Models;
using MyShop.Core.ViewModels;
using System.Collections.Generic;
using System.Web;

namespace MyShop.Services
{
    public interface IBasketService
    {
        void AddToBasket(HttpContextBase httpContext, string productId, string userEmail);
        void RemoveFromBasket(HttpContextBase httpContext, string itemId, string userEmail);
        BasketSummaryViewModel GetBasketSummary(HttpContextBase httpcontext, string userEmail);
        List<BasketItemViewModel> GetBasketItems(HttpContextBase httpContext, string userEmail);
        void AddToBasketPlusOrMinus(string basketItemId, bool isAdded);
        void ClearBasket(HttpContextBase httpContext, string userEmail);
        void AddToWishList(Product product);
        void RemoveFromWishList(string Id);
        List<ProductViewModel> GetWishList();
    }
}