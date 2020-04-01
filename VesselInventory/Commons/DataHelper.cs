using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VesselInventory.DTO;
using VesselInventory.Models;

namespace VesselInventory.Commons.HelperFunctions
{
    public static class DataHelper
    {
        public static IEnumerable<LookupValue> GetLookupValues(string lookupType)
        {
            using (var context = new VesselInventoryContext()) {
                return context.Database.SqlQuery<LookupValue>("usp_LookupValue_GetLookupValueList @p0",
                    parameters: new[] {
                        lookupType
                    }).ToList();
            }
        }

        public static IEnumerable<ItemGroupDimensionDTO> GetItems(string search_keyword = "")
        {
            using (var context = new VesselInventoryContext()) {
                return context.Database.SqlQuery<ItemGroupDimensionDTO>("usp_Item_GetItemList @p0",
                    parameters: new[] {
                        search_keyword
                    }).ToList();
            }
        }
    }
}
