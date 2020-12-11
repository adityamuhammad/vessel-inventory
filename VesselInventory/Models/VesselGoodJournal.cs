namespace VesselInventory.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("VesselGoodJournal")]
    public partial class VesselGoodJournal
    {
        public int VesselGoodJournalId { get; set; }

        [Required]
        public string DocumentReference { get; set; }

        [Required]
        public string DocumentType { get; set; }

        public int ItemId { get; set; }

        public int ItemGroupId { get; set; }

        [Required]
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
        [StringLength(10)]
        public string InOut { get; set; }

        public int ShipId { get; set; }

        public string Uom { get; set; }
        [Required]
        [StringLength(30)]
        public string ShipName { get; set; }

        [Column(TypeName = "date")]
        public DateTime VesselGoodJournalDate { get; set; }

        [Required]
        [StringLength(30)]
        public string SyncStatus { get; set; }

        public bool IsHidden { get; set; }
    }
}
