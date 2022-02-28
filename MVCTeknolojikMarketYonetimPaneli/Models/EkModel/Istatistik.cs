using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MVCTeknolojikMarketYonetimPaneli.Models.EkModel
{
    public class Istatistik
    {

        string urun;

        public string Urun
        {
            get { return urun; }
            set { urun = value; }
        }

        string musteri;

        public string Musteri
        {
            get { return musteri; }
            set { musteri = value; }
        }

        string personel;

        public string Personel
        {
            get { return personel; }
            set { personel = value; }
        }

        string kategori;

        public string Kategori
        {
            get { return kategori; }
            set { kategori = value; }
        }

        string sube;

        public string Sube
        {
            get { return sube; }
            set { sube = value; }
        }


    }
}