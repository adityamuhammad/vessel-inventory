using System.Collections.Generic;
using System.Linq;
using System.Transactions;
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

    public class VesselGoodIssuedRepository 
        : GenericRepository<VesselGoodIssued>
        , IVesselGoodIssuedRepository
    {
        public IEnumerable<VesselGoodIssued> GetGoodIssuedDataGrid(
            string search, int page, int rows, 
            string sortColumnName, string sortBy)
        {
            using (var context = new VesselInventoryContext())
            {
                return context.vessel_good_issued.SqlQuery (
                        "usp_VesselGoodIssued_GetGoodIssuedList @p0, @p1, @p2, @p3, @p4",
                        parameters: new object[] { search, page, rows, sortColumnName, sortBy }).ToList();
            }
        }

        public int GetGoodIssuedTotalPage(string search, int rows)
        {
            using (var context = new VesselInventoryContext())
            {
                return context.Database.SqlQuery<int>(
                        "usp_VesselGoodIssued_GetGoodIssuedPages @p0, @p1",
                        parameters: new object[] { search, rows }).Single();
            }
        }

        public VesselGoodIssued SaveTransaction(VesselGoodIssued vesselGoodIssued)
        {
            using(var scope = new TransactionScope())
            {
                Save(vesselGoodIssued);
                using(var context = new VesselInventoryContext())
                {
                    context.Database.ExecuteSqlCommand("usp_DocSequence_IncrementSeqNumber 3");
                }
                scope.Complete();
                return vesselGoodIssued;
            }
        }
    }
}
