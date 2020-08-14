using System.Collections.Generic;
using System.Linq;
using System.Transactions;
using VesselInventory.Filters;
using VesselInventory.Models;

namespace VesselInventory.Repository.Impl
{
    public class VesselGoodIssuedRepository 
        : GenericRepository<VesselGoodIssued>
        , IVesselGoodIssuedRepository
    {
        public IEnumerable<VesselGoodIssued> GetGoodIssuedDataGrid(PageFilter pageFilter)
        {
            using (var context = new AppVesselInventoryContext())
            {
                return context.VesselGoodIssued.SqlQuery (
                        "usp_VesselGoodIssued_GetGoodIssuedList @p0, @p1, @p2, @p3, @p4",
                        parameters: new object[] 
                            {
                                pageFilter.Search,
                                pageFilter.PageNum,
                                pageFilter.NumRows,
                                pageFilter.SortName,
                                pageFilter.SortType}).ToList();
            }
        }

        public int GetGoodIssuedTotalPage(PageFilter pageFilter)
        {
            using (var context = new AppVesselInventoryContext())
            {
                return context.Database.SqlQuery<int>(
                        "usp_VesselGoodIssued_GetGoodIssuedPages @p0, @p1",
                        parameters: new object[] { pageFilter.Search, pageFilter.NumRows}).Single();
            }
        }

        public VesselGoodIssued SaveTransaction(VesselGoodIssued vesselGoodIssued)
        {
            using(var scope = new TransactionScope())
            {
                Save(vesselGoodIssued);
                using(var context = new AppVesselInventoryContext())
                {
                    context.Database.ExecuteSqlCommand("usp_DocSequence_IncrementSeqNumber 3");
                }
                scope.Complete();
                return vesselGoodIssued;
            }
        }
    }
}
