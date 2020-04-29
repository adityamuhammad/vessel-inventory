namespace VesselInventory.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("ShipInitial")]
    public partial class ShipInitial
    {
        public int ShipInitialId { get; set; }

        public int ShipId { get; set; }

        public int BargeId { get; set; }
    }
}
