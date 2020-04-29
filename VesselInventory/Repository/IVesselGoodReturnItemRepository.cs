using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VesselInventory.Models;
using VesselInventory.Utility;

namespace VesselInventory.Repository
{
    public interface IVesselGoodReturnItemRepository : IGenericRepository<VesselGoodReturnItem>
    {
        IEnumerable<VesselGoodReturnItem> GetGoodReturnItem(int vesselGoodReturnId);
        void SaveTransaction(VesselGoodReturnItem vesselGoodReturnItem);
        void UpdateTransaction(int id,VesselGoodReturnItem vesselGoodReturnItem);
        void DeleteTransaction(int id); 
    }

    public class VesselGoodReturnItemRepository
        : GenericRepository<VesselGoodReturnItem>
        , IVesselGoodReturnItemRepository
    {
        public void DeleteTransaction(int id)
        {
            using (var context = new AppVesselInventoryContext())
            {
                StringBuilder execSp = new StringBuilder();
                execSp.Append("usp_VesselGoodReturnItem_DeleteTransaction");
                execSp.Append(" @vessel_good_return_item_id,");
                execSp.Append(" @last_modified_by,");
                execSp.Append(" @last_modified_date");
                context.Database.ExecuteSqlCommand
                    (execSp.ToString(),
                     new SqlParameter("@vessel_good_return_item_id", id),
                     new SqlParameter("@last_modified_by", Auth.Instance.PersonName),
                     new SqlParameter("@last_modified_date", DateTime.Now)
                     );
            }
        }

        public IEnumerable<VesselGoodReturnItem> GetGoodReturnItem(int vesselGoodReturnId)
        {
            using (var context = new AppVesselInventoryContext())
            {
                return (from item in context.VesselGoodReturnItem
                        where item.VesselGoodReturnId == vesselGoodReturnId && 
                        item.IsHidden == false select item)
                        .ToList();
            }
        }

        public void SaveTransaction(VesselGoodReturnItem vesselGoodReturnItem)
        {
            using (var context = new AppVesselInventoryContext())
            {
                StringBuilder execSp = new StringBuilder();
                execSp.Append("usp_VesselGoodReturnItem_SaveTransaction");
                execSp.Append(" @vessel_good_return_id,");
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
                execSp.Append(" @reason,");
                execSp.Append(" @created_by,");
                execSp.Append(" @created_date,");
                execSp.Append(" @sync_status,");
                execSp.Append(" @is_hidden");
                context.Database.ExecuteSqlCommand
                    (execSp.ToString(),
                     new SqlParameter("@vessel_good_return_id", vesselGoodReturnItem.VesselGoodReturnId),
                     new SqlParameter("@item_id", vesselGoodReturnItem.ItemId),
                     new SqlParameter("@item_name", vesselGoodReturnItem.ItemName),
                     new SqlParameter("@brand_type_id", vesselGoodReturnItem.BrandTypeId),
                     new SqlParameter("@brand_type_name", vesselGoodReturnItem.BrandTypeName),
                     new SqlParameter("@color_size_id", vesselGoodReturnItem.ColorSizeId),
                     new SqlParameter("@color_size_name", vesselGoodReturnItem.ColorSizeName),
                     new SqlParameter("@item_dimension_number", vesselGoodReturnItem.ItemDimensionNumber),
                     new SqlParameter("@item_group_id", vesselGoodReturnItem.ItemGroupId),
                     new SqlParameter("@qty", vesselGoodReturnItem.Qty),
                     new SqlParameter("@uom", vesselGoodReturnItem.Uom),
                     new SqlParameter("@reason", vesselGoodReturnItem.Reason),
                     new SqlParameter("@created_by", vesselGoodReturnItem.CreatedBy),
                     new SqlParameter("@created_date", vesselGoodReturnItem.CreatedDate),
                     new SqlParameter("@sync_status", vesselGoodReturnItem.SyncStatus),
                     new SqlParameter("@is_hidden", vesselGoodReturnItem.IsHidden)
                     );
            }
        }

        public void UpdateTransaction(int id, VesselGoodReturnItem vesselGoodReturnItem)
        {
            using (var context = new AppVesselInventoryContext())
            {
                StringBuilder execSp = new StringBuilder();
                execSp.Append("usp_VesselGoodReturnItem_UpdateTransaction");
                execSp.Append(" @vessel_good_return_item_id,");
                execSp.Append(" @qty,");
                execSp.Append(" @reason,");
                execSp.Append(" @last_modified_by,");
                execSp.Append(" @last_modified_date");
                context.Database.ExecuteSqlCommand
                    (execSp.ToString(),
                     new SqlParameter("@vessel_good_return_item_id", vesselGoodReturnItem.VesselGoodReturnItemId),
                     new SqlParameter("@qty", vesselGoodReturnItem.Qty),
                     new SqlParameter("@reason", vesselGoodReturnItem.Reason),
                     new SqlParameter("@last_modified_by", Auth.Instance.PersonName),
                     new SqlParameter("@last_modified_date", DateTime.Now)
                     );
            }
        }
    }
}
