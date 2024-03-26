using MyShop.Core.ViewModels;
using System.Collections.Generic;
using System.Web;

namespace MyShop.Services
{
    public interface IBasketService
    {
        void AddToBasket(HttpContextBase httpContext, string productId);
        void RemoveFromBasket(HttpContextBase httpContext, string itemId);
        BasketSummaryViewModel GetBasketSummary(HttpContextBase httpcontext);
        List<BasketItemViewModel> GetBasketItems(HttpContextBase httpContext);
    }
}