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
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    using System.Data.Entity.Core.Objects;
    using System.Linq;
    
    public partial class TeknolojikMarketEntities : DbContext
    {
        public TeknolojikMarketEntities()
            : base("name=TeknolojikMarketEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<TBL_ADMIN> TBL_ADMIN { get; set; }
        public virtual DbSet<TBL_BOLUM> TBL_BOLUM { get; set; }
        public virtual DbSet<TBL_DUYURU> TBL_DUYURU { get; set; }
        public virtual DbSet<TBL_GELIR> TBL_GELIR { get; set; }
        public virtual DbSet<TBL_GIDER> TBL_GIDER { get; set; }
        public virtual DbSet<TBL_KATEGORI> TBL_KATEGORI { get; set; }
        public virtual DbSet<TBL_MESAJPERYON> TBL_MESAJPERYON { get; set; }
        public virtual DbSet<TBL_MESAJYONPER> TBL_MESAJYONPER { get; set; }
        public virtual DbSet<TBL_MUSTERI> TBL_MUSTERI { get; set; }
        public virtual DbSet<TBL_PERSONEL> TBL_PERSONEL { get; set; }
        public virtual DbSet<TBL_SATIS> TBL_SATIS { get; set; }
        public virtual DbSet<TBL_SUBE> TBL_SUBE { get; set; }
        public virtual DbSet<TBL_URUN> TBL_URUN { get; set; }
        public virtual DbSet<TBL_YONETICI> TBL_YONETICI { get; set; }
    
        public virtual ObjectResult<SOHBET_Result> SOHBET(string bASLIK)
        {
            var bASLIKParameter = bASLIK != null ?
                new ObjectParameter("BASLIK", bASLIK) :
                new ObjectParameter("BASLIK", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<SOHBET_Result>("SOHBET", bASLIKParameter);
        }
    }
}
