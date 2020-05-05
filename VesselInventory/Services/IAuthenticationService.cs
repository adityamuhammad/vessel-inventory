using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VesselInventory.Models;

namespace VesselInventory.Services
{
    public interface IAuthenticationService
    {
        UserVessel Authenticate(string username, string password);
    }
    class AuthenticationService : IAuthenticationService
    {
        public AuthenticationService() { }

        public UserVessel Authenticate(string username, string password)
        {
            StringBuilder sqlAuthenticate = new StringBuilder();
            sqlAuthenticate.Append("select top 1 UserId, Username, DepartmentName, ShipId from UserVessel ");
            sqlAuthenticate.Append("where Username = @username ");
            sqlAuthenticate.Append("and Password = @password ");
            string sql = sqlAuthenticate.ToString();
            using (var context = new AppVesselInventoryContext())
            {
                return context.Database.SqlQuery<UserVessel>(sql,
                    new SqlParameter("@username", username),
                    new SqlParameter("@password", password)
                    ).Single();
            }
        }
    }
}
