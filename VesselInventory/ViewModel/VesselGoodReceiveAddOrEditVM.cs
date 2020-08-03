using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;
using VesselInventory.Commons;
using VesselInventory.Commons.Enums;
using VesselInventory.Commons.HelperFunctions;
using VesselInventory.Dto;
using VesselInventory.Models;
using VesselInventory.Repository;
using VesselInventory.Services;
using VesselInventory.Utility;
using VesselInventory.Views;
using Unity;

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
            VesselGoodReceiveId = vesselGoodReceiveId;
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
                            OfficeGoodIssuedNumber = textScanList[i];
                        if (i == 1)
                            ShipId = int.Parse(textScanList[i]);
                        if (i == 2)
                            ShipName = textScanList[i];
                        if (i == 3)
                            BargeId = int.Parse(textScanList[i]);
                        if (i == 4)
                            BargeName = textScanList[i];
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
            OfficeGoodIssuedNumber = "";
            ShipId = 0;
            ShipName = "";
            BargeId = 0;
            BargeName = "";
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
                    .GetGoodReceiveItemRejected(VesselGoodReceiveId);
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
        public int VesselGoodReceiveId
        {
            get => VesselGoodReceiveDataView.VesselGoodReceiveId;
            set => VesselGoodReceiveDataView.VesselGoodReceiveId = value;
        }
        public string VesselGoodReceiveNumber
        {
            get => VesselGoodReceiveDataView.VesselGoodReceiveNumber;
            set => VesselGoodReceiveDataView.VesselGoodReceiveNumber = value;
        }

        public DateTime? VesselGoodReceiveDate
        {
            get => VesselGoodReceiveDataView.VesselGoodReceiveDate;
            set
            {
                VesselGoodReceiveDataView.VesselGoodReceiveDate = DateTime.Parse(value.ToString());
                OnPropertyChanged("VesselGoodReceiveDate");
            }
        }

        public string OfficeGoodIssuedNumber
        {
            get => VesselGoodReceiveDataView.OfficeGoodIssuedNumber;
            set
            {
                VesselGoodReceiveDataView.OfficeGoodIssuedNumber = value;
                OnPropertyChanged("OfficeGoodIssuedNumber");
            }
        }

        public int ShipId
        {
            get => VesselGoodReceiveDataView.ShipId;
            set
            {
                VesselGoodReceiveDataView.ShipId = value;
                OnPropertyChanged("ShipId");
            }
        }

        public int BargeId
        {
            get => VesselGoodReceiveDataView.BargeId;
            set
            {
                VesselGoodReceiveDataView.BargeId = value;
                OnPropertyChanged("BargeId");
            }
        }

        public string ShipName
        {
            get => VesselGoodReceiveDataView.ShipName;
            set
            {
                VesselGoodReceiveDataView.ShipName = value;
                OnPropertyChanged("ShipName");
            }
        }


        public string BargeName
        {
            get => VesselGoodReceiveDataView.BargeName;
            set
            {
                VesselGoodReceiveDataView.BargeName = value;
                OnPropertyChanged("BargeName");
            }
        }
        #endregion

        /// <summary>
        /// Data Load Methods
        /// </summary>
        #region
        private void LoadAttributesValue()
        {
            if (RecordHelper.IsNewRecord(VesselGoodReceiveId))
            {
                IsItemEnabled = false;
                VesselGoodReceiveNumber = CommonDataHelper.GetSequenceNumber(2) + '-' + ShipBarge.ShipCode;
                VesselGoodReceiveDate = DateTime.Now;
            }
            else
            {
                VesselGoodReceiveDataView = _vesselGoodReceiveRepository.GetById(VesselGoodReceiveId);
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
                    (this, vesselGoodReceiveId: VesselGoodReceiveId);
            else
                vesselGoodReceiveItemRejectAddOrEditVM.InitializeData
                    (this, VesselGoodReceiveId,(int)parameter );

            _windowService.ShowDialogWindow<VesselGoodReceive_ItemRejectAddOrEditView>
                (vesselGoodReceiveItemRejectAddOrEditVM);
        }

        private void DeleteItemAction(object parameter)
        {
            MessageBoxResult confirmDialog = DialogHelper.DialogConfirmation(
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
                if (ShipId != ShipBarge.ShipId)
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
            if (RecordHelper.IsNewRecord(VesselGoodReceiveId))
            {
                VesselGoodReceiveDataView.CreatedBy = Auth.Instance.PersonName;
                VesselGoodReceiveDataView.CreatedDate = DateTime.Now;
                VesselGoodReceiveDataView.SyncStatus = SyncStatus.Not_Sync.GetDescription();
                VesselGoodReceiveDataView = _vesselGoodReceiveRepository.SaveTransaction(VesselGoodReceiveDataView);
            } 
            else
            {
                VesselGoodReceiveDataView.LastModifiedBy = Auth.Instance.PersonName;
                VesselGoodReceiveDataView.LastModifiedDate = DateTime.Now;
                VesselGoodReceiveDataView = _vesselGoodReceiveRepository.Update
                    (VesselGoodReceiveId,VesselGoodReceiveDataView);

            }
        }

        private bool IsSaveCanExecute(object parameter)
        {
            if (ShipId < 1 || BargeId < 1)
                return false;
            if (string.IsNullOrWhiteSpace(OfficeGoodIssuedNumber))
                return false;
            return true;
        }
        #endregion
    }
}
