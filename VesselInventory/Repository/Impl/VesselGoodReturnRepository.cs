﻿using System.Collections.Generic;
using System.Linq;
using System.Transactions;
using VesselInventory.Filters;
using VesselInventory.Models;

namespace VesselInventory.Repository.Impl
{
    public class VesselGoodReturnRepository
        : GenericRepository<VesselGoodReturn>
        , IVesselGoodReturnRepository
    {
        public IEnumerable<VesselGoodReturn> GetGoodReturnDataGrid(PageFilter pageFilter)
        {
            using (var context = new AppVesselInventoryContext())
            {
                return context.VesselGoodReturn.SqlQuery (
                        "usp_VesselGoodReturn_GetGoodReturnList @p0, @p1, @p2, @p3, @p4",
                        parameters: new object[] {
                            pageFilter.Search,
                            pageFilter.PageNum,
                            pageFilter.NumRows,
                            pageFilter.SortName,
                            pageFilter.SortType
                        }).ToList();
            }
        }

        public int GetGoodReturnTotalPage(PageFilter pageFilter)
        {
            using (var context = new AppVesselInventoryContext())
            {
                return context.Database.SqlQuery<int>(
                        "usp_VesselGoodReturn_GetGoodReturnPages @p0, @p1",
                        parameters: new object[] {
                            pageFilter.Search,
                            pageFilter.NumRows
                        }).Single();
            }
        }

        public VesselGoodReturn SaveTransaction(VesselGoodReturn vesselGoodReturn)
        {
            using(var scope = new TransactionScope())
            {
                Save(vesselGoodReturn);
                using(var context = new AppVesselInventoryContext())
                {
                    context.Database.ExecuteSqlCommand("usp_DocSequence_IncrementSeqNumber 4");
                }
                scope.Complete();
                return vesselGoodReturn;
            }
        }
    }
}
