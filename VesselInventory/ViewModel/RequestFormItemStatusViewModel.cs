using System.Collections.ObjectModel;
using VesselInventory.DTO;
using VesselInventory.Repository;
using VesselInventory.Utility;
using System.Collections.Generic;

namespace VesselInventory.ViewModel
{
    class RequestFormItemStatusViewModel : RequestFormViewModelBase
    {
        public RelayCommand NextPageCommand { get; private set; }
        public RelayCommand PrevPageCommand { get; private set; }

        private readonly IRequestFormItemRepository _requestFormItemRepository;

        public RequestFormItemStatusViewModel(IRequestFormItemRepository requestFormItemRepository)
        {
            _requestFormItemRepository = requestFormItemRepository;
            InitializeCommands();
            ResetCurrentPage();
            RefreshItemStatus();
        }

        private void InitializeCommands()
        {
            NextPageCommand = new RelayCommand(NextPageAction, IsNextPageCanExecute);
            PrevPageCommand = new RelayCommand(PrevPageAction, IsPrevPageCanExecute);
        }

        public ObservableCollection<ItemStatusDTO> ItemStatusCollection { get; } 
            = new ObservableCollection<ItemStatusDTO>();

        private void ResetCurrentPage()
        {
            CurrentPage = 1;
        }
        private void UpdateTotalPage()
        {
            TotalPage = _requestFormItemRepository.
                GetItemStatusTotalPage(
                    ItemIdSearch, 
                    ItemNameSearch, 
                    ItemStatusSearch, 
                    RFNumberSearch, 
                    DepartmentSearch
                );
        }

        private IEnumerable<ItemStatusDTO> ItemStatusList
        {
            get
            {
                return _requestFormItemRepository.
                    GetItemStatus(
                        ItemIdSearch,
                        ItemNameSearch,
                        ItemStatusSearch,
                        RFNumberSearch,
                        DepartmentSearch,
                        CurrentPage
                    );
            }
        }

        void RefreshItemStatus()
        {
            ItemStatusCollection.Clear();
            foreach (var _ in ItemStatusList)
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
                ResetCurrentPage();
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
                ResetCurrentPage();
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
                ResetCurrentPage();
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
                ResetCurrentPage();
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
                ResetCurrentPage();
                RefreshItemStatus();
            }
        }

        private void NextPageAction(object parameter)
        {
            IncrementCurrentPage();
            RefreshItemStatus();
        }

        private void IncrementCurrentPage() => CurrentPage = CurrentPage + 1;
        private void DecrementCurrentPage() => CurrentPage = CurrentPage - 1;
        private bool IsNextPageCanExecute(object parameter)
        {
            if(CurrentPage == TotalPage)
                return false;
            return true;
        }
        private void PrevPageAction(object parameter)
        {
            DecrementCurrentPage();
            RefreshItemStatus();
        }

        private bool IsPrevPageCanExecute(object parameter)
        {
            if(CurrentPage == 1)
                return false;
            return true;
        }
    }
}
