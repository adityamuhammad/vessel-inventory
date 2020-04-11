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
    public class RequestFormVMBase : ViewModelBase
    {
        public RelayCommand SwitchTabCommand { get; private set; }
        public RequestFormVMBase()
        {
            SwitchTabCommand = new RelayCommand(SwitchTabAction);
        }
        private void SwitchTabAction(object parameter)
        {
            var container = ((App)Application.Current).UnityContainer;
            switch ((string)parameter)
            {
                case "List":
                    Navigate.To(container.Resolve<RequestFormVM>());
                    break;
                case "ItemStatus":
                    Navigate.To(container.Resolve<RequestFormItemStatusVM>());
                    break;
                case "ItemPending":
                    Navigate.To(container.Resolve<RequestFormItemPendingVM>());
                    break;
                default:
                    break;
            }
        }
    }
}
