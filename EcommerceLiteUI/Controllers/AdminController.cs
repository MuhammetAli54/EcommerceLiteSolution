using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using EcommerceLiteBLL.Repository;

namespace EcommerceLiteUI.Controllers
{

    [Authorize(Roles ="Admin")]
    public class AdminController : BaseController
    {
        //Global alan
        OrderRepo myOrderRepo = new OrderRepo();
        CategoryRepo myCategoryRepo = new CategoryRepo();
        // GET: Admin
        public ActionResult DashBoard()
        {
            var orderList = myOrderRepo.GetAll();
            //1 aylık sipariş sayısı
            var newOrderCount = orderList.Where(x => x.RegisterDate >= DateTime.Now.AddMonths(-1)).ToList().Count();
            ViewBag.NewOrderCount = newOrderCount;

            return View();
        }

        public ActionResult DashBoard2()
        {
            var orderList = myOrderRepo.GetAll();
            //1 aylık sipariş sayısı
            var newOrderCount = orderList.Where(x => x.RegisterDate >= DateTime.Now.AddMonths(-1)).ToList().Count();
            ViewBag.NewOrderCount = newOrderCount;

            var model = myCategoryRepo.GetBaseCategoriesProductCount();
            return View(model);
        }
    }
}