using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EcommerceLiteUI.Controllers
{
    public class AdminController : BaseController
    {
        // GET: Admin
        public ActionResult DashBoard()
        {
            return View();
        }
    }
}