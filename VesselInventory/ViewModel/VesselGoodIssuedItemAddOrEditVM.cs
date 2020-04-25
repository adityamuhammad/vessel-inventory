using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using VesselInventory.Commons.HelperFunctions;
using VesselInventory.Dto;
using VesselInventory.Models;
using VesselInventory.Repository;
using VesselInventory.Services;
using VesselInventory.Utility;

namespace VesselInventory.ViewModel
{
    class VesselGoodIssuedItemAddOrEditVM : ViewModelBase, IParentLoadable
    {
        public override string Title => "Good Issued Item";
        public RelayCommand ListBoxChangedCommand { get; private set; }
        public RelayCommand<IClosable> SaveCommand { get; private set; }
        IVesselGoodIssuedItemRepository _vesselGoodIssuedItemRepository;
        public VesselGoodIssuedItemAddOrEditVM(IVesselGoodIssuedItemRepository vesselGoodIssuedItemRepository)
        {
            _vesselGoodIssuedItemRepository = vesselGoodIssuedItemRepository;
            ListBoxChangedCommand = new RelayCommand(AutoCompleteChanged);
            SaveCommand = new RelayCommand<IClosable>(SaveAction);
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
        private bool _IsVisibleSearchItem = true;
        public bool IsVisibleSearchItem {
            get
            {
                if (true)
                    _IsVisibleSearchItem = true;
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

        public int vessel_good_issued_item_id
        {
            get => VesselGoodIssuedItemDataView.vessel_good_issued_item_id;
            set => VesselGoodIssuedItemDataView.vessel_good_issued_item_id = value;
        }

        public int vessel_good_issued_id
        {
            get => VesselGoodIssuedItemDataView.vessel_good_issued_id;
            set => VesselGoodIssuedItemDataView.vessel_good_issued_id = value;
        }

        public int item_group_id
        {
            get => VesselGoodIssuedItemDataView.item_group_id;
            set => VesselGoodIssuedItemDataView.item_group_id = value;
        }

        public string item_dimension_number
        {
            get => VesselGoodIssuedItemDataView.item_dimension_number;
            set => VesselGoodIssuedItemDataView.item_dimension_number = value;
        }

        public int item_id
        {
            get => VesselGoodIssuedItemDataView.item_id;
            set
            {
                VesselGoodIssuedItemDataView.item_id = value;
                OnPropertyChanged("item_id");
            }
        }

        public string item_name
        {
            get => VesselGoodIssuedItemDataView.item_name;
            set
            {
                VesselGoodIssuedItemDataView.item_name = value;
                OnPropertyChanged("item_name");
            }
        }

        public string brand_type_id
        {
            get => VesselGoodIssuedItemDataView.brand_type_id;
            set
            {
                VesselGoodIssuedItemDataView.brand_type_id = value;
                OnPropertyChanged("brand_type_id");
            }
        }

        public string brand_type_name
        {
            get => VesselGoodIssuedItemDataView.brand_type_name;
            set
            {
                VesselGoodIssuedItemDataView.brand_type_name = value;
                OnPropertyChanged("brand_type_name");
            }
        }
        public string color_size_id
        {
            get => VesselGoodIssuedItemDataView.color_size_id;
            set
            {
                VesselGoodIssuedItemDataView.color_size_id = value;
                OnPropertyChanged("color_size_id");
            }
        }

        public string color_size_name
        {
            get => VesselGoodIssuedItemDataView.color_size_name;
            set
            {
                VesselGoodIssuedItemDataView.color_size_name = value;
                OnPropertyChanged("color_size_name");
            }
        }

        public decimal qty
        {
            get => VesselGoodIssuedItemDataView.qty;
            set
            {
                VesselGoodIssuedItemDataView.qty = value;
                OnPropertyChanged("qty");
            }
        }

        public string uom
        {
            get => VesselGoodIssuedItemDataView.uom;
            set
            {
                VesselGoodIssuedItemDataView.uom = value;
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
                .GetItems(ItemSelectKeyword, "vessel_good_issued_item", 20))
                ItemCollection.Add(_);
        }

        private void SaveAction(IClosable window)
        {
            _vesselGoodIssuedItemRepository.SaveTransaction(VesselGoodIssuedItemDataView);
            CloseWindow(window);
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

        public void LoadDataGrid()
        {
            throw new NotImplementedException();
        }
    }
}
