﻿using System.Collections.ObjectModel;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using VesselInventory.Dto;
using VesselInventory.Repository;
using VesselInventory.Utility;
using VesselInventory.Services;
using VesselInventory.Reports;
using CrystalDecisions.CrystalReports.Engine;
using VesselInventory.Filters;
using System.Diagnostics;

namespace VesselInventory.ViewModel
{
    class RequestFormItemStatusVM : RequestFormVMBase
    {
        public override string Title => "Tracking Items";
        public ObservableCollection<ItemStatusDto> ItemStatusCollection { get; } 
            = new ObservableCollection<ItemStatusDto>();
        public RelayCommand SearchCommand { get; private set; }
        public RelayCommand NextPageCommand { get; private set; }
        public RelayCommand PrevPageCommand { get; private set; }
        public RelayCommand ReportCommand { get; private set; }
        public ReportDocument Report { get; private set; }

        private readonly IWindowService _windowService;
        private readonly IRequestFormItemRepository _requestFormItemRepository;

        public RequestFormItemStatusVM(IWindowService windowService, IRequestFormItemRepository requestFormItemRepository)
        {
            _requestFormItemRepository = requestFormItemRepository;
            _windowService = windowService;

            PresentationTraceSources.DataBindingSource.Switch.Level = SourceLevels.Critical;
            Report = new TrackRequestItemReport();

            InitializeCommands();
            ResetCurrentPage();
            LoadDataGrid();
        }

        private void InitializeCommands()
        {
            SearchCommand = new RelayCommand(SearchAction);
            ReportCommand = new RelayCommand(ReportTracking);
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
            }
        }


        private PageFilter PageFilter
        {
            get => new PageFilter
            {
                PageNum = CurrentPage,
                NumRows = DataGridRows,
                SortName = "RequestForm.RequestFormNumber",
                SortType = "DESC"
            };
        }
        private IEnumerable<ItemStatusDto> ItemStatusList
        {
            get => _requestFormItemRepository.GetItemStatusDataGrid(RequestFormItemFilter, PageFilter);
        }

        protected void LoadDataGrid()
        {
            ItemStatusCollection.Clear();
            foreach (var _ in ItemStatusList)
                ItemStatusCollection.Add(_);
            UpdateTotalPage();
        }


        private int TotalPageFromDatabase
        {
            get => _requestFormItemRepository.GetItemStatusTotalPage(RequestFormItemFilter, PageFilter);
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
            LoadDataGrid();
        }
        private void PrevPageAction(object parameter)
        {
            DecrementCurrentPage();
            LoadDataGrid();
        }
        private void SearchAction(object parameter)
        {
            ResetCurrentPage();
            LoadDataGrid();
        }

        private RequestFormItemFilter RequestFormItemFilter
        {
            get => new RequestFormItemFilter
            {
                ItemId = ItemIdSearch,
                ItemStatus = ItemStatusSearch,
                ItemName = ItemNameSearch,
                DepartmentName = DepartmentSearch,
                RequestFormNumber = RFNumberSearch
            };
        }
        private void ReportTracking(object parameter)
        {
            var dataReport = _requestFormItemRepository.GetItemStatusReport(RequestFormItemFilter);
            Report.SetDataSource(dataReport);
            _windowService.ShowDialogWindow<ReportView>(this);
        }

    }
}
