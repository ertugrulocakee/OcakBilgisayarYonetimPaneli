using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MVCTeknolojikMarketYonetimPaneli.Controllers
{
    public class CikisController : Controller
    {
        // GET: Cikis
        [Authorize(Roles="Personel,Yönetici,Admin")]
        public ActionResult Index()
        {

            Session.RemoveAll();

            return RedirectToAction("Giris", "Giris");
        }
    }
}