using System.Collections.ObjectModel;
using VesselInventory.Models;
using VesselInventory.Utility;
using VesselInventory.Repository;
using VesselInventory.Views;
using VesselInventory.Services;
using System.Collections.Generic;
using System.Windows;
using Unity;

namespace VesselInventory.ViewModel
{
    public class RequestFormVM : RequestFormVMBase, IParentLoadable
    {
        public RelayCommand NextPageCommand { get; private set; }
        public RelayCommand PrevPageCommand { get; private set; }
        public RelayCommand OpenDialogRequestFormCommand { get; private set; }

        private readonly IWindowService _windowService;
        private readonly IRequestFormRepository _requestFormRepository;

        public RequestFormVM(IWindowService windowService, IRequestFormRepository requestFormRepository)
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

        private int TotalPageFromDatabase
        {
            get
            {
                return _requestFormRepository.
                    GetRequestFormTotalPage(SearchKeyword);
            }
        }

        /// <summary>
        /// UI Properties
        /// </summary>
        #region
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
        #endregion

        /// <summary>
        /// Entity and Collections
        /// </summary>
        #region
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
        #endregion

        /// <summary>
        /// Load UI methods
        /// </summary>
        #region
        public void LoadDataGrid()
        {
            RequestFormCollection.Clear();
            foreach (var rf in RequestFormList)
                RequestFormCollection.Add(rf);
            UpdateTotalPage();
        }
        #endregion

        public void OpenRequestFormAction(object parameter)
        {
            var container = ((App)Application.Current).UnityContainer;
            var requestFormAddOrEditVM = container.Resolve<RequestFormAddOrEditVM>();
            if (parameter is null)
                requestFormAddOrEditVM.InitializeData(this);
            else
                requestFormAddOrEditVM.InitializeData(this, (int)parameter);

            _windowService.ShowWindow<RequestForm_AddOrEditView>(requestFormAddOrEditVM);
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
