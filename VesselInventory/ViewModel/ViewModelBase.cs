using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
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

        public RelayCommand OnOffNavigationBar { get; private set; }
        public ViewModelBase()
        {
            OnOffNavigationBar = new RelayCommand(OnOffNavigationBarAction);
        }
        /// <summary>
        /// Window specific behavior
        /// </summary>
        #region
        private double _navigationBarLength = 180;
        public double NavigationBarLength
        {
            get => _navigationBarLength;
            set
            {
                 _navigationBarLength = value;
                OnPropertyChanged("NavigationBarLength");
            }
        }
        private bool _isVisibleExpandButton = false;
        public bool IsVisibleExpandButton
        {
            get => _isVisibleExpandButton;
            set
            {
                if (_isVisibleExpandButton == value)
                    return;
                 _isVisibleExpandButton = value;
                OnPropertyChanged("IsVisibleExpandButton");
            }
        }

        private bool _isVisibleShortenedButton = true;
        public bool IsVisibleShortenedButton
        {
            get => _isVisibleShortenedButton;
            set
            {
                if (_isVisibleShortenedButton == value)
                    return;
                 _isVisibleShortenedButton = value;
                OnPropertyChanged("IsVisibleShortenedButton");
            }
        }

        private void OnOffNavigationBarAction(object parameter)
        {
            if(parameter != null)
            {
                if((string)parameter == "On")
                {
                    IsVisibleShortenedButton = false;
                    IsVisibleExpandButton = true;
                    NavigationBarLength = 45;
                } else
                {
                    IsVisibleShortenedButton = true;
                    IsVisibleExpandButton = false;
                    NavigationBarLength = 180;
                }
            }
        }

        #endregion
    }
}
