using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;
using VesselInventory.Models;
using VesselInventory.Repository;
using VesselInventory.Services;
using VesselInventory.Utility;
using VesselInventory.Views;
using Unity;

namespace VesselInventory.ViewModel
{
    public class VesselGoodReceiveVM : ViewModelBase, IParentLoadable
    {
        private readonly IVesselGoodReceiveRepository _vesselGoodReceiveRepository;
        private readonly IWindowService _windowService;
        public RelayCommand NextPageCommand { get; private set; }
        public RelayCommand PrevPageCommand { get; private set; }
        public RelayCommand OpenDialogReceiveCommand { get; private set; }

        public VesselGoodReceiveVM(IWindowService windowService, IVesselGoodReceiveRepository vesselGoodReceiveRepository)
        {
            InitializeCommands();
            _windowService = windowService;
            _vesselGoodReceiveRepository = vesselGoodReceiveRepository;

            CurrentPage = 1;
            LoadDataGrid();
        }

        private void InitializeCommands()
        {
            NextPageCommand = new RelayCommand(NextPageAction, IsNextPageCanExecute);
            PrevPageCommand = new RelayCommand(PrevPageAction, IsPrevPageCanExecute);
            OpenDialogReceiveCommand = new RelayCommand(AddOrEditReceiveAction);
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
                CurrentPage = 1;
                LoadDataGrid();
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

        private void UpdateTotalPage()
        {
            TotalPage = _vesselGoodReceiveRepository
                .GetGoodReceiveTotalPage(SearchKeyword);
        }

        private IEnumerable<VesselGoodReceive> GoodReceives
        {
            get
            {
                return _vesselGoodReceiveRepository.
                    GetGoodReceive(SearchKeyword, CurrentPage);
            }
        }

        public ObservableCollection<VesselGoodReceive> VesselGoodReceiveCollection { get; } 
            = new ObservableCollection<VesselGoodReceive>();
        public void LoadDataGrid()
        {
            VesselGoodReceiveCollection.Clear();
            foreach (var _ in GoodReceives)
                VesselGoodReceiveCollection.Add(_);
            UpdateTotalPage();
        }

        public void AddOrEditReceiveAction(object parameter)
        {
            var container = ((App)Application.Current).UnityContainer;
            var vesselGoodReceiveAddOrEditVM = container.Resolve<VesselGoodReceiveAddOrEditVM>();
            if (parameter is null)
                vesselGoodReceiveAddOrEditVM.InitializeData(this);
            else
                vesselGoodReceiveAddOrEditVM.InitializeData(this, (int)parameter);
            _windowService.ShowWindow<VesselGoodReceive_AddOrEditView>
                    (vesselGoodReceiveAddOrEditVM);
        }
    }
}
