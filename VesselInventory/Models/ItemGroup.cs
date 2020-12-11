namespace VesselInventory.Models
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("ItemGroup")]
    public partial class ItemGroup
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int ItemGroupId { get; set; }

        [Required]
        public string ItemGroupName { get; set; }

        [Required]
        public string ItemGroupType { get; set; }

        [Required]
        public string SyncStatus { get; set; }
    }
}
