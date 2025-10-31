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

namespace QWellApp.ViewModels
{
    public class LabSummaryViewModel : ViewModelBase, SummaryViewModel
    {
        // Fields
        private IEnumerable<MedicalSummary> _medicalSummaryList;
        private IEnumerable<ProcedureSummary> _procedureSummaryList;
        private IEnumerable<LabSummary> _labSummaryList;
        private IEnumerable<ChannelSummary> _channelSummaryList;
        private Report _medicalReportSummary;
        private Report _procedureReportSummary;
        private Report _labReportSummary;
        private Report _channelReportSummary;
        private Report _fullReportSummary;
        private string _noResultsLab;
        private string _sumOfTotalLab;
        private DateTime _startDate = DateTime.Today;
        private DateTime _endDate = DateTime.Today;
        private int _selectedId;
        private DateTime _summaryDate;
        private string _chitNumber;
        private DateTime _admitDate;
        private float? _opdCharge;
        private float? _pharmacyBill;
        private float? _procedureBill;
        private float? _labPaid;
        private float? _amount;
        private bool _summaryListVisibility = true;
        private bool _generateReportButtonVisibility = false;
        private bool _downloadButtonVisibility = false;

        private ISummaryRepository summaryRepository;

        // Properties
        public IEnumerable<MedicalSummary> MedicalSummaryList
        {
            get
            {
                return _medicalSummaryList;
            }
            set
            {
                _medicalSummaryList = value;
                OnPropertyChanged(nameof(MedicalSummaryList));
            }
        }
        public IEnumerable<ProcedureSummary> ProcedureSummaryList
        {
            get
            {
                return _procedureSummaryList;
            }
            set
            {
                _procedureSummaryList = value;
                OnPropertyChanged(nameof(ProcedureSummaryList));
            }
        }
        public IEnumerable<LabSummary> LabSummaryList
        {
            get
            {
                return _labSummaryList;
            }
            set
            {
                _labSummaryList = value;
                OnPropertyChanged(nameof(LabSummaryList));
            }
        }
        public IEnumerable<ChannelSummary> ChannelSummaryList
        {
            get
            {
                return _channelSummaryList;
            }
            set
            {
                _channelSummaryList = value;
                OnPropertyChanged(nameof(ChannelSummaryList));
            }
        }

        public Report MedicalReportSummary
        {
            get
            {
                return _medicalReportSummary;
            }
            set
            {
                _medicalReportSummary = value;
                OnPropertyChanged(nameof(MedicalReportSummary));
            }
        }
        public Report ProcedureReportSummary
        {
            get
            {
                return _procedureReportSummary;
            }
            set
            {
                _procedureReportSummary = value;
                OnPropertyChanged(nameof(ProcedureReportSummary));
            }
        }
        public Report LabReportSummary
        {
            get
            {
                return _labReportSummary;
            }
            set
            {
                _labReportSummary = value;
                OnPropertyChanged(nameof(LabReportSummary));
            }
        }
        public Report ChannelReportSummary
        {
            get
            {
                return _channelReportSummary;
            }
            set
            {
                _channelReportSummary = value;
                OnPropertyChanged(nameof(ChannelReportSummary));
            }
        }

        public Report FullReportSummary
        {
            get
            {
                return _fullReportSummary;
            }
            set
            {
                _fullReportSummary = value;
                OnPropertyChanged(nameof(FullReportSummary));
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

        public DateTime SummaryDate
        {
            get => _summaryDate;
            set { _summaryDate = value; OnPropertyChanged(nameof(SummaryDate)); }
        }

        public string ChitNumber
        {
            get => _chitNumber;
            set { _chitNumber = value; OnPropertyChanged(nameof(ChitNumber)); }
        }

        public DateTime AdmitDate
        {
            get => _admitDate;
            set { _admitDate = value; OnPropertyChanged(nameof(AdmitDate)); }
        }

        public float? OPDCharge
        {
            get => _opdCharge;
            set { _opdCharge = value; OnPropertyChanged(nameof(OPDCharge)); }
        }

        public float? PharmacyBill
        {
            get => _pharmacyBill;
            set { _pharmacyBill = value; OnPropertyChanged(nameof(PharmacyBill)); }
        }

        public float? ProcedureBill
        {
            get { return _procedureBill; }
            set
            {
                _procedureBill = value;
                OnPropertyChanged(nameof(ProcedureBill));
            }
        }

        public float? LabPaid
        {
            get => _labPaid;
            set { _labPaid = value; OnPropertyChanged(nameof(LabPaid)); }
        }

        public float? Amount
        {
            get => _amount;
            set { _amount = value; OnPropertyChanged(nameof(Amount)); }
        }


        public bool SummaryListVisibility
        {
            get => _summaryListVisibility;
            set { _summaryListVisibility = value; OnPropertyChanged(nameof(SummaryListVisibility)); }
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
        public string NoResultsLab
        {
            get => _noResultsLab;
            set { _noResultsLab = value; OnPropertyChanged(nameof(NoResultsLab)); }
        }
        public string SumOfTotalLab
        {
            get => _sumOfTotalLab;
            set { _sumOfTotalLab = value; OnPropertyChanged(nameof(SumOfTotalLab)); }
        }

        // Commands
        public ICommand LoadReportResults { get; }

        // Constructor
        public LabSummaryViewModel()
        {
            summaryRepository = new SummaryRepository();
            MedicalSummaryList = new List<MedicalSummary>();
            MedicalReportSummary = new Report();
            LoadMedicalSummaryList(StartDate, EndDate);
            LoadProcedureSummaryList(StartDate, EndDate);
            LoadLabSummaryList(StartDate, EndDate);
            LoadChannelSummaryList(StartDate, EndDate);
            LoadReportResults = new RelayCommand(ExecuteSearchCommand, CanExecuteForAdminsCommand);
            ButtonVisibility();
        }


        private bool CanExecuteForAdminsCommand(object obj)
        {
            var valideUser = Properties.Settings.Default.Username;
            var validEmployeeType = Properties.Settings.Default.EmployeeType;
            var validStatus = Properties.Settings.Default.Status;

            LoadAllSummaryList();

            if (valideUser != null && (validEmployeeType.Equals(EmployeeTypeEnum.Admin.ToString()) || validEmployeeType.Equals(EmployeeTypeEnum.Manager.ToString())) && validStatus.Equals(UserStatusEnum.Active.ToString()))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private async void LoadMedicalSummaryList(DateTime startDate, DateTime endDate)
        {
            IEnumerable<MedicalSummary> summaries = await summaryRepository.GetMedicalSummary(StartDate, EndDate);
            MedicalSummaryList = summaries;
            Report report = await summaryRepository.GenerateReport(summaries, StartDate, EndDate);
            MedicalReportSummary = report;
        }

        private async void LoadProcedureSummaryList(DateTime startDate, DateTime endDate)
        {
            IEnumerable<ProcedureSummary> summaries = await summaryRepository.GetProcedureSummary(StartDate, EndDate);
            ProcedureSummaryList = summaries;
            Report report = await summaryRepository.GenerateReport(summaries, StartDate, EndDate);
            ProcedureReportSummary = report;
        }

        private async void LoadLabSummaryList(DateTime startDate, DateTime endDate)
        {
            IEnumerable<LabSummary> summaries = await summaryRepository.GetLabSummary(StartDate, EndDate);
            LabSummaryList = summaries;
            Report report = await summaryRepository.GenerateReport(summaries, StartDate, EndDate);
            LabReportSummary = report;
            if (LabSummaryList.Any())
            {
                NoResultsLab = "Hidden";
                SumOfTotalLab = $"Total: {LabReportSummary.TotalIncome ?? 0:F2}";
            }
            else
            {
                NoResultsLab = "Visible";
            }
        }

        private async void LoadChannelSummaryList(DateTime startDate, DateTime endDate)
        {
            IEnumerable<ChannelSummary> summaries = await summaryRepository.GetChannelSummary(StartDate, EndDate);
            ChannelSummaryList = summaries;
            Report report = await summaryRepository.GenerateReport(summaries, StartDate, EndDate);
            ChannelReportSummary = report;
        }

        private void LoadAllSummaryList()
        {
            if (MedicalReportSummary != null && ProcedureReportSummary != null && LabReportSummary != null && ChannelReportSummary != null)
            {
                Report report = summaryRepository.GenerateFullReport(MedicalReportSummary, ProcedureReportSummary, LabReportSummary, ChannelReportSummary);
                FullReportSummary = report;
            }
        }

        private void ExecuteSearchCommand(object obj)
        {
            LoadMedicalSummaryList(StartDate, EndDate);
            LoadProcedureSummaryList(StartDate, EndDate);
            LoadLabSummaryList(StartDate, EndDate);
            LoadChannelSummaryList(StartDate, EndDate);
            LoadAllSummaryList();
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
