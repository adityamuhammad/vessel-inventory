using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
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
    class VesselGoodIssuedAddOrEditVM : ViewModelBase, IParentLoadable
    {
        public RelayCommand SaveCommand { get; private set; }
        public RelayCommand AddOrEditItemCommand { get; private set; }
        public RelayCommand DeleteItemCommand { get; private set; }
        private IParentLoadable _parentLoadable;
        private readonly IWindowService _windowService;
        private readonly IVesselGoodIssuedRepository _vesselGoodIssuedRepository;
        private readonly IVesselGoodIssuedItemRepository _vesselGoodIssuedItemRepository; 
        private readonly IUnityContainer container = ((App)Application.Current).UnityContainer;
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
            SaveCommand = new RelayCommand(SaveAction,IsSaveCanExecute);
            AddOrEditItemCommand = new RelayCommand(AddOrEditItemAction);
            DeleteItemCommand = new RelayCommand(DeleteItemAction);
        }

        public void InitializeData(IParentLoadable parentLoadable, int vesselGoodIssuedId = 0)
        {
            _parentLoadable = parentLoadable;
            vessel_good_issued_id = vesselGoodIssuedId;
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
        public int vessel_good_issued_id
        {
            get => VesselGoodIssuedDataView.vessel_good_issued_id;
            set => VesselGoodIssuedDataView.vessel_good_issued_id = value;
        }
        public string vessel_good_issued_number
        {
            get => VesselGoodIssuedDataView.vessel_good_issued_number;
            set => VesselGoodIssuedDataView.vessel_good_issued_number = value;
        }

        public DateTime vessel_good_issued_date
        {
            get => VesselGoodIssuedDataView.vessel_good_issued_date;
            set
            {
                VesselGoodIssuedDataView.vessel_good_issued_date = DateTime.Parse(value.ToString());
                OnPropertyChanged("vessel_good_issued_date");
            }
        }
        public int ship_id
        {
            get => VesselGoodIssuedDataView.ship_id;
            set => VesselGoodIssuedDataView.ship_id = value;
        }

        public string ship_name
        {
            get => VesselGoodIssuedDataView.ship_name;
            set => VesselGoodIssuedDataView.ship_name = value;
        }
        public string notes
        {
            get => VesselGoodIssuedDataView.notes;
            set
            {
                VesselGoodIssuedDataView.notes = value;
                OnPropertyChanged("notes");
            }
        }
        #endregion

        /// <summary>
        /// Entity, Collections and other attributes
        /// </summary>
        #region
        public ObservableCollection<VesselGoodIssuedItem> GoodIssuedItemCollections { get; set; } 
            = new ObservableCollection<VesselGoodIssuedItem>();
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
            if(RecordHelper.IsNewRecord(vessel_good_issued_id))
            {
                IsItemEnabled = false;
                vessel_good_issued_number = CommonDataHelper.GetSequenceNumber(3) + '-' + ShipBarge.ship_code;
                vessel_good_issued_date = DateTime.Now;
                ship_id = ShipBarge.ship_id;
                ship_name = ShipBarge.ship_name;
            } else
            {
                VesselGoodIssuedDataView = _vesselGoodIssuedRepository
                    .GetById(vessel_good_issued_id);
                IsItemEnabled = true;
                LoadDataGrid();
            }

        }
        public void LoadDataGrid()
        {
            GoodIssuedItemCollections.Clear();
            foreach (var item in _vesselGoodIssuedItemRepository
                .GetGoodIssuedItem(vessel_good_issued_id))
                GoodIssuedItemCollections.Add(item);
            TotalItem = GoodIssuedItemCollections.Count;
        }
        #endregion

        /// <summary>
        /// Button Actions
        /// </summary>
        /// <param name="parameter"></param>
        #region
        private void DeleteItemAction(object parameter)
        {
            throw new NotImplementedException();
        }

        private void AddOrEditItemAction(object parameter)
        {
            var vesselGoodIssuedItemAddOrEditVM = container.Resolve<VesselGoodIssuedItemAddOrEditVM>();
            if (parameter is null)
                vesselGoodIssuedItemAddOrEditVM.InitializeData(this, vessel_good_issued_id);
            else
                vesselGoodIssuedItemAddOrEditVM.InitializeData(this, vessel_good_issued_id, (int)parameter);
            _windowService.ShowDialogWindow<VesselGoodIssued_ItemAddOrEditView>(vesselGoodIssuedItemAddOrEditVM);
        }

        private bool IsSaveCanExecute(object parameter)
        {
            return true;
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
                ResponseMessage.Error(GlobalNamespace.ErrorSave + ' ' + ex.Message);
            }
        }

        private void SaveOrUpdate()
        {
            if (RecordHelper.IsNewRecord(vessel_good_issued_id))
            {
                VesselGoodIssuedDataView = _vesselGoodIssuedRepository
                    .SaveTransaction(VesselGoodIssuedDataView);

            } else
            {
                VesselGoodIssuedDataView.last_modified_by = Auth.Instance.personalname;
                VesselGoodIssuedDataView.last_modified_date = DateTime.Now;
                VesselGoodIssuedDataView = _vesselGoodIssuedRepository
                    .Update(vessel_good_issued_id,
                    VesselGoodIssuedDataView);
            }
                        }
        #endregion
    }
}
