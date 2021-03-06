//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace MVCTeknolojikMarketYonetimPaneli.Models.Model
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    
    public partial class TBL_MESAJPERYON
    {
        public int MESAJID { get; set; }
        public Nullable<int> PERSONEL { get; set; }
        public Nullable<short> YONETICI { get; set; }


        //[Required(ErrorMessage = "Mesaj ba?l??? bo? olamaz!")]
        [StringLength(20, ErrorMessage = "Mesaj Ba?l??? Maksimum 20 karakterden olu?abilir!")]
        public string MESAJBASLIGI { get; set; }

        //[Required(ErrorMessage = "Mesaj i?eri?i bo? olamaz!")]
        [StringLength(250, ErrorMessage = "Mesaj ??eri?i Maksimum 250 karakterden olu?abilir!")]
        public string MESAJICERIGI { get; set; }


        public Nullable<bool> MESAJYON { get; set; }
        public Nullable<System.DateTime> TARIH { get; set; }
        public Nullable<bool> DURUM { get; set; }
    
        public virtual TBL_PERSONEL TBL_PERSONEL { get; set; }
        public virtual TBL_YONETICI TBL_YONETICI { get; set; }
    }
}
