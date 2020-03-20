using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using VesselInventory.Utility;

namespace VesselInventory.ViewModel
{
    public class RequestFormItemAddOrEditViewModel : ViewModelBase
    {

        public RelayCommand ListBoxChanged { get; private set; }
        public RequestFormItemAddOrEditViewModel()
        {
            IsVisibleListBoxItem = false;
            ListBoxChanged = new RelayCommand((parameter) => { MessageBox.Show("ok"); });
        }

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
    }
}
