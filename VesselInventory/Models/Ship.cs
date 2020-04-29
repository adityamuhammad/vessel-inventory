using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VesselInventory.Models
{
    [Table("ship")]
    public partial class Ship
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int ship_id { get; set; }


        [Required]
        [StringLength(30)]
        public string ship_name { get; set; }

        [Required]
        [StringLength(10)]
        public string ship_code { get; set; }

        [StringLength(20)]
        public string ship_alias_axapta{ get; set; }
        public bool is_barge { get; set; }
    }
}
