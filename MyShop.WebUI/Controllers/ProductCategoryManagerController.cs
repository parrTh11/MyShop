using MyShop.Core;
using MyShop.Core.Models;
using MyShop.Core.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace MyShop.WebUI.Controllers
{
    public class ProductCategoryManagerController : Controller
    {
        IRepository<ProductCategory> context;
        

        public ProductCategoryManagerController(IRepository<ProductCategory> context)
        {
            this.context = context;
          
        }
        // GET: ProductManager
        public ActionResult Index()
        {
            List<ProductCategory> productCategories = context.Collection().ToList();
            return View(productCategories);
        }

        public ActionResult Create()
        {
            ProductCategoryViewModel productCategories = new ProductCategoryViewModel();
            return View(productCategories);
        }

        [HttpPost]
        public ActionResult Create(ProductCategoryViewModel productCategories)
        {
            if (!ModelState.IsValid)
            {
                return View(productCategories);
            }
            else
            {
                if(!string.IsNullOrWhiteSpace(productCategories.ToString()))
                {
                    ProductCategory productCategory = new ProductCategory();
                    productCategory.Category = productCategories.Category;

                    context.Insert(productCategory);
                    context.Commit();
                    return RedirectToAction("Index");
                }
                else
                {
                    throw new ArgumentNullException();
                }
            }
        }

        public ActionResult Edit(string Id)
        {
            ProductCategory productCategories = context.Find(Id);
            if (productCategories == null)
            {
                return HttpNotFound();
            }
            else
            {
                ProductCategoryViewModel productCategory = new ProductCategoryViewModel();
                productCategory.Id = productCategories.Id;
                productCategory.Category = productCategories.Category;
                return View(productCategory);
            }
        }

        [HttpPost]
        public ActionResult Edit(ProductCategoryViewModel productCategories, string Id)
        {
            ProductCategory productCategoryToEdit = context.Find(Id);

            if (productCategoryToEdit == null)
            {
                return HttpNotFound();
            }
            else
            {
                if (!ModelState.IsValid)
                {
                    return View(productCategories);
                }
 
                    productCategoryToEdit.Category = productCategories.Category;

                    context.Commit();

                    return RedirectToAction("Index");

            }
        }

        public ActionResult Delete(string Id)
        {
            ProductCategory productCategoryToDelete = context.Find(Id);

            if (productCategoryToDelete == null)
            {
                return HttpNotFound();
            }
            else
            {
                return View(productCategoryToDelete);
            }
        }

        [HttpPost]
        [ActionName("Delete")]
        public ActionResult ConfirmDelete(string Id)
        {
            ProductCategory productCategoryToDelete = context.Find(Id);

            if (productCategoryToDelete == null)
            {
                return HttpNotFound();
            }
            else
            {
                context.Delete(Id);
                context.Commit();
                return RedirectToAction("Index");
            }
        }
    }
}