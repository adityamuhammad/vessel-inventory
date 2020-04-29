namespace VesselInventory.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("LookupValue")]
    public partial class LookupValue
    {
        public int LookupValueId { get; set; }

        [StringLength(50)]
        public string Descriptions { get; set; }

        [StringLength(50)]
        public string LookupType { get; set; }

        [StringLength(5)]
        public string Abbreviation { get; set; }
    }
}
