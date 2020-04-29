namespace VesselInventory.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;
    using VesselInventory.Commons.Enums;
    using VesselInventory.Utility;

    public partial class VesselGoodReturnItem
    {
        [Key]
        public int VesselGoodReturnItemId { get; set; }

        public int VesselGoodReturnId { get; set; }

        public int ItemId { get; set; }
        public int ItemGroupId { get; set; }

        [Required]
        [StringLength(25)]
        public string ItemDimensionNumber { get; set; }

        [Required]
        [StringLength(50)]
        public string ItemName { get; set; }

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
        [StringLength(30)]
        public string Reason { get; set; }

        [Required]
        public decimal Qty { get; set; }

        [Required]
        [StringLength(10)]
        public string Uom { get; set; }

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
