using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using VesselInventory.DTO;
using VesselInventory.Helpers;
using VesselInventory.Models;
using VesselInventory.Utility;

namespace VesselInventory.ViewModel
{
    public class RequestFormItemAddOrEditViewModel : ViewModelBase
    {

        rf_item _rf_item = new rf_item();
        public RelayCommand ListBoxChanged { get; private set; }
        public RequestFormItemAddOrEditViewModel()
        {
            IsVisibleListBoxItem = false;
            ListBoxChanged = new RelayCommand(AutoCompleteChanged);
            RefreshItems();
        }

        #region
        public int? item_id
        {
            get => _rf_item.item_id;
            set
            {
                _rf_item.item_id = value;
                OnPropertyChanged("item_id");
            }
        }

        public string item_name
        {
            get => _rf_item.item_name;
            set
            {
                _rf_item.item_name = value;
                OnPropertyChanged("item_name");
            }
        }

        public string brand_type_id
        {
            get => _rf_item.brand_type_id;
            set
            {
                _rf_item.brand_type_id = value;
                OnPropertyChanged("brand_type_id");
            }
        }

        public string brand_type_name
        {
            get => _rf_item.brand_type_name;
            set
            {
                _rf_item.brand_type_name = value;
                OnPropertyChanged("brand_type_name");
            }
        }

        public string color_size_id
        {
            get => _rf_item.color_size_id;
            set
            {
                _rf_item.color_size_id = value;
                OnPropertyChanged("color_size_id");
            }
        }

        public string color_size_name
        {
            get => _rf_item.color_size_name;
            set
            {
                _rf_item.color_size_name = value;
                OnPropertyChanged("color_size_name");
            }
        }
        #endregion

        private string _itemSelectKeyword = "";
        public string ItemSelectKeyword
        {
            get => _itemSelectKeyword;
            set
            {
                _itemSelectKeyword = value;
                OnPropertyChanged("ItemSelectKeyword");
                if (value == "")
                {
                    IsVisibleListBoxItem = false;
                } else
                {
                    IsVisibleListBoxItem = true;
                    RefreshItems();
                }
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

        private ObservableCollection<ItemGroupDimensionDTO> _items = new ObservableCollection<ItemGroupDimensionDTO>();
        public ObservableCollection<ItemGroupDimensionDTO> Items
        {
            get => _items;
            set
            {
                _items = value;
                OnPropertyChanged("Items");
            }
        }

        public void RefreshItems()
        {
            _items.Clear();
            foreach(var _ in GenericHelper.GetItems(ItemSelectKeyword))
            {
                _items.Add(new ItemGroupDimensionDTO
                {
                    item_id = _.item_id,
                    item_name = _.item_name,
                    brand_type_id = _.brand_type_id,
                    brand_type_name = _.brand_type_name,
                    color_size_id = _.color_size_id,
                    color_size_name = _.color_size_name,
                    item_dimension_number = _.item_dimension_number,
                    item_group_id = _.item_group_id,
                    item_group_name = _.item_group_name,
                    uom = _.uom
                });
            }
        }

        public void AutoCompleteChanged(object parameter)
        {
            IsVisibleListBoxItem = false;
            ItemGroupDimensionDTO item = (ItemGroupDimensionDTO)parameter;
            if (item != null)
            {
                item_id = item.item_id;
                item_name = item.item_name;
                brand_type_id = item.brand_type_id;
                brand_type_name = item.brand_type_name;
                color_size_id = item.color_size_id;
                color_size_name = item.color_size_name;
                ItemSelectKeyword = "";
            }
        }
    }
}
