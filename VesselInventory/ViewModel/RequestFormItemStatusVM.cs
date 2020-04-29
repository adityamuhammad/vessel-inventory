using System.Collections.ObjectModel;
using VesselInventory.Dto;
using VesselInventory.Repository;
using VesselInventory.Utility;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace VesselInventory.ViewModel
{
    class RequestFormItemStatusVM : RequestFormVMBase
    {
        public override string Title => "Tracking Items";
        public RelayCommand NextPageCommand { get; private set; }
        public RelayCommand PrevPageCommand { get; private set; }

        private readonly IRequestFormItemRepository _requestFormItemRepository;

        public RequestFormItemStatusVM(IRequestFormItemRepository requestFormItemRepository)
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

        private int DataGridRows => 10;
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

        private string _itemIdSearch;
        public string ItemIdSearch
        {
            get => _itemIdSearch;
            set
            {
                Regex numericRegex = new Regex(@"^\d+$");
                _itemIdSearch = value;
                if (!numericRegex.IsMatch(value.ToString()) || _itemIdSearch.StartsWith("0"))
                    _itemIdSearch = null;

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
        public ObservableCollection<ItemStatusDto> ItemStatusCollection { get; } 
            = new ObservableCollection<ItemStatusDto>();


        private IEnumerable<ItemStatusDto> ItemStatusList
        {
            get
            {
                return _requestFormItemRepository.
                    GetItemStatusDataGrid(ItemIdSearch, ItemNameSearch,
                        ItemStatusSearch, RFNumberSearch,
                        DepartmentSearch, CurrentPage, DataGridRows, "RequestForm.RequestFormNumber",  "DESC");
            }
        }

        protected void RefreshItemStatus()
        {
            ItemStatusCollection.Clear();
            foreach (var _ in ItemStatusList)
                ItemStatusCollection.Add(_);
            UpdateTotalPage();
        }


        private int TotalPageFromDatabase
        {
            get
            {
                return _requestFormItemRepository.
                            GetItemStatusTotalPage(ItemIdSearch, 
                                ItemNameSearch, ItemStatusSearch, 
                                RFNumberSearch, DepartmentSearch, DataGridRows);

            }
        }

        private void UpdateTotalPage() => TotalPage = TotalPageFromDatabase;
        private void ResetCurrentPage() => CurrentPage = 1;
        private void IncrementCurrentPage() => CurrentPage = CurrentPage + 1;
        private void DecrementCurrentPage() => CurrentPage = CurrentPage - 1;
        private bool IsNextPageCanExecute(object parameter) => !(CurrentPage >= TotalPage);
        private bool IsPrevPageCanExecute(object parameter) => !(CurrentPage <= 1);

        private void NextPageAction(object parameter)
        {
            IncrementCurrentPage();
            RefreshItemStatus();
        }
        private void PrevPageAction(object parameter)
        {
            DecrementCurrentPage();
            RefreshItemStatus();
        }
    }
}
