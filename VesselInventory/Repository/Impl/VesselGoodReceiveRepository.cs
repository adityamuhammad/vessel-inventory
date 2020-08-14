using System.Collections.Generic;
using System.Linq;
using System.Transactions;
using VesselInventory.Filters;
using VesselInventory.Models;

namespace VesselInventory.Repository.Impl
{
    public class VesselGoodReceiveRepository 
        : GenericRepository<VesselGoodReceive>
        , IVesselGoodReceiveRepository
    {
        public VesselGoodReceiveRepository() { }

        public IEnumerable<VesselGoodReceive> GetGoodReceiveDataGrid(PageFilter pageFilter)
        {
            using (var context = new AppVesselInventoryContext())
            {
                return context.VesselGoodReceive.SqlQuery(
                        "usp_VesselGoodReceive_GetGoodReceiveList @p0, @p1, @p2, @p3, @p4",
                        parameters: new object[] {
                            pageFilter.Search,
                            pageFilter.PageNum,
                            pageFilter.NumRows,
                            pageFilter.SortName,
                            pageFilter.SortType
                        }).ToList();
            }
        }

        public int GetGoodReceiveTotalPage(PageFilter pageFilter)
        {
            using (var context = new AppVesselInventoryContext())
            {
                return context.Database.SqlQuery<int>(
                        "usp_VesselGoodReceive_GetGoodReceivePages @p0, @p1",
                        parameters: new object[] {
                            pageFilter.Search,
                            pageFilter.NumRows
                        }).Single();
            }
        }

        public VesselGoodReceive SaveTransaction(VesselGoodReceive vesselGoodReceive)
        {
            using(var scope = new TransactionScope())
            {
                base.Save(vesselGoodReceive);
                using(var context = new AppVesselInventoryContext())
                {
                    context.Database.ExecuteSqlCommand("usp_DocSequence_IncrementSeqNumber 2");
                }
                scope.Complete();
                return vesselGoodReceive;
            }
        }
    }
}
