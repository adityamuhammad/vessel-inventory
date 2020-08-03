using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using VesselInventory.Models;

namespace VesselInventory.Repository
{
    public interface IVesselGoodJournalRepository
    {
        IEnumerable<VesselGoodJournal> GetGoodJournals(int itemId, string ItemDimensionNumber, string search, string documentTypeFilter, int page, int rows);
        int GetJournalLogTotalPage(int itemId, string itemDimensionNumber, string search, string documentTypeFilter, int rows);
    }

    public class VesselGoodJournalRepository : IVesselGoodJournalRepository
    {
        public IEnumerable<VesselGoodJournal> GetGoodJournals(int itemId, string itemDimensionNumber, string search, string documentTypeFilter, int page, int rows)
        {
            using(var context = new AppVesselInventoryContext())
            {
                return context.VesselGoodJournal.SqlQuery(
                    "usp_VesselGoodJournal_GetGoodJournalList @itemId, @itemDimensionNumber, @search, @documentTypeFilter, @page, @rows",
                    new SqlParameter("@itemId", itemId),
                    new SqlParameter("@itemDimensionNumber", itemDimensionNumber),
                    new SqlParameter("@search", search),
                    new SqlParameter("@documentTypeFilter", documentTypeFilter),
                    new SqlParameter("@page", page),
                    new SqlParameter("@rows", rows)
                ).ToList();

            }
        }

        public int GetJournalLogTotalPage(int itemId, string itemDimensionNumber, string search, string documentTypeFilter, int rows)
        {
            using (var context = new AppVesselInventoryContext())
            {
                return context.Database.SqlQuery<int>(
                    "usp_VesselGoodJournal_GetGoodJournalPages @p0, @p1, @p2, @p3, @p4",
                    parameters: new object[] { itemId, itemDimensionNumber, search, documentTypeFilter, rows }
                ).Single();
            }
        }
    }
}
