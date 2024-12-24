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
    public class CommissionViewModel : ViewModelBase
    {
        // Fields
        private IEnumerable<Commission> _commissionList;
        private string _noResults;
        private int _selectedId;
        private DateTime _startDate = DateTime.Today;
        private DateTime _endDate = DateTime.Today;
        private string _dates;
        private string _firstName;
        private string _lastName;
        private string _role;
        private string _chitNumbers;
        private float _totCommission;
        private bool _commissionListVisibility = true;
        private bool _generateReportButtonVisibility = false;
        private bool _downloadButtonVisibility = true;

        private ICommissionRepository commissionRepository;
        private IUserRepository userRepository;

        // Properties
        public IEnumerable<Commission> CommissionList
        {
            get
            {
                return _commissionList;
            }
            set
            {
                _commissionList = value;
                OnPropertyChanged(nameof(CommissionList));
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

        public string Dates
        {
            get { return _dates; }
            set
            {
                _dates = value;
                OnPropertyChanged(nameof(Dates));
            }
        }

        public string FirstName
        {
            get { return _firstName; }
            set
            {
                _firstName = value;
                OnPropertyChanged(nameof(FirstName));
            }
        }

        public string LastName
        {
            get { return _lastName; }
            set
            {
                _lastName = value;
                OnPropertyChanged(nameof(LastName));
            }
        }

        public string Role
        {
            get { return _role; }
            set
            {
                _role = value;
                OnPropertyChanged(nameof(Role));
            }
        }

        public string ChitNumbers
        {
            get { return _chitNumbers; }
            set
            {
                _chitNumbers = value;
                OnPropertyChanged(nameof(ChitNumbers));
            }
        }

        public float TotCommission
        {
            get { return _totCommission; }
            set
            {
                _totCommission = value;
                OnPropertyChanged(nameof(TotCommission));
            }
        }


        public string NoResults
        {
            get => _noResults;
            set { _noResults = value; OnPropertyChanged(nameof(NoResults)); }
        }
        public bool CommissionListVisibility
        {
            get => _commissionListVisibility;
            set { _commissionListVisibility = value; OnPropertyChanged(nameof(CommissionListVisibility)); }
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
        public ICommand LoadCommissionResults { get; }

        // Constructor
        public CommissionViewModel()
        {
            commissionRepository = new CommissionRepository();
            userRepository = new UserRepository();
            CommissionList = new List<Commission>();
            LoadCommissionList(StartDate, EndDate);
            LoadCommissionResults = new RelayCommand(ExecuteSearchCommand, CanExecuteForAdminsCommand);
            ButtonVisibility();
        }

        private bool CanExecuteForAdminsCommand(object obj)
        {
            var valideUser = Properties.Settings.Default.Username;
            var validEmployeeType = Properties.Settings.Default.EmployeeType;
            var validStatus = Properties.Settings.Default.Status;
            //LoadCommissionList(StartDate, EndDate);

            if (valideUser != null && (validEmployeeType.Equals(EmployeeTypeEnum.Admin.ToString()) || validEmployeeType.Equals(EmployeeTypeEnum.Manager.ToString())) && validStatus.Equals(UserStatusEnum.Active.ToString()))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private async void LoadCommissionList(DateTime startDate, DateTime endDate)
        {
            IEnumerable<Commission> commissions = Enumerable.Empty<Commission>();
            if (!System.Windows.Application.Current.Properties.Contains("PageName"))
            {
                return;
            }
            string pageName = (string)System.Windows.Application.Current.Properties["PageName"];
            if (pageName == "Medical Commissions")
            {
                commissions = await commissionRepository.GetMedicalCommissions(startDate, endDate);
            }
            if (pageName == "Lab Commissions")
            {
                commissions = await commissionRepository.GetLabCommissions(startDate, endDate);
            }
            if (pageName == "Procedure Commissions")
            {
                commissions = await commissionRepository.GetProcedureCommissions(startDate, endDate);
            }
            CommissionList = commissions;
            //Report report = await summaryRepository.GenerateReport(summaries, date);
            //ReportSummary = report;
            if (CommissionList.Any())
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
            LoadCommissionList(StartDate, EndDate);
        }

        private void ButtonVisibility()
        {
            var valideUser = Properties.Settings.Default.Username;
            var validEmployeeType = Properties.Settings.Default.EmployeeType;
            var validStatus = Properties.Settings.Default.Status;

            if (valideUser != null && (validEmployeeType.Equals(EmployeeTypeEnum.Admin.ToString()) || validEmployeeType.Equals(EmployeeTypeEnum.Manager.ToString())) && validStatus.Equals(UserStatusEnum.Active.ToString()))
            {
                CommissionListVisibility = true;
                GenerateReportButtonVisibility = true;
                DownloadButtonVisibility = true;
            }
            else
            {
                CommissionListVisibility = false;
                GenerateReportButtonVisibility = false;
                DownloadButtonVisibility = false;
            }
        }
    }
}
