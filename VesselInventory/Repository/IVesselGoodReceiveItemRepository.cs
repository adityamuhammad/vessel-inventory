using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VesselInventory.Models;

namespace VesselInventory.Repository
{
    public interface IVesselGoodReceiveItemRepository
    {
        IEnumerable<VesselGoodReceiveItem> GetGoodReceiveItem(int vesselGoodReceiveId);
    }
    public class VesselGoodReceiveItemRepository : IVesselGoodReceiveItemRepository
    {
        public IEnumerable<VesselGoodReceiveItem> GetGoodReceiveItem(int vesselGoodReceiveId)
        {
            using (var context = new VesselInventoryContext())
            {
                return context.vessel_good_receive_item.SqlQuery(
                    "usp_VesselGoodReceiveItem_GetVesselGoodReceiveItemList @p0",
                    parameters: vesselGoodReceiveId.ToString()).ToList();
            }
        }
    }
}
