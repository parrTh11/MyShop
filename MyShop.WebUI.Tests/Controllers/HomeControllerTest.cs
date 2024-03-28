using Microsoft.VisualStudio.TestTools.UnitTesting;
using MyShop.Core;
using MyShop.Core.Models;
using MyShop.Core.ViewModels;
using MyShop.WebUI;
using MyShop.WebUI.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;

namespace MyShop.WebUI.Tests.Controllers
{
    [TestClass]
    public class HomeControllerTest
    {
        [TestMethod]
        public void IndexPageDoesReturnProducts()
        {
            IRepository<Product> ProductContext = new Mocks.MockContext<Product>();
            IRepository<ProductCategory> ProductCategoryContext = new Mocks.MockContext<ProductCategory>();

            ProductContext.Insert(new Product());

            HomeController controller = new HomeController(ProductContext, ProductCategoryContext);

            var result = controller.Index() as ViewResult;
            var viewModel = (ProductListViewModel)result.ViewData.Model;

            Assert.AreEqual(1, viewModel.Products.Count());
        }
    }
}
