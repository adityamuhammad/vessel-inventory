using System.Collections.ObjectModel;
using System.Windows;
using VesselInventory.Models;
using VesselInventory.Repository;
using VesselInventory.Utility;
using VesselInventory.Services;
using VesselInventory.Views;
using Unity;
using VesselInventory.Filters;

namespace VesselInventory.ViewModel
{
    class VesselGoodIssuedVM : ViewModelBase, IDataGrid
    {
        public override string Title => "Issued Goods";
        public ObservableCollection<VesselGoodIssued> VesselGoodIssuedCollection { get; } 
            = new ObservableCollection<VesselGoodIssued>();

        private readonly IVesselGoodIssuedRepository _vesselGoodIssuedRepository;
        private readonly IUnityContainer UnityContainer = ((App)Application.Current).UnityContainer;
        private readonly IWindowService _windowService;
        public RelayCommand SearchCommand { get; private set; }
        public RelayCommand NextPageCommand { get; private set; }
        public RelayCommand PrevPageCommand { get; private set; }
        public RelayCommand OpenDialogIssuedCommand { get; private set; }
        public VesselGoodIssuedVM(IVesselGoodIssuedRepository vesselGoodIssuedRepository,
            IWindowService windowService)
        {
            _vesselGoodIssuedRepository = vesselGoodIssuedRepository;
            _windowService = windowService;
            InitializeCommands();
            ResetCurrentPage();
            LoadDataGrid();
        }
        private void InitializeCommands()
        {
            SearchCommand = new RelayCommand(SearchAction);
            NextPageCommand = new RelayCommand(NextPageAction, IsNextPageCanExecute);
            PrevPageCommand = new RelayCommand(PrevPageAction, IsPrevPageCanExecute);
            OpenDialogIssuedCommand = new RelayCommand(AddOrEditIssuedAction);
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

        public int DataGridRows => 10;
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
            }
        }
        #endregion

        /// <summary>
        /// Entity and Collections
        /// </summary>
        #region
        #endregion

        /// <summary>
        /// Load Method and behavior
        /// </summary>
        #region
        public void LoadDataGrid()
        {
            VesselGoodIssuedCollection.Clear();
            foreach (var goodIssued in _vesselGoodIssuedRepository.GetGoodIssuedDataGrid(PageFilter))
                VesselGoodIssuedCollection.Add(goodIssued);
            UpdateTotalPage();
        }
        
        private PageFilter PageFilter
        {
            get => new PageFilter
            {
                Search = SearchKeyword,
                PageNum = CurrentPage,
                NumRows = DataGridRows,
                SortName = "VesselGoodIssuedId",
                SortType = "DESC"
            };
        }
        private void UpdateTotalPage()
        {
            TotalPage = _vesselGoodIssuedRepository.GetGoodIssuedTotalPage(PageFilter);
        }
        #endregion

        /// <summary>
        /// Button Actions
        /// </summary>
        /// <param name="parameter"></param>
        #region
        private void AddOrEditIssuedAction(object parameter)
        {
            var vesselGoodIssuedAddOrEditVM = UnityContainer.Resolve<VesselGoodIssuedAddOrEditVM>();
            if (parameter is null)
                vesselGoodIssuedAddOrEditVM.InitializeData(this);
            else
                vesselGoodIssuedAddOrEditVM.InitializeData(this, (int)parameter);
            _windowService.ShowDialogWindow<VesselGoodIssued_AddOrEditView>(vesselGoodIssuedAddOrEditVM);
        }
        private void SearchAction(object parameter)
        {
            ResetCurrentPage();
            LoadDataGrid();
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
        #endregion
    }
}
