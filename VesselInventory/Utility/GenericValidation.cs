using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Objects;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VesselInventory.Models;

namespace VesselInventory.Utility
{
    class GenericValidation
    {
        public bool ValidateIsUnique(string tableName, string keyToCheck, string valueToCheck)
        {
            using(var context = new VesselInventoryContext())
            {
            }
        }
    }
}
