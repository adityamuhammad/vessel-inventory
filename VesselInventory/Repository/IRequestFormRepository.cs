using System.Collections.Generic;
using VesselInventory.Dto;
using VesselInventory.Models;

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
}
