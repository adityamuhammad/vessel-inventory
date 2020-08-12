using System.Collections.Generic;
using VesselInventory.Models;

namespace VesselInventory.Repository
{
    public interface IVesselGoodIssuedRepository : IGenericRepository<VesselGoodIssued>
    {
        IEnumerable<VesselGoodIssued> GetGoodIssuedDataGrid(
            string search, int page, int rows, 
            string sortColumnName, string sortBy);
        int GetGoodIssuedTotalPage(string search, int rows);
        VesselGoodIssued SaveTransaction(VesselGoodIssued vesselGoodReceive);
    }

}
