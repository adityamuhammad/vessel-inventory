namespace VesselInventory.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class doc_sequence
    {
        [Key]
        public int doc_sequence_id { get; set; }

        [StringLength(50)]
        public string doc_sequence_name { get; set; }

        [StringLength(30)]
        public string doc_sequence_number { get; set; }

        [StringLength(30)]
        public string doc_sequence_format { get; set; }
    }
}
