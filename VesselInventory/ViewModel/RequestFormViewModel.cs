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
using System.Windows.Data;

namespace VesselInventory.ViewModel
{
    public class RequestFormViewModel : ViewModelBase, IParentLoadable
    {
        private RequestFormRepository _requestFormRepository;

        public RelayCommand NextPageCommand { get; private set; }
        public RelayCommand PrevPageCommand { get; private set; }
        public RelayCommand OpenDialogRequestFormCommand { get; private set; }
        public RelayCommand SwitchTab { get; private set; }

        private IWindowService _windowService;

        public RequestFormViewModel()
        {
            _requestFormRepository = new RequestFormRepository();
            _windowService = new WindowService();
            SetCommands();
            CurrentPage = 1;
            TotalPage = _requestFormRepository.GetRequestFormTotalPage(SearchKeyword);
            LoadGrid();
        }

        private void SetCommands()
        {
            NextPageCommand = new RelayCommand(NextPageCommandAction, IsNextPageCanUse);
            PrevPageCommand = new RelayCommand(PrevPageCommandAction, IsPrevPageCanUse);
            OpenDialogRequestFormCommand = new RelayCommand(OnOpenRequestForm);
            SwitchTab = new RelayCommand(SwitchTabAction);
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
                LoadGrid();
            }
        }
        public ObservableCollection<RF> RequestFormCollection { get; } = new ObservableCollection<RF>();

        public void LoadGrid()
        {
            RequestFormCollection.Clear();
            foreach (var _ in _requestFormRepository.GetRequestFormList(SearchKeyword,CurrentPage))
                RequestFormCollection.Add(_);
        }
        public void OnOpenRequestForm(object parameter)
        {
            if (parameter  != null)
            {
                int rf_id = int.Parse(parameter.ToString());
                _windowService.ShowWindow<RequestForm_AddOrEditView>(new RequestFormAddOrEditViewModel(this,rf_id));
            } else
            {
                _windowService.ShowWindow<RequestForm_AddOrEditView>(new RequestFormAddOrEditViewModel(this));
            }
        }

        private void NextPageCommandAction(object parameter)
        {
            CurrentPage = CurrentPage + 1;
            LoadGrid();
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
            LoadGrid();
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
