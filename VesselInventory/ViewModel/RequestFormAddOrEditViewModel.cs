using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using ToastNotifications;
using VesselInventory.DTO;
using VesselInventory.Helpers;
using VesselInventory.Helpers.Enums;
using VesselInventory.Models;
using VesselInventory.Repository;
using VesselInventory.Utility;
using ToastNotifications.Messages;

namespace VesselInventory.ViewModel
{
    public class RequestFormAddOrEditViewModel : ViewModelBase
    {
        private RequestFormShipBargeDTO _requestFormShipBargeDTO;
        private RequestFormRepository _requestFormRepository;
        private rf _rf = new rf();

        private ObservableCollection<string> _departmentList = new ObservableCollection<string>();
        private ObservableCollection<rf_item> _rfItemList = new ObservableCollection<rf_item>();


        public RelayCommand Save { get; private set; }

        public RequestFormAddOrEditViewModel(int rf_id_params = 0)
        {
            _requestFormRepository = new RequestFormRepository(new Models.VesselInventoryContext());
            Save = new RelayCommand(SaveAction, IsSaveCanExecute );
            LoadDepartmentList();

            if (rf_id_params != 0)
            {
                Title = "Edit Request Form";
                IsVisibleButtonUpdate = true;
                IsVisibleButtonSave = false;
                IsItemEnabled = true;
                IsVisibleBargeCheck = false;

                _rf = _requestFormRepository.GetById(rf_id_params);
            } else
            {
                Title = "Add Request Form";
                IsVisibleButtonSave = true;
                IsVisibleButtonUpdate = false;
                IsItemEnabled = false;
                IsVisibleBargeCheck = true;

                rf_id = rf_id_params;
                _rf.department_name = DepartmentList.First();

                _rf.target_delivery_date = DateTime.Now;
                _requestFormShipBargeDTO = _requestFormRepository.GetRrequestFormShipBarge();
                ship_code = _requestFormShipBargeDTO.ship_code;
                barge_code = _requestFormShipBargeDTO.barge_code;
                ship_id = _requestFormShipBargeDTO.ship_id;
                barge_id = _requestFormShipBargeDTO.barge_id;
                ship_name = _requestFormShipBargeDTO.ship_name;
                barge_name = _requestFormShipBargeDTO.barge_name;
                rf_number = _requestFormShipBargeDTO.rf_number + '-' + ship_code;
                rf_id = rf_id_params;
            }
        }

        // <summary>
        // UI properties
        // </summary>
        public override string Title { get; set; }

        private bool _IsVisibleButtonUpdate;
        public bool IsVisibleButtonUpdate
        {
            get => _IsVisibleButtonUpdate;
            set
            {
                if  (_IsVisibleButtonUpdate == value)
                    return;
                _IsVisibleButtonUpdate = value;
                OnPropertyChanged("IsVisibleButtonUpdate");
            }
        }

        private bool _IsVisibleButtonSave;
        public bool IsVisibleButtonSave
        {
            get => _IsVisibleButtonSave;
            set
            {
                if  (_IsVisibleButtonSave == value)
                    return;
                _IsVisibleButtonSave = value;
                OnPropertyChanged("IsVisibleButtonSave");
            }
        }

        private bool _IsVisibleBargeCheck;
        public bool IsVisibleBargeCheck
        {
            get => _IsVisibleBargeCheck;
            set
            {
                if  (_IsVisibleBargeCheck == value)
                    return;
                _IsVisibleBargeCheck = value;
                OnPropertyChanged("IsVisiblBargeCheck");
            }
        }

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

        private bool _IsCheckedBargeRequest = false;
        public bool IsCheckedBargeRequest
        {
            get => _IsCheckedBargeRequest;
            set
            {
                if  (_IsCheckedBargeRequest == value)
                    return;
                _IsCheckedBargeRequest = value;

                if (_requestFormShipBargeDTO is null)
                    return;

                if (_IsCheckedBargeRequest)
                {
                    rf_number = _requestFormShipBargeDTO.rf_number + '-' + ship_code + '-' + barge_code;
                    ship_name = _requestFormShipBargeDTO.barge_name;
                    ship_id = _requestFormShipBargeDTO.barge_id;
                }
                else
                {
                    rf_number = _requestFormShipBargeDTO.rf_number + '-' + ship_code;
                    ship_name = _requestFormShipBargeDTO.ship_name;
                    ship_id = _requestFormShipBargeDTO.ship_id;
                }
                OnPropertyChanged("IsCheckedBargeRequest");
            }
        }
        
        // <summary>
        // Reserved columns
        // </summary>
        private int _barge_id;
        public int barge_id { get => _barge_id; set => _barge_id = value; }

        private string _barge_name;
        public string barge_name { get => _barge_name; set => _barge_name = value; }

        private string _ship_code;
        public string ship_code { get => _ship_code; set => _ship_code = value; }
        private string _barge_code;
        public string barge_code { get => _barge_code; set => _barge_code = value; }

        public int ship_id { get => _rf.ship_id; set => _rf.ship_id = value; }
        public int rf_id { get => _rf.rf_id; set => _rf.rf_id = value; }
        public string rf_number
        {
            get => _rf.rf_number;
            set
            {
                _rf.rf_number = value;
                OnPropertyChanged("rf_number");
            }
        }
        public string ship_name
        {
            get => _rf.ship_name;
            set
            {
                _rf.ship_name = value;
                OnPropertyChanged("ship_name");
            }
        }

        public ObservableCollection<string> DepartmentList
        {
            get => _departmentList;
            set
            {
                _departmentList = value;
                OnPropertyChanged("DepartmentList");
            }
        }

        public ObservableCollection<rf_item> RfItems
        {
            get => _rfItemList;
            set
            {
                _rfItemList = value;
                OnPropertyChanged("RfItems");
            }
        }

        private void LoadDepartmentList()
        {
            foreach(var department in GenericHelper.GetLookupValues("DEPARTMENT") ) {
                _departmentList.Add(department.description);
            }
        }

        // <summary>
        // UI Columns
        // </summary>
        [Required(ErrorMessage ="Project Number cannot be empty.")]
        public string project_number {
            get => _rf.project_number;
            set
            {
                _rf.project_number = value;
                OnPropertyChanged("project_number");
            }
        }

        [Required]
        public string department_name
        {
            get
            {
                return _rf.department_name;
            }
            set
            {
                _rf.department_name = value;
                OnPropertyChanged("department_name");
            }
        }

        [Required]
        public DateTime target_delivery_date
        {
            get => _rf.target_delivery_date;
            set
            {
                _rf.target_delivery_date = DateTime.Parse(value.ToString());
                OnPropertyChanged("target_delivery_date");
            }
        }
        
        [Column(TypeName = "text")]
        [Required]
        public string notes
        {
            get => _rf.notes;
            set
            {
                _rf.notes = value;
                OnPropertyChanged("notes");
            }
        }

        public void SaveAction(object parameter)
        {
            if (rf_id == 0)
            {
                _rf.status = Status.DRAFT.ToString();
                _rf.created_by = "Aditya";
                _rf.sync_status = SyncStatus.NOT_SYNC.ToString();
                _rf = _requestFormRepository.SaveTransaction(_rf);
            } else
            {
                _rf = _requestFormRepository.Update(rf_id, _rf);
            }
            IsVisibleButtonSave = false;
            IsVisibleButtonUpdate = true;
            IsItemEnabled = true;
            Notifier toasMessage = ToastNotification.Instance.GetInstance();
            toasMessage.ShowSuccess("Data saved successfully.");

            Navigate.To(new RequestFormViewModel());
        }
        public bool IsSaveCanExecute(object parameter)
        {
            if (project_number == "")
                return false;
            if (project_number is null)
                return false;
            if(project_number.Trim().Length < 0)
                return false;
            return true;
        }
    }
}
