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
        public RequestFormItemPendingVM(IWindowService windowService, 
            IRequestFormItemRepository requestFormItemRepository)
        {
            _windowService = windowService;
            _requestFormItemRepository = requestFormItemRepository;

            InitializeCommands();
            ResetCurrentPage();
            LoadDataGrid();
        }

        private void InitializeCommands()
        {
            NextPageCommand = new RelayCommand(NextPageAction, IsNextPageCanExecute);
            PrevPageCommand = new RelayCommand(PrevPageAction, IsPrevPageCanExecute);
            UploadDocumentFormCommand = new RelayCommand(OpenUploadDocumentFormAction);
        }

        /// <summary>
        /// UI Properties
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
        /// Data Collections, Entity and custom attributes
        /// </summary>
        #region
        public ObservableCollection<ItemPendingDTO> ItemPendingCollection { get; } 
            = new ObservableCollection<ItemPendingDTO>();
        #endregion

        /// <summary>
        /// Load Methods and manipulate data
        /// </summary>
        #region
        public void LoadDataGrid()
        {
            ItemPendingCollection.Clear();
            foreach(var _ in _requestFormItemRepository
                    .GetItemPending(SearchKeyword,CurrentPage))
                ItemPendingCollection.Add(_);
            UpdateTotalPage();
        }

        private void UpdateTotalPage()
        {
            TotalPage = _requestFormItemRepository.
                GetItemPendingTotalPage(SearchKeyword);
        }
        private void ResetCurrentPage() => CurrentPage = 1;
        private void IncrementCurrentPage() => CurrentPage = CurrentPage + 1;
        private void DecrementCurrentPage() => CurrentPage = CurrentPage - 1;
        #endregion

        /// <summary>
        /// Button Behavior
        /// </summary>
        /// <param name="parameter"></param>
        /// <returns></returns>
        #region
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
        private void OpenUploadDocumentFormAction(object parameter)
        {
            var container = ((App)Application.Current).UnityContainer;
            var requestFormItemUploadDocumentVM = container.Resolve<RequestFormItemUploadDocVM>();
            requestFormItemUploadDocumentVM.InitializeData(this, (int)parameter);
            _windowService.ShowDialogWindow<RequestForm_ItemUploadDocumentView>
                (requestFormItemUploadDocumentVM);
        }
        #endregion
    }
}
