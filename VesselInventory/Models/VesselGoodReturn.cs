namespace VesselInventory.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("VesselGoodReturn")]
    public partial class VesselGoodReturn
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public VesselGoodReturn()
        {
            VesselGoodReturnItem = new HashSet<VesselGoodReturnItem>();
        }

        public int VesselGoodReturnId { get; set; }

        [Required]
        [StringLength(25)]
        public string VesselGoodReturnNumber { get; set; }

        [Column(TypeName = "date")]
        public DateTime VesselGoodReturnDate { get; set; }

        public int ShipId { get; set; }

        [Required]
        [StringLength(30)]
        public string ShipName { get; set; }

        [Column(TypeName = "text")]
        public string Notes { get; set; }

        public DateTime CreatedDate { get; set; }

        [Required]
        [StringLength(30)]
        public string CreatedBy { get; set; }

        public DateTime? LastModifiedDate { get; set; }

        [StringLength(30)]
        public string LastModifiedBy { get; set; }

        [Required]
        [StringLength(30)]
        public string SyncStatus { get; set; }

        public bool IsHidden { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<VesselGoodReturnItem> VesselGoodReturnItem { get; set; }
    }
}
