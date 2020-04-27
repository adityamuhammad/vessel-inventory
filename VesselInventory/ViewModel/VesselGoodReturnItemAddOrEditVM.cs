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
            get => VesselGoodReturnItemDataView.vessel_good_return_item_id;
            set => VesselGoodReturnItemDataView.vessel_good_return_item_id = value;
        }

        public int vessel_good_return_id
        {
            get => VesselGoodReturnItemDataView.vessel_good_return_id;
            set => VesselGoodReturnItemDataView.vessel_good_return_id = value;
        }

        public int item_group_id
        {
            get => VesselGoodReturnItemDataView.item_group_id;
            set => VesselGoodReturnItemDataView.item_group_id = value;
        }

        public string item_dimension_number
        {
            get => VesselGoodReturnItemDataView.item_dimension_number;
            set => VesselGoodReturnItemDataView.item_dimension_number = value;
        }

        public int item_id
        {
            get => VesselGoodReturnItemDataView.item_id;
            set
            {
                VesselGoodReturnItemDataView.item_id = value;
                OnPropertyChanged("item_id");
            }
        }

        public string item_name
        {
            get => VesselGoodReturnItemDataView.item_name;
            set
            {
                VesselGoodReturnItemDataView.item_name = value;
                OnPropertyChanged("item_name");
            }
        }

        public string brand_type_id
        {
            get => VesselGoodReturnItemDataView.brand_type_id;
            set
            {
                VesselGoodReturnItemDataView.brand_type_id = value;
                OnPropertyChanged("brand_type_id");
            }
        }

        public string brand_type_name
        {
            get => VesselGoodReturnItemDataView.brand_type_name;
            set
            {
                VesselGoodReturnItemDataView.brand_type_name = value;
                OnPropertyChanged("brand_type_name");
            }
        }
        public string color_size_id
        {
            get => VesselGoodReturnItemDataView.color_size_id;
            set
            {
                VesselGoodReturnItemDataView.color_size_id = value;
                OnPropertyChanged("color_size_id");
            }
        }

        public string color_size_name
        {
            get => VesselGoodReturnItemDataView.color_size_name;
            set
            {
                VesselGoodReturnItemDataView.color_size_name = value;
                OnPropertyChanged("color_size_name");
            }
        }

        public decimal qty
        {
            get => VesselGoodReturnItemDataView.qty;
            set
            {
                VesselGoodReturnItemDataView.qty = value;
                OnPropertyChanged("qty");
            }
        }

        public string uom
        {
            get => VesselGoodReturnItemDataView.uom;
            set
            {
                VesselGoodReturnItemDataView.uom = value;
                OnPropertyChanged("uom");
            }
        }
        public string reason
        {
            get
            {
                if (VesselGoodReturnItemDataView.reason is null)
                    VesselGoodReturnItemDataView.reason = ReasonCollection.First();
                return VesselGoodReturnItemDataView.reason;
            }
            set
            {
                VesselGoodReturnItemDataView.reason = value;
                OnPropertyChanged("reason");
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
    }
}
