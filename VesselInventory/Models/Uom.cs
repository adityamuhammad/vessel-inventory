namespace VesselInventory.Models
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("Uom")]
    public partial class Uom
    {
        public int UomId { get; set; }

        [Required]
        public string UomName { get; set; }
    }
}
