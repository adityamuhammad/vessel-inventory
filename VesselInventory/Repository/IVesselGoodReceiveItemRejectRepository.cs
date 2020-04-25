using System.Collections.Generic;
using System.Linq;
using VesselInventory.Models;
namespace VesselInventory.Repository
{
    public interface IVesselGoodReceiveItemRejectRepository : IGenericRepository<VesselGoodReceiveItemReject>
    {
        IEnumerable<VesselGoodReceiveItemReject> GetGoodReceiveItemRejected(int vesselGoodReceiveId);
    }

    public class VesselGoodReceiveItemRejectRepository :
        GenericRepository<VesselGoodReceiveItemReject>,
        IVesselGoodReceiveItemRejectRepository
    {
        public IEnumerable<VesselGoodReceiveItemReject> GetGoodReceiveItemRejected(int vesselGoodReceiveId)
        {
            using (var context = new VesselInventoryContext())
            {
                return (from item in context.vessel_good_receive_item_reject
                        where item.vessel_good_receive_id == vesselGoodReceiveId && 
                        item.is_hidden == false select item)
                        .ToList();
            }
        }

        public override void Delete(int id)
        {
            using (var context = new VesselInventoryContext())
            {
                var current = context.vessel_good_receive_item_reject.Find(id);
                if (current is null) return;
                current.is_hidden = true;
                context.SaveChanges();
            }
        }
    }
}
