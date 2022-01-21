using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using EcommerceLiteEntity.Models;
using EcommerceLiteBLL.Repository;

namespace EcommerceLiteUI.Controllers
{
    public class ProductController : Controller
    {
        // Global alan
        ProductRepo myProductRepo = new ProductRepo();
        CategoryRepo myCategoryRepo = new CategoryRepo();
        public ActionResult ProductList()
        {
            var allProductList = myProductRepo.GetAll();
            return View(allProductList);
        }

        [HttpGet]
        public ActionResult Create()
        {
            List<SelectListItem> allCategories = new List<SelectListItem>();
            myCategoryRepo.GetAll().ForEach(x=> allCategories.Add(new SelectListItem() { 
            Text=x.CategoryName,
            Value=x.Id.ToString()
            }));
            ViewBag.CategoryList = allCategories;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Product model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    ModelState.AddModelError("", "Veri girişleri düzgün olmalıdır!");
                    return View(model);
                }
               int insertResult = myProductRepo.Insert(model);
                if (insertResult > 0)
                {
                    return RedirectToAction("ProductList", "Product");
                }
                else
                {
                    ModelState.AddModelError("", "Ürün ekleme işleminde bir hata oluştu! Tekrar deneyiniz!");
                    return View(model);
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Beklenmedik bir hata oluştu!");
                //TODO: ex loglanacak
                return View(model);
            }
        }
    }
}