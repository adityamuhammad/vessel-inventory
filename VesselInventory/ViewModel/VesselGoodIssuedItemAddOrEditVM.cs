using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using VesselInventory.Commons;
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
        public RelayCommand ListBoxChangedCommand { get; private set; }
        public RelayCommand<IClosable> SaveCommand { get; private set; }
        private readonly IVesselGoodIssuedItemRepository _vesselGoodIssuedItemRepository;
        private IParentLoadable _parentLoadable;
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

        public void InitializeData(IParentLoadable parentLoadable, 
            int vesselGoodIssuedId, 
            int vesselGoodIssuedItemId = 0)
        {
            vessel_good_issued_id = vesselGoodIssuedId;
            vessel_good_issued_item_id = vesselGoodIssuedItemId;
            _parentLoadable = parentLoadable;
            LoadAttributes();
        }

        private void LoadAttributes()
        {
            if (!RecordHelper.IsNewRecord(vessel_good_issued_item_id))
            {
                VesselGoodIssuedItemDataView = _vesselGoodIssuedItemRepository.GetById(vessel_good_issued_item_id);
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
                return (RecordHelper.IsNewRecord(vessel_good_issued_item_id));
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

        public int vessel_good_issued_item_id
        {
            get => VesselGoodIssuedItemDataView.VesselGoodIssuedItemId;
            set => VesselGoodIssuedItemDataView.VesselGoodIssuedItemId = value;
        }

        public int vessel_good_issued_id
        {
            get => VesselGoodIssuedItemDataView.VesselGoodIssuedId;
            set => VesselGoodIssuedItemDataView.VesselGoodIssuedId = value;
        }

        public int item_group_id
        {
            get => VesselGoodIssuedItemDataView.ItemGroupId;
            set => VesselGoodIssuedItemDataView.ItemGroupId = value;
        }

        public string item_dimension_number
        {
            get => VesselGoodIssuedItemDataView.ItemDimensionNumber;
            set => VesselGoodIssuedItemDataView.ItemDimensionNumber = value;
        }

        public int item_id
        {
            get => VesselGoodIssuedItemDataView.ItemId;
            set
            {
                VesselGoodIssuedItemDataView.ItemId = value;
                OnPropertyChanged("item_id");
            }
        }

        public string item_name
        {
            get => VesselGoodIssuedItemDataView.ItemName;
            set
            {
                VesselGoodIssuedItemDataView.ItemName = value;
                OnPropertyChanged("item_name");
            }
        }

        public string brand_type_id
        {
            get => VesselGoodIssuedItemDataView.BrandTypeId;
            set
            {
                VesselGoodIssuedItemDataView.BrandTypeId = value;
                OnPropertyChanged("brand_type_id");
            }
        }

        public string brand_type_name
        {
            get => VesselGoodIssuedItemDataView.BrandTypeName;
            set
            {
                VesselGoodIssuedItemDataView.BrandTypeName = value;
                OnPropertyChanged("brand_type_name");
            }
        }
        public string color_size_id
        {
            get => VesselGoodIssuedItemDataView.ColorSizeId;
            set
            {
                VesselGoodIssuedItemDataView.ColorSizeId = value;
                OnPropertyChanged("color_size_id");
            }
        }

        public string color_size_name
        {
            get => VesselGoodIssuedItemDataView.ColorSizeName;
            set
            {
                VesselGoodIssuedItemDataView.ColorSizeName = value;
                OnPropertyChanged("color_size_name");
            }
        }

        public decimal qty
        {
            get => VesselGoodIssuedItemDataView.Qty;
            set
            {
                VesselGoodIssuedItemDataView.Qty = value;
                OnPropertyChanged("qty");
            }
        }

        public string uom
        {
            get => VesselGoodIssuedItemDataView.Uom;
            set
            {
                VesselGoodIssuedItemDataView.Uom = value;
                OnPropertyChanged("uom");
            }
        }

        public ObservableCollection<ItemGroupDimensionDto> ItemCollection { get; set; } 
            = new ObservableCollection<ItemGroupDimensionDto>();

        private VesselGoodIssuedItem VesselGoodIssuedItemDataView { get; set; } = new VesselGoodIssuedItem();
        public void LoadItem()
        {
            ItemCollection.Clear();
            foreach(var _ in CommonDataHelper
                .GetItems(ItemSelectKeyword, "vessel_good_issued_item", vessel_good_issued_id))
                ItemCollection.Add(_);
        }

        private void SaveAction(IClosable window)
        {
            try
            {
                SaveOrUpdate();
                _parentLoadable.LoadDataGrid();
                CloseWindow(window);
                ResponseMessage.Success(GlobalNamespace.SuccessSave);
            }
            catch (Exception ex)
            {
                ResponseMessage.Error(GlobalNamespace.Error + ' ' + GlobalNamespace.ErrorSave + ' ' +ex.Message);
            }
        }

        private void ItemCheckUnique()
        {
            if (ItemUniqueValidator.ValidateVesselGoodIssuedItem(VesselGoodIssuedItemDataView))
                throw new Exception(GlobalNamespace.ItemDimensionAlreadyExist);
        }

        private void SaveOrUpdate()
        {
            if (RecordHelper.IsNewRecord(vessel_good_issued_item_id))
            {
                ItemCheckUnique();
                _vesselGoodIssuedItemRepository
                    .SaveTransaction(VesselGoodIssuedItemDataView);

            } else
            {
                _vesselGoodIssuedItemRepository
                    .UpdateTransaction(vessel_good_issued_item_id, 
                    VesselGoodIssuedItemDataView);

            }
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
    }
}
