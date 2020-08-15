using System.Data.SqlClient;
using System.Linq;
using System.Text.RegularExpressions;
using VesselInventory.Models;

namespace VesselInventory.Validations
{
    public class ItemMinimumQtyValidator
    {
        public static bool IsZeroQty(decimal qty)
        {
            Regex regex = new Regex(@"^\s*(?=.*[1-9])\d*(?:\.\d{1,2})?\s*$");
            return !regex.IsMatch(qty.ToString("#.##"));
        }

        public static bool IsStockAvailable(int itemId, string itemDimensionNumber, decimal qty, string typeDocument = "", int documentReferenceId =0)
        {
            using (var context =  new AppVesselInventoryContext())
            {
                decimal stock =  context.Database.SqlQuery<decimal>
                    ("usp_Item_CheckInStock @itemId, @itemDimensionNumber, @typeDocument, @documentReferenceId",
                    new SqlParameter("@itemId", itemId),
                    new SqlParameter("@itemDimensionNumber", itemDimensionNumber),
                    new SqlParameter("@typeDocument", typeDocument),
                    new SqlParameter("@documentReferenceId", documentReferenceId))
                    .SingleOrDefault();
                return (stock >= qty);
            }

        }
    }
}
