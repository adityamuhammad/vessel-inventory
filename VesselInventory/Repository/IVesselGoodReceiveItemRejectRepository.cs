using System.Collections.Generic;
using System.Linq;
using VesselInventory.Models;
namespace VesselInventory.Repository
{
    public interface IVesselGoodReceiveItemRejectRepository
    {
        VesselGoodReceiveItemReject GetById(int id);
        VesselGoodReceiveItemReject Save(VesselGoodReceiveItemReject vesselGoodReceiveItemReject);
        VesselGoodReceiveItemReject Update(int id,VesselGoodReceiveItemReject vesselGoodReceiveItemReject);
        int Delete(int id);
        IEnumerable<VesselGoodReceiveItemReject> GetGoodReceiveItemRejected(int vesselGoodReceiveId);
    }

    public class VesselGoodReceiveItemRejectRepository :
        Repository<VesselGoodReceiveItemReject>,
        IVesselGoodReceiveItemRejectRepository
    {
        public IEnumerable<VesselGoodReceiveItemReject> GetGoodReceiveItemRejected(int vesselGoodReceiveId)
        {
            using (var context = new VesselInventoryContext())
            {
                return context.vessel_good_receive_item_reject.SqlQuery(
                    "usp_VesselGoodReceiveItemReject_GetVesselGoodReceiveItemRejectedList @p0",
                    parameters: vesselGoodReceiveId.ToString()).ToList();
            }
        }
    }
}
