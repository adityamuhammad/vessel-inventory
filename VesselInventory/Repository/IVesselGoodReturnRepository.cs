using System.Collections.Generic;
using VesselInventory.Models;

namespace VesselInventory.Repository
{
    public interface IVesselGoodReturnRepository 
        : IGenericRepository<VesselGoodReturn>
    {
        IEnumerable<VesselGoodReturn> GetGoodReturnDataGrid(
            string search, int page, int rows, 
            string sortColumnName, string sortBy);
        int GetGoodReturnTotalPage(string search, int rows);
        VesselGoodReturn SaveTransaction(VesselGoodReturn vesselGoodReturn);
    }

}
