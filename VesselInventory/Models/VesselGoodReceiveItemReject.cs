namespace VesselInventory.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("VesselGoodReceiveItemReject")]
    public partial class VesselGoodReceiveItemReject
    {
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

        public virtual VesselGoodReceive VesselGoodReceive { get; set; }
    }
}
