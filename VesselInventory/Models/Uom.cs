namespace VesselInventory.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Uom")]
    public partial class Uom
    {
        public int UomId { get; set; }

        [Required]
        [StringLength(10)]
        public string UomName { get; set; }
    }
}
