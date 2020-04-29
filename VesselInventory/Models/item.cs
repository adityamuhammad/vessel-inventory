namespace VesselInventory.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Item
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int ItemId { get; set; }

        public int ItemGroupId { get; set; }

        [Required]
        [StringLength(50)]
        public string ItemName { get; set; }

        [Required]
        [StringLength(10)]
        public string Uom { get; set; }

        [Required]
        [StringLength(15)]
        public string SyncStatus { get; set; }
    }
}
