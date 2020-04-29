using System.Collections.Generic;
using System.Linq;
using VesselInventory.Models;
namespace VesselInventory.Repository
{
    public interface IVesselGoodReceiveItemRejectRepository : IGenericRepository<VesselGoodReceiveItemReject>
    {
        IEnumerable<VesselGoodReceiveItemReject> GetGoodReceiveItemRejected(int vesselGoodReceiveId);
    }

    public class VesselGoodReceiveItemRejectRepository 
        : GenericRepository<VesselGoodReceiveItemReject>
        , IVesselGoodReceiveItemRejectRepository
    {
        public IEnumerable<VesselGoodReceiveItemReject> GetGoodReceiveItemRejected(int vesselGoodReceiveId)
        {
            using (var context = new VesselInventoryContext())
            {
                return (from item in context.VesselGoodReceiveItemReject
                        where item.VesselGoodReceiveId == vesselGoodReceiveId && 
                        item.IsHidden == false select item)
                        .ToList();
            }
        }

        public override void Delete(int id)
        {
            using (var context = new VesselInventoryContext())
            {
                var current = context.VesselGoodReceiveItemReject.Find(id);
                if (current is null) return;
                current.IsHidden = true;
                context.SaveChanges();
            }
        }
    }
}
