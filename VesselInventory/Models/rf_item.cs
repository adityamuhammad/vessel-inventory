namespace VesselInventory.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class rf_item
    {
        [Key]
        public int rf_item_id { get; set; }

        public int? rf_id { get; set; }

        public int? item_id { get; set; }

        [StringLength(50)]
        public string item_name { get; set; }

        public int? item_group_id { get; set; }

        [StringLength(30)]
        public string item_dimension_number { get; set; }

        [StringLength(5)]
        public string brand_type_id { get; set; }

        [StringLength(30)]
        public string brand_type_name { get; set; }

        [StringLength(5)]
        public string color_size_id { get; set; }

        [StringLength(30)]
        public string color_size_name { get; set; }

        public decimal? qty { get; set; }

        [StringLength(10)]
        public string uom { get; set; }

        [StringLength(15)]
        public string priority { get; set; }

        [StringLength(50)]
        public string reason { get; set; }

        [Column(TypeName = "text")]
        public string remarks { get; set; }

        [StringLength(200)]
        public string attachment_path { get; set; }

        [StringLength(15)]
        public string sync_status { get; set; }

        public DateTime? created_date { get; set; }

        [StringLength(30)]
        public string created_by { get; set; }

        public DateTime? last_modified_date { get; set; }

        [StringLength(30)]
        public string last_modified_by { get; set; }
    }
}
