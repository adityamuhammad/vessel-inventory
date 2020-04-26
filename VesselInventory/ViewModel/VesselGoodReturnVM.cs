using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VesselInventory.Models;
using VesselInventory.Repository;
using VesselInventory.Utility;

namespace VesselInventory.ViewModel
{
    class VesselGoodReturnVM : ViewModelBase
    {
        public RelayCommand NextPageCommand { get; private set; }
        public RelayCommand PrevPageCommand { get; private set; }
        public RelayCommand OpenDialogReturnCommand { get; private set; }

        private readonly IVesselGoodReturnRepository _vesselGoodReturnRepository;
        public VesselGoodReturnVM(IVesselGoodReturnRepository vesselGoodReturnRepository)
        {
            _vesselGoodReturnRepository = vesselGoodReturnRepository;
            InitializeCommands();
            ResetCurrentPage();
            LoadDataGrid();

        }
        private void InitializeCommands()
        {
            NextPageCommand = new RelayCommand(NextPageAction, IsNextPageCanExecute);
            PrevPageCommand = new RelayCommand(PrevPageAction, IsPrevPageCanExecute);
            OpenDialogReturnCommand = new RelayCommand(AddOrEditReturnAction);
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

        private int DataGridRows => 10;
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
        public ObservableCollection<VesselGoodReturn> VesselGoodReturnCollection { get; } 
            = new ObservableCollection<VesselGoodReturn>();
        #endregion

        /// <summary>
        /// Load Method and behavior
        /// </summary>
        public void LoadDataGrid()
        {
            VesselGoodReturnCollection.Clear();
            foreach (var goodReturn in _vesselGoodReturnRepository
                .GetGoodReturnDataGrid(SearchKeyword, CurrentPage, DataGridRows, "vessel_good_return_id", "DESC"))
                VesselGoodReturnCollection.Add(goodReturn);
            UpdateTotalPage();
        }
        
        private void UpdateTotalPage()
        {
            TotalPage = _vesselGoodReturnRepository
                .GetGoodReturnTotalPage(SearchKeyword, DataGridRows);
        }


        private void AddOrEditReturnAction(object obj)
        {
            throw new NotImplementedException();
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
