namespace VesselInventory.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Ship")]
    public partial class Ship
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int ShipId { get; set; }

        [Required]
        [StringLength(30)]
        public string ShipName { get; set; }

        [Required]
        [StringLength(10)]
        public string ShipCode { get; set; }

        public bool IsBarge { get; set; }
    }
}