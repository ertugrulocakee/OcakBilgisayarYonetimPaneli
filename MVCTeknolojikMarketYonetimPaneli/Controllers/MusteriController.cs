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

    public class MusteriController : Controller
    {
        // GET: Musteri

        TeknolojikMarketEntities db = new TeknolojikMarketEntities();

        [Authorize(Roles="Personel")]
        public ActionResult Index(string ad,string soyad,int sayfa=1)
        {

            int subeID = Convert.ToInt32(Session["PersonelSube"].ToString());

            var musteriler = db.TBL_MUSTERI.Where(x => x.MUSTERIDURUM == true && x.MUSTERISUBE == subeID).ToList();

            if(!string.IsNullOrEmpty(ad)){

                musteriler = musteriler.Where(x => x.MUSTERIAD.Contains(ad)).ToList();

            }

            if (!string.IsNullOrEmpty(soyad))
            {

                musteriler = musteriler.Where(x => x.MUSTERISOYAD.Contains(soyad)).ToList();

            }


            if (!string.IsNullOrEmpty(soyad) && !string.IsNullOrEmpty(ad))
            {

                musteriler = musteriler.Where(x => x.MUSTERISOYAD.Contains(soyad) && x.MUSTERIAD.Contains(ad)).ToList();

            }



            return View(musteriler.ToPagedList(sayfa, 10));

        }

        [Authorize(Roles = "Personel")]
        [HttpGet]
        public ActionResult YeniMusteri()
        {

            SubelerVeCinsiyetler();
  
            return View();

        }

        [HttpPost]
        public ActionResult YeniMusteri(TBL_MUSTERI musteri)
        {

            try
            {
                var sube = db.TBL_SUBE.Where(m => m.SUBEID == musteri.TBL_SUBE.SUBEID).FirstOrDefault();
                musteri.TBL_SUBE = sube;
            }
            catch
            {

                ViewBag.Message = "Lutfen once bir şube ekleyiniz!";

                SubelerVeCinsiyetler();

                return View();

            }
          
            if (ModelState.IsValid)
            {

                char[] TC = musteri.MUSTERITC.ToCharArray();
                char[] Ad = musteri.MUSTERIAD.ToCharArray();
                char[] Soyad = musteri.MUSTERISOYAD.ToCharArray();

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


                var musteriKontrol = db.TBL_MUSTERI.Where(m => m.MUSTERITC == musteri.MUSTERITC || m.MUSTERITELEFON == musteri.MUSTERITELEFON || m.MUSTERIMAIL == musteri.MUSTERIMAIL);

                if (musteriKontrol.Any()) // böyle bir müşteri varsa 
                {

                    ViewBag.Message = "Aynı TC Numarası veya Telefon Numarası veya Mail Adresine sahip musteri olamaz!";

                    SubelerVeCinsiyetler();

                    return View();

                }


                    musteri.MUSTERIDURUM = true;

                    db.TBL_MUSTERI.Add(musteri);

                    db.SaveChanges();

                    ViewBag.Message = "Muşteri başarıyla eklendi/kaydoldu.";

                    SubelerVeCinsiyetler();
                  
                    return View();
               
            }
            else
            {

                SubelerVeCinsiyetler();


                return View();
            }
           


        }


        public ActionResult MusteriSil(int id)
        {

            var musteri = db.TBL_MUSTERI.Find(id);

            musteri.MUSTERIDURUM = false;

            db.SaveChanges();

            return RedirectToAction("Index", "Musteri");

        }

        [Authorize(Roles = "Personel")]
        [HttpGet]
        public ActionResult MusteriGetir(int id)
        {

            var musteri = db.TBL_MUSTERI.Find(id);

            SubelerVeCinsiyetler();
           
            return View(musteri);


        }

        [HttpPost]
        public ActionResult MusteriGetir(TBL_MUSTERI mstr)
        {

            try
            {
                var sube = db.TBL_SUBE.Where(m => m.SUBEID == mstr.TBL_SUBE.SUBEID).FirstOrDefault();
                mstr.TBL_SUBE = sube;

            }
            catch
            {

                ViewBag.Message = "Guncelleme işlemi yapmadan once lutfen sube eklemesi yapın!";

                SubelerVeCinsiyetler();

                return View();

            }

    
            if (ModelState.IsValid)
            {

                char[] TC = mstr.MUSTERITC.ToCharArray();
                char[] Ad = mstr.MUSTERIAD.ToCharArray();
                char[] Soyad = mstr.MUSTERISOYAD.ToCharArray();

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


                var musteriKontrol = db.TBL_MUSTERI.Where(x => x.MUSTERIID != mstr.MUSTERIID).Where(m => m.MUSTERITC == mstr.MUSTERITC || m.MUSTERITELEFON == mstr.MUSTERITELEFON || m.MUSTERIMAIL == mstr.MUSTERIMAIL);

                if (musteriKontrol.Any()) // böyle bir müşteri varsa 
                {

                    ViewBag.Message = "Aynı TC Numarası veya Telefon Numarası veya Mail Adresine sahip musteri olamaz!";

                    SubelerVeCinsiyetler();

                    return View();

                }


                var musteri = db.TBL_MUSTERI.Find(mstr.MUSTERIID);

                musteri.MUSTERIAD = mstr.MUSTERIAD;
                musteri.MUSTERISOYAD = mstr.MUSTERISOYAD;
                musteri.MUSTERITELEFON = mstr.MUSTERITELEFON;
                musteri.MUSTERIMAIL = mstr.MUSTERIMAIL;
                musteri.MUSTERITC = mstr.MUSTERITC;
                musteri.MUSTERICINSIYET = mstr.MUSTERICINSIYET;
                musteri.MUSTERIYAS = mstr.MUSTERIYAS;
                musteri.TBL_SUBE = mstr.TBL_SUBE;

                db.SaveChanges();

                ViewBag.Message = "Muşteri başarıyla guncellendi.";

                SubelerVeCinsiyetler();

                return View();

            }
            else
            {

                SubelerVeCinsiyetler();

                return View();

            }


        }

        [Authorize(Roles = "Personel")]
        public ActionResult SilinmisMusteriler(string ad, string soyad, int sayfa = 1)
        {

              int subeID = Convert.ToInt32(Session["PersonelSube"].ToString());

            var musteriler = db.TBL_MUSTERI.Where(x => x.MUSTERIDURUM == false && x.MUSTERISUBE == subeID).ToList();


            if (!string.IsNullOrEmpty(ad))
            {

                musteriler = musteriler.Where(x => x.MUSTERIAD.Contains(ad)).ToList();

            }

            if (!string.IsNullOrEmpty(soyad))
            {

                musteriler = musteriler.Where(x => x.MUSTERISOYAD.Contains(soyad)).ToList();

            }


            if (!string.IsNullOrEmpty(soyad) && !string.IsNullOrEmpty(ad))
            {

                musteriler = musteriler.Where(x => x.MUSTERISOYAD.Contains(soyad) && x.MUSTERIAD.Contains(ad)).ToList();

            }


            return View(musteriler.ToPagedList(sayfa, 10));


        }

        public ActionResult MusteriyiGeriGetir(int id)
        {

            var musteri = db.TBL_MUSTERI.Find(id);

            musteri.MUSTERIDURUM = true;

            db.SaveChanges();

            return RedirectToAction("Index", "Musteri");


        }


        protected void SubelerVeCinsiyetler()
        {

            List<SelectListItem> subeler = (from i in db.TBL_SUBE.Where(m=>m.SUBEDURUM == true).ToList()
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


    

    }



}