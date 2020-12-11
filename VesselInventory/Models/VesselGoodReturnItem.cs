namespace VesselInventory.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("VesselGoodReturnItem")]
    public partial class VesselGoodReturnItem
    {
        public int VesselGoodReturnItemId { get; set; }

        public int VesselGoodReturnId { get; set; }

        public int ItemId { get; set; }

        public int ItemGroupId { get; set; }

        [Required]
        [StringLength(25)]
        public string ItemDimensionNumber { get; set; }

        [Required]
        public string ItemName { get; set; }

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
        [StringLength(30)]
        public string Reason { get; set; }

        [Required]
        [StringLength(10)]
        public string Uom { get; set; }

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

        public virtual VesselGoodReturn VesselGoodReturn { get; set; }
    }
}
