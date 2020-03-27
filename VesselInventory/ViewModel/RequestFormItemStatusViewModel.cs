using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VesselInventory.DTO;
using VesselInventory.Models;
using VesselInventory.Repository;
using VesselInventory.Utility;

namespace VesselInventory.ViewModel
{
    class RequestFormItemStatusViewModel : ViewModelBase
    {
        private readonly RequestFormItemRepository _requestFormItemRepository;
        public RelayCommand NextPageCommand { get; private set; }
        public RelayCommand PrevPageCommand { get; private set; }
        public RelayCommand SwitchTab { get; private set; }
        public RequestFormItemStatusViewModel()
        {
            _requestFormItemRepository = new RequestFormItemRepository();
            SwitchTab = new RelayCommand(SwitchTabAction);
            NextPageCommand = new RelayCommand(NextPageCommandAction,IsNextPageCanUse);
            PrevPageCommand = new RelayCommand(PrevPageCommandAction,IsPrevPageCanUse);
            CurrentPage = 1;
            UpdateTotalPage();
            RefreshItemStatus();
        }

        private ObservableCollection<ItemStatusDTO> _itemStatusColelction = new ObservableCollection<ItemStatusDTO>();
        public ObservableCollection<ItemStatusDTO> ItemStatusCollection
        {
            get => _itemStatusColelction;
        }

        void UpdateTotalPage()
        {
            TotalPage = _requestFormItemRepository.GetItemStatusTotalPage(ItemIdSearch, ItemNameSearch, GroupSearch, RFNumberSearch, DepartmentSearch);
        }

        void RefreshItemStatus()
        {
            _itemStatusColelction.Clear();
            foreach (var _ in _requestFormItemRepository.GetItemStatus(ItemIdSearch,ItemNameSearch,GroupSearch,RFNumberSearch,DepartmentSearch,CurrentPage))
            {
                _itemStatusColelction.Add(new ItemStatusDTO
                {
                    item_id = _.item_id,
                    department_name = _.department_name,
                    item_description = _.item_description,
                    item_group_name = _.item_group_name,
                    priority = _.priority,
                    qty = _.qty,
                    rf_number = _.rf_number,
                    status = _.status,
                    sync_status = _.sync_status,
                    target_delivery_date = _.target_delivery_date,
                    uom = _.uom
                });
            }
        }

        private int _currentPage;
        public int CurrentPage
        {
            get => _currentPage;
            set
            {
                _currentPage = value;
                OnPropertyChanged("CurrentPage");
            }
        }

        private int _totalPage;
        public int TotalPage
        {
            get => _totalPage;
            set
            {
                _totalPage = value;
                OnPropertyChanged("TotalPage");
            }
        }

        private string _rfNumberSearch = string.Empty;
        public string RFNumberSearch
        {
            get => _rfNumberSearch;
            set
            {
                _rfNumberSearch = value;
                OnPropertyChanged("RFNumberSearch");
                CurrentPage = 1;
                UpdateTotalPage();
                RefreshItemStatus();
            }
        }

        private string _itemIdSearch = string.Empty;
        public string ItemIdSearch
        {
            get => _itemIdSearch;
            set
            {
                _itemIdSearch = value;
                OnPropertyChanged("ItemIdSearch");
                CurrentPage = 1;
                UpdateTotalPage();
                RefreshItemStatus();
            }
        }

        private string _itemNameSearch = string.Empty;
        public string ItemNameSearch
        {
            get => _itemNameSearch;
            set
            {
                _itemNameSearch = value;
                OnPropertyChanged("ItemNameSearch");
                CurrentPage = 1;
                UpdateTotalPage();
                RefreshItemStatus();
            }
        }

        private string _groupSearch = string.Empty;
        public string GroupSearch
        {
            get => _groupSearch;
            set
            {
                _groupSearch = value;
                OnPropertyChanged("GroupSearch");
                CurrentPage = 1;
                UpdateTotalPage();
                RefreshItemStatus();
            }
        }

        private string _departmentSearch = string.Empty;
        public string DepartmentSearch
        {
            get => _departmentSearch;
            set
            {
                _departmentSearch = value;
                OnPropertyChanged("DepartmentSearch");
                CurrentPage = 1;
                UpdateTotalPage();
                RefreshItemStatus();
            }
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
        private void NextPageCommandAction(object parameter)
        {
            CurrentPage = CurrentPage + 1;
            RefreshItemStatus();
        }

        private bool IsNextPageCanUse(object parameter)
        {
            if(CurrentPage == TotalPage)
                return false;
            return true;
        }
        private void PrevPageCommandAction(object parameter)
        {
            CurrentPage = CurrentPage - 1;
            RefreshItemStatus();
        }

        private bool IsPrevPageCanUse(object parameter)
        {
            if(CurrentPage == 1)
                return false;
            return true;
        }
    }
}
