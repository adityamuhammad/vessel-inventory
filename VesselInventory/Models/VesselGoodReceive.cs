namespace VesselInventory.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;
    using VesselInventory.Commons.Enums;

    [Table("vessel_good_receive")]
    public partial class VesselGoodReceive
    {
        [Key]
        public int vessel_good_receive_id { get; set; }

        [Required]
        [StringLength(25)]
        public string vessel_good_receive_number { get; set; }

        [Required]
        [StringLength(25)]
        public string good_issued_number { get; set; }

        [Column(TypeName = "date")]
        public DateTime vessel_good_receive_date { get; set; } = DateTime.Now;

        public int ship_id { get; set; }

        [Required]
        [StringLength(30)]
        public string ship_name { get; set; }
        public string ship_code { get; set; }

        public int barge_id { get; set; }
        public string barge_code { get; set; }

        [Required]
        [StringLength(30)]
        public string barge_name { get; set; }

        public DateTime created_date { get; set; } = DateTime.Now;

        [Required]
        [StringLength(30)]
        public string created_by { get; set; } = "Aditya";

        public DateTime? last_modified_date { get; set; }

        [StringLength(30)]
        public string last_modified_by { get; set; }

        [Required]
        [StringLength(10)]
        public string sync_status { get; set; } = SyncStatus.NOT_SYNC.GetDescription();
        public bool is_hidden { get; set; } = false;
    }
}
