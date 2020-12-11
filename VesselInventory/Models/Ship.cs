namespace VesselInventory.Models
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("Ship")]
    public partial class Ship
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int ShipId { get; set; }

        [Required]
        public string ShipName { get; set; }

        [Required]
        public string ShipCode { get; set; }

        public bool IsBarge { get; set; }
    }
}
