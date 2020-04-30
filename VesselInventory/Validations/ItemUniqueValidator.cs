﻿using System;
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
            sqlStatementBuilder.Append("and ItemId = @itemId ");
            sqlStatementBuilder.Append("and ItemDimensionNumber = @itemDimensionNumber ");
            sqlStatementBuilder.Append("and IsHidden = 0");
            string sqlStatement = sqlStatementBuilder.ToString();
            return string.Format(sqlStatement, tableName, headerIdName);
        }
        public static bool ValidateRequestFormItem(RequestFormItem data)
        {
            using (var context =  new AppVesselInventoryContext())
            {
                return context.Database.SqlQuery<int>
                    (FetchOneRowSqlStatement("RequestFormItem", "RequestFormId"),
                    new SqlParameter("@id", data.RequestFormId),
                    new SqlParameter("@itemId", data.ItemId),
                    new SqlParameter("@itemDimensionNumber", data.ItemDimensionNumber))
                    .SingleOrDefault()
                    .Equals(1);
            }
            
        }

        public static bool ValidateVesselGoodIssuedItem(VesselGoodIssuedItem data)
        {
            using (var context =  new AppVesselInventoryContext())
            {
                return context.Database.SqlQuery<int>
                    (FetchOneRowSqlStatement("VesselGoodIssuedItem", "VesselGoodIssuedId"),
                    new SqlParameter("@id", data.VesselGoodIssuedId),
                    new SqlParameter("@itemId", data.ItemId),
                    new SqlParameter("@itemDimensionNumber", data.ItemDimensionNumber))
                    .SingleOrDefault()
                    .Equals(1);
            }
            
        }
        public static bool ValidateVesselGoodReceiveItemReject(VesselGoodReceiveItemReject data)
        {
            using (var context =  new AppVesselInventoryContext())
            {
                return context.Database.SqlQuery<int>
                    (FetchOneRowSqlStatement("VesselGoodReceiveItemReject", "VesselGoodReceiveId"),
                    new SqlParameter("@id", data.VesselGoodReceiveId),
                    new SqlParameter("@itemId", data.ItemId),
                    new SqlParameter("@itemDimensionNumber", data.ItemDimensionNumber))
                    .SingleOrDefault()
                    .Equals(1);
            }
            
        }
        public static bool ValidateVesselGoodReturnItem(VesselGoodReturnItem data)
        {
            using (var context =  new AppVesselInventoryContext())
            {
                return context.Database.SqlQuery<int>
                    (FetchOneRowSqlStatement("VesselGoodReturnItem", "VesselGoodReturnId"),
                    new SqlParameter("@id", data.VesselGoodReturnId),
                    new SqlParameter("@itemId", data.ItemId),
                    new SqlParameter("@itemDimensionNumber", data.ItemDimensionNumber))
                    .SingleOrDefault()
                    .Equals(1);
            }
            
        }
    }
}
