﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Unity;
using VesselInventory.Commons;
using VesselInventory.Commons.HelperFunctions;
using VesselInventory.Dto;
using VesselInventory.Models;
using VesselInventory.Services;
using VesselInventory.Utility;

namespace VesselInventory.ViewModel
{
    class VesselGoodReturnAddOrEditVM : ViewModelBase, IParentLoadable
    {
        public RelayCommand SaveCommand { get; private set; }
        public RelayCommand AddOrEditItemCommand { get; private set; }
        public RelayCommand DeleteItemCommand { get; private set; }
        private IParentLoadable _parentLoadable;
        private readonly IWindowService _windowService;
        private readonly IUnityContainer UnityContainer = ((App)Application.Current).UnityContainer;
        public VesselGoodReturnAddOrEditVM(IWindowService windowService)
        {
            _windowService = windowService;
            InitializeCommands();
        }

        private void InitializeCommands()
        {
            SaveCommand = new RelayCommand(SaveAction,IsSaveCanExecute);
            AddOrEditItemCommand = new RelayCommand(AddOrEditItemAction);
            DeleteItemCommand = new RelayCommand(DeleteItemAction);
        }

        /// <summary>
        /// UI props
        /// </summary>
        #region
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
        public int vessel_good_return_id
        {
            get => VesselGoodReturnDataView.vessel_good_return_id;
            set => VesselGoodReturnDataView.vessel_good_return_id = value;
        }
        public string vessel_good_return_number
        {
            get => VesselGoodReturnDataView.vessel_good_return_number;
            set => VesselGoodReturnDataView.vessel_good_return_number = value;
        }

        public DateTime vessel_good_return_date
        {
            get
            {
                if (VesselGoodReturnDataView.vessel_good_return_date == default(DateTime) )
                    VesselGoodReturnDataView.vessel_good_return_date = DateTime.Now;
                return VesselGoodReturnDataView.vessel_good_return_date;
            }
            set
            {
                VesselGoodReturnDataView.vessel_good_return_date = DateTime.Parse(value.ToString());
                OnPropertyChanged("vessel_good_return_date");
            }
        }
        public int ship_id
        {
            get => VesselGoodReturnDataView.ship_id;
            set => VesselGoodReturnDataView.ship_id = value;
        }

        public string ship_name
        {
            get => VesselGoodReturnDataView.ship_name;
            set => VesselGoodReturnDataView.ship_name = value;
        }
        public string notes
        {
            get => VesselGoodReturnDataView.notes;
            set
            {
                VesselGoodReturnDataView.notes = value;
                OnPropertyChanged("notes");
            }
        }
        #endregion
        /// <summary>
        /// Entity, Collections and other attributes
        /// </summary>
        #region
        public ObservableCollection<VesselGoodReturnItem> GoodReturnItemCollections { get; set; } 
            = new ObservableCollection<VesselGoodReturnItem>();
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
            //if(RecordHelper.IsNewRecord(vessel_good_return_id))
            //{
            //    IsItemEnabled = false;
            //    vessel_good_return_number = CommonDataHelper.GetSequenceNumber(3) + '-' + ShipBarge.ship_code;
            //    ship_id = ShipBarge.ship_id;
            //    ship_name = ShipBarge.ship_name;
            //} else
            //{
            //    VesselGoodIssuedDataView = _vesselGoodIssuedRepository
            //        .GetById(vessel_good_issued_id);
            //    IsItemEnabled = true;
            //    LoadDataGrid();
            //}

        }
        public void LoadDataGrid()
        {
            //GoodIssuedItemCollections.Clear();
            //foreach (var item in _vesselGoodIssuedItemRepository
            //    .GetGoodIssuedItem(vessel_good_issued_id))
            //    GoodIssuedItemCollections.Add(item);
            //TotalItem = GoodIssuedItemCollections.Count;
        }
        #endregion
        /// <summary>
        /// Button Actions
        /// </summary>
        /// <param name="parameter"></param>
        #region
        private void AddOrEditItemAction(object parameter)
        {
            //var vesselGoodIssuedItemAddOrEditVM = UnityContainer.Resolve<VesselGoodIssuedItemAddOrEditVM>();
            //if (parameter is null)
            //    vesselGoodIssuedItemAddOrEditVM.InitializeData(this, vessel_good_issued_id);
            //else
            //    vesselGoodIssuedItemAddOrEditVM.InitializeData(this, vessel_good_issued_id, (int)parameter);
            //_windowService.ShowDialogWindow<VesselGoodIssued_ItemAddOrEditView>(vesselGoodIssuedItemAddOrEditVM);
        }

        private bool IsSaveCanExecute(object parameter)
        {
            return true;
        }

        private void DeleteItemAction(object parameter)
        {
            //MessageBoxResult confirmDialog = UIHelper.DialogConfirmation(
            //    GlobalNamespace.DeleteConfirmation, GlobalNamespace.DeleteConfirmationDescription );
            //if (confirmDialog == MessageBoxResult.No)
            //    return;
            //_vesselGoodIssuedItemRepository.DeleteTransaction((int)parameter);
            //ResponseMessage.Success(GlobalNamespace.SuccessDelete);
            //LoadDataGrid();
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
            //if (RecordHelper.IsNewRecord(vessel_good_issued_id))
            //{
            //    VesselGoodIssuedDataView = _vesselGoodIssuedRepository
            //        .SaveTransaction(VesselGoodIssuedDataView);

            //} else
            //{
            //    VesselGoodIssuedDataView.last_modified_by = Auth.Instance.personalname;
            //    VesselGoodIssuedDataView.last_modified_date = DateTime.Now;
            //    VesselGoodIssuedDataView = _vesselGoodIssuedRepository
            //        .Update(vessel_good_issued_id,
            //        VesselGoodIssuedDataView);
            //}
        }
        #endregion
    }
}