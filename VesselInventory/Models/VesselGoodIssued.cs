namespace VesselInventory.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("VesselGoodIssued")]
    public partial class VesselGoodIssued
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public VesselGoodIssued()
        {
            VesselGoodIssuedItem = new HashSet<VesselGoodIssuedItem>();
        }

        public int VesselGoodIssuedId { get; set; }

        [Required]
        [StringLength(25)]
        public string VesselGoodIssuedNumber { get; set; }

        [Column(TypeName = "date")]
        public DateTime VesselGoodIssuedDate { get; set; }

        public int ShipId { get; set; }

        [Required]
        [StringLength(30)]
        public string ShipName { get; set; }

        [Column(TypeName = "text")]
        public string Notes { get; set; }

        [Required]
        [StringLength(30)]
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
        public virtual ICollection<VesselGoodIssuedItem> VesselGoodIssuedItem { get; set; }
    }
}
