using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
    }

    public class VesselGoodReturnRepository
        : GenericRepository<VesselGoodReturn>
        , IVesselGoodReturnRepository
    {
        public IEnumerable<VesselGoodReturn> GetGoodReturnDataGrid(
            string search, int page, int rows, 
            string sortColumnName, string sortBy)
        {
            using (var context = new VesselInventoryContext())
            {
                return context.vessel_good_return.SqlQuery (
                        "usp_VesselGoodReturn_GetGoodReturnList @p0, @p1, @p2, @p3, @p4",
                        parameters: new object[] { search, page, rows, sortColumnName, sortBy }).ToList();
            }
        }

        public int GetGoodReturnTotalPage(string search, int rows)
        {
            using (var context = new VesselInventoryContext())
            {
                return context.Database.SqlQuery<int>(
                        "usp_VesselGoodReturn_GetGoodReturnPages @p0, @p1",
                        parameters: new object[] { search, rows }).Single();
            }
        }
    }
}
