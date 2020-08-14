using System.Collections.ObjectModel;
using VesselInventory.Models;
using VesselInventory.Repository;

namespace VesselInventory.ViewModel
{
    class VesselGoodReceiveItemVM : ViewModelBase
    {
        public ObservableCollection<VesselGoodReceiveItem> GoodReceiveItemCollection { get; set; } 
            = new ObservableCollection<VesselGoodReceiveItem>();

        private readonly IVesselGoodReceiveItemRepository _vesselGoodReceiveItemRepository;

        public VesselGoodReceiveItemVM(IVesselGoodReceiveItemRepository vesselGoodReceiveItemRepository)
        {
            _vesselGoodReceiveItemRepository = vesselGoodReceiveItemRepository;
        }
        public void InitializeData(int vesselGoodReceiveId)
        {
            LoadDataGrid(vesselGoodReceiveId);
        }

        public override string Title => "Good Receive Item Detail";
        private int _totalItem = 0;
        public int TotalItem
        {
            get => _totalItem;
            set
            {
                _totalItem = value;
                OnPropertyChanged("TotalItem");
            }
        }

        private void LoadDataGrid(int vesselGoodReceiveId)
        {
            GoodReceiveItemCollection.Clear();
            foreach(var item in _vesselGoodReceiveItemRepository.GetGoodReceiveItem(vesselGoodReceiveId))
                GoodReceiveItemCollection.Add(item);
            TotalItem = GoodReceiveItemCollection.Count;
        }
    }
}
