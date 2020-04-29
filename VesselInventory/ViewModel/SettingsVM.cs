﻿using System;
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
            barge_id = ShipInitialDataView.barge_id;
            ShipInitialDataView = _shipInitialRepository.GetById(ship_initial_id);
            ChangeBargeCommand = new RelayCommand(ChangeBarge);
        }

        private void ChangeBarge(object obj)
        {
            ShipInitialDataView = _shipInitialRepository.Update(ship_initial_id, ShipInitialDataView);
            ResponseMessage.Success(GlobalNamespace.SuccessSave);
        }

        private ShipBargeDto ShipBarge => CommonDataHelper.GetShipBargeApairs();
        public ShipInitial ShipInitialDataView { get; set; } = new ShipInitial();
        public int ship_initial_id => 1;
        public string ship_name => ShipBarge.ship_name;
        public int ship_id => ShipInitialDataView.ship_id;
        public int barge_id
        {
            get
            {
                return ShipInitialDataView.barge_id;
            }
            set
            {
                ShipInitialDataView.barge_id = value;
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
