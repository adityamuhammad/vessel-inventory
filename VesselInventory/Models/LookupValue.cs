namespace VesselInventory.Models
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("LookupValue")]
    public partial class LookupValue
    {
        public int LookupValueId { get; set; }

        [StringLength(50)]
        public string Descriptions { get; set; }

        [StringLength(50)]
        public string LookupType { get; set; }

        [StringLength(5)]
        public string Abbreviation { get; set; }
    }
}
