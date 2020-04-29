namespace VesselInventory.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;
    using VesselInventory.Commons.Enums;
    using VesselInventory.Utility;

    public partial class RequestFormItem
    {
        [Key]
        public int RequestFormItemId { get; set; }

        public int RequestFormId { get; set; }

        public int ItemId { get; set; }

        [StringLength(50)]
        public string ItemName { get; set; }

        public int? ItemGroupId { get; set; }

        [StringLength(30)]
        public string ItemDimensionNumber { get; set; }

        [StringLength(5)]
        public string BrandTypeId { get; set; }

        [StringLength(30)]
        public string BrandTypeName { get; set; }

        [StringLength(5)]
        public string ColorSizeId { get; set; }

        [StringLength(30)]
        public string ColorSizeName { get; set; }

        [Required]
        public decimal Qty { get; set; }

        [StringLength(10)]
        public string Uom { get; set; }

        [StringLength(15)]
        public string Priority { get; set; }

        [StringLength(50)]
        public string Reason { get; set; }

        [Column(TypeName = "text")]
        public string Remarks { get; set; }

        [StringLength(200)]
        public string AttachmentPath { get; set; }

        [StringLength(50)]
        public string ItemStatus { get; set; }

        [StringLength(15)]
        public string SyncStatus { get; set; } = Commons.Enums.SyncStatus.Not_Sync.GetDescription();

        public DateTime? CreatedDate { get; set; } = DateTime.Now;

        [StringLength(30)]
        public string CreatedBy { get; set; } = Auth.Instance.personalname;

        public DateTime? LastModifiedDate { get; set; }

        [StringLength(30)]
        public string LastModifiedBy { get; set; }

        public bool IsHidden { get; set; } = false;
    }
}
