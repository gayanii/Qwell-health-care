using Azure.Core.GeoJson;
using QWellApp.Enums;
using QWellApp.Models;
using QWellApp.Repositories;
using QWellApp.ViewModels.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;
using static System.Runtime.InteropServices.JavaScript.JSType;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ListView;

namespace QWellApp.ViewModels
{
    public class StockViewModel : ViewModelBase
    {
        // Fields
        private IEnumerable<Stock> _stockList;
        private string _noResults;
        private int _selectedId;
        private DateTime _startDate = DateTime.Today;
        private DateTime _endDate = DateTime.Today;
        private string _brandName;
        private string _generic;
        private string _collectedStock;
        private string _soldStock;
        private bool _stockListVisibility = true;
        private bool _generateReportButtonVisibility = false;
        private bool _downloadButtonVisibility = false;

        private IStockRepository stockRepository;
        private IUserRepository userRepository;

        // Properties
        public IEnumerable<Stock> StockList
        {
            get
            {
                return _stockList;
            }
            set
            {
                _stockList = value;
                OnPropertyChanged(nameof(StockList));
            }
        }


        public int SelectedId
        {
            get => _selectedId;
            set { _selectedId = value; OnPropertyChanged(nameof(SelectedId)); }
        }

        public DateTime StartDate
        {
            get { return _startDate; }
            set
            {
                _startDate = value;
                OnPropertyChanged(nameof(StartDate));
            }
        }

        public DateTime EndDate
        {
            get { return _endDate; }
            set
            {
                _endDate = value;
                OnPropertyChanged(nameof(EndDate));
            }
        }

        public string BrandName
        {
            get { return _brandName; }
            set
            {
                _brandName = value;
                OnPropertyChanged(nameof(BrandName));
            }
        }

        public string Generic
        {
            get { return _generic; }
            set
            {
                _generic = value;
                OnPropertyChanged(nameof(Generic));
            }
        }

        public string CollectedStock
        {
            get { return _collectedStock; }
            set
            {
                _collectedStock = value;
                OnPropertyChanged(nameof(CollectedStock));
            }
        }

        public string SoldStock
        {
            get { return _soldStock; }
            set
            {
                _soldStock = value;
                OnPropertyChanged(nameof(SoldStock));
            }
        }

        public string NoResults
        {
            get => _noResults;
            set { _noResults = value; OnPropertyChanged(nameof(NoResults)); }
        }
        public bool StockListVisibility
        {
            get => _stockListVisibility;
            set { _stockListVisibility = value; OnPropertyChanged(nameof(StockListVisibility)); }
        }

        public bool GenerateReportButtonVisibility
        {
            get => _generateReportButtonVisibility;
            set
            {
                _generateReportButtonVisibility = value;
                OnPropertyChanged(nameof(GenerateReportButtonVisibility));
            }
        }

        public bool DownloadButtonVisibility
        {
            get => _downloadButtonVisibility;
            set
            {
                _downloadButtonVisibility = value;
                OnPropertyChanged(nameof(DownloadButtonVisibility));
            }
        }

        // Commands
        public ICommand LoadStockResults { get; }

        // Constructor
        public StockViewModel()
        {
            stockRepository = new StockRepository();
            StockList = new List<Stock>();
            LoadStockList(StartDate, EndDate);
            LoadStockResults = new RelayCommand(ExecuteSearchCommand, CanExecuteForAdminsCommand);
            ButtonVisibility();
        }

        private bool CanExecuteForAdminsCommand(object obj)
        {
            var valideUser = Properties.Settings.Default.Username;
            var validEmployeeType = Properties.Settings.Default.EmployeeType;
            var validStatus = Properties.Settings.Default.Status;

            if (valideUser != null && (validEmployeeType.Equals(EmployeeTypeEnum.Admin.ToString()) || validEmployeeType.Equals(EmployeeTypeEnum.Manager.ToString())) && validStatus.Equals(UserStatusEnum.Active.ToString()))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private async void LoadStockList(DateTime startDate, DateTime endDate)
        {
            IEnumerable<Stock> stocks = await stockRepository.GetStocks(startDate, endDate);
            StockList = stocks;
            //Report report = await summaryRepository.GenerateReport(summaries, date);
            //ReportSummary = report;
            if (StockList.Any())
            {
                NoResults = "Hidden";
            }
            else
            {
                NoResults = "Visible";
            }
        }
        private void ExecuteSearchCommand(object obj)
        {
            LoadStockList(StartDate, EndDate);
        }

        private void ButtonVisibility()
        {
            var valideUser = Properties.Settings.Default.Username;
            var validEmployeeType = Properties.Settings.Default.EmployeeType;
            var validStatus = Properties.Settings.Default.Status;

            if (valideUser != null && (validEmployeeType.Equals(EmployeeTypeEnum.Admin.ToString()) || validEmployeeType.Equals(EmployeeTypeEnum.Manager.ToString())) && validStatus.Equals(UserStatusEnum.Active.ToString()))
            {
                StockListVisibility = true;
                GenerateReportButtonVisibility = true;
                DownloadButtonVisibility = true;
            }
            else
            {
                StockListVisibility = false;
                GenerateReportButtonVisibility = false;
                DownloadButtonVisibility = false;
            }
        }
    }
}
