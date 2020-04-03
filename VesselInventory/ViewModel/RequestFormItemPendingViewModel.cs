using System.Collections.ObjectModel;
using VesselInventory.DTO;
using VesselInventory.Repository;
using VesselInventory.Services;
using VesselInventory.Utility;
using VesselInventory.Views;

namespace VesselInventory.ViewModel
{
    class RequestFormItemPendingViewModel : ViewModelBase, IParentLoadable
    {
        public RelayCommand SwitchTab { get; private set; }
        public RelayCommand NextPageCommand { get; private set; }
        public RelayCommand PrevPageCommand { get; private set; }
        public RelayCommand UploadDocumentForm { get; private set; }

        private readonly IWindowService _windowService;
        private readonly IRequestFormItemRepository _requestFormItemRepository;
        public RequestFormItemPendingViewModel()
        {
            _requestFormItemRepository = new RequestFormItemRepository();
            _windowService = new WindowService();

            SetCommands();
            CurrentPage = 1;
            LoadGrid();
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

        private void SetCommands()
        {
            SwitchTab = new RelayCommand(SwitchTabAction);
            NextPageCommand = new RelayCommand(NextPageCommandAction, IsNextPageCanUse);
            PrevPageCommand = new RelayCommand(PrevPageCommandAction, IsPrevPageCanUse);
            UploadDocumentForm = new RelayCommand(OnOpenUploadDocumentForm);
        }

        public ObservableCollection<ItemPendingDTO> ItemPendingCollection { get; } = new ObservableCollection<ItemPendingDTO>();
        public void LoadGrid()
        {
            ItemPendingCollection.Clear();
            foreach (var _ in _requestFormItemRepository.GetItemPending(SearchKeyword,CurrentPage))
                ItemPendingCollection.Add(_);
            UpdateTotalPage();
        }

        private void UpdateTotalPage()
        {
            TotalPage = _requestFormItemRepository.GetItemPendingTotalPage(SearchKeyword);
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

        private void OnOpenUploadDocumentForm(object parameter)
        {
            _windowService.ShowWindow<RequestForm_ItemUploadDocumentView>
                (new RequestFormItemUploadDocument(this,int.Parse(parameter.ToString())));
        }
    }
}
