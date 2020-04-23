namespace VesselInventory.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("lookup_value")]
    public partial class LookupValue
    {
        [Key]
        public int lookup_value_id { get; set; }

        [StringLength(50)]
        public string description { get; set; }

        [StringLength(50)]
        public string lookup_type { get; set; }

        [StringLength(5)]
        public string abbreviation { get; set; }

    }
}
