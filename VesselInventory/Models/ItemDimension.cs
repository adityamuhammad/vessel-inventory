namespace VesselInventory.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("item_dimension")]
    public partial class ItemDimension
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int item_dimension_id { get; set; }

        [StringLength(30)]
        public string item_dimension_number { get; set; }

        public int? item_id { get; set; }

        [StringLength(5)]
        public string brand_type_id { get; set; }

        [StringLength(30)]
        public string brand_type_name { get; set; }

        [StringLength(5)]
        public string color_size_id { get; set; }

        [StringLength(30)]
        public string color_size_name { get; set; }

        [StringLength(15)]
        public string sync_status { get; set; }
    }
}
