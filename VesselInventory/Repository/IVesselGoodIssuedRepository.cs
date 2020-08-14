using System.Collections.Generic;
using VesselInventory.Filters;
using VesselInventory.Models;

namespace VesselInventory.Repository
{
    public interface IVesselGoodIssuedRepository : IGenericRepository<VesselGoodIssued>
    {
        IEnumerable<VesselGoodIssued> GetGoodIssuedDataGrid(PageFilter pageFilter);
        int GetGoodIssuedTotalPage(PageFilter pageFilter);
        VesselGoodIssued SaveTransaction(VesselGoodIssued vesselGoodIssued);
    }

}
