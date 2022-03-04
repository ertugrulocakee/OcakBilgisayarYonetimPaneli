using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PagedList;
using PagedList.Mvc;
using MVCTeknolojikMarketYonetimPaneli.Models.Model;
using MVCTeknolojikMarketYonetimPaneli.Models.EkModel;

namespace MVCTeknolojikMarketYonetimPaneli.Controllers
{
    public class MesajController : Controller
    {
        // GET: Mesaj

        TeknolojikMarketEntities db = new TeknolojikMarketEntities();


        [Authorize(Roles="Personel")]
        public ActionResult Index(string mesajbasligi, int sayfa = 1)
        {

            string kullaniciAdi = Session["KullaniciAdi"].ToString();

            string str = Request.Params["btnmesajtarih"];

            var mesajlar = db.TBL_MESAJYONPER.Where(m => m.DURUM == true && m.TBL_PERSONEL.KULLANICIADI == kullaniciAdi).ToList();

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

            var mesaj = db.TBL_MESAJYONPER.Where(m => m.MESAJID == id).FirstOrDefault();

            mesaj.DURUM = false;

            db.SaveChanges();

            return RedirectToAction("Index", "Mesaj");

        }


        [Authorize(Roles="Personel")]  
        [HttpGet]
        public ActionResult MesajGoruntule(int id)
        {

            if (TempData["ileti"] != null)
            {

                ViewBag.Message = TempData["ileti"].ToString();

            }


          var mesaj = db.TBL_MESAJYONPER.Where(m => m.MESAJID == id).FirstOrDefault();

          
          return View(Tuple.Create<TBL_MESAJYONPER, TBL_MESAJPERYON>(mesaj, new TBL_MESAJPERYON()));

        }


        [Authorize(Roles = "Personel")]
        public ActionResult SilinmisMesajlar(string mesajbasligi, int sayfa = 1)
        {


            string kullaniciAdi = Session["KullaniciAdi"].ToString();

            string str = Request.Params["btnmesajtarih"];

            var mesajlar = db.TBL_MESAJYONPER.Where(m => m.DURUM == false && m.TBL_PERSONEL.KULLANICIADI == kullaniciAdi).ToList();

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


            var mesaj = db.TBL_MESAJYONPER.Where(m => m.MESAJID == id).FirstOrDefault();

            mesaj.DURUM = true;

            db.SaveChanges();

            return RedirectToAction("Index", "Mesaj");


        }

             
        [HttpPost]
        public ActionResult MesajGoruntule([Bind(Prefix = "Item1")]TBL_MESAJYONPER Model1,[Bind(Prefix = "Item2")]TBL_MESAJPERYON Model2)
        {
            
                string kullaniciAdi = Session["KullaniciAdi"].ToString();

                int subeId = Convert.ToInt32(Session["PersonelSube"].ToString());

                var personel = db.TBL_PERSONEL.Where(m => m.KULLANICIADI == kullaniciAdi).FirstOrDefault();

                var yonetici = db.TBL_YONETICI.Where(m => m.TBL_SUBE.SUBEID == subeId && m.YONETICIDURUM == true).FirstOrDefault();

                if (String.IsNullOrEmpty(Model2.MESAJICERIGI))
                {

                    TempData["ileti"] = "Yanıt yoneticiye iletilemedi! Yanitiniz bos olamaz ve 250 karakteri gecemez!";

                    return RedirectToAction("MesajGoruntule/" + Model1.MESAJID, "Mesaj");


                }

                if (ModelState.IsValid)
                {

                    Model2.TBL_PERSONEL = personel;

                    Model2.TBL_YONETICI = yonetici;

                    Model2.TARIH = DateTime.Now;

                    Model2.DURUM = true;

                    Model2.MESAJYON = false;

                    db.TBL_MESAJPERYON.Add(Model2);

                    db.SaveChanges();


                    return RedirectToAction("SohbetiGoster/"+Model1.MESAJID,"Mesaj");


                }
                else
                {

                    TempData["ileti"] = "Yanıt yoneticiye iletilemedi! Yanitiniz bos olamaz ve 250 karakteri gecemez!";

                    return RedirectToAction("MesajGoruntule/" + Model1.MESAJID, "Mesaj");

                }


            }


        [Authorize(Roles="Personel")]
        [HttpGet]
        public ActionResult SohbetiGoster(int id)
        {


            if (TempData["ileti"] != null)
            {

                ViewBag.Message = TempData["ileti"].ToString();

            }

           
            var mesaj = db.TBL_MESAJYONPER.Find(id);

            string baslik =  mesaj.MESAJBASLIGI.ToString();

            
            var mesajlar = db.SOHBET(baslik).ToList();

            Class1 mesajIdBaslik = new Class1();

            mesajIdBaslik.MesajId = id;

            mesajIdBaslik.MesajBaslik = baslik;
            
            return View(Tuple.Create<List<SOHBET_Result>, TBL_MESAJPERYON, Class1>(mesajlar, new TBL_MESAJPERYON(),mesajIdBaslik));

        }


        [HttpPost]
        public ActionResult SohbetiGoster([Bind(Prefix = "Item1")]List<SOHBET_Result> Model1, [Bind(Prefix = "Item2")]TBL_MESAJPERYON Model2, [Bind(Prefix="Item3")]Class1 Model3)
        {

            string kullaniciAdi = Session["KullaniciAdi"].ToString();

            int subeId = Convert.ToInt32(Session["PersonelSube"].ToString());

            var personel = db.TBL_PERSONEL.Where(m => m.KULLANICIADI == kullaniciAdi).FirstOrDefault();

            var yonetici = db.TBL_YONETICI.Where(m => m.TBL_SUBE.SUBEID == subeId && m.YONETICIDURUM == true).FirstOrDefault();

            if (String.IsNullOrEmpty(Model2.MESAJICERIGI))
            {

                TempData["ileti"] = "Yanıt yoneticiye iletilemedi! Yanitiniz bos olamaz ve 250 karakteri gecemez!";

                return RedirectToAction("SohbetiGoster/" + Model3.MesajId, "Mesaj");


            }

            if (ModelState.IsValid)
            {

                Model2.TBL_PERSONEL = personel;

                Model2.TBL_YONETICI = yonetici;

                Model2.TARIH = DateTime.Now;

                Model2.DURUM = true;

                Model2.MESAJYON = false;

                db.TBL_MESAJPERYON.Add(Model2);

                db.SaveChanges();


                return RedirectToAction("SohbetiGoster/"+Model3.MesajId,"Mesaj");


            }
            else
            {

                TempData["ileti"] = "Yanıt yoneticiye iletilemedi! Yanitiniz bos olamaz ve 250 karakteri gecemez!";

                return RedirectToAction("SohbetiGoster/"+ Model3.MesajId,"Mesaj");

            }


        }

        [Authorize(Roles="Personel")]
        public ActionResult SilinmisMesajiGoster(int id)
        {


            var mesaj = db.TBL_MESAJYONPER.Where(m => m.MESAJID == id).FirstOrDefault();


            return View(mesaj);


        }


        [Authorize(Roles="Personel")]
        [HttpGet]
        public ActionResult YeniMesaj()
        {


            return View();

        }

        [HttpPost]
        public ActionResult YeniMesaj(TBL_MESAJPERYON mesaj)
        {

            if (String.IsNullOrEmpty(mesaj.MESAJBASLIGI) || String.IsNullOrEmpty(mesaj.MESAJICERIGI))
            {


                ViewBag.Message = "Mesaj başlığı ve icerigi boş olamaz!";

                return View();

            }

            string kullaniciAdi = Session["KullaniciAdi"].ToString();

            int subeId = Convert.ToInt32(Session["PersonelSube"].ToString());

            var personel = db.TBL_PERSONEL.Where(m => m.KULLANICIADI == kullaniciAdi).FirstOrDefault();

            var yonetici = db.TBL_YONETICI.Where(m => m.TBL_SUBE.SUBEID == subeId && m.YONETICIDURUM == true).FirstOrDefault();



            if (ModelState.IsValid)
            {


                var msjPersonel = db.TBL_MESAJPERYON.Where(m => m.MESAJBASLIGI == mesaj.MESAJBASLIGI).ToList();

                var msjYonetici = db.TBL_MESAJYONPER.Where(m => m.MESAJBASLIGI == mesaj.MESAJBASLIGI).ToList();

                if (msjPersonel.Any())
                {

                    ViewBag.Message = "Boyle bir baslik daha once firma icinde acilmis!";

                    return View();

                }

                if (msjYonetici.Any())
                {


                    ViewBag.Message = "Boyle bir baslik daha once firma icinde acilmis!";

                    return View();

                }


                mesaj.TBL_PERSONEL = personel;

                mesaj.TBL_YONETICI = yonetici;

                mesaj.TARIH = DateTime.Now;

                mesaj.DURUM = true;

                mesaj.MESAJYON = false;

                db.TBL_MESAJPERYON.Add(mesaj);

                db.SaveChanges();

                return RedirectToAction("SohbetiGosterIki/"+mesaj.MESAJID,"Mesaj");


            }
            else
            {

                return View();


            }
            
       
        }

        [Authorize(Roles = "Personel")]
        public ActionResult GidenMesajlar(string mesajbasligi, int sayfa = 1)
        {

            string kullaniciAdi = Session["KullaniciAdi"].ToString();

            string str = Request.Params["btnmesajtarih"];

            var mesajlar = db.TBL_MESAJPERYON.Where(m => m.DURUM == true && m.TBL_PERSONEL.KULLANICIADI == kullaniciAdi).ToList();

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


        [Authorize(Roles = "Personel")]
        public ActionResult GidenMesajiGoruntule(int id)
        {

            var mesaj = db.TBL_MESAJPERYON.Where(m => m.MESAJID == id).FirstOrDefault();


            return View(mesaj);


        }


        [Authorize(Roles="Personel")]
        [HttpGet]
        public ActionResult SohbetiGosterIki(int id)
        {

            if (TempData["ileti"] != null)
            {

                ViewBag.Message = TempData["ileti"].ToString();

            }

            var mesaj = db.TBL_MESAJPERYON.Find(id);

            string baslik = mesaj.MESAJBASLIGI.ToString();

            var mesajlar = db.SOHBET(baslik).ToList();

            Class1 mesajIdBaslik = new Class1();

            mesajIdBaslik.MesajId = id;

            mesajIdBaslik.MesajBaslik = baslik;

            return View(Tuple.Create<List<SOHBET_Result>, TBL_MESAJPERYON, Class1>(mesajlar, new TBL_MESAJPERYON(), mesajIdBaslik));

        }

        [HttpPost]
        public ActionResult SohbetiGosterIki([Bind(Prefix = "Item1")]List<SOHBET_Result> Model1, [Bind(Prefix = "Item2")]TBL_MESAJPERYON Model2, [Bind(Prefix = "Item3")]Class1 Model3)
        {

            string kullaniciAdi = Session["KullaniciAdi"].ToString();

            int subeId = Convert.ToInt32(Session["PersonelSube"].ToString());

            var personel = db.TBL_PERSONEL.Where(m => m.KULLANICIADI == kullaniciAdi).FirstOrDefault();

            var yonetici = db.TBL_YONETICI.Where(m => m.TBL_SUBE.SUBEID == subeId && m.YONETICIDURUM == true).FirstOrDefault();

            if (String.IsNullOrEmpty(Model2.MESAJICERIGI))
            {

                TempData["ileti"] = "Yanıt yoneticiye iletilemedi! Yanitiniz bos olamaz ve 250 karakteri gecemez!";

                return RedirectToAction("SohbetiGosterIki/" + Model3.MesajId, "Mesaj");


            }

            if (ModelState.IsValid)
            {

                Model2.TBL_PERSONEL = personel;

                Model2.TBL_YONETICI = yonetici;

                Model2.TARIH = DateTime.Now;

                Model2.DURUM = true;

                Model2.MESAJYON = false;

                db.TBL_MESAJPERYON.Add(Model2);

                db.SaveChanges();


                return RedirectToAction("SohbetiGosterIki/" + Model3.MesajId, "Mesaj");


            }
            else
            {

                TempData["ileti"] = "Yanıt yoneticiye iletilemedi! Yanitiniz bos olamaz ve 250 karakteri gecemez!";

                return RedirectToAction("SohbetiGosterIki/" + Model3.MesajId, "Mesaj");

            }

        }

        public ActionResult GidenMesajiSil(int id)
        {

            var mesaj = db.TBL_MESAJPERYON.Where(m => m.MESAJID == id).FirstOrDefault();

            mesaj.DURUM = false;

            db.SaveChanges();

            return RedirectToAction("GidenMesajlar", "Mesaj");

        }


        [Authorize(Roles="Personel")]
        public ActionResult SilinmisGidenMesajlar(string mesajbasligi, int sayfa = 1)
        {

            string kullaniciAdi = Session["KullaniciAdi"].ToString();

            string str = Request.Params["btnmesajtarih"];

            var mesajlar = db.TBL_MESAJPERYON.Where(m => m.DURUM == false && m.TBL_PERSONEL.KULLANICIADI == kullaniciAdi).ToList();

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

            var mesaj = db.TBL_MESAJPERYON.Where(m => m.MESAJID == id).FirstOrDefault();

            mesaj.DURUM = true;

            db.SaveChanges();

            return RedirectToAction("GidenMesajlar", "Mesaj");


        }
            





    }
}