using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;
using Unity;
using VesselInventory.Commons;
using VesselInventory.Commons.HelperFunctions;
using VesselInventory.Dto;
using VesselInventory.Models;
using VesselInventory.Repository;
using VesselInventory.Services;
using VesselInventory.Utility;
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

        public VesselGoodReceiveAddOrEditVM(IWindowService windowService,
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
            LoadAttributesValue();
        }


        /// <summary>
        /// UI Props
        /// </summary>
        #region
        public override string Title => "Good Receive";
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
                    ResponseMessage.Error(GlobalNamespace.Error + ex.Message);
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
        /// Entity, And Collections
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

        private VesselGoodReceive VesselGoodReceiveDataView { get; set; }  = new VesselGoodReceive();
        private ShipBargeDto ShipBarge => CommonDataHelper.GetShipBargeApairs();
        #endregion

        /// <summary>
        /// Column Attributes
        /// </summary>
        #region
        public int vessel_good_receive_id
        {
            get => VesselGoodReceiveDataView.vessel_good_receive_id;
            set => VesselGoodReceiveDataView.vessel_good_receive_id = value;
        }
        public string vessel_good_receive_number
        {
            get => VesselGoodReceiveDataView.vessel_good_receive_number;
            set => VesselGoodReceiveDataView.vessel_good_receive_number = value;
        }

        public DateTime vessel_good_receive_date
        {
            get => VesselGoodReceiveDataView.vessel_good_receive_date;
            set
            {
                VesselGoodReceiveDataView.vessel_good_receive_date = DateTime.Parse(value.ToString());
                OnPropertyChanged("vessel_good_issued");
            }
        }

        public string good_issued_number
        {
            get => VesselGoodReceiveDataView.good_issued_number;
            set
            {
                VesselGoodReceiveDataView.good_issued_number = value;
                OnPropertyChanged("good_issued_number");
            }
        }

        public int ship_id
        {
            get => VesselGoodReceiveDataView.ship_id;
            set
            {
                VesselGoodReceiveDataView.ship_id = value;
                OnPropertyChanged("ship_id");
            }
        }

        public int barge_id
        {
            get => VesselGoodReceiveDataView.barge_id;
            set
            {
                VesselGoodReceiveDataView.barge_id = value;
                OnPropertyChanged("barge_id");
            }
        }

        public string ship_code
        {
            get => VesselGoodReceiveDataView.ship_code;
            set
            {
                VesselGoodReceiveDataView.ship_code = value;
                OnPropertyChanged("ship_code");
            }
        }

        public string ship_name
        {
            get => VesselGoodReceiveDataView.ship_name;
            set
            {
                VesselGoodReceiveDataView.ship_name = value;
                OnPropertyChanged("ship_name");
            }
        }

        public string barge_code
        {
            get => VesselGoodReceiveDataView.barge_code;
            set
            {
                VesselGoodReceiveDataView.barge_code = value;
                OnPropertyChanged("barge_code");
            }
        }

        public string barge_name
        {
            get => VesselGoodReceiveDataView.barge_name;
            set
            {
                VesselGoodReceiveDataView.barge_name = value;
                OnPropertyChanged("barge_name");
            }
        }
        #endregion

        /// <summary>
        /// Data Load Methods
        /// </summary>
        #region
        private void LoadAttributesValue()
        {
            if (RecordHelper.IsNewRecord(vessel_good_receive_id))
            {
                IsItemEnabled = false;
                vessel_good_receive_number = CommonDataHelper.GetSequenceNumber(2) + '-' + ShipBarge.ship_code;
                vessel_good_receive_date = DateTime.Now;
            }
            else
            {
                VesselGoodReceiveDataView = _vesselGoodReceiveRepository
                    .GetById(vessel_good_receive_id);
                IsItemEnabled = true;
                LoadDataGrid();
            }
        }

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

            _windowService.ShowDialogWindow<VesselGoodReceive_ItemRejectAddOrEditView>
                (vesselGoodReceiveItemRejectAddOrEditVM);
        }

        private void DeleteItemAction(object parameter)
        {
            MessageBoxResult confirmDialog = UIHelper.DialogConfirmation(
                GlobalNamespace.DeleteConfirmation, GlobalNamespace.DeleteConfirmationDescription );
            if (confirmDialog == MessageBoxResult.No)
                return;
            _vesselGoodReceiveItemRejectRepository.Delete((int)parameter);
            ResponseMessage.Success(GlobalNamespace.SuccessDelete);
            LoadDataGrid();
        }
        private void SaveAction(object parameter)
        {
            try
            {
                if (ship_code != ShipBarge.ship_code)
                    throw new Exception(GlobalNamespace.ShipDoesNotMatch);

                SaveOrUpdate();
                IsItemEnabled = true;
                ResponseMessage.Success(GlobalNamespace.SuccessSave);
                _parentLoadable.LoadDataGrid();
            } catch (Exception ex)
            {
                ResponseMessage.Error(GlobalNamespace.Error + ex.Message);
            }
        }
        private void SaveOrUpdate()
        {
            if (RecordHelper.IsNewRecord(vessel_good_receive_id))
                VesselGoodReceiveDataView = _vesselGoodReceiveRepository
                    .SaveTransaction(VesselGoodReceiveDataView);
            else
                VesselGoodReceiveDataView = _vesselGoodReceiveRepository
                    .Update(vessel_good_receive_id,VesselGoodReceiveDataView);
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
