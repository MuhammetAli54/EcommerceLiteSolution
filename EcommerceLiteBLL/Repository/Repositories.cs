using EcommerceLiteEntity.Models;
using EcommerceLiteEntity.ViewModels;
using Mapster;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EcommerceLiteBLL.Repository
{
    public class Repositories
    {
    }

    public class CategoryRepo:RepositoryBase<Category,int> 
    {
        public List<ProductCountModel> GetBaseCategoriesProductCount()
        {
            List<ProductCountModel> list = new List<ProductCountModel>();
            dbContext = new EcommerceLiteDAL.MyContext();
            var categoryList = this.Queryable()
                .Where(x => x.BaseCategoryId == null).ToList();
            foreach (var item in categoryList)
            {
                int productCount = 0;
                #region Ana Kategori için
                var baseCategoryProductList = from p in dbContext.Products
                                              where p.CategoryId == item.Id
                                              select p;
                productCount = baseCategoryProductList.ToList().Count;
                #endregion
                //sub categoryleri
                var subCategoryList = this.Queryable()
                .Where(x => x.BaseCategoryId == item.Id).ToList();

                foreach (var subitem in subCategoryList)
                {
                    var productList = from p in dbContext.Products
                                      where p.CategoryId == subitem.Id
                                      select p;
                    productCount += productList.ToList().Count;
                }
                list.Add(new ProductCountModel()
                {
                    BaseCategory = item.Adapt<CategoryViewModel>(),
                    BaseCategoryName=item.CategoryName,
                    ProductCount = productCount
                });
            }
            return list;
        }
    }
    public class ProductRepo:RepositoryBase<Product,int> { }
    public class OrderRepo:RepositoryBase<Order,int> { }
    public class OrderDetailRepo:RepositoryBase<OrderDetail,int> { }
    public class CustomerRepo:RepositoryBase<Customer,string> { }
    public class AdminRepo:RepositoryBase<Admin,string> { }
    public class PassiveUserRepo:RepositoryBase<PassiveUser,string> { }
    public class ProductPictureRepo:RepositoryBase<ProductPicture,int> { }
}
