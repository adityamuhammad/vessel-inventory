namespace VesselInventory.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class UserVessel
    {
        public int UserId { get; set; }

        public string Username { get; set; }

        public string Password { get; set; }

        public string DepartmentName { get; set; }
        public int ShipId { get; set; }

    }
}
