using System.Collections.Generic;
using System.Linq;
using VesselInventory.Dto;
using VesselInventory.Filters;
using VesselInventory.Models;

namespace VesselInventory.Repository.Impl
{
    public class OnHandRepository : IOnHandRepository
    {
        public IEnumerable<OnHandDto> GetOnHandDataGrid(PageFilter pageFilter)
        {
            using(var context = new AppVesselInventoryContext())
            {
                return context.Database.SqlQuery<OnHandDto>(
                    "usp_OnHand_GetItemOnHandList @p0, @p1, @p2", 
                    parameters: new object[] { pageFilter.Search, pageFilter.PageNum, pageFilter.NumRows} 
                ).ToList();
            }
        }

        public int GetOnHandTotalPage(PageFilter pageFilter)
        {
            using (var context = new AppVesselInventoryContext())
            {
                return context.Database.SqlQuery<int>(
                    "usp_OnHand_GetItemOnHandPages @p0, @p1",
                    parameters: new object[] { pageFilter.Search, pageFilter.NumRows }
                ).Single();
            }
        }
    }
}
