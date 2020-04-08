using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using VesselInventory.Utility;
using Unity;

namespace VesselInventory.ViewModel
{
    public class RequestFormViewModelBase : ViewModelBase
    {
        public RelayCommand SwitchTabCommand { get; private set; }
        public RequestFormViewModelBase()
        {
            SwitchTabCommand = new RelayCommand(SwitchTabAction);
        }
        private void SwitchTabAction(object parameter)
        {
            var container = ((App)Application.Current).UnityContainer;
            switch ((string)parameter)
            {
                case "List":
                    Navigate.To(container.Resolve<RequestFormViewModel>());
                    break;
                case "ItemStatus":
                    Navigate.To(container.Resolve<RequestFormItemStatusViewModel>());
                    break;
                case "ItemPending":
                    Navigate.To(container.Resolve<RequestFormItemPendingViewModel>());
                    break;
                default:
                    break;
            }
        }
    }
}
