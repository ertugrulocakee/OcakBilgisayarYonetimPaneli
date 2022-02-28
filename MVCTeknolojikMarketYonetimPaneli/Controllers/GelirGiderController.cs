using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MVCTeknolojikMarketYonetimPaneli.Models.Model;
using MVCTeknolojikMarketYonetimPaneli.Models.EkModel;

namespace MVCTeknolojikMarketYonetimPaneli.Controllers
{
    public class GelirGiderController : Controller
    {
        // GET: GelirGider

        TeknolojikMarketEntities db = new TeknolojikMarketEntities();


        [Authorize(Roles="Yönetici")]
        public ActionResult Index()
        {

            int subeID = Convert.ToInt32(Session["YoneticiSube"].ToString());

            string aylikGelir, aylikGider, yillikGelir, yillikGider,toplamGelir,toplamGider,aylikKarZarar,yillikKarZarar,KarZarar;


            try
            {

                aylikGelir = (db.TBL_GELIR.Where(m => m.GELIRSUBE == subeID && m.GELIRTARIHI.Value.Month == DateTime.Now.Month && m.GELIRTARIHI.Value.Year == DateTime.Now.Year).Sum(m => m.SATISKARI).Value).ToString();
                

            }
            catch(Exception e)
            {

                Console.WriteLine(e.Message);

                aylikGelir = "Henüz bu ayın geliri girilmedi!";

            }

            try
            {

                aylikGider = (db.TBL_GIDER.Where(m => m.GIDERSUBE == subeID && m.GIDERTARIHI.Value.Month == DateTime.Now.Month && m.GIDERTARIHI.Value.Year == DateTime.Now.Year).Sum(m => m.GIDERKIRA + m.GIDERENERJI + m.GIDERDIGER + m.GIDERPERSONEL).Value).ToString();
                
  
            }
            catch (Exception e)
            {

                aylikGider = "Henüz bu ayın gideri girilmedi!";

                Console.WriteLine(e.Message);

            }
            

            try {

                aylikKarZarar = (decimal.Parse(aylikGelir) - decimal.Parse(aylikGider)).ToString();
                

            }
            catch (Exception e)
            {

                aylikKarZarar = "Henüz bu ayın geliri veya gideri girilmedi!";

                Console.WriteLine(e.Message);

            }


            try
            {

                yillikGelir = (db.TBL_GELIR.Where(m => m.GELIRSUBE == subeID && m.GELIRTARIHI.Value.Year == DateTime.Now.Year).Sum(m => m.SATISKARI).Value).ToString();
                

            }
            catch (Exception e)
            {

                yillikGelir = "Henüz bu yılın bir geliri girilmiş değil!";

                Console.WriteLine(e.Message);

            }


            try
            {

                yillikGider = (db.TBL_GIDER.Where(m => m.GIDERSUBE == subeID && m.GIDERTARIHI.Value.Year == DateTime.Now.Year).Sum(m => m.GIDERKIRA + m.GIDERENERJI + m.GIDERDIGER + m.GIDERPERSONEL).Value).ToString();
                

            }
            catch (Exception e)
            {

                Console.WriteLine(e.Message);

                yillikGider = "Henüz bu yılın bir gideri girilmiş değil!";

            }
          

            try
            {


                yillikKarZarar = (decimal.Parse(yillikGelir) - decimal.Parse(yillikGider)).ToString();
                

            }
            catch (Exception e)
            {

                yillikKarZarar = "Henüz bu yılın bir geliri veya gideri girilmiş değil!";

                Console.WriteLine(e.Message);

            }


            try
            {

                toplamGelir = (db.TBL_GELIR.Where(m => m.GELIRSUBE == subeID).Sum(m => m.SATISKARI).Value).ToString();
               

            }
            catch (Exception e)
            {

                toplamGelir = "Henüz bir gelir girdisi yok!";
                Console.WriteLine(e.Message);

            }


            try
            {

                toplamGider = (db.TBL_GIDER.Where(m => m.GIDERSUBE == subeID).Sum(m => m.GIDERKIRA + m.GIDERENERJI + m.GIDERDIGER + m.GIDERPERSONEL).Value).ToString();
               

            }
            catch (Exception e)
            {

                toplamGider = "Henüz bir gider girdisi yok!";
                Console.WriteLine(e.Message);

            }
    
            try
            {

                KarZarar = (decimal.Parse(toplamGelir) - decimal.Parse(toplamGider)).ToString();
                

            }
            catch(Exception e)
            {

                KarZarar = "Henüz bir gelir veya gider girdisi yok!";

                Console.WriteLine(e.Message);

            }


            if (aylikGelir != "Henüz bu ayın geliri girilmedi!")
            {

                aylikGelir += " TL";

            }


            if (yillikGelir != "Henüz bu yılın bir geliri girilmiş değil!")
            {

                yillikGelir += " TL";

            }

            if (aylikGider != "Henüz bu ayın gideri girilmedi!")
            {

                aylikGider += " TL";
            }

            if (yillikGider != "Henüz bu yılın bir gideri girilmiş değil!")
            {


                yillikGider += " TL";
            }

            if (aylikKarZarar != "Henüz bu ayın geliri veya gideri girilmedi!")
            {

                aylikKarZarar += " TL";
            }

            if (yillikKarZarar != "Henüz bu yılın bir geliri veya gideri girilmiş değil!")
            {
                yillikKarZarar += " TL";

            }

            if (toplamGelir != "Henüz bir gelir girdisi yok!")
            {

                toplamGelir += " TL";
            }

            if (toplamGider != "Henüz bir gider girdisi yok!")
            {

                toplamGider += " TL";

            }

            if (KarZarar != "Henüz bir gelir veya gider girdisi yok!")
            {

                KarZarar += " TL";
            }
            
            ZamanaGoreGelir zamanaGoreGelir = new ZamanaGoreGelir();

            zamanaGoreGelir.AylikGelir = aylikGelir;

            zamanaGoreGelir.YillikGelir = yillikGelir;

            zamanaGoreGelir.ToplamGelir = toplamGelir;

            ZamanaGoreGider zamanaGoreGider = new ZamanaGoreGider();

            zamanaGoreGider.AylikGider = aylikGider;

            zamanaGoreGider.YillikGider = yillikGider;

            zamanaGoreGider.ToplamGider = toplamGider;

            KarZarar karZarar = new KarZarar();

            karZarar.AylikKarZarar = aylikKarZarar;

            karZarar.YillikKarZarar = yillikKarZarar;

            karZarar.KarZararDengesi = KarZarar;


            return View(Tuple.Create<ZamanaGoreGelir, ZamanaGoreGider, KarZarar>(zamanaGoreGelir, zamanaGoreGider, karZarar));

            
        }

        [HttpGet]
        [Authorize(Roles = "Yönetici")]
        public ActionResult GelirEkle()
        {



            return View();


        }



        [HttpPost]
        public ActionResult GelirEkle(TBL_GELIR gelir)
        {

            int subeID = Convert.ToInt32(Session["YoneticiSube"].ToString());

            if (ModelState.IsValid)
            {

                var glr = db.TBL_GELIR.Where(m=>m.TBL_SUBE.SUBEID == subeID).Where(m => m.GELIRTARIHI.Value.Month == DateTime.Now.Month && m.GELIRTARIHI.Value.Year == DateTime.Now.Year);

                if (glr.Any())
                {

                    ViewBag.Message = "Ay-Yıl zaman ikilisi icin sadece bir gelir girebilirsin!";

                    return View();

                }

            
                var sube = db.TBL_SUBE.Where(m => m.SUBEID == subeID).FirstOrDefault();

                gelir.TBL_SUBE = sube;

                gelir.GELIRTARIHI = DateTime.Now;

                gelir.DURUM = true;

                db.TBL_GELIR.Add(gelir);

                db.SaveChanges();


                return RedirectToAction("Index", "GelirGider");

            }
            else
            {

                return View();

            }

        }


        [HttpGet]
        [Authorize(Roles = "Yönetici")]
        public ActionResult GiderEkle()
        {


            return View();

        }


        [HttpPost]
        public ActionResult GiderEkle(TBL_GIDER gider)
        {

            int subeID = Convert.ToInt32(Session["YoneticiSube"].ToString());

            if (ModelState.IsValid)
            {

                var gdr = db.TBL_GIDER.Where(m=>m.TBL_SUBE.SUBEID == subeID).Where(m => m.GIDERTARIHI.Value.Month == DateTime.Now.Month && m.GIDERTARIHI.Value.Year == DateTime.Now.Year);

                if (gdr.Any())
                {

                    ViewBag.Message = "Ay-Yıl zaman ikilisi icin sadece bir gider girebilirsin!";

                    return View();

                }
                
                var sube = db.TBL_SUBE.Where(m => m.SUBEID == subeID).FirstOrDefault();

                gider.TBL_SUBE = sube;

                gider.GIDERTARIHI = DateTime.Now;

                gider.DURUM = true;

                db.TBL_GIDER.Add(gider);

                db.SaveChanges();

                return RedirectToAction("Index", "GelirGider");

            }
            else
            {


                return View();

            }


           

        }


        [Authorize(Roles="Admin")]
        public ActionResult GenelGelirGider()
        {
         
            string aylikGelir, aylikGider, yillikGelir, yillikGider, toplamGelir, toplamGider, aylikKarZarar, yillikKarZarar, KarZarar;


            try
            {

                aylikGelir = (db.TBL_GELIR.Where(m=> m.GELIRTARIHI.Value.Month == DateTime.Now.Month && m.GELIRTARIHI.Value.Year == DateTime.Now.Year).Sum(m => m.SATISKARI).Value).ToString();


            }
            catch (Exception e)
            {

                Console.WriteLine(e.Message);

                aylikGelir = "Henüz bu ayın geliri girilmedi!";

            }

            try
            {

                aylikGider = (db.TBL_GIDER.Where(m => m.GIDERTARIHI.Value.Month == DateTime.Now.Month && m.GIDERTARIHI.Value.Year == DateTime.Now.Year).Sum(m => m.GIDERKIRA + m.GIDERENERJI + m.GIDERDIGER + m.GIDERPERSONEL).Value).ToString();


            }
            catch (Exception e)
            {

                aylikGider = "Henüz bu ayın gideri girilmedi!";

                Console.WriteLine(e.Message);

            }


            try
            {

                aylikKarZarar = (decimal.Parse(aylikGelir) - decimal.Parse(aylikGider)).ToString();


            }
            catch (Exception e)
            {

                aylikKarZarar = "Henüz bu ayın geliri veya gideri girilmedi!";

                Console.WriteLine(e.Message);

            }


            try
            {

                yillikGelir = (db.TBL_GELIR.Where(m => m.GELIRTARIHI.Value.Year == DateTime.Now.Year).Sum(m => m.SATISKARI).Value).ToString();


            }
            catch (Exception e)
            {

                yillikGelir = "Henüz bu yılın bir geliri girilmiş değil!";

                Console.WriteLine(e.Message);

            }


            try
            {

                yillikGider = (db.TBL_GIDER.Where(m => m.GIDERTARIHI.Value.Year == DateTime.Now.Year).Sum(m => m.GIDERKIRA + m.GIDERENERJI + m.GIDERDIGER + m.GIDERPERSONEL).Value).ToString();


            }
            catch (Exception e)
            {

                Console.WriteLine(e.Message);

                yillikGider = "Henüz bu yılın bir gideri girilmiş değil!";

            }


            try
            {


                yillikKarZarar = (decimal.Parse(yillikGelir) - decimal.Parse(yillikGider)).ToString();


            }
            catch (Exception e)
            {

                yillikKarZarar = "Henüz bu yılın bir geliri veya gideri girilmiş değil!";

                Console.WriteLine(e.Message);

            }


            try
            {

                toplamGelir = (db.TBL_GELIR.Sum(m => m.SATISKARI).Value).ToString();


            }
            catch (Exception e)
            {

                toplamGelir = "Henüz bir gelir girdisi yok!";
                Console.WriteLine(e.Message);

            }


            try
            {

                toplamGider = (db.TBL_GIDER.Sum(m => m.GIDERKIRA + m.GIDERENERJI + m.GIDERDIGER + m.GIDERPERSONEL).Value).ToString();


            }
            catch (Exception e)
            {

                toplamGider = "Henüz bir gider girdisi yok!";
                Console.WriteLine(e.Message);

            }

            try
            {

                KarZarar = (decimal.Parse(toplamGelir) - decimal.Parse(toplamGider)).ToString();


            }
            catch (Exception e)
            {

                KarZarar = "Henüz bir gelir veya gider girdisi yok!";

                Console.WriteLine(e.Message);

            }


            if (aylikGelir != "Henüz bu ayın geliri girilmedi!")
            {

                aylikGelir += " TL";

            }


            if (yillikGelir != "Henüz bu yılın bir geliri girilmiş değil!")
            {

                yillikGelir += " TL";

            }

            if (aylikGider != "Henüz bu ayın gideri girilmedi!")
            {

                aylikGider += " TL";
            }

            if (yillikGider != "Henüz bu yılın bir gideri girilmiş değil!")
            {


                yillikGider += " TL";
            }

            if (aylikKarZarar != "Henüz bu ayın geliri veya gideri girilmedi!")
            {

                aylikKarZarar += " TL";
            }

            if (yillikKarZarar != "Henüz bu yılın bir geliri veya gideri girilmiş değil!")
            {
                yillikKarZarar += " TL";

            }

            if (toplamGelir != "Henüz bir gelir girdisi yok!")
            {

                toplamGelir += " TL";
            }

            if (toplamGider != "Henüz bir gider girdisi yok!")
            {

                toplamGider += " TL";

            }

            if (KarZarar != "Henüz bir gelir veya gider girdisi yok!")
            {

                KarZarar += " TL";
            }



            ZamanaGoreGelir zamanaGoreGelir = new ZamanaGoreGelir();

            zamanaGoreGelir.AylikGelir = aylikGelir;

            zamanaGoreGelir.YillikGelir = yillikGelir;

            zamanaGoreGelir.ToplamGelir = toplamGelir;

            ZamanaGoreGider zamanaGoreGider = new ZamanaGoreGider();

            zamanaGoreGider.AylikGider = aylikGider;

            zamanaGoreGider.YillikGider = yillikGider;

            zamanaGoreGider.ToplamGider = toplamGider;

            KarZarar karZarar = new KarZarar();

            karZarar.AylikKarZarar = aylikKarZarar;

            karZarar.YillikKarZarar = yillikKarZarar;

            karZarar.KarZararDengesi = KarZarar;


            return View(Tuple.Create<ZamanaGoreGelir, ZamanaGoreGider, KarZarar>(zamanaGoreGelir, zamanaGoreGider, karZarar));


        }

                
    }
}