using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
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
        public ObservableCollection<ItemGroupDimensionDto> ItemCollection { get; set; } 
            = new ObservableCollection<ItemGroupDimensionDto>();
        public RelayCommand ListBoxChangedCommand { get; private set; }
        public RelayCommand<IClosable> SaveCommand { get; private set; }
        private readonly IVesselGoodReturnItemRepository _vesselGoodReturnItemRepository;
        private IDataGrid _parentLoadable;
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
        public void InitializeData(IDataGrid parentLoadable, 
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
            get => (RecordHelper.IsNewRecord(VesselGooReturnItemId));
        }

        private bool _IsVisibleListBoxItem = false;
        public bool IsVisibleListBoxItem
        {
            get => _IsVisibleListBoxItem;
            set
            {
                if  (_IsVisibleListBoxItem == value) return;

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

        private VesselGoodReturnItem VesselGoodReturnItemDataView { get; set; } = new VesselGoodReturnItem();
        public void LoadItem()
        {
            ItemCollection.Clear();
            foreach(var _ in CommonDataHelper.GetItems(ItemSelectKeyword, "VesselGoodReturnItem", VesselGoodReturnId))
                ItemCollection.Add(_);
        }

        private void CheckZeroQty()
        {
            if (ItemMinimumQtyValidator.IsZeroQty(Qty))
                throw new ValidationException(GlobalNamespace.QtyCannotBeZero);
        }

        private void CheckInStockSave()
        {
            if (!ItemMinimumQtyValidator.IsStockAvailable(ItemId, ItemDimensionNumber, Qty))
                throw new ValidationException(GlobalNamespace.StockIsNotAvailable);
        }

        private void CheckInStockUpdate()
        {
            if (!ItemMinimumQtyValidator.IsStockAvailable(ItemId, ItemDimensionNumber, Qty, Title, VesselGoodReturnId))
                throw new ValidationException(GlobalNamespace.StockIsNotAvailable);
        }

        private void SaveAction(IClosable window)
        {
            try
            {
                CheckZeroQty();
                SaveOrUpdate();
                LoadDataGrid();
                CloseWindow(window);
                ResponseMessage.Success(GlobalNamespace.SuccessSave);
            } catch (ValidationException ex)
            {
                ResponseMessage.Error(string.Format("{0} {1}",GlobalNamespace.ErrorSave ,ex.Message));
            }
            catch (Exception)
            {
                ResponseMessage.Error(string.Format("{0} {1}", GlobalNamespace.Error, GlobalNamespace.ErrorSave));
            }
        }

        private void LoadDataGrid()
        {
            _parentLoadable.LoadDataGrid();
        }

        private void ItemCheckUnique()
        {
            if (ItemUniqueValidator.ValidateVesselGoodReturnItem(VesselGoodReturnItemDataView))
                throw new ValidationException(GlobalNamespace.ItemDimensionAlreadyExist);
        }

        private void SaveOrUpdate()
        {
            if (RecordHelper.IsNewRecord(VesselGooReturnItemId))
            {
                ItemCheckUnique();
                CheckInStockSave();
                Save();

            }
            else
            {
                CheckInStockUpdate();
                Update();

            }
        }

        private void Update()
        {
            VesselGoodReturnItemDataView.LastModifiedBy = Auth.Instance.PersonName;
            VesselGoodReturnItemDataView.LastModifiedDate = DateTime.Now;
            _vesselGoodReturnItemRepository.UpdateTransaction(VesselGoodReturnItemDataView);
        }

        private void Save()
        {
            VesselGoodReturnItemDataView.CreatedBy = Auth.Instance.PersonName;
            VesselGoodReturnItemDataView.CreatedDate = DateTime.Now;
            VesselGoodReturnItemDataView.SyncStatus = SyncStatus.Not_Sync.GetDescription();
            _vesselGoodReturnItemRepository.SaveTransaction(VesselGoodReturnItemDataView);
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
