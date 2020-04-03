using ToastNotifications;
using ToastNotifications.Messages;
using VesselInventory.Models;
using VesselInventory.Repository;
using VesselInventory.Services;
using VesselInventory.Utility;

namespace VesselInventory.ViewModel
{
    class RequestFormItemUploadDocument : ViewModelBase
    {
        public RelayCommand OpenFileDialog { get; private set; }
        public RelayCommand<IClosable> Save { get; private set; }
        public RelayCommand<IClosable> Close { get; private set; }

        private Notifier _toasMessage = ToastNotification.Instance.GetInstance();
        private RFItem _rf_item = new RFItem();

        private readonly IOService _iOService;
        private readonly IUploadService _uploadService;
        private readonly IParentLoadable _parentLoadable;
        private readonly IRequestFormItemRepository _requestFormItemRepository;

        public RequestFormItemUploadDocument(IParentLoadable parentLoadable, int _rf_item_id)
        {
            _requestFormItemRepository = new RequestFormItemRepository();
            _rf_item = _requestFormItemRepository.FindById(_rf_item_id);
            _parentLoadable = parentLoadable;
            _uploadService = new UploadService();
            _iOService = new OpenPdfFileDialog();
            OpenFileDialog = new RelayCommand(OpenFile);
            Save = new RelayCommand<IClosable>(SaveAction);
            Close = new RelayCommand<IClosable>(CloseAction);
        }

        public int rf_item_id {
            get => _rf_item.rf_item_id;
            set => _rf_item.rf_item_id = value;
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
            get => _rf_item.attachment_path;
            set
            {
                _rf_item.attachment_path = value;
                OnPropertyChanged("attachment_path");
            }
        }

        private void CloseAction(IClosable window)
        {
            if (window != null)
                window.Close();
        }
        private void OpenFile(object parameter)
        {
            var filename = _iOService.OpenFileDialog();
            if (filename != "Error")
                attachment_local_path = filename;
        }

        private void SaveAction(IClosable window)
        {
            if(attachment_local_path.Trim() != string.Empty)
            {
                string targetDirectoryPath = @"C:\\VesselInventory\\Attachments\\";
                _uploadService.UploadFile(attachment_local_path,targetDirectoryPath);
                attachment_path = _uploadService.GetUploadedPath();
                _requestFormItemRepository.Update(rf_item_id,_rf_item);
                _parentLoadable.LoadGrid();
                _toasMessage.ShowSuccess("Data saved successfully.");
            }
            if (window != null)
                window.Close();
            return;
        }
    }
}
