using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using VesselInventory.Filters;
using VesselInventory.Models;

namespace VesselInventory.Repository.Impl
{
    public class VesselGoodJournalRepository : IVesselGoodJournalRepository
    {
        public IEnumerable<VesselGoodJournal> GetGoodJournals(GoodJournalFilter goodJournalFilter, PageFilter pageFilter)
        {
            using(var context = new AppVesselInventoryContext())
            {
                return context.VesselGoodJournal.SqlQuery(
                    "usp_VesselGoodJournal_GetGoodJournalList @itemId, @itemDimensionNumber, @search, @documentTypeFilter, @page, @rows",
                    new SqlParameter("@itemId", goodJournalFilter.ItemId),
                    new SqlParameter("@itemDimensionNumber", goodJournalFilter.ItemDimensionNumber),
                    new SqlParameter("@search", pageFilter.Search),
                    new SqlParameter("@documentTypeFilter", goodJournalFilter.DocumentType),
                    new SqlParameter("@page", pageFilter.PageNum),
                    new SqlParameter("@rows", pageFilter.NumRows)
                ).ToList();

            }
        }

        public int GetJournalLogTotalPage(GoodJournalFilter goodJournalFilter, PageFilter pageFilter)
        {
            using (var context = new AppVesselInventoryContext())
            {
                return context.Database.SqlQuery<int>(
                    "usp_VesselGoodJournal_GetGoodJournalPages @p0, @p1, @p2, @p3, @p4",
                    parameters: new object[] {
                        goodJournalFilter.ItemId,
                        goodJournalFilter.ItemDimensionNumber,
                        pageFilter.Search,
                        goodJournalFilter.DocumentType,
                        pageFilter.NumRows
                    }).Single();
            }
        }
    }
}
