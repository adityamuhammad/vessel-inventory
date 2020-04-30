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
                        where item.VesselGoodIssuedId == vesselGoodIssuedId 
                        && item.IsHidden == false
                        orderby item.CreatedDate descending
                        select item)
                        .ToList();

            }
        }

        public void SaveTransaction(VesselGoodIssuedItem vesselGoodIssuedItem)
        {
            using (var context = new AppVesselInventoryContext())
            {
                StringBuilder execSp = new StringBuilder();
                execSp.Append("usp_VesselGoodIssuedItem_SaveTransaction");
                execSp.Append(" @vesselGoodIssuedId,");
                execSp.Append(" @itemId,");
                execSp.Append(" @itemName,");
                execSp.Append(" @brandTypeId,");
                execSp.Append(" @brandTypeName,");
                execSp.Append(" @colorSizeId,");
                execSp.Append(" @colorSizeName,");
                execSp.Append(" @itemDimensionNumber,");
                execSp.Append(" @itemGroupId,");
                execSp.Append(" @qty,");
                execSp.Append(" @uom,");
                execSp.Append(" @createdBy,");
                execSp.Append(" @createdDate,");
                execSp.Append(" @syncStatus,");
                execSp.Append(" @isHidden");
                context.Database.ExecuteSqlCommand
                    (execSp.ToString(),
                     new SqlParameter("@vesselGoodIssuedId", vesselGoodIssuedItem.VesselGoodIssuedId),
                     new SqlParameter("@itemId", vesselGoodIssuedItem.ItemId),
                     new SqlParameter("@itemName", vesselGoodIssuedItem.ItemName),
                     new SqlParameter("@brandTypeId", vesselGoodIssuedItem.BrandTypeId),
                     new SqlParameter("@brandTypeName", vesselGoodIssuedItem.BrandTypeName),
                     new SqlParameter("@colorSizeId", vesselGoodIssuedItem.ColorSizeId),
                     new SqlParameter("@colorSizeName", vesselGoodIssuedItem.ColorSizeName),
                     new SqlParameter("@itemDimensionNumber", vesselGoodIssuedItem.ItemDimensionNumber),
                     new SqlParameter("@itemGroupId", vesselGoodIssuedItem.ItemGroupId),
                     new SqlParameter("@qty", vesselGoodIssuedItem.Qty),
                     new SqlParameter("@uom", vesselGoodIssuedItem.Uom),
                     new SqlParameter("@createdBy", vesselGoodIssuedItem.CreatedBy),
                     new SqlParameter("@createdDate", vesselGoodIssuedItem.CreatedDate),
                     new SqlParameter("@syncStatus", vesselGoodIssuedItem.SyncStatus),
                     new SqlParameter("@isHidden", vesselGoodIssuedItem.IsHidden)
                     );
            }
        }
        public void UpdateTransaction(int id,VesselGoodIssuedItem vesselGoodIssuedItem)
        {
            using (var context = new AppVesselInventoryContext())
            {
                StringBuilder execSp = new StringBuilder();
                execSp.Append("usp_VesselGoodIssuedItem_UpdateTransaction");
                execSp.Append(" @vesselGoodIssuedItemId,");
                execSp.Append(" @qty,");
                execSp.Append(" @lastModifiedBy,");
                execSp.Append(" @lastModifiedDate");
                context.Database.ExecuteSqlCommand
                    (execSp.ToString(),
                     new SqlParameter("@vesselGoodIssuedItemId", vesselGoodIssuedItem.VesselGoodIssuedItemId),
                     new SqlParameter("@qty", vesselGoodIssuedItem.Qty),
                     new SqlParameter("@lastModifiedBy", vesselGoodIssuedItem.LastModifiedBy),
                     new SqlParameter("@lastModifiedDate", vesselGoodIssuedItem.LastModifiedDate)
                     );
            }
        }
        public void DeleteTransaction(int id)
        {
            using (var context = new AppVesselInventoryContext())
            {
                StringBuilder execSp = new StringBuilder();
                execSp.Append("usp_VesselGoodIssuedItem_DeleteTransaction");
                execSp.Append(" @vesselGoodIssuedItemId,");
                execSp.Append(" @lastModifiedBy,");
                execSp.Append(" @lastModifiedDate");
                context.Database.ExecuteSqlCommand
                    (execSp.ToString(),
                     new SqlParameter("@vesselGoodIssuedItemId", id),
                     new SqlParameter("@lastModifiedBy", Auth.Instance.PersonName),
                     new SqlParameter("@lastModifiedDate", DateTime.Now)
                     );
            }
        }
    }
}
