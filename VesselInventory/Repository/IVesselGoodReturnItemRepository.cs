using System.Collections.Generic;
using VesselInventory.Models;

namespace VesselInventory.Repository
{
    public interface IVesselGoodReturnItemRepository : IGenericRepository<VesselGoodReturnItem>
    {
        IEnumerable<VesselGoodReturnItem> GetGoodReturnItem(int vesselGoodReturnId);
        void SaveTransaction(VesselGoodReturnItem vesselGoodReturnItem);
        void UpdateTransaction(VesselGoodReturnItem vesselGoodReturnItem);
        void DeleteTransaction(int id); 
    }

}
