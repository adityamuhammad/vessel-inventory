namespace VesselInventory.Models
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("Item")]
    public partial class Item
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int ItemId { get; set; }

        public int ItemGroupId { get; set; }

        [Required]
        public string ItemName { get; set; }

        [Required]
        public string Uom { get; set; }

        [Required]
        public string SyncStatus { get; set; }
    }
}
