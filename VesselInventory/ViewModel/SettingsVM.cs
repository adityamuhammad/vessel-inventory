using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VesselInventory.Commons;
using VesselInventory.Commons.HelperFunctions;
using VesselInventory.Dto;
using VesselInventory.Models;
using VesselInventory.Repository;
using VesselInventory.Utility;

namespace VesselInventory.ViewModel
{
    public class SettingsVM : ViewModelBase
    {
        public override string Title => "Settings";
        private readonly IGenericRepository<Ship> _shipRepository;
        private readonly IGenericRepository<ShipInitial> _shipInitialRepository;
        public RelayCommand ChangeBargeCommand { get; set; }
        public SettingsVM(
            IGenericRepository<Ship> shipRepository,
            IGenericRepository<ShipInitial> shipInitialRepository)
        {
            _shipRepository = shipRepository;
            _shipInitialRepository = shipInitialRepository;
            BargeId = ShipInitialDataView.BargeId;
            ShipInitialDataView = _shipInitialRepository.GetById(ShipInitialId);
            ChangeBargeCommand = new RelayCommand(ChangeBarge);
        }

        private void ChangeBarge(object obj)
        {
            ShipInitialDataView = _shipInitialRepository.Update(ShipInitialId, ShipInitialDataView);
            ResponseMessage.Success(GlobalNamespace.SuccessSave);
        }

        private ShipBargeDto ShipBarge => CommonDataHelper.GetShipBargeApairs();
        public ShipInitial ShipInitialDataView { get; set; } = new ShipInitial();
        public int ShipInitialId => 1;
        public string ShipName => ShipBarge.ShipName;
        public int ShipId => ShipInitialDataView.ShipId;
        public int BargeId
        {
            get
            {
                return ShipInitialDataView.BargeId;
            }
            set
            {
                ShipInitialDataView.BargeId = value;
                OnPropertyChanged("BargeId");
            }
        } 
        public IList<BargeDto> BargeCollection
        {
            get
            {
                var ship = _shipRepository.GetWhere( _ => _.IsBarge == true).ToList();
                var data = (from barge in ship
                            select new BargeDto {
                                    BargeId = barge.ShipId,
                                    BargeName = barge.ShipName});
                return data.ToList();
            }
        }
    }
}
