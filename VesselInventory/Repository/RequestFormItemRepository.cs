using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VesselInventory.DTO;
using VesselInventory.Models;

namespace VesselInventory.Repository
{
    public class RequestFormItemRepository : Repository<rf_item>
    {
        public RequestFormItemRepository() { }

        public IEnumerable<rf_item> GetRFItemList(int rf_id)
        {
            using (var context = new VesselInventoryContext())
            {
                return context.rf_item.SqlQuery(
                    "usp_RequestFormItem_GetRFItemList @p0",
                    parameters: new[] { rf_id.ToString() }
                    ).ToList();
            }
        }

        public IEnumerable<ItemStatusDTO> GetItemStatus
            (
                string item_id = "",
                string item_name = "",
                string item_group_name = "",
                string rf_number = "",
                string department_name = "",
                int page = 1, 
                int rows = 10
            )
        {
            using (var context = new VesselInventoryContext())
            {
                return context.Database.SqlQuery<ItemStatusDTO>(
                    "usp_RequestFormItem_GetItemStatus @p0, @p1, @p2, @p3, @p4,@p5, @p6",
                    parameters: new[]
                    {
                        item_id,
                        item_name,
                        item_group_name,
                        rf_number,
                        department_name,
                        page.ToString(),
                        rows.ToString(),
                    }).ToList();
            }
        }
        public int GetItemStatusTotalPage
            (
                string item_id = "",
                string item_name = "",
                string item_group_name = "",
                string rf_number = "",
                string department_name = "",
                int rows = 10
            )
        {
            using (var context = new VesselInventoryContext())
            {
                return context.Database.SqlQuery<int>(
                    "usp_RequestFormItem_GetItemStatusTotalPage @p0, @p1, @p2, @p3, @p4, @p5",
                    parameters: new[]
                    {
                        item_id,
                        item_name,
                        item_group_name,
                        rf_number,
                        department_name,
                        rows.ToString(),
                    }).Single();
            }
        }
    }
}
