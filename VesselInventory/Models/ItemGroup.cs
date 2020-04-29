namespace VesselInventory.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("ItemGroup")]
    public partial class ItemGroup
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int ItemGroupId { get; set; }

        [Required]
        [StringLength(50)]
        public string ItemGroupName { get; set; }

        [Required]
        [StringLength(10)]
        public string ItemGroupType { get; set; }

        [Required]
        [StringLength(15)]
        public string SyncStatus { get; set; }
    }
}
