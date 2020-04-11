using System.Collections.ObjectModel;
using System.Windows;
using VesselInventory.DTO;
using VesselInventory.Repository;
using VesselInventory.Services;
using VesselInventory.Utility;
using VesselInventory.Views;
using Unity;

namespace VesselInventory.ViewModel
{
    public class RequestFormItemPendingVM : RequestFormVMBase, IParentLoadable
    {
        public RelayCommand NextPageCommand { get; private set; }
        public RelayCommand PrevPageCommand { get; private set; }
        public RelayCommand UploadDocumentFormCommand { get; private set; }

        private readonly IWindowService _windowService;
        private readonly IRequestFormItemRepository _requestFormItemRepository;
        public RequestFormItemPendingVM(IWindowService windowService, IRequestFormItemRepository requestFormItemRepository)
        {
            _windowService = windowService;
            _requestFormItemRepository = requestFormItemRepository;

            InitializeCommands();
            CurrentPage = 1;
            LoadDataGrid();
        }

        private void InitializeCommands()
        {
            NextPageCommand = new RelayCommand(NextPageAction, IsNextPageCanExecute);
            PrevPageCommand = new RelayCommand(PrevPageAction, IsPrevPageCanExecute);
            UploadDocumentFormCommand = new RelayCommand(OpenUploadDocumentFormAction);
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
                LoadDataGrid();
            }
        }


        public ObservableCollection<ItemPendingDTO> ItemPendingCollection { get; } 
            = new ObservableCollection<ItemPendingDTO>();
        public void LoadDataGrid()
        {
            ItemPendingCollection.Clear();
            foreach (var _ in _requestFormItemRepository.GetItemPending(SearchKeyword,CurrentPage))
                ItemPendingCollection.Add(_);
            UpdateTotalPage();
        }

        private void UpdateTotalPage()
        {
            TotalPage = _requestFormItemRepository.
                GetItemPendingTotalPage(SearchKeyword);
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
        
        private void NextPageAction(object parameter)
        {
            CurrentPage = CurrentPage + 1;
            LoadDataGrid();
        }

        private bool IsNextPageCanExecute(object parameter)
        {
            if(CurrentPage == TotalPage)
                return false;
            return true;
        }
        private void PrevPageAction(object parameter)
        {
            CurrentPage = CurrentPage - 1;
            LoadDataGrid();
        }

        private bool IsPrevPageCanExecute(object parameter)
        {
            if(CurrentPage == 1)
                return false;
            return true;
        }

        private void OpenUploadDocumentFormAction(object parameter)
        {
            var container = ((App)Application.Current).UnityContainer;
            var requestFormItemUploadDocumentVM = container.Resolve<RequestFormItemUploadDocVM>();
            requestFormItemUploadDocumentVM.InitializeData(this, (int)parameter);
            _windowService.ShowWindow<RequestForm_ItemUploadDocumentView>
                (requestFormItemUploadDocumentVM);
        }
    }
}
