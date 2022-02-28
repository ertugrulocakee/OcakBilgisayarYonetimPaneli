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
    public class MesajYoneticiController : Controller
    {
        // GET: MesajYonetici

        TeknolojikMarketEntities db = new TeknolojikMarketEntities();


        [Authorize(Roles = "Yönetici")]
        public ActionResult Index(string mesajbasligi, int sayfa = 1)
        {

            string kullaniciAdi = Session["KullaniciAdi"].ToString();

            string str = Request.Params["btnmesajtarih"];

            var mesajlar = db.TBL_MESAJPERYON.Where(m => m.DURUM == true && m.TBL_YONETICI.KULLANICIADI == kullaniciAdi).ToList();

            if (!String.IsNullOrEmpty(str))
            {


                if (str == "a-z")
                {

                    mesajlar = mesajlar.OrderBy(m => m.TARIH).ToList();


                }
                else if (str == "z-a")
                {

                    mesajlar = mesajlar.OrderByDescending(m => m.TARIH).ToList();

                }




            }


            if (!String.IsNullOrEmpty(mesajbasligi))
            {

                mesajlar = mesajlar.Where(m => (m.MESAJBASLIGI).Contains(mesajbasligi)).ToList();


            }



            return View(mesajlar.ToPagedList(sayfa, 20));

        }


        public ActionResult MesajSil(int id)
        {

            var mesaj = db.TBL_MESAJPERYON.Where(m => m.MESAJID == id).FirstOrDefault();

            mesaj.DURUM = false;

            db.SaveChanges();

            return RedirectToAction("Index", "MesajYonetici");

        }


        [Authorize(Roles = "Yönetici")]
        [HttpGet]
        public ActionResult MesajGoruntule(int id)
        {

            if (TempData["ileti"] != null)
            {

                ViewBag.Message = TempData["ileti"].ToString();

            }


            var mesaj = db.TBL_MESAJPERYON.Where(m => m.MESAJID == id).FirstOrDefault();


            return View(Tuple.Create<TBL_MESAJPERYON, TBL_MESAJYONPER>(mesaj, new TBL_MESAJYONPER()));

        }

        [Authorize(Roles = "Yönetici")]
        public ActionResult SilinmisMesajlar(string mesajbasligi, int sayfa = 1)
        {


            string kullaniciAdi = Session["KullaniciAdi"].ToString();

            string str = Request.Params["btnmesajtarih"];

            var mesajlar = db.TBL_MESAJPERYON.Where(m => m.DURUM == false && m.TBL_YONETICI.KULLANICIADI == kullaniciAdi).ToList();

            if (!String.IsNullOrEmpty(str))
            {


                if (str == "a-z")
                {

                    mesajlar = mesajlar.OrderBy(m => m.TARIH).ToList();


                }
                else if (str == "z-a")
                {

                    mesajlar = mesajlar.OrderByDescending(m => m.TARIH).ToList();

                }




            }


            if (!String.IsNullOrEmpty(mesajbasligi))
            {

                mesajlar = mesajlar.Where(m => (m.MESAJBASLIGI).Contains(mesajbasligi)).ToList();


            }



            return View(mesajlar.ToPagedList(sayfa, 20));


        }


        public ActionResult MesajGeriGetir(int id)
        {


            var mesaj = db.TBL_MESAJPERYON.Where(m => m.MESAJID == id).FirstOrDefault();

            mesaj.DURUM = true;

            db.SaveChanges();

            return RedirectToAction("Index", "MesajYonetici");


        }


        [HttpPost]
        public ActionResult MesajGoruntule([Bind(Prefix = "Item1")]TBL_MESAJPERYON Model1, [Bind(Prefix = "Item2")]TBL_MESAJYONPER Model2)
        {

            string kullaniciAdi = Session["KullaniciAdi"].ToString();

            //int subeId = Convert.ToInt32(Session["YoneticiSube"].ToString());

            string personelKullaniciAdi = Model1.TBL_PERSONEL.KULLANICIADI.ToString();

            var yonetici = db.TBL_YONETICI.Where(m => m.KULLANICIADI == kullaniciAdi).FirstOrDefault();

            var personel = db.TBL_PERSONEL.Where(m => m.KULLANICIADI == personelKullaniciAdi).FirstOrDefault();

            if (String.IsNullOrEmpty(Model2.MESAJICERIGI))
            {

                TempData["ileti"] = "Yanıt personele iletilemedi! Yanitiniz bos olamaz ve 250 karakteri gecemez!";

                return RedirectToAction("MesajGoruntule/" + Model1.MESAJID, "MesajYonetici");


            }

            if (ModelState.IsValid)
            {

                Model2.TBL_PERSONEL = personel;

                Model2.TBL_YONETICI = yonetici;

                Model2.TARIH = DateTime.Now;

                Model2.DURUM = true;

                Model2.MESAJYON = true;

                db.TBL_MESAJYONPER.Add(Model2);

                db.SaveChanges();


                return RedirectToAction("SohbetiGoster/" + Model1.MESAJID, "MesajYonetici");


            }
            else
            {

              

                TempData["ileti"] = "Yanıt personele iletilemedi! Yanitiniz bos olamaz ve 250 karakteri gecemez!";

                return RedirectToAction("MesajGoruntule/" + Model1.MESAJID, "MesajYonetici");

            }


        }


        [Authorize(Roles = "Yönetici")]
        public ActionResult SohbetiGoster(int id)
        {


            var mesaj = db.TBL_MESAJPERYON.Find(id);

            string baslik = mesaj.MESAJBASLIGI.ToString();


            var mesajlar = db.SOHBET(baslik).ToList();

            return View(mesajlar);


        }


        [Authorize(Roles = "Yönetici")]
        public ActionResult SilinmisMesajiGoster(int id)
        {


            var mesaj = db.TBL_MESAJPERYON.Where(m => m.MESAJID == id).FirstOrDefault();


            return View(mesaj);


        }


        [Authorize(Roles = "Yönetici")]
        [HttpGet]
        public ActionResult YeniMesaj()
        {

            personelleriGetir();

            return View();

        }

        [HttpPost]
        public ActionResult YeniMesaj(TBL_MESAJYONPER mesaj)
        {

            if (String.IsNullOrEmpty(mesaj.MESAJBASLIGI) || String.IsNullOrEmpty(mesaj.MESAJICERIGI))
            {


                ViewBag.Message = "Mesaj baslıgı ve icerigi bos olamaz!";

                personelleriGetir();

                return View();

            }

            string kullaniciAdi = Session["KullaniciAdi"].ToString();

            

            var yonetici = db.TBL_YONETICI.Where(m => m.KULLANICIADI == kullaniciAdi).FirstOrDefault();

            try
            {

                var personel = db.TBL_PERSONEL.Where(m => m.PERSONELID == mesaj.TBL_PERSONEL.PERSONELID).FirstOrDefault();

                mesaj.TBL_PERSONEL = personel;

            }
            catch
            {

                ViewBag.Message = "Once bir personel ekleyiniz!";

                personelleriGetir();

                return View();
            }
     

            var msjPersonel = db.TBL_MESAJPERYON.Where(m => m.MESAJBASLIGI == mesaj.MESAJBASLIGI).ToList();

            var msjYonetici = db.TBL_MESAJYONPER.Where(m => m.MESAJBASLIGI == mesaj.MESAJBASLIGI).ToList();

            if (msjPersonel.Any())
            {

                ViewBag.Message = "Boyle bir baslik zaten firma icinde acilmis!";

                personelleriGetir();

                return View();

            }

            if (msjYonetici.Any())
            {

                ViewBag.Message = "Boyle bir baslik zaten firma icinde acilmis!";

                personelleriGetir();

                return View();

            }

            if (ModelState.IsValid)
            {

            

                mesaj.TBL_YONETICI = yonetici;

                mesaj.TARIH = DateTime.Now;

                mesaj.DURUM = true;

                mesaj.MESAJYON = true;

                db.TBL_MESAJYONPER.Add(mesaj);

                db.SaveChanges();

                
                return RedirectToAction("SohbetiGosterIki/" + mesaj.MESAJID, "MesajYonetici");


            }
            else
            {

               
                personelleriGetir();

                return View();


            }


        }


        protected void personelleriGetir()
        {

            int subeId = Convert.ToInt32(Session["YoneticiSube"].ToString());

            List<SelectListItem> personeller = (from i in db.TBL_PERSONEL.Where(m => m.PERSONELDURUM == true && m.PERSONELSUBE == subeId).ToList()
                                            select new SelectListItem
                                            {

                                                Text = i.PERSONELAD + " " + i.PERSONELSOYAD,
                                                Value = i.PERSONELID.ToString()

                                            }).ToList();

            ViewBag.per = personeller;



        }


        [Authorize(Roles = "Yönetici")]
        public ActionResult GidenMesajlar(string mesajbasligi, int sayfa = 1)
        {

            string kullaniciAdi = Session["KullaniciAdi"].ToString();

            string str = Request.Params["btnmesajtarih"];

            var mesajlar = db.TBL_MESAJYONPER.Where(m => m.DURUM == true && m.TBL_YONETICI.KULLANICIADI == kullaniciAdi).ToList();

            if (!String.IsNullOrEmpty(str))
            {


                if (str == "a-z")
                {

                    mesajlar = mesajlar.OrderBy(m => m.TARIH).ToList();


                }
                else if (str == "z-a")
                {

                    mesajlar = mesajlar.OrderByDescending(m => m.TARIH).ToList();

                }




            }


            if (!String.IsNullOrEmpty(mesajbasligi))
            {

                mesajlar = mesajlar.Where(m => (m.MESAJBASLIGI).Contains(mesajbasligi)).ToList();


            }



            return View(mesajlar.ToPagedList(sayfa, 20));

        }


        [Authorize(Roles = "Yönetici")]
        public ActionResult GidenMesajiGoruntule(int id)
        {

            var mesaj = db.TBL_MESAJYONPER.Where(m => m.MESAJID == id).FirstOrDefault();


            return View(mesaj);


        }


        [Authorize(Roles = "Yönetici")]
        public ActionResult SohbetiGosterIki(int id)
        {

            var mesaj = db.TBL_MESAJYONPER.Find(id);

            string baslik = mesaj.MESAJBASLIGI.ToString();


            var mesajlar = db.SOHBET(baslik).ToList();

            return View(mesajlar);


        }


        public ActionResult GidenMesajiSil(int id)
        {

            var mesaj = db.TBL_MESAJYONPER.Where(m => m.MESAJID == id).FirstOrDefault();

            mesaj.DURUM = false;

            db.SaveChanges();

            return RedirectToAction("GidenMesajlar", "MesajYonetici");

        }


        [Authorize(Roles = "Yönetici")]
        public ActionResult SilinmisGidenMesajlar(string mesajbasligi, int sayfa = 1)
        {

            string kullaniciAdi = Session["KullaniciAdi"].ToString();

            string str = Request.Params["btnmesajtarih"];

            var mesajlar = db.TBL_MESAJYONPER.Where(m => m.DURUM == false && m.TBL_YONETICI.KULLANICIADI == kullaniciAdi).ToList();

            if (!String.IsNullOrEmpty(str))
            {


                if (str == "a-z")
                {

                    mesajlar = mesajlar.OrderBy(m => m.TARIH).ToList();


                }
                else if (str == "z-a")
                {

                    mesajlar = mesajlar.OrderByDescending(m => m.TARIH).ToList();

                }




            }


            if (!String.IsNullOrEmpty(mesajbasligi))
            {

                mesajlar = mesajlar.Where(m => (m.MESAJBASLIGI).Contains(mesajbasligi)).ToList();


            }



            return View(mesajlar.ToPagedList(sayfa, 20));



        }



        public ActionResult GidenMesajiGeriGetir(int id)
        {

            var mesaj = db.TBL_MESAJYONPER.Where(m => m.MESAJID == id).FirstOrDefault();

            mesaj.DURUM = true;

            db.SaveChanges();

            return RedirectToAction("GidenMesajlar", "MesajYonetici");


        }
            






    }
}