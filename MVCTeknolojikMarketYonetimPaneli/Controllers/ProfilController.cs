using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MVCTeknolojikMarketYonetimPaneli.Models.Model;

namespace MVCTeknolojikMarketYonetimPaneli.Controllers
{
    public class ProfilController : Controller
    {
        // GET: Profil

        TeknolojikMarketEntities db = new TeknolojikMarketEntities();


        [Authorize(Roles = "Personel")]
        public ActionResult Index()
        {

            string kullaniciAdi = Session["KullaniciAdi"].ToString();
            var personel = db.TBL_PERSONEL.Where(m => m.KULLANICIADI.Equals(kullaniciAdi)).FirstOrDefault();
            return View(personel);

        }


        [Authorize(Roles = "Personel")]
        [HttpGet]
        public ActionResult SifreGuncelle(int id)
        {

           
            var personel = db.TBL_PERSONEL.Find(id);


            return View(personel);


        }

        [HttpPost]
        public ActionResult SifreGuncelle(TBL_PERSONEL personel)
        {

            if (String.IsNullOrEmpty(personel.SIFRE) || String.IsNullOrEmpty(personel.KULLANICIADI))
            {

                ViewBag.Message = "Kullanıcı Adı ve Şifre boş olamaz!";

                return View();
            }
           


            if (ModelState.IsValid)
            {


                char[] kullaniciAdi = personel.KULLANICIADI.ToCharArray();
                char[] kullaniciSifre = personel.SIFRE.ToCharArray();

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

                var prsl = db.TBL_PERSONEL.Where(m => m.PERSONELID != personel.PERSONELID).Where(m => m.KULLANICIADI == personel.KULLANICIADI || m.SIFRE == personel.SIFRE);
                var yonetici = db.TBL_YONETICI.Where(m => m.KULLANICIADI == personel.KULLANICIADI || m.SIFRE == personel.SIFRE);
                var admin = db.TBL_ADMIN.Where(m => m.KULLANICIADI == personel.KULLANICIADI || m.SIFRE == personel.SIFRE);

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

                var perso = db.TBL_PERSONEL.Find(personel.PERSONELID);

                perso.KULLANICIADI = personel.KULLANICIADI;

                perso.SIFRE = personel.SIFRE;

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