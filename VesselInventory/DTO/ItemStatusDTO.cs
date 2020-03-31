using System;

namespace VesselInventory.DTO
{
    public class ItemStatusDTO
    {
        public int item_id { get; set; }
        public string item_description { get; set; }
        public decimal? qty { get; set; }
        public string uom { get; set; }
        public string item_group_name { get; set; }
        public string priority { get; set; }
        public string rf_number { get; set; }
        public string item_status { get; set; }
        public string department_name { get; set; }
        public DateTime target_delivery_date { get; set; }
        public string sync_status { get; set; }
    }
}
