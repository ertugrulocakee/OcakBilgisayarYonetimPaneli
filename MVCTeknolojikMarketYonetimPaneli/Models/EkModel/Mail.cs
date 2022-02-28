using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace MVCTeknolojikMarketYonetimPaneli.Models.EkModel
{
    public class Mail
    {
        [Required(ErrorMessage="Kullanıcı adınız boş olamaz!")]
        [StringLength(20,ErrorMessage="Kullanıcı adı 20 karakteri geçemez!")]
        public string KullaniciAdi { get; set; }
       
    }
}