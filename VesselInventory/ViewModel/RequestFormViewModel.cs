﻿using System;
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
using System.Windows.Data;

namespace VesselInventory.ViewModel
{
    public class RequestFormViewModel : ViewModelBase
    {
        private RequestFormRepository _requestFormRepository;

        public RelayCommand NextPageCommand { get; private set; }
        public RelayCommand PrevPageCommand { get; private set; }
        public RelayCommand OpenDialogRequestFormCommand { get; private set; }
        public RelayCommand SwitchTab { get; private set; }

        public RequestFormViewModel()
        {
            _requestFormRepository = new RequestFormRepository();
            NextPageCommand = new RelayCommand(NextPageCommandAction,IsNextPageCanUse);
            PrevPageCommand = new RelayCommand(PrevPageCommandAction,IsPrevPageCanUse);
            OpenDialogRequestFormCommand = new RelayCommand(OnOpenRequestForm);
            SwitchTab = new RelayCommand(SwitchTabAction);
            CurrentPage = 1;
            TotalPage = _requestFormRepository.GetRequestFormTotalPage(SearchKeyword);
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

        private string _searchKeyword = string.Empty;
        public string SearchKeyword
        {
            get => _searchKeyword;
            set
            {
                _searchKeyword = value;
                OnPropertyChanged("SearchKeyword");
                CurrentPage = 1;
                TotalPage = _requestFormRepository.GetRequestFormTotalPage(SearchKeyword);
                RefreshRequestForm();
            }
        }

        private ObservableCollection<rf> _requestFormCollection = new ObservableCollection<rf>();
        public ObservableCollection<rf> RequestFormCollection
        {
            get => _requestFormCollection;
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

        private void NextPageCommandAction(object parameter)
        {
            CurrentPage = CurrentPage + 1;
            RefreshRequestForm();
        }

        private bool IsNextPageCanUse(object parameter)
        {
            if(CurrentPage == TotalPage)
                return false;
            return true;
        }
        private void PrevPageCommandAction(object parameter)
        {
            CurrentPage = CurrentPage - 1;
            RefreshRequestForm();
        }

        private bool IsPrevPageCanUse(object parameter)
        {
            if(CurrentPage == 1)
                return false;
            return true;
        }
        private void SwitchTabAction(object parameter)
        {
            switch ((string)parameter)
            {
                case "List":
                    Navigate.To(new RequestFormViewModel());
                    break;
                case "ItemStatus":
                    Navigate.To(new RequestFormItemStatusViewModel());
                    break;
                case "ItemPending":
                    Navigate.To(new RequestFormItemPendingViewModel());
                    break;
                default:
                    Navigate.To(new RequestFormViewModel());
                    break;
            }
        }

    }
}
