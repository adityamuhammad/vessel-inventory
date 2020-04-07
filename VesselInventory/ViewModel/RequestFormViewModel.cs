using System.Collections.ObjectModel;
using VesselInventory.Models;
using VesselInventory.Utility;
using VesselInventory.Repository;
using VesselInventory.Views;
using VesselInventory.Services;

namespace VesselInventory.ViewModel
{
    public class RequestFormViewModel : ViewModelBase, IParentLoadable
    {
        public RelayCommand NextPageCommand { get; private set; }
        public RelayCommand PrevPageCommand { get; private set; }
        public RelayCommand OpenDialogRequestFormCommand { get; private set; }
        public RelayCommand SwitchTabCommand { get; private set; }

        private readonly IWindowService _windowService;
        private readonly IRequestFormRepository _requestFormRepository;

        public RequestFormViewModel()
        {
            _windowService = new WindowService();
            _requestFormRepository = new RequestFormRepository();
            InitializeCommands();
            CurrentPage = 1;
            LoadGrid();
        }

        private void InitializeCommands()
        {
            NextPageCommand = new RelayCommand(NextPageCommandAction, IsNextPageCanUse);
            PrevPageCommand = new RelayCommand(PrevPageCommandAction, IsPrevPageCanUse);
            OpenDialogRequestFormCommand = new RelayCommand(OnOpenRequestForm);
            SwitchTabCommand = new RelayCommand(SwitchTabAction);
        }

        void UpdateTotalPage()
        {
            TotalPage = _requestFormRepository.GetRequestFormTotalPage(SearchKeyword);
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
        public ObservableCollection<RequestForm> RequestFormCollection { get; } = new ObservableCollection<RequestForm>();

        public void LoadGrid()
        {
            RequestFormCollection.Clear();
            foreach (var _ in _requestFormRepository.GetRequestFormList(SearchKeyword,CurrentPage))
                RequestFormCollection.Add(_);
            UpdateTotalPage();
        }
        public void OnOpenRequestForm(object parameter)
        {
            if (parameter  != null)
                _windowService.ShowWindow<RequestForm_AddOrEditView>
                    (new RequestFormAddOrEditViewModel(this,int.Parse(parameter.ToString())));
            else
                _windowService.ShowWindow<RequestForm_AddOrEditView>
                    (new RequestFormAddOrEditViewModel(this));
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
        private void SwitchTabAction(object parameter)
        {
            switch ((string)parameter)
            {
                case "List":
                    Navigate.To(new RequestFormViewModel());
                    break;
                case "ItemStatus":
                    Navigate.To(new RequestFormItemStatusViewModel());
                    break;
                case "ItemPending":
                    Navigate.To(new RequestFormItemPendingViewModel());
                    break;
                default:
                    Navigate.To(new RequestFormViewModel());
                    break;
            }
        }
    }
}
