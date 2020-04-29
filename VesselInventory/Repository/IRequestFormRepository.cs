﻿using System;
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
            string search, int page, int rows, 
            string sortByColumnName, string sortBy);
        int GetRequestFormTotalPage(string search, int rows = 10);
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
                using (var context = new VesselInventoryContext())
                {
                    context.Database.ExecuteSqlCommand("usp_DocSequence_IncrementSeqNumber 1");
                }
                scope.Complete();
                return rf;
            }
        }

        public IEnumerable<RequestForm> GetRequestFormDataGrid(
            string search, int page, int rows,
            string sortColumnName, string sortBy)
        {
            using(var context = new VesselInventoryContext())
            {
                return context.RequestForm.SqlQuery(
                    "usp_RequestForm_GetRequestFormList @p0, @p1, @p2, @p3, @p4", 
                    parameters: new object[] { search, page, rows, sortColumnName, sortBy }
                ).ToList();
            }
        }

        public int GetRequestFormTotalPage(string search, int rows = 10)
        {
            using (var context = new VesselInventoryContext())
            {
                return context.Database.SqlQuery<int>(
                    "usp_RequestForm_GetRequestFormPages @p0, @p1",
                    parameters: new object[] { search, rows }
                ).Single();
            }
        }

        public RequestFormShipBargeDto GetRrequestFormShipBarge()
        {
            using (var context = new VesselInventoryContext())
            {
                return context.Database.SqlQuery<RequestFormShipBargeDto>(
                    "usp_RequestForm_GetRequestFormShipBarge"
                ).Single();
            }
        }

        public void Release(int id)
        {
            using (var context = new VesselInventoryContext())
            {
                var requestForm = context.RequestForm.Find(id);
                requestForm.last_modified_by = Auth.Instance.personalname;
                requestForm.last_modified_date = DateTime.Now;
                requestForm.status = Status.Release.GetDescription();
                context.SaveChanges();
            }
        }
    }
}
