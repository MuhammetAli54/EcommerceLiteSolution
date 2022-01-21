using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using EcommerceLiteBLL.Repository;
using EcommerceLiteEntity.Models;

namespace EcommerceLiteUI.Controllers
{
    public class CategoryController : Controller
    {
        //Global alan
        CategoryRepo myCategoryRepo = new CategoryRepo();

        public ActionResult CategoryList()
        {
            var allCategories = myCategoryRepo.Queryable().Where(x => x.BaseCategory == null).ToList();
            ViewBag.CategoryCount = allCategories.Count;
            return View(allCategories);
        }

        public ActionResult Create(int? id, bool IsSendFromSubCategory = false)
        {
            ViewBag.CategoryName = string.Empty;
            if (id != null)
            {
                Category model = new Category()
                {
                    Id = id.Value
                };
                ViewBag.CategoryName = myCategoryRepo.GetById(id.Value).CategoryName;
                return View(model);
            }
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Category model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    ModelState.AddModelError("", "Geçersiz veri girişi! Lütfen verileri düzgün giriniz!");
                    return View(model);
                }
                Category newCategory = new Category()
                {
                    CategoryName = model.CategoryName,
                    CategoryDescription = model.CategoryDescription,
                    RegisterDate = DateTime.Now,
                    BaseCategoryId = null
                };
                if (model.Id > 0)
                {
                    newCategory.BaseCategoryId = model.Id;
                }
                int insertResult = myCategoryRepo.Insert(newCategory);
                if (insertResult > 0 && model.Id == 0)
                {
                    return RedirectToAction("CategoryList", "Category");
                }
                else if (insertResult > 0 && model.Id > 0)
                {
                    return RedirectToAction("SubCategoryList", "Category", new { id = model.Id });
                }
                else
                {
                    throw new Exception("Kategori eklenemedi!");
                }
            }
            catch (Exception ex)
            {
                ///TODO: ex loglanacak
                ModelState.AddModelError("", "Beklenmedik bir hata oluştu! Tekrar deneyiniz.HATA: " + ex.Message);
                return View(model);
            }
        }

        public ActionResult SubCategoryList(int id)

        {
            var subCategories = myCategoryRepo.Queryable().Where(x => x.BaseCategory != null && x.BaseCategoryId == id).ToList();
            ViewBag.CategoryId = id;
            ViewBag.CategoryName = myCategoryRepo.GetById(id).CategoryName;
            ViewBag.SubCategoryCount = subCategories.Count;
            return View(subCategories);
        }

    }
}