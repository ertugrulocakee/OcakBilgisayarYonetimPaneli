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
    public class SubeController : Controller
    {
        // GET: Sube

        TeknolojikMarketEntities db = new TeknolojikMarketEntities();

        [Authorize(Roles="Admin")]
        public ActionResult Index(string ad, int sayfa=1)
        {

            var sube = db.TBL_SUBE.Where(m => m.SUBEDURUM == true).ToList();


            string str = Request.Params["btnsubetarih"];

            if (!String.IsNullOrEmpty(str))
            {


                if (str == "a-z")
                {

                    sube = sube.OrderBy(m => m.SUBEKURULUSTARIHI).ToList();


                }
                else if (str == "z-a")
                {

                    sube = sube.OrderByDescending(m => m.SUBEKURULUSTARIHI).ToList();

                }

            }


            if (!String.IsNullOrEmpty(ad))
            {


                sube = sube.Where(m => m.SUBEAD.Contains(ad)).ToList();


            }


            return View(sube.ToPagedList(sayfa, 10));
        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        public ActionResult SubeEkle()
        {

            Sehirler();

            return View();


        }


        [HttpPost]
        public ActionResult SubeEkle(TBL_SUBE sube)
        {

            try
            {
                if (sube.SUBEAD == "" || sube.SUBETELEFON == "" || sube.SUBEKURULUSTARIHI.ToString() == "")
                {

                    ViewBag.Message = "Bos deger girmeyin!";

                    Sehirler();

                    return View();

                }

            }
            catch (Exception e)
            {

                Console.WriteLine(e);

                ViewBag.Message = "Bos deger girmeyin!";

                Sehirler();

                return View();

            }
            
            if (ModelState.IsValid)
            {

                Char[] subeAd = sube.SUBEAD.ToCharArray();

                foreach (char x in subeAd)
                {

                    if (!Char.IsLetter(x) && x != ' ')
                    {

                        ViewBag.Message = "Sube adı sadece harflerden oluşmalıdır!";

                        Sehirler();

                        return View();
                    }

                }


                sube.SUBEDURUM = true;

                db.TBL_SUBE.Add(sube);

                db.SaveChanges();

                ViewBag.Message = "Sube basariyla olusturuldu!";

                Sehirler();

                return View();


            }
            else
            {

                Sehirler();

                return View();


            }


        }


        public ActionResult SubeSil(int id)
        {

            var sube = db.TBL_SUBE.Find(id);

            sube.SUBEDURUM = false;

            sube.SUBEBITISTARIHI = DateTime.Now;

            db.SaveChanges();

            return RedirectToAction("Index", "Sube");

        }

        public ActionResult SubeGeriGetir(int id)
        {

            var sube = db.TBL_SUBE.Find(id);

            sube.SUBEDURUM = true;

            sube.SUBEBITISTARIHI = null;

            db.SaveChanges();

            return RedirectToAction("Index", "Sube");

        }


        [Authorize(Roles="Admin")]
        [HttpGet]
        public ActionResult SubeGuncelle(int id)
        {

            var sube = db.TBL_SUBE.Find(id);


            Sehirler();


            return View(sube);


        }


        [HttpPost]
        public ActionResult SubeGuncelle(TBL_SUBE sube)
        {

            try
            {
                if (sube.SUBEAD == "" || sube.SUBETELEFON == "" || sube.SUBEKURULUSTARIHI.ToString() == "")
                {

                    ViewBag.Message = "Bos deger girmeyin!";

                    Sehirler();

                    return View();

                }

            }
            catch (Exception e)
            {

                Console.WriteLine(e);

                ViewBag.Message = "Bos deger girmeyin!";

                Sehirler();

                return View();

            }

            if (ModelState.IsValid)
            {

                Char[] subeAd = sube.SUBEAD.ToCharArray();

                foreach (char x in subeAd)
                {

                    if (!Char.IsLetter(x) && x != ' ')
                    {

                        ViewBag.Message = "Sube adı sadece harflerden oluşmalıdır!";

                        Sehirler();

                        return View();
                    }

                }


                var Sube = db.TBL_SUBE.Find(sube.SUBEID);

                Sube.SUBEAD = sube.SUBEAD;

                Sube.SUBESEHIR = sube.SUBESEHIR;

                Sube.SUBETELEFON = sube.SUBETELEFON;

                Sube.SUBEKURULUSTARIHI = sube.SUBEKURULUSTARIHI;

                db.SaveChanges();

                ViewBag.Message = "Sube basariyla guncellendi!";

                Sehirler();

                return View();


            }
            else
            {

                Sehirler();

                return View();


            }



        }

        
        [Authorize(Roles="Admin")]
        public ActionResult SilinmisSubeler(string ad, int sayfa = 1)
        {


            var sube = db.TBL_SUBE.Where(m => m.SUBEDURUM == false).ToList();


            string str = Request.Params["btnsubetarih"];

            if (!String.IsNullOrEmpty(str))
            {


                if (str == "a-z")
                {

                    sube = sube.OrderBy(m => m.SUBEBITISTARIHI).ToList();


                }
                else if (str == "z-a")
                {

                    sube = sube.OrderByDescending(m => m.SUBEBITISTARIHI).ToList();

                }

            }


            if (!String.IsNullOrEmpty(ad))
            {


                sube = sube.Where(m => m.SUBEAD.Contains(ad)).ToList();


            }


            return View(sube.ToPagedList(sayfa, 10));


        }



        protected void Sehirler()
        {

            List<SelectListItem> sehirler = new List<SelectListItem>();

            sehirler.Add(new SelectListItem { Text = "Ordu", Value = "Ordu", Selected=true});
            sehirler.Add(new SelectListItem { Text = "İstanbul", Value = "İstanbul"});
            sehirler.Add(new SelectListItem { Text = "Ankara", Value = "Ankara"});
            sehirler.Add(new SelectListItem { Text = "İzmir", Value = "İzmir" });
            sehirler.Add(new SelectListItem { Text = "Adana", Value = "Adana" });
            sehirler.Add(new SelectListItem { Text = "Adıyaman", Value = "Adıyaman" });
            sehirler.Add(new SelectListItem { Text = "Afyonkarahisar", Value = "Afyonkarahisar" });
            sehirler.Add(new SelectListItem { Text = "Ağrı", Value = "Ağrı" });
            sehirler.Add(new SelectListItem { Text = "Amasya", Value = "Amasya" });
            sehirler.Add(new SelectListItem { Text = "Antalya", Value = "Antalya" });
            sehirler.Add(new SelectListItem { Text = "Artvin", Value = "Artvin" });
            sehirler.Add(new SelectListItem { Text = "Aydın", Value = "Aydın" });
            sehirler.Add(new SelectListItem { Text = "Balıkesir", Value = "Balıkesir" });
            sehirler.Add(new SelectListItem { Text = "Bilecik", Value = "Bilecik" });
            sehirler.Add(new SelectListItem { Text = "Bingöl", Value = "Bingöl" });
            sehirler.Add(new SelectListItem { Text = "Bitlis", Value = "Bitlis" });
            sehirler.Add(new SelectListItem { Text = "Bolu", Value = "Bolu" });
            sehirler.Add(new SelectListItem { Text = "Burdur", Value = "Burdur" });
            sehirler.Add(new SelectListItem { Text = "Bursa", Value = "Bursa" });
            sehirler.Add(new SelectListItem { Text = "Çanakkale", Value = "Çanakkale" });
            sehirler.Add(new SelectListItem { Text = "Çankırı", Value = "Çankırı" });
            sehirler.Add(new SelectListItem { Text = "Çorum", Value = "Çorum" });
            sehirler.Add(new SelectListItem { Text = "Denizli", Value = "Denizli" });
            sehirler.Add(new SelectListItem { Text = "Diyarbakır", Value = "Diyarbakır" });
            sehirler.Add(new SelectListItem { Text = "Edirne", Value = "Edirne" });
            sehirler.Add(new SelectListItem { Text = "Elazığ", Value = "Elazığ" });
            sehirler.Add(new SelectListItem { Text = "Erzincan", Value = "Erzincan" });
            sehirler.Add(new SelectListItem { Text = "Erzurum", Value = "Erzurum" });
            sehirler.Add(new SelectListItem { Text = "Eskişehir", Value = "Eskişehir" });
            sehirler.Add(new SelectListItem { Text = "Gaziantep", Value = "Gaziantep" });
            sehirler.Add(new SelectListItem { Text = "Giresun", Value = "Giresun" });
            sehirler.Add(new SelectListItem { Text = "Gümüşhane", Value = "Gümüşhane" });
            sehirler.Add(new SelectListItem { Text = "Hakkâri", Value = "Hakkâri" });
            sehirler.Add(new SelectListItem { Text = "Hatay", Value = "Hatay" });
            sehirler.Add(new SelectListItem { Text = "Isparta", Value = "Isparta" });
            sehirler.Add(new SelectListItem { Text = "Mersin", Value = "Mersin" });
            sehirler.Add(new SelectListItem { Text = "Kars", Value = "Kars" });
            sehirler.Add(new SelectListItem { Text = "Kastamonu", Value = "Kastamonu" });
            sehirler.Add(new SelectListItem { Text = "Kayseri", Value = "Kayseri" });
            sehirler.Add(new SelectListItem { Text = "Kırklareli", Value = "Kırklareli" });
            sehirler.Add(new SelectListItem { Text = "Kırşehir", Value = "Kırşehir" });
            sehirler.Add(new SelectListItem { Text = "Kocaeli", Value = "Kocaeli" });
            sehirler.Add(new SelectListItem { Text = "Konya", Value = "Konya" });
            sehirler.Add(new SelectListItem { Text = "Kütahya", Value = "Kütahya" });
            sehirler.Add(new SelectListItem { Text = "Malatya", Value = "Malatya" });
            sehirler.Add(new SelectListItem { Text = "Manisa", Value = "Manisa" });
            sehirler.Add(new SelectListItem { Text = "Kahramanmaraş", Value = "Kahramanmaraş" });
            sehirler.Add(new SelectListItem { Text = "Mardin", Value = "Mardin" });
            sehirler.Add(new SelectListItem { Text = "Muğla", Value = "Muğla" });
            sehirler.Add(new SelectListItem { Text = "Muş", Value = "Muş" });
            sehirler.Add(new SelectListItem { Text = "Nevşehir", Value = "Nevşehir" });
            sehirler.Add(new SelectListItem { Text = "Niğde", Value = "Niğde" });
            sehirler.Add(new SelectListItem { Text = "Rize", Value = "Rize" });
            sehirler.Add(new SelectListItem { Text = "Sakarya", Value = "Sakarya" });
            sehirler.Add(new SelectListItem { Text = "Samsun", Value = "Samsun" });
            sehirler.Add(new SelectListItem { Text = "Siirt", Value = "Siirt" });
            sehirler.Add(new SelectListItem { Text = "Sinop", Value = "Sinop" });
            sehirler.Add(new SelectListItem { Text = "Sivas", Value = "Sivas" });
            sehirler.Add(new SelectListItem { Text = "Tekirdağ", Value = "Tekirdağ" });
            sehirler.Add(new SelectListItem { Text = "Tokat", Value = "Tokat" });
            sehirler.Add(new SelectListItem { Text = "Trabzon", Value = "Trabzon" });
            sehirler.Add(new SelectListItem { Text = "Tunceli", Value = "Tunceli" });
            sehirler.Add(new SelectListItem { Text = "Şanlıurfa", Value = "Şanlıurfa" });
            sehirler.Add(new SelectListItem { Text = "Uşak", Value = "Uşak" });
            sehirler.Add(new SelectListItem { Text = "Van", Value = "Van" });
            sehirler.Add(new SelectListItem { Text = "Yozgat", Value = "Yozgat" });
            sehirler.Add(new SelectListItem { Text = "Zonguldak", Value = "Zonguldak" });
            sehirler.Add(new SelectListItem { Text = "Aksaray", Value = "Aksaray" });
            sehirler.Add(new SelectListItem { Text = "Bayburt", Value = "Bayburt" });
            sehirler.Add(new SelectListItem { Text = "Karaman", Value = "Karaman" });
            sehirler.Add(new SelectListItem { Text = "Kırıkkale", Value = "Kırıkkale" });
            sehirler.Add(new SelectListItem { Text = "Batman", Value = "Batman" });
            sehirler.Add(new SelectListItem { Text = "Şırnak", Value = "Şırnak" });
            sehirler.Add(new SelectListItem { Text = "Bartın", Value = "Bartın" });
            sehirler.Add(new SelectListItem { Text = "Ardahan", Value = "Ardahan" });
            sehirler.Add(new SelectListItem { Text = "Iğdır", Value = "Iğdır" });
            sehirler.Add(new SelectListItem { Text = "Yalova", Value = "Yalova" });
            sehirler.Add(new SelectListItem { Text = "Karabük", Value = "Karabük" });
            sehirler.Add(new SelectListItem { Text = "Kilis", Value = "Kilis" });
            sehirler.Add(new SelectListItem { Text = "Osmaniye", Value = "Osmaniye" });
            sehirler.Add(new SelectListItem { Text = "Düzce", Value = "Düzce" });

            ViewBag.shr = sehirler;


        }


    }
}