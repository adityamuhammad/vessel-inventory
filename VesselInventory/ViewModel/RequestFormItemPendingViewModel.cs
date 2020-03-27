using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VesselInventory.Utility;

namespace VesselInventory.ViewModel
{
    class RequestFormItemPendingViewModel : ViewModelBase
    {
        public RelayCommand SwitchTab { get; private set; }
        public RequestFormItemPendingViewModel()
        {
            SwitchTab = new RelayCommand(SwitchTabAction);
        }

        private void SwitchTabAction(object parameter)
        {
            switch ((string)parameter)
            {
                case "List":
                    Navigate.To(new RequestFormViewModel());
                    break;
                case "ItemStatus":
                    Navigate.To(new RequestFormItemStatusViewModel());
                    break;
                case "ItemPending":
                    Navigate.To(new RequestFormItemPendingViewModel());
                    break;
                default:
                    Navigate.To(new RequestFormViewModel());
                    break;
            }
        }
    }
}
