namespace VesselInventory.Dto
{
    public class ItemPendingDto
    {
        public int RequestFormItemId { get; set; }
        public int ItemId { get; set; }
        public string ItemDescriptions { get; set; }
        public decimal Qty { get; set; }
        public string Uom { get; set; }
        public string Priority { get; set; }
        public string AttachmentPath { get; set; }
        public string AttachmentStatus { get; set; }
        public string RequestFormNumber { get; set; }
    }
}
