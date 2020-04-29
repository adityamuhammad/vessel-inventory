using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VesselInventory.Models
{
    [Table("ship_initial")]
    public class ShipInitial
    {
        [Key]
        public int ship_initial_id { get; set; }
        [Required]
        public int ship_id { get; set; }
        [Required]
        public int barge_id { get; set; }
    }
}
