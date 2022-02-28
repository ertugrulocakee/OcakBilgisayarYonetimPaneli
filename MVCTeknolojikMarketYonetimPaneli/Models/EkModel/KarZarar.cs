using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MVCTeknolojikMarketYonetimPaneli.Models.EkModel
{
    public class KarZarar
    {

        string aylikKarZarar;

        public string AylikKarZarar
        {
            get { return aylikKarZarar; }
            set { aylikKarZarar = value; }
        }

        string yillikKarZarar;

        public string YillikKarZarar
        {
            get { return yillikKarZarar; }
            set { yillikKarZarar = value; }
        }



        string karZararDengesi;

        public string KarZararDengesi
        {
            get { return karZararDengesi; }
            set { karZararDengesi = value; }
        }
    }
}