using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Net.Mime;
using System.Text;
using System.Threading.Tasks;
using EcommerceLiteEntity.ViewModels;

namespace EcommerceLiteBLL.Settings
{
    public static class SiteSettings
    {
        public static string SiteMail { get; set; } = "yazilim103@gmail.com";
        public static string SiteMailPassword { get; set; } = "betul103103";

        public static string SiteMailSmtpHost = "smtp.gmail.com";

        public static int SiteMailSmtpPort = 587;

        public static bool SiteMailEnableSsl = true;

        public async static Task SendMail(MailModel model)
        {
            try
            {
                using (var smtp = new SmtpClient())
                {
                    var message = new MailMessage();
                    message.To.Add(new MailAddress(model.To));
                    message.From = new MailAddress(SiteMail);
                    message.Subject = model.Subject;
                    message.IsBodyHtml = true;
                    message.Body = model.Message;
                    message.BodyEncoding = Encoding.UTF8;

                    if (!string.IsNullOrEmpty(model.Cc))
                    {
                        message.CC.Add(new MailAddress(model.Bcc));
                    }
                    if (!string.IsNullOrEmpty(model.Bcc))
                    {
                        message.Bcc.Add(new MailAddress(model.Bcc));
                    }
                    var theCredential = new NetworkCredential()
                    {
                        UserName=SiteMail,
                        Password=SiteMailPassword
                    };
                    smtp.Credentials = theCredential;
                    smtp.Host = SiteMailSmtpHost;
                    smtp.Port = SiteMailSmtpPort;
                    smtp.EnableSsl = SiteMailEnableSsl;
                    await smtp.SendMailAsync(message);
                }
            }
            catch (Exception ex)
            {
                //TODO: ex Loglanacak
            }
        }

        public static void SendMail(byte[] iconBytes,MailModel model)
        {
            try
            {
                System.IO.MemoryStream iconBitmap = new System.IO.MemoryStream(iconBytes);
                LinkedResource iconResource = new LinkedResource(iconBitmap, MediaTypeNames.Image.Jpeg);
                iconResource.ContentId = "Icon";

                string htmlBody = @"<html><head>";
                htmlBody += @"<style>";
                htmlBody += @"body{ font-family:'Calibri',sans-serif; }";
                htmlBody += @"</style>";
                htmlBody += @"</head><body>";
                htmlBody += model.Message;
                htmlBody += @"<img style='float:left' width='250px' height='250px' src='cid:" + iconResource.ContentId + @"'/>";
                htmlBody += @"</body></html>";

                var message = new MailMessage();
                message.To.Add(new MailAddress(model.To));
                message.From = new MailAddress(SiteMail);
                message.Subject = model.Subject;
                message.IsBodyHtml = true;
                message.Body = htmlBody;
                message.BodyEncoding = Encoding.UTF8;
                if (!string.IsNullOrEmpty(model.Cc))
                {
                    message.CC.Add(new MailAddress(model.Cc));
                }
                if (!string.IsNullOrEmpty(model.Bcc))
                {
                    message.Bcc.Add(new MailAddress(model.Bcc));
                }
                var theCredential = new NetworkCredential()
                {
                    UserName = SiteMail,
                    Password = SiteMailPassword
                };

                AlternateView alternativeView = AlternateView.CreateAlternateViewFromString(htmlBody, null, MediaTypeNames.Text.Html);
                alternativeView.LinkedResources.Add(iconResource);
                message.AlternateViews.Add(alternativeView);

                SmtpClient client = new SmtpClient();
                client.Credentials = theCredential;
                client.Port = SiteMailSmtpPort; // You can use Port 25 if 587 is blocked
                client.Host = SiteMailSmtpHost;
                client.EnableSsl = SiteMailEnableSsl;
                client.Send(message);
            }
            catch (Exception ex)
            {
                LogManager.LogMessage(ex.ToString(), pageInfo: "void SendMail", userInfo: model.To);
            }
        }

        public static string UrlFormatConverter(string name)
        {
            string theResult = name.ToLower()
            .Replace("'", "")
            .Replace(" ", "-")
            .Replace("<", "")
            .Replace(">", "")
            .Replace("&", "")
            .Replace("[", "")
            .Replace("!", "")
            .Replace("]", "")
            .Replace("ı", "i")
            .Replace("ö", "o")
            .Replace("ü", "u")
            .Replace("ş", "s")
            .Replace("ç", "c")
            .Replace("ğ", "g")
            .Replace("İ", "I")
            .Replace("Ö", "O")
            .Replace("Ü", "U")
            .Replace("Ş", "S")
            .Replace("Ç", "C")
            .Replace("Ğ", "G")
            .Replace("|", "")
            .Replace(".", "-")
            .Replace("?", "-")
            .Replace(";", "-");

            return theResult;
        }

    }
}
