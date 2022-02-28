using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MVCTeknolojikMarketYonetimPaneli.Models.EkModel
{
    public class ZamanaGoreGelir
    {

        string aylikGelir;

        public string AylikGelir
        {
            get { return aylikGelir; }
            set { aylikGelir = value; }
        }

        string yillikGelir;

        public string YillikGelir
        {
            get { return yillikGelir; }
            set { yillikGelir = value; }
        }

        string toplamGelir;

        public string ToplamGelir
        {
            get { return toplamGelir; }
            set { toplamGelir = value; }
        }

    }
}