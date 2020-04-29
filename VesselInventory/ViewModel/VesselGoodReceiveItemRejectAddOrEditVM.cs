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
                    uoms.Add(uom.UomName);
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
            get => GoodReceiveItemRejectDataView.VesselGoodReceiveItemRejectId;
            set => GoodReceiveItemRejectDataView.VesselGoodReceiveItemRejectId = value;
        }
        public int vessel_good_receive_id
        {
            get => GoodReceiveItemRejectDataView.VesselGoodReceiveId;
            set => GoodReceiveItemRejectDataView.VesselGoodReceiveId = value;
        }

        public string rf_number
        {
            get => GoodReceiveItemRejectDataView.RequestFormNumber;
            set
            {
                GoodReceiveItemRejectDataView.RequestFormNumber = value;
                OnPropertyChanged("rf_number");
            }
        }
        public int item_id
        {
            get => GoodReceiveItemRejectDataView.ItemId;
            set
            {
                GoodReceiveItemRejectDataView.ItemId = value;
                OnPropertyChanged("item_id");
            }
        }
        public int item_group_id
        {
            get => GoodReceiveItemRejectDataView.ItemGroupId;
            set
            {
                GoodReceiveItemRejectDataView.ItemGroupId = value;
                OnPropertyChanged("item_group_id");
            }
        }
        public string item_name
        {
            get => GoodReceiveItemRejectDataView.ItemName;
            set
            {
                GoodReceiveItemRejectDataView.ItemName = value;
                OnPropertyChanged("item_name");
            }
        }
        public string item_dimension_number
        {
            get => GoodReceiveItemRejectDataView.ItemDimensionNumber;
            set
            {
                GoodReceiveItemRejectDataView.ItemDimensionNumber = value;
                OnPropertyChanged("item_dimension_number");
            }
        }
        public string brand_type_id
        {
            get => GoodReceiveItemRejectDataView.BrandTypeId;
            set
            {
                GoodReceiveItemRejectDataView.BrandTypeId = value;
                OnPropertyChanged("brand_type_id");
            }
        }
        public string brand_type_name
        {
            get => GoodReceiveItemRejectDataView.BrandTypeName;
            set
            {
                GoodReceiveItemRejectDataView.BrandTypeName = value;
                OnPropertyChanged("brand_type_name");
            }
        }
        public string color_size_id
        {
            get => GoodReceiveItemRejectDataView.ColorSizeId;
            set
            {
                GoodReceiveItemRejectDataView.ColorSizeId = value;
                OnPropertyChanged("color_size_id");
            }
        }
        public string color_size_name
        {
            get => GoodReceiveItemRejectDataView.ColorSizeName;
            set
            {
                GoodReceiveItemRejectDataView.ColorSizeName = value;
                OnPropertyChanged("color_size_name");
            }
        }

        public decimal qty
        {
            get => GoodReceiveItemRejectDataView.Qty;
            set
            {
                GoodReceiveItemRejectDataView.Qty = value;
                OnPropertyChanged("qty");
            }
        }

        public string uom
        {
            get => GoodReceiveItemRejectDataView.Uom;
            set
            {
                GoodReceiveItemRejectDataView.Uom = value;
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
