using System.Collections.Generic;
using VesselInventory.Models;
namespace VesselInventory.Repository
{
    public interface IVesselGoodReceiveItemRejectRepository : IGenericRepository<VesselGoodReceiveItemReject>
    {
        IEnumerable<VesselGoodReceiveItemReject> GetGoodReceiveItemRejected(int vesselGoodReceiveId);
    }

}
