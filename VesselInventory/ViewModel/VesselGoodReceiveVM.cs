using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;
using VesselInventory.Models;
using VesselInventory.Repository;
using VesselInventory.Services;
using VesselInventory.Utility;
using VesselInventory.Views;
using Unity;
using VesselInventory.Filters;

namespace VesselInventory.ViewModel
{
    public class VesselGoodReceiveVM : ViewModelBase, IParentLoadable
    {
        public override string Title => "Receive Goods";
        private readonly IVesselGoodReceiveRepository _vesselGoodReceiveRepository;
        private readonly IWindowService _windowService;
        private readonly IUnityContainer UnityContainer = ((App)Application.Current).UnityContainer;
        public RelayCommand SearchCommand { get; private set; }
        public RelayCommand NextPageCommand { get; private set; }
        public RelayCommand PrevPageCommand { get; private set; }
        public RelayCommand OpenDialogReceiveCommand { get; private set; }
        public RelayCommand OpenDialogReceiveItemDetailCommand { get; private set; }

        public VesselGoodReceiveVM(IWindowService windowService, 
            IVesselGoodReceiveRepository vesselGoodReceiveRepository)
        {
            InitializeCommands();
            _windowService = windowService;
            _vesselGoodReceiveRepository = vesselGoodReceiveRepository;
            ResetCurrentPage();
            LoadDataGrid();
        }
        private void InitializeCommands()
        {
            SearchCommand = new RelayCommand(SearchAction);
            NextPageCommand = new RelayCommand(NextPageAction, IsNextPageCanExecute);
            PrevPageCommand = new RelayCommand(PrevPageAction, IsPrevPageCanExecute);
            OpenDialogReceiveCommand = new RelayCommand(AddOrEditReceiveAction);
            OpenDialogReceiveItemDetailCommand = new RelayCommand(ReceiveItemDetailAction);
        }


        /// <summary>
        /// UI Properties And Attributes
        /// </summary>
        #region
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
        #endregion


        /// <summary>
        /// Collections and load Method
        /// </summary>
        #region
        public int DataGridRows => 10;
        private PageFilter PageFilter
        {
            get => new PageFilter
            {
                Search = SearchKeyword,
                PageNum = CurrentPage,
                NumRows = DataGridRows,
                SortName = "VesselGoodReceiveId",
                SortType = "DESC" 
            };
        }
        private IEnumerable<VesselGoodReceive> GoodReceives
        {
            get => _vesselGoodReceiveRepository.GetGoodReceiveDataGrid(PageFilter);
        }

        public ObservableCollection<VesselGoodReceive> VesselGoodReceiveCollection { get; } 
            = new ObservableCollection<VesselGoodReceive>();
        public void LoadDataGrid()
        {
            VesselGoodReceiveCollection.Clear();
            foreach (var goodReceive in GoodReceives)
                VesselGoodReceiveCollection.Add(goodReceive);
            UpdateTotalPage();
        }
        #endregion

        /// <summary>
        /// Datagrid and Button Behavior
        /// </summary>
        #region
        private void UpdateTotalPage()
        {
            TotalPage = _vesselGoodReceiveRepository.GetGoodReceiveTotalPage(PageFilter);
        }
        private void ResetCurrentPage() => CurrentPage = 1;
        private void IncrementCurrentPage() => CurrentPage = CurrentPage + 1;
        private void DecrementCurrentPage() => CurrentPage = CurrentPage - 1;
        private bool IsNextPageCanExecute(object parameter) => !(CurrentPage >= TotalPage);
        private bool IsPrevPageCanExecute(object parameter) => !(CurrentPage <= 1);

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

        private void PrevPageAction(object parameter)
        {
            DecrementCurrentPage();
            LoadDataGrid();
        }

        public void AddOrEditReceiveAction(object parameter)
        {
            var vesselGoodReceiveAddOrEditVM = UnityContainer.Resolve<VesselGoodReceiveAddOrEditVM>();
            if (parameter is null)
                vesselGoodReceiveAddOrEditVM.InitializeData(this);
            else
                vesselGoodReceiveAddOrEditVM.InitializeData(this, (int)parameter);
            _windowService.ShowDialogWindow<VesselGoodReceive_AddOrEditView>
                    (vesselGoodReceiveAddOrEditVM);
        }

        private void ReceiveItemDetailAction(object parameter)
        {
            var vesselGoodReceiveItemVM = UnityContainer.Resolve<VesselGoodReceiveItemVM>();
            vesselGoodReceiveItemVM.InitializeData((int)parameter);
            _windowService.ShowDialogWindow<VesselGoodReceive_ItemDetailView>
                    (vesselGoodReceiveItemVM);
        }
        #endregion
    }
}
