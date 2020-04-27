using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using VesselInventory.Models;

namespace VesselInventory.Validations
{
    public class ItemUniqueValidator
    {
        private static string FetchOneRowSqlStatement(string tableName, string headerIdName)
        {
            StringBuilder sqlStatementBuilder = new StringBuilder();
            sqlStatementBuilder.Append("select top 1 1 from {0} ");
            sqlStatementBuilder.Append("where {1} = @id ");
            sqlStatementBuilder.Append("and item_id = @item_id ");
            sqlStatementBuilder.Append("and item_dimension_number = @item_dimension_number ");
            sqlStatementBuilder.Append("and is_hidden = 0");
            string sqlStatement = sqlStatementBuilder.ToString();
            return string.Format(sqlStatement, tableName, headerIdName);
        }
        public static bool ValidateRequestFormItem(RequestFormItem data)
        {
            using (var context =  new VesselInventoryContext())
            {
                return context.Database.SqlQuery<int>
                    (FetchOneRowSqlStatement("rf_item", "rf_id"),
                    new SqlParameter("@id", data.rf_id),
                    new SqlParameter("@item_id", data.item_id),
                    new SqlParameter("@item_dimension_number", data.item_dimension_number))
                    .SingleOrDefault()
                    .Equals(1);
            }
            
        }

        public static bool ValidateVesselGoodIssuedItem(VesselGoodIssuedItem data)
        {
            using (var context =  new VesselInventoryContext())
            {
                return context.Database.SqlQuery<int>
                    (FetchOneRowSqlStatement("vessel_good_issued_item", "vessel_good_issued_id"),
                    new SqlParameter("@id", data.vessel_good_issued_id),
                    new SqlParameter("@item_id", data.item_id),
                    new SqlParameter("@item_dimension_number", data.item_dimension_number))
                    .SingleOrDefault()
                    .Equals(1);
            }
            
        }
        public static bool ValidateVesselGoodReceiveItemReject(VesselGoodReceiveItemReject data)
        {
            using (var context =  new VesselInventoryContext())
            {
                return context.Database.SqlQuery<int>
                    (FetchOneRowSqlStatement("vessel_good_receive_item_reject", "vessel_good_receive_id"),
                    new SqlParameter("@id", data.vessel_good_receive_id),
                    new SqlParameter("@item_id", data.item_id),
                    new SqlParameter("@item_dimension_number", data.item_dimension_number))
                    .SingleOrDefault()
                    .Equals(1);
            }
            
        }
    }
}
