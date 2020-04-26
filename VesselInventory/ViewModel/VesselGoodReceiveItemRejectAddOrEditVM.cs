using System;
using System.Collections.Generic;
using VesselInventory.Commons;
using VesselInventory.Models;
using VesselInventory.Repository;
using VesselInventory.Services;
using VesselInventory.Utility;
using VesselInventory.Validations;

namespace VesselInventory.ViewModel
{
    public class VesselGoodReceiveItemRejectAddOrEditVM : ViewModelBase
    {
        public RelayCommand<IClosable> SaveCommand { get; private set; }
        private IParentLoadable _parentLoadable;
        private readonly IVesselGoodReceiveItemRejectRepository _vesselGoodReceiveItemRejectRepository;
        private readonly IGenericRepository<Uom> _UOMRepository;
        public VesselGoodReceiveItemRejectAddOrEditVM(IGenericRepository<Uom> UOMRepository,
            IVesselGoodReceiveItemRejectRepository vesselGoodReceiveItemRejectRepository)
        {
            _vesselGoodReceiveItemRejectRepository = vesselGoodReceiveItemRejectRepository;
            _UOMRepository = UOMRepository;
            InitializeCommands();
        }

        private void InitializeCommands()
        {
            SaveCommand = new RelayCommand<IClosable>(SaveAction);
        }

        public void InitializeData(
            IParentLoadable parentLoadable, 
            int vesselGoodReceiveId, int vesselGoodReceiveItemRejectId = 0)
        {
            _parentLoadable = parentLoadable;
            this.vessel_good_receive_id = vesselGoodReceiveId;
            this.vessel_good_receive_item_reject_id = vesselGoodReceiveItemRejectId;
            LoadAttributesValue();
        }
        ///<summary>
        /// UI Properties
        /// </summary>
        #region
        public override string Title => "Good Receive Item (Reject)";
        #endregion

        ///<summary>
        /// Collection and Entities
        /// </summary>
        #region
        private VesselGoodReceiveItemReject GoodReceiveItemRejectDataView
        {
            get; set;
        } = new VesselGoodReceiveItemReject();

        public IList<string> UomCollection
        {
            get
            {
                IList<string> uoms = new List<string>();
                foreach (var uom in _UOMRepository.GetAll())
                    uoms.Add(uom.uom_name);
                return uoms;
            }
        }
        #endregion


        /// <summary>
        /// Columns Attributes
        /// </summary>
        #region
        private int vessel_good_receive_item_reject_id
        {
            get => GoodReceiveItemRejectDataView.vessel_good_receive_item_reject_id;
            set => GoodReceiveItemRejectDataView.vessel_good_receive_item_reject_id = value;
        }
        public int vessel_good_receive_id
        {
            get => GoodReceiveItemRejectDataView.vessel_good_receive_id;
            set => GoodReceiveItemRejectDataView.vessel_good_receive_id = value;
        }

        public string rf_number
        {
            get => GoodReceiveItemRejectDataView.rf_number;
            set
            {
                GoodReceiveItemRejectDataView.rf_number = value;
                OnPropertyChanged("rf_number");
            }
        }
        public int item_id
        {
            get => GoodReceiveItemRejectDataView.item_id;
            set
            {
                GoodReceiveItemRejectDataView.item_id = value;
                OnPropertyChanged("item_id");
            }
        }
        public int item_group_id
        {
            get => GoodReceiveItemRejectDataView.item_group_id;
            set
            {
                GoodReceiveItemRejectDataView.item_group_id = value;
                OnPropertyChanged("item_group_id");
            }
        }
        public string item_name
        {
            get => GoodReceiveItemRejectDataView.item_name;
            set
            {
                GoodReceiveItemRejectDataView.item_name = value;
                OnPropertyChanged("item_name");
            }
        }
        public string item_dimension_number
        {
            get => GoodReceiveItemRejectDataView.item_dimension_number;
            set
            {
                GoodReceiveItemRejectDataView.item_dimension_number = value;
                OnPropertyChanged("item_dimension_number");
            }
        }
        public string brand_type_id
        {
            get => GoodReceiveItemRejectDataView.brand_type_id;
            set
            {
                GoodReceiveItemRejectDataView.brand_type_id = value;
                OnPropertyChanged("brand_type_id");
            }
        }
        public string brand_type_name
        {
            get => GoodReceiveItemRejectDataView.brand_type_name;
            set
            {
                GoodReceiveItemRejectDataView.brand_type_name = value;
                OnPropertyChanged("brand_type_name");
            }
        }
        public string color_size_id
        {
            get => GoodReceiveItemRejectDataView.color_size_id;
            set
            {
                GoodReceiveItemRejectDataView.color_size_id = value;
                OnPropertyChanged("color_size_id");
            }
        }
        public string color_size_name
        {
            get => GoodReceiveItemRejectDataView.color_size_name;
            set
            {
                GoodReceiveItemRejectDataView.color_size_name = value;
                OnPropertyChanged("color_size_name");
            }
        }

        public decimal qty
        {
            get => GoodReceiveItemRejectDataView.qty;
            set
            {
                GoodReceiveItemRejectDataView.qty = value;
                OnPropertyChanged("qty");
            }
        }

        public string uom
        {
            get => GoodReceiveItemRejectDataView.uom;
            set
            {
                GoodReceiveItemRejectDataView.uom = value;
                OnPropertyChanged("uom");
            }
        }
        #endregion

        /// <summary>
        /// Load Data Methods
        /// </summary>
        #region
        private void LoadAttributesValue()
        {
            if (!RecordHelper.IsNewRecord(vessel_good_receive_item_reject_id))
                GoodReceiveItemRejectDataView = _vesselGoodReceiveItemRejectRepository
                    .GetById(vessel_good_receive_item_reject_id);
        }

        public void LoadDataGrid()
        {
            _parentLoadable.LoadDataGrid();
        }
        #endregion

        /// <summary>
        /// Buttons action and behavior
        /// </summary>
        #region
        private void SaveAction(IClosable window)
        {
            try
            {
                SaveOrUpdate();
                LoadDataGrid();
                CloseWindow(window);
                ResponseMessage.Success(GlobalNamespace.SuccessSave);

            }
            catch (Exception ex)
            {
                ResponseMessage.Error(GlobalNamespace.Error + ex.Message);

            }
        }

        private void ItemCheckUnique()
        {
            if (ItemUniqueValidator.ValidateVesselGoodReceiveItemReject(GoodReceiveItemRejectDataView))
                throw new Exception(GlobalNamespace.ItemDimensionAlreadyExist);
        }
        private void SaveOrUpdate()
        {
            if (RecordHelper.IsNewRecord(vessel_good_receive_item_reject_id))
            {
                ItemCheckUnique();
                _vesselGoodReceiveItemRejectRepository
                    .Save(GoodReceiveItemRejectDataView);
            }
            else
            {
                _vesselGoodReceiveItemRejectRepository
                    .Update(vessel_good_receive_item_reject_id, GoodReceiveItemRejectDataView);
            }
        }
        #endregion
    }
}
