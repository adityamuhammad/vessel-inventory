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
    class VesselGoodReturnAddOrEditVM : ViewModelBase, IDataGrid
    {
        public ObservableCollection<VesselGoodReturnItem> GoodReturnItemCollections { get; set; } 
            = new ObservableCollection<VesselGoodReturnItem>();
        public RelayCommand SaveCommand { get; private set; }
        public RelayCommand AddOrEditItemCommand { get; private set; }
        public RelayCommand DeleteItemCommand { get; private set; }
        private IDataGrid _parentLoadable;
        private readonly IWindowService _windowService;
        private readonly IUnityContainer UnityContainer = ((App)Application.Current).UnityContainer;
        private readonly IVesselGoodReturnRepository _vesselGoodReturnRepository;
        private readonly IVesselGoodReturnItemRepository _vesselGoodReturnItemRepository;
        public VesselGoodReturnAddOrEditVM( IWindowService windowService,
            IVesselGoodReturnRepository vesselGoodReturnRepository,
            IVesselGoodReturnItemRepository vesselGoodReturnItemRepository)
        {
            _windowService = windowService;
            _vesselGoodReturnRepository = vesselGoodReturnRepository;
            _vesselGoodReturnItemRepository = vesselGoodReturnItemRepository;
            InitializeCommands();
        }

        private void InitializeCommands()
        {
            SaveCommand = new RelayCommand(SaveAction);
            AddOrEditItemCommand = new RelayCommand(AddOrEditItemAction, IsAddOrEditItemCanExecute);
            DeleteItemCommand = new RelayCommand(DeleteItemAction, IsDeleteItemCanExecute);
        }

        public void InitializeData(IDataGrid parentLoadable, int vesselGoodReturnId = 0)
        {
            _parentLoadable = parentLoadable;
            VesselGoodReturnId = vesselGoodReturnId;
            LoadAttributes();
        }

        /// <summary>
        /// UI props
        /// </summary>
        #region

        private bool _isCanModify;
        public bool IsCanModify
        {
            get => _isCanModify;
            set
            {
                if  (_isCanModify == value) return; _isCanModify = value;

                OnPropertyChanged("IsCanModify");
            }
        }
        public override string Title => "Good Return";
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
        #endregion

        /// <summary>
        /// Columns
        /// </summary>
        #region
        public int VesselGoodReturnId
        {
            get => VesselGoodReturnDataView.VesselGoodReturnId;
            set => VesselGoodReturnDataView.VesselGoodReturnId = value;
        }
        public string VesselGoodReturnNumber
        {
            get => VesselGoodReturnDataView.VesselGoodReturnNumber;
            set => VesselGoodReturnDataView.VesselGoodReturnNumber = value;
        }

        public DateTime VesselGoodReturnDate
        {
            get
            {
                if (VesselGoodReturnDataView.VesselGoodReturnDate == default(DateTime) )
                    VesselGoodReturnDataView.VesselGoodReturnDate = DateTime.Now;
                return VesselGoodReturnDataView.VesselGoodReturnDate;
            }
            set
            {
                VesselGoodReturnDataView.VesselGoodReturnDate = DateTime.Parse(value.ToString());
                OnPropertyChanged("VesselGoodReturnDate");
            }
        }
        public int ShipId
        {
            get => VesselGoodReturnDataView.ShipId;
            set => VesselGoodReturnDataView.ShipId = value;
        }

        public string ShipName
        {
            get => VesselGoodReturnDataView.ShipName;
            set => VesselGoodReturnDataView.ShipName = value;
        }
        public string Notes
        {
            get => VesselGoodReturnDataView.Notes;
            set
            {
                VesselGoodReturnDataView.Notes = value;
                OnPropertyChanged("Notes");
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
        private VesselGoodReturn VesselGoodReturnDataView { get; set; }  = new VesselGoodReturn();
        private ShipBargeDto ShipBarge => CommonDataHelper.GetShipBargeApairs();
        #endregion

        /// <summary>
        /// Load methods
        /// </summary>
        #region

        private void LoadAttributes()
        {
            if (RecordHelper.IsNewRecord(VesselGoodReturnId))
            {
                IsItemEnabled = false;
                VesselGoodReturnNumber = CommonDataHelper.GetSequenceNumber(4) + '-' + ShipBarge.ShipCode;
                ShipId = ShipBarge.ShipId;
                ShipName = ShipBarge.ShipName;
                IsCanModify = true;
            }
            else
            {
                IsItemEnabled = true;
                VesselGoodReturnDataView = _vesselGoodReturnRepository.GetById(VesselGoodReturnId);
                if (VesselGoodReturnDataView.SyncStatus.Trim() != SyncStatus.Not_Sync.GetDescription())
                {
                    IsCanModify = false;
                } else
                {
                    IsCanModify = true;
                }
                LoadDataGrid();
            }

        }
        public void LoadDataGrid()
        {
            GoodReturnItemCollections.Clear();
            foreach (var item in _vesselGoodReturnItemRepository.GetGoodReturnItem(VesselGoodReturnId))
                GoodReturnItemCollections.Add(item);
            TotalItem = GoodReturnItemCollections.Count;
        }
        #endregion
        /// <summary>
        /// Button Actions
        /// </summary>
        /// <param name="parameter"></param>
        #region
        private void AddOrEditItemAction(object parameter)
        {
            var vesselGoodReturnItemAddOrEditVM = UnityContainer.Resolve<VesselGoodReturnItemAddOrEditVM>();

            if (parameter is null)
                vesselGoodReturnItemAddOrEditVM.InitializeData(this, VesselGoodReturnId);
            else
                vesselGoodReturnItemAddOrEditVM.InitializeData(this, VesselGoodReturnId, (int)parameter);

            _windowService.ShowDialogWindow<VesselGoodReturn_ItemAddOrEditView>(vesselGoodReturnItemAddOrEditVM);
        }

        private void DeleteItemAction(object parameter)
        {
            MessageBoxResult confirmDialog = DialogHelper.DialogConfirmation(
                GlobalNamespace.DeleteConfirmation, GlobalNamespace.DeleteConfirmationDescription);

            if (confirmDialog == MessageBoxResult.No) return;

            _vesselGoodReturnItemRepository.DeleteTransaction((int)parameter);

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
            }
            catch (Exception)
            {
                ResponseMessage.Error(string.Format("{0}", GlobalNamespace.ErrorSave));
            }
        }

        private void SaveOrUpdate()
        {
            if (RecordHelper.IsNewRecord(VesselGoodReturnId)) Save();
            else Update();
        }

        private void Update()
        {
            VesselGoodReturnDataView.LastModifiedBy = Auth.Instance.PersonName;
            VesselGoodReturnDataView.LastModifiedDate = DateTime.Now;
            VesselGoodReturnDataView = _vesselGoodReturnRepository
                .Update(VesselGoodReturnId,
                VesselGoodReturnDataView);
        }

        private void Save()
        {
            VesselGoodReturnDataView.CreatedBy = Auth.Instance.PersonName;
            VesselGoodReturnDataView.CreatedDate = DateTime.Now;
            VesselGoodReturnDataView.SyncStatus = SyncStatus.Not_Sync.GetDescription();
            VesselGoodReturnDataView = _vesselGoodReturnRepository
                .SaveTransaction(VesselGoodReturnDataView);
        }
        private bool IsAddOrEditItemCanExecute(object parameter)
        {
            return IsCanModify;
        }
        private bool IsDeleteItemCanExecute(object parameter)
        {
            return IsCanModify;
        }
        #endregion
    }
}
