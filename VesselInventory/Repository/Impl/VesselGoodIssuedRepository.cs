﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using VesselInventory.Models;

namespace VesselInventory.Repository.Impl
{
    public class VesselGoodIssuedRepository 
        : GenericRepository<VesselGoodIssued>
        , IVesselGoodIssuedRepository
    {
        public IEnumerable<VesselGoodIssued> GetGoodIssuedDataGrid(
            string search, int page, int rows, 
            string sortColumnName, string sortBy)
        {
            using (var context = new AppVesselInventoryContext())
            {
                return context.VesselGoodIssued.SqlQuery (
                        "usp_VesselGoodIssued_GetGoodIssuedList @p0, @p1, @p2, @p3, @p4",
                        parameters: new object[] { search, page, rows, sortColumnName, sortBy }).ToList();
            }
        }

        public int GetGoodIssuedTotalPage(string search, int rows)
        {
            using (var context = new AppVesselInventoryContext())
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