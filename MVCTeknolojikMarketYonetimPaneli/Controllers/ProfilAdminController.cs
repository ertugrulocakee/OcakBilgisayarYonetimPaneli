using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MVCTeknolojikMarketYonetimPaneli.Models.Model;

namespace MVCTeknolojikMarketYonetimPaneli.Controllers
{
    public class ProfilAdminController : Controller
    {
        // GET: ProfilAdmin

        TeknolojikMarketEntities db = new TeknolojikMarketEntities();

        [Authorize(Roles="Admin")]
        public ActionResult Index()
        {

            string kullaniciAdi = Session["KullaniciAdi"].ToString();
            var admin = db.TBL_ADMIN.Where(m => m.KULLANICIADI.Equals(kullaniciAdi)).FirstOrDefault();

            return View(admin);

        }


        [Authorize(Roles="Admin")]
        [HttpGet]
        public ActionResult SifreGuncelle(int id)
        {

            var admin = db.TBL_ADMIN.Find(id);

            return View(admin);

        }

        [HttpPost]
        public ActionResult SifreGuncelle(TBL_ADMIN admin)
        {


            if (ModelState.IsValid)
            {
                char[] kullaniciAdi = admin.KULLANICIADI.ToCharArray();
                char[] kullaniciSifre = admin.SIFRE.ToCharArray();

                foreach (char x in kullaniciAdi)
                {

                    if (!Char.IsLetterOrDigit(x))
                    {

                        ViewBag.Message = "Kullanıcı adı sadece harflerden ve rakamlardan oluşmalıdır!";

                        return View();
                    }

                }

                foreach (char x in kullaniciSifre)
                {

                    if (!Char.IsDigit(x))
                    {

                        ViewBag.Message = "Şifre sadece rakamlardan oluşmalıdır!";

                        return View();


                    }


                }


                var prsl = db.TBL_PERSONEL.Where(m => m.KULLANICIADI == admin.KULLANICIADI || m.SIFRE == admin.SIFRE);
                var yonetici = db.TBL_YONETICI.Where(m => m.KULLANICIADI == admin.KULLANICIADI || m.SIFRE == admin.SIFRE);
                var admn = db.TBL_ADMIN.Where(m => m.ADMINID != admin.ADMINID).Where(m => m.KULLANICIADI == admin.KULLANICIADI || m.SIFRE == admin.SIFRE);

                if (prsl.Any())
                {

                    ViewBag.Message = "Boyle bir kullanıcı adı veya şifreye sahip personel var!";

                    return View();

                }

                if (yonetici.Any())
                {

                    ViewBag.Message = "Boyle bir kullanıcı adı veya şifreye sahip yonetici var!";

                    return View();


                }

                if (admn.Any())
                {

                    ViewBag.Message = "Boyle bir kullanıcı adı veya şifreye sahip admin var!";

                    return View();


                }


                var Admin = db.TBL_ADMIN.Find(admin.ADMINID);

                Admin.KULLANICIADI = admin.KULLANICIADI;

                Admin.SIFRE = admin.SIFRE;

                db.SaveChanges();

                Session.RemoveAll();

                return RedirectToAction("Giris", "Giris");


            }
            else
            {

                return View();

            }


        }



    }
}