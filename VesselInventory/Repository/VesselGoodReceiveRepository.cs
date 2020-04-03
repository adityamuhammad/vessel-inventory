using System.Collections.Generic;
using System.Linq;
using VesselInventory.Models;

namespace VesselInventory.Repository
{
    public interface IVesselGoodReceiveRepository
    {
       IEnumerable<VesselGoodReceive> GetGoodReceive(string search_keyword, int page, int rows = 10);
       int GetGoodReceiveTotalPage(string search_keyword, int rows = 10);
    }

    public class VesselGoodReceiveRepository : 
        Repository<VesselGoodReceive>, 
        IVesselGoodReceiveRepository
    {
        public VesselGoodReceiveRepository() { }

        public IEnumerable<VesselGoodReceive> GetGoodReceive(string search_keyword , int page, int rows)
        {
            using (var context = new VesselInventoryContext())
            {
                return context.vessel_good_receive.SqlQuery (
                        "usp_VesselGoodReceive_GetGoodReceiveList @p0, @p1, @p2",
                        parameters: new[] {
                            search_keyword,
                            page.ToString(),
                            rows.ToString()
                    }).ToList();
            }
        }

        public int GetGoodReceiveTotalPage(string search_keyword, int rows = 10)
        {
            using (var context = new VesselInventoryContext())
            {
                return context.Database.SqlQuery<int>(
                        "usp_VesselGoodReceive_GetGoodReceivePages @p0, @p1",
                        parameters: new[] {
                            search_keyword,
                            rows.ToString()
                    }).Single();
            }
        }
    }
}
