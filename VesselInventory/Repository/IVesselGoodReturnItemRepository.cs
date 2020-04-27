using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VesselInventory.Models;

namespace VesselInventory.Repository
{
    public interface IVesselGoodReturnItemRepository : IGenericRepository<VesselGoodReturnItem>
    {
        IEnumerable<VesselGoodReturnItem> GetGoodReturnItem(int vesselGoodReturnId);
        void SaveTransaction(VesselGoodReturnItem vesselGoodIssuedItem);
        void UpdateTransaction(int id,VesselGoodReturnItem vesselGoodIssuedItem);
        void DeleteTransaction(int id); 
    }

    public class VesselGoodReturnItemRepository
        : GenericRepository<VesselGoodReturnItem>
        , IVesselGoodReturnItemRepository
    {
        public void DeleteTransaction(int id)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<VesselGoodReturnItem> GetGoodReturnItem(int vesselGoodReturnId)
        {
            using (var context = new VesselInventoryContext())
            {
                return (from item in context.vessel_good_return_item
                        where item.vessel_good_return_id == vesselGoodReturnId && 
                        item.is_hidden == false select item)
                        .ToList();
            }
        }

        public void SaveTransaction(VesselGoodReturnItem vesselGoodIssuedItem)
        {
            throw new NotImplementedException();
        }

        public void UpdateTransaction(int id, VesselGoodReturnItem vesselGoodIssuedItem)
        {
            throw new NotImplementedException();
        }
    }
}
