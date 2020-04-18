using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using VesselInventory.Services;
using VesselInventory.Utility;

namespace VesselInventory.ViewModel
{
    public abstract class ViewModelBase : ObservableObject
    {
        /// <summary>
        /// Generic properties window
        /// </summary>
        #region
        private string _title = "Modal Dialog";
        private double _height = 300;
        private double _width = 300;
        public virtual string Title { get => _title; set => _title = value; }
        public virtual double Height { get => _height; set => _height = value; }
        public virtual double Width { get => _width; set => _width = value; }
        #endregion
        public RelayCommand<IClosable> CloseCommand { get; private set; }
        public RelayCommand<IClosable> LogoutCommand { get; private set; }
        public ViewModelBase()
        {
            CloseCommand = new RelayCommand<IClosable>(CloseWindow);
            LogoutCommand = new RelayCommand<IClosable>(CloseWindow);
        }
        protected void CloseWindow(IClosable window)
        {
            if (window != null)
                window.Close();
        }
    }
}
