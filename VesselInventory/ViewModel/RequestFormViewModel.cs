using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VesselInventory.Models;
using VesselInventory.Utility;
using VesselInventory.Repository;
using System.Windows;
using VesselInventory.Views;
using VesselInventory.Services;

namespace VesselInventory.ViewModel
{
    public class RequestFormViewModel : ViewModelBase
    {
        private RequestFormRepository _requestFormRepository;

        public RelayCommand NextPageCommand { get; private set; }
        public RelayCommand PrevPageCommand { get; private set; }
        public RelayCommand OpenDialogRequestFormCommand { get; private set; }

        public RequestFormViewModel()
        {
            _requestFormRepository = new RequestFormRepository(new VesselInventoryContext());
            NextPageCommand = new RelayCommand(NextPageCommandAction,IsNextPageCanUse);
            PrevPageCommand = new RelayCommand(PrevPageCommandAction,IsPrevPageCanUse);
            OpenDialogRequestFormCommand = new RelayCommand(OnOpenRequestForm);
            CurrentPage = 1;
            TotalPage = _requestFormRepository.GetRequestFormTotalPage();
            RefreshRequestForm();
        }

        private int _currentPage;
        public int CurrentPage
        {
            get => _currentPage;
            set
            {
                _currentPage = value;
                OnPropertyChanged("CurrentPage");
            }
        }

        private int _totalPage;
        public int TotalPage
        {
            get => _totalPage;
            set
            {
                _totalPage = value;
                OnPropertyChanged("TotalPage");
            }
        }

        private string _searchKeyword = "";
        public string SearchKeyword
        {
            get => _searchKeyword;
            set
            {
                _searchKeyword = value;
                OnPropertyChanged("SearchKeyword");
                TotalPage = _requestFormRepository.GetRequestFormTotalPage(SearchKeyword);
                CurrentPage = 1;
                RefreshRequestForm();
            }
        }

        private ObservableCollection<rf> _requestFormCollection = new ObservableCollection<rf>();
        public ObservableCollection<rf> RequestFormCollection
        {
            get => _requestFormCollection;
            set
            {
                _requestFormCollection = value;
                OnPropertyChanged("RequestFormCollection");
            }
        }
        public void RefreshRequestForm()
        {
            _requestFormCollection.Clear();
            foreach (var _ in _requestFormRepository.GetRequestFormList(SearchKeyword,CurrentPage)){
                _requestFormCollection.Add(new rf
                {
                    rf_id = _.rf_id,
                    rf_number = _.rf_number,
                    rf_date = _.rf_date,
                    department_name = _.department_name,
                    sync_status = _.sync_status,
                    ship_name = _.ship_name,
                    status = _.status,
                    project_number = _.project_number
                });
            }
        }

        public void NextPageCommandAction(object parameter)
        {
            _currentPage = _currentPage + 1;
            OnPropertyChanged("CurrentPage");
            RefreshRequestForm();
        }

        public void OnOpenRequestForm(object parameter)
        {
            WindowService windowService = new WindowService();
            if (parameter  != null)
            {
                int rf_id = int.Parse(parameter.ToString());
                windowService.ShowWindow<RequestForm_AddOrEditView>(new RequestFormAddOrEditViewModel(rf_id));
            } else
            {
                windowService.ShowWindow<RequestForm_AddOrEditView>(new RequestFormAddOrEditViewModel());
            }
        }

        public bool IsNextPageCanUse(object parameter)
        {
            if(_currentPage == _totalPage)
                return false;
            return true;
        }
        public void PrevPageCommandAction(object parameter)
        {
            _currentPage = _currentPage - 1;
            OnPropertyChanged("CurrentPage");
            RefreshRequestForm();
        }

        public bool IsPrevPageCanUse(object parameter)
        {
            if(_currentPage == 1)
                return false;
            return true;
        }
    }
}
