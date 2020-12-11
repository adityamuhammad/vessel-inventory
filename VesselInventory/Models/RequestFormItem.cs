namespace VesselInventory.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("RequestFormItem")]
    public partial class RequestFormItem
    {
        public int RequestFormItemId { get; set; }

        public int RequestFormId { get; set; }

        public int ItemId { get; set; }

        [Required]
        public string ItemName { get; set; }

        public int ItemGroupId { get; set; }

        [Required]
        public string ItemDimensionNumber { get; set; }

        [Required]
        public string BrandTypeId { get; set; }

        [Required]
        public string BrandTypeName { get; set; }

        [Required]
        public string ColorSizeId { get; set; }

        [Required]
        public string ColorSizeName { get; set; }

        public decimal Qty { get; set; }

        [Required]
        public string Uom { get; set; }

        [Required]
        public string Priority { get; set; }

        [Required]
        public string Reason { get; set; }

        [Column(TypeName = "text")]
        public string Remarks { get; set; }

        [StringLength(200)]
        public string AttachmentPath { get; set; }

        [StringLength(50)]
        public string ItemStatus { get; set; }
        public decimal LastRequestQty { get; set; }

        [Column(TypeName = "date")]
        public DateTime? LastRequestDate { get; set; }
        public decimal LastSupplyQty { get; set; }

        [Column(TypeName = "date")]
        public DateTime? LastSupplyDate { get; set; }
        public decimal Rob { get; set; }

        [Required]
        public string SyncStatus { get; set; }

        public DateTime CreatedDate { get; set; }

        public string CreatedBy { get; set; }

        public DateTime? LastModifiedDate { get; set; }

        public string LastModifiedBy { get; set; }

        public bool IsHidden { get; set; }
        public bool IsDocumentPending { get; set; }

        public virtual RequestForm RequestForm { get; set; }
    }
}
