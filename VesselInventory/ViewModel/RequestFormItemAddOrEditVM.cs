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
using VesselInventory.Validations;

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
                    reasons.Add(_.Descriptions);
                return reasons;
            }
        }

        public IList<string> PriorityCollection
        {
            get
            {
                IList<string> priorities = new List<string>();
                foreach (var _ in CommonDataHelper.GetLookupValues("PRIORITY"))
                    priorities.Add(_.Descriptions);
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
            get => RequestFormItemDataView.RequestFormId;
            set
            {
                RequestFormItemDataView.RequestFormId = value;
            }
        }
        public int rf_item_id
        {
            get => RequestFormItemDataView.RequestFormItemId;
            set
            {
                RequestFormItemDataView.RequestFormItemId = value;
            }
        }
        public string item_dimension_number
        {
            get => RequestFormItemDataView.ItemDimensionNumber;
            set
            {
                RequestFormItemDataView.ItemDimensionNumber = value;
            }
        }
        public int? item_group_id
        {
            get => RequestFormItemDataView.ItemGroupId;
            set
            {
                RequestFormItemDataView.ItemGroupId = value;
            }
        }

        [Required]
        [Display(Name ="Item Id")]
        public int item_id
        {
            get => RequestFormItemDataView.ItemId;
            set
            {
                RequestFormItemDataView.ItemId = value;
                OnPropertyChanged("item_id");
            }
        }

        public string item_name
        {
            get => RequestFormItemDataView.ItemName;
            set
            {
                RequestFormItemDataView.ItemName = value;
                OnPropertyChanged("item_name");
            }
        }

        public string brand_type_id
        {
            get => RequestFormItemDataView.BrandTypeId;
            set
            {
                RequestFormItemDataView.BrandTypeId = value;
                OnPropertyChanged("brand_type_id");
            }
        }

        public string brand_type_name
        {
            get => RequestFormItemDataView.BrandTypeName;
            set
            {
                RequestFormItemDataView.BrandTypeName = value;
                OnPropertyChanged("brand_type_name");
            }
        }

        public string color_size_id
        {
            get => RequestFormItemDataView.ColorSizeId;
            set
            {
                RequestFormItemDataView.ColorSizeId = value;
                OnPropertyChanged("color_size_id");
            }
        }

        public string color_size_name
        {
            get => RequestFormItemDataView.ColorSizeName;
            set
            {
                RequestFormItemDataView.ColorSizeName = value;
                OnPropertyChanged("color_size_name");
            }
        }

        public string attachment_path
        {
            get => RequestFormItemDataView.AttachmentPath;
            set
            {
                RequestFormItemDataView.AttachmentPath = value;
                OnPropertyChanged("attachment_path");
            }
        }

        [Required]
        public decimal qty
        {
            get
            {
                return RequestFormItemDataView.Qty;
            }
            set
            {
                RequestFormItemDataView.Qty = value;
                OnPropertyChanged("qty");
            }
        }

        public string reason
        {
            get
            {
                if (RequestFormItemDataView.Reason is null)
                    RequestFormItemDataView.Reason = ReasonCollection.First();
                return RequestFormItemDataView.Reason;
            }
            set
            {
                RequestFormItemDataView.Reason = value;
                OnPropertyChanged("reason");
            }
        }

        public string priority
        {
            get
            {
                if (RequestFormItemDataView.Priority is null)
                    RequestFormItemDataView.Priority = PriorityCollection.First();
                return RequestFormItemDataView.Priority;
            }
            set
            {
                RequestFormItemDataView.Priority = value;
                OnPropertyChanged("priority");
            }
        }

        public string remarks
        {
            get => RequestFormItemDataView.Remarks;
            set
            {
                RequestFormItemDataView.Remarks = value;
                OnPropertyChanged("remarks");
            }
        }

        public string uom
        {
            get => RequestFormItemDataView.Uom;
            set
            {
                RequestFormItemDataView.Uom = value;
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

        public bool IsVisibleSearchItem {
            get
            {
                return (RecordHelper.IsNewRecord(rf_item_id));
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
                item_id = item.ItemId;
                item_name = item.ItemName;
                brand_type_id = item.BrandTypeId;
                brand_type_name = item.BrandTypeName;
                color_size_id = item.ColorSizeId;
                color_size_name = item.ColorSizeName;
                item_dimension_number = item.ItemDimensionNumber;
                item_group_id = item.ItemGroupId;
                uom = item.Uom;
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
            {
                ItemCheckUnique();
                _requestFormItemRepository.Save(RequestFormItemDataView);
            } else
            {
                RequestFormItemDataView.LastModifiedBy = Auth.Instance.personalname;
                RequestFormItemDataView.LastModifiedDate = DateTime.Now;
                _requestFormItemRepository.Update(rf_item_id,RequestFormItemDataView);
            }
        }

        private void LoadParentDataGrid()
        {
            _parentLoadable.LoadDataGrid();
        }

        private void ItemCheckUnique()
        {
            if (ItemUniqueValidator.ValidateRequestFormItem(RequestFormItemDataView))
                throw new Exception(GlobalNamespace.ItemDimensionAlreadyExist);
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
