namespace VesselInventory.Filters
{
    public class PageFilter
    {
        public string Search { get; set; }
        public int PageNum { get; set; }
        public int NumRows { get; set; }
        public string SortName { get; set; }
        public string SortType { get; set; }
    }
}
