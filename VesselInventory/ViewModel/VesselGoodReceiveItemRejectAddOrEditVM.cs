using VesselInventory.Models;
using VesselInventory.Repository;
using VesselInventory.Utility;

namespace VesselInventory.ViewModel
{
    public class VesselGoodReceiveItemRejectAddOrEditVM : ViewModelBase
    {
        public RelayCommand SaveCommand { get; private set; }
        private readonly IVesselGoodReceiveItemRejectRepository _vesselGoodReceiveItemRejectRepository;
        private readonly IRepository<Uom> _UOMRepository;
        public VesselGoodReceiveItemRejectAddOrEditVM(
            IVesselGoodReceiveItemRejectRepository vesselGoodReceiveItemRejectRepository,
            IRepository<Uom> UOMRepository)
        {
            _vesselGoodReceiveItemRejectRepository = vesselGoodReceiveItemRejectRepository;
            _UOMRepository = UOMRepository;
            InitializeCommands();
            SaveCommand = new RelayCommand(SaveAction);
        }

        private void SaveAction(object parameter)
        {
        }

        public void LoadDataGrid()
        {
            throw new System.NotImplementedException();
        }

        private void InitializeCommands()
        {
        }

    }
}
