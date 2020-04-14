using System;
using System.Collections.Generic;
using System.Linq;
using VesselInventory.Commons.Enums;
using VesselInventory.DTO;
using VesselInventory.Models;
using System.Transactions;

namespace VesselInventory.Repository
{
    public interface IRequestFormRepository
    {
        IEnumerable<RequestForm> GetRequestFormList(string search, int page, int rows = 10);
        int GetRequestFormTotalPage(string search, int rows = 10);
        RequestFormShipBargeDTO GetRrequestFormShipBarge();
        RequestForm SaveRequestForm(RequestForm requestForm);
        RequestForm Update(int id, RequestForm requestForm);
        void Release(int id);
        RequestForm GetById(int id);
    }
    public class RequestFormRepository : 
        Repository<RequestForm>, 
        IRequestFormRepository
    {
        public RequestFormRepository() { }

        public RequestForm SaveRequestForm(RequestForm rf)
        {
            using (var scope = new TransactionScope())
            {
                base.Save(rf);
                using (var context = new VesselInventoryContext())
                {
                    context.Database.ExecuteSqlCommand("usp_DocSequence_IncrementSeqNumber 1");
                }
                scope.Complete();
                return rf;
            }
        }

        public IEnumerable<RequestForm> GetRequestFormList(string search = "",int page = 1,int rows = 10)
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

        public void Release(int id)
        {
            using (var context = new VesselInventoryContext())
            {
                var requestForm = context.rfs.Find(id);
                requestForm.status = Status.RELEASE.GetDescription();
                context.SaveChanges();
            }
        }
    }
}
