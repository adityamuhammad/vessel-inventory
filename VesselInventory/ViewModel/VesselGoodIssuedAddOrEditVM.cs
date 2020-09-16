using System;
using System.Collections.ObjectModel;
using System.Windows;
using Unity;
using VesselInventory.Commons;
using VesselInventory.Commons.Enums;
using VesselInventory.Commons.HelperFunctions;
using VesselInventory.Dto;
using VesselInventory.Models;
using VesselInventory.Repository;
using VesselInventory.Services;
using VesselInventory.Utility;
using VesselInventory.Views;

namespace VesselInventory.ViewModel
{
    class VesselGoodIssuedAddOrEditVM : ViewModelBase, IDataGrid
    {
        public ObservableCollection<VesselGoodIssuedItem> GoodIssuedItemCollections { get; set; } 
            = new ObservableCollection<VesselGoodIssuedItem>();
        public RelayCommand SaveCommand { get; private set; }
        public RelayCommand AddOrEditItemCommand { get; private set; }
        public RelayCommand DeleteItemCommand { get; private set; }
        private IDataGrid _parentLoadable;
        private readonly IWindowService _windowService;
        private readonly IVesselGoodIssuedRepository _vesselGoodIssuedRepository;
        private readonly IVesselGoodIssuedItemRepository _vesselGoodIssuedItemRepository; 
        private readonly IUnityContainer UnityContainer = ((App)Application.Current).UnityContainer;
        public VesselGoodIssuedAddOrEditVM(IWindowService windowService, 
            IVesselGoodIssuedRepository vesselGoodIssuedRepository,
            IVesselGoodIssuedItemRepository vesselGoodIssuedItemRepository)
        {
            InitializeCommands();
            _vesselGoodIssuedRepository = vesselGoodIssuedRepository;
            _vesselGoodIssuedItemRepository = vesselGoodIssuedItemRepository;
            _windowService = windowService;
        }

        private void InitializeCommands()
        {
            SaveCommand = new RelayCommand(SaveAction);
            AddOrEditItemCommand = new RelayCommand(AddOrEditItemAction);
            DeleteItemCommand = new RelayCommand(DeleteItemAction);
        }

        public void InitializeData(IDataGrid parentLoadable, int vesselGoodIssuedId = 0)
        {
            _parentLoadable = parentLoadable;
            VesselGoodIssuedId = vesselGoodIssuedId;
            LoadAttributes();
        }

        /// <summary>
        /// UI props
        /// </summary>
        #region
        public override string Title => "Good Issued";
        private bool _IsItemEnabled;
        public bool IsItemEnabled
        {
            get => _IsItemEnabled;
            set
            {
                if  (_IsItemEnabled == value) return;
                _IsItemEnabled = value;
                OnPropertyChanged("IsItemEnabled");
            }
        }
        #endregion

        /// <summary>
        /// Columns
        /// </summary>
        #region
        public int VesselGoodIssuedId
        {
            get => VesselGoodIssuedDataView.VesselGoodIssuedId;
            set => VesselGoodIssuedDataView.VesselGoodIssuedId = value;
        }
        public string VesselGoodIssuedNumber
        {
            get => VesselGoodIssuedDataView.VesselGoodIssuedNumber;
            set => VesselGoodIssuedDataView.VesselGoodIssuedNumber = value;
        }

        public DateTime VesselGoodIssuedDate
        {
            get
            {
                if (VesselGoodIssuedDataView.VesselGoodIssuedDate == default(DateTime) )
                    VesselGoodIssuedDataView.VesselGoodIssuedDate = DateTime.Now;
                return VesselGoodIssuedDataView.VesselGoodIssuedDate;
            }
            set
            {
                VesselGoodIssuedDataView.VesselGoodIssuedDate = DateTime.Parse(value.ToString());
                OnPropertyChanged("VesselGoodIssuedDate");
            }
        }
        public int ShipId
        {
            get => VesselGoodIssuedDataView.ShipId;
            set => VesselGoodIssuedDataView.ShipId = value;
        }

        public string ShipName
        {
            get => VesselGoodIssuedDataView.ShipName;
            set => VesselGoodIssuedDataView.ShipName = value;
        }
        public string Notes
        {
            get => VesselGoodIssuedDataView.Notes;
            set
            {
                VesselGoodIssuedDataView.Notes = value;
                OnPropertyChanged("notes");
            }
        }
        #endregion

        /// <summary>
        /// Entity, Collections and other attributes
        /// </summary>
        #region
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
        private VesselGoodIssued VesselGoodIssuedDataView { get; set; }  = new VesselGoodIssued();
        private ShipBargeDto ShipBarge => CommonDataHelper.GetShipBargeApairs();
        #endregion

        /// <summary>
        /// Load methods
        /// </summary>
        #region
        private void LoadAttributes()
        {
            if(RecordHelper.IsNewRecord(VesselGoodIssuedId))
            {
                IsItemEnabled = false;
                VesselGoodIssuedNumber = CommonDataHelper.GetSequenceNumber(3) + '-' + ShipBarge.ShipCode;
                VesselGoodIssuedDate = DateTime.Now;
                ShipId = ShipBarge.ShipId;
                ShipName = ShipBarge.ShipName;
            } else
            {
                VesselGoodIssuedDataView = _vesselGoodIssuedRepository.GetById(VesselGoodIssuedId);
                IsItemEnabled = true;
                LoadDataGrid();
            }

        }
        public void LoadDataGrid()
        {
            GoodIssuedItemCollections.Clear();
            foreach (var item in _vesselGoodIssuedItemRepository.GetGoodIssuedItem(VesselGoodIssuedId))
                GoodIssuedItemCollections.Add(item);
            TotalItem = GoodIssuedItemCollections.Count;
        }
        #endregion

        /// <summary>
        /// Button Actions
        /// </summary>
        /// <param name="parameter"></param>
        #region
        private void AddOrEditItemAction(object parameter)
        {
            var vesselGoodIssuedItemAddOrEditVM = UnityContainer.Resolve<VesselGoodIssuedItemAddOrEditVM>();

            if (parameter is null)
                vesselGoodIssuedItemAddOrEditVM
                    .InitializeData(this, VesselGoodIssuedId);
            else
                vesselGoodIssuedItemAddOrEditVM
                    .InitializeData(this, VesselGoodIssuedId, (int)parameter);

            _windowService.ShowDialogWindow
                <VesselGoodIssued_ItemAddOrEditView>
                    (vesselGoodIssuedItemAddOrEditVM);
        }

        private void DeleteItemAction(object parameter)
        {
            MessageBoxResult confirmDialog = DialogHelper.DialogConfirmation(
                GlobalNamespace.DeleteConfirmation, GlobalNamespace.DeleteConfirmationDescription );
            if (confirmDialog == MessageBoxResult.No) return;

            _vesselGoodIssuedItemRepository.DeleteTransaction((int)parameter);

            ResponseMessage.Success(GlobalNamespace.SuccessDelete);

            LoadDataGrid();
        }

        private void SaveAction(object parameter)
        {
            try
            {
                SaveOrUpdate();
                IsItemEnabled = true;
                _parentLoadable.LoadDataGrid();
                ResponseMessage.Success(GlobalNamespace.SuccessSave);
            } catch (Exception ex) {
                ResponseMessage.Error(string.Format("{0} {1}", GlobalNamespace.ErrorSave, ex.Message));
            }
        }

        private void SaveOrUpdate()
        {
            if (RecordHelper.IsNewRecord(VesselGoodIssuedId)) Save();
            else Update();
        }
        private void Save()
        {
            VesselGoodIssuedDataView.CreatedDate = DateTime.Now;
            VesselGoodIssuedDataView.CreatedBy = Auth.Instance.PersonName;
            VesselGoodIssuedDataView.SyncStatus = SyncStatus.Not_Sync.GetDescription();
            VesselGoodIssuedDataView = _vesselGoodIssuedRepository.SaveTransaction(VesselGoodIssuedDataView);
        }

        private void Update()
        {
            VesselGoodIssuedDataView.LastModifiedBy = Auth.Instance.PersonName;
            VesselGoodIssuedDataView.LastModifiedDate = DateTime.Now;
            VesselGoodIssuedDataView = _vesselGoodIssuedRepository.Update(VesselGoodIssuedId, VesselGoodIssuedDataView);
        }
        #endregion
    }
}
