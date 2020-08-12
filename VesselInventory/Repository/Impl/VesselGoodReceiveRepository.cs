﻿using System.Collections.Generic;
using System.Linq;
using System.Transactions;
using VesselInventory.Models;

namespace VesselInventory.Repository.Impl
{
    public class VesselGoodReceiveRepository 
        : GenericRepository<VesselGoodReceive>
        , IVesselGoodReceiveRepository
    {
        public VesselGoodReceiveRepository() { }

        public IEnumerable<VesselGoodReceive> GetGoodReceiveDataGrid(
            string search , int page, int rows, 
            string sortColumnName, string sortBy)
        {
            using (var context = new AppVesselInventoryContext())
            {
                return context.VesselGoodReceive.SqlQuery(
                        "usp_VesselGoodReceive_GetGoodReceiveList @p0, @p1, @p2, @p3, @p4",
                        parameters: new object[] { search, page, rows, sortColumnName, sortBy }).ToList();
            }
        }

        public int GetGoodReceiveTotalPage(string search, int rows)
        {
            using (var context = new AppVesselInventoryContext())
            {
                return context.Database.SqlQuery<int>(
                        "usp_VesselGoodReceive_GetGoodReceivePages @p0, @p1",
                        parameters: new object[] { search, rows }).Single();
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