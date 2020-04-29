using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
            vessel_good_return_id = vesselGoodReturnId;
            vessel_good_return_item_id = vesselGoodReturnItemId;
            _parentLoadable = parentLoadable;
            LoadAttributes();
        }

        private void LoadAttributes()
        {
            if (!RecordHelper.IsNewRecord(vessel_good_return_item_id))
            {
                VesselGoodReturnItemDataView = _vesselGoodReturnItemRepository.GetById(vessel_good_return_item_id);
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
                return (RecordHelper.IsNewRecord(vessel_good_return_item_id));
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

        public int vessel_good_return_item_id
        {
            get => VesselGoodReturnItemDataView.VesselGoodReturnItemId;
            set => VesselGoodReturnItemDataView.VesselGoodReturnItemId = value;
        }

        public int vessel_good_return_id
        {
            get => VesselGoodReturnItemDataView.VesselGoodReturnId;
            set => VesselGoodReturnItemDataView.VesselGoodReturnId = value;
        }

        public int item_group_id
        {
            get => VesselGoodReturnItemDataView.ItemGroupId;
            set => VesselGoodReturnItemDataView.ItemGroupId = value;
        }

        public string item_dimension_number
        {
            get => VesselGoodReturnItemDataView.ItemDimensionNumber;
            set => VesselGoodReturnItemDataView.ItemDimensionNumber = value;
        }

        public int item_id
        {
            get => VesselGoodReturnItemDataView.ItemId;
            set
            {
                VesselGoodReturnItemDataView.ItemId = value;
                OnPropertyChanged("item_id");
            }
        }

        public string item_name
        {
            get => VesselGoodReturnItemDataView.ItemName;
            set
            {
                VesselGoodReturnItemDataView.ItemName = value;
                OnPropertyChanged("item_name");
            }
        }

        public string brand_type_id
        {
            get => VesselGoodReturnItemDataView.BrandTypeId;
            set
            {
                VesselGoodReturnItemDataView.BrandTypeId = value;
                OnPropertyChanged("brand_type_id");
            }
        }

        public string brand_type_name
        {
            get => VesselGoodReturnItemDataView.BrandTypeName;
            set
            {
                VesselGoodReturnItemDataView.BrandTypeName = value;
                OnPropertyChanged("brand_type_name");
            }
        }
        public string color_size_id
        {
            get => VesselGoodReturnItemDataView.ColorSizeId;
            set
            {
                VesselGoodReturnItemDataView.ColorSizeId = value;
                OnPropertyChanged("color_size_id");
            }
        }

        public string color_size_name
        {
            get => VesselGoodReturnItemDataView.ColorSizeName;
            set
            {
                VesselGoodReturnItemDataView.ColorSizeName = value;
                OnPropertyChanged("color_size_name");
            }
        }

        public decimal qty
        {
            get => VesselGoodReturnItemDataView.Qty;
            set
            {
                VesselGoodReturnItemDataView.Qty = value;
                OnPropertyChanged("qty");
            }
        }

        public string uom
        {
            get => VesselGoodReturnItemDataView.Uom;
            set
            {
                VesselGoodReturnItemDataView.Uom = value;
                OnPropertyChanged("uom");
            }
        }
        public string reason
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
                OnPropertyChanged("reason");
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
                .GetItems(ItemSelectKeyword, "vessel_good_return_item", vessel_good_return_id))
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
            if (ItemUniqueValidator.ValidateVesselGoodReturnItem(VesselGoodReturnItemDataView))
                throw new Exception(GlobalNamespace.ItemDimensionAlreadyExist);
        }

        private void SaveOrUpdate()
        {
            if (RecordHelper.IsNewRecord(vessel_good_return_item_id))
            {
                ItemCheckUnique();
                _vesselGoodReturnItemRepository
                    .SaveTransaction(VesselGoodReturnItemDataView);

            } else
            {
                _vesselGoodReturnItemRepository
                    .UpdateTransaction(vessel_good_return_item_id, 
                    VesselGoodReturnItemDataView);

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
