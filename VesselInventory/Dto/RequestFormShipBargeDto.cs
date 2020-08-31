namespace VesselInventory.Dto
{
    public class RequestFormShipBargeDto
    {
        public string RequestFormNumber { get; set; }
        public int ShipInitialId { get; set; }
        public int ShipId { get; set; }
        public int BargeId { get; set; }
        public string ShipName { get; set; }
        public string Bargename { get; set; }
        public string ShipCode { get; set; }
        public string BargeCode { get; set; }
    }
}
