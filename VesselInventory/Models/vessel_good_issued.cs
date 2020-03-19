namespace VesselInventory.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class vessel_good_issued
    {
        [Key]
        public int vessel_good_issued_id { get; set; }

        [Required]
        [StringLength(25)]
        public string vessel_good_issued_number { get; set; }

        [Column(TypeName = "date")]
        public DateTime vessel_good_issued_date { get; set; }

        public int ship_id { get; set; }

        [Required]
        [StringLength(30)]
        public string ship_name { get; set; }

        [Column(TypeName = "text")]
        [Required]
        public string notes { get; set; }

        [Required]
        [StringLength(15)]
        public string sync_status { get; set; }

        public DateTime created_date { get; set; }

        [Required]
        [StringLength(30)]
        public string created_by { get; set; }

        public DateTime? last_modified_date { get; set; }

        [StringLength(30)]
        public string last_modified_by { get; set; }
    }
}
