using System.Collections.Generic;
using VesselInventory.Models;

namespace VesselInventory.Repository
{
    public interface IVesselGoodReceiveItemRepository
    {
        IEnumerable<VesselGoodReceiveItem> GetGoodReceiveItem(int vesselGoodReceiveId);
    }
}
