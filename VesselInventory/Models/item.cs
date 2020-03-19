namespace VesselInventory.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("item")]
    public partial class item
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int item_id { get; set; }

        public int item_group_id { get; set; }

        [Required]
        [StringLength(50)]
        public string item_name { get; set; }

        [Required]
        [StringLength(10)]
        public string uom { get; set; }

        [Required]
        [StringLength(15)]
        public string sync_status { get; set; }
    }
}
