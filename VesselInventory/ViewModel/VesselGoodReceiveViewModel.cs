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

        public VesselGoodReceiveViewModel()
        {
            NextPageCommand = new RelayCommand(NextPageCommandAction, IsNextPageCanUse);
            PrevPageCommand = new RelayCommand(PrevPageCommandAction, IsPrevPageCanUse);
            OpenDialogReceiveCommand = new RelayCommand(Receive);

            _vesselGoodReceiveRepository = new VesselGoodReceiveRepository();
            _windowService = new WindowService();
            CurrentPage = 1;
            LoadGrid();
        }

        public ObservableCollection<VesselGoodReceive> VesselGoodReceiveCollection { get; } = new ObservableCollection<VesselGoodReceive>();
        private string _searchKeyword = string.Empty;
        public string SearchKeyword
        {
            get => _searchKeyword;
            set
            {
                _searchKeyword = value;
                OnPropertyChanged("SearchKeyword");
                CurrentPage = 1;
                LoadGrid();
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
            LoadGrid();
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
            LoadGrid();
        }

        private bool IsPrevPageCanUse(object parameter)
        {
            if(CurrentPage == 1)
                return false;
            return true;
        }

        private void UpdateTotalPage()
        {
            TotalPage = _vesselGoodReceiveRepository.GetGoodReceiveTotalPage(SearchKeyword);
        }

        public void LoadGrid()
        {
            VesselGoodReceiveCollection.Clear();
            foreach (var _ in _vesselGoodReceiveRepository.GetGoodReceive(SearchKeyword, CurrentPage))
                VesselGoodReceiveCollection.Add(_);
            UpdateTotalPage();
        }

        public void Receive(object parameter)
        {
            _windowService.ShowWindow<VesselGoodReceive_AddOrEditView>
                (new VesselGoodReceiveAddOrEditViewModel());
        }
    }
}
