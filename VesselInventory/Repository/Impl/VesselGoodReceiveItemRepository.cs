using System.Collections.Generic;
using System.Linq;
using VesselInventory.Models;

namespace VesselInventory.Repository.Impl
{
    public class VesselGoodReceiveItemRepository 
        : IVesselGoodReceiveItemRepository
    {
        public IEnumerable<VesselGoodReceiveItem> GetGoodReceiveItem(int vesselGoodReceiveId)
        {
            using (var context = new AppVesselInventoryContext())
            {
                return (from item in context.VesselGoodReceiveItem
                        where item.VesselGoodReceiveId == vesselGoodReceiveId 
                        && item.IsHidden == false
                        orderby item.CreatedDate descending
                        select item)
                        .ToList();
            }
        }
    }
}
