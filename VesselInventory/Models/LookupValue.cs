namespace VesselInventory.Models
{
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("LookupValue")]
    public partial class LookupValue
    {
        public int LookupValueId { get; set; }

        public string Descriptions { get; set; }

        public string LookupType { get; set; }

        public string Abbreviation { get; set; }
    }
}
