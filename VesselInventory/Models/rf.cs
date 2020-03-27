namespace VesselInventory.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;
    using VesselInventory.Utility;

    [Table("rf")]
    public partial class rf : ObservableObject
    {
        private int _rf_id;
        [Key]
        public int rf_id
        {
            get => _rf_id;
            set
            {
                _rf_id = value;
                OnPropertyChanged();
            }
        }

        private string _rf_number;
        [Required]
        [StringLength(25)]
        public string rf_number {
            get => _rf_number;
            set
            {
                _rf_number = value;
                OnPropertyChanged();
            }
        }

        private DateTime? _rf_date;
        [Column(TypeName = "date")]
        public DateTime? rf_date
        {
            get => _rf_date;
            set
            {
                _rf_date = value;
                OnPropertyChanged();
            }
        }

        private string _project_number { get; set; }
        [StringLength(25)]
        [Required]
        public string project_number
        {
            get => _project_number;
            set
            {
                _project_number = value;
                OnPropertyChanged();
            }
        }

        private string _department_name;
        [Required]
        [StringLength(15)]
        public string department_name
        {
            get => _department_name;
            set
            {
                _department_name = value;
                OnPropertyChanged("department_name");
            }
        }

        private DateTime _target_delivery_date;
        [Column(TypeName = "date")]
        public DateTime target_delivery_date
        {
            get => _target_delivery_date;
            set
            {
                _target_delivery_date = value;
                OnPropertyChanged();
            }
        }

        private string _status;
        [Required]
        [StringLength(15)]
        public string status {
            get => _status;
            set
            {
                _status = value;
                OnPropertyChanged();
            }
        }

        private int _ship_id;
        public int ship_id
        {
            get => _ship_id;
            set
            {
                _ship_id = value;
                OnPropertyChanged();
            }
        }

        private string _ship_name;
        [Required]
        [StringLength(30)]
        public string ship_name
        {
            get => _ship_name;
            set
            {
                _ship_name = value;
                OnPropertyChanged();
            }
        }

        private string _notes;
        [Column(TypeName = "text")]
        public string notes
        {
            get => _notes;
            set
            {
                _notes = value;
                OnPropertyChanged();
            }
        }

        private string _sync_status;
        [StringLength(15)]
        public string sync_status
        {
            get => _sync_status;
            set
            {
                _sync_status = value;
                OnPropertyChanged();
            }
        }

        private DateTime? _created_date;
        public DateTime? created_date
        {
            get => _created_date;
            set
            {
                _created_date = value;
                OnPropertyChanged();
            }
        }

        private string _created_by;
        [StringLength(30)]
        public string created_by
        {
            get => _created_by;
            set
            {
                _created_by = value;
                OnPropertyChanged();
            }
        }

        private DateTime? _last_modified_date;
        public DateTime? last_modified_date
        {
            get => _last_modified_date;
            set
            {
                _last_modified_date = value;
                OnPropertyChanged();
            }
        }

        private string _last_modified_by;
        [StringLength(30)]
        public string last_modified_by
        {
            get => _last_modified_by;
            set
            {
                _last_modified_by = value;
                OnPropertyChanged();
            }
        }
    }
}
