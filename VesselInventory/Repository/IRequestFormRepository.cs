using System;
using System.Collections.Generic;
using System.Linq;
using VesselInventory.Commons.Enums;
using VesselInventory.Dto;
using VesselInventory.Models;
using System.Transactions;
using VesselInventory.Utility;

namespace VesselInventory.Repository
{
    public interface IRequestFormRepository : IGenericRepository<RequestForm>
    {
        IEnumerable<RequestForm> GetRequestFormDataGrid(
            string departmentName, string search, int page, int rows, 
            string sortByColumnName, string sortBy);
        int GetRequestFormTotalPage(string departmentName, string search, int rows);
        RequestFormShipBargeDto GetRrequestFormShipBarge();
        RequestForm SaveTransaction(RequestForm requestForm);
        void Release(int id);
    }
    public class RequestFormRepository 
        : GenericRepository<RequestForm>
        , IRequestFormRepository
    {
        public RequestFormRepository() { }

        public RequestForm SaveTransaction(RequestForm rf)
        {
            using (var scope = new TransactionScope())
            {
                Save(rf);
                using (var context = new AppVesselInventoryContext())
                {
                    context.Database.ExecuteSqlCommand("usp_DocSequence_IncrementSeqNumber 1");
                }
                scope.Complete();
                return rf;
            }
        }

        public IEnumerable<RequestForm> GetRequestFormDataGrid(
            string departmentName, string search, int page, int rows,
            string sortColumnName, string sortBy)
        {
            using(var context = new AppVesselInventoryContext())
            {
                return context.RequestForm.SqlQuery(
                    "usp_RequestForm_GetRequestFormList @p0, @p1, @p2, @p3, @p4, @p5", 
                    parameters: new object[] { departmentName, search, page, rows, sortColumnName, sortBy }
                ).ToList();
            }
        }

        public int GetRequestFormTotalPage(string departmentName, string search, int rows)
        {
            using (var context = new AppVesselInventoryContext())
            {
                return context.Database.SqlQuery<int>(
                    "usp_RequestForm_GetRequestFormPages @p0, @p1, @p2",
                    parameters: new object[] {departmentName, search, rows }
                ).Single();
            }
        }

        public RequestFormShipBargeDto GetRrequestFormShipBarge()
        {
            using (var context = new AppVesselInventoryContext())
            {
                return context.Database.SqlQuery<RequestFormShipBargeDto>(
                    "usp_RequestForm_GetRequestFormShipBarge"
                ).Single();
            }
        }

        public void Release(int id)
        {
            using (var context = new AppVesselInventoryContext())
            {
                var requestForm = context.RequestForm.Find(id);
                requestForm.LastModifiedBy = Auth.Instance.PersonName;
                requestForm.LastModifiedDate = DateTime.Now;
                requestForm.Status = Status.Release.GetDescription();
                context.SaveChanges();
            }
        }

    }
}
