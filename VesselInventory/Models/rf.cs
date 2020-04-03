namespace VesselInventory.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;
    using VesselInventory.Commons.Enums;

    [Table("rf")]
    public partial class RF
    {
        [Key]
        public int rf_id { get; set; }

        [Required]
        [StringLength(25)]
        public string rf_number { get; set; }

        [Column(TypeName = "date")]
        public DateTime? rf_date { get; set; }

        [StringLength(25)]
        [Required]
        public string project_number { get; set; }

        [Required]
        [StringLength(15)]
        public string department_name { get; set; }

        [Column(TypeName = "date")]
        public DateTime target_delivery_date { get; set; }

        [Required]
        [StringLength(15)]
        public string status { get; set; } = Status.DRAFT.GetDescription();

        public int ship_id { get; set; }

        [Required]
        [StringLength(30)]
        public string ship_name { get; set; }

        [Column(TypeName = "text")]
        public string notes { get; set; }

        [StringLength(15)]
        public string sync_status { get; set; } = SyncStatus.NOT_SYNC.GetDescription();

        public DateTime? created_date { get; set; }

        [StringLength(30)]
        public string created_by { get; set; }

        public DateTime? last_modified_date { get; set; }

        [StringLength(30)]
        public string last_modified_by { get; set; }
    }
}
