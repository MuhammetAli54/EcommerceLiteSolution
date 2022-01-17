using EcommerceLiteEntity.IdentityModels;
using EcommerceLiteEntity.Models;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EcommerceLiteDAL
{
    public class MyContext : IdentityDbContext<ApplicationUser>
    {
        public MyContext() : base("name=MyCon")
        {
        }

        public virtual DbSet<Customer> Customers { get; set; }
        public virtual DbSet<PassiveUser> PassiveUsers { get; set; }
        public virtual DbSet<Admin> Admins { get; set; }
        public virtual DbSet<Category> Categories { get; set; }
        public virtual DbSet<Product> Products { get; set; }
        public virtual DbSet<Order> Orders { get; set; }
        public virtual DbSet<OrderDetail> OrderDetails { get; set; }
    }
}
