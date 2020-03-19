using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VesselInventory.Utility;

namespace VesselInventory.ViewModel
{
    public abstract class ViewModelBase : ObservableObject
    {
        private string _title = "Modal Dialog";
        private double _height = 300;
        private double _width = 300;
        public virtual string Title { get => _title; set => _title = value; }
        public virtual double Height { get => _height; set => _height = value; }
        public virtual double Width { get => _width; set => _width = value; }
    }
}
