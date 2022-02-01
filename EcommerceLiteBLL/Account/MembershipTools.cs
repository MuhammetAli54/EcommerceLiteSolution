using EcommerceLiteDAL;
using EcommerceLiteEntity.IdentityModels;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace EcommerceLiteBLL.Account
{
    public static class MembershipTools
    {
        public static UserStore<ApplicationUser> NewUserStore()
        {
            return new UserStore<ApplicationUser>(new MyContext());
        }
        public static UserManager<ApplicationUser> NewUserManager()
        {
            return new UserManager<ApplicationUser>(NewUserStore());
        }
        public static RoleStore<ApplicationRole> NewRoleStore()
        {
            return new RoleStore<ApplicationRole>(new MyContext());
        }
        public static RoleManager<ApplicationRole> NewRoleManager()
        {
            return new RoleManager<ApplicationRole>(NewRoleStore());
        }
        public static string GetUserName(string id)
        {
            var myUsermanager = NewUserManager();
            var user = myUsermanager.FindById(id);
            if (user!=null)
            {
                return user.UserName;
            }
            return null;
        }
        public static string GetNameSurname()
        {
            var id = HttpContext.Current.User.Identity.GetUserId();
            if (string.IsNullOrEmpty(id))
            {
                return "Ziyaretçi";
            }
            else
            {
                var myUserManager = NewUserManager();
                var user = myUserManager.FindById(id);
                string nameSurname = null;
                nameSurname = user != null ? string.Format("{0} {1}", user.Name, user.Surname) : null;

                return nameSurname;
            }
        }

        public static ApplicationUser GetUser()
        {
            var id = HttpContext.Current.User.Identity.GetUserId();
            if (string.IsNullOrEmpty(id))
            {
                return null;
            }
            else
            {
                var myUserManager = NewUserManager();
                var user = myUserManager.FindById(id);
                return user;
            }
        }
    }
}
