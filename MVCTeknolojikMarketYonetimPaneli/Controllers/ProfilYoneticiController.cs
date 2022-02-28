using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MVCTeknolojikMarketYonetimPaneli.Models.Model;

namespace MVCTeknolojikMarketYonetimPaneli.Controllers
{
    public class ProfilYoneticiController : Controller
    {
        // GET: ProfilYonetici

        TeknolojikMarketEntities db = new TeknolojikMarketEntities();


        [Authorize(Roles = "Yönetici")]
        public ActionResult Index()
        {

            string kullaniciAdi = Session["KullaniciAdi"].ToString();
            var yonetici = db.TBL_YONETICI.Where(m => m.KULLANICIADI.Equals(kullaniciAdi)).FirstOrDefault();
            return View(yonetici);

           
        }



        [Authorize(Roles = "Yönetici")]
        [HttpGet]
        public ActionResult SifreGuncelle(int id)
        {


            var yonetici = db.TBL_YONETICI.Find(id);


            return View(yonetici);

        }

        [HttpPost]
        public ActionResult SifreGuncelle(TBL_YONETICI yntc)
        {


            if (ModelState.IsValid)
            {

                char[] kullaniciAdi = yntc.KULLANICIADI.ToCharArray();
                char[] kullaniciSifre = yntc.SIFRE.ToCharArray();

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


                var prsl = db.TBL_PERSONEL.Where(m => m.KULLANICIADI == yntc.KULLANICIADI || m.SIFRE == yntc.SIFRE);
                var yonetici = db.TBL_YONETICI.Where(m => m.YONETICIID != yntc.YONETICIID).Where(m => m.KULLANICIADI == yntc.KULLANICIADI || m.SIFRE == yntc.SIFRE);
                var admin = db.TBL_ADMIN.Where(m => m.KULLANICIADI == yntc.KULLANICIADI || m.SIFRE == yntc.SIFRE);

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

                if (admin.Any())
                {

                    ViewBag.Message = "Boyle bir kullanıcı adı veya şifreye sahip admin var!";

                    return View();


                }


                var yontci = db.TBL_YONETICI.Find(yntc.YONETICIID);

                yontci.KULLANICIADI = yntc.KULLANICIADI;

                yontci.SIFRE = yntc.SIFRE;

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