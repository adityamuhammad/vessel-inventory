namespace VesselInventory.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class VesselGoodJournal
    {
        [Key]
        public int VesselGoodJournalId { get; set; }

        [Column(TypeName = "date")]
        public DateTime VesselGoodJournalDate { get; set; }

        [Required]
        [StringLength(10)]
        public string InOut { get; set; }

        [Required]
        [StringLength(25)]
        public string DocumentReference { get; set; }

        [Required]
        [StringLength(10)]
        public string DocumentType { get; set; }

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

        public decimal Qty { get; set; }

        public int ShipId { get; set; }

        [Required]
        [StringLength(30)]
        public string ShipName { get; set; }
    }
}
