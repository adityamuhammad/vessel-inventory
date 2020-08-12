using System.Collections.Generic;
using VesselInventory.Models;

namespace VesselInventory.Repository
{
    public interface IVesselGoodReceiveRepository : IGenericRepository<VesselGoodReceive>
    {
        IEnumerable<VesselGoodReceive> GetGoodReceiveDataGrid(
            string search, int page, int rows, 
            string sortColumnName, string sortBy);
        int GetGoodReceiveTotalPage(string search, int rows);
        VesselGoodReceive SaveTransaction(VesselGoodReceive vesselGoodReceive);
    }

}
