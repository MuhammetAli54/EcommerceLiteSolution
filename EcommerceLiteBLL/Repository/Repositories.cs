using EcommerceLiteEntity.Models;
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

    public class CategoryRepo:RepositoryBase<Category,int> { }
    public class ProductRepo:RepositoryBase<Product,int> { }
    public class OrderRepo:RepositoryBase<Order,int> { }
    public class OrderDetailRepo:RepositoryBase<OrderDetail,int> { }
    public class CustomerRepo:RepositoryBase<Customer,string> { }
    public class AdminRepo:RepositoryBase<Admin,string> { }
    public class PassiveUserRepo:RepositoryBase<PassiveUser,string> { }
    public class ProductPictureRepo:RepositoryBase<ProductPicture,int> { }
}
