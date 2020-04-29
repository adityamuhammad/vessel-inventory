using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VesselInventory.Models
{
    public class Uom
    {
        [Key]
        public int UomId { get; set; }
        public string UomName { get; set; }
    }
}
