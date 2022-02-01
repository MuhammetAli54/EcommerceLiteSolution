using EcommerceLiteBLL.Repository;
using EcommerceLiteEntity.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace EcommerceLiteUI.Models
{
    public class ProductViewModel
    {
        CategoryRepo myCategoryRepo = new CategoryRepo();
        ProductPictureRepo myProductPictureRepo = new ProductPictureRepo();


        public int Id { get; set; }
        public DateTime RegisterDate { get; set; } = DateTime.Now;
        [Required]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "Ürün adının uzunluğu 2 ile 50 karakter aralığında olmalıdır!")]
        public string ProductName { get; set; }
        [StringLength(500, ErrorMessage = "Ürün açlıklamasının uzunluğu en fazla 500 karakter olmalıdır!")]
        public string Description { get; set; }

        [StringLength(8, ErrorMessage = "Ürün kodunun uzunluğu en fazla 8 karakter olmalıdır!")]
        public string ProductCode { get; set; }

        [Required]
        [DataType(DataType.Currency)]
        public decimal Price { get; set; }
        [Required]
        public int Quantity { get; set; }
        [Required]
        public int CategoryId { get; set; }

        private Category _category = new Category();
        public Category CategoryOfProduct { get; set; }

        public List<ProductPicture> PicturesOfProduct { get; set; }

        public List<string> ProductPicturesList { get; set; } = new List<string>();

        public List<HttpPostedFileBase> Files { get; set; } = new List<HttpPostedFileBase>();

        public void SetCategory()
        {
            if (CategoryId > 0)
            {
                CategoryOfProduct = myCategoryRepo.GetById(CategoryId);
                if (CategoryOfProduct.BaseCategoryId > 0)
                {
                    CategoryOfProduct.CategoryList = new List<Category>();

                    CategoryOfProduct.BaseCategory = myCategoryRepo.GetById(CategoryOfProduct.BaseCategoryId.Value);
                    CategoryOfProduct.CategoryList.Add(CategoryOfProduct.BaseCategory);
                    bool isOver = false;
                    Category theBaseCategory = CategoryOfProduct.BaseCategory;
                    while (!isOver)
                    {

                        if (theBaseCategory.BaseCategoryId > 0)
                        {
                            CategoryOfProduct.CategoryList.Add(myCategoryRepo.GetById(theBaseCategory.BaseCategoryId.Value));
                            theBaseCategory = myCategoryRepo.GetById(theBaseCategory.BaseCategoryId.Value);
                        }
                        else
                        {
                            isOver = true;
                        }
                    }
                    CategoryOfProduct.CategoryList = CategoryOfProduct.CategoryList.OrderBy(x => x.Id).ToList();
                }
            }
        }

        public void SetProductPictures()
        {
            if (Id > 0)
            {
                PicturesOfProduct = myProductPictureRepo.Queryable().Where(x => x.ProductId == Id).ToList();
            }
        }

    }

}