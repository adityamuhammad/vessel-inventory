namespace VesselInventory.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("vessel_good_receive")]
    public partial class VesselGoodReceive
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int vessel_good_receive_id { get; set; }

        [Required]
        [StringLength(25)]
        public string good_issued_number { get; set; }

        [Required]
        [StringLength(25)]
        public string vessel_good_receive_number { get; set; }

        [Column(TypeName = "date")]
        public DateTime vessel_good_receive_date { get; set; }

        public int ship_id { get; set; }

        [Required]
        [StringLength(30)]
        public string ship_name { get; set; }

        public int barge_id { get; set; }

        [Required]
        [StringLength(30)]
        public string barge_name { get; set; }

        [Required]
        [StringLength(10)]
        public string sync_status { get; set; }
    }
}
