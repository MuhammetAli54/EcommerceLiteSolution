using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EcommerceLiteBLL.Settings
{
   public static class Constants
    {
        public static string EmailAddress 
        {
            get
            {
                try
                {
                    return ConfigurationManager.AppSettings["MAILADDRESS"].ToString();
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message + "WebConfig dosyasında 'MAILADDRESS' adlı bir anahtar olmalıdır!");
                }
            }
        }

        public static string EmailPassword
        {
            get
            {
                try
                {
                    return ConfigurationManager.AppSettings["MAILPASSWORD"].ToString();
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message + "WebConfig dosyasında 'MAILPASSWORD' adlı bir anahtar olmalıdır!");
                }
            }
        }

    }
}
