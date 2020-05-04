﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VesselInventory.Dto;
using VesselInventory.Models;

namespace VesselInventory.Repository
{
    public interface IOnHandRepository
    {
        IEnumerable<OnHandDto> GetOnHandDataGrid(string search, int page, int rows);
        int GetOnHandTotalPage(string search, int rows);
    }

    public class OnHandRepository : IOnHandRepository
    {
        public IEnumerable<OnHandDto> GetOnHandDataGrid(string search, int page, int rows)
        {
            using(var context = new AppVesselInventoryContext())
            {
                return context.Database.SqlQuery<OnHandDto>(
                    "usp_OnHand_GetItemOnHandList @p0, @p1, @p2", 
                    parameters: new object[] { search, page, rows} 
                ).ToList();
            }
        }

        public int GetOnHandTotalPage(string search, int rows)
        {
            using (var context = new AppVesselInventoryContext())
            {
                return context.Database.SqlQuery<int>(
                    "usp_OnHand_GetItemOnHandPages @p0, @p1",
                    parameters: new object[] { search, rows }
                ).Single();
            }
        }
    }
}
