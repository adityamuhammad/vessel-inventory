using System.Collections.Generic;
using VesselInventory.Models;

namespace VesselInventory.Repository
{
    public interface IVesselGoodIssuedItemRepository : IGenericRepository<VesselGoodIssuedItem>
    {
        IEnumerable<VesselGoodIssuedItem> GetGoodIssuedItem(int vesselGoodIssuedId);
        void SaveTransaction(VesselGoodIssuedItem vesselGoodIssuedItem);
        void UpdateTransaction(int id,VesselGoodIssuedItem vesselGoodIssuedItem);
        void DeleteTransaction(int id); 
    }

}
