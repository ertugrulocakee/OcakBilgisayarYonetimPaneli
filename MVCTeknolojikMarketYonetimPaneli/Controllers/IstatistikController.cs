using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MVCTeknolojikMarketYonetimPaneli.Models.Model;
using MVCTeknolojikMarketYonetimPaneli.Models.EkModel;

namespace MVCTeknolojikMarketYonetimPaneli.Controllers
{
    public class IstatistikController : Controller
    {
        // GET: Istatistik
        TeknolojikMarketEntities db = new TeknolojikMarketEntities();


        [Authorize(Roles="Yönetici")]
        public ActionResult Index()
        {

            int subeID = Convert.ToInt32(Session["YoneticiSube"].ToString());

            string urunAd,personelAdSoyad,musteriAdSoyad,kategoriAdi;

            try
            {

                 urunAd = db.TBL_SATIS.Where(m => m.TBL_SUBE.SUBEID == subeID).GroupBy(m => m.TBL_URUN.URUNAD).OrderByDescending((x) => x.Count()).FirstOrDefault().Key.ToString();

               
            }
            catch (Exception e)
            {

                urunAd = "Satış işlemi girilmeye başlandıkça şubenin en çok satılan ürünü size gösterilecek!";

              
                Console.WriteLine(e.Message);

            }

            try
            {

                personelAdSoyad = db.TBL_SATIS.Where(m => m.TBL_SUBE.SUBEID == subeID).GroupBy(m => (m.TBL_PERSONEL.PERSONELAD + " " + m.TBL_PERSONEL.PERSONELSOYAD)).OrderByDescending((x) => x.Count()).FirstOrDefault().Key.ToString();


            }
            catch (Exception e)
            {

                personelAdSoyad = "Satış işlemi girilmeye başlandıkça şubenin en başarılı satış personeli size gösterilecek!";

                Console.WriteLine(e.Message);

            }

            try
            {

                musteriAdSoyad = db.TBL_SATIS.Where(m => m.TBL_SUBE.SUBEID == subeID).GroupBy(m => (m.TBL_MUSTERI.MUSTERIAD + " " + m.TBL_MUSTERI.MUSTERISOYAD)).OrderByDescending((x) => x.Count()).FirstOrDefault().Key.ToString();

            }
            catch (Exception e)
            {

                musteriAdSoyad = "Satış işlemi girilmeye başlandıkça şubenin en müdavim müşterisi size gösterilecek!";

                Console.WriteLine(e.Message);

            }

            try
            {

                kategoriAdi = db.TBL_SATIS.Where(m => m.TBL_SUBE.SUBEID == subeID).GroupBy(m => m.TBL_URUN.TBL_KATEGORI.KATEGORIAD).OrderByDescending((x) => x.Count()).FirstOrDefault().Key.ToString();

            }
            catch (Exception e)
            {

                kategoriAdi = "Satış işlemi girilmeye başlandıkça şubenin en başarılı kategorisi (satış bakımından) size gösterilecek!";

                Console.WriteLine(e.Message);

            }

           
            Istatistik istatistik = new Istatistik();

            istatistik.Urun = urunAd;

            istatistik.Musteri = musteriAdSoyad;

            istatistik.Personel = personelAdSoyad;

            istatistik.Kategori = kategoriAdi;

            return View(istatistik);

        }


        [Authorize(Roles = "Admin")]
        public ActionResult GenelIstatistik()
        {

            string urunAd, personelAdSoyad, musteriAdSoyad,kategoriAdi,subeAdi;

            try
            {

                urunAd = db.TBL_SATIS.GroupBy(m => m.TBL_URUN.URUNAD).OrderByDescending((x) => x.Count()).FirstOrDefault().Key.ToString();


            }
            catch (Exception e)
            {

                urunAd = "Satış işlemi girilmeye başlandıkça firmanın en çok satılan ürünü size gösterilecek!";


                Console.WriteLine(e.Message);

            }

            try
            {

                personelAdSoyad = db.TBL_SATIS.GroupBy(m => (m.TBL_PERSONEL.PERSONELAD + " " + m.TBL_PERSONEL.PERSONELSOYAD)).OrderByDescending((x) => x.Count()).FirstOrDefault().Key.ToString();


            }
            catch (Exception e)
            {

                personelAdSoyad = "Satış işlemi girilmeye başlandıkça firmanın en başarılı satış personeli size gösterilecek!";

                Console.WriteLine(e.Message);

            }

            try
            {

                musteriAdSoyad = db.TBL_SATIS.GroupBy(m => (m.TBL_MUSTERI.MUSTERIAD + " " + m.TBL_MUSTERI.MUSTERISOYAD)).OrderByDescending((x) => x.Count()).FirstOrDefault().Key.ToString();

            }
            catch (Exception e)
            {

                musteriAdSoyad = "Satış işlemi girilmeye başlandıkça firmanın en müdavim müşterisi size gösterilecek!";

                Console.WriteLine(e.Message);

            }


            try
            {

                kategoriAdi = db.TBL_SATIS.GroupBy(m => m.TBL_URUN.TBL_KATEGORI.KATEGORIAD).OrderByDescending((x) => x.Count()).FirstOrDefault().Key.ToString();

            }
            catch (Exception e)
            {

                kategoriAdi = "Satış işlemi girilmeye başlandıkça firmanın en başarılı kategorisi (satış bakımından) size gösterilecek!";

                Console.WriteLine(e.Message);

            }

            try
            {

                subeAdi = db.TBL_SATIS.GroupBy(m => m.TBL_SUBE.SUBEAD).OrderByDescending((x) => x.Count()).FirstOrDefault().Key.ToString();

            }
            catch (Exception e)
            {

                subeAdi = "Satış işlemi girilmeye başlandıkça firmanın en başarılı şubesi size gösterilecek!";

                Console.WriteLine(e.Message);

            }


            Istatistik istatistik = new Istatistik();

            istatistik.Urun = urunAd;

            istatistik.Musteri = musteriAdSoyad;

            istatistik.Personel = personelAdSoyad;

            istatistik.Kategori = kategoriAdi;

            istatistik.Sube = subeAdi;

            return View(istatistik);


        }



    }
}