using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VesselInventory.Dto;
using VesselInventory.Repository;
using VesselInventory.Utility;

namespace VesselInventory.ViewModel
{
    public class OnHandVM : ViewModelBase
    {
        public override string Title => "On Hand";
        public RelayCommand SearchCommand { get; private set; }
        public RelayCommand NextPageCommand { get; private set; }
        public RelayCommand PrevPageCommand { get; private set; }
        public RelayCommand OpenDialogLogCommand { get; private set; }
        private readonly IOnHandRepository _onHandRepository; 
        public OnHandVM(IOnHandRepository onHandRepository)
        {
            InitializeCommands();
            _onHandRepository = onHandRepository;
            ResetCurrentPage();
            LoadDataGrid();
        }
        private void InitializeCommands()
        {
            SearchCommand = new RelayCommand(SearchAction);
            NextPageCommand = new RelayCommand(NextPageAction, IsNextPageCanExecute);
            PrevPageCommand = new RelayCommand(PrevPageAction, IsPrevPageCanExecute);
            OpenDialogLogCommand = new RelayCommand(ViewLog);
        }

        /// <summary>
        /// UI properties
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

        public int DataGridRows => 10;
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
            }
        }
        #endregion

        /// <summary>
        /// Entity and Collections
        /// </summary>
        #region
        public ObservableCollection<OnHandDto> OnHandCollection { get; } 
            = new ObservableCollection<OnHandDto>();
        #endregion

        private void ViewLog(object obj)
        {
            throw new NotImplementedException();
        }

        public void LoadDataGrid()
        {
            OnHandCollection.Clear();
            foreach (var onHand in _onHandRepository
                .GetOnHandDataGrid(SearchKeyword, CurrentPage, DataGridRows))
                OnHandCollection.Add(onHand);
            UpdateTotalPage();
        }
        
        private void UpdateTotalPage()
        {
            TotalPage = _onHandRepository
                .GetOnHandTotalPage(SearchKeyword, DataGridRows);
        }

        private void NextPageAction(object parameter)
        {
            IncrementCurrentPage();
            LoadDataGrid();
        }
        private bool IsNextPageCanExecute(object parameter) => !(CurrentPage >= TotalPage);

        private void PrevPageAction(object parameter)
        {
            DecrementCurrentPage();
            LoadDataGrid();
        }
        private bool IsPrevPageCanExecute(object parameter) => !(CurrentPage <= 1);

        private void SearchAction(object obj)
        {
            ResetCurrentPage();
            LoadDataGrid();
        }
        private void ResetCurrentPage() => CurrentPage = 1;
        private void IncrementCurrentPage() => CurrentPage = CurrentPage + 1;
        private void DecrementCurrentPage() => CurrentPage = CurrentPage - 1;
    }
}
