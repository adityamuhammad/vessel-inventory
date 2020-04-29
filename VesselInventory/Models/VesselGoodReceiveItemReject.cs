namespace VesselInventory.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;
    using VesselInventory.Commons.Enums;
    using VesselInventory.Utility;

    public partial class VesselGoodReceiveItemReject
    {
        [Key]
        public int VesselGoodReceiveItemRejectId { get; set; }

        public int VesselGoodReceiveId { get; set; }

        [Required]
        [StringLength(25)]
        public string RequestFormNumber { get; set; }

        public int ItemId { get; set; }

        public int ItemGroupId { get; set; }

        [Required]
        [StringLength(50)]
        public string ItemName { get; set; }

        [Required]
        [StringLength(25)]
        public string ItemDimensionNumber { get; set; }

        [Required]
        [StringLength(5)]
        public string BrandTypeId { get; set; }

        [Required]
        [StringLength(30)]
        public string BrandTypeName { get; set; }

        [Required]
        [StringLength(5)]
        public string ColorSizeId { get; set; }

        [Required]
        [StringLength(30)]
        public string ColorSizeName { get; set; }

        [Required]
        [StringLength(10)]
        public string Uom { get; set; }

        public decimal Qty { get; set; }

        public DateTime CreatedDate { get; set; } = DateTime.Now;

        [Required]
        [StringLength(30)]
        public string CreatedBy { get; set; } = Auth.Instance.personalname;

        public DateTime? LastModifiedDate { get; set; }

        [StringLength(30)]
        public string LastModifiedBy { get; set; }

        [StringLength(15)]
        public string SyncStatus { get; set; } = Commons.Enums.SyncStatus.Not_Sync.GetDescription();
        public bool IsHidden { get; set; } = false;
    }
}
