using System;

namespace VesselInventory.Dto
{
    public class ItemStatusDto
    {
        public int ItemId { get; set; }
        public string ItemDescriptions { get; set; }
        public string ItemDimensionNumber { get; set; }
        public string BrandTypeId { get; set; }
        public string ColorSizeId { get; set; }
        public decimal Qty { get; set; }
        public string Uom { get; set; }
        public string ItemGroupName { get; set; }
        public string Priority { get; set; }
        public string RequestFormNumber { get; set; }
        public string ItemStatus { get; set; }
        public string DepartmentName { get; set; }
        public DateTime TargetDeliveryDate { get; set; }
        public string SyncStatus { get; set; }
    }
}
