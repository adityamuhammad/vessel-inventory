using System.Collections.Generic;
using VesselInventory.Filters;
using VesselInventory.Models;

namespace VesselInventory.Repository
{
    public interface IVesselGoodReturnRepository 
        : IGenericRepository<VesselGoodReturn>
    {
        IEnumerable<VesselGoodReturn> GetGoodReturnDataGrid(PageFilter pageFilter);
        int GetGoodReturnTotalPage(PageFilter pageFilter);
        VesselGoodReturn SaveTransaction(VesselGoodReturn vesselGoodReturn);
    }

}
