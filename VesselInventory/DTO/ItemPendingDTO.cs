namespace VesselInventory.Dto
{
    public class ItemPendingDto
    {
        public int rf_item_id { get; set; }
        public int item_id { get; set; }
        public string item_description { get; set; }
        public decimal qty { get; set; }
        public string uom { get; set; }
        public string priority { get; set; }
        public string attachment_path { get; set; }
        public string attachment_status { get; set; }
        public string rf_number { get; set; }
    }
}
