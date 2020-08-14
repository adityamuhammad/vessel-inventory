using System.Collections.ObjectModel;
using System.Windows;
using Unity;
using VesselInventory.Filters;
using VesselInventory.Models;
using VesselInventory.Repository;
using VesselInventory.Services;
using VesselInventory.Utility;
using VesselInventory.Views;

namespace VesselInventory.ViewModel
{
    class VesselGoodReturnVM : ViewModelBase, IParentLoadable
    {
        public override string Title => "Return Goods";
        public RelayCommand SearchCommand { get; private set; }
        public RelayCommand NextPageCommand { get; private set; }
        public RelayCommand PrevPageCommand { get; private set; }
        public RelayCommand OpenDialogReturnCommand { get; private set; }

        private readonly IVesselGoodReturnRepository _vesselGoodReturnRepository;
        private readonly IUnityContainer UnityContainer = ((App)Application.Current).UnityContainer;
        private readonly IWindowService _windowService;
        public VesselGoodReturnVM(IWindowService windowService, 
            IVesselGoodReturnRepository vesselGoodReturnRepository)
        {
            _vesselGoodReturnRepository = vesselGoodReturnRepository;
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
        public ObservableCollection<VesselGoodReturn> VesselGoodReturnCollection { get; } 
            = new ObservableCollection<VesselGoodReturn>();
        #endregion

        /// <summary>
        /// Load Method and behavior
        /// </summary>
        private PageFilter PageFilter
        {
            get => new PageFilter
            {
                Search = SearchKeyword,
                PageNum = CurrentPage,
                NumRows = DataGridRows,
                SortName = "VesselGoodReturnId",
                SortType = "DESC"
            };
        }
        public void LoadDataGrid()
        {
            VesselGoodReturnCollection.Clear();
            foreach (var goodReturn in _vesselGoodReturnRepository.GetGoodReturnDataGrid(PageFilter))
                VesselGoodReturnCollection.Add(goodReturn);
            UpdateTotalPage();
        }
        
        private void UpdateTotalPage()
        {
            TotalPage = _vesselGoodReturnRepository
                .GetGoodReturnTotalPage(PageFilter);
        }


        private void AddOrEditReturnAction(object parameter)
        {
            var vesselGoodReturnAddOrEditVM = UnityContainer.Resolve<VesselGoodReturnAddOrEditVM>();
            if (parameter is null)
                vesselGoodReturnAddOrEditVM.InitializeData(this);
            else
                vesselGoodReturnAddOrEditVM.InitializeData(this, (int)parameter);
            _windowService.ShowDialogWindow<VesselGoodReturn_AddOrEditView>(vesselGoodReturnAddOrEditVM);
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
        private void SearchAction(object parameter)
        {
            ResetCurrentPage();
            LoadDataGrid();
        }

        private void ResetCurrentPage() => CurrentPage = 1;
        private void IncrementCurrentPage() => CurrentPage = CurrentPage + 1;
        private void DecrementCurrentPage() => CurrentPage = CurrentPage - 1;
    }
}
