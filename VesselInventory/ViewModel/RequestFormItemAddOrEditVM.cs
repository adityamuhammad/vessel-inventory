using System;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using VesselInventory.Dto;
using VesselInventory.Models;
using VesselInventory.Repository;
using VesselInventory.Services;
using VesselInventory.Utility;
using VesselInventory.Commons.HelperFunctions;
using VesselInventory.Commons;

namespace VesselInventory.ViewModel
{
    public class RequestFormItemAddOrEditVM : ViewModelBase
    {
        public RelayCommand<IClosable> SaveCommand { get; private set; }
        public RelayCommand ListBoxChangedCommand { get; private set; }
        public RelayCommand OpenFileDialogCommand { get; private set; }

        private readonly IRequestFormItemRepository _requestFormItemRepository;
        private readonly IOService _IOService;
        private readonly IUploadService _uploadService;
        private IParentLoadable _parentLoadable;

        public RequestFormItemAddOrEditVM(IUploadService uploadService,
            IOService IOService,
            IRequestFormItemRepository requestFormItemRepository)
        {
            _uploadService = uploadService;
            _IOService = IOService;
            _requestFormItemRepository = requestFormItemRepository;
            InitializeCommands();

        }

        private void InitializeCommands()
        {
            ListBoxChangedCommand = new RelayCommand(AutoCompleteChanged);
            OpenFileDialogCommand = new RelayCommand(OpenFile);
            SaveCommand = new RelayCommand<IClosable>(SaveAction);
        }

        public void InitializeData(IParentLoadable parentLoadable, 
            int requestFormId, int requestFormItemId = 0)
        {
            _parentLoadable = parentLoadable;
            this.rf_id = requestFormId;
            this.rf_item_id = requestFormItemId;
            SetUIAttributesValue();
        }

        /// <summary>
        /// UI Collection Data, Entity, And Custom Attributes
        /// </summary>
        #region
        private RequestFormItem RequestFormItemDataView
        {
            get; set;
        } = new RequestFormItem();

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

        public IList<string> ReasonCollection
        {
            get
            {
                IList<string> reasons = new List<string>();
                foreach (var _ in CommonDataHelper.GetLookupValues("REASON"))
                    reasons.Add(_.description);
                return reasons;
            }
        }

        public IList<string> PriorityCollection
        {
            get
            {
                IList<string> priorities = new List<string>();
                foreach (var _ in CommonDataHelper.GetLookupValues("PRIORITY"))
                    priorities.Add(_.description);
                return priorities;
            }
        }

        public ObservableCollection<ItemGroupDimensionDto> ItemCollection { get; set; } 
            = new ObservableCollection<ItemGroupDimensionDto>();
        #endregion

        /// <summary>
        /// UI Columns or Field Attributes
        /// </summary>
        #region
        public int rf_id
        {
            get => RequestFormItemDataView.rf_id;
            set
            {
                RequestFormItemDataView.rf_id = value;
            }
        }
        public int rf_item_id
        {
            get => RequestFormItemDataView.rf_item_id;
            set
            {
                RequestFormItemDataView.rf_item_id = value;
            }
        }
        public string item_dimension_number
        {
            get => RequestFormItemDataView.item_dimension_number;
            set
            {
                RequestFormItemDataView.item_dimension_number = value;
            }
        }
        public int? item_group_id
        {
            get => RequestFormItemDataView.item_group_id;
            set
            {
                RequestFormItemDataView.item_group_id = value;
            }
        }

        [Required]
        [Display(Name ="Item Id")]
        public int item_id
        {
            get => RequestFormItemDataView.item_id;
            set
            {
                RequestFormItemDataView.item_id = value;
                OnPropertyChanged("item_id");
            }
        }

        public string item_name
        {
            get => RequestFormItemDataView.item_name;
            set
            {
                RequestFormItemDataView.item_name = value;
                OnPropertyChanged("item_name");
            }
        }

        public string brand_type_id
        {
            get => RequestFormItemDataView.brand_type_id;
            set
            {
                RequestFormItemDataView.brand_type_id = value;
                OnPropertyChanged("brand_type_id");
            }
        }

        public string brand_type_name
        {
            get => RequestFormItemDataView.brand_type_name;
            set
            {
                RequestFormItemDataView.brand_type_name = value;
                OnPropertyChanged("brand_type_name");
            }
        }

        public string color_size_id
        {
            get => RequestFormItemDataView.color_size_id;
            set
            {
                RequestFormItemDataView.color_size_id = value;
                OnPropertyChanged("color_size_id");
            }
        }

        public string color_size_name
        {
            get => RequestFormItemDataView.color_size_name;
            set
            {
                RequestFormItemDataView.color_size_name = value;
                OnPropertyChanged("color_size_name");
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

        [Required]
        public decimal qty
        {
            get
            {
                return RequestFormItemDataView.qty;
            }
            set
            {
                RequestFormItemDataView.qty = value;
                OnPropertyChanged("qty");
            }
        }

        public string reason
        {
            get
            {
                if (RequestFormItemDataView.reason is null)
                    RequestFormItemDataView.reason = ReasonCollection.First();
                return RequestFormItemDataView.reason;
            }
            set
            {
                RequestFormItemDataView.reason = value;
                OnPropertyChanged("reason");
            }
        }

        public string priority
        {
            get
            {
                if (RequestFormItemDataView.priority is null)
                    RequestFormItemDataView.priority = PriorityCollection.First();
                return RequestFormItemDataView.priority;
            }
            set
            {
                RequestFormItemDataView.priority = value;
                OnPropertyChanged("priority");
            }
        }

        public string remarks
        {
            get => RequestFormItemDataView.remarks;
            set
            {
                RequestFormItemDataView.remarks = value;
                OnPropertyChanged("remarks");
            }
        }

        public string uom
        {
            get => RequestFormItemDataView.uom;
            set
            {
                RequestFormItemDataView.uom = value;
                OnPropertyChanged("uom");
            }
        }
        #endregion

        /// <summary>
        /// UI Properties Behavior
        /// </summary>
        #region
        public override string Title => "Request Form Item";
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

        private bool _IsVisibleSearchItem = true;
        public bool IsVisibleSearchItem {
            get
            {
                if (!RecordHelper.IsNewRecord(rf_item_id))
                    _IsVisibleSearchItem = false;
                return _IsVisibleSearchItem;
            }
            set
            {
                if  (_IsVisibleSearchItem == value)
                    return;
                _IsVisibleSearchItem = value;
                OnPropertyChanged("IsVisibleSearchItem");
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
        /// Load Methods
        /// </summary>
        #region
        private void SetUIAttributesValue()
        {
            if (!RecordHelper.IsNewRecord(rf_item_id))
                RequestFormItemDataView = _requestFormItemRepository
                    .GetById(rf_item_id);
        }

        public void LoadItem()
        {
            ItemCollection.Clear();
            foreach(var _ in CommonDataHelper
                .GetItems(ItemSelectKeyword, "rf_item", rf_id))
                ItemCollection.Add(_);
        }

        private void AutoCompleteChanged(object parameter)
        {
            IsVisibleListBoxItem = false;
            ItemGroupDimensionDto item = (ItemGroupDimensionDto)parameter;
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
            if(!string.IsNullOrWhiteSpace(attachment_local_path))
            {
                bool IsUploaded = _uploadService.UploadFile(
                    attachment_local_path, GlobalNamespace.AttachmentPathLocation);
                if (IsUploaded)
                    attachment_path = _uploadService.GetUploadedPath();
            }
        }

        private void SaveOrUpdate()
        {
            if (RecordHelper.IsNewRecord(rf_item_id))
                _requestFormItemRepository.Save(RequestFormItemDataView);
            else
                _requestFormItemRepository.Update(rf_item_id,RequestFormItemDataView);
        }

        private void LoadParentDataGrid()
        {
            _parentLoadable.LoadDataGrid();
        }

        private void SaveAction(IClosable window)
        {
            try
            {
                Upload();
                SaveOrUpdate();
                LoadParentDataGrid();
                CloseWindow(window);
                ResponseMessage.Success(GlobalNamespace.SuccessSave);
            } catch (Exception ex)
            {
                ResponseMessage.Error(GlobalNamespace.Error + ex.Message);
            }
        }

        private void OpenFile(object parameter)
        {
            var filename =_IOService.OpenFileDialog();
            if (filename != null)
                attachment_local_path = filename;
        } 
        #endregion
    }
}
