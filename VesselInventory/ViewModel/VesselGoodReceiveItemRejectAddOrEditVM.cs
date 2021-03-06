﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using VesselInventory.Commons;
using VesselInventory.Commons.Enums;
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
        public RelayCommand ClearTextScannCommand { get; private set; }
        private IDataGrid _parentLoadable;
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
            ClearTextScannCommand = new RelayCommand(ClearTextScannAction);
        }

        public void InitializeData(
            IDataGrid parentLoadable,
            int vesselGoodReceiveId, int vesselGoodReceiveItemRejectId = 0)
        {
            _parentLoadable = parentLoadable;
            this.VesselGoodReceiveId = vesselGoodReceiveId;
            this.VesselGoodReceiveItemRejectId = vesselGoodReceiveItemRejectId;
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
        private string _textScann = string.Empty;
        private void Reset()
        {
            RequestFormNumber = "";
            ItemId = 0;
            ItemGroupId = 0;
            ItemName = "";
            ItemDimensionNumber = "";
            BrandTypeId = "";
            BrandTypeName = "";
            ColorSizeId = "";
            ColorSizeName = "";
        }
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
                        if (i == 0) RequestFormNumber = textScanList[i];
                        else if (i == 1) ItemId = int.Parse(textScanList[i]);
                        else if (i == 2) ItemGroupId = int.Parse(textScanList[i]);
                        else if (i == 3) ItemName = textScanList[i];
                        else if (i == 4) ItemDimensionNumber = textScanList[i];
                        else if (i == 5) BrandTypeId = textScanList[i];
                        else if (i == 6) BrandTypeName = textScanList[i];
                        else if (i == 7) ColorSizeId = textScanList[i];
                        else if (i == 8) ColorSizeName = textScanList[i];
                    }

                } catch (Exception) { }
                OnPropertyChanged("TextScann");
            }
        }
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
        private int VesselGoodReceiveItemRejectId
        {
            get => GoodReceiveItemRejectDataView.VesselGoodReceiveItemRejectId;
            set => GoodReceiveItemRejectDataView.VesselGoodReceiveItemRejectId = value;
        }
        public int VesselGoodReceiveId
        {
            get => GoodReceiveItemRejectDataView.VesselGoodReceiveId;
            set => GoodReceiveItemRejectDataView.VesselGoodReceiveId = value;
        }

        public string RequestFormNumber
        {
            get => GoodReceiveItemRejectDataView.RequestFormNumber;
            set
            {
                GoodReceiveItemRejectDataView.RequestFormNumber = value;
                OnPropertyChanged("RequestFormNumber");
            }
        }
        public int ItemId
        {
            get => GoodReceiveItemRejectDataView.ItemId;
            set
            {
                GoodReceiveItemRejectDataView.ItemId = value;
                OnPropertyChanged("ItemId");
            }
        }
        public int ItemGroupId
        {
            get => GoodReceiveItemRejectDataView.ItemGroupId;
            set
            {
                GoodReceiveItemRejectDataView.ItemGroupId = value;
                OnPropertyChanged("ItemGroupId");
            }
        }
        public string ItemName
        {
            get => GoodReceiveItemRejectDataView.ItemName;
            set
            {
                GoodReceiveItemRejectDataView.ItemName = value;
                OnPropertyChanged("ItemName");
            }
        }
        public string ItemDimensionNumber
        {
            get => GoodReceiveItemRejectDataView.ItemDimensionNumber;
            set
            {
                GoodReceiveItemRejectDataView.ItemDimensionNumber = value;
                OnPropertyChanged("ItemDimensionNumber");
            }
        }
        public string BrandTypeId
        {
            get => GoodReceiveItemRejectDataView.BrandTypeId;
            set
            {
                GoodReceiveItemRejectDataView.BrandTypeId = value;
                OnPropertyChanged("BrandTypeId");
            }
        }
        public string BrandTypeName
        {
            get => GoodReceiveItemRejectDataView.BrandTypeName;
            set
            {
                GoodReceiveItemRejectDataView.BrandTypeName = value;
                OnPropertyChanged("BrandTypeName");
            }
        }
        public string ColorSizeId
        {
            get => GoodReceiveItemRejectDataView.ColorSizeId;
            set
            {
                GoodReceiveItemRejectDataView.ColorSizeId = value;
                OnPropertyChanged("ColorSizeId");
            }
        }
        public string ColorSizeName
        {
            get => GoodReceiveItemRejectDataView.ColorSizeName;
            set
            {
                GoodReceiveItemRejectDataView.ColorSizeName = value;
                OnPropertyChanged("ColorSizeName");
            }
        }

        public decimal Qty
        {
            get => GoodReceiveItemRejectDataView.Qty;
            set
            {
                GoodReceiveItemRejectDataView.Qty = value;
                OnPropertyChanged("Qty");
            }
        }

        public string Uom
        {
            get => GoodReceiveItemRejectDataView.Uom;
            set
            {
                GoodReceiveItemRejectDataView.Uom = value;
                OnPropertyChanged("Uom");
            }
        }
        #endregion

        /// <summary>
        /// Load Data Methods
        /// </summary>
        #region
        private void LoadAttributesValue()
        {
            if (!RecordHelper.IsNewRecord(VesselGoodReceiveItemRejectId))
                GoodReceiveItemRejectDataView = _vesselGoodReceiveItemRejectRepository
                    .GetById(VesselGoodReceiveItemRejectId);
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
        private void CheckZeroQty()
        {
            if (ItemMinimumQtyValidator.IsZeroQty(Qty))
                throw new ValidationException(GlobalNamespace.QtyCannotBeZero);
        }
        private void SaveAction(IClosable window)
        {
            try
            {
                CheckZeroQty();
                SaveOrUpdate();
                LoadDataGrid();
                CloseWindow(window);
                ResponseMessage.Success(GlobalNamespace.SuccessSave);

            } catch (ValidationException ex){
                ResponseMessage.Error(GlobalNamespace.ErrorSave + ex.Message);
            }
            catch (Exception)
            {
                ResponseMessage.Error(GlobalNamespace.ErrorSave);
            }
        }

        private void ItemCheckUnique()
        {
            if (ItemUniqueValidator.ValidateVesselGoodReceiveItemReject(GoodReceiveItemRejectDataView))
                throw new ValidationException(GlobalNamespace.ItemDimensionAlreadyExist);
        }
        private void SaveOrUpdate()
        {
            if (RecordHelper.IsNewRecord(VesselGoodReceiveItemRejectId))
            {
                ItemCheckUnique();
                Save();
            }
            else
            {
                Update();
            }
        }

        private void Update()
        {
            GoodReceiveItemRejectDataView.LastModifiedBy = Auth.Instance.PersonName;
            GoodReceiveItemRejectDataView.LastModifiedDate = DateTime.Now;
            _vesselGoodReceiveItemRejectRepository
                .Update(VesselGoodReceiveItemRejectId, GoodReceiveItemRejectDataView);
        }

        private void Save()
        {
            GoodReceiveItemRejectDataView.CreatedBy = Auth.Instance.PersonName;
            GoodReceiveItemRejectDataView.CreatedDate = DateTime.Now;
            GoodReceiveItemRejectDataView.SyncStatus = SyncStatus.Not_Sync.GetDescription();
            _vesselGoodReceiveItemRejectRepository
                .Save(GoodReceiveItemRejectDataView);
        }
        private void ClearTextScannAction(object parameter)
        {
            TextScann = string.Empty;
        }
        #endregion
    }
}
