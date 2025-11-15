using iText.Kernel.Pdf.Canvas.Parser.ClipperLib;
using QWellApp.Enums;
using QWellApp.Helpers;
using QWellApp.Models;
using QWellApp.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace QWellApp.ViewModels.Common
{
    public abstract class BaseSummaryViewModel : ViewModelBase, ISummaryViewModel
    {
        // Fields
        private static DateTime _sharedStartDate = DateTime.Today;
        private static DateTime _sharedEndDate = DateTime.Today;
        private static int _sharedStartTime = 1;
        private static int _sharedEndTime = 1;
        private IEnumerable<KeyValuePair<int, string>> _timeframeList;
        private IEnumerable<MedicalSummary> _medicalSummaryList;
        private IEnumerable<ProcedureSummary> _procedureSummaryList;
        private IEnumerable<LabSummary> _labSummaryList;
        private IEnumerable<ChannelSummary> _channelSummaryList;
        private Report _medicalReportSummary;
        private Report _procedureReportSummary;
        private Report _labReportSummary;
        private Report _channelReportSummary;
        private Report _fullReportSummary;
        private string _noResultsMed;
        private string _sumOfTotalMed;
        private string _noResultsPro;
        private string _sumOfTotalPro;
        private string _noResultsLab;
        private string _sumOfTotalLab;
        private string _noResultsCha;
        private string _sumOfTotalCha;
        private DateTime _previousStartDate;
        private DateTime _previousEndDate;
        private DateTime _startDateTime;
        private DateTime _endDateTime;
        private bool _summaryListVisibility = true;
        private bool _generateReportButtonVisibility = false;
        private bool _downloadButtonVisibility = false;

        protected readonly SummaryRepository summaryRepository;
        protected readonly Validation validator;

        // Properties
        public DateTime StartDate
        {
            get => _sharedStartDate;
            set
            {
                if (_sharedStartDate != value)
                {
                    _sharedStartDate = value;
                    OnPropertyChanged(nameof(StartDate));
                    LoadAllSummaries(); // Auto reload if required
                }
            }
        }

        public DateTime EndDate
        {
            get => _sharedEndDate;
            set
            {
                if (_sharedEndDate != value)
                {
                    _sharedEndDate = value;
                    OnPropertyChanged(nameof(EndDate));
                    LoadAllSummaries();
                }
            }
        }

        public int StartTime
        {
            get => _sharedStartTime;
            set
            {
                if (_sharedStartTime != value)
                {
                    _sharedStartTime = value;
                    OnPropertyChanged(nameof(StartTime));
                    LoadAllSummaries();
                }
            }
        }

        public int EndTime
        {
            get => _sharedEndTime;
            set
            {
                if (_sharedEndTime != value)
                {
                    _sharedEndTime = value;
                    OnPropertyChanged(nameof(EndTime));
                    LoadAllSummaries();
                }
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

        public IEnumerable<MedicalSummary> MedicalSummaryList
        {
            get => _medicalSummaryList;
            set
            {
                _medicalSummaryList = value;
                OnPropertyChanged(nameof(MedicalSummaryList));
            }
        }

        public IEnumerable<ProcedureSummary> ProcedureSummaryList
        {
            get => _procedureSummaryList;
            set
            {
                _procedureSummaryList = value;
                OnPropertyChanged(nameof(ProcedureSummaryList));
            }
        }

        public IEnumerable<LabSummary> LabSummaryList
        {
            get => _labSummaryList;
            set
            {
                _labSummaryList = value;
                OnPropertyChanged(nameof(LabSummaryList));
            }
        }

        public IEnumerable<ChannelSummary> ChannelSummaryList
        {
            get => _channelSummaryList;
            set
            {
                _channelSummaryList = value;
                OnPropertyChanged(nameof(ChannelSummaryList));
            }
        }

        public Report MedicalReportSummary
        {
            get => _medicalReportSummary;
            set
            {
                _medicalReportSummary = value;
                OnPropertyChanged(nameof(MedicalReportSummary));
            }
        }

        public Report ProcedureReportSummary
        {
            get => _procedureReportSummary;
            set
            {
                _procedureReportSummary = value;
                OnPropertyChanged(nameof(ProcedureReportSummary));
            }
        }
        
        public Report LabReportSummary
        {
            get => _labReportSummary;
            set
            {
                _labReportSummary = value;
                OnPropertyChanged(nameof(LabReportSummary));
            }
        }

        public Report ChannelReportSummary
        {
            get => _channelReportSummary;
            set
            {
                _channelReportSummary = value;
                OnPropertyChanged(nameof(ChannelReportSummary));
            }
        }

        public Report FullReportSummary
        {
            get => _fullReportSummary;
            set
            {
                _fullReportSummary = value;
                OnPropertyChanged(nameof(FullReportSummary));
            }
        }

        public string NoResultsMed
        {
            get => _noResultsMed;
            set 
            { 
                _noResultsMed = value; 
                OnPropertyChanged(nameof(NoResultsMed)); 
            }
        }

        public string SumOfTotalMed
        {
            get => _sumOfTotalMed;
            set { _sumOfTotalMed = value; OnPropertyChanged(nameof(SumOfTotalMed)); }
        }

        public string NoResultsPro
        {
            get => _noResultsPro;
            set 
            { 
                _noResultsPro = value; 
                OnPropertyChanged(nameof(NoResultsPro)); 
            }
        }

        public string SumOfTotalPro
        {
            get => _sumOfTotalPro;
            set { _sumOfTotalPro = value; OnPropertyChanged(nameof(SumOfTotalPro)); }
        }

        public string NoResultsLab
        {
            get => _noResultsLab;
            set 
            { 
                _noResultsLab = value; 
                OnPropertyChanged(nameof(NoResultsLab)); 
            }
        }

        public string SumOfTotalLab
        {
            get => _sumOfTotalLab;
            set 
            { 
                _sumOfTotalLab = value; 
                OnPropertyChanged(nameof(SumOfTotalLab)); 
            }
        }

        public string NoResultsCha
        {
            get => _noResultsCha;
            set 
            { 
                _noResultsCha = value; 
                OnPropertyChanged(nameof(NoResultsCha)); 
            }
        }

        public string SumOfTotalCha
        {
            get => _sumOfTotalCha;
            set 
            { 
                _sumOfTotalCha = value; 
                OnPropertyChanged(nameof(SumOfTotalCha)); 
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

        public bool SummaryListVisibility
        {
            get => _summaryListVisibility;
            set 
            { 
                _summaryListVisibility = value; 
                OnPropertyChanged(nameof(SummaryListVisibility)); 
            }
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
        public ICommand InitializeCommand { get; }

        protected BaseSummaryViewModel()
        {
            summaryRepository = new SummaryRepository();
            validator = new Validation();
            TimeframeList = EnumHelper.GetIntEnumDescriptionList<TimeframeListEnum>();

            // Store initial valid values
            _previousStartDate = StartDate;
            _previousEndDate = EndDate;

            InitializeCommand = new RelayCommand(async _ => await LoadAllSummaries());
            InitializeCommand.Execute(null);
            ButtonVisibility();
        }

        public async Task LoadAllSummaries()
        {
            StartDateTime = validator.CalculateStartDateTime(StartDate, StartTime);
            EndDateTime = validator.CalculateEndDateTime(EndDate, EndTime);

            if (StartDateTime > EndDateTime)
            {   
                MessageBox.Show("Start date & time should be less than end date & time");

                // time validation is done in xaml.cs files
                _sharedStartDate = _previousStartDate;
                _sharedEndDate = _previousEndDate;

                OnPropertyChanged(nameof(StartDate));
                OnPropertyChanged(nameof(EndDate));

                return;
            }

            // Update previous after validation success
            _previousStartDate = StartDate;
            _previousEndDate = EndDate;

            await LoadMedicalSummaryList();
            await LoadProcedureSummaryList();
            await LoadLabSummaryList();
            await LoadChannelSummaryList();

            LoadFullReportSummary();
        }

        public bool CanExecuteForAdminsCommand(object obj = null)
        {
            var validUser = Properties.Settings.Default.Username;
            var validEmployeeType = Properties.Settings.Default.EmployeeType;
            var validStatus = Properties.Settings.Default.Status;

            return validUser != null &&
                   (validEmployeeType == EmployeeTypeEnum.Admin.ToString() ||
                    validEmployeeType == EmployeeTypeEnum.Manager.ToString()) &&
                   validStatus == UserStatusEnum.Active.ToString();
        }

        protected async Task LoadMedicalSummaryList()
        {
            MedicalSummaryList = await summaryRepository.GetMedicalSummary(StartDateTime, EndDateTime);
            Report report = await summaryRepository.GenerateReport(MedicalSummaryList, StartDateTime, EndDateTime);
            MedicalReportSummary = report;
            if (MedicalSummaryList.Any())
            {
                NoResultsMed = "Hidden";
                SumOfTotalMed = $"Total: {MedicalReportSummary.TotalIncome ?? 0:F2}";
            }
            else
            {
                NoResultsMed = "Visible";
                SumOfTotalMed = $"Total: {0:F2}";
            }
        }

        protected async Task LoadProcedureSummaryList()
        {
            ProcedureSummaryList = await summaryRepository.GetProcedureSummary(StartDateTime, EndDateTime);
            Report report = await summaryRepository.GenerateReport(ProcedureSummaryList, StartDateTime, EndDateTime);
            ProcedureReportSummary = report;
            if (ProcedureSummaryList.Any())
            {
                NoResultsPro = "Hidden";
                SumOfTotalPro = $"Total: {ProcedureReportSummary.TotalIncome ?? 0:F2}";
            }
            else
            {
                NoResultsPro = "Visible";
                SumOfTotalPro = $"Total: {0:F2}";
            }
        }

        protected async Task LoadLabSummaryList()
        {
            LabSummaryList = await summaryRepository.GetLabSummary(StartDateTime, EndDateTime);
            Report report = await summaryRepository.GenerateReport(LabSummaryList, StartDateTime, EndDateTime);
            LabReportSummary = report;
            if (LabSummaryList.Any())
            {
                NoResultsLab = "Hidden";
                SumOfTotalLab = $"Total: {LabReportSummary.TotalIncome ?? 0:F2}";
            }
            else
            {
                NoResultsLab = "Visible";
                SumOfTotalLab = $"Total: {0:F2}";
            }
        }

        protected async Task LoadChannelSummaryList()
        {
            ChannelSummaryList = await summaryRepository.GetChannelSummary(StartDateTime, EndDateTime);
            Report report = await summaryRepository.GenerateReport(ChannelSummaryList, StartDateTime, EndDateTime);
            ChannelReportSummary = report;
            if (ChannelSummaryList.Any())
            {
                NoResultsCha = "Hidden";
                SumOfTotalCha = $"Total: {ChannelReportSummary.TotalIncome ?? 0:F2}";
            }
            else
            {
                NoResultsCha = "Visible";
                SumOfTotalCha = $"Total: {0:F2}";
            }
        }

        protected void LoadFullReportSummary()
        {
            // Get medical,procedure,lab,channel records list to show in the UI and in the downloaded pdf 1st table
            FullReportSummary =
                summaryRepository.GenerateFullReport(
                    MedicalReportSummary, ProcedureReportSummary, LabReportSummary, ChannelReportSummary
                );
        }

        private void ButtonVisibility()
        {
            var valideUser = Properties.Settings.Default.Username;
            var validEmployeeType = Properties.Settings.Default.EmployeeType;
            var validStatus = Properties.Settings.Default.Status;

            if (valideUser != null && (validEmployeeType.Equals(EmployeeTypeEnum.Admin.ToString()) || validEmployeeType.Equals(EmployeeTypeEnum.Manager.ToString())) && validStatus.Equals(UserStatusEnum.Active.ToString()))
            {
                SummaryListVisibility = true;
                GenerateReportButtonVisibility = true;
                DownloadButtonVisibility = true;
            }
            else
            {
                SummaryListVisibility = false;
                GenerateReportButtonVisibility = false;
                DownloadButtonVisibility = false;
            }
        }
    }
}
