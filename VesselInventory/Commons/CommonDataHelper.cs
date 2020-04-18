using System.Collections.Generic;
using System.Linq;
using VesselInventory.Dto;
using VesselInventory.Models;

namespace VesselInventory.Commons.HelperFunctions
{
    public static class CommonDataHelper
    {
        public static IEnumerable<LookupValue> GetLookupValues(string lookupType)
        {
            using (var context = new VesselInventoryContext()) {
                return context.Database
                    .SqlQuery<LookupValue>
                    ("usp_LookupValue_GetLookupValueList @p0", lookupType).ToList();
            }
        }

        public static IEnumerable<ItemGroupDimensionDto> GetItems(string search_keyword)
        {
            using (var context = new VesselInventoryContext()) {
                return context.Database
                    .SqlQuery<ItemGroupDimensionDto>
                        ("usp_Item_GetItemList @p0", search_keyword).ToList();
            }
        }

        public static ShipBargeDto GetShipBargeApairs()
        {
            using (var context = new VesselInventoryContext())
            {
                return context.Database
                    .SqlQuery<ShipBargeDto>
                    ("usp_Generic_GetCurrentShipBargeApairs").Single();
            }
        }

        public static string GetSequenceNumber(int sequence_id)
        {
            using (var context = new VesselInventoryContext())
            {
                return context.Database
                    .SqlQuery<string>
                        ("usp_DocSequence_GetSequenceNumber @p0", sequence_id).Single();
            }
        }
    }
}
