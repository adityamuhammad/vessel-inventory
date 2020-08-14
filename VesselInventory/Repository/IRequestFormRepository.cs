using System.Collections.Generic;
using VesselInventory.Dto;
using VesselInventory.Filters;
using VesselInventory.Models;

namespace VesselInventory.Repository
{
    public interface IRequestFormRepository : IGenericRepository<RequestForm>
    {
        IEnumerable<RequestForm> GetRequestFormDataGrid(string departmentName, PageFilter pageFilter);
        int GetRequestFormTotalPage(string departmentName,PageFilter pageFilter);
        RequestFormShipBargeDto GetRrequestFormShipBarge();
        RequestForm SaveTransaction(RequestForm requestForm);
        void Release(int id);
    }
}
