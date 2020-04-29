namespace VesselInventory.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("User")]
    public partial class User
    {
        public int UserId { get; set; }

        [Required]
        [StringLength(30)]
        public string Username { get; set; }

        [Required]
        public string Password { get; set; }

        public int ShipId { get; set; }

        public int DepartmentId { get; set; }
    }
}
