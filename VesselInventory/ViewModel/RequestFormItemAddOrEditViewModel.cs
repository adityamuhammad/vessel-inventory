using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using ToastNotifications;
using ToastNotifications.Messages;
using VesselInventory.DTO;
using VesselInventory.Models;
using VesselInventory.Repository;
using VesselInventory.Services;
using VesselInventory.Utility;
using VesselInventory.Commons.HelperFunctions;
using System;
using System.Collections.Generic;
using Unity;

namespace VesselInventory.ViewModel
{
    public class RequestFormItemAddOrEditViewModel : ViewModelBase
    {
        private RequestFormItem _requestFormItem = new RequestFormItem();
        private Notifier _toasMessage = ToastNotification.Instance.GetInstance();

        public RelayCommand<IClosable> CloseCommand { get; private set; }
        public RelayCommand<IClosable> SaveCommand { get; private set; }
        public RelayCommand ListBoxChangedCommand { get; private set; }
        public RelayCommand OpenFileDialogCommand { get; private set; }

        private readonly IRequestFormItemRepository _requestFormItemRepository;
        private readonly IOService _iOService;
        private readonly IUploadService _uploadService;
        private readonly IParentLoadable _parentLoadable;

        public RequestFormItemAddOrEditViewModel
            (
                IParentLoadable parentLoadable,
                IUploadService uploadService,
                IOService iOService,
                IRequestFormItemRepository requestFormItemRepository,
                int rf_id,
                int rf_item_id = 0
            )
        {
            InitializeCommands();
            _parentLoadable = parentLoadable;
            _uploadService = uploadService;
            _iOService = iOService;
            _requestFormItemRepository = requestFormItemRepository;

            this.rf_id = rf_id;

            if (rf_item_id != 0)
                _requestFormItem = _requestFormItemRepository.GetById(rf_item_id);
        }

        private bool IsNewRecord
        {
            get
            {
                return (rf_item_id == 0);
            }
        }

        private void InitializeCommands()
        {
            ListBoxChangedCommand = new RelayCommand(AutoCompleteChanged);
            OpenFileDialogCommand = new RelayCommand(OpenFile);
            SaveCommand = new RelayCommand<IClosable>(SaveAction);
            CloseCommand = new RelayCommand<IClosable>(CloseWindow);
        }

        /// <summary>
        /// UI Columns and Field
        /// </summary>
        #region
        public int rf_id
        {
            get => _requestFormItem.rf_id;
            set
            {
                _requestFormItem.rf_id = value;
            }
        }
        public int rf_item_id
        {
            get => _requestFormItem.rf_item_id;
            set
            {
                _requestFormItem.rf_item_id = value;
            }
        }
        public string item_dimension_number
        {
            get => _requestFormItem.item_dimension_number;
            set
            {
                _requestFormItem.item_dimension_number = value;
            }
        }
        public int? item_group_id
        {
            get => _requestFormItem.item_group_id;
            set
            {
                _requestFormItem.item_group_id = value;
            }
        }

        [Required(ErrorMessage = "Please select item.")]
        public int item_id
        {
            get => _requestFormItem.item_id;
            set
            {
                _requestFormItem.item_id = value;
                OnPropertyChanged("item_id");
            }
        }

        public string item_name
        {
            get => _requestFormItem.item_name;
            set
            {
                _requestFormItem.item_name = value;
                OnPropertyChanged("item_name");
            }
        }

        public string brand_type_id
        {
            get => _requestFormItem.brand_type_id;
            set
            {
                _requestFormItem.brand_type_id = value;
                OnPropertyChanged("brand_type_id");
            }
        }

        public string brand_type_name
        {
            get => _requestFormItem.brand_type_name;
            set
            {
                _requestFormItem.brand_type_name = value;
                OnPropertyChanged("brand_type_name");
            }
        }

        public string color_size_id
        {
            get => _requestFormItem.color_size_id;
            set
            {
                _requestFormItem.color_size_id = value;
                OnPropertyChanged("color_size_id");
            }
        }

        public string color_size_name
        {
            get => _requestFormItem.color_size_name;
            set
            {
                _requestFormItem.color_size_name = value;
                OnPropertyChanged("color_size_name");
            }
        }

        public string attachment_path
        {
            get => _requestFormItem.attachment_path;
            set
            {
                _requestFormItem.attachment_path = value;
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

        [Required(ErrorMessage = "Qty cannot be empty")]
        public decimal qty
        {
            get => _requestFormItem.qty;
            set
            {
                _requestFormItem.qty = value;
                OnPropertyChanged("qty");
            }
        }

        public string reason
        {
            get
            {
                if (_requestFormItem.reason is null)
                    _requestFormItem.reason = ReasonCollection.First();
                return _requestFormItem.reason;
            }
            set
            {
                _requestFormItem.reason = value;
                OnPropertyChanged("reason");
            }
        }

        public string priority
        {
            get
            {
                if (_requestFormItem.priority is null)
                    _requestFormItem.priority = PriorityCollection.First();
                return _requestFormItem.priority;
            }
            set
            {
                _requestFormItem.priority = value;
                OnPropertyChanged("priority");
            }
        }

        public string remarks
        {
            get => _requestFormItem.remarks;
            set
            {
                _requestFormItem.remarks = value;
                OnPropertyChanged("remarks");
            }
        }
        public string uom
        {
            get => _requestFormItem.uom;
            set
            {
                _requestFormItem.uom = value;
                OnPropertyChanged("uom");
            }
        }
        #endregion

        /// <summary>
        /// UI Properties Behaviour
        /// </summary>
        #region
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
                    LoadItem();
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
        #endregion


        /// <summary>
        /// UI Collection Data
        /// </summary>
        #region
        public IList<string> ReasonCollection
        {
            get
            {
                List<string> reasons = new List<string>();
                foreach (var _ in DataHelper.GetLookupValues("REASON"))
                    reasons.Add(_.description);
                return reasons;
            }
        }

        public IList<string> PriorityCollection
        {
            get
            {
                List<string> priorities = new List<string>();
                foreach (var _ in DataHelper.GetLookupValues("PRIORITY"))
                    priorities.Add(_.description);
                return priorities;
            }
        }

        public ObservableCollection<ItemGroupDimensionDTO> ItemCollection { get; set; } 
            = new ObservableCollection<ItemGroupDimensionDTO>();
        public void LoadItem()
        {
            ItemCollection.Clear();
            foreach(var _ in DataHelper.GetItems(ItemSelectKeyword))
                ItemCollection.Add(_);
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
        #endregion

        /// <summary>
        /// Button Action and behavior
        /// </summary>
        #region
        private void Upload()
        {
            if(attachment_local_path.Trim() != string.Empty)
            {
                string targetDirectoryPath = @"C:\\VesselInventory\\Attachments\\";
                _uploadService.UploadFile(attachment_local_path,targetDirectoryPath);
                attachment_path = _uploadService.GetUploadedPath();
            }
        }
        private void SaveAction(IClosable window)
        {
            try
            {
                Upload();
                if (rf_item_id == 0)
                    _requestFormItemRepository.Save(_requestFormItem);
                else
                    _requestFormItemRepository.Update(rf_item_id,_requestFormItem);

                _parentLoadable.LoadDataGrid();
                _toasMessage.ShowSuccess("Data saved successfully.");
                CloseWindow(window);
            } catch (Exception ex)
            {
                _toasMessage.ShowError("Something went wrong : " + ex.Message.ToString());
            }
}

        private void OpenFile(object parameter)
        {
            var filename =_iOService.OpenFileDialog();
            if (filename != null)
                attachment_local_path = filename;
        } 
        private void CloseWindow(IClosable window)
        {
            if (window != null)
                window.Close();
        }
        #endregion
    }
}
