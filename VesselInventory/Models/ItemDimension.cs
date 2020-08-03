namespace VesselInventory.Models
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("ItemDimension")]
    public partial class ItemDimension
    {
        public int ItemDimensionId { get; set; }

        [Required]
        [StringLength(30)]
        public string ItemDimensionNumber { get; set; }

        public int ItemId { get; set; }

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
        [StringLength(15)]
        public string SyncStatus { get; set; }
    }
}
