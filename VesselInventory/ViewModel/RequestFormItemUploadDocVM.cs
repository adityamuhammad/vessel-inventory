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
        public RelayCommand<IClosable> CloseCommand { get; private set; }

        private readonly IOService _iOService;
        private readonly IUploadService _uploadService;
        private IParentLoadable _parentLoadable;
        private readonly IRequestFormItemRepository _requestFormItemRepository;

        public RequestFormItemUploadDocVM(
            IOService ioService, 
            IUploadService uploadService, 
            IRequestFormItemRepository requestFormItemRepository)
        {
            _iOService = ioService;
            _uploadService = uploadService;
            _requestFormItemRepository = requestFormItemRepository;
            InitializeCommands();
        }
        public void InitializeData(IParentLoadable parentLoadable, int rf_item_id)
        {
            RequestFormItemEntity = _requestFormItemRepository.GetById(rf_item_id);
            _parentLoadable = parentLoadable;
        }

        private void InitializeCommands()
        {
            OpenFileDialogCommand = new RelayCommand(OpenFile);
            SaveCommand = new RelayCommand<IClosable>(SaveAction);
            CloseCommand = new RelayCommand<IClosable>(CloseWindow);
        }
        private RequestFormItem RequestFormItemEntity
        {
            get; set;
        } = new RequestFormItem();

        public int rf_item_id {
            get => RequestFormItemEntity.rf_item_id;
            set => RequestFormItemEntity.rf_item_id = value;
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
            get => RequestFormItemEntity.attachment_path;
            set
            {
                RequestFormItemEntity.attachment_path = value;
                OnPropertyChanged("attachment_path");
            }
        }

        private void CloseWindow(IClosable window)
        {
            if (window != null)
                window.Close();
        }
        private void OpenFile(object parameter)
        {
            var filename = _iOService.OpenFileDialog();
            if (filename != null)
                attachment_local_path = filename;
        }

        private void Upload()
        {
            string targetDirectoryPath = @"C:\\VesselInventory\\Attachments\\";
            bool IsUploaded = _uploadService.UploadFile(attachment_local_path,targetDirectoryPath);
            if (IsUploaded)
                attachment_path = _uploadService.GetUploadedPath();
        }

        private void SaveAction(IClosable window)
        {
            if (!string.IsNullOrWhiteSpace(attachment_local_path))
            {
                Upload();
                _requestFormItemRepository.Update(rf_item_id,RequestFormItemEntity);
                _parentLoadable.LoadDataGrid();
                ResponseMessage.Success("Data saved successfully.");
                CloseWindow(window);
            }
            return;
        }
    }
}
