using System.Linq;
using VesselInventory.Models;

namespace VesselInventory.Services
{
    public interface IAuthenticationService
    {
        UserVessel Authenticate(string username, string password);
    }
}
