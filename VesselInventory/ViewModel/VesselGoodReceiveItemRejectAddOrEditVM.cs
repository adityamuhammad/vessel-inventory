using System;
using System.Collections.Generic;
using VesselInventory.Commons;
using VesselInventory.Models;
using VesselInventory.Repository;
using VesselInventory.Services;
using VesselInventory.Utility;

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
            if(!IsNewRecord)
                GoodReceiveItemRejectEntity = _vesselGoodReceiveItemRejectRepository
                    .GetById(vessel_good_receive_item_reject_id);
        }

        ///<summary>
        /// UI Properties
        /// </summary>
        #region
        #endregion

        ///<summary>
        /// Collection and Entities
        /// </summary>
        #region
        private bool IsNewRecord => (vessel_good_receive_item_reject_id == 0);
        private VesselGoodReceiveItemReject GoodReceiveItemRejectEntity
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
            get => GoodReceiveItemRejectEntity.vessel_good_receive_item_reject_id;
            set => GoodReceiveItemRejectEntity.vessel_good_receive_item_reject_id = value;
        }
        public int vessel_good_receive_id
        {
            get => GoodReceiveItemRejectEntity.vessel_good_receive_id;
            set => GoodReceiveItemRejectEntity.vessel_good_receive_id = value;
        }

        public string rf_number
        {
            get => GoodReceiveItemRejectEntity.rf_number;
            set
            {
                GoodReceiveItemRejectEntity.rf_number = value;
                OnPropertyChanged("rf_number");
            }
        }
        public int item_id
        {
            get => GoodReceiveItemRejectEntity.item_id;
            set
            {
                GoodReceiveItemRejectEntity.item_id = value;
                OnPropertyChanged("item_id");
            }
        }
        public int item_group_id
        {
            get => GoodReceiveItemRejectEntity.item_group_id;
            set
            {
                GoodReceiveItemRejectEntity.item_group_id = value;
                OnPropertyChanged("item_group_id");
            }
        }
        public string item_name
        {
            get => GoodReceiveItemRejectEntity.item_name;
            set
            {
                GoodReceiveItemRejectEntity.item_name = value;
                OnPropertyChanged("item_name");
            }
        }
        public string item_dimension_number
        {
            get => GoodReceiveItemRejectEntity.item_dimension_number;
            set
            {
                GoodReceiveItemRejectEntity.item_dimension_number = value;
                OnPropertyChanged("item_dimension_number");
            }
        }
        public string brand_type_id
        {
            get => GoodReceiveItemRejectEntity.brand_type_id;
            set
            {
                GoodReceiveItemRejectEntity.brand_type_id = value;
                OnPropertyChanged("brand_type_id");
            }
        }
        public string brand_type_name
        {
            get => GoodReceiveItemRejectEntity.brand_type_name;
            set
            {
                GoodReceiveItemRejectEntity.brand_type_name = value;
                OnPropertyChanged("brand_type_name");
            }
        }
        public string color_size_id
        {
            get => GoodReceiveItemRejectEntity.color_size_id;
            set
            {
                GoodReceiveItemRejectEntity.color_size_id = value;
                OnPropertyChanged("color_size_id");
            }
        }
        public string color_size_name
        {
            get => GoodReceiveItemRejectEntity.color_size_name;
            set
            {
                GoodReceiveItemRejectEntity.color_size_name = value;
                OnPropertyChanged("color_size_name");
            }
        }

        public decimal qty
        {
            get => GoodReceiveItemRejectEntity.qty;
            set
            {
                GoodReceiveItemRejectEntity.qty = value;
                OnPropertyChanged("qty");
            }
        }

        public string uom
        {
            get => GoodReceiveItemRejectEntity.uom;
            set
            {
                GoodReceiveItemRejectEntity.uom = value;
                OnPropertyChanged("uom");
            }
        }
        #endregion

        /// <summary>
        /// Load Data Methods
        /// </summary>
        #region
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
                if(IsNewRecord)
                    _vesselGoodReceiveItemRejectRepository
                        .Save(GoodReceiveItemRejectEntity);
                else
                    _vesselGoodReceiveItemRejectRepository
                        .Update(vessel_good_receive_item_reject_id,GoodReceiveItemRejectEntity);
                LoadDataGrid();
                CloseWindow(window);
                ResponseMessage.Success(GlobalNamespace.SuccessSave);

            } catch (Exception ex)
            {
                ResponseMessage.Error(GlobalNamespace.Error + ex.Message);

            }
        }
        #endregion
    }
}
