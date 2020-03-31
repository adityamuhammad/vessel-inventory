namespace VesselInventory.DTO
{
    public class RequestFormShipBargeDTO
    {
        public string rf_number { get; set; }
        public int ship_initial_id { get; set; }
        public int ship_id { get; set; }
        public int barge_id { get; set; }
        public string ship_name { get; set; }
        public string barge_name { get; set; }
        public string ship_code { get; set; }
        public string barge_code { get; set; }
    }
}
