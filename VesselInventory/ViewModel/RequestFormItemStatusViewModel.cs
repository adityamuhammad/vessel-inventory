using System.Collections.ObjectModel;
using VesselInventory.DTO;
using VesselInventory.Repository;
using VesselInventory.Utility;

namespace VesselInventory.ViewModel
{
    class RequestFormItemStatusViewModel : ViewModelBase
    {
        public RelayCommand NextPageCommand { get; private set; }
        public RelayCommand PrevPageCommand { get; private set; }
        public RelayCommand SwitchTab { get; private set; }

        private readonly IRequestFormItemRepository _requestFormItemRepository;

        public RequestFormItemStatusViewModel()
        {
            _requestFormItemRepository = new RequestFormItemRepository();
            SetCommands();
            CurrentPage = 1;
            RefreshItemStatus();
        }

        private void SetCommands()
        {
            SwitchTab = new RelayCommand(SwitchTabAction);
            NextPageCommand = new RelayCommand(NextPageCommandAction, IsNextPageCanUse);
            PrevPageCommand = new RelayCommand(PrevPageCommandAction, IsPrevPageCanUse);
        }
        public ObservableCollection<ItemStatusDTO> ItemStatusCollection { get; } = new ObservableCollection<ItemStatusDTO>();

        void UpdateTotalPage()
        {
            TotalPage = _requestFormItemRepository.GetItemStatusTotalPage(ItemIdSearch, ItemNameSearch, ItemStatusSearch, RFNumberSearch, DepartmentSearch);
        }

        void RefreshItemStatus()
        {
            ItemStatusCollection.Clear();
            foreach (var _ in _requestFormItemRepository.GetItemStatus(ItemIdSearch,ItemNameSearch,ItemStatusSearch,RFNumberSearch,DepartmentSearch,CurrentPage))
                ItemStatusCollection.Add(_);
            UpdateTotalPage();
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
                RefreshItemStatus();
            }
        }

        private string _itemStatusSearch = string.Empty;
        public string ItemStatusSearch
        {
            get => _itemStatusSearch;
            set
            {
                _itemStatusSearch = value;
                OnPropertyChanged("ItemStatusSearch");
                CurrentPage = 1;
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
