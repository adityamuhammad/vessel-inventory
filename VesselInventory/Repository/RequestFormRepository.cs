using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VesselInventory.DTO;
using VesselInventory.Models;

namespace VesselInventory.Repository
{

    class RequestFormRepository : Repository<RF>
    {
        public RequestFormRepository() { }

        public IEnumerable<RF> GetRequestFormList(string search = "",int page = 1,int rows = 10)
        {
            using(var context = new VesselInventoryContext())
            {
                return context.rfs.SqlQuery(
                    "usp_RequestForm_GetRequestFormList @p0, @p1, @p2", 
                    parameters: new[] {
                        search,
                        page.ToString(),
                        rows.ToString()
                    }
                ).ToList();
            }
        }

        public int GetRequestFormTotalPage(string search = "", int rows = 10)
        {
            using (var context = new VesselInventoryContext())
            {
                return context.Database.SqlQuery<int>(
                    "usp_RequestForm_GetRequestFormPages @p0, @p1",
                    parameters: new[]
                    {
                        search,
                        rows.ToString()
                    }
                ).Single();
            }
        }

        public RequestFormShipBargeDTO GetRrequestFormShipBarge()
        {
            using (var context = new VesselInventoryContext())
            {
                return context.Database.SqlQuery<RequestFormShipBargeDTO>(
                    "usp_RequestForm_GetRequestFormShipBarge"
                ).Single();
            }
        }

        public RF SaveTransaction(RF rf)
        {
            base.Save(rf);
            using (var context = new VesselInventoryContext())
            {
                context.Database.ExecuteSqlCommand("usp_DocSequence_IncrementSeqNumber 1");
            }
            return rf;
        }
    }
}
