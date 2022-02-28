using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MVCTeknolojikMarketYonetimPaneli.Models.EkModel
{
    public class ZamanaGoreGider
    {

        string aylikGider;

        public string AylikGider
        {
            get { return aylikGider; }
            set { aylikGider = value; }
        }


        string yillikGider;

        public string YillikGider
        {
            get { return yillikGider; }
            set { yillikGider = value; }
        }


        string toplamGider;

        public string ToplamGider
        {
            get { return toplamGider; }
            set { toplamGider = value; }
        }

    }
}