using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VesselInventory.DTO;
using VesselInventory.Models;

namespace VesselInventory.Helpers
{
    public static class GenericHelper
    {
        public static IEnumerable<lookup_value> GetLookupValues(string lookupType)
        {
            using (var context = new VesselInventoryContext()) {
                return context.Database.SqlQuery<lookup_value>("usp_LookupValue_GetCollectionByLookupType @p0",
                    parameters: new[] {
                        lookupType
                    }).ToList();
            }
        }

        public static IEnumerable<ItemGroupDimensionDTO> GetItems(string search_keyword = "")
        {
            using (var context = new VesselInventoryContext()) {
                return context.Database.SqlQuery<ItemGroupDimensionDTO>("usp_Item_GetItems @p0",
                    parameters: new[] {
                        search_keyword
                    }).ToList();
            }

        }
    }
}
