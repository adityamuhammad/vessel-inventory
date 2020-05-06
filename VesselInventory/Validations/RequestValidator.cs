using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VesselInventory.Models;

namespace VesselInventory.Validations
{
    public class RequestValidator
    {

        public static bool IsAnyDraftDocument(string department, int shipId)
        {
            StringBuilder sqlBuilder = new StringBuilder();
            sqlBuilder.Append("select top 1 1 from RequestForm ");
            sqlBuilder.Append("where DepartmentName = @department ");
            sqlBuilder.Append("and ShipId = @shipId ");
            sqlBuilder.Append("and Status = 'DRAFT' ");
            sqlBuilder.Append("and IsHidden = 0 ");
            string sqlStatement = sqlBuilder.ToString();
            using (var context =  new AppVesselInventoryContext())
            {
                return context.Database.SqlQuery<int>
                    (sqlStatement,
                    new SqlParameter("@department", department),
                    new SqlParameter("@shipId", shipId))
                    .SingleOrDefault().Equals(1);
            }
        }

        public static bool IsAnyDocumentCreatedInThreeDays(string department, int shipId)
        {
            StringBuilder sqlBuilder = new StringBuilder();
            sqlBuilder.Append("select top 1 1 from RequestForm ");
            sqlBuilder.Append("where DepartmentName = @department ");
            sqlBuilder.Append("and ShipId = @shipId ");
            sqlBuilder.Append("and IsHidden = 0 ");
            sqlBuilder.Append("and CreatedDate > dateadd(day,-3,getdate()) ");
            string sqlStatement = sqlBuilder.ToString();
            using (var context =  new AppVesselInventoryContext())
            {
                return context.Database.SqlQuery<int>
                    (sqlStatement,
                    new SqlParameter("@department", department),
                    new SqlParameter("@shipId", shipId))
                    .SingleOrDefault().Equals(1);
            }
        }
    }
}
