using System.Collections.Generic;
using VesselInventory.Dto;

namespace VesselInventory.Repository
{
    public interface IOnHandRepository
    {
        IEnumerable<OnHandDto> GetOnHandDataGrid(string search, int page, int rows);
        int GetOnHandTotalPage(string search, int rows);
    }

}
