using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MVCTeknolojikMarketYonetimPaneli.Models.Model;
using System.Web.Security;
using MVCTeknolojikMarketYonetimPaneli.Models.EkModel;
using System.Net.Mail;
using System.Text;

namespace MVCTeknolojikMarketYonetimPaneli.Controllers
{
    public class GirisController : Controller
    {
        // GET: Giris

        TeknolojikMarketEntities db = new TeknolojikMarketEntities();

       
        [HttpGet]
        public ActionResult Giris()
        {

            if (TempData["ileti"] != null)
            {

                ViewBag.Message = TempData["ileti"].ToString();

            }

            return View();
        }

   
        [HttpPost]
        public ActionResult Giris(TBL_PERSONEL personel,TBL_YONETICI yonetici, TBL_ADMIN admin)
        {

            string str = Request.Params["btn1"];

            if (str == "Personel")
            {
                var bilgiler = db.TBL_PERSONEL.FirstOrDefault(m => m.KULLANICIADI == personel.KULLANICIADI && m.SIFRE == personel.SIFRE);
                if (bilgiler != null)
                {

                    FormsAuthentication.SetAuthCookie(bilgiler.KULLANICIADI, false);
                    Session["KullaniciAdi"] = bilgiler.KULLANICIADI;
                    Session["PersonelSube"] = bilgiler.TBL_SUBE.SUBEID;
                    Session["Sube"] = bilgiler.TBL_SUBE.SUBEID;
                    return RedirectToAction("Index", "AnaSayfa");

                }
                else
                {

                    TempData["ileti"] = "Boyle bir personel yok!";

                    return RedirectToAction("Giris","Giris");
                }


            }
            else if (str == "Yönetici")
            {

                var bilgiler = db.TBL_YONETICI.FirstOrDefault(m => m.KULLANICIADI == yonetici.KULLANICIADI && m.SIFRE == yonetici.SIFRE);
                if (bilgiler != null)
                {

                    FormsAuthentication.SetAuthCookie(bilgiler.KULLANICIADI, false);
                    Session["KullaniciAdi"] = bilgiler.KULLANICIADI;
                    Session["YoneticiSube"] = bilgiler.TBL_SUBE.SUBEID;
                    Session["Sube"] = bilgiler.TBL_SUBE.SUBEID;
                    return RedirectToAction("Index", "Anasayfa");

                }
                else
                {

                    TempData["ileti"] = "Boyle bir yonetici yok!";
                    return RedirectToAction("Giris", "Giris");
                }

            }
            else if (str == "Admin")
            {

                var bilgiler = db.TBL_ADMIN.FirstOrDefault(m => m.KULLANICIADI == admin.KULLANICIADI && m.SIFRE == admin.SIFRE);
                if (bilgiler != null)
                {

                    FormsAuthentication.SetAuthCookie(bilgiler.KULLANICIADI, false);
                    Session["KullaniciAdi"] = bilgiler.KULLANICIADI;
                    return RedirectToAction("Index", "AnaSayfa");

                }
                else
                {

                    TempData["ileti"] = "Boyle bir admin yok!";
                    return RedirectToAction("Giris", "Giris");
                }

            }
            else
            {
                TempData["ileti"] = "Boyle bir kullanıcı yok!";

                return RedirectToAction("Giris", "Giris");
            }

     
        }


        [HttpPost]
        public ActionResult SifreUnutma(Mail model)
        {

            if (ModelState.IsValid)
            {
                
                var kullaniciYonetici = db.TBL_YONETICI.Where(m => m.KULLANICIADI == model.KullaniciAdi).ToList();

                var kullaniciPersonel = db.TBL_PERSONEL.Where(m => m.KULLANICIADI == model.KullaniciAdi).ToList();

                if (kullaniciYonetici.Any())
                {

                    var yonetici = kullaniciYonetici.FirstOrDefault();

                    var body = new StringBuilder();
                    body.AppendLine("Merhaba " + yonetici.YONETICIAD+" "+yonetici.YONETICISOYAD+"."+"\n");
                    body.AppendLine("Şifreniz: " + yonetici.SIFRE );

                    try
                    {

                        MailSender(body.ToString(), yonetici.YONETICIMAIL);

                        TempData["ileti"] = "Şifreniz e-posta adresinize gonderildi!";

                    }catch(Exception e){

                        Console.WriteLine(e.Message);

                        TempData["ileti"] = "Şifreniz e-posta adresinize teknik bir aksaklık yuzunden atılamadı!";

                    }
                                    
                }else if (kullaniciPersonel.Any())
                {

                    var personel = kullaniciPersonel.FirstOrDefault();

                    var body = new StringBuilder();
                    body.AppendLine("Merhaba " + personel.PERSONELAD + " " + personel.PERSONELSOYAD + "." + "\n");
                    body.AppendLine("Şifreniz: " + personel.SIFRE);

                    try
                    {

                        MailSender(body.ToString(), personel.PERSONELMAIL);

                        TempData["ileti"] = "Şifreniz e-posta adresinize gonderildi!";

                    }
                    catch (Exception e)
                    {

                        Console.WriteLine(e.Message);

                        TempData["ileti"] = "Şifreniz e-posta adresinize teknik bir aksaklık yuzunden atılamadı!";

                    } 

                }
                else
                {

                    TempData["ileti"] = "Boyle bir kullanici adi yoktur!";
                 

                }

                return RedirectToAction("Giris", "Giris");

            }
            else
            {

                TempData["ileti"] = "Kullanıcı adı boş olamaz ve 20 karakteri gecemez!";

                return RedirectToAction("Giris", "Giris");

            }

         

        }


        protected void MailSender(string body,string mail)
        {

            var fromAddress = new MailAddress("ertoocak@gmail.com");
            var toAddress = new MailAddress(mail);
            const string subject = "Ocak Bilgisayar | Şifre Hatırlatma";
            using (var smtp = new SmtpClient
            {
                Host = "smtp.gmail.com",
                Port = 587,
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new System.Net.NetworkCredential(fromAddress.Address, "123test123")
            })
            {
                using (var message = new MailMessage(fromAddress, toAddress) { Subject = subject, Body = body })
                {
                    smtp.Send(message);
                }
            }
        }



    }
}