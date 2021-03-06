﻿using System;
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
using VesselInventory.Commons.Enums;
using System.Text;

namespace VesselInventory.ViewModel
{
    public class RequestFormItemAddOrEditVM : ViewModelBase
    {
        public ObservableCollection<ItemGroupDimensionDto> ItemCollection { get; set; } 
            = new ObservableCollection<ItemGroupDimensionDto>();
        public RelayCommand<IClosable> SaveCommand { get; private set; }
        public RelayCommand ListBoxChangedCommand { get; private set; }
        public RelayCommand OpenFileDialogCommand { get; private set; }

        private readonly IRequestFormItemRepository _requestFormItemRepository;
        private readonly IOService _IOService;
        private readonly IUploadService _uploadService;
        private IDataGrid _parentLoadable;

        public RequestFormItemAddOrEditVM(
            IUploadService uploadService,
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

        public void InitializeData(IDataGrid parentLoadable, 
            int requestFormId, int requestFormItemId = 0)
        {
            _parentLoadable = parentLoadable;
            RequestFormId = requestFormId;
            RequestFormItemId = requestFormItemId;
            SetUIAttributesValue();
        }

        /// <summary>
        /// UI Collection Data, Entity, And Custom Attributes
        /// </summary>
        #region
        private RequestFormItem RequestFormItemDataView { get; set; } = new RequestFormItem();

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

        #endregion

        /// <summary>
        /// UI Columns or Field Attributes
        /// </summary>
        #region
        public int RequestFormId
        {
            get => RequestFormItemDataView.RequestFormId;
            set => RequestFormItemDataView.RequestFormId = value;
        }
        public int RequestFormItemId
        {
            get => RequestFormItemDataView.RequestFormItemId;
            set => RequestFormItemDataView.RequestFormItemId = value;
        }
        public string ItemDimensionNumber
        {
            get => RequestFormItemDataView.ItemDimensionNumber;
            set => RequestFormItemDataView.ItemDimensionNumber = value;
        }
        public int ItemGroupId
        {
            get => RequestFormItemDataView.ItemGroupId;
            set => RequestFormItemDataView.ItemGroupId = value;
        }

        [Required]
        [Display(Name ="Item Id")]
        public int ItemId
        {
            get => RequestFormItemDataView.ItemId;
            set
            {
                RequestFormItemDataView.ItemId = value;
                OnPropertyChanged("ItemId");
            }
        }

        public string ItemName
        {
            get => RequestFormItemDataView.ItemName;
            set
            {
                RequestFormItemDataView.ItemName = value;
                OnPropertyChanged("ItemName");
            }
        }

        public string BrandTypeId
        {
            get => RequestFormItemDataView.BrandTypeId;
            set
            {
                RequestFormItemDataView.BrandTypeId = value;
                OnPropertyChanged("BrandTypeId");
            }
        }

        public string BrandTypeName
        {
            get => RequestFormItemDataView.BrandTypeName;
            set
            {
                RequestFormItemDataView.BrandTypeName = value;
                OnPropertyChanged("BrandTypeName");
            }
        }

        public string ColorSizeId
        {
            get => RequestFormItemDataView.ColorSizeId;
            set
            {
                RequestFormItemDataView.ColorSizeId = value;
                OnPropertyChanged("ColorSizeId");
            }
        }

        public string ColorSizeName
        {
            get => RequestFormItemDataView.ColorSizeName;
            set
            {
                RequestFormItemDataView.ColorSizeName = value;
                OnPropertyChanged("ColorSizeName");
            }
        }

        public string AttachmentPath
        {
            get => RequestFormItemDataView.AttachmentPath;
            set
            {
                RequestFormItemDataView.AttachmentPath = value;
                OnPropertyChanged("AttachmenetPath");
            }
        }

        [Required]
        public decimal Qty
        {
            get => RequestFormItemDataView.Qty;
            set
            {
                RequestFormItemDataView.Qty = value;
                OnPropertyChanged("Qty");
            }
        }

        public string Reason
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
                OnPropertyChanged("Reason");
            }
        }

        public string Priority
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
                OnPropertyChanged("Priority");
            }
        }

        public string Remarks
        {
            get => RequestFormItemDataView.Remarks;
            set
            {
                RequestFormItemDataView.Remarks = value;
                OnPropertyChanged("Remarks");
            }
        }

        public string Uom
        {
            get => RequestFormItemDataView.Uom;
            set
            {
                RequestFormItemDataView.Uom = value;
                OnPropertyChanged("Uom");
            }
        }

        public decimal LastRequestQty
        {
            get => RequestFormItemDataView.LastRequestQty;
            set
            {
                RequestFormItemDataView.LastRequestQty = value;
                OnPropertyChanged("LastRequestQty");
            }
        }

        public DateTime? LastRequestDate
        {
            get
            {
                if (RequestFormItemDataView.LastRequestDate == default(DateTime) )
                    RequestFormItemDataView.LastRequestDate = DateTime.Now;
                return RequestFormItemDataView.LastRequestDate;
            }
            set
            {
                RequestFormItemDataView.LastRequestDate = DateTime.Parse(value.ToString());
                OnPropertyChanged("LastRequestDate");

            }
        }
        public decimal LastSupplyQty
        {
            get => RequestFormItemDataView.LastSupplyQty;
            set
            {
                RequestFormItemDataView.LastSupplyQty = value;
                OnPropertyChanged("LastSupplyQty");
            }
        }

        public DateTime? LastSupplyDate
        {
            get
            {
                if (RequestFormItemDataView.LastSupplyDate == default(DateTime) )
                    RequestFormItemDataView.LastSupplyDate = DateTime.Now;
                return RequestFormItemDataView.LastSupplyDate;
            }
            set
            {
                RequestFormItemDataView.LastSupplyDate = DateTime.Parse(value.ToString());
                OnPropertyChanged("LastSupplyDate");
            }
        }

        public decimal Rob
        {
            get => RequestFormItemDataView.Rob;
            set
            {
                RequestFormItemDataView.Rob = value;
                OnPropertyChanged("Rob");
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

        public bool IsVisibleSearchItem => (RecordHelper.IsNewRecord(RequestFormItemId));

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
            if (!RecordHelper.IsNewRecord(RequestFormItemId))
                RequestFormItemDataView = _requestFormItemRepository
                    .GetById(RequestFormItemId);
        }

        public void LoadItem()
        {
            ItemCollection.Clear();
            foreach(var _ in CommonDataHelper.GetItems(ItemSelectKeyword, "RequestFormItem", RequestFormId))
                ItemCollection.Add(_);
        }

        private void AutoCompleteChanged(object parameter)
        {
            IsVisibleListBoxItem = false;
            ItemGroupDimensionDto item = (ItemGroupDimensionDto)parameter;
            if (item != null)
            {
                var lastReq = _requestFormItemRepository.GetLastRequestItem(item.ItemId, item.ItemDimensionNumber);
                ItemId = item.ItemId;
                ItemName = item.ItemName;
                BrandTypeId = item.BrandTypeId;
                BrandTypeName = item.BrandTypeName;
                ColorSizeId = item.ColorSizeId;
                ColorSizeName = item.ColorSizeName;
                ItemDimensionNumber = item.ItemDimensionNumber;
                ItemGroupId = item.ItemGroupId;
                Uom = item.Uom;
                LastRequestQty = lastReq.LastRequestQty;
                LastRequestDate = lastReq.LastRequestDate.GetValueOrDefault();
                LastSupplyQty = lastReq.LastSupplyQty;
                LastSupplyDate = lastReq.LastSupplyDate.GetValueOrDefault();
                Rob = lastReq.Rob;
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
            if(!string.IsNullOrWhiteSpace(AttachmentLocalPath))
            {
                bool IsUploaded = _uploadService.UploadFile(AttachmentLocalPath, GlobalNamespace.AttachmentPathLocation);

                if (IsUploaded) AttachmentPath = _uploadService.GetUploadedPath();
            }
        }


        private void LoadParentDataGrid()
        {
            _parentLoadable.LoadDataGrid();
        }

        private void SaveAction(IClosable window)
        {
            try
            {
                CheckZeroQty();
                Upload();
                SaveOrUpdate();
                LoadParentDataGrid();
                CloseWindow(window);
                ResponseMessage.Success(GlobalNamespace.SuccessSave);
            }
            catch (ValidationException ex)
            {
                ResponseMessage.Error(ex.Message);
            }
            catch (Exception)
            {
                ResponseMessage.Error(GlobalNamespace.ErrorSave + " " + GlobalNamespace.WrongInput);
            }
        }
        private void SaveOrUpdate()
        {
            if (RecordHelper.IsNewRecord(RequestFormItemId))
            {
                ItemCheckUnique();
                Save();
            } else
            {
                Update();
            }
        }
        private void ItemCheckUnique()
        {

            if (ItemUniqueValidator.ValidateRequestFormItem(RequestFormItemDataView))
                throw new ValidationException(GlobalNamespace.ItemDimensionAlreadyExist);
        }

        private void CheckZeroQty()
        {
            if (ItemMinimumQtyValidator.IsZeroQty(Qty))
                throw new ValidationException(GlobalNamespace.QtyCannotBeZero);
        }
        private void Save()
        {
            RequestFormItemDataView.SyncStatus = SyncStatus.Not_Sync.GetDescription();
            RequestFormItemDataView.CreatedBy = Auth.Instance.PersonName;
            RequestFormItemDataView.CreatedDate = DateTime.Now;
            RequestFormItemDataView.ItemStatus = ItemStatus.Wait_Sync.GetDescription();
            _requestFormItemRepository.Save(RequestFormItemDataView);
        }

        private void Update()
        {
            RequestFormItemDataView.LastModifiedBy = Auth.Instance.PersonName;
            RequestFormItemDataView.LastModifiedDate = DateTime.Now;
            _requestFormItemRepository.Update(RequestFormItemId,RequestFormItemDataView);
        }
        private void OpenFile(object parameter)
        {
            var filename =_IOService.OpenFileDialog();

            if (filename != null) AttachmentLocalPath = filename;
        } 
        #endregion
    }
}
