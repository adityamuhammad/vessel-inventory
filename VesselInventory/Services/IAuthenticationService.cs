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
        bool Authenticate(string username, string password);
    }
    class AuthenticationService : IAuthenticationService
    {
        public AuthenticationService() { }

        public bool Authenticate(string username, string password)
        {
            StringBuilder sqlAuthenticate = new StringBuilder();
            sqlAuthenticate.Append("select top 1 1 from UserVessel ");
            sqlAuthenticate.Append("where Username = @username ");
            sqlAuthenticate.Append("and Password = @password ");
            string sql = sqlAuthenticate.ToString();
            using (var context = new AppVesselInventoryContext())
            {
                return context.Database.SqlQuery<int>(sql,
                    new SqlParameter("@username", username),
                    new SqlParameter("@password", password)
                    ).SingleOrDefault().Equals(1);
            }
        }
    }
}
