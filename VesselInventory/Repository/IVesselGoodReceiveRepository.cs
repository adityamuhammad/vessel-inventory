using System.Collections.Generic;
using VesselInventory.Filters;
using VesselInventory.Models;

namespace VesselInventory.Repository
{
    public interface IVesselGoodReceiveRepository : IGenericRepository<VesselGoodReceive>
    {
        IEnumerable<VesselGoodReceive> GetGoodReceiveDataGrid(PageFilter pageFilter);
        int GetGoodReceiveTotalPage(PageFilter pageFilter);
        VesselGoodReceive SaveTransaction(VesselGoodReceive vesselGoodReceive);
    }

}
