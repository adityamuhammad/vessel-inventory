namespace VesselInventory.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("VesselGoodReceive")]
    public partial class VesselGoodReceive
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public VesselGoodReceive()
        {
            VesselGoodReceiveItemReject = new HashSet<VesselGoodReceiveItemReject>();
            VesselGoodReceiveItem = new HashSet<VesselGoodReceiveItem>();
        }

        public int VesselGoodReceiveId { get; set; }

        [Required]
        [StringLength(25)]
        public string OfficeGoodIssuedNumber { get; set; }

        [Required]
        [StringLength(25)]
        public string VesselGoodReceiveNumber { get; set; }

        [Column(TypeName = "date")]
        public DateTime? VesselGoodReceiveDate { get; set; }

        public int ShipId { get; set; }

        [Required]
        [StringLength(30)]
        public string ShipName { get; set; }

        public int BargeId { get; set; }

        [Required]
        [StringLength(30)]
        public string BargeName { get; set; }

        [Required]
        [StringLength(10)]
        public string SyncStatus { get; set; }

        public DateTime CreatedDate { get; set; }

        [Required]
        [StringLength(30)]
        public string CreatedBy { get; set; }

        public DateTime? LastModifiedDate { get; set; }

        [StringLength(30)]
        public string LastModifiedBy { get; set; }

        public bool IsHidden { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<VesselGoodReceiveItemReject> VesselGoodReceiveItemReject { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<VesselGoodReceiveItem> VesselGoodReceiveItem { get; set; }
    }
}
