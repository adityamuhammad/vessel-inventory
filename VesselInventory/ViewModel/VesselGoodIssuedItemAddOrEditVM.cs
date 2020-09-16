using System;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
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
    class VesselGoodIssuedItemAddOrEditVM : ViewModelBase
    {
        public override string Title => "Good Issued Item";
        public ObservableCollection<ItemGroupDimensionDto> ItemCollection { get; set; } 
            = new ObservableCollection<ItemGroupDimensionDto>();
        public RelayCommand ListBoxChangedCommand { get; private set; }
        public RelayCommand<IClosable> SaveCommand { get; private set; }
        private readonly IVesselGoodIssuedItemRepository _vesselGoodIssuedItemRepository;
        private IDataGrid _parentLoadable;
        public VesselGoodIssuedItemAddOrEditVM(IVesselGoodIssuedItemRepository vesselGoodIssuedItemRepository)
        {
            _vesselGoodIssuedItemRepository = vesselGoodIssuedItemRepository;
            InitializeCommands();
        }

        private void InitializeCommands()
        {
            ListBoxChangedCommand = new RelayCommand(AutoCompleteChanged);
            SaveCommand = new RelayCommand<IClosable>(SaveAction);
        }

        public void InitializeData(IDataGrid parentLoadable, 
            int vesselGoodIssuedId, 
            int vesselGoodIssuedItemId = 0)
        {
            VesselGoodIssuedId = vesselGoodIssuedId;
            VesselGoodIssuedItemId = vesselGoodIssuedItemId;
            _parentLoadable = parentLoadable;
            LoadAttributes();
        }

        private void LoadAttributes()
        {
            if (!RecordHelper.IsNewRecord(VesselGoodIssuedItemId))
            {
                VesselGoodIssuedItemDataView = _vesselGoodIssuedItemRepository.GetById(VesselGoodIssuedItemId);
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
        public bool IsVisibleSearchItem
        {
            get => (RecordHelper.IsNewRecord(VesselGoodIssuedItemId));
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

        public int VesselGoodIssuedItemId
        {
            get => VesselGoodIssuedItemDataView.VesselGoodIssuedItemId;
            set => VesselGoodIssuedItemDataView.VesselGoodIssuedItemId = value;
        }

        public int VesselGoodIssuedId
        {
            get => VesselGoodIssuedItemDataView.VesselGoodIssuedId;
            set => VesselGoodIssuedItemDataView.VesselGoodIssuedId = value;
        }

        public int ItemGroupId
        {
            get => VesselGoodIssuedItemDataView.ItemGroupId;
            set => VesselGoodIssuedItemDataView.ItemGroupId = value;
        }

        public string ItemDimensionNumber
        {
            get => VesselGoodIssuedItemDataView.ItemDimensionNumber;
            set => VesselGoodIssuedItemDataView.ItemDimensionNumber = value;
        }

        public int ItemId
        {
            get => VesselGoodIssuedItemDataView.ItemId;
            set
            {
                VesselGoodIssuedItemDataView.ItemId = value;
                OnPropertyChanged("ItemId");
            }
        }

        public string ItemName
        {
            get => VesselGoodIssuedItemDataView.ItemName;
            set
            {
                VesselGoodIssuedItemDataView.ItemName = value;
                OnPropertyChanged("ItemName");
            }
        }

        public string BrandTypeId
        {
            get => VesselGoodIssuedItemDataView.BrandTypeId;
            set
            {
                VesselGoodIssuedItemDataView.BrandTypeId = value;
                OnPropertyChanged("BrandTypeId");
            }
        }

        public string BrandTypeName
        {
            get => VesselGoodIssuedItemDataView.BrandTypeName;
            set
            {
                VesselGoodIssuedItemDataView.BrandTypeName = value;
                OnPropertyChanged("BrandTypeName");
            }
        }
        public string ColorSizeId
        {
            get => VesselGoodIssuedItemDataView.ColorSizeId;
            set
            {
                VesselGoodIssuedItemDataView.ColorSizeId = value;
                OnPropertyChanged("ColorSizeId");
            }
        }

        public string ColorSizeName
        {
            get => VesselGoodIssuedItemDataView.ColorSizeName;
            set
            {
                VesselGoodIssuedItemDataView.ColorSizeName = value;
                OnPropertyChanged("ColorSizeName");
            }
        }

        public decimal Qty
        {
            get => VesselGoodIssuedItemDataView.Qty;
            set
            {
                VesselGoodIssuedItemDataView.Qty = value;
                OnPropertyChanged("Qty");
            }
        }

        public string Uom
        {
            get => VesselGoodIssuedItemDataView.Uom;
            set
            {
                VesselGoodIssuedItemDataView.Uom = value;
                OnPropertyChanged("Uom");
            }
        }


        private VesselGoodIssuedItem VesselGoodIssuedItemDataView { get; set; } = new VesselGoodIssuedItem();
        public void LoadItem()
        {
            ItemCollection.Clear();
            foreach(var _ in CommonDataHelper.GetItems(ItemSelectKeyword, "VesselGoodIssuedItem", VesselGoodIssuedId))
                ItemCollection.Add(_);
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
            catch (ValidationException ex)
            {
                ResponseMessage.Error(
                    string.Format("{0} {1}", 
                    GlobalNamespace.ErrorSave ,ex.Message));
            } catch (Exception)
            {
                ResponseMessage.Error(
                    string.Format("{0} {1}", 
                    GlobalNamespace.Error, 
                    GlobalNamespace.ErrorSave ));
            }
        }

        private void ItemCheckUnique()
        {
            if (ItemUniqueValidator.ValidateVesselGoodIssuedItem(VesselGoodIssuedItemDataView))
                throw new ValidationException(GlobalNamespace.ItemDimensionAlreadyExist);
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
            if (!ItemMinimumQtyValidator.IsStockAvailable(ItemId, ItemDimensionNumber, Qty, Title, VesselGoodIssuedId))
                throw new ValidationException(GlobalNamespace.StockIsNotAvailable);
        }

        private void SaveOrUpdate()
        {
            if (RecordHelper.IsNewRecord(VesselGoodIssuedItemId))
            {
                ItemCheckUnique();
                CheckInStockSave();
                Save();

            } else
            {
                CheckInStockUpdate();
                update();
            }
        }

        private void update()
        {
            VesselGoodIssuedItemDataView.LastModifiedBy = Auth.Instance.PersonName;
            VesselGoodIssuedItemDataView.LastModifiedDate = DateTime.Now;
            _vesselGoodIssuedItemRepository.UpdateTransaction(VesselGoodIssuedItemDataView);

        }
        private void Save()
        {

            VesselGoodIssuedItemDataView.CreatedBy = Auth.Instance.PersonName;
            VesselGoodIssuedItemDataView.CreatedDate = DateTime.Now;
            VesselGoodIssuedItemDataView.SyncStatus = SyncStatus.Not_Sync.GetDescription();
            _vesselGoodIssuedItemRepository.SaveTransaction(VesselGoodIssuedItemDataView);
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
