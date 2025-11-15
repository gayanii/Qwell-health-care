using Azure.Core.GeoJson;
using iText.Kernel.Pdf.Canvas.Parser.ClipperLib;
using QWellApp.Enums;
using QWellApp.Helpers;
using QWellApp.Models;
using QWellApp.Repositories;
using QWellApp.ViewModels.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
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
        private int _startTime = 1;
        private int _endTime = 1;
        private string _dates;
        private string _firstName;
        private string _lastName;
        private string _role;
        private string _chitNumbers;
        private float _totCommission;
        private DateTime _startDateTime;
        private DateTime _endDateTime;
        private DateTime _previousStartDate;
        private DateTime _previousEndDate;
        private bool _commissionListVisibility = true;
        private bool _generateReportButtonVisibility = false;
        private bool _downloadButtonVisibility = true;
        private IEnumerable<KeyValuePair<int, string>> _timeframeList;

        private ICommissionRepository commissionRepository;
        private IUserRepository userRepository;
        protected readonly Validation validator;

        // Properties
        public IEnumerable<Commission> CommissionList
        {
            get => _commissionList;
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
            get => _startDate;
            set
            {
                if (_startDate != value)
                {
                    _startDate = value;
                    OnPropertyChanged(nameof(StartDate));
                    LoadCommissionList();
                }
            }
        }

        public DateTime EndDate
        {
            get => _endDate; 
            set
            {
                if (_endDate != value)
                {
                    _endDate = value;
                    OnPropertyChanged(nameof(EndDate));
                    LoadCommissionList();
                }
            }
        }

        public int StartTime
        {
            get => _startTime;
            set
            {
                if (_startTime != value)
                {
                    _startTime = value;
                    OnPropertyChanged(nameof(StartTime));
                    LoadCommissionList();
                }
            }
        }

        public int EndTime
        {
            get => _endTime;
            set
            {
                if (_endTime != value)
                {
                    _endTime = value;
                    OnPropertyChanged(nameof(EndTime));
                    LoadCommissionList();
                }
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

        public DateTime StartDateTime
        {
            get => _startDateTime;
            set
            {
                _startDateTime = value;
                OnPropertyChanged(nameof(StartDateTime));
            }
        }

        public DateTime EndDateTime
        {
            get => _endDateTime;
            set
            {
                _endDateTime = value;
                OnPropertyChanged(nameof(EndDateTime));
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

        public IEnumerable<KeyValuePair<int, string>> TimeframeList
        {
            get => _timeframeList;
            private set
            {
                _timeframeList = value;
                OnPropertyChanged(nameof(TimeframeList));
            }
        }

        // Commands
        public ICommand LoadCommissionResults { get; }

        // Constructor
        public CommissionViewModel()
        {
            commissionRepository = new CommissionRepository();
            userRepository = new UserRepository();
            validator = new Validation();

            TimeframeList = EnumHelper.GetIntEnumDescriptionList<TimeframeListEnum>();

            // Store initial valid values
            _previousStartDate = StartDate;
            _previousEndDate = EndDate;

            LoadCommissionList();
            LoadCommissionResults = new RelayCommand(ExecuteSearchCommand, CanExecuteForAdminsCommand);
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

        private async Task LoadCommissionList()
        {
            StartDateTime = validator.CalculateStartDateTime(StartDate, StartTime);
            EndDateTime = validator.CalculateEndDateTime(EndDate, EndTime);

            if (StartDateTime > EndDateTime)
            {
                
                MessageBox.Show("Start date & time should be less than end date & time");

                // time validation is done in xaml.cs files
                StartDate = _previousStartDate;
                EndDate = _previousEndDate;

                OnPropertyChanged(nameof(StartDate));
                OnPropertyChanged(nameof(EndDate));

                return;
            }

            // Update previous after validation success
            _previousStartDate = StartDate;
            _previousEndDate = EndDate;

            IEnumerable<Commission> commissions = Enumerable.Empty<Commission>();
            if (!System.Windows.Application.Current.Properties.Contains("PageName"))
            {
                return;
            }
            string pageName = (string)System.Windows.Application.Current.Properties["PageName"];
            if (pageName == "Medical Commissions")
            {
                commissions = await commissionRepository.GetMedicalCommissions(StartDateTime, EndDateTime);
            }
            if (pageName == "Lab Commissions")
            {
                commissions = await commissionRepository.GetLabCommissions(StartDateTime, EndDateTime);
            }
            if (pageName == "Procedure Commissions")
            {
                commissions = await commissionRepository.GetProcedureCommissions(StartDateTime, EndDateTime);
            }
            if (pageName == "Chanelling Commissions")
            {
                commissions = await commissionRepository.GetChannelCommissions(StartDateTime, EndDateTime);
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
            LoadCommissionList();
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
