namespace VesselInventory.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;
    using VesselInventory.Commons.Enums;
    using VesselInventory.Utility;

    [Table("vessel_good_receive_item_reject")]
    public partial class VesselGoodReceiveItemReject
    {
        [Key]
        public int vessel_good_receive_item_reject_id { get; set; }

        public int vessel_good_receive_id { get; set; }

        [Required]
        [StringLength(25)]
        public string rf_number { get; set; }

        public int item_id { get; set; }

        public int item_group_id { get; set; }

        [Required]
        [StringLength(50)]
        public string item_name { get; set; }

        [Required]
        [StringLength(25)]
        public string item_dimension_number { get; set; }

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

        [Required]
        [StringLength(10)]
        public string uom { get; set; }

        public decimal qty { get; set; }

        public DateTime created_date { get; set; } = DateTime.Now;

        [Required]
        [StringLength(30)]
        public string created_by { get; set; } = Auth.Instance.username;

        public DateTime? last_modified_date { get; set; }

        [StringLength(30)]
        public string last_modified_by { get; set; }

        [StringLength(15)]
        public string sync_status { get; set; } = SyncStatus.Not_Sync.GetDescription();
        public bool is_hidden { get; set; } = false;
    }
}
