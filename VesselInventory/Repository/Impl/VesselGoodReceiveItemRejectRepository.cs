using System.Collections.Generic;
using System.Linq;
using VesselInventory.Models;

namespace VesselInventory.Repository.Impl
{
    public class VesselGoodReceiveItemRejectRepository 
        : GenericRepository<VesselGoodReceiveItemReject>
        , IVesselGoodReceiveItemRejectRepository
    {
        public IEnumerable<VesselGoodReceiveItemReject> GetGoodReceiveItemRejected(int vesselGoodReceiveId)
        {
            using (var context = new AppVesselInventoryContext())
            {
                return (from item in context.VesselGoodReceiveItemReject
                        where item.VesselGoodReceiveId == vesselGoodReceiveId 
                        && item.IsHidden == false
                        orderby item.CreatedDate descending
                        select item)
                        .ToList();
            }
        }

        public override void Delete(int id)
        {
            using (var context = new AppVesselInventoryContext())
            {
                var current = context.VesselGoodReceiveItemReject.Find(id);
                if (current is null) return;
                current.IsHidden = true;
                context.SaveChanges();
            }
        }
    }
}
