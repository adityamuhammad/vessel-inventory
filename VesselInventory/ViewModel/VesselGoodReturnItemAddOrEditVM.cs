using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using VesselInventory.Commons;
using VesselInventory.Commons.Enums;
using VesselInventory.Commons.HelperFunctions;
using VesselInventory.Dto;
using VesselInventory.Models;
using VesselInventory.Repository;
using VesselInventory.Services;
using VesselInventory.Utility;
using VesselInventory.Validations;

namespace VesselInventory.ViewModel
{
    class VesselGoodReturnItemAddOrEditVM : ViewModelBase
    {
        public override string Title => "Good Return Item";
        public RelayCommand ListBoxChangedCommand { get; private set; }
        public RelayCommand<IClosable> SaveCommand { get; private set; }
        private readonly IVesselGoodReturnItemRepository _vesselGoodReturnItemRepository;
        private IParentLoadable _parentLoadable;
        public VesselGoodReturnItemAddOrEditVM(IVesselGoodReturnItemRepository vesselGoodReturnItemRepository)
        {
            _vesselGoodReturnItemRepository = vesselGoodReturnItemRepository;
            InitializeCommands();
        }
        private void InitializeCommands()
        {
            ListBoxChangedCommand = new RelayCommand(AutoCompleteChanged);
            SaveCommand = new RelayCommand<IClosable>(SaveAction);
        }
        public void InitializeData(IParentLoadable parentLoadable, 
            int vesselGoodReturnId, 
            int vesselGoodReturnItemId = 0)
        {
            VesselGoodReturnId = vesselGoodReturnId;
            VesselGooReturnItemId = vesselGoodReturnItemId;
            _parentLoadable = parentLoadable;
            LoadAttributes();
        }

        private void LoadAttributes()
        {
            if (!RecordHelper.IsNewRecord(VesselGooReturnItemId))
            {
                VesselGoodReturnItemDataView = _vesselGoodReturnItemRepository.GetById(VesselGooReturnItemId);
            }
        }

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
                return (RecordHelper.IsNewRecord(VesselGooReturnItemId));
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

        public int VesselGooReturnItemId
        {
            get => VesselGoodReturnItemDataView.VesselGoodReturnItemId;
            set => VesselGoodReturnItemDataView.VesselGoodReturnItemId = value;
        }

        public int VesselGoodReturnId
        {
            get => VesselGoodReturnItemDataView.VesselGoodReturnId;
            set => VesselGoodReturnItemDataView.VesselGoodReturnId = value;
        }

        public int ItemGroupId
        {
            get => VesselGoodReturnItemDataView.ItemGroupId;
            set => VesselGoodReturnItemDataView.ItemGroupId = value;
        }

        public string ItemDimensionNumber
        {
            get => VesselGoodReturnItemDataView.ItemDimensionNumber;
            set => VesselGoodReturnItemDataView.ItemDimensionNumber = value;
        }

        public int ItemId
        {
            get => VesselGoodReturnItemDataView.ItemId;
            set
            {
                VesselGoodReturnItemDataView.ItemId = value;
                OnPropertyChanged("ItemId");
            }
        }

        public string ItemName
        {
            get => VesselGoodReturnItemDataView.ItemName;
            set
            {
                VesselGoodReturnItemDataView.ItemName = value;
                OnPropertyChanged("ItemName");
            }
        }

        public string BrandTypeId
        {
            get => VesselGoodReturnItemDataView.BrandTypeId;
            set
            {
                VesselGoodReturnItemDataView.BrandTypeId = value;
                OnPropertyChanged("BrandTypeId");
            }
        }

        public string BrandTypeName
        {
            get => VesselGoodReturnItemDataView.BrandTypeName;
            set
            {
                VesselGoodReturnItemDataView.BrandTypeName = value;
                OnPropertyChanged("BrandTypeName");
            }
        }
        public string ColorSizeId
        {
            get => VesselGoodReturnItemDataView.ColorSizeId;
            set
            {
                VesselGoodReturnItemDataView.ColorSizeId = value;
                OnPropertyChanged("ColorSizeId");
            }
        }

        public string ColorSizeName
        {
            get => VesselGoodReturnItemDataView.ColorSizeName;
            set
            {
                VesselGoodReturnItemDataView.ColorSizeName = value;
                OnPropertyChanged("ColorSizeName");
            }
        }

        public decimal Qty
        {
            get => VesselGoodReturnItemDataView.Qty;
            set
            {
                VesselGoodReturnItemDataView.Qty = value;
                OnPropertyChanged("Qty");
            }
        }

        public string Uom
        {
            get => VesselGoodReturnItemDataView.Uom;
            set
            {
                VesselGoodReturnItemDataView.Uom = value;
                OnPropertyChanged("Uom");
            }
        }
        public string Reason
        {
            get
            {
                if (VesselGoodReturnItemDataView.Reason is null)
                    VesselGoodReturnItemDataView.Reason = ReasonCollection.First();
                return VesselGoodReturnItemDataView.Reason;
            }
            set
            {
                VesselGoodReturnItemDataView.Reason = value;
                OnPropertyChanged("Reason");
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

        public ObservableCollection<ItemGroupDimensionDto> ItemCollection { get; set; } 
            = new ObservableCollection<ItemGroupDimensionDto>();

        private VesselGoodReturnItem VesselGoodReturnItemDataView { get; set; } = new VesselGoodReturnItem();
        public void LoadItem()
        {
            ItemCollection.Clear();
            foreach(var _ in CommonDataHelper
                .GetItems(ItemSelectKeyword, "VesselGoodReturnItem", VesselGoodReturnId))
                ItemCollection.Add(_);
        }

        private void CheckZeroQty()
        {
            if (ItemMinimumQtyValidator.IsZeroQty(Qty))
                throw new Exception(GlobalNamespace.QtyCannotBeZero);
        }

        private void CheckInStockSave()
        {
            if (!ItemMinimumQtyValidator.IsStockAvailable(ItemId, ItemDimensionNumber, Qty))
                throw new Exception(GlobalNamespace.StockIsNotAvailable);
        }

        private void CheckInStockUpdate()
        {
            if (!ItemMinimumQtyValidator.IsStockAvailable(ItemId, ItemDimensionNumber, Qty, Title, VesselGoodReturnId))
                throw new Exception(GlobalNamespace.StockIsNotAvailable);
        }

        private void SaveAction(IClosable window)
        {
            try
            {
                CheckZeroQty();
                SaveOrUpdate();
                _parentLoadable.LoadDataGrid();
                CloseWindow(window);
                ResponseMessage.Success(GlobalNamespace.SuccessSave);
            }
            catch (Exception ex)
            {
                ResponseMessage.Error(string.Format("{0} {1} {2}", GlobalNamespace.Error, GlobalNamespace.ErrorSave ,ex.Message));
            }
        }

        private void ItemCheckUnique()
        {
            if (ItemUniqueValidator.ValidateVesselGoodReturnItem(VesselGoodReturnItemDataView))
                throw new Exception(GlobalNamespace.ItemDimensionAlreadyExist);
        }

        private void SaveOrUpdate()
        {
            if (RecordHelper.IsNewRecord(VesselGooReturnItemId))
            {
                ItemCheckUnique();
                CheckInStockSave();
                VesselGoodReturnItemDataView.CreatedBy = Auth.Instance.PersonName;
                VesselGoodReturnItemDataView.CreatedDate = DateTime.Now;
                VesselGoodReturnItemDataView.SyncStatus = SyncStatus.Not_Sync.GetDescription();
                _vesselGoodReturnItemRepository.SaveTransaction(VesselGoodReturnItemDataView);

            } else
            {
                CheckInStockUpdate();
                VesselGoodReturnItemDataView.LastModifiedBy = Auth.Instance.PersonName;
                VesselGoodReturnItemDataView.LastModifiedDate = DateTime.Now;
                _vesselGoodReturnItemRepository.UpdateTransaction(VesselGooReturnItemId, 
                    VesselGoodReturnItemDataView);

            }
        }

        private void AutoCompleteChanged(object parameter)
        {
            IsVisibleListBoxItem = false;
            ItemGroupDimensionDto item = (ItemGroupDimensionDto)parameter;
            if (item != null)
            {
                ItemId = item.ItemId;
                ItemName = item.ItemName;
                BrandTypeId = item.BrandTypeId;
                BrandTypeName = item.BrandTypeName;
                ColorSizeId = item.ColorSizeId;
                ColorSizeName = item.ColorSizeName;
                ItemDimensionNumber = item.ItemDimensionNumber;
                ItemGroupId = item.ItemGroupId;
                Uom = item.Uom;
                ItemSelectKeyword = string.Empty;
            }
        }
    }
}
