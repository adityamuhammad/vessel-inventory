using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using VesselInventory.Dto;
using VesselInventory.Models;
using VesselInventory.Utility;

namespace VesselInventory.Repository
{
    public interface IRequestFormItemRepository : IGenericRepository<RequestFormItem>
    {
        IEnumerable<RequestFormItem> GetRequestFormItemList(int rf_id);
        IEnumerable<ItemStatusDto> GetItemStatusDataGrid(
            string item_id, string item_name, 
            string item_status, string rf_number, 
            string department_name, int page, int rows,
            string sortColumnName, string sortBy);
        int GetItemStatusTotalPage(
            string item_id, string item_name, 
            string item_status, string rf_number, 
            string department_name, int rows);
        IEnumerable<ItemPendingDto> GetItemPendingDataGrid(
            string rf_number, int page, int rows, 
            string sortColumnName, string sortBy);
        int GetItemPendingTotalPage(string rf_number, int rows);
    }

    public class RequestFormItemRepository : 
        GenericRepository<RequestFormItem>, 
        IRequestFormItemRepository
    {
        public RequestFormItemRepository() { }

        public IEnumerable<RequestFormItem> GetRequestFormItemList(int rf_id)
        {
            using (var context = new VesselInventoryContext())
            {
                return (from item in context.request_form_item
                        where item.rf_id == rf_id && 
                        item.is_hidden == false select item)
                        .ToList();
            }
        }

        public IEnumerable<ItemStatusDto> GetItemStatusDataGrid(string item_id, 
            string item_name, string item_status, 
            string rf_number, string department_name, 
            int page, int rows, string sortColumnName, string sortBy)
        {

            using (var context = new VesselInventoryContext())
            {
                return context.Database.SqlQuery<ItemStatusDto>(
                    "usp_RequestFormItem_GetItemStatusList @p0, @p1, @p2, @p3, @p4,@p5, @p6, @p7, @p8",
                    parameters: new object[] {
                        item_id, item_name,
                        item_status, rf_number,
                        department_name, page,
                        rows, sortColumnName, sortBy
                    }).ToList();
            }
        }
        public int GetItemStatusTotalPage (string item_id, 
            string item_name, string item_status, 
            string rf_number, string department_name, 
            int rows)
        {
            using (var context = new VesselInventoryContext())
            {
                return context.Database.SqlQuery<int>(
                    "usp_RequestFormItem_GetItemStatusPages @p0, @p1, @p2, @p3, @p4, @p5",
                    parameters: new object[] {
                        item_id,
                        item_name,
                        item_status,
                        rf_number,
                        department_name,
                        rows,
                    }).Single();
            }
        }

        public IEnumerable<ItemPendingDto> GetItemPendingDataGrid(
            string rf_number, int page, int rows,
            string sortColumnName, string sortBy
            )
        {
            using (var context = new VesselInventoryContext())
            {
                return context.Database.SqlQuery<ItemPendingDto>(
                    "usp_RequestFormItem_GetItemPendingList @p0, @p1, @p2, @p3, @p4",
                    parameters: new object[] { rf_number, page, rows, sortColumnName, sortBy }).ToList();
            }
        }

        public int GetItemPendingTotalPage(string rf_number, int rows)
        {
            using (var context = new VesselInventoryContext())
            {
                return context.Database.SqlQuery<int>(
                    "usp_RequestFormItem_GetItemPendingPages @p0, @p1",
                    parameters: new object[] { rf_number, rows }).Single();
            }
        }

        public override void Delete(int id)
        {
            using (var context = new VesselInventoryContext())
            {
                var current = context.request_form_item.Find(id);
                if (current is null) return;
                current.is_hidden = true;
                current.last_modified_date = DateTime.Now;
                current.last_modified_by = Auth.Instance.personalname;
                context.SaveChanges();
            }
        }
    }
}
