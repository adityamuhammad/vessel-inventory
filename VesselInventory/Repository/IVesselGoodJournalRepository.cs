using System.Collections.Generic;
using VesselInventory.Filters;
using VesselInventory.Models;

namespace VesselInventory.Repository
{
    public interface IVesselGoodJournalRepository
    {
        IEnumerable<VesselGoodJournal> GetGoodJournals(GoodJournalFilter goodJournalFilter, PageFilter pageFilter);
        int GetJournalLogTotalPage(GoodJournalFilter goodJournalFilter, PageFilter pageFilter);
    }

}
