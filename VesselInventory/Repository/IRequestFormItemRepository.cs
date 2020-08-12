using System.Collections.Generic;
using VesselInventory.Dto;
using VesselInventory.Models;

namespace VesselInventory.Repository
{
    public interface IRequestFormItemRepository : IGenericRepository<RequestFormItem>
    {
        IEnumerable<RequestFormItem> GetRequestFormItemList(int requestFormId);
        IEnumerable<ItemStatusDto> GetItemStatusReport(
            string itemId, string itemName,
            string itemStatus, string requestFormNumber,
            string departmentName);

        IEnumerable<ItemStatusDto> GetItemStatusDataGrid(
            string itemId, string itemName, 
            string itemStatus, string requestFormNumber, 
            string departmentName, int page, int rows,
            string sortColumnName, string sortBy);
        int GetItemStatusTotalPage(
            string itemId, string itemName, 
            string itemStatus, string requestFormNumber, 
            string departmentName, int rows);
        IEnumerable<ItemPendingDto> GetItemPendingDataGrid(
            string departmentName, string search, int page, int rows, 
            string sortColumnName, string sortBy);
        int GetItemPendingTotalPage(string departmentName, string search, int rows);
        LastRequestItemDto GetLastRequestItem(int itemId, string itemDimensionNumber);
    }

}
