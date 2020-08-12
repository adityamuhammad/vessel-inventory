using System.Collections.Generic;
using VesselInventory.Models;

namespace VesselInventory.Repository
{
    public interface IVesselGoodJournalRepository
    {
        IEnumerable<VesselGoodJournal> GetGoodJournals(int itemId, string ItemDimensionNumber, string search, string documentTypeFilter, int page, int rows);
        int GetJournalLogTotalPage(int itemId, string itemDimensionNumber, string search, string documentTypeFilter, int rows);
    }

}
