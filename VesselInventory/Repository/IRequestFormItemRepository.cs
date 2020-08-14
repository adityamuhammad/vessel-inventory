using System.Collections.Generic;
using VesselInventory.Dto;
using VesselInventory.Filters;
using VesselInventory.Models;

namespace VesselInventory.Repository
{
    public interface IRequestFormItemRepository : IGenericRepository<RequestFormItem>
    {
        IEnumerable<RequestFormItem> GetRequestFormItemList(int requestFormId);
        IEnumerable<ItemStatusDto> GetItemStatusReport(RequestFormItemFilter requestFormItemFilter);

        IEnumerable<ItemStatusDto> GetItemStatusDataGrid(RequestFormItemFilter requestFormItemFilter, PageFilter pageFilter);
        int GetItemStatusTotalPage(RequestFormItemFilter requestFormItemFilter, PageFilter pageFilter);
        IEnumerable<ItemPendingDto> GetItemPendingDataGrid(string departmentName, PageFilter pageFilter);
        int GetItemPendingTotalPage(string departmentName,PageFilter pageFilter);
        LastRequestItemDto GetLastRequestItem(int itemId, string itemDimensionNumber);
    }

}
