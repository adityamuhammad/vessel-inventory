namespace VesselInventory.Dto
{
    public class OnHandDto
    {
        public int ItemId { get; set; }
        public string BrandTypeId { get; set; }
        public string ColorSizeId { get; set; }
        public string ItemDimensionNumber { get; set; }
        public string ItemDescriptions { get; set; }
        public string ItemGroupName { get; set; }
        public decimal OnOrder { get; set; }
        public decimal InStock { get; set; }
        public string Uom { get; set; }
    }
}
