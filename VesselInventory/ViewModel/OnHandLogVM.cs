using System.Collections.Generic;
using System.Collections.ObjectModel;
using VesselInventory.Filters;
using VesselInventory.Models;
using VesselInventory.Repository;
using VesselInventory.Utility;

namespace VesselInventory.ViewModel
{
    public class OnHandLogVM : ViewModelBase
    {
        public override string Title => "Journal Log";
        public RelayCommand SearchCommand { get; private set; }
        public RelayCommand NextPageCommand { get; private set; }
        public RelayCommand PrevPageCommand { get; private set; }
        private readonly IVesselGoodJournalRepository _vesselGoodJournalRepository;
        public OnHandLogVM(IVesselGoodJournalRepository vesselGoodJournalRepository)
        {
            _vesselGoodJournalRepository = vesselGoodJournalRepository;
            InitializeCommands();
        }

        public void InitializeData(int itemId, string itemDimensionNumber)
        {
            ItemId = itemId;
            ItemDimensionNumber = itemDimensionNumber;
            ResetCurrentPage();
            LoadDataGrid();
        }

        public int ItemId {get; set; }
        public string ItemDimensionNumber {get; set; }

        private void InitializeCommands()
        {
            SearchCommand = new RelayCommand(SearchAction);
            NextPageCommand = new RelayCommand(NextPageAction, IsNextPageCanExecute);
            PrevPageCommand = new RelayCommand(PrevPageAction, IsPrevPageCanExecute);
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

        private string _documentTypeSelected = "_All";
        public string DocumentTypeSelected {
            get
            {
                return _documentTypeSelected;
            }
            set
            {
                _documentTypeSelected = value;
                OnPropertyChanged("DocumentTypeSelected");
            }
        }

        public IList<string> DocumentTypeCollection
        {
            get
            {
                return new List<string> {
                    "_All",
                    "Vessel Good Receive",
                    "Vessel Good Issued",
                    "Vessel Good Return"
                };

            }
        }

        public ObservableCollection<VesselGoodJournal> JournalLogCollection { get; } 
            = new ObservableCollection<VesselGoodJournal>();
        #endregion
        
        private PageFilter PageFilter
        {
            get => new PageFilter
            {
                Search = SearchKeyword,
                NumRows = DataGridRows,
                PageNum = CurrentPage
            };
        }

        private GoodJournalFilter GoodJournalFilter
        {
            get => new GoodJournalFilter
            {
                ItemId = ItemId,
                ItemDimensionNumber = ItemDimensionNumber,
                DocumentType = DocumentTypeSelected
            };
        }
        private void LoadDataGrid()
        {
            JournalLogCollection.Clear();
            foreach (var data in _vesselGoodJournalRepository.GetGoodJournals(GoodJournalFilter, PageFilter))
            {
                JournalLogCollection.Add(data);
            }
            UpdateTotalPage();

        }

        private void UpdateTotalPage()
        {
            TotalPage = _vesselGoodJournalRepository.GetJournalLogTotalPage(GoodJournalFilter, PageFilter);
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
