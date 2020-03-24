using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VesselInventory.Models;

namespace VesselInventory.Repository
{
    public class RequestFormItemRepository : Repository<rf_item>
    {
        VesselInventoryContext _vesselInventoryContext;
        public RequestFormItemRepository(VesselInventoryContext vesselInventoryContext) 
            : base(vesselInventoryContext)
        {
            _vesselInventoryContext = vesselInventoryContext;
        }

        public IEnumerable<rf_item> GetRFItemList(int rf_id)
        {
            return _vesselInventoryContext.rf_item.SqlQuery(
                "usp_RequestFormItem_GetRFItemList @p0",
                parameters: new[] { rf_id.ToString() }
                ).ToList();
        }

    }
}
