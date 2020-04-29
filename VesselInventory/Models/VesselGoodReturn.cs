namespace VesselInventory.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;
    using VesselInventory.Commons.Enums;
    using VesselInventory.Utility;

    public partial class VesselGoodReturn
    {
        [Key]
        public int VesselGoodReturnId { get; set; }

        [Required]
        [StringLength(25)]
        public string VesselGoodreturnNumber { get; set; }

        [Column(TypeName = "date")]
        public DateTime VesselGoodReturnDate { get; set; }

        public int ShipId { get; set; }

        [Required]
        [StringLength(30)]
        public string ShipName { get; set; }

        [Column(TypeName = "text")]
        public string Notes { get; set; }

        [Required]
        [StringLength(15)]
        public string SyncStatus { get; set; } = Commons.Enums.SyncStatus.Not_Sync.GetDescription();

        public DateTime CreatedDate { get; set; } = DateTime.Now;

        [Required]
        [StringLength(30)]
        public string CreatedBy { get; set; } = Auth.Instance.personalname;

        public DateTime? LastModifiedDate { get; set; }

        [StringLength(30)]
        public string LastModifiedBy { get; set; }
        public bool IsHidden { get; set; } = false;
    }
}
