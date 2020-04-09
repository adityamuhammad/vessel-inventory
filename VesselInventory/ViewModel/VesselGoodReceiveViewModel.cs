using System.Collections.Generic;
using System.Collections.ObjectModel;
using VesselInventory.Models;
using VesselInventory.Repository;
using VesselInventory.Services;
using VesselInventory.Utility;
using VesselInventory.Views;

namespace VesselInventory.ViewModel
{
    public class VesselGoodReceiveViewModel : ViewModelBase, IParentLoadable
    {
        private readonly IVesselGoodReceiveRepository _vesselGoodReceiveRepository;
        private readonly IWindowService _windowService;
        public RelayCommand NextPageCommand { get; private set; }
        public RelayCommand PrevPageCommand { get; private set; }
        public RelayCommand OpenDialogReceiveCommand { get; private set; }

        public VesselGoodReceiveViewModel(IWindowService windowService, IVesselGoodReceiveRepository vesselGoodReceiveRepository)
        {
            InitializeCommands();

            _windowService = windowService;
            _vesselGoodReceiveRepository = vesselGoodReceiveRepository;

            CurrentPage = 1;
            LoadDataGrid();
        }

        private void InitializeCommands()
        {
            NextPageCommand = new RelayCommand(NextPageCommandAction, IsNextPageCanUse);
            PrevPageCommand = new RelayCommand(PrevPageCommandAction, IsPrevPageCanUse);
            OpenDialogReceiveCommand = new RelayCommand(Receive);
        }

        public ObservableCollection<VesselGoodReceive> VesselGoodReceiveCollection { get; } 
            = new ObservableCollection<VesselGoodReceive>();
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
        private void NextPageCommandAction(object parameter)
        {
            CurrentPage = CurrentPage + 1;
            LoadDataGrid();
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
            LoadDataGrid();
        }

        private bool IsPrevPageCanUse(object parameter)
        {
            if(CurrentPage == 1)
                return false;
            return true;
        }

        private void UpdateTotalPage()
        {
            TotalPage = _vesselGoodReceiveRepository.
                GetGoodReceiveTotalPage(SearchKeyword);
        }

        private IEnumerable<VesselGoodReceive> GoodReceives
        {
            get
            {
                return _vesselGoodReceiveRepository.
                    GetGoodReceive(SearchKeyword, CurrentPage);
            }
        }

        public void LoadDataGrid()
        {
            VesselGoodReceiveCollection.Clear();
            foreach (var _ in GoodReceives)
                VesselGoodReceiveCollection.Add(_);
            UpdateTotalPage();
        }

        public void Receive(object parameter)
        {
            if(parameter is null)
                _windowService.ShowWindow<VesselGoodReceive_AddOrEditView>
                    (new VesselGoodReceiveAddOrEditViewModel(this));
            else
                _windowService.ShowWindow<VesselGoodReceive_AddOrEditView>
                    (new VesselGoodReceiveAddOrEditViewModel(this,(int)parameter));
        }
    }
}
