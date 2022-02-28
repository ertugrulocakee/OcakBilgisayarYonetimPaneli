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
    public class BolumController : Controller
    {
        // GET: Bolum

        TeknolojikMarketEntities db = new TeknolojikMarketEntities();

        [Authorize(Roles = "Admin")]
        public ActionResult Index(string ad, int sayfa = 1)
        {

            var bolumler = db.TBL_BOLUM.Where(m => m.BOLUMDURUM == true).ToList();

            if (!String.IsNullOrEmpty(ad))
            {


                bolumler = bolumler.Where(m => m.BOLUMAD.Contains(ad)).ToList();

            }

            return View(bolumler.ToPagedList(sayfa,5));
        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        public ActionResult BolumEkle()
        {

            return View();
        }


        [HttpPost]
        public ActionResult BolumEkle(TBL_BOLUM bolum)
        {

            if (String.IsNullOrEmpty(bolum.BOLUMAD))
            {

                ViewBag.Message = "Bolum adı boş olamaz!";


                return View();
            }


            if (ModelState.IsValid)
            {


                Char[] bolumAd = bolum.BOLUMAD.ToCharArray();

                foreach (char x in bolumAd)
                {

                    if (!Char.IsLetter(x) && x != ' ')
                    {

                        ViewBag.Message = "Bolum adı sadece harflerden oluşmalıdır!";

                        return View();
                    }

                }


                var blm = db.TBL_BOLUM.Where(m => m.BOLUMAD == bolum.BOLUMAD);

                if (blm.Any())
                {

                    ViewBag.Message = "Boyle bir bolum zaten daha once olusturulmus!";

                    return View();

                }

                bolum.BOLUMDURUM = true;

                db.TBL_BOLUM.Add(bolum);

                db.SaveChanges();

                ViewBag.Message = "Bolum basariyla eklendi!";

                return View();

            }
            else
            {

               return View();

            }

      
        }


        public ActionResult BolumSil(int id){


            var bolum = db.TBL_BOLUM.Find(id);

            bolum.BOLUMDURUM = false;
            
            db.SaveChanges();

            return RedirectToAction("Index","Bolum");

        }

        public ActionResult BolumuGeriGetir(int id)
        {

            var bolum = db.TBL_BOLUM.Find(id);

            bolum.BOLUMDURUM = true;

            db.SaveChanges();

            return RedirectToAction("Index", "Bolum");

        }


        [Authorize(Roles = "Admin")]
        [HttpGet]
        public ActionResult BolumGuncelle(int id)
        {

            var bolum = db.TBL_BOLUM.Find(id);


            return View(bolum);

        }


        [HttpPost]
        public ActionResult BolumGuncelle(TBL_BOLUM bolum)
        {

            if (String.IsNullOrEmpty(bolum.BOLUMAD))
            {

                ViewBag.Message = "Bolum adı boş olamaz!";


                return View();
            }


            if (ModelState.IsValid)
            {

                Char[] bolumAd = bolum.BOLUMAD.ToCharArray();

                foreach (char x in bolumAd)
                {

                    if (!Char.IsLetter(x) && x != ' ')
                    {

                        ViewBag.Message = "Bolum adı sadece harflerden oluşmalıdır!";

                        return View();
                    }

                }


                var blm = db.TBL_BOLUM.Where(m => m.BOLUMID != bolum.BOLUMID).Where(m => m.BOLUMAD == bolum.BOLUMAD);

                if (blm.Any())
                {

                    ViewBag.Message = "Boyle bir bolum zaten daha once olusturulmus!";

                    return View();

                }


                var bolm = db.TBL_BOLUM.Find(bolum.BOLUMID);

                bolm.BOLUMAD = bolum.BOLUMAD;

                db.SaveChanges();

                ViewBag.Message = "Bolum basariyla guncellendi!";

                return View();

            }
            else
            {

                return View();

            }

      
        }

        [Authorize(Roles = "Admin")]
        public ActionResult SilinmisBolumler(string ad, int sayfa = 1)
        {

            var bolumler = db.TBL_BOLUM.Where(m => m.BOLUMDURUM == false).ToList();

            if (!String.IsNullOrEmpty(ad))
            {


                bolumler = bolumler.Where(m => m.BOLUMAD.Contains(ad)).ToList();

            }

            return View(bolumler.ToPagedList(sayfa,5));


        }



    }
}