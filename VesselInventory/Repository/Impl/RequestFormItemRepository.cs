using System;
using System.Collections.Generic;
using System.Linq;
using VesselInventory.Dto;
using VesselInventory.Models;
using VesselInventory.Utility;

namespace VesselInventory.Repository.Impl
{
    public class RequestFormItemRepository 
        : GenericRepository<RequestFormItem>
        , IRequestFormItemRepository
    {
        public RequestFormItemRepository() { }

        public IEnumerable<RequestFormItem> GetRequestFormItemList(int requestFormId)
        {
            using (var context = new AppVesselInventoryContext())
            {
                return (from item in context.RequestFormItem
                        where item.RequestFormId == requestFormId 
                        && item.IsHidden == false
                        orderby item.CreatedDate 
                        descending select item)
                        .ToList();
            }
        }

        public IEnumerable<ItemStatusDto> GetItemStatusDataGrid(
            string itemId, string itemName, string itemStatus, 
            string requestFormNumber, string departmentName, 
            int page, int rows, string sortColumnName, string sortBy)
        {

            using (var context = new AppVesselInventoryContext())
            {
                return context.Database.SqlQuery<ItemStatusDto>(
                    "usp_RequestFormItem_GetItemStatusList @p0, @p1, @p2, @p3, @p4,@p5, @p6, @p7, @p8",
                    parameters: new object[] {
                        itemId, itemName,
                        itemStatus, requestFormNumber,
                        departmentName, page,
                        rows, sortColumnName, sortBy
                    }).ToList();
            }
        }
        public int GetItemStatusTotalPage (
            string itemId, string itemName, string itemStatus, 
            string requestFormNumber, string departmentName, int rows)
        {
            using (var context = new AppVesselInventoryContext())
            {
                return context.Database.SqlQuery<int>(
                    "usp_RequestFormItem_GetItemStatusPages @p0, @p1, @p2, @p3, @p4, @p5",
                    parameters: new object[] {
                        itemId,
                        itemName,
                        itemStatus,
                        requestFormNumber,
                        departmentName,
                        rows,
                    }).Single();
            }
        }

        public IEnumerable<ItemPendingDto> GetItemPendingDataGrid(
            string departmentName, string search, int page, int rows,
            string sortColumnName, string sortBy
            )
        {
            using (var context = new AppVesselInventoryContext())
            {
                return context.Database.SqlQuery<ItemPendingDto>(
                    "usp_RequestFormItem_GetItemPendingList @p0, @p1, @p2, @p3, @p4, @p5",
                    parameters: new object[] {departmentName, search, page, rows, sortColumnName, sortBy }).ToList();
            }
        }

        public int GetItemPendingTotalPage(string departmentName, string search, int rows)
        {
            using (var context = new AppVesselInventoryContext())
            {
                return context.Database.SqlQuery<int>(
                    "usp_RequestFormItem_GetItemPendingPages @p0, @p1, @p2",
                    parameters: new object[] {departmentName, search, rows }).Single();
            }
        }

        public override void Delete(int id)
        {
            using (var context = new AppVesselInventoryContext())
            {
                var current = context.RequestFormItem.Find(id);
                if (current is null) return;
                current.IsHidden = true;
                current.LastModifiedDate = DateTime.Now;
                current.LastModifiedBy = Auth.Instance.PersonName;
                context.SaveChanges();
            }
        }

        public LastRequestItemDto GetLastRequestItem(int itemId, string itemDimensionNumber)
        {
            using (var context = new AppVesselInventoryContext())
            {
                return context.Database.SqlQuery<LastRequestItemDto>(
                    "usp_RequestFormItem_GetLastItemRequest @p0, @p1",
                    parameters: new object[] {itemId, itemDimensionNumber}).SingleOrDefault();
            }
        }
        public IEnumerable<ItemStatusDto> GetItemStatusReport(string itemId, string itemName, string itemStatus, string requestFormNumber, string departmentName)
        {
            using (var context = new AppVesselInventoryContext())
            {
                return context.Database.SqlQuery<ItemStatusDto>(
                    "usp_RequestFormItem_ReportItemStatus @p0, @p1, @p2, @p3, @p4",
                    parameters: new object[] {
                        itemId, itemName,
                        itemStatus, requestFormNumber,
                        departmentName
                    }).ToList();
            }
        }

    }
}
