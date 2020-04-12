using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VesselInventory.Models
{
    [Table("uom")]
    public class Uom
    {
        [Key]
        public int uom_id { get; set; }
        public string uom_name { get; set; }
    }
}
