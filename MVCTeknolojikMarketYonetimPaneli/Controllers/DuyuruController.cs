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
    public class DuyuruController : Controller
    {
        // GET: Duyuru

        TeknolojikMarketEntities db = new TeknolojikMarketEntities();

        [Authorize(Roles = "Personel,Yönetici")]       
        public ActionResult Index(int sayfa = 1)
        {

            int subeId = Convert.ToInt32(Session["Sube"].ToString());

            var duyurular = db.TBL_DUYURU.Where(m => m.TBL_SUBE.SUBEID == subeId && m.DURUM == true).ToList();


            string str = Request.Params["btnmesajtarih"];

            if (!String.IsNullOrEmpty(str))
            {


                if (str == "a-z")
                {

                    duyurular = duyurular.OrderBy(m => m.TARIH).ToList();


                }
                else if (str == "z-a")
                {

                    duyurular = duyurular.OrderByDescending(m => m.TARIH).ToList();

                }




            }



            return View(duyurular.ToPagedList(sayfa, 10));
        }

        [Authorize(Roles="Personel,Yönetici")]
        public ActionResult DuyuruGoruntule(int id)
        {

            var duyuru = db.TBL_DUYURU.Where(m => m.DUYURUID == id).FirstOrDefault();


            return View(duyuru);

        }



        [Authorize(Roles="Yönetici")]
        [HttpGet]
        public ActionResult DuyuruEkle()
        {


            return View();
        }



        [HttpPost]
        public ActionResult DuyuruEkle(TBL_DUYURU duyuru)
        {

            int subeId = Convert.ToInt32(Session["YoneticiSube"].ToString());

            string kullaniciAdi =  Session["KullaniciAdi"].ToString();

            var sube = db.TBL_SUBE.Where(m=>m.SUBEID == subeId).FirstOrDefault();

            var yonetici = db.TBL_YONETICI.Where(m=>m.KULLANICIADI == kullaniciAdi).FirstOrDefault();

            if (ModelState.IsValid)
            {

                duyuru.TBL_SUBE = sube;

                duyuru.TBL_YONETICI = yonetici;

                duyuru.TARIH = DateTime.Now;

                duyuru.DURUM = true;

                db.TBL_DUYURU.Add(duyuru);

                db.SaveChanges();

                ViewBag.Message = "Duyuru başarıyla oluşturuldu!";

                return View();
            }
            else
            {

                return View();

            }


        }

        public ActionResult DuyuruSil(int id)
        {


            var duyuru = db.TBL_DUYURU.Find(id);

            duyuru.DURUM = false;

            db.SaveChanges();

            return RedirectToAction("Index","Duyuru");

        }

        public ActionResult DuyuruGeriGetir(int id)
        {

            var duyuru = db.TBL_DUYURU.Find(id);

            duyuru.DURUM = true;

            db.SaveChanges();

            return RedirectToAction("Index", "Duyuru");


        }


        [Authorize(Roles = "Yönetici")]
        [HttpGet]
        public ActionResult DuyuruGuncelle(int id)
        {


            var duyuru = db.TBL_DUYURU.Find(id);

            return View(duyuru); 
        }


        [HttpPost]
        public ActionResult DuyuruGuncelle(TBL_DUYURU duyuru)
        {


            if (ModelState.IsValid)
            {

                var dyru = db.TBL_DUYURU.Find(duyuru.DUYURUID);

                dyru.TARIH = DateTime.Now;

                dyru.DUYURUICERIGI = duyuru.DUYURUICERIGI;

                db.SaveChanges();

                ViewBag.Message = "Duyuru basariyla guncellendi!";

                return View();

            }
            else
            {
            
                return View();

            }



        }


        [Authorize(Roles = "Yönetici")]
        public ActionResult SilinmisDuyurular(int sayfa = 1)
        {

            int subeId = Convert.ToInt32(Session["Sube"].ToString());

            var duyurular = db.TBL_DUYURU.Where(m => m.TBL_SUBE.SUBEID == subeId && m.DURUM == false).ToList();


            string str = Request.Params["btnmesajtarih"];

            if (!String.IsNullOrEmpty(str))
            {


                if (str == "a-z")
                {

                    duyurular = duyurular.OrderBy(m => m.TARIH).ToList();


                }
                else if (str == "z-a")
                {

                    duyurular = duyurular.OrderByDescending(m => m.TARIH).ToList();

                }


            }


            return View(duyurular.ToPagedList(sayfa, 10)); 

        }



    }
}

