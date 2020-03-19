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

namespace VesselInventory.ViewModel
{
    public class RequestFormViewModel : ViewModelBase
    {
        private ObservableCollection<rf> _requestFormCollection = new ObservableCollection<rf>();
        private RequestFormRepository _requestFormRepository;
        private string _searchKeyword = "";
        public int _currentPage;
        public int _totalPage;

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
            RefreshGrid();
        }
        public int CurrentPage
        {
            get => _currentPage;
            set
            {
                _currentPage = value;
                OnPropertyChanged("CurrentPage");
            }
        }

        public int TotalPage
        {
            get => _totalPage;
            set
            {
                _totalPage = value;
                OnPropertyChanged("TotalPage");
            }
        }

        public string SearchKeyword
        {
            get => _searchKeyword;
            set
            {
                _searchKeyword = value;
                OnPropertyChanged("SearchKeyword");
                TotalPage = _requestFormRepository.GetRequestFormTotalPage(SearchKeyword);
                CurrentPage = 1;
                RefreshGrid();
            }
        }

        public ObservableCollection<rf> RequestFormCollection
        {
            get => _requestFormCollection;
            set
            {
                _requestFormCollection = value;
                OnPropertyChanged("RequestFormCollection");
            }
        }
        public void RefreshGrid()
        {
            _requestFormCollection.Clear();
            foreach (var data in _requestFormRepository.GetRequestFormList(SearchKeyword,CurrentPage)){
                _requestFormCollection.Add(new rf
                {
                    rf_id = data.rf_id,
                    rf_number = data.rf_number,
                    rf_date = data.rf_date,
                    department_name = data.department_name,
                    sync_status = data.sync_status,
                    ship_name = data.ship_name,
                    status = data.status,
                    project_number = data.project_number
                });
            }
        }

        public void NextPageCommandAction(object parameter)
        {
            _currentPage = _currentPage + 1;
            OnPropertyChanged("CurrentPage");
            RefreshGrid();
        }

        public void OnOpenRequestForm(object parameter)
        {
            RequestForm_AddOrEditView requestForm_Modal = new RequestForm_AddOrEditView();
            if (parameter  != null)
            {
                int rf_id = int.Parse(parameter.ToString());
                requestForm_Modal.DataContext = new RequestFormAddOrEditViewModel(rf_id);
            } else
            {
                requestForm_Modal.DataContext = new RequestFormAddOrEditViewModel();
            }
                
            requestForm_Modal.ShowDialog();
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
            RefreshGrid();
        }

        public bool IsPrevPageCanUse(object parameter)
        {
            if(_currentPage == 1)
                return false;
            return true;
        }
    }
}
