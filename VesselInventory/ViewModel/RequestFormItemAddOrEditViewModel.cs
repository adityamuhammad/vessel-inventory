using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using ToastNotifications;
using ToastNotifications.Messages;
using VesselInventory.DTO;
using VesselInventory.Helpers;
using VesselInventory.Models;
using VesselInventory.Repository;
using VesselInventory.Services;
using VesselInventory.Utility;

namespace VesselInventory.ViewModel
{
    public class RequestFormItemAddOrEditViewModel : ViewModelBase
    {

        RequestFormItemRepository _requestFormItemRepository;
        RFItem _rf_item = new RFItem();
        public RelayCommand<IClosable> Close { get; private set; }
        public RelayCommand ListBoxChanged { get; private set; }
        public RelayCommand OpenFileDialog { get; private set; }
        public RelayCommand<IClosable> Save { get; private set; }

        private IOService _iOService;
        private IUploadService _uploadService;
        private IParentLoadable _parentLoadable;

        Notifier _toasMessage = ToastNotification.Instance.GetInstance();

        public RequestFormItemAddOrEditViewModel(IParentLoadable parentLoadable, int rf_id) : this(parentLoadable, rf_id, 0) { }
        public RequestFormItemAddOrEditViewModel(IParentLoadable parentLoadable, int _rf_id, int rf_item_id)
        {
            _parentLoadable = parentLoadable;
            _uploadService = new UploadService();
            _iOService = new OpenPdfFileDialog();
            _requestFormItemRepository = new RequestFormItemRepository();

            IsVisibleListBoxItem = false;

            SetCommands();
            LoadPriorityList();
            LoadReasonList();
            RefreshItems();

            rf_id = _rf_id;
            reason = _reasonList.First();
            priority = _priorityList.First();

            if (rf_item_id != 0)
                _rf_item = _requestFormItemRepository.FindById(rf_item_id);
        }

        private void SetCommands()
        {
            ListBoxChanged = new RelayCommand(AutoCompleteChanged);
            OpenFileDialog = new RelayCommand(OpenFile);
            Save = new RelayCommand<IClosable>(SaveAction, IsCanSave);
            Close = new RelayCommand<IClosable>(CloseAction);
        }

        #region
        public int? rf_id
        {
            get => _rf_item.rf_id;
            set
            {
                _rf_item.rf_id = value;
            }
        }
        public int rf_item_id
        {
            get => _rf_item.rf_item_id;
            set
            {
                _rf_item.rf_item_id = value;
            }
        }
        public string item_dimension_number
        {
            get => _rf_item.item_dimension_number;
            set
            {
                _rf_item.item_dimension_number = value;
            }
        }
        public int? item_group_id
        {
            get => _rf_item.item_group_id;
            set
            {
                _rf_item.item_group_id = value;
            }
        }

        public int? item_id
        {
            get => _rf_item.item_id;
            set
            {
                _rf_item.item_id = value;
                OnPropertyChanged("item_id");
            }
        }

        public string item_name
        {
            get => _rf_item.item_name;
            set
            {
                _rf_item.item_name = value;
                OnPropertyChanged("item_name");
            }
        }

        public string brand_type_id
        {
            get => _rf_item.brand_type_id;
            set
            {
                _rf_item.brand_type_id = value;
                OnPropertyChanged("brand_type_id");
            }
        }

        public string brand_type_name
        {
            get => _rf_item.brand_type_name;
            set
            {
                _rf_item.brand_type_name = value;
                OnPropertyChanged("brand_type_name");
            }
        }

        public string color_size_id
        {
            get => _rf_item.color_size_id;
            set
            {
                _rf_item.color_size_id = value;
                OnPropertyChanged("color_size_id");
            }
        }

        public string color_size_name
        {
            get => _rf_item.color_size_name;
            set
            {
                _rf_item.color_size_name = value;
                OnPropertyChanged("color_size_name");
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

        [Required(ErrorMessage ="Qty cannot be empty")]
        [RegularExpression(@"^[0-9]*$",ErrorMessage ="Invalid Format")]
        public decimal? qty
        {
            get => _rf_item.qty;
            set
            {
                _rf_item.qty = value;
                OnPropertyChanged("qty");
            }
        }

        public string reason
        {
            get => _rf_item.reason;
            set
            {
                _rf_item.reason = value;
                OnPropertyChanged("reason");
            }
        }

        public string priority
        {
            get => _rf_item.priority;
            set
            {
                _rf_item.priority = value;
                OnPropertyChanged("priority");
            }
        }

        public string remarks
        {
            get => _rf_item.remarks;
            set
            {
                _rf_item.remarks = value;
                OnPropertyChanged("remarks");
            }
        }
        public string uom
        {
            get => _rf_item.uom;
            set
            {
                _rf_item.uom = value;
                OnPropertyChanged("uom");
            }
        }
        #endregion

        private string _itemSelectKeyword = string.Empty;
        public string ItemSelectKeyword
        {
            get => _itemSelectKeyword;
            set
            {
                _itemSelectKeyword = value;
                OnPropertyChanged("ItemSelectKeyword");
                if (value == string.Empty)
                {
                    IsVisibleListBoxItem = false;
                } else
                {
                    IsVisibleListBoxItem = true;
                    RefreshItems();
                }
            }
        }

        private bool _IsVisibleListBoxItem = false;
        public bool IsVisibleListBoxItem
        {
            get => _IsVisibleListBoxItem;
            set
            {
                if  (_IsVisibleListBoxItem == value)
                    return;
                _IsVisibleListBoxItem = value;
                OnPropertyChanged("IsVisibleListBoxItem");
            }
        }

        private ObservableCollection<ItemGroupDimensionDTO> _items = new ObservableCollection<ItemGroupDimensionDTO>();
        public ObservableCollection<ItemGroupDimensionDTO> Items
        {
            get => _items;
            set
            {
                _items = value;
                OnPropertyChanged("Items");
            }
        }

        private ObservableCollection<string> _reasonList = new ObservableCollection<string>();
        public ObservableCollection<string> ReasonList
        {
            get => _reasonList;
            set
            {
                _reasonList = value;
                OnPropertyChanged("ReasonList");
            }
        }
        private void LoadReasonList()
        {
            foreach(var _ in DataHelper.GetLookupValues("REASON")){
                _reasonList.Add(_.description);
            }
        }

        private ObservableCollection<string> _priorityList = new ObservableCollection<string>();
        public ObservableCollection<string> PriorityList
        {
            get => _priorityList;
            set
            {
                _priorityList = value;
                OnPropertyChanged("PriorityList");
            }
        }
        private void LoadPriorityList()
        {
            foreach(var _ in DataHelper.GetLookupValues("PRIORITY")){
                _priorityList.Add(_.description);
            }
        }

        public void RefreshItems()
        {
            _items.Clear();
            foreach(var _ in DataHelper.GetItems(ItemSelectKeyword))
                _items.Add(_);
        }

        private void AutoCompleteChanged(object parameter)
        {
            IsVisibleListBoxItem = false;
            ItemGroupDimensionDTO item = (ItemGroupDimensionDTO)parameter;
            if (item != null)
            {
                item_id = item.item_id;
                item_name = item.item_name;
                brand_type_id = item.brand_type_id;
                brand_type_name = item.brand_type_name;
                color_size_id = item.color_size_id;
                color_size_name = item.color_size_name;
                item_dimension_number = item.item_dimension_number;
                item_group_id = item.item_group_id;
                uom = item.uom;
                ItemSelectKeyword = string.Empty;
            }
        }

        private void SaveAction(IClosable window)
        {

            if(attachment_local_path.Trim() != string.Empty || attachment_local_path != null)
            {
                string targetDirectoryPath = @"C:\\VesselInventory\\Attachments\\";
                _uploadService.UploadFile(attachment_local_path,targetDirectoryPath);
                attachment_path = _uploadService.GetUploadedPath();
            }

            if (rf_item_id == 0)
                _requestFormItemRepository.Save(_rf_item);
            else
                _requestFormItemRepository.Update(rf_item_id,_rf_item);

            _parentLoadable.LoadGrid();

            _toasMessage.ShowSuccess("Data saved successfully.");

            if (window != null)
                window.Close();
        }

        private bool IsCanSave(object parameter)
        {
            return true;
        }

        private void OpenFile(object parameter)
        {
            var filename =_iOService.OpenFileDialog();
            if (filename != "Error")
                attachment_local_path = filename;
        } 
        private void CloseAction(IClosable window)
        {
            if (window != null)
                window.Close();
        }
    }
}
