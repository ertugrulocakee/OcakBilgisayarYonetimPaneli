using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MVCTeknolojikMarketYonetimPaneli.Models.Model;
using PagedList.Mvc;
using PagedList;

namespace MVCTeknolojikMarketYonetimPaneli.Controllers
{
    public class SatisController : Controller
    {
        // GET: Satis

        TeknolojikMarketEntities db = new TeknolojikMarketEntities();


        [Authorize(Roles="Personel")]
        public ActionResult Index(string personeladsoyad, string musteriadsoyad, string urunad, int sayfa=1)
        {

            int subeID = Convert.ToInt32(Session["PersonelSube"].ToString());

            var satislar = db.TBL_SATIS.Where(m=> m.TBL_SUBE.SUBEID == subeID).Where(m=> m.SATISDURUM == true).ToList();


            string str = Request.Params["btnSatisTarihi"];

            if (!String.IsNullOrEmpty(str))
            {

                if (str == "a-z")
                {

                    satislar = satislar.OrderBy(m => m.SATISTARIHI).ToList();


                }
                else if (str == "z-a")
                {

                    satislar = satislar.OrderByDescending(m => m.SATISTARIHI).ToList();

                }


            }


            if(!String.IsNullOrEmpty(personeladsoyad)){

                satislar = satislar.Where(m=>(m.TBL_PERSONEL.PERSONELAD+" "+m.TBL_PERSONEL.PERSONELSOYAD).Contains(personeladsoyad)).ToList();
                             

            }

            
            if(!String.IsNullOrEmpty(musteriadsoyad)){

                satislar = satislar.Where(m=>(m.TBL_MUSTERI.MUSTERIAD+" "+m.TBL_MUSTERI.MUSTERISOYAD).Contains(musteriadsoyad)).ToList();
                             

            }

            if(!String.IsNullOrEmpty(urunad)){

                satislar = satislar.Where(m=>m.TBL_URUN.URUNAD.Contains(urunad)).ToList();
                             

            }


            return View(satislar.ToPagedList(sayfa, 20));

        }

        [Authorize(Roles = "Personel")]
        [HttpGet]
        public ActionResult YeniSatis()
        {


            musterilerVeUrunler();


            return View();
        }

        [HttpPost]
        public ActionResult YeniSatis(TBL_SATIS satis)
        {
           

            int subeID = Convert.ToInt32(Session["PersonelSube"].ToString());

            string kullaniciAdi = Session["KullaniciAdi"].ToString();

            var personel = db.TBL_PERSONEL.Where(m=>m.KULLANICIADI == kullaniciAdi).FirstOrDefault();

            if (!personel.TBL_BOLUM.BOLUMAD.Equals("Satış"))
            {

                ViewBag.Message = "Satış Islemini sadece satış personeli yapabilir!";

                musterilerVeUrunler();

                return View();

            }

            var sube = db.TBL_SUBE.Where(m => m.SUBEID == subeID).FirstOrDefault();

            try
            {

                var musteri = db.TBL_MUSTERI.Where(m => m.MUSTERIID == satis.TBL_MUSTERI.MUSTERIID).FirstOrDefault();
                var urun = db.TBL_URUN.Where(m => m.URUNID == satis.TBL_URUN.URUNID).FirstOrDefault();

                satis.TBL_MUSTERI = musteri;
                satis.TBL_URUN = urun;

            }
            catch
            {

                ViewBag.Message = "Satış Islemini kayıt etmeden once lutfen bir musteri ve urun ekleyiniz!";

                musterilerVeUrunler();

                return View();

            }


            var urn = db.TBL_URUN.Find(satis.TBL_URUN.URUNID);

            if (urn.URUNSTOK == 0)
            {

                ViewBag.Message = "Stokta secilen urun kalmamis!";
                musterilerVeUrunler();
                return View();

            }


            satis.TBL_PERSONEL = personel;       
            satis.TBL_SUBE = sube;

            satis.SATISTARIHI = DateTime.Now;

            satis.SATISDURUM = true;
     
            satis.SATISFIYATI = urn.URUNSATISFIYAT;

            db.TBL_SATIS.Add(satis);

            db.SaveChanges();

            musterilerVeUrunler();

            ViewBag.Message = "Satış işlemi başarıyla kayıt edildi!";

            return View();

        }


        [Authorize(Roles="Personel")]
        [HttpGet]
        public ActionResult SatisGetir(int id)
        {


            var satis = db.TBL_SATIS.Find(id);

            musterilerVeUrunler();

            return View(satis);

        }



        [HttpPost]
        public ActionResult SatisGetir(TBL_SATIS satis)
        {

            string kullaniciAdi = Session["KullaniciAdi"].ToString();

            var personel = db.TBL_PERSONEL.Where(m => m.KULLANICIADI == kullaniciAdi).FirstOrDefault();

            if (!personel.TBL_BOLUM.BOLUMAD.Equals("Satış"))
            {

                ViewBag.Message = "Satış Islemini sadece satış personeli yapabilir!";

                musterilerVeUrunler();

                return View();

            }


            try
            {

                var musteri = db.TBL_MUSTERI.Where(m => m.MUSTERIID == satis.TBL_MUSTERI.MUSTERIID).FirstOrDefault();
                var urun = db.TBL_URUN.Where(m => m.URUNID == satis.TBL_URUN.URUNID).FirstOrDefault();
           
                satis.TBL_MUSTERI = musteri;
                satis.TBL_URUN = urun;
               
            }
            catch
            {

                ViewBag.Message = "Satış işlemini guncellemeden once lutfen bir muşteri ve urun eklemesi yapin!";

                musterilerVeUrunler();

                return View();


            }

            var urn = db.TBL_URUN.Find(satis.TBL_URUN.URUNID);

            if (urn.URUNSTOK == 0)
            {

                ViewBag.Message = "Stokta secilen urun kalmamis!";
                musterilerVeUrunler();
                return View();

            }

            var sts = db.TBL_SATIS.Find(satis.SATISID);

            sts.TBL_URUN = satis.TBL_URUN;

            sts.TBL_MUSTERI = satis.TBL_MUSTERI;

            sts.SATISTARIHI = DateTime.Now;

            sts.SATISFIYATI = urn.URUNSATISFIYAT;

            db.SaveChanges();

            ViewBag.Message = "Satış işlemi başarıyla guncellendi!";

            musterilerVeUrunler();

            return View();
                
        }

        public ActionResult SatisSil(int id)
        {

            var satis = db.TBL_SATIS.Find(id);

            satis.SATISDURUM = false;

            db.SaveChanges();

            return RedirectToAction("Index", "Satis");

        }


        [Authorize(Roles = "Personel")]
        public ActionResult SilinmisSatislar(string personeladsoyad, string musteriadsoyad, string urunad, int sayfa = 1)
        {

            int subeID = Convert.ToInt32(Session["PersonelSube"].ToString());

            var satislar = db.TBL_SATIS.Where(m => m.TBL_SUBE.SUBEID == subeID).Where(m => m.SATISDURUM == false).ToList();


            string str = Request.Params["btnSatisTarihi"];

            if (!String.IsNullOrEmpty(str))
            {

                if (str == "a-z")
                {

                    satislar = satislar.OrderBy(m => m.SATISTARIHI).ToList();


                }
                else if (str == "z-a")
                {

                    satislar = satislar.OrderByDescending(m => m.SATISTARIHI).ToList();

                }


            }


            if (!String.IsNullOrEmpty(personeladsoyad))
            {

                satislar = satislar.Where(m => (m.TBL_PERSONEL.PERSONELAD + " " + m.TBL_PERSONEL.PERSONELSOYAD).Contains(personeladsoyad)).ToList();


            }


            if (!String.IsNullOrEmpty(musteriadsoyad))
            {

                satislar = satislar.Where(m => (m.TBL_MUSTERI.MUSTERIAD + " " + m.TBL_MUSTERI.MUSTERISOYAD).Contains(musteriadsoyad)).ToList();


            }

            if (!String.IsNullOrEmpty(urunad))
            {

                satislar = satislar.Where(m => m.TBL_URUN.URUNAD.Contains(urunad)).ToList();


            }


            return View(satislar.ToPagedList(sayfa, 20));
            
        }


        public ActionResult SatisiGeriGetir(int id)
        {

            var satis = db.TBL_SATIS.Find(id);

            satis.SATISDURUM = true;

            db.SaveChanges();

            return RedirectToAction("Index", "Satis");

        }



        protected void musterilerVeUrunler()
        {

             int personelID = Convert.ToInt32(Session["PersonelSube"].ToString());

            List<SelectListItem> musteriler = (from i in db.TBL_MUSTERI.Where(m=>m.TBL_SUBE.SUBEID == personelID && m.MUSTERIDURUM == true).ToList()
                                            select new SelectListItem
                                            {

                                                Text = i.MUSTERIAD+" "+i.MUSTERISOYAD,
                                                Value = i.MUSTERIID.ToString()

                                            }).ToList();

            ViewBag.mst = musteriler;


            List<SelectListItem> urunler = (from i in db.TBL_URUN.Where(m=>m.TBL_SUBE.SUBEID == personelID && m.URUNDURUM == true).ToList()
                                            select new SelectListItem
                                            {

                                                Text = i.URUNAD,
                                                Value = i.URUNID.ToString()

                                            }).ToList();

            ViewBag.urn = urunler;



        }


    }
}