using System;
using VesselInventory.Commons;
using VesselInventory.Models;
using VesselInventory.Repository;
using VesselInventory.Services;
using VesselInventory.Utility;

namespace VesselInventory.ViewModel
{
    class RequestFormItemUploadDocVM : ViewModelBase
    {
        public RelayCommand OpenFileDialogCommand { get; private set; }
        public RelayCommand<IClosable> SaveCommand { get; private set; }

        private readonly IOService _IOService;
        private readonly IUploadService _uploadService;
        private IParentLoadable _parentLoadable;
        private readonly IRequestFormItemRepository _requestFormItemRepository;

        public RequestFormItemUploadDocVM(IOService IOService, 
            IUploadService uploadService, 
            IRequestFormItemRepository requestFormItemRepository)
        {
            _IOService = IOService;
            _uploadService = uploadService;
            _requestFormItemRepository = requestFormItemRepository;
            InitializeCommands();
        }
        public void InitializeData(IParentLoadable parentLoadable, int requestFormItemId)
        {
            _parentLoadable = parentLoadable;
            RequestFormItemId = requestFormItemId;
            LoadAttributes();
        }

        private void LoadAttributes()
        {
            RequestFormItemDataView = _requestFormItemRepository.GetById(RequestFormItemId);
        }

        private void InitializeCommands()
        {
            OpenFileDialogCommand = new RelayCommand(OpenFile);
            SaveCommand = new RelayCommand<IClosable>(SaveAction);
        }
        public override string Title => "Upload Item Document";
        private RequestFormItem RequestFormItemDataView
        {
            get; set;
        } = new RequestFormItem();

        public int RequestFormItemId {
            get => RequestFormItemDataView.RequestFormItemId;
            set => RequestFormItemDataView.RequestFormItemId = value;
        }

        private string _attachmentLocalPath = string.Empty;
        public string AttachmentLocalPath
        {
            get => _attachmentLocalPath;
            set
            {
                _attachmentLocalPath = value;
                OnPropertyChanged("AttachmentLocalPath");
            }
        }

        public string AttachmentPath
        {
            get => RequestFormItemDataView.AttachmentPath;
            set
            {
                RequestFormItemDataView.AttachmentPath = value;
                OnPropertyChanged("AttachmentPath");
            }
        }

        private void OpenFile(object parameter)
        {
            var filename = _IOService.OpenFileDialog();
            if (filename != null)
                AttachmentLocalPath = filename;
        }

        private void Upload()
        {
            bool IsUploaded = _uploadService.UploadFile(AttachmentLocalPath,GlobalNamespace.AttachmentPathLocation);
            if (IsUploaded) AttachmentPath = _uploadService.GetUploadedPath();
        }

        private void SaveAction(IClosable window)
        {
            if (!string.IsNullOrWhiteSpace(AttachmentLocalPath))
            {
                Upload();
                Update();
                _parentLoadable.LoadDataGrid();
                CloseWindow(window);
                ResponseMessage.Success(GlobalNamespace.SuccessSave);
            }
            return;
        }

        private void Update()
        {
            RequestFormItemDataView.LastModifiedBy = Auth.Instance.PersonName;
            RequestFormItemDataView.LastModifiedDate = DateTime.Now;
            _requestFormItemRepository.Update(RequestFormItemId, RequestFormItemDataView);
        }
    }
}
