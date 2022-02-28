using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PagedList;
using PagedList.Mvc;
using MVCTeknolojikMarketYonetimPaneli.Models.Model;

namespace MVCTeknolojikMarketYonetimPaneli.Controllers
{
    public class YoneticiController : Controller
    {
        // GET: Yonetici

        TeknolojikMarketEntities db = new TeknolojikMarketEntities();

        [Authorize(Roles="Admin")]
        public ActionResult Index(string ad, string soyad, int sayfa = 1)
        {
            var yoneticiler = db.TBL_YONETICI.Where(m=>m.YONETICIDURUM == true).ToList();

            if (!String.IsNullOrEmpty(ad) && !String.IsNullOrEmpty(soyad))
            {

                yoneticiler = yoneticiler.Where(m => m.YONETICIAD.Contains(ad) && m.YONETICISOYAD.Contains(soyad)).ToList();

            }


            if (!String.IsNullOrEmpty(ad))
            {

                yoneticiler = yoneticiler.Where(m => m.YONETICIAD.Contains(ad)).ToList();

            }

            if (!String.IsNullOrEmpty(soyad))
            {

                yoneticiler = yoneticiler.Where(m => m.YONETICISOYAD.Contains(soyad)).ToList();

            }

            return View(yoneticiler.ToPagedList(sayfa, 10));

        }


        [Authorize(Roles = "Admin")]
        public ActionResult SilinmisYoneticiler(string ad, string soyad, int sayfa = 1)
        {
            var yoneticiler = db.TBL_YONETICI.Where(m => m.YONETICIDURUM == false).ToList();

            if (!String.IsNullOrEmpty(ad) && !String.IsNullOrEmpty(soyad))
            {

                yoneticiler = yoneticiler.Where(m => m.YONETICIAD.Contains(ad) && m.YONETICISOYAD.Contains(soyad)).ToList();

            }


            if (!String.IsNullOrEmpty(ad))
            {

                yoneticiler = yoneticiler.Where(m => m.YONETICIAD.Contains(ad)).ToList();

            }

            if (!String.IsNullOrEmpty(soyad))
            {

                yoneticiler = yoneticiler.Where(m => m.YONETICISOYAD.Contains(soyad)).ToList();

            }

            return View(yoneticiler.ToPagedList(sayfa, 10));

        }

        public ActionResult YoneticiSil(int id)
        {

            var yonetici = db.TBL_YONETICI.Find(id);

            yonetici.YONETICIDURUM = false;

            yonetici.YONETICIBITISTARIHI = DateTime.Now;
            
            db.SaveChanges();

            return RedirectToAction("Index","Yonetici");

        }


        public ActionResult YoneticiyiGeriGetir(int id)
        {

            var yonetici = db.TBL_YONETICI.Find(id);

            var yontci = db.TBL_YONETICI.Where(m => m.TBL_SUBE.SUBEID == yonetici.TBL_SUBE.SUBEID && m.YONETICIDURUM == true).ToList();

            if(!yontci.Any())
            {


                yonetici.YONETICIDURUM = true;

                yonetici.YONETICIBITISTARIHI = null;

                yonetici.YONETICIBASLANGICTARIHI = DateTime.Now;

                db.SaveChanges();


                return RedirectToAction("Index", "Yonetici");

            }
            else
            {

                return RedirectToAction("SilinmisYoneticiler", "Yonetici");

            }

        }


        protected void SubelerVeCinsiyetler()
        {

            List<SelectListItem> subeler = (from i in db.TBL_SUBE.Where(m => m.SUBEDURUM == true).ToList()
                                            select new SelectListItem
                                            {

                                                Text = i.SUBEAD,
                                                Value = i.SUBEID.ToString()

                                            }).ToList();

            ViewBag.sb = subeler;


            List<SelectListItem> cinsiyetler = new List<SelectListItem>();

            cinsiyetler.Add(new SelectListItem { Text = "Erkek", Value = "Erkek" });

            cinsiyetler.Add(new SelectListItem { Text = "Kadın", Value = "Kadın", Selected = true });



            ViewBag.cns = cinsiyetler;


        }


        [Authorize(Roles="Admin")]
        [HttpGet]
        public ActionResult YoneticiEkle()
        {

            SubelerVeCinsiyetler();

            return View();

        }

        [HttpPost]
        public ActionResult YoneticiEkle(TBL_YONETICI yonetici)
        {

            if (String.IsNullOrEmpty(yonetici.YONETICIAD) || String.IsNullOrEmpty(yonetici.YONETICISOYAD) || String.IsNullOrEmpty(yonetici.YONETICIMAIL) || String.IsNullOrEmpty(yonetici.YONETICITELEFON) || String.IsNullOrEmpty(yonetici.YONETICITC) || yonetici.YONETICIMAAS == null || yonetici.YONETICIYAS == null || String.IsNullOrEmpty(yonetici.KULLANICIADI) || String.IsNullOrEmpty(yonetici.SIFRE))
            {

                ViewBag.Message = "Boş değer girmeyin!";

                SubelerVeCinsiyetler();

                return View();

            }

            try
            {

                var yntc = db.TBL_YONETICI.Where(m => m.TBL_SUBE.SUBEID == yonetici.TBL_SUBE.SUBEID && m.YONETICIDURUM == true).ToList();

                if (!yntc.Any())
                {

                    var sube = db.TBL_SUBE.Where(m => m.SUBEID == yonetici.TBL_SUBE.SUBEID).FirstOrDefault();
                    yonetici.TBL_SUBE = sube;

                }
                else
                {

                    ViewBag.Message = "Sectiginiz subenin bir yoneticisi var!";

                    SubelerVeCinsiyetler();

                    return View();

                }
             
            }
            catch
            {

                ViewBag.Message = "Once sube ekleyiniz!";

                SubelerVeCinsiyetler();

                return View();

            }


            if(ModelState.IsValid){


                char[] TC = yonetici.YONETICITC.ToCharArray();
                char[] Ad = yonetici.YONETICIAD.ToCharArray();
                char[] Soyad = yonetici.YONETICISOYAD.ToCharArray();

                foreach (char x in TC)
                {

                    if (!char.IsDigit(x))
                    {

                        ViewBag.Message = "TC Kimlik Numarası rakamlardan oluşmalıdır!";
                        SubelerVeCinsiyetler();
                        return View();
                    }

                }

                foreach (char x in Ad)
                {

                    if (!char.IsLetter(x))
                    {

                        ViewBag.Message = "Ad harflerden oluşmalıdır!";
                        SubelerVeCinsiyetler();
                        return View();



                    }

                }

                foreach (char x in Soyad)
                {

                    if (!char.IsLetter(x))
                    {

                        ViewBag.Message = "Soyad harflerden oluşmalıdır!";
                        SubelerVeCinsiyetler();
                        return View();

                    }

                }


                char[] kullaniciAdi = yonetici.KULLANICIADI.ToCharArray();
                char[] kullaniciSifre = yonetici.SIFRE.ToCharArray();

                foreach (char x in kullaniciAdi)
                {

                    if (!Char.IsLetterOrDigit(x))
                    {

                        ViewBag.Message = "Kullanıcı adı sadece harflerden ve rakamlardan oluşmalıdır!";

                        SubelerVeCinsiyetler();

                        return View();
                    }

                }

                foreach (char x in kullaniciSifre)
                {

                    if (!Char.IsDigit(x))
                    {

                        ViewBag.Message = "Şifre sadece rakamlardan oluşmalıdır!";

                        SubelerVeCinsiyetler();

                        return View();


                    }


                }

           
                var personelKontrol = db.TBL_PERSONEL.Where(m => m.PERSONELTC == yonetici.YONETICITC || m.PERSONELTELEFON == yonetici.YONETICITELEFON || m.PERSONELMAIL == yonetici.YONETICIMAIL);

                var musteriKontrol = db.TBL_MUSTERI.Where(m => m.MUSTERITC == yonetici.YONETICITC || m.MUSTERITELEFON == yonetici.YONETICITELEFON || m.MUSTERIMAIL == yonetici.YONETICIMAIL);

                var yoneticiKontrol = db.TBL_YONETICI.Where(m => m.YONETICITC == yonetici.YONETICITC || m.YONETICITELEFON == yonetici.YONETICITELEFON || m.YONETICIMAIL == yonetici.YONETICIMAIL);

                if (personelKontrol.Any())
                {

                    ViewBag.Message = "Bir personel ile aynı TC veya Telefona veya maile sahip bir yonetici olusturulamaz!";

                    SubelerVeCinsiyetler();

                    return View();


                }

                if (musteriKontrol.Any())
                {

                    ViewBag.Message = "Bir musteri ile aynı TC veya Telefona veya maile sahip bir yonetici olusturulamaz!";

                    SubelerVeCinsiyetler();

                    return View();


                }


                if (yoneticiKontrol.Any())
                {

                    ViewBag.Message = "Bir yonetici ile aynı TC veya Telefona veya maile sahip bir yonetici olusturulamaz!";

                    SubelerVeCinsiyetler();

                    return View();


                }


                var prsl = db.TBL_PERSONEL.Where(m => m.KULLANICIADI == yonetici.KULLANICIADI || m.SIFRE == yonetici.SIFRE);
                var yonetci = db.TBL_YONETICI.Where(m => m.KULLANICIADI == yonetici.KULLANICIADI || m.SIFRE == yonetici.SIFRE);
                var admin = db.TBL_ADMIN.Where(m => m.KULLANICIADI == yonetici.KULLANICIADI || m.SIFRE == yonetici.SIFRE);


                if (prsl.Any())
                {

                    ViewBag.Message = "Boyle bir kullanıcı adı veya şifreye sahip personel var!";

                    SubelerVeCinsiyetler();

                    return View();

                }

                if (yonetci.Any())
                {

                    ViewBag.Message = "Boyle bir kullanıcı adı veya sifreye sahip yonetici var!";

                    SubelerVeCinsiyetler();

                    return View();


                }

                if (admin.Any())
                {

                    ViewBag.Message = "Boyle bir kullanıcı adı veya şifreye sahip admin var!";

                    SubelerVeCinsiyetler();

                    return View();


                }
          
             
                yonetici.YONETICIDURUM = true;

                yonetici.YONETICIBASLANGICTARIHI = DateTime.Now;

                yonetici.KULLANICITIPI = "Yönetici";

                db.TBL_YONETICI.Add(yonetici);

                db.SaveChanges();

                ViewBag.Message = "Yonetici basariyla eklendi!";

                SubelerVeCinsiyetler();

                return View();

            }
            else
            {

             
                SubelerVeCinsiyetler();

                return View();

            }



        }

        [Authorize(Roles="Admin")]
        [HttpGet]
        public ActionResult YoneticiGuncelle(int id)
        {

            var yonetici = db.TBL_YONETICI.Find(id);

            SubelerVeCinsiyetler();

            return View(yonetici);

        }

        [HttpPost]
        public ActionResult YoneticiGuncelle(TBL_YONETICI yonetici)
        {

            if (String.IsNullOrEmpty(yonetici.YONETICIAD) || String.IsNullOrEmpty(yonetici.YONETICISOYAD) || String.IsNullOrEmpty(yonetici.YONETICIMAIL) || String.IsNullOrEmpty(yonetici.YONETICITELEFON) || String.IsNullOrEmpty(yonetici.YONETICITC) || yonetici.YONETICIMAAS == null || yonetici.YONETICIYAS == null || String.IsNullOrEmpty(yonetici.KULLANICIADI) || String.IsNullOrEmpty(yonetici.SIFRE))
            {

                ViewBag.Message = "Boş değer girmeyin!";

                SubelerVeCinsiyetler();

                return View();

            }

            try
            {

                var yntc = db.TBL_YONETICI.Where(m=>m.YONETICIID != yonetici.YONETICIID).Where(m => m.TBL_SUBE.SUBEID == yonetici.TBL_SUBE.SUBEID && m.YONETICIDURUM == true).ToList();

                if (!yntc.Any())
                {

                    var sube = db.TBL_SUBE.Where(m => m.SUBEID == yonetici.TBL_SUBE.SUBEID).FirstOrDefault();
                    yonetici.TBL_SUBE = sube;

                }
                else
                {

                    ViewBag.Message = "Sectiginiz subenin bir yoneticisi var!";

                    SubelerVeCinsiyetler();

                    return View();

                }

            }
            catch
            {

                ViewBag.Message = "Once sube ekleyiniz!";

                SubelerVeCinsiyetler();

                return View();

            }


            if (ModelState.IsValid)
            {

                char[] TC = yonetici.YONETICITC.ToCharArray();
                char[] Ad = yonetici.YONETICIAD.ToCharArray();
                char[] Soyad = yonetici.YONETICISOYAD.ToCharArray();

                foreach (char x in TC)
                {

                    if (!char.IsDigit(x))
                    {

                        ViewBag.Message = "TC Kimlik Numarası rakamlardan oluşmalıdır!";
                        SubelerVeCinsiyetler();
                        return View();
                    }

                }

                foreach (char x in Ad)
                {

                    if (!char.IsLetter(x))
                    {

                        ViewBag.Message = "Ad harflerden oluşmalıdır!";
                        SubelerVeCinsiyetler();
                        return View();



                    }

                }

                foreach (char x in Soyad)
                {

                    if (!char.IsLetter(x))
                    {

                        ViewBag.Message = "Soyad harflerden oluşmalıdır!";
                        SubelerVeCinsiyetler();
                        return View();

                    }

                }


                char[] kullaniciAdi = yonetici.KULLANICIADI.ToCharArray();
                char[] kullaniciSifre = yonetici.SIFRE.ToCharArray();

                foreach (char x in kullaniciAdi)
                {

                    if (!Char.IsLetterOrDigit(x))
                    {

                        ViewBag.Message = "Kullanıcı adı sadece harflerden ve rakamlardan oluşmalıdır!";

                        SubelerVeCinsiyetler();

                        return View();
                    }

                }

                foreach (char x in kullaniciSifre)
                {

                    if (!Char.IsDigit(x))
                    {

                        ViewBag.Message = "Şifre sadece rakamlardan oluşmalıdır!";

                        SubelerVeCinsiyetler();

                        return View();


                    }


                }


                var personelKontrol = db.TBL_PERSONEL.Where(m => m.PERSONELTC == yonetici.YONETICITC || m.PERSONELTELEFON == yonetici.YONETICITELEFON || m.PERSONELMAIL == yonetici.YONETICIMAIL);

                var musteriKontrol = db.TBL_MUSTERI.Where(m => m.MUSTERITC == yonetici.YONETICITC || m.MUSTERITELEFON == yonetici.YONETICITELEFON || m.MUSTERIMAIL == yonetici.YONETICIMAIL);

                var yoneticiKontrol = db.TBL_YONETICI.Where(m => m.YONETICIID != yonetici.YONETICIID).Where(m => m.YONETICITC == yonetici.YONETICITC || m.YONETICITELEFON == yonetici.YONETICITELEFON || m.YONETICIMAIL == yonetici.YONETICIMAIL);

                if (personelKontrol.Any())
                {

                    ViewBag.Message = "Bir personel ile aynı TC veya Telefona veya maile sahip bir yonetici olusturulamaz!";

                    SubelerVeCinsiyetler();

                    return View();


                }

                if (musteriKontrol.Any())
                {

                    ViewBag.Message = "Bir musteri ile aynı TC veya Telefona veya maile sahip bir yonetici olusturulamaz!";

                    SubelerVeCinsiyetler();

                    return View();


                }


                if (yoneticiKontrol.Any())
                {

                    ViewBag.Message = "Bir yonetici ile aynı TC veya Telefona veya maile sahip bir yonetici olusturulamaz!";

                    SubelerVeCinsiyetler();

                    return View();


                }



                var prsl = db.TBL_PERSONEL.Where(m => m.KULLANICIADI == yonetici.KULLANICIADI || m.SIFRE == yonetici.SIFRE);
                var yonetci = db.TBL_YONETICI.Where(m=>m.YONETICIID != yonetici.YONETICIID).Where(m => m.KULLANICIADI == yonetici.KULLANICIADI || m.SIFRE == yonetici.SIFRE);
                var admin = db.TBL_ADMIN.Where(m => m.KULLANICIADI == yonetici.KULLANICIADI || m.SIFRE == yonetici.SIFRE);


                if (prsl.Any())
                {

                    ViewBag.Message = "Boyle bir kullanıcı adı veya şifreye sahip personel var!";

                    SubelerVeCinsiyetler();

                    return View();

                }

                if (yonetci.Any())
                {

                    ViewBag.Message = "Boyle bir kullanıcı adı veya sifreye sahip yonetici var!";

                    SubelerVeCinsiyetler();

                    return View();


                }

                if (admin.Any())
                {

                    ViewBag.Message = "Boyle bir kullanıcı adı veya şifreye sahip admin var!";

                    SubelerVeCinsiyetler();

                    return View();


                }

                              
                var Yonetici = db.TBL_YONETICI.Find(yonetici.YONETICIID);

                Yonetici.YONETICIAD = yonetici.YONETICIAD;

                Yonetici.YONETICISOYAD = yonetici.YONETICISOYAD;

                Yonetici.YONETICIMAIL = yonetici.YONETICIMAIL;

                Yonetici.YONETICITC = yonetici.YONETICITC;

                Yonetici.YONETICITELEFON = yonetici.YONETICITELEFON;

                Yonetici.YONETICIMAAS = yonetici.YONETICIMAAS;

                Yonetici.YONETICICINSIYET = yonetici.YONETICICINSIYET;

                Yonetici.YONETICIYAS = yonetici.YONETICIYAS;

                Yonetici.KULLANICIADI = yonetici.KULLANICIADI;

                Yonetici.SIFRE = yonetici.SIFRE;

                Yonetici.TBL_SUBE = yonetici.TBL_SUBE;

                db.SaveChanges();

                ViewBag.Message = "Yonetici basariyla guncellendi!";

                SubelerVeCinsiyetler();

                return View();

            }
            else
            {


                SubelerVeCinsiyetler();

                return View();

            }




        }


    }
}