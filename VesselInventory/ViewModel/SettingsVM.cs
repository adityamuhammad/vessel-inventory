using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VesselInventory.Commons.HelperFunctions;
using VesselInventory.Dto;
using VesselInventory.Models;
using VesselInventory.Repository;

namespace VesselInventory.ViewModel
{
    public class SettingsVM : ViewModelBase
    {
        public override string Title => "Settings";
        private readonly IGenericRepository<Ship> _shipRepository;
        public SettingsVM(IGenericRepository<Ship> shipRepository)
        {
            _shipRepository = shipRepository;
            barge_id = ShipBarge.barge_id;
        }

        private ShipBargeDto ShipBarge => CommonDataHelper.GetShipBargeApairs();
        public string ship_name => ShipBarge.ship_name;
        public int ship_id => ShipBarge.ship_id;
        private int _barge_id;
        public int barge_id
        {
            get
            {
                return _barge_id;
            }
            set
            {
                _barge_id = value;
                OnPropertyChanged("barge_id");
            }
        } 
        public IList<BargeDto> BargeCollection
        {
            get
            {
                var ship = _shipRepository.GetWhere(_ => _.is_barge == true).ToList();
                var data = (from barge in ship
                            select new BargeDto {
                                    barge_id = barge.ship_id,
                                    barge_name = barge.ship_name});
                return data.ToList();
            }
        }
    }
}
