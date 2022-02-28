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
    public class UrunController : Controller
    {
        // GET: Urun

        TeknolojikMarketEntities db = new TeknolojikMarketEntities();

        [Authorize(Roles="Personel")]
        public ActionResult Index(string ad, int sayfa = 1)
        {

            int subeId = Convert.ToInt32(Session["PersonelSube"].ToString());

            var urunler = db.TBL_URUN.Where(m => m.TBL_SUBE.SUBEID == subeId && m.URUNDURUM == true).ToList();


            if (!String.IsNullOrEmpty(ad))
            {

                urunler = urunler.Where(m => m.URUNAD.Contains(ad)).ToList();

            }

            return View(urunler.ToPagedList(sayfa, 10));
        }

    
        
        [Authorize(Roles="Personel")]
        [HttpGet]
        public ActionResult YeniUrun()
        {

            kategoriler();

            return View();

        }

        [HttpPost]
        public ActionResult YeniUrun(TBL_URUN urun)
        {

            try
            {

                var kategori = db.TBL_KATEGORI.Where(m => m.KATEGORIID == urun.TBL_KATEGORI.KATEGORIID).FirstOrDefault();
                urun.TBL_KATEGORI = kategori;
            }
            catch
            {

                ViewBag.Message = "Hic kategori yok! Urun eklemeden once kategori ekleyin!";

                kategoriler();

                return View();

            }


            if (String.IsNullOrEmpty(urun.URUNAD))
            {


                ViewBag.Message = "Urun adı boş olamaz!";

                kategoriler();

                return View();

            }

            int subeId = Convert.ToInt32(Session["PersonelSube"].ToString());

      
            if (ModelState.IsValid)
            {

                Char[] marka = urun.URUNMARKA.ToCharArray();

                foreach (char x in marka)
                {

                    if (!Char.IsLetter(x) && x != ' ')
                    {

                        ViewBag.Message = "Marka adı sadece harflerden oluşmalıdır!";

                        kategoriler();

                        return View();
                    }

                }

                var sube = db.TBL_SUBE.Where(m => m.SUBEID == subeId).FirstOrDefault();
                urun.TBL_SUBE = sube;

              
                var urunKontrol = db.TBL_URUN.Where(m => m.URUNAD == urun.URUNAD && m.URUNDURUM == true).Where(m => m.TBL_SUBE.SUBEID == subeId);

                var eskiUrunKontrol = db.TBL_URUN.Where(m => m.URUNAD == urun.URUNAD && m.URUNDURUM == false).Where(m => m.TBL_SUBE.SUBEID == subeId);

                if (urunKontrol.Any())
                {

                    ViewBag.Message = "Şubede boyle bir urun zaten var!";

                    kategoriler();

                    return View();

                }

                if (eskiUrunKontrol.Any())
                {

                    ViewBag.Message = "Şubede boyle bir urun zaten varmış ama silinmiş. Silinmiş Urunler sayfasından bu urunu geri getir!";

                    kategoriler();

                    return View();

                }

              
                    urun.URUNDURUM = true;

                    db.TBL_URUN.Add(urun);

                    db.SaveChanges();

                    ViewBag.Message = "Urun başarıyla eklendi!";

                    kategoriler();

                    return View();

            }
            else
            {
   
                kategoriler();

                return View();

            }

        }


        [Authorize(Roles="Personel")]
        [HttpGet]
        public ActionResult UrunGetir(int id)
        {

            var urun = db.TBL_URUN.Find(id);

            kategoriler();

            return View(urun);

        }


        [HttpPost]
        public ActionResult UrunGetir(TBL_URUN urun)
        {

            try
            {

                var kategori = db.TBL_KATEGORI.Where(m => m.KATEGORIID == urun.TBL_KATEGORI.KATEGORIID).FirstOrDefault();
                urun.TBL_KATEGORI = kategori;


            }
            catch
            {

                ViewBag.Message = "Hic kategori yok! Urunu guncellemeden once kategori ekleyin!";

                kategoriler();

                return View();

            }

            if (String.IsNullOrEmpty(urun.URUNAD))
            {


                ViewBag.Message = "Urun adı boş olamaz!";

                kategoriler();

                return View();

            }

            int subeId = Convert.ToInt32(Session["PersonelSube"].ToString());
    
            if (ModelState.IsValid)
            {

                Char[] marka = urun.URUNMARKA.ToCharArray();

                foreach (char x in marka)
                {

                    if (!Char.IsLetter(x) && x != ' ')
                    {

                        ViewBag.Message = "Marka adı sadece harflerden oluşmalıdır!";

                        kategoriler();

                        return View();
                    }

                }

        
                var urunKontrol = db.TBL_URUN.Where(m => m.URUNID != urun.URUNID).Where(m => m.URUNAD == urun.URUNAD && m.URUNDURUM == true).Where(m => m.TBL_SUBE.SUBEID == subeId);

                var eskiUrunKontrol = db.TBL_URUN.Where(m => m.URUNAD == urun.URUNAD && m.URUNDURUM == false).Where(m => m.TBL_SUBE.SUBEID == subeId);

                if (urunKontrol.Any())
                {

                    ViewBag.Message = "Şubede boyle bir urun zaten var!";

                    kategoriler();

                    return View();

                }

                if (eskiUrunKontrol.Any())
                {

                    ViewBag.Message = "Şubede boyle bir urun zaten varmış ama silinmiş. Silinmiş Urunler sayfasından bu urunu geri getir!";

                    kategoriler();

                    return View();

                }


                var urn = db.TBL_URUN.Find(urun.URUNID);

                urn.TBL_KATEGORI = urun.TBL_KATEGORI;

                urn.URUNAD = urun.URUNAD;
                urn.URUNSTOK = urun.URUNSTOK;

                urn.URUNALISFIYAT = urun.URUNALISFIYAT;
                urn.URUNSATISFIYAT = urun.URUNSATISFIYAT;

                urn.URUNMARKA = urun.URUNMARKA;


                db.SaveChanges();

                ViewBag.Message = "Urun başarıyla guncellendi!";

                kategoriler();

                return View();



            }
            else
            {

                kategoriler();

                return View();

            }

        }

        public ActionResult UrunSil(int id)
        {

            var urun = db.TBL_URUN.Find(id);

            urun.URUNDURUM = false;

            db.SaveChanges();

            return RedirectToAction("Index", "Urun");

        }

        [Authorize(Roles="Personel")]
        public ActionResult SilinmisUrunler(string ad, int sayfa=1 )
        {


            int subeId = Convert.ToInt32(Session["PersonelSube"].ToString());

            var urunler = db.TBL_URUN.Where(m => m.TBL_SUBE.SUBEID == subeId && m.URUNDURUM == false).ToList();


            if (!String.IsNullOrEmpty(ad))
            {

                urunler = urunler.Where(m => m.URUNAD.Contains(ad)).ToList();

            }

            return View(urunler.ToPagedList(sayfa, 10));

        }


        public ActionResult UrunuGeriGetir(int id)
        {

            var urun = db.TBL_URUN.Find(id);

            urun.URUNDURUM = true;

            db.SaveChanges();

            return RedirectToAction("Index", "Urun");

        }


        protected void kategoriler()
        {

            int subeId = Convert.ToInt32(Session["PersonelSube"].ToString());

            List<SelectListItem> kategoriler = (from i in db.TBL_KATEGORI.Where(m => m.TBL_SUBE.SUBEID == subeId && m.KATEGORIDURUM == true).ToList()
                                                select new SelectListItem
                                                {

                                                    Text = i.KATEGORIAD,
                                                    Value = i.KATEGORIID.ToString()

                                                }).ToList();


            ViewBag.kt = kategoriler;

        }
       


    }
}