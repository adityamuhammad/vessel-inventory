﻿using System;
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
        void SaveTransaction(VesselGoodIssuedItem vesselGoodIssuedItem);
        void UpdateTransaction(int id,VesselGoodIssuedItem vesselGoodIssuedItem);
        void DeleteTransaction(int id); 
        IEnumerable<VesselGoodIssuedItem> GetGoodIssuedItem(int vesselGoodIssuedId);
    }

    public class VesselGoodIssuedItemRepository : 
        GenericRepository<VesselGoodIssuedItem>,
        IVesselGoodIssuedItemRepository
    {
        public void SaveTransaction(VesselGoodIssuedItem vesselGoodIssuedItem)
        {
            using (var context = new VesselInventoryContext())
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
                     new SqlParameter("@vessel_good_issued_id", vesselGoodIssuedItem.vessel_good_issued_id),
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
                     new SqlParameter("@created_by", vesselGoodIssuedItem.created_by),
                     new SqlParameter("@created_date", vesselGoodIssuedItem.created_date),
                     new SqlParameter("@sync_status", vesselGoodIssuedItem.sync_status),
                     new SqlParameter("@is_hidden", vesselGoodIssuedItem.is_hidden)
                     );
            }
        }
        public void UpdateTransaction(int id,VesselGoodIssuedItem vesselGoodIssuedItem)
        {
            using (var context = new VesselInventoryContext())
            {
                StringBuilder execSp = new StringBuilder();
                execSp.Append("usp_VesselGoodIssuedItem_UpdateTransaction");
                execSp.Append(" @vessel_good_issued_item_id,");
                execSp.Append(" @qty,");
                execSp.Append(" @last_modified_by,");
                execSp.Append(" @last_modified_date");
                context.Database.ExecuteSqlCommand
                    (execSp.ToString(),
                     new SqlParameter("@vessel_good_issued_item_id", vesselGoodIssuedItem.vessel_good_issued_item_id),
                     new SqlParameter("@qty", vesselGoodIssuedItem.qty),
                     new SqlParameter("@last_modified_by", Auth.Instance.personalname),
                     new SqlParameter("@last_modified_date", DateTime.Now)
                     );
            }
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

        public void DeleteTransaction(int id)
        {
            using (var context = new VesselInventoryContext())
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