using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using EcommerceLiteBLL.Repository;
using EcommerceLiteEntity.ViewModels;

namespace EcommerceLiteUI.Controllers
{
    public class ChartController : Controller
    {
        //Global alan
        CategoryRepo myCategorRepo = new CategoryRepo();
        public ActionResult VisualizePieChartResult()
        {
            //PieChartta göstermek istediğimiz datayı alacağız
            //Bu dataya dashboard taki ajax işlemine gönderebilmek için return Json olarak işlem yapacağız.
            var data = myCategorRepo.GetBaseCategoriesProductCount();
            return Json(data,JsonRequestBehavior.AllowGet);
        }
    }
}