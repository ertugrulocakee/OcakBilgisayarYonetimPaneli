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
    public class KategoriController : Controller
    {
        // GET: Kategori

        TeknolojikMarketEntities db = new TeknolojikMarketEntities();

        [Authorize(Roles = "Personel")]
        public ActionResult Index(string ad, int sayfa=1)
        {

            int personelSube =  Convert.ToInt32(Session["PersonelSube"].ToString());

            var kategoriler = db.TBL_KATEGORI.Where(m => m.KATEGORIDURUM == true).Where(m => m.TBL_SUBE.SUBEID == personelSube).ToList();

            if (!string.IsNullOrEmpty(ad))
            {

              kategoriler = kategoriler.Where(x => x.KATEGORIAD.Contains(ad)).ToList();

            }

            return View(kategoriler.ToPagedList(sayfa, 5));
        }

        [Authorize(Roles="Personel")]
        [HttpGet]
        public ActionResult YeniKategori()
        {



            return View();

        }

        [HttpPost]
        public ActionResult YeniKategori(TBL_KATEGORI kategori)
        {

            if (String.IsNullOrEmpty(kategori.KATEGORIAD))
            {

                ViewBag.Message = "Kategori Adı boş bırakılamaz!";

                return View();

            }

            int personelSube = Convert.ToInt32(Session["PersonelSube"].ToString());

       

            if (ModelState.IsValid)
            {

                Char[] kategoriler = kategori.KATEGORIAD.ToCharArray();

                foreach (char x in kategoriler)
                {

                    if (!Char.IsLetter(x) && x != ' ')
                    {

                        ViewBag.Message = "Kategori adı sadece harflerden oluşmalıdır!";

                        return View();
                    }

                }


                var kategoriKontrol = db.TBL_KATEGORI.Where(m => m.KATEGORIAD == kategori.KATEGORIAD && m.KATEGORIDURUM == true).Where(m => m.TBL_SUBE.SUBEID == personelSube);

                var eskiKategoriKontrol = db.TBL_KATEGORI.Where(m => m.KATEGORIAD == kategori.KATEGORIAD && m.KATEGORIDURUM == false).Where(m => m.TBL_SUBE.SUBEID == personelSube);

                if (kategoriKontrol.Any())
                {

                    ViewBag.Message = "Şubede boyle bir kategori zaten var!";

                    return View();

                }

                if (eskiKategoriKontrol.Any())
                {

                    ViewBag.Message = "Şubede boyle bir kategori zaten varmış ama silinmiş. Silinmiş Kategoriler sayfasından bu kategoriyi geri getir!";

                    return View();

                }


                var sube = db.TBL_SUBE.Where(m => m.SUBEID == personelSube).FirstOrDefault();
                kategori.TBL_SUBE = sube;

                kategori.KATEGORIDURUM = true;

                db.TBL_KATEGORI.Add(kategori);

                db.SaveChanges();

                ViewBag.Message = "Kategori başarıyla eklendi!";


                return View();


            }
            else
            {

                return View();

            }
         
        }

        public ActionResult KategoriSil(int id)
        {

            var kategori = db.TBL_KATEGORI.Find(id);

            kategori.KATEGORIDURUM = false;

            db.SaveChanges();

            return RedirectToAction("Index","Kategori");


        }



        [Authorize(Roles = "Personel")]
        [HttpGet]
        public ActionResult KategoriGetir(int id)
        {

            var kategori = db.TBL_KATEGORI.Find(id);



            return View(kategori);
        }

        [HttpPost]
        public ActionResult KategoriGetir(TBL_KATEGORI kategori)
        {

            if (String.IsNullOrEmpty(kategori.KATEGORIAD))
            {

                ViewBag.Message = "Kategori Adı boş bırakılamaz!";

                return View();

            }

            int personelSube = Convert.ToInt32(Session["PersonelSube"].ToString());

       
            if (ModelState.IsValid)
            {

                Char[] kategoriler = kategori.KATEGORIAD.ToCharArray();

                foreach (char x in kategoriler)
                {

                    if (!Char.IsLetter(x) && x != ' ')
                    {

                        ViewBag.Message = "Kategori adı sadece harflerden oluşmalıdır!";

                        return View();
                    }

                }


                var kategoriKontrol = db.TBL_KATEGORI.Where(m => m.KATEGORIAD == kategori.KATEGORIAD && m.KATEGORIDURUM == true).Where(m => m.TBL_SUBE.SUBEID == personelSube);

                var eskiKategoriKontrol = db.TBL_KATEGORI.Where(m => m.KATEGORIAD == kategori.KATEGORIAD && m.KATEGORIDURUM == false).Where(m => m.TBL_SUBE.SUBEID == personelSube);


                if (kategoriKontrol.Any())
                {

                    ViewBag.Message = "Şubede boyle bir kategori zaten var!";

                    return View();

                }

                if (eskiKategoriKontrol.Any())
                {

                    ViewBag.Message = "Şubede boyle bir kategori zaten varmış ama silinmiş. Silinmiş Kategoriler sayfasından bu kategoriyi geri getir!";

                    return View();

                }


                var ktgr = db.TBL_KATEGORI.Find(kategori.KATEGORIID);

                ktgr.KATEGORIAD = kategori.KATEGORIAD;

                db.SaveChanges();

                ViewBag.Message = "Kategori başarıyla guncellendi!";


                return View();

            }
            else
            {

                return View();

            }

        }

        [Authorize(Roles = "Personel")]
        public ActionResult SilinmisKategoriler(string ad, int sayfa = 1)
        {

            int personelSube = Convert.ToInt32(Session["PersonelSube"].ToString());

            var kategoriler = db.TBL_KATEGORI.Where(m => m.KATEGORIDURUM == false).Where(m => m.TBL_SUBE.SUBEID == personelSube).ToList();

            if (!string.IsNullOrEmpty(ad))
            {

                kategoriler = kategoriler.Where(x => x.KATEGORIAD.Contains(ad)).ToList();

            }

            return View(kategoriler.ToPagedList(sayfa, 5));


        }


        public ActionResult KategoriGeriGetir(int id)
        {


            var kategori = db.TBL_KATEGORI.Find(id);

            kategori.KATEGORIDURUM = true;

            db.SaveChanges();

            return RedirectToAction("Index", "Kategori");

        }



       
    }
}