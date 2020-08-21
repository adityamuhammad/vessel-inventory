using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using VesselInventory.Models;
using VesselInventory.Utility;

namespace VesselInventory.Repository.Impl
{
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
                execSp.Append(" @vesselGoodReturnItemId,");
                execSp.Append(" @lastModifiedBy,");
                execSp.Append(" @lastModifiedDate");
                context.Database.ExecuteSqlCommand
                    (execSp.ToString(),
                     new SqlParameter("@vesselGoodReturnItemId", id),
                     new SqlParameter("@lastModifiedBy", Auth.Instance.PersonName),
                     new SqlParameter("@lastModifiedDate", DateTime.Now)
                     );
            }
        }

        public IEnumerable<VesselGoodReturnItem> GetGoodReturnItem(int vesselGoodReturnId)
        {
            using (var context = new AppVesselInventoryContext())
            {
                return (from item in context.VesselGoodReturnItem
                        where item.VesselGoodReturnId == vesselGoodReturnId 
                        && item.IsHidden == false orderby item.CreatedDate descending
                        select item)
                        .ToList();
            }
        }

        public void SaveTransaction(VesselGoodReturnItem vesselGoodReturnItem)
        {
            using (var context = new AppVesselInventoryContext())
            {
                StringBuilder execSp = new StringBuilder();
                execSp.Append("usp_VesselGoodReturnItem_SaveTransaction");
                execSp.Append(" @vesselGoodReturnId,");
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
                execSp.Append(" @reason,");
                execSp.Append(" @createdBy,");
                execSp.Append(" @createdDate,");
                execSp.Append(" @syncStatus,");
                execSp.Append(" @isHidden");
                context.Database.ExecuteSqlCommand
                    (execSp.ToString(),
                     new SqlParameter("@vesselGoodReturnId", vesselGoodReturnItem.VesselGoodReturnId),
                     new SqlParameter("@itemId", vesselGoodReturnItem.ItemId),
                     new SqlParameter("@itemName", vesselGoodReturnItem.ItemName),
                     new SqlParameter("@brandTypeId", vesselGoodReturnItem.BrandTypeId),
                     new SqlParameter("@brandTypeName", vesselGoodReturnItem.BrandTypeName),
                     new SqlParameter("@colorSizeId", vesselGoodReturnItem.ColorSizeId),
                     new SqlParameter("@colorSizeName", vesselGoodReturnItem.ColorSizeName),
                     new SqlParameter("@itemDimensionNumber", vesselGoodReturnItem.ItemDimensionNumber),
                     new SqlParameter("@itemGroupId", vesselGoodReturnItem.ItemGroupId),
                     new SqlParameter("@qty", vesselGoodReturnItem.Qty),
                     new SqlParameter("@uom", vesselGoodReturnItem.Uom),
                     new SqlParameter("@reason", vesselGoodReturnItem.Reason),
                     new SqlParameter("@createdBy", vesselGoodReturnItem.CreatedBy),
                     new SqlParameter("@createdDate", vesselGoodReturnItem.CreatedDate),
                     new SqlParameter("@syncStatus", vesselGoodReturnItem.SyncStatus),
                     new SqlParameter("@isHidden", vesselGoodReturnItem.IsHidden)
                     );
            }
        }

        public void UpdateTransaction(VesselGoodReturnItem vesselGoodReturnItem)
        {
            using (var context = new AppVesselInventoryContext())
            {
                StringBuilder execSp = new StringBuilder();
                execSp.Append("usp_VesselGoodReturnItem_UpdateTransaction");
                execSp.Append(" @vesselGoodReturnItemId,");
                execSp.Append(" @qty,");
                execSp.Append(" @reason,");
                execSp.Append(" @lastModifiedBy,");
                execSp.Append(" @lastModifiedDate");
                context.Database.ExecuteSqlCommand
                    (execSp.ToString(),
                     new SqlParameter("@vesselGoodReturnItemId", vesselGoodReturnItem.VesselGoodReturnItemId),
                     new SqlParameter("@qty", vesselGoodReturnItem.Qty),
                     new SqlParameter("@reason", vesselGoodReturnItem.Reason),
                     new SqlParameter("@lastModifiedBy", vesselGoodReturnItem.LastModifiedBy),
                     new SqlParameter("@lastModifiedDate", vesselGoodReturnItem.LastModifiedDate)
                     );
            }
        }
    }
}
