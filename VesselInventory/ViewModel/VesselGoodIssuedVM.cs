using System;
using System.Collections.ObjectModel;
using VesselInventory.Models;
using VesselInventory.Repository;
using VesselInventory.Utility;

namespace VesselInventory.ViewModel
{
    class VesselGoodIssuedVM : ViewModelBase
    {
        private readonly IVesselGoodIssuedRepository _vesselGoodIssuedRepository;
        public RelayCommand NextPageCommand { get; private set; }
        public RelayCommand PrevPageCommand { get; private set; }
        public VesselGoodIssuedVM(IVesselGoodIssuedRepository vesselGoodIssuedRepository)
        {
            _vesselGoodIssuedRepository = vesselGoodIssuedRepository;
            InitializeCommands();
            ResetCurrentPage();
            LoadDataGrid();
        }
        private void InitializeCommands()
        {
            NextPageCommand = new RelayCommand(NextPageAction, IsNextPageCanExecute);
            PrevPageCommand = new RelayCommand(PrevPageAction, IsPrevPageCanExecute);
        }

        /// <summary>
        /// UI properties
        /// </summary>
        #region
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

        private string _searchKeyword = string.Empty;
        public string SearchKeyword
        {
            get => _searchKeyword;
            set
            {
                _searchKeyword = value;
                OnPropertyChanged("SearchKeyword");
                ResetCurrentPage();
                LoadDataGrid();
            }
        }
        #endregion

        /// <summary>
        /// Entity and Collections
        /// </summary>
        #region
        public ObservableCollection<VesselGoodIssued> VesselGoodIssuedCollection { get; } 
            = new ObservableCollection<VesselGoodIssued>();
        #endregion

        /// <summary>
        /// Load Method and behavior
        /// </summary>
        private void LoadDataGrid()
        {
            VesselGoodIssuedCollection.Clear();
            foreach (var goodIssued in _vesselGoodIssuedRepository
                .GetGoodIssued(SearchKeyword,CurrentPage))
                VesselGoodIssuedCollection.Add(goodIssued);
            UpdateTotalPage();
        }
        
        private void UpdateTotalPage()
        {
            TotalPage = _vesselGoodIssuedRepository
                .GetGoodIssuedTotalPage(SearchKeyword);
        }

        private void NextPageAction(object parameter)
        {
            IncrementCurrentPage();
            LoadDataGrid();
        }
        private bool IsNextPageCanExecute(object parameter) => !(CurrentPage >= TotalPage);

        private void PrevPageAction(object parameter)
        {
            DecrementCurrentPage();
            LoadDataGrid();
        }
        private bool IsPrevPageCanExecute(object parameter) => !(CurrentPage <= 1);

        private void ResetCurrentPage() => CurrentPage = 1;
        private void IncrementCurrentPage() => CurrentPage = CurrentPage + 1;
        private void DecrementCurrentPage() => CurrentPage = CurrentPage - 1;
    }
}
