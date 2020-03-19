namespace VesselInventory.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("uom")]
    public partial class uom
    {
        [Key]
        public int uom_id { get; set; }

        [Required]
        [StringLength(10)]
        public string uom_name { get; set; }
    }
}
