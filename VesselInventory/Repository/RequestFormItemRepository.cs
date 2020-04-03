﻿using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using VesselInventory.DTO;
using VesselInventory.Models;

namespace VesselInventory.Repository
{
    public interface IRequestFormItemRepository
    {
        RequestFormItem FindById(int id);
        RequestFormItem Save(RequestFormItem rFItem);
        RequestFormItem Update(int id, RequestFormItem rFItem);
        IEnumerable<RequestFormItem> GetRFItemList(int rf_id);
        IEnumerable<ItemStatusDTO> GetItemStatus(string item_id, string item_name, string item_status, string rf_number, string department_name, int page, int rows = 10);
        int GetItemStatusTotalPage(string item_id, string item_name, string item_status, string rf_number, string department_name, int rows = 10);
        IEnumerable<ItemPendingDTO> GetItemPending(string rf_number, int page, int rows = 10);
        int GetItemPendingTotalPage(string rf_number, int rows = 10);

    }
    public class RequestFormItemRepository : Repository<RequestFormItem>, IRequestFormItemRepository
    {
        public RequestFormItemRepository() { }

        public new RequestFormItem FindById(int id) => base.FindById(id);

        public new RequestFormItem Save(RequestFormItem rFItem) => base.Save(rFItem);

        public new RequestFormItem Update(int id, RequestFormItem rFItem) => base.Update(id, rFItem);

        public IEnumerable<RequestFormItem> GetRFItemList(int rf_id)
        {
            using (var context = new VesselInventoryContext())
            {
                return context.rf_item.SqlQuery(
                    "usp_RequestFormItem_GetRFItemList @p0",
                    parameters: new[] { rf_id.ToString() }
                    ).ToList();
            }
        }

        public IEnumerable<ItemStatusDTO> GetItemStatus ( string item_id = "", string item_name = "", string item_status = "", string rf_number = "", string department_name = "", int page = 1, int rows = 10 )
        {
            Regex numericRegex = new Regex(@"^\d+$");
            if (!numericRegex.IsMatch(item_id))
                item_id = "";

            using (var context = new VesselInventoryContext())
            {
                return context.Database.SqlQuery<ItemStatusDTO>(
                    "usp_RequestFormItem_GetItemStatusList @p0, @p1, @p2, @p3, @p4,@p5, @p6",
                    parameters: new[]
                    {
                        item_id,
                        item_name,
                        item_status,
                        rf_number,
                        department_name,
                        page.ToString(),
                        rows.ToString(),
                    }).ToList();
            }
        }
        public int GetItemStatusTotalPage ( string item_id = "", string item_name = "", string item_status = "", string rf_number = "", string department_name = "", int rows = 10 )
        {
            Regex numericRegex = new Regex(@"^\d+$");
            if (!numericRegex.IsMatch(item_id))
                item_id = "";

            using (var context = new VesselInventoryContext())
            {
                return context.Database.SqlQuery<int>(
                    "usp_RequestFormItem_GetItemStatusPages @p0, @p1, @p2, @p3, @p4, @p5",
                    parameters: new[]
                    {
                        item_id,
                        item_name,
                        item_status,
                        rf_number,
                        department_name,
                        rows.ToString(),
                    }).Single();
            }
        }

        public IEnumerable<ItemPendingDTO> GetItemPending(string rf_number = "", int page = 1, int rows = 10)
        {
            using (var context = new VesselInventoryContext())
            {
                return context.Database.SqlQuery<ItemPendingDTO>(
                    "usp_RequestFormItem_GetItemPendingList @p0, @p1, @p2",
                    parameters: new[]
                    {
                        rf_number,
                        page.ToString(),
                        rows.ToString(),
                    }).ToList();
            }
        }

        public int GetItemPendingTotalPage(string rf_number = "", int rows = 10)
        {
            using (var context = new VesselInventoryContext())
            {
                return context.Database.SqlQuery<int>(
                    "usp_RequestFormItem_GetItemPendingPages @p0, @p1",
                    parameters: new[]
                    {
                        rf_number,
                        rows.ToString(),
                    }).Single();
            }
        }
    }
}
