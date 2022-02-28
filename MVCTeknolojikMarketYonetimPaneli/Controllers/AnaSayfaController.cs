using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MVCTeknolojikMarketYonetimPaneli.Controllers
{
    public class AnaSayfaController : Controller
    {
        // GET: AnaSayfa

        [Authorize(Roles="Personel,Yönetici,Admin")]
        public ActionResult Index()
        {


            try
            {

                ViewBag.message = Session["KullaniciAdi"].ToString();

                 return View();

            }catch(Exception e){

                Console.WriteLine(e.Message);

                return RedirectToAction("Giris", "Giris");

            }

           
        }
    }
}