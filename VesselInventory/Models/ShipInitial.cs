namespace VesselInventory.Models
{
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("ShipInitial")]
    public partial class ShipInitial
    {
        public int ShipInitialId { get; set; }

        public int ShipId { get; set; }

        public int BargeId { get; set; }
    }
}
