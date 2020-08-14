using System.Collections.ObjectModel;
using System.Windows;
using VesselInventory.Dto;
using VesselInventory.Repository;
using VesselInventory.Services;
using VesselInventory.Utility;
using VesselInventory.Views;
using Unity;
using VesselInventory.Commons;
using System.IO;
using VesselInventory.Filters;

namespace VesselInventory.ViewModel
{
    public class RequestFormItemPendingVM : RequestFormVMBase, IParentLoadable
    {
        public ObservableCollection<ItemPendingDto> ItemPendingCollection { get; } 
            = new ObservableCollection<ItemPendingDto>();
        public override string Title => "Pending Items";
        public RelayCommand SearchCommand { get; private set; }
        public RelayCommand NextPageCommand { get; private set; }
        public RelayCommand PrevPageCommand { get; private set; }
        public RelayCommand UploadDocumentFormCommand { get; private set; }
        public RelayCommand PreviewPdfCommand { get; private set; }

        private readonly IUnityContainer UnityContainer = ((App)Application.Current).UnityContainer;
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
            SearchCommand = new RelayCommand(SearchAction);
            NextPageCommand = new RelayCommand(NextPageAction, IsNextPageCanExecute);
            PrevPageCommand = new RelayCommand(PrevPageAction, IsPrevPageCanExecute);
            UploadDocumentFormCommand = new RelayCommand(OpenUploadDocumentFormAction);
            PreviewPdfCommand = new RelayCommand(PreviewPdfAction);
        }

        /// <summary>
        /// UI Properties
        /// </summary>
        #region
        public int DataGridRows => 10;
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
            }
        }
        #endregion

        /// <summary>
        /// Data Collections, Entity and custom attributes
        /// </summary>
        #region
        #endregion

        /// <summary>
        /// Load Methods and manipulate data
        /// </summary>
        #region
        private PageFilter PageFilter
        {
            get => new PageFilter
            {
                Search = SearchKeyword,
                NumRows = DataGridRows,
                PageNum = CurrentPage,
                SortName = "RequestForm.RequestFormNumber",
                SortType = "DESC"
            };
        }
        public void LoadDataGrid()
        {
            ItemPendingCollection.Clear();
            foreach(var _ in _requestFormItemRepository.GetItemPendingDataGrid(Auth.Instance.DepartmentName,PageFilter))
                ItemPendingCollection.Add(_);
            UpdateTotalPage();
        }

        private void UpdateTotalPage()
        {
            TotalPage = _requestFormItemRepository.GetItemPendingTotalPage(Auth.Instance.DepartmentName,PageFilter);
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
            var requestFormItemUploadDocumentVM = UnityContainer.Resolve<RequestFormItemUploadDocVM>();

            requestFormItemUploadDocumentVM.InitializeData(this, (int)parameter);

            _windowService.ShowDialogWindow<RequestForm_ItemUploadDocumentView>(requestFormItemUploadDocumentVM);
        }
        private void PreviewPdfAction(object parameter)
        {

            string attachmentFileName = (string)parameter;
            string fileLocation = GlobalNamespace.AttachmentPathLocation + attachmentFileName;

            if (string.IsNullOrWhiteSpace(attachmentFileName))
            {
                ResponseMessage.Info(GlobalNamespace.AttachmentNotUploaded);
                return;
            }

            if (!File.Exists(fileLocation)){
                ResponseMessage.Warning(GlobalNamespace.AttachmentMissing);
                return;
            }
            var previewPdf = UnityContainer.Resolve<PreviewPdf>();
            previewPdf.SetAttachment(fileLocation);
            previewPdf.ShowDialog();
        }
        private void SearchAction(object parameter)
        {
            ResetCurrentPage();
            LoadDataGrid();
        }
        #endregion
    }
}
