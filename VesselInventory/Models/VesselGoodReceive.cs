namespace VesselInventory.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;
    using VesselInventory.Commons.Enums;
    using VesselInventory.Utility;

    public partial class VesselGoodReceive
    {
        [Key]
        public int VesselGoodReceiveId { get; set; }

        [Required]
        [StringLength(25)]
        public string VesselGoodReceiveNumber { get; set; }

        [Required]
        [StringLength(25)]
        public string OfficeGoodIssuedNumber { get; set; }

        [Column(TypeName = "date")]
        public DateTime VesselGoodReceiveDate { get; set; } = DateTime.Now;

        public int ShipId { get; set; }

        [Required]
        [StringLength(30)]
        public string ShipName { get; set; }

        public int BargeId { get; set; }

        [Required]
        [StringLength(30)]
        public string BargeName { get; set; }

        public DateTime CreatedDate { get; set; } = DateTime.Now;

        [Required]
        [StringLength(30)]
        public string CreatedBy { get; set; } = Auth.Instance.personalname;

        public DateTime? LastModifiedDate { get; set; }

        [StringLength(30)]
        public string LastModifiedBy { get; set; }

        [Required]
        [StringLength(10)]
        public string SyncStatus { get; set; } = Commons.Enums.SyncStatus.Not_Sync.GetDescription();
        public bool IsHidden { get; set; } = false;
    }
}
