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
        public void InitializeData(IParentLoadable parentLoadable, int rf_item_id)
        {
            RequestFormItemDataView = _requestFormItemRepository.GetById(rf_item_id);
            _parentLoadable = parentLoadable;
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

        public int rf_item_id {
            get => RequestFormItemDataView.rf_item_id;
            set => RequestFormItemDataView.rf_item_id = value;
        }

        private string _attachment_local_path = string.Empty;
        public string attachment_local_path
        {
            get => _attachment_local_path;
            set
            {
                _attachment_local_path = value;
                OnPropertyChanged("attachment_local_path");
            }
        }

        public string attachment_path
        {
            get => RequestFormItemDataView.attachment_path;
            set
            {
                RequestFormItemDataView.attachment_path = value;
                OnPropertyChanged("attachment_path");
            }
        }

        private void OpenFile(object parameter)
        {
            var filename = _IOService.OpenFileDialog();
            if (filename != null)
                attachment_local_path = filename;
        }

        private void Upload()
        {
            bool IsUploaded = _uploadService.UploadFile(
                attachment_local_path,GlobalNamespace.AttachmentPathLocation);
            if (IsUploaded)
                attachment_path = _uploadService.GetUploadedPath();
        }

        private void SaveAction(IClosable window)
        {
            if (!string.IsNullOrWhiteSpace(attachment_local_path))
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
            RequestFormItemDataView.last_modified_by = Auth.Instance.personalname;
            RequestFormItemDataView.last_modified_date = DateTime.Now;
            _requestFormItemRepository.Update(rf_item_id, RequestFormItemDataView);
        }
    }
}
