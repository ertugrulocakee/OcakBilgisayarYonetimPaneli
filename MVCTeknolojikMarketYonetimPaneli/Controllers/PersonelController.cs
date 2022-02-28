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
    public class PersonelController : Controller
    {
        // GET: Personel

        TeknolojikMarketEntities db = new TeknolojikMarketEntities();

        [Authorize(Roles="Yönetici")]
        public ActionResult Index(string ad ,string soyad, int sayfa = 1)
        {

             int subeID = Convert.ToInt32(Session["YoneticiSube"].ToString());

            var personeller = db.TBL_PERSONEL.Where(x => x.PERSONELDURUM == true && x.PERSONELSUBE == subeID).ToList();

            if (!string.IsNullOrEmpty(ad))
            {

                personeller = personeller.Where(x => x.PERSONELAD.Contains(ad)).ToList();

            }

            if (!string.IsNullOrEmpty(soyad))
            {

                personeller = personeller.Where(x => x.PERSONELSOYAD.Contains(soyad)).ToList();

            }


            if (!string.IsNullOrEmpty(soyad) && !string.IsNullOrEmpty(ad))
            {

                personeller = personeller.Where(x => x.PERSONELSOYAD.Contains(soyad) && x.PERSONELAD.Contains(ad)).ToList();

            }



            return View(personeller.ToPagedList(sayfa, 10));

          
        }


        [Authorize(Roles="Yönetici")]
        [HttpGet]
        public ActionResult YeniPersonel()
        {

            bolumlerVeCinsiyetler();


            return View();
        }


        protected void bolumlerVeCinsiyetler()
        {

            List<SelectListItem> bolumler = (from i in db.TBL_BOLUM.Where(m => m.BOLUMDURUM == true).ToList()
                                            select new SelectListItem
                                            {

                                                Text = i.BOLUMAD,
                                                Value = i.BOLUMID.ToString()

                                            }).ToList();

            ViewBag.blm = bolumler;


            List<SelectListItem> cinsiyetler = new List<SelectListItem>();

            cinsiyetler.Add(new SelectListItem { Text = "Erkek", Value = "Erkek" });

            cinsiyetler.Add(new SelectListItem { Text = "Kadın", Value = "Kadın", Selected = true });



            ViewBag.cns = cinsiyetler;


        }


        [HttpPost]
        public ActionResult YeniPersonel(TBL_PERSONEL perso)
        {

            if (String.IsNullOrEmpty(perso.PERSONELAD) || String.IsNullOrEmpty(perso.PERSONELSOYAD) || String.IsNullOrEmpty(perso.PERSONELMAIL) || String.IsNullOrEmpty(perso.PERSONELTELEFON) || String.IsNullOrEmpty(perso.PERSONELTC) || perso.PERSONELMAAS == null || perso.PERSONELYAS == null || String.IsNullOrEmpty(perso.KULLANICIADI) || String.IsNullOrEmpty(perso.SIFRE))
            {

                ViewBag.Message = "Boş değer girmeyin!";

                bolumlerVeCinsiyetler();

                return View();

            }

            int subeID = Convert.ToInt32(Session["YoneticiSube"].ToString());


            try
            {
                var sube = db.TBL_SUBE.Where(m => m.SUBEID == subeID).FirstOrDefault();
                perso.TBL_SUBE = sube;
            }
            catch
            {

                ViewBag.Message = "Once sube ekleyiniz!";

                bolumlerVeCinsiyetler();

                return View();

            }


            try
            {

                var bolum = db.TBL_BOLUM.Where(m => m.BOLUMID == perso.TBL_BOLUM.BOLUMID).FirstOrDefault();

                perso.TBL_BOLUM = bolum;

            }
            catch
            {

                ViewBag.Message = "Once bolum ekleyiniz!";

                bolumlerVeCinsiyetler();

                return View();

            }



    
            if (ModelState.IsValid)
            {

                char[] TC = perso.PERSONELTC.ToCharArray();
                char[] Ad = perso.PERSONELAD.ToCharArray();
                char[] Soyad = perso.PERSONELSOYAD.ToCharArray();

                foreach (char x in TC)
                {

                    if (!char.IsDigit(x))
                    {

                        ViewBag.Message = "TC Kimlik Numarası rakamlardan oluşmalıdır!";
                        bolumlerVeCinsiyetler();
                        return View();
                    }

                }

                foreach (char x in Ad)
                {

                    if (!char.IsLetter(x))
                    {

                        ViewBag.Message = "Ad harflerden oluşmalıdır!";
                        bolumlerVeCinsiyetler();
                        return View();



                    }

                }

                foreach (char x in Soyad)
                {

                    if (!char.IsLetter(x))
                    {

                        ViewBag.Message = "Soyad harflerden oluşmalıdır!";
                        bolumlerVeCinsiyetler();
                        return View();

                    }

                }


                char[] kullaniciAdi = perso.KULLANICIADI.ToCharArray();
                char[] kullaniciSifre = perso.SIFRE.ToCharArray();

                foreach (char x in kullaniciAdi)
                {

                    if (!Char.IsLetterOrDigit(x))
                    {

                        ViewBag.Message = "Kullanıcı adı sadece harflerden ve rakamlardan oluşmalıdır!";

                        bolumlerVeCinsiyetler();

                        return View();
                    }

                }

                foreach (char x in kullaniciSifre)
                {

                    if (!Char.IsDigit(x))
                    {

                        ViewBag.Message = "Şifre sadece rakamlardan oluşmalıdır!";

                        bolumlerVeCinsiyetler();

                        return View();


                    }


                }

                var personelKontrol = db.TBL_PERSONEL.Where(m => m.PERSONELTC == perso.PERSONELTC || m.PERSONELTELEFON == perso.PERSONELTELEFON || m.PERSONELMAIL == perso.PERSONELMAIL);

                var musteriKontrol = db.TBL_MUSTERI.Where(m => m.MUSTERITC == perso.PERSONELTC || m.MUSTERITELEFON == perso.PERSONELTELEFON || m.MUSTERIMAIL == perso.PERSONELMAIL);

                var yoneticiKontrol = db.TBL_YONETICI.Where(m => m.YONETICITC == perso.PERSONELTC || m.YONETICITELEFON == perso.PERSONELTELEFON || m.YONETICIMAIL == perso.PERSONELMAIL);

                if (personelKontrol.Any())
                {

                    ViewBag.Message = "Bir personel ile aynı TC veya Telefona veya maile sahip bir personel olusturulamaz!";

                    bolumlerVeCinsiyetler();

                    return View();


                }

                if (musteriKontrol.Any())
                {

                    ViewBag.Message = "Bir musteri ile aynı TC veya Telefona veya maile sahip bir personel olusturulamaz!";

                    bolumlerVeCinsiyetler();

                    return View();


                }


                if (yoneticiKontrol.Any())
                {

                    ViewBag.Message = "Bir yonetici ile aynı TC veya Telefona veya maile sahip bir personel olusturulamaz!";

                    bolumlerVeCinsiyetler();

                    return View();


                }


                var prsl = db.TBL_PERSONEL.Where(m => m.KULLANICIADI == perso.KULLANICIADI || m.SIFRE == perso.SIFRE);
                var yonetici = db.TBL_YONETICI.Where(m => m.KULLANICIADI == perso.KULLANICIADI || m.SIFRE == perso.SIFRE);
                var admin = db.TBL_ADMIN.Where(m => m.KULLANICIADI == perso.KULLANICIADI || m.SIFRE == perso.SIFRE);


                if (prsl.Any())
                {

                    ViewBag.Message = "Boyle bir kullanıcı adı veya şifreye sahip personel var!";

                    bolumlerVeCinsiyetler();

                    return View();

                }

                if (yonetici.Any())
                {

                    ViewBag.Message = "Boyle bir kullanıcı adı veya sifreye sahip yonetici var!";

                    bolumlerVeCinsiyetler();

                    return View();


                }

                if (admin.Any())
                {

                    ViewBag.Message = "Boyle bir kullanıcı adı veya şifreye sahip admin var!";

                    bolumlerVeCinsiyetler();

                    return View();


                }

                    

                perso.PERSONELDURUM = true;

                perso.PERSONELBASLANGICTARIHI = DateTime.Now;

                perso.KULLANICITIPI = "Personel";

                db.TBL_PERSONEL.Add(perso);

                db.SaveChanges();

                ViewBag.Message = "Personel basariyla eklendi!";

                bolumlerVeCinsiyetler();

                return View();

            }
            else
            {

             
                bolumlerVeCinsiyetler();

                return View();

            }



        }

        public ActionResult PersonelSil(int id)
        {

            var personel = db.TBL_PERSONEL.Find(id);

            personel.PERSONELDURUM = false;

            personel.PERSONELBITISTARIHI = DateTime.Now;


            db.SaveChanges();

            return RedirectToAction("Index", "Personel");

        }


        [Authorize(Roles="Yönetici")]
        public ActionResult SilinmisPersoneller(string ad, string soyad, int sayfa = 1)
        {


            int subeID = Convert.ToInt32(Session["YoneticiSube"].ToString());

            var personeller = db.TBL_PERSONEL.Where(x => x.PERSONELDURUM == false && x.PERSONELSUBE == subeID).ToList();

            if (!string.IsNullOrEmpty(ad))
            {

                personeller = personeller.Where(x => x.PERSONELAD.Contains(ad)).ToList();

            }

            if (!string.IsNullOrEmpty(soyad))
            {

                personeller = personeller.Where(x => x.PERSONELSOYAD.Contains(soyad)).ToList();

            }


            if (!string.IsNullOrEmpty(soyad) && !string.IsNullOrEmpty(ad))
            {

                personeller = personeller.Where(x => x.PERSONELSOYAD.Contains(soyad) && x.PERSONELAD.Contains(ad)).ToList();

            }



            return View(personeller.ToPagedList(sayfa, 10));


        }


        public ActionResult PersonelGeriGetir(int id)
        {


            var personel = db.TBL_PERSONEL.Find(id);

            personel.PERSONELDURUM = true;

            personel.PERSONELBITISTARIHI = null;

            personel.PERSONELBASLANGICTARIHI = DateTime.Now;

            db.SaveChanges();

            return RedirectToAction("Index", "Personel");


        }

        [Authorize(Roles="Yönetici")]
        [HttpGet]
        public ActionResult PersonelGuncelle(int id)
        {


            var personel = db.TBL_PERSONEL.Find(id);


            bolumlerVeCinsiyetler();


            return View(personel);


        }


        [HttpPost]
        public ActionResult PersonelGuncelle(TBL_PERSONEL perso)
        {


            if (String.IsNullOrEmpty(perso.PERSONELAD) || String.IsNullOrEmpty(perso.PERSONELSOYAD) || String.IsNullOrEmpty(perso.PERSONELMAIL) || String.IsNullOrEmpty(perso.PERSONELTELEFON) || String.IsNullOrEmpty(perso.PERSONELTC) || perso.PERSONELMAAS == null || perso.PERSONELYAS == null || String.IsNullOrEmpty(perso.KULLANICIADI) || String.IsNullOrEmpty(perso.SIFRE))
            {

                ViewBag.Message = "Boş deger girmeyin!";

                bolumlerVeCinsiyetler();

                return View();

            }


            try
            {

                var bolum = db.TBL_BOLUM.Where(m => m.BOLUMID == perso.TBL_BOLUM.BOLUMID).FirstOrDefault();

                perso.TBL_BOLUM = bolum;

            }
            catch
            {

                ViewBag.Message = "Once bolum ekleyiniz!";

                bolumlerVeCinsiyetler();

                return View();

            }



            if (ModelState.IsValid)
            {


                char[] TC = perso.PERSONELTC.ToCharArray();
                char[] Ad = perso.PERSONELAD.ToCharArray();
                char[] Soyad = perso.PERSONELSOYAD.ToCharArray();

                foreach (char x in TC)
                {

                    if (!char.IsDigit(x))
                    {

                        ViewBag.Message = "TC Kimlik Numarası rakamlardan oluşmalıdır!";
                        bolumlerVeCinsiyetler();
                        return View();
                    }

                }

                foreach (char x in Ad)
                {

                    if (!char.IsLetter(x))
                    {

                        ViewBag.Message = "Ad harflerden oluşmalıdır!";
                        bolumlerVeCinsiyetler();
                        return View();



                    }

                }

                foreach (char x in Soyad)
                {

                    if (!char.IsLetter(x))
                    {

                        ViewBag.Message = "Soyad harflerden oluşmalıdır!";
                        bolumlerVeCinsiyetler();
                        return View();

                    }

                }


                char[] kullaniciAdi = perso.KULLANICIADI.ToCharArray();
                char[] kullaniciSifre = perso.SIFRE.ToCharArray();

                foreach (char x in kullaniciAdi)
                {

                    if (!Char.IsLetterOrDigit(x))
                    {

                        ViewBag.Message = "Kullanıcı adı sadece harflerden ve rakamlardan oluşmalıdır!";

                        bolumlerVeCinsiyetler();

                        return View();
                    }

                }

                foreach (char x in kullaniciSifre)
                {

                    if (!Char.IsDigit(x))
                    {

                        ViewBag.Message = "Şifre sadece rakamlardan oluşmalıdır!";

                        bolumlerVeCinsiyetler();

                        return View();


                    }


                }

             
                var personelKontrol = db.TBL_PERSONEL.Where(m => m.PERSONELID != perso.PERSONELID).Where(m => m.PERSONELTC == perso.PERSONELTC || m.PERSONELTELEFON == perso.PERSONELTELEFON || m.PERSONELMAIL == perso.PERSONELMAIL);

                var musteriKontrol = db.TBL_MUSTERI.Where(m => m.MUSTERITC == perso.PERSONELTC || m.MUSTERITELEFON == perso.PERSONELTELEFON || m.MUSTERIMAIL == perso.PERSONELMAIL);

                var yoneticiKontrol = db.TBL_YONETICI.Where(m => m.YONETICITC == perso.PERSONELTC || m.YONETICITELEFON == perso.PERSONELTELEFON || m.YONETICIMAIL == perso.PERSONELMAIL);

                if (personelKontrol.Any())
                {

                    ViewBag.Message = "Bir personel ile aynı TC veya Telefona veya maile sahip bir personel olusturulamaz!";

                    bolumlerVeCinsiyetler();

                    return View();


                }

                if (musteriKontrol.Any())
                {

                    ViewBag.Message = "Bir musteri ile aynı TC veya Telefona veya maile sahip bir personel olusturulamaz!";

                    bolumlerVeCinsiyetler();

                    return View();


                }


                if (yoneticiKontrol.Any())
                {

                    ViewBag.Message = "Bir yonetici ile aynı TC veya Telefona veya maile sahip bir personel olusturulamaz!";

                    bolumlerVeCinsiyetler();

                    return View();


                }


                var prsl = db.TBL_PERSONEL.Where(m => m.PERSONELID != perso.PERSONELID).Where(m => m.KULLANICIADI == perso.KULLANICIADI || m.SIFRE == perso.SIFRE);
                var yonetici = db.TBL_YONETICI.Where(m => m.KULLANICIADI == perso.KULLANICIADI || m.SIFRE == perso.SIFRE);
                var admin = db.TBL_ADMIN.Where(m => m.KULLANICIADI == perso.KULLANICIADI || m.SIFRE == perso.SIFRE);


                if (prsl.Any())
                {

                    ViewBag.Message = "Boyle bir kullanıcı adı veya şifreye sahip personel var!";

                    bolumlerVeCinsiyetler();

                    return View();

                }

                if (yonetici.Any())
                {

                    ViewBag.Message = "Boyle bir kullanıcı adı veya şifreye sahip yonetici var!";

                    bolumlerVeCinsiyetler();

                    return View();


                }

                if (admin.Any())
                {

                    ViewBag.Message = "Boyle bir kullanıcı adı veya şifreye sahip admin var!";

                    bolumlerVeCinsiyetler();

                    return View();


                }


                      
                var personel = db.TBL_PERSONEL.Find(perso.PERSONELID);


                personel.TBL_BOLUM = perso.TBL_BOLUM;

                personel.PERSONELAD = perso.PERSONELAD;

                personel.PERSONELSOYAD = perso.PERSONELSOYAD;

                personel.PERSONELMAIL = perso.PERSONELMAIL;

                personel.PERSONELTELEFON = perso.PERSONELTELEFON;

                personel.PERSONELTC = perso.PERSONELTC;

                personel.PERSONELMAAS = perso.PERSONELMAAS;

                personel.PERSONELCINSIYET = perso.PERSONELCINSIYET;

                personel.PERSONELYAS = perso.PERSONELYAS;

                personel.KULLANICIADI = perso.KULLANICIADI;

                personel.SIFRE = perso.SIFRE;

                db.SaveChanges();

                ViewBag.Message = "Personel basariyla guncellendi!";

                bolumlerVeCinsiyetler();

                return View();

            }
            else
            {


                bolumlerVeCinsiyetler();

                return View();

            }



        }


    }
}