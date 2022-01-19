using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EcommerceLiteUI.Controllers
{
    public class BaseController : Controller
    {
        [NonAction]
        public string CreateRandomNewPassword()
        {
            //abcd1234
            Random rnd = new Random();
            int theNumber = rnd.Next(1000, 5000);
            char[] theString = Guid.NewGuid().ToString().Replace("-", "").ToArray();
            string thePassword = string.Empty;
            for (int i = 0; i < theString.Length; i++)
            {
                if (thePassword.Length == 4) break;
                if (char.IsLetter(theString[i]))
                    thePassword += theString[i].ToString();
            }
            thePassword += theNumber;
            return thePassword;
        }
    }
}