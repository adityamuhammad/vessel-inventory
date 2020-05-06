using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VesselInventory.Dto
{
    public class LastRequestItemDto
    {
        public decimal LastRequestQty { get; set; }
        public DateTime? LastRequestDate { get; set; }
        public decimal LastSupplyQty { get; set; }
        public DateTime? LastSupplyDate { get; set; }
        public decimal Rob { get; set; }
    }
}
