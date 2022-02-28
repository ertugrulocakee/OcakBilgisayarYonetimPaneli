using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MVCTeknolojikMarketYonetimPaneli.Models.Model;
using PagedList;
using PagedList.Mvc;

namespace MVCTeknolojikMarketYonetimPaneli.Controllers
{
    public class AdminController : Controller
    {
        // GET: Admin

        TeknolojikMarketEntities db = new TeknolojikMarketEntities();

        [Authorize(Roles="Admin")]
        public ActionResult Index(string kullanici, int sayfa = 1)
        {
            var admin = db.TBL_ADMIN.ToList();

            if (!String.IsNullOrEmpty(kullanici))
            {

                admin = admin.Where(m => m.KULLANICIADI.Contains(kullanici)).ToList();

            }

            return View(admin.ToPagedList(sayfa, 10));
        }

        [Authorize(Roles="Admin")]
        [HttpGet]
        public ActionResult AdminEkle()
        {


            return View();

        }

        [HttpPost]
        public ActionResult AdminEkle(TBL_ADMIN admin)
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
                var admn = db.TBL_ADMIN.Where(m => m.KULLANICIADI == admin.KULLANICIADI || m.SIFRE == admin.SIFRE);

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


                admin.KULLANICITIPI = "Admin";

                db.TBL_ADMIN.Add(admin);

                db.SaveChanges();

                ViewBag.Message = "Admin başarıyla eklendi!";
            

                return View();


            }
            else
            {

                return View();

            }



        }


    }
}