using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VesselInventory.DTO
{
    public class ItemGroupDimensionDTO
    {
        public int item_id { get; set; }
        public string item_name { get; set; }
        public string uom { get; set; }
        public string item_dimension_number { get; set; }
        public string brand_type_id { get; set; }
        public string brand_type_name { get; set; }
        public string color_size_id { get; set; }
        public string color_size_name { get; set; }
        public int item_group_id { get; set; }
        public string item_group_name { get; set; }
    }
}
