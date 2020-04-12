using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;
using VesselInventory.Commons;
using VesselInventory.Commons.HelperFunctions;
using VesselInventory.DTO;
using VesselInventory.Models;
using VesselInventory.Repository;
using VesselInventory.Services;
using VesselInventory.Utility;
using Unity;
using VesselInventory.Views;

namespace VesselInventory.ViewModel
{
    class VesselGoodReceiveAddOrEditVM : ViewModelBase, IParentLoadable
    {
        public RelayCommand SaveCommand { get; private set; }
        public RelayCommand AddOrEditItemCommand { get; private set; }
        public RelayCommand DeleteItemCommand { get; private set; }

        private IParentLoadable _parentLoadable;
        private readonly IVesselGoodReceiveRepository _vesselGoodReceiveRepository;
        private readonly IVesselGoodReceiveItemRejectRepository _vesselGoodReceiveItemRejectRepository;
        private readonly IWindowService _windowService;

        public VesselGoodReceiveAddOrEditVM(
            IWindowService windowService,
            IVesselGoodReceiveRepository vesselGoodReceiveRepository,
            IVesselGoodReceiveItemRejectRepository vesselGoodReceiveItemRejectRepository)
        {
            InitializeCommands();
            _vesselGoodReceiveRepository = vesselGoodReceiveRepository;
            _vesselGoodReceiveItemRejectRepository = vesselGoodReceiveItemRejectRepository;
            _windowService = windowService;
        }

        private void InitializeCommands()
        {
            SaveCommand = new RelayCommand(SaveAction,IsSaveCanExecute);
            AddOrEditItemCommand = new RelayCommand(AddOrEditItemAction);
            DeleteItemCommand = new RelayCommand(DeleteItemAction);
        }

        public void InitializeData(IParentLoadable parentLoadable, int vesselGoodReceiveId = 0)
        {
            vessel_good_receive_id = vesselGoodReceiveId;
            _parentLoadable = parentLoadable;
            if(!IsNewRecord)
            {
                VesselGoodReceiveEntity = _vesselGoodReceiveRepository
                    .GetById(vessel_good_receive_id);
                IsItemEnabled = true;
                LoadDataGrid();
            } else
            {
                IsItemEnabled = false;
                vessel_good_receive_number = DataHelper.GetSequenceNumber(2) + '-' + ShipBarge.ship_code;
                vessel_good_receive_date = DateTime.Now;
            }
        }

        /// <summary>
        /// UI Props
        /// </summary>
        #region
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
                try
                {
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

                } catch (Exception ex)
                {
                    ResponseMessage.Error("The format is invalid, details :" + ex.Message);
                }
                _textScann = "";
                OnPropertyChanged("TextScann");
            }
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

        #endregion

        /// <summary>
        /// Columns, Entity, And Collections
        /// </summary>
        #region
        public ObservableCollection<VesselGoodReceiveItemReject> GoodReceiveItemRejectCollection { get; set; } 
            = new ObservableCollection<VesselGoodReceiveItemReject>();
        private IEnumerable<VesselGoodReceiveItemReject> GoodReceiveItemRejectList
        {
            get
            {
                return _vesselGoodReceiveItemRejectRepository
                    .GetGoodReceiveItemRejected(vessel_good_receive_id);
            }
        }
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

        private bool IsNewRecord => (vessel_good_receive_id == 0);
        private VesselGoodReceive VesselGoodReceiveEntity { get; set; }  = new VesselGoodReceive();
        private ShipBargeDTO ShipBarge => DataHelper.GetShipBargeApairs();
        #endregion

        /// <summary>
        /// Column Attributes
        /// </summary>
        #region
        public DateTime vessel_good_receive_date
        {
            get => VesselGoodReceiveEntity.vessel_good_receive_date;
            set
            {
                VesselGoodReceiveEntity.vessel_good_receive_date = DateTime.Parse(value.ToString());
                OnPropertyChanged("vessel_good_receive");
            }
        }

        public string vessel_good_receive_number
        {
            get => VesselGoodReceiveEntity.vessel_good_receive_number;
            set => VesselGoodReceiveEntity.vessel_good_receive_number = value;
        }

        public int vessel_good_receive_id
        {
            get => VesselGoodReceiveEntity.vessel_good_receive_id;
            set => VesselGoodReceiveEntity.vessel_good_receive_id = value;
        }

        public string good_issued_number
        {
            get => VesselGoodReceiveEntity.good_issued_number;
            set
            {
                VesselGoodReceiveEntity.good_issued_number = value;
                OnPropertyChanged("good_issued_number");
            }
        }

        public int ship_id
        {
            get => VesselGoodReceiveEntity.ship_id;
            set
            {
                VesselGoodReceiveEntity.ship_id = value;
                OnPropertyChanged("ship_id");
            }
        }

        public int barge_id
        {
            get => VesselGoodReceiveEntity.barge_id;
            set
            {
                VesselGoodReceiveEntity.barge_id = value;
                OnPropertyChanged("barge_id");
            }
        }

        public string ship_code
        {
            get => VesselGoodReceiveEntity.ship_code;
            set
            {
                VesselGoodReceiveEntity.ship_code = value;
                OnPropertyChanged("ship_code");
            }
        }

        public string ship_name
        {
            get => VesselGoodReceiveEntity.ship_name;
            set
            {
                VesselGoodReceiveEntity.ship_name = value;
                OnPropertyChanged("ship_name");
            }
        }

        public string barge_code
        {
            get => VesselGoodReceiveEntity.barge_code;
            set
            {
                VesselGoodReceiveEntity.barge_code = value;
                OnPropertyChanged("barge_code");
            }
        }

        public string barge_name
        {
            get => VesselGoodReceiveEntity.barge_name;
            set
            {
                VesselGoodReceiveEntity.barge_name = value;
                OnPropertyChanged("barge_name");
            }
        }
        #endregion

        /// <summary>
        /// Data Load Methods
        /// </summary>
        #region
        public void LoadDataGrid()
        {
            GoodReceiveItemRejectCollection.Clear();
            foreach (var itemReject in GoodReceiveItemRejectList)
                GoodReceiveItemRejectCollection.Add(itemReject);
            TotalItem = GoodReceiveItemRejectCollection.Count;
        }
        #endregion

        /// <summary>
        /// Button behavior
        /// </summary>
        /// <param name="parameter"></param>
        #region
        private void AddOrEditItemAction(object parameter)
        {
            var container = ((App)Application.Current).UnityContainer;
            var vesselGoodReceiveItemRejectAddOrEditVM = container.Resolve<VesselGoodReceiveItemRejectAddOrEditVM>();
            if(parameter is null)
                vesselGoodReceiveItemRejectAddOrEditVM.InitializeData
                    (this, vesselGoodReceiveId: vessel_good_receive_id);
            else
                vesselGoodReceiveItemRejectAddOrEditVM.InitializeData
                    (this, vessel_good_receive_id,(int)parameter );

            _windowService.ShowWindow<VesselGoodReceive_ItemRejectAddOrEditView>(vesselGoodReceiveItemRejectAddOrEditVM);
        }

        private void DeleteItemAction(object parameter)
        {
            MessageBoxResult confirmDialog = UIHelper.DialogConfirmation("Delete Confirmation","Are you sure?" );
            if (confirmDialog == MessageBoxResult.No)
                return;
            _vesselGoodReceiveItemRejectRepository.Delete((int)parameter);
            ResponseMessage.Success("Data deleted successfully.");
            LoadDataGrid();
        }

        private void SaveAction(object parameter)
        {
            try
            {
                if (ship_code != ShipBarge.ship_code)
                    throw new Exception("Cannot process, The Ship Code does not match.");

                if (IsNewRecord)
                    VesselGoodReceiveEntity = _vesselGoodReceiveRepository
                        .SaveVesselGoodReceive(VesselGoodReceiveEntity);
                else
                    VesselGoodReceiveEntity = _vesselGoodReceiveRepository
                        .Update(vessel_good_receive_id,VesselGoodReceiveEntity);

                IsItemEnabled = true;
                ResponseMessage.Success("Data saved successfully.");
                _parentLoadable.LoadDataGrid();
            } catch (Exception ex)
            {
                ResponseMessage.Error("Error  : " + ex.Message);
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
        #endregion
    }
}
