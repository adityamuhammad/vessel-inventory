namespace VesselInventory.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("DocSequence")]
    public partial class DocSequence
    {
        public int DocSequenceId { get; set; }

        [StringLength(50)]
        public string DocSequenceName { get; set; }

        [StringLength(30)]
        public string DocSequenceNumber { get; set; }

        [StringLength(30)]
        public string DocSequenceFormat { get; set; }
    }
}
