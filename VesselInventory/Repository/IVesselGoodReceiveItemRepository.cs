using System.Collections.Generic;
using System.Linq;
using VesselInventory.Models;

namespace VesselInventory.Repository
{
    public interface IVesselGoodReceiveItemRepository
    {
        IEnumerable<VesselGoodReceiveItem> GetGoodReceiveItem(int vesselGoodReceiveId);
    }
    public class VesselGoodReceiveItemRepository 
        : IVesselGoodReceiveItemRepository
    {
        public IEnumerable<VesselGoodReceiveItem> GetGoodReceiveItem(int vesselGoodReceiveId)
        {
            using (var context = new VesselInventoryContext())
            {
                return (from item in context.vessel_good_receive_item
                        where item.vessel_good_receive_id == vesselGoodReceiveId && 
                        item.is_hidden == false select item)
                        .ToList();
            }
        }
    }
}
