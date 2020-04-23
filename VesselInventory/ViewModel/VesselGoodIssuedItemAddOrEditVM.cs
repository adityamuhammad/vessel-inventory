using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using VesselInventory.Commons.HelperFunctions;
using VesselInventory.Dto;
using VesselInventory.Models;
using VesselInventory.Services;
using VesselInventory.Utility;

namespace VesselInventory.ViewModel
{
    class VesselGoodIssuedItemAddOrEditVM : ViewModelBase, IParentLoadable
    {
        public override string Title => "Good Issued Item";
        public RelayCommand ListBoxChangedCommand { get; private set; }
        public RelayCommand<IClosable> SaveCommand { get; private set; }
        public VesselGoodIssuedItemAddOrEditVM()
        {
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
            get => VesselGoodIssuedItemEntity.vessel_good_issued_item_id;
            set => VesselGoodIssuedItemEntity.vessel_good_issued_item_id = value;
        }

        public int vessel_good_issued_id
        {
            get => VesselGoodIssuedItemEntity.vessel_good_issued_id;
            set => VesselGoodIssuedItemEntity.vessel_good_issued_id = value;
        }

        public int item_group_id
        {
            get => VesselGoodIssuedItemEntity.item_group_id;
            set => VesselGoodIssuedItemEntity.item_group_id = value;
        }

        public string item_dimension_number
        {
            get => VesselGoodIssuedItemEntity.item_dimension_number;
            set => VesselGoodIssuedItemEntity.item_dimension_number = value;
        }

        public int item_id
        {
            get => VesselGoodIssuedItemEntity.item_id;
            set
            {
                VesselGoodIssuedItemEntity.item_id = value;
                OnPropertyChanged("item_id");
            }
        }

        public string item_name
        {
            get => VesselGoodIssuedItemEntity.item_name;
            set
            {
                VesselGoodIssuedItemEntity.item_name = value;
                OnPropertyChanged("item_name");
            }
        }

        public string brand_type_id
        {
            get => VesselGoodIssuedItemEntity.brand_type_id;
            set
            {
                VesselGoodIssuedItemEntity.brand_type_id = value;
                OnPropertyChanged("brand_type_id");
            }
        }

        public string brand_type_name
        {
            get => VesselGoodIssuedItemEntity.brand_type_name;
            set
            {
                VesselGoodIssuedItemEntity.brand_type_name = value;
                OnPropertyChanged("brand_type_name");
            }
        }
        public string color_size_id
        {
            get => VesselGoodIssuedItemEntity.color_size_id;
            set
            {
                VesselGoodIssuedItemEntity.color_size_id = value;
                OnPropertyChanged("color_size_id");
            }
        }

        public string color_size_name
        {
            get => VesselGoodIssuedItemEntity.color_size_name;
            set
            {
                VesselGoodIssuedItemEntity.color_size_name = value;
                OnPropertyChanged("color_size_name");
            }
        }

        public decimal qty
        {
            get => VesselGoodIssuedItemEntity.qty;
            set
            {
                VesselGoodIssuedItemEntity.qty = value;
                OnPropertyChanged("qty");
            }
        }

        public string uom
        {
            get => VesselGoodIssuedItemEntity.uom;
            set
            {
                VesselGoodIssuedItemEntity.uom = value;
                OnPropertyChanged("uom");
            }
        }

        public ObservableCollection<ItemGroupDimensionDto> ItemCollection { get; set; } 
            = new ObservableCollection<ItemGroupDimensionDto>();

        private VesselGoodIssuedItem VesselGoodIssuedItemEntity { get; set; } = new VesselGoodIssuedItem();
        public void LoadItem()
        {
            ItemCollection.Clear();
            foreach(var _ in CommonDataHelper
                .GetItems(ItemSelectKeyword, "vessel_good_issued_item", 20))
                ItemCollection.Add(_);
        }

        private void SaveAction(IClosable parameter)
        {
            throw new NotImplementedException();
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
