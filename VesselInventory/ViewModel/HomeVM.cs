using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VesselInventory.Commons.HelperFunctions;
using VesselInventory.Dto;

namespace VesselInventory.ViewModel
{
    class HomeVM : ViewModelBase
    {
        public override string Title => "Home";
        public ShipBargeDto ShipBarge => CommonDataHelper.GetShipBargeApairs();
        public HomeVM()
        {
        }
    }
}
