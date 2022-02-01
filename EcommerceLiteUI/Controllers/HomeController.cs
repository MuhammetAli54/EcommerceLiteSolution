using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using EcommerceLiteBLL.Account;
using EcommerceLiteBLL.Repository;
using EcommerceLiteBLL.Settings;
using EcommerceLiteEntity.Models;
using EcommerceLiteEntity.ViewModels;
using EcommerceLiteUI.Models;
using Mapster;
using QRCoder;

namespace EcommerceLiteUI.Controllers
{
    public class HomeController : BaseController
    {
        //Global alan
        CategoryRepo myCategoryRepo = new CategoryRepo();
        ProductRepo myProductRepo = new ProductRepo();
        OrderRepo myOrderRepo = new OrderRepo();
        OrderDetailRepo myOrderDetailRepo = new OrderDetailRepo();
        CustomerRepo myCustomerRepo = new CustomerRepo();

        public ActionResult Index()
        {
            var categoryList = myCategoryRepo.Queryable().Where(x => x.BaseCategoryId == null).ToList();
            ViewBag.CategoryList = categoryList;
            var productList = myProductRepo.Queryable().Where(x => x.Quantity >= 1).ToList();
            List<ProductViewModel> model = new List<ProductViewModel>();
            foreach (var item in productList)
            {
                model.Add(item.Adapt<ProductViewModel>());
            }
            foreach (var item in model)
            {
                item.SetCategory();
                item.SetProductPictures();
            }
            return View(model);
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public ActionResult AddToCart(int id)
        {
            try
            {
                var shoppingCart = Session["ShoppingCart"] as List<CartViewModel>;
                if (shoppingCart == null)
                {
                    shoppingCart = new List<CartViewModel>();
                }
                if (id > 0)
                {
                    var product = myProductRepo.GetById(id);
                    if (product == null)
                    {
                        TempData["AddToCart"] = "Ürün eklemesi başarısız.Lütfen Tekrar Deneyiniz!";
                        return RedirectToAction("Index", "Home");
                    }
                    var productAddtoCart = product.Adapt<CartViewModel>();
                    if (shoppingCart.Count(x => x.Id == productAddtoCart.Id) > 0)
                    {
                        shoppingCart.FirstOrDefault(x => x.Id == productAddtoCart.Id).Quantity++;
                    }
                    else
                    {
                        productAddtoCart.Quantity = 1;
                        shoppingCart.Add(productAddtoCart);
                    }
                    Session["ShoppingCart"] = shoppingCart;
                    TempData["AddToCart"] = "Ürün Eklendi";
                }

                return RedirectToAction("Index", "Home");
            }
            catch (Exception ex)
            {
                TempData["AddToCart"] = "Ürün eklemesi başarısız.Lütfen Tekrar Deneyiniz!";
                return RedirectToAction("Index", "Home");
            }
        }

        [Authorize]
        public async  Task<ActionResult> Buy()
        {
            try
            {
                var shoppingCart = Session["ShoppingCart"] as List<CartViewModel>;
                if (shoppingCart != null)
                {
                    if (shoppingCart.Count > 0)
                    {
                        var user = MembershipTools.GetUser();
                        var customer = myCustomerRepo.Queryable().FirstOrDefault(x => x.UserId == user.Id);
                        Order newOrder = new Order()
                        {
                            CustomerTcNumber = customer.TcNumber,
                            RegisterDate = DateTime.Now,
                            OrderNumber = "1234567"
                        };
                        int orderInsertResult = myOrderRepo.Insert(newOrder);
                        if (orderInsertResult > 0)
                        {
                            bool isSuccess = false;
                            foreach (var item in shoppingCart)
                            {
                                OrderDetail newOrderDetail = new OrderDetail()
                                {
                                    OrderId = newOrder.Id,
                                    ProductId = item.Id,
                                    Discount = 0,
                                    ProductPrice = item.Price,
                                    Quantity = item.Quantity,
                                    RegisterDate = DateTime.Now
                                };
                                if (newOrderDetail.Discount > 0)
                                {
                                    newOrderDetail.TotalPrice = newOrderDetail.Quantity * (newOrderDetail.ProductPrice - (newOrderDetail.ProductPrice * Convert.ToDecimal(newOrderDetail.Discount / 100)));
                                }
                                else
                                {
                                    newOrderDetail.TotalPrice = newOrderDetail.Quantity * newOrderDetail.ProductPrice;
                                }
                                int detailInsertResult = myOrderDetailRepo.Insert(newOrderDetail);
                                if (detailInsertResult > 0)
                                {
                                    isSuccess = true;
                                }
                            }
                            if (isSuccess)
                            {
                                //Email ile QR gönderilecek
                                #region SendEmail

                                QRCodeGenerator qrGenerator = new QRCodeGenerator();
                                QRCodeData qrData = qrGenerator.CreateQrCode(newOrder.OrderNumber, QRCodeGenerator.ECCLevel.Q);
                                QRCode qrCode = new QRCode(qrData);
                                Bitmap qrBitmap = qrCode.GetGraphic(64);
                                byte[] bitmapArray = BitmapToByteArray(qrBitmap);
                                string qrUri = string.Format("data:image/png;base64,{0}", Convert.ToBase64String(bitmapArray));

                                List<OrderDetail> orderDetailList =
                   new List<OrderDetail>();
                                orderDetailList = myOrderDetailRepo.Queryable()
                                    .Where(x => x.OrderId == newOrder.Id).ToList();

                                string message = $"Merhaba {user.Name} {user.Surname} <br/><br/>" +
                                                   $"{orderDetailList.Count} adet ürünlerinizin siparişini aldık.<br/><br/>" +
                                                   $"Toplam Tutar:{orderDetailList.Sum(x => x.TotalPrice).ToString()} ₺ <br/> <br/>" +
                                                   $"<table><tr><th>Ürün Adı</th><th>Adet</th><th>Birim Fiyat</th><th>Toplam</th></tr>";
                                foreach (var item in orderDetailList)
                                {
                                    message += $"<tr><td>{myProductRepo.GetById(item.ProductId).ProductName}</td><td>{item.Quantity}</td><td>{item.TotalPrice}</td></tr>";
                                }
                                message += "</table><br/>Siparişinize ait QR kodunuz: <br/><br/><br/>";
                                message += $"<a href='/Home/Order/{newOrder.Id}'><img src=\"{qrUri}\" height=250px;  width=250px; class='img-thumbnail' /></a>";
                                await SiteSettings.SendMail(new MailModel()
                                {
                                    To = user.Email,
                                    Subject = "ECommerceLite - Siparişiniz alındı",
                                    Message = message

                                });

                                #endregion

                                return RedirectToAction("Order", "Home", new { id = newOrder.Id });
                            }
                            else
                            {
                                //sonra bakılacak
                            }
                        }
                    }
                }
                return RedirectToAction("Index", "Home");
            }
            catch (Exception ex)
            {
                //ex loglanacak
                //TempData ile sonuç anasayfaya gönderilebilir
                return RedirectToAction("Index", "Home");
            }
        }

      //private void SendOrderMailWithQRCode(int id)
      //  {
      //      try
      //      {
      //          Order customerOrder = myOrderRepo.GetById(id);
      //          List<OrderDetail> orderDetailList = new List<OrderDetail>();
      //          orderDetailList = myOrderDetailRepo.Queryable().Where(x => x.OrderId == customerOrder.Id).ToList();
      //          QRCodeGenerator qrGenerator = new QRCodeGenerator();
      //          QRCodeData qrData = qrGenerator.CreateQrCode(customerOrder.OrderNumber, QRCodeGenerator.ECCLevel.Q);
      //          QRCode qrCode = new QRCode(qrData);
      //          Bitmap qrBitmap = qrCode.GetGraphic(60);
      //          byte[] bitmapArray = BitmapToByteArray(qrBitmap);
      //          string qrUri = string.Format("data:image/png;base64,{0}", Convert.ToBase64String(bitmapArray));

      //          var user = MembershipTools.GetUser();
      //          string message = $"Merhaba {user.Name} {user.Surname} <br/>" +
      //                             $"{orderDetailList.Count} adet ürünlerinizin siparişini aldık.<br/>" +
      //                             $"Toplam Tutar:{orderDetailList.Sum(x => x.TotalPrice).ToString()} ₺ <br/> <br/>" +
      //                             $"<table><tr><th>Ürün Adı</th><th>Adet</th><th>Birim Fiyat</th><th>Toplam</th></tr>";
      //          foreach (var item in orderDetailList)
      //          {
      //              message += $"<tr><td>{myProductRepo.GetById(item.ProductId).ProductName}</td><td>{item.Quantity}</td><td>{item.TotalPrice}</td></tr>";
      //          }
      //          message += "</table><br/>Siparişinize ait QR kodunuz: <br/>";
      //          message += $"<a href='/Home/Order/{customerOrder.Id}'><img src='{qrUri}' height=250px;  width=250px; class='img-thumbnail' /></a>";
      //          await SiteSettings.SendMail(new MailModel()
      //          {
      //              To = user.Email,
      //              Subject = "ECommerceLite - Siparişiniz alındı",
      //              Message = message

      //          });
      //      }
      //      catch (Exception ex)
      //      {
      //          // ex loglanacak
      //      }
      //  }

        [Authorize]
        public ActionResult Order(int? id)
        {
            try
            {
                if (id > 0)
                {
                    Order customerOrder = myOrderRepo.GetById(id.Value);
                    List<OrderDetail> orderDetails = new List<OrderDetail>();
                    if (customerOrder != null)
                    {
                        orderDetails = myOrderDetailRepo.Queryable().Where(x => x.OrderId == customerOrder.Id).ToList();
                        foreach (var item in orderDetails)
                        {
                            item.Product = myProductRepo.GetById(item.ProductId);
                        }
                        ViewBag.OrderSuccess = "Siparişiniz başarıyla alınmıştır.";
                        Session["ShoppingCart"] = null;
                        return View(orderDetails);
                    }
                    else
                    {
                        ModelState.AddModelError("", "Ürün bulunamadı! Tekrar deneyiniz");
                        return View(orderDetails);
                    }
                }
                else
                {
                    ModelState.AddModelError("", "Ürün bulunamadı! Tekrar deneyiniz");
                    return View(new List<OrderDetail>());
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Beklenmedik bir hata oluştu!");
                //ex loglanacak
                return View(new List<OrderDetail>());
            }
        }
    }
}