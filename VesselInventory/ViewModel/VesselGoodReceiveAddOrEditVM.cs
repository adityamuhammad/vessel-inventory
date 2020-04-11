using System;
using ToastNotifications;
using ToastNotifications.Messages;
using VesselInventory.Commons.HelperFunctions;
using VesselInventory.DTO;
using VesselInventory.Models;
using VesselInventory.Repository;
using VesselInventory.Services;
using VesselInventory.Utility;

namespace VesselInventory.ViewModel
{
    class VesselGoodReceiveAddOrEditVM : ViewModelBase, IParentLoadable
    {
        VesselGoodReceive _vesselGoodReceive = new VesselGoodReceive();
        public RelayCommand SaveCommand { get; private set; }
        public RelayCommand AddOrEditItemRejectCommand { get; private set; }

        private Notifier _toasMessage = ToastNotification.Instance.GetInstance();

        private readonly IParentLoadable _parentLoadable;
        private readonly IVesselGoodReceiveRepository _vesselGoodReceiveRepository;

        public VesselGoodReceiveAddOrEditVM(IParentLoadable parentLoadable) : this(parentLoadable,0){}
        public VesselGoodReceiveAddOrEditVM(IParentLoadable parentLoadable  ,int vessel_good_receive_id)
        {
            InitializeCommands();
            _parentLoadable = parentLoadable;
            _vesselGoodReceiveRepository = new VesselGoodReceiveRepository();

            if(vessel_good_receive_id > 0)
            {
                _vesselGoodReceive = _vesselGoodReceiveRepository.GetById(vessel_good_receive_id);
                IsItemEnabled = true;
            } else
            {
                IsItemEnabled = false;
                this.vessel_good_receive_id = vessel_good_receive_id;
                vessel_good_receive_number = DataHelper.GetSequenceNumber(2) + '-' + ShipBarge.ship_code;
                vessel_good_receive_date = DateTime.Now;
            }
        }

        private void InitializeCommands()
        {
            SaveCommand = new RelayCommand(Save,IsSaveCanExecute);
        }

        private bool _IsItemEnabled;
        public bool IsItemEnabled
        {
            get => _IsItemEnabled;
            set
            {
                if  (_IsItemEnabled == value)
                    return;
                _IsItemEnabled = value;
                OnPropertyChanged("IsItemEnabled");
            }
        }


        private string _textScann = "";
        public string TextScann
        {
            get => _textScann;
            set
            {
                _textScann = value;
                Reset();
                string[] textScanList = _textScann.Split('|');
                for (int i = 0; i < textScanList.Length; i++)
                {
                    if (i == 0)
                        good_issued_number = textScanList[i];

                    if (i == 1)
                        ship_id = int.Parse(textScanList[i]);

                    if (i == 2)
                        ship_code = textScanList[i];

                    if (i == 3)
                        ship_name = textScanList[i];

                    if (i == 4)
                        barge_id = int.Parse(textScanList[i]);

                    if (i == 5)
                        barge_code = textScanList[i];

                    if (i == 6)
                        barge_name = textScanList[i];
                }
                _textScann = "";
                OnPropertyChanged("TextScann");
            }
        }
        public DateTime vessel_good_receive_date
        {
            get => _vesselGoodReceive.vessel_good_receive_date;
            set
            {
                _vesselGoodReceive.vessel_good_receive_date = DateTime.Parse(value.ToString());
                OnPropertyChanged("vessel_good_receive");
            }
        }

        public string vessel_good_receive_number
        {
            get => _vesselGoodReceive.vessel_good_receive_number;
            set => _vesselGoodReceive.vessel_good_receive_number = value;
        }

        private void Reset()
        {
            good_issued_number = "";
            ship_id = 0;
            ship_code = "";
            ship_name = "";
            barge_id = 0;
            barge_code = "";
            barge_name = "";
        }

        public int vessel_good_receive_id
        {
            get => _vesselGoodReceive.vessel_good_receive_id;
            set => _vesselGoodReceive.vessel_good_receive_id = value;
        }

        public string good_issued_number
        {
            get => _vesselGoodReceive.good_issued_number;
            set
            {
                _vesselGoodReceive.good_issued_number = value;
                OnPropertyChanged("good_issued_number");
            }
        }


        public int ship_id
        {
            get => _vesselGoodReceive.ship_id;
            set
            {
                _vesselGoodReceive.ship_id = value;
                OnPropertyChanged("ship_id");
            }
        }

        public int barge_id
        {
            get => _vesselGoodReceive.barge_id;
            set
            {
                _vesselGoodReceive.barge_id = value;
                OnPropertyChanged("barge_id");
            }
        }

        public string ship_code
        {
            get => _vesselGoodReceive.ship_code;
            set
            {
                _vesselGoodReceive.ship_code = value;
                OnPropertyChanged("ship_code");
            }
        }

        public string ship_name
        {
            get => _vesselGoodReceive.ship_name;
            set
            {
                _vesselGoodReceive.ship_name = value;
                OnPropertyChanged("ship_name");
            }
        }

        public string barge_code
        {
            get => _vesselGoodReceive.barge_code;
            set
            {
                _vesselGoodReceive.barge_code = value;
                OnPropertyChanged("barge_code");
            }
        }

        public string barge_name
        {
            get => _vesselGoodReceive.barge_name;
            set
            {
                _vesselGoodReceive.barge_name = value;
                OnPropertyChanged("barge_name");
            }
        }

        private ShipBargeDTO ShipBarge => DataHelper.GetShipBargeApairs();

        private void Save(object parameter)
        {
            try
            {
                if (ship_code != ShipBarge.ship_code)
                    throw new Exception("Cannot process, The Ship Code does not match.");

                if (vessel_good_receive_id == 0)
                    _vesselGoodReceive = _vesselGoodReceiveRepository.SaveVesselGoodReceive(_vesselGoodReceive);
                else
                    _vesselGoodReceive = _vesselGoodReceiveRepository.Update(vessel_good_receive_id,_vesselGoodReceive);

                IsItemEnabled = true;
                _toasMessage.ShowSuccess("Data saved successfully.");
                _parentLoadable.LoadDataGrid();

            } catch (Exception ex)
            {
                _toasMessage.ShowWarning(ex.Message);
            }
        }
        private bool IsSaveCanExecute(object parameter)
        {
            if (ship_id < 1 || barge_id < 1)
                return false;
            if (string.IsNullOrWhiteSpace(good_issued_number))
                return false;
            return true;
        }

        public void LoadDataGrid()
        {
            throw new System.NotImplementedException();
        }
    }
}
