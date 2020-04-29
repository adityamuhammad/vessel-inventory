namespace VesselInventory.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("RequestFormItem")]
    public partial class RequestFormItem
    {
        public int RequestFormItemId { get; set; }

        public int RequestFormId { get; set; }

        public int ItemId { get; set; }

        [Required]
        [StringLength(50)]
        public string ItemName { get; set; }

        public int ItemGroupId { get; set; }

        [Required]
        [StringLength(30)]
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

        public decimal Qty { get; set; }

        [Required]
        [StringLength(10)]
        public string Uom { get; set; }

        [Required]
        [StringLength(15)]
        public string Priority { get; set; }

        [Required]
        [StringLength(50)]
        public string Reason { get; set; }

        [Column(TypeName = "text")]
        public string Remarks { get; set; }

        [StringLength(200)]
        public string AttachmentPath { get; set; }

        [StringLength(50)]
        public string ItemStatus { get; set; }

        [Required]
        [StringLength(15)]
        public string SyncStatus { get; set; }

        public DateTime CreatedDate { get; set; }

        [StringLength(30)]
        public string CreatedBy { get; set; }

        public DateTime? LastModifiedDate { get; set; }

        [StringLength(30)]
        public string LastModifiedBy { get; set; }

        public bool IsHidden { get; set; }

        public virtual RequestForm RequestForm { get; set; }
    }
}
