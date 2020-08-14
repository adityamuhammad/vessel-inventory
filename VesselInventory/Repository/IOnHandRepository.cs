using System.Collections.Generic;
using VesselInventory.Dto;
using VesselInventory.Filters;

namespace VesselInventory.Repository
{
    public interface IOnHandRepository
    {
        IEnumerable<OnHandDto> GetOnHandDataGrid(PageFilter pageFilter);
        int GetOnHandTotalPage(PageFilter pageFilter);
    }

}
