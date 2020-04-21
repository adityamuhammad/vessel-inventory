using System.Collections.Generic;
using System.Linq;
using System.Transactions;
using VesselInventory.Models;

namespace VesselInventory.Repository
{
    public interface IVesselGoodIssuedRepository
    {
        IEnumerable<VesselGoodIssued> GetGoodIssued(string search, int page, int rows = 10);
        int GetGoodIssuedTotalPage(string search, int rows = 10);
        VesselGoodIssued GetById(int id);
        VesselGoodIssued Update(int id, VesselGoodIssued vesselGoodReceive);
        VesselGoodIssued SaveVesselGoodIssued(VesselGoodIssued vesselGoodReceive);
    }

    public class VesselGoodIssuedRepository :
        GenericRepository<VesselGoodIssued>,
        IVesselGoodIssuedRepository
    {
        public IEnumerable<VesselGoodIssued> GetGoodIssued(string search, int page, int rows)
        {
            using (var context = new VesselInventoryContext())
            {
                return context.vessel_good_issued.SqlQuery (
                        "usp_VesselGoodIssued_GetGoodIssuedList @p0, @p1, @p2",
                        parameters: new object[] { search, page, rows }).ToList();
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

        public VesselGoodIssued SaveVesselGoodIssued(VesselGoodIssued vesselGoodIssued)
        {
            using(var scope = new TransactionScope())
            {
                base.Save(vesselGoodIssued);
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
