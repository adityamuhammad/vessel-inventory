using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using VesselInventory.Models;

namespace VesselInventory.Repository
{
    public interface IVesselGoodIssuedItemRepository
    {
        VesselGoodIssuedItem GetById(int id);
        int SaveTransaction(VesselGoodIssuedItem vesselGoodIssuedItem);
        void Update(int id,VesselGoodIssuedItem vesselGoodIssuedItem);
        void Delete(int id);
        IEnumerable<VesselGoodIssuedItem> GetGoodIssuedItem(int vesselGoodIssuedId);
    }

    public class VesselGoodIssuedItemRepository : 
        GenericRepository<VesselGoodIssuedItem>,
        IVesselGoodIssuedItemRepository
    {
        public int SaveTransaction(VesselGoodIssuedItem vesselGoodIssuedItem)
        {
            using (var context = new VesselInventoryContext())
            {
                return context.Database.SqlQuery<int>
                    ("usp_VesselGoodIssuedItem_Save @vessel_good_issued_id, @item_id, @item_name, @brand_type_id, @brand_type_name, @color_size_id, @color_size_name, @item_dimension_number, @item_group_id, @qty, @uom, @created_by, @created_date, @sync_status, @is_hidden",
                     new SqlParameter("@vessel_good_issued_id", 7),
                     new SqlParameter("@item_id", vesselGoodIssuedItem.item_id),
                     new SqlParameter("@item_name", vesselGoodIssuedItem.item_name),
                     new SqlParameter("@brand_type_id", vesselGoodIssuedItem.brand_type_id),
                     new SqlParameter("@brand_type_name", vesselGoodIssuedItem.brand_type_name),
                     new SqlParameter("@color_size_id", vesselGoodIssuedItem.color_size_id),
                     new SqlParameter("@color_size_name", vesselGoodIssuedItem.color_size_name),
                     new SqlParameter("@item_dimension_number", vesselGoodIssuedItem.item_dimension_number),
                     new SqlParameter("@item_group_id", vesselGoodIssuedItem.item_group_id),
                     new SqlParameter("@qty", vesselGoodIssuedItem.qty),
                     new SqlParameter("@uom", vesselGoodIssuedItem.uom),
                     new SqlParameter("@created_by","njai" ),
                     new SqlParameter("@created_date", vesselGoodIssuedItem.created_date),
                     new SqlParameter("@sync_status", vesselGoodIssuedItem.sync_status),
                     new SqlParameter("@is_hidden", vesselGoodIssuedItem.is_hidden)
                     ).Single();
            }
        }
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
