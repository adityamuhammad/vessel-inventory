using System.Collections.ObjectModel;
using VesselInventory.Models;
using VesselInventory.Utility;
using VesselInventory.Repository;
using VesselInventory.Views;
using VesselInventory.Services;
using System.Collections.Generic;

namespace VesselInventory.ViewModel
{
    public class RequestFormViewModel : RequestFormViewModelBase, IParentLoadable
    {
        public RelayCommand NextPageCommand { get; private set; }
        public RelayCommand PrevPageCommand { get; private set; }
        public RelayCommand OpenDialogRequestFormCommand { get; private set; }

        private readonly IWindowService _windowService;
        private readonly IRequestFormRepository _requestFormRepository;

        public RequestFormViewModel(IWindowService windowService, IRequestFormRepository requestFormRepository)
        {
            _windowService = windowService;
            _requestFormRepository = requestFormRepository;
            InitializeCommands();
            ResetCurrentPage();
            LoadDataGrid();
        }

        private void InitializeCommands()
        {
            NextPageCommand = new RelayCommand(NextPageAction, IsNextPageCanExecute);
            PrevPageCommand = new RelayCommand(PrevPageAction, IsPrevPageCanExecute);
            OpenDialogRequestFormCommand = new RelayCommand(OpenRequestFormAction);
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
                ResetCurrentPage();
                LoadDataGrid();
            }
        }

        public ObservableCollection<RequestForm> RequestFormCollection { get; } 
            = new ObservableCollection<RequestForm>();

        private IEnumerable<RequestForm> RequestFormList
        {
            get
            {
                return _requestFormRepository.
                    GetRequestFormList(
                        SearchKeyword, CurrentPage
                     );
            }
        }

        public void LoadDataGrid()
        {
            RequestFormCollection.Clear();
            foreach (var rf in RequestFormList)
                RequestFormCollection.Add(rf);
            UpdateTotalPage();
        }

        public void OpenRequestFormAction(object parameter)
        {
            if (parameter is null)
                _windowService.ShowWindow<RequestForm_AddOrEditView>
                    (new RequestFormAddOrEditViewModel(this));
            else
                _windowService.ShowWindow<RequestForm_AddOrEditView>
                    (new RequestFormAddOrEditViewModel(this,(int)parameter));
        }

        private int TotalPageFromDatabase
        {
            get
            {
                return _requestFormRepository.
                    GetRequestFormTotalPage(SearchKeyword);
            }
        }

        private void UpdateTotalPage() => TotalPage = TotalPageFromDatabase;
        private void ResetCurrentPage() => CurrentPage = 1;
        private void IncrementCurrentPage() => CurrentPage = CurrentPage + 1;
        private void DecrementCurrentPage() => CurrentPage = CurrentPage - 1;
        private bool IsNextPageCanExecute(object parameter) => !(CurrentPage >= TotalPage);
        private bool IsPrevPageCanExecute(object parameter) => !(CurrentPage <= 1);

        private void NextPageAction(object parameter)
        {
            IncrementCurrentPage();
            LoadDataGrid();
        }
        private void PrevPageAction(object parameter)
        {
            DecrementCurrentPage();
            LoadDataGrid();
        }
    }
}
