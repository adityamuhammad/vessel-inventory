using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VesselInventory.Models
{
    public class ShipInitial
    {
        [Key]
        public int ShipInitialId { get; set; }
        [Required]
        public int ShipId { get; set; }
        [Required]
        public int BargeId { get; set; }
    }
}
