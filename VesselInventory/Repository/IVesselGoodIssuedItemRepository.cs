using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using VesselInventory.Models;
using VesselInventory.Utility;

namespace VesselInventory.Repository
{
    public interface IVesselGoodIssuedItemRepository : IGenericRepository<VesselGoodIssuedItem>
    {
        IEnumerable<VesselGoodIssuedItem> GetGoodIssuedItem(int vesselGoodIssuedId);
        void SaveTransaction(VesselGoodIssuedItem vesselGoodIssuedItem);
        void UpdateTransaction(int id,VesselGoodIssuedItem vesselGoodIssuedItem);
        void DeleteTransaction(int id); 
    }

    public class VesselGoodIssuedItemRepository 
        : GenericRepository<VesselGoodIssuedItem>
        , IVesselGoodIssuedItemRepository
    {
        public IEnumerable<VesselGoodIssuedItem> GetGoodIssuedItem(int vesselGoodIssuedId)
        {
            using (var context = new AppVesselInventoryContext())
            {
                return (from item in context.VesselGoodIssuedItem
                        where item.VesselGoodIssuedId == vesselGoodIssuedId && 
                        item.IsHidden == false select item)
                        .ToList();

            }
        }

        public void SaveTransaction(VesselGoodIssuedItem vesselGoodIssuedItem)
        {
            using (var context = new AppVesselInventoryContext())
            {
                StringBuilder execSp = new StringBuilder();
                execSp.Append("usp_VesselGoodIssuedItem_SaveTransaction");
                execSp.Append(" @vessel_good_issued_id,");
                execSp.Append(" @item_id,");
                execSp.Append(" @item_name,");
                execSp.Append(" @brand_type_id,");
                execSp.Append(" @brand_type_name,");
                execSp.Append(" @color_size_id,");
                execSp.Append(" @color_size_name,");
                execSp.Append(" @item_dimension_number,");
                execSp.Append(" @item_group_id,");
                execSp.Append(" @qty,");
                execSp.Append(" @uom,");
                execSp.Append(" @created_by,");
                execSp.Append(" @created_date,");
                execSp.Append(" @sync_status,");
                execSp.Append(" @is_hidden");
                context.Database.ExecuteSqlCommand
                    (execSp.ToString(),
                     new SqlParameter("@vessel_good_issued_id", vesselGoodIssuedItem.VesselGoodIssuedId),
                     new SqlParameter("@item_id", vesselGoodIssuedItem.ItemId),
                     new SqlParameter("@item_name", vesselGoodIssuedItem.ItemName),
                     new SqlParameter("@brand_type_id", vesselGoodIssuedItem.BrandTypeId),
                     new SqlParameter("@brand_type_name", vesselGoodIssuedItem.BrandTypeName),
                     new SqlParameter("@color_size_id", vesselGoodIssuedItem.ColorSizeId),
                     new SqlParameter("@color_size_name", vesselGoodIssuedItem.ColorSizeName),
                     new SqlParameter("@item_dimension_number", vesselGoodIssuedItem.ItemDimensionNumber),
                     new SqlParameter("@item_group_id", vesselGoodIssuedItem.ItemGroupId),
                     new SqlParameter("@qty", vesselGoodIssuedItem.Qty),
                     new SqlParameter("@uom", vesselGoodIssuedItem.Uom),
                     new SqlParameter("@created_by", vesselGoodIssuedItem.CreatedBy),
                     new SqlParameter("@created_date", vesselGoodIssuedItem.CreatedDate),
                     new SqlParameter("@sync_status", vesselGoodIssuedItem.SyncStatus),
                     new SqlParameter("@is_hidden", vesselGoodIssuedItem.IsHidden)
                     );
            }
        }
        public void UpdateTransaction(int id,VesselGoodIssuedItem vesselGoodIssuedItem)
        {
            using (var context = new AppVesselInventoryContext())
            {
                StringBuilder execSp = new StringBuilder();
                execSp.Append("usp_VesselGoodIssuedItem_UpdateTransaction");
                execSp.Append(" @vessel_good_issued_item_id,");
                execSp.Append(" @qty,");
                execSp.Append(" @last_modified_by,");
                execSp.Append(" @last_modified_date");
                context.Database.ExecuteSqlCommand
                    (execSp.ToString(),
                     new SqlParameter("@vessel_good_issued_item_id", vesselGoodIssuedItem.VesselGoodIssuedItemId),
                     new SqlParameter("@qty", vesselGoodIssuedItem.Qty),
                     new SqlParameter("@last_modified_by", Auth.Instance.personalname),
                     new SqlParameter("@last_modified_date", DateTime.Now)
                     );
            }
        }
        public void DeleteTransaction(int id)
        {
            using (var context = new AppVesselInventoryContext())
            {
                StringBuilder execSp = new StringBuilder();
                execSp.Append("usp_VesselGoodIssuedItem_DeleteTransaction");
                execSp.Append(" @vessel_good_issued_item_id,");
                execSp.Append(" @last_modified_by,");
                execSp.Append(" @last_modified_date");
                context.Database.ExecuteSqlCommand
                    (execSp.ToString(),
                     new SqlParameter("@vessel_good_issued_item_id", id),
                     new SqlParameter("@last_modified_by", Auth.Instance.personalname),
                     new SqlParameter("@last_modified_date", DateTime.Now)
                     );
            }
        }
    }
}
