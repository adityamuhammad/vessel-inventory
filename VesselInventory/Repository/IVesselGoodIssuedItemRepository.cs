using System;
using System.Collections.Generic;
using System.Linq;
using VesselInventory.Models;

namespace VesselInventory.Repository
{
    public interface IVesselGoodIssuedItemRepository
    {
        VesselGoodIssuedItem GetById(int id);
        void Save(VesselGoodIssuedItem vesselGoodIssuedItem);
        void Update(int id,VesselGoodIssuedItem vesselGoodIssuedItem);
        void Delete(int id);
        IEnumerable<VesselGoodIssuedItem> GetGoodIssuedItem(int vesselGoodIssuedId);
    }

    public class VesselGoodIssuedItemRepository : 
        GenericRepository<VesselGoodIssuedItem>,
        IVesselGoodIssuedItemRepository
    {
        public new void Save(VesselGoodIssuedItem vesselGoodIssuedItem)
        { }
        public new void Update(int id,VesselGoodIssuedItem vesselGoodIssuedItem)
        { }
        public override void Delete(int id)
        {
        }

        public IEnumerable<VesselGoodIssuedItem> GetGoodIssuedItem(int vesselGoodIssuedId)
        {
            using (var context = new VesselInventoryContext())
            {
                return (from item in context.vessel_good_issued_item
                        where item.vessel_good_issued_id == vesselGoodIssuedId && 
                        item.is_hidden == false select item)
                        .ToList();

            }
        }
    }
}
