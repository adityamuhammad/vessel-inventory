namespace VesselInventory.Models
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("ItemDimension")]
    public partial class ItemDimension
    {
        [Required]
        public string ItemDimensionNumber { get; set; }

        public int ItemId { get; set; }

        [Required]
        public string BrandTypeId { get; set; }

        [Required]
        public string BrandTypeName { get; set; }

        [Required]
        public string ColorSizeId { get; set; }

        [Required]
        public string ColorSizeName { get; set; }

        [Required]
        public string SyncStatus { get; set; }
    }
}
