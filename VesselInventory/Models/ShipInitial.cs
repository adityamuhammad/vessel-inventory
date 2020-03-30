namespace VesselInventory.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class ShipInitial
    {
        [Key]
        public int ship_initial_id { get; set; }

        public int ship_id { get; set; }

        public int barge_id { get; set; }
    }
}
