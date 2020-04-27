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
            using (var context = new VesselInventoryContext())
            {
                StringBuilder execSp = new StringBuilder();
                execSp.Append("usp_VesselGoodReturnItem_DeleteTransaction");
                execSp.Append(" @vessel_good_return_item_id,");
                execSp.Append(" @last_modified_by,");
                execSp.Append(" @last_modified_date");
                context.Database.ExecuteSqlCommand
                    (execSp.ToString(),
                     new SqlParameter("@vessel_good_return_item_id", id),
                     new SqlParameter("@last_modified_by", Auth.Instance.personalname),
                     new SqlParameter("@last_modified_date", DateTime.Now)
                     );
            }
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

        public void SaveTransaction(VesselGoodReturnItem vesselGoodReturnItem)
        {
            using (var context = new VesselInventoryContext())
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
                     new SqlParameter("@vessel_good_return_id", vesselGoodReturnItem.vessel_good_return_id),
                     new SqlParameter("@item_id", vesselGoodReturnItem.item_id),
                     new SqlParameter("@item_name", vesselGoodReturnItem.item_name),
                     new SqlParameter("@brand_type_id", vesselGoodReturnItem.brand_type_id),
                     new SqlParameter("@brand_type_name", vesselGoodReturnItem.brand_type_name),
                     new SqlParameter("@color_size_id", vesselGoodReturnItem.color_size_id),
                     new SqlParameter("@color_size_name", vesselGoodReturnItem.color_size_name),
                     new SqlParameter("@item_dimension_number", vesselGoodReturnItem.item_dimension_number),
                     new SqlParameter("@item_group_id", vesselGoodReturnItem.item_group_id),
                     new SqlParameter("@qty", vesselGoodReturnItem.qty),
                     new SqlParameter("@uom", vesselGoodReturnItem.uom),
                     new SqlParameter("@reason", vesselGoodReturnItem.reason),
                     new SqlParameter("@created_by", vesselGoodReturnItem.created_by),
                     new SqlParameter("@created_date", vesselGoodReturnItem.created_date),
                     new SqlParameter("@sync_status", vesselGoodReturnItem.sync_status),
                     new SqlParameter("@is_hidden", vesselGoodReturnItem.is_hidden)
                     );
            }
        }

        public void UpdateTransaction(int id, VesselGoodReturnItem vesselGoodReturnItem)
        {
            using (var context = new VesselInventoryContext())
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
                     new SqlParameter("@vessel_good_return_item_id", vesselGoodReturnItem.vessel_good_return_item_id),
                     new SqlParameter("@qty", vesselGoodReturnItem.qty),
                     new SqlParameter("@reason", vesselGoodReturnItem.reason),
                     new SqlParameter("@last_modified_by", Auth.Instance.personalname),
                     new SqlParameter("@last_modified_date", DateTime.Now)
                     );
            }
        }
    }
}
