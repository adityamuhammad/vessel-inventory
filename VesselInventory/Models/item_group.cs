namespace VesselInventory.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class item_group
    {
        [Key]
        public int item_group_id { get; set; }

        [Required]
        [StringLength(50)]
        public string item_group_name { get; set; }

        [Required]
        [StringLength(10)]
        public string item_group_type { get; set; }

        [Required]
        [StringLength(15)]
        public string sync_status { get; set; }
    }
}
