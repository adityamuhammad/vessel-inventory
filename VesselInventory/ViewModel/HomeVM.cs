using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using VesselInventory.Commons.HelperFunctions;
using VesselInventory.Dto;
using VesselInventory.Utility;
using Unity;

namespace VesselInventory.ViewModel
{
    class HomeVM : ViewModelBase
    {
        public override string Title => "Home";
        public ShipBargeDto ShipBarge => CommonDataHelper.GetShipBargeApairs();
        public RequestSummaryDto RequestSummary => CommonDataHelper.GetRequestSummaries();
        
        public RelayCommand MoreRequestCommand { get; private set; }
        public RelayCommand MoreItemRequestCommand { get; private set; }
        public HomeVM()
        {
            MoreRequestCommand = new RelayCommand(MoreRequest);
            MoreItemRequestCommand = new RelayCommand(MoreItemRequest);
        }

        private void MoreItemRequest(object obj)
        {
            var container = ((App)Application.Current).UnityContainer;
            Navigate.To(container.Resolve<RequestFormItemStatusVM>());
        }

        private void MoreRequest(object obj)
        {
            var container = ((App)Application.Current).UnityContainer;
            Navigate.To(container.Resolve<RequestFormVM>());
        }
    }
}
