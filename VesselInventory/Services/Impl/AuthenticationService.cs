using System.Linq;
using VesselInventory.Models;

namespace VesselInventory.Services.Impl
{
    class AuthenticationService : IAuthenticationService
    {
        public AuthenticationService() { }

        public UserVessel Authenticate(string username, string password)
        {
            using (var context = new AppVesselInventoryContext())
            {
                return (from user in context.UserVessel
                        where user.Username == username
                        && user.Password == password
                        select user).SingleOrDefault();
            }
        }
    }
}
