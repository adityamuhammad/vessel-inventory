namespace VesselInventory.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;
    using VesselInventory.Commons.Enums;
    using VesselInventory.Utility;

    [Table("vessel_good_return")]
    public partial class VesselGoodReturn
    {
        [Key]
        public int vessel_good_return_id { get; set; }

        [Required]
        [StringLength(25)]
        public string vessel_good_return_number { get; set; }

        [Column(TypeName = "date")]
        public DateTime vessel_good_return_date { get; set; }

        public int ship_id { get; set; }

        [Required]
        [StringLength(30)]
        public string ship_name { get; set; }

        [Column(TypeName = "text")]
        public string notes { get; set; }

        [Required]
        [StringLength(15)]
        public string sync_status { get; set; } = SyncStatus.Not_Sync.GetDescription();

        public DateTime created_date { get; set; } = DateTime.Now;

        [Required]
        [StringLength(30)]
        public string created_by { get; set; } = Auth.Instance.personalname;

        public DateTime? last_modified_date { get; set; }

        [StringLength(30)]
        public string last_modified_by { get; set; }
        public bool is_hidden { get; set; } = false;
    }
}
