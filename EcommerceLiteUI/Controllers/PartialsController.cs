using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EcommerceLiteUI.Controllers
{
    public class PartialsController : BaseController
    {
        public PartialViewResult AdminSideBarResult()
        {
            //TODO: name surname alınacak
            @TempData["NameSurname"] = "";
            return PartialView("_PartialAdminSideBar");
        }
       public PartialViewResult AdminSideBarMenuResult()
        {
            return PartialView("_PartialAdminSideBarMenu");
        }
    }
}