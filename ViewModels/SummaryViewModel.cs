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
    public class SummaryViewModel : ViewModelBase
    {
        // Fields
        private IEnumerable<MedicalSummary> _medicalSummaryList;
        private IEnumerable<ProcedureSummary> _procedureSummaryList;
        private IEnumerable<LabSummary> _labSummaryList;
        private Report _medicalReportSummary;
        private Report _procedureReportSummary;
        private Report _labReportSummary;
        private Report _fullReportSummary;
        private string _noResultsMed;
        private string _noResultsPro;
        private string _noResultsLab;
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

        public string NoResultsMed
        {
            get => _noResultsMed;
            set { _noResultsMed = value; OnPropertyChanged(nameof(NoResultsMed)); }
        }

        public string NoResultsLab
        {
            get => _noResultsLab;
            set { _noResultsLab = value; OnPropertyChanged(nameof(NoResultsLab)); }
        }

        public string NoResultsPro
        {
            get => _noResultsPro;
            set { _noResultsPro = value; OnPropertyChanged(nameof(NoResultsPro)); }
        }

        // Commands
        public ICommand LoadReportResults { get; }

        // Constructor
        public SummaryViewModel()
        {
            summaryRepository = new SummaryRepository();
            MedicalSummaryList = new List<MedicalSummary>();
            MedicalReportSummary = new Report();
            LoadMedicalSummaryList(StartDate, EndDate);
            LoadProcedureSummaryList(StartDate, EndDate);
            LoadLabSummaryList(StartDate, EndDate);
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
            if (MedicalSummaryList.Any())
            {
                NoResultsMed = "Hidden";
            }
            else
            {
                NoResultsMed = "Visible";
            }
        }

        private async void LoadProcedureSummaryList(DateTime startDate, DateTime endDate)
        {
            IEnumerable<ProcedureSummary> summaries = await summaryRepository.GetProcedureSummary(StartDate, EndDate);
            ProcedureSummaryList = summaries;
            Report report = await summaryRepository.GenerateReport(summaries, StartDate, EndDate);
            ProcedureReportSummary = report;
            if (ProcedureSummaryList.Any())
            {
                NoResultsPro = "Hidden";
            }
            else
            {
                NoResultsPro = "Visible";
            }
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
            }
            else
            {
                NoResultsLab = "Visible";
            }
        }

        private void LoadAllSummaryList()
        {
            if (MedicalReportSummary != null && ProcedureReportSummary != null && LabReportSummary != null)
            {
                Report report = summaryRepository.GenerateFullReport(MedicalReportSummary, ProcedureReportSummary, LabReportSummary);
                FullReportSummary = report;
            }
        }

        private void ExecuteSearchCommand(object obj)
        {
            LoadMedicalSummaryList(StartDate, EndDate);
            LoadProcedureSummaryList(StartDate, EndDate);
            LoadLabSummaryList(StartDate, EndDate);
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
