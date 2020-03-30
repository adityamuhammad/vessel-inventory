namespace VesselInventory.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("ship")]
    public partial class Ship
    {
        [Key]
        public int ship_id { get; set; }

        [Required]
        [StringLength(30)]
        public string ship_name { get; set; }

        [Required]
        [StringLength(10)]
        public string ship_code { get; set; }

        [Required]
        [StringLength(20)]
        public string ship_alias_axapta { get; set; }

        public bool is_barge { get; set; }

        public bool is_hidden { get; set; }
    }
}
