namespace VesselInventory.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class vessel_good_journal
    {
        [Key]
        public int vessel_good_journal_id { get; set; }

        [Column(TypeName = "date")]
        public DateTime vessel_good_journal_date { get; set; }

        [Required]
        [StringLength(1)]
        public string in_out { get; set; }

        [Required]
        [StringLength(25)]
        public string document_reference { get; set; }

        [Required]
        [StringLength(10)]
        public string document_type { get; set; }

        public int item_id { get; set; }

        public int item_group_id { get; set; }

        [Required]
        [StringLength(25)]
        public string item_dimension_number { get; set; }

        [Required]
        [StringLength(50)]
        public string item_name { get; set; }

        [Required]
        [StringLength(5)]
        public string brand_type_id { get; set; }

        [Required]
        [StringLength(30)]
        public string brand_type_name { get; set; }

        [Required]
        [StringLength(5)]
        public string color_size_id { get; set; }

        [Required]
        [StringLength(30)]
        public string color_size_name { get; set; }

        public decimal qty { get; set; }

        public int ship_id { get; set; }

        [Required]
        [StringLength(30)]
        public string ship_name { get; set; }
    }
}
