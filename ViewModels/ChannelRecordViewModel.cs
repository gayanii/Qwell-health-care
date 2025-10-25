using Microsoft.IdentityModel.Tokens;
using QWellApp.Models;
using QWellApp.Repositories;
using QWellApp.ViewModels.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Input;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ListView;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;
using System.Runtime.InteropServices;
using System.Security;
using System.Windows;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Window;
using QWellApp.Enums;
using System.Collections.ObjectModel;
using Newtonsoft.Json;

namespace QWellApp.ViewModels
{
    public class ChannelRecordViewModel : ViewModelBase, INotifyPropertyChanged
    {
        //Fields
        private IEnumerable<ChannelRecordView> _channelRecordList;
        private string _noResults;
        private string _searchWord = "";
        public int _selectedId;
        private string _chitNumber;
        private float _opdCharge;
        private float _otherCharges;
        private float _pharmacyBill;
        private DateTime _admitDate = DateTime.Today;
        private float _totalBill;
        private string _addedBy;
        private string _patient;
        private string _doctor;
        private float _docComm;
        private float _nurse1Comm;
        private float _nurse2Comm;
        private string _nurse1;
        private string _nurse2;
        private float _consultantFee;
        private string _med1;
        private string _med2;
        private string _med3;
        private string _med4;
        private string _med5;
        private string _med6;
        private string _med7;
        private string _med8;
        private string _med9;
        private string _med10;
        private string _med11;
        private string _med12;
        private string _med13;
        private string _med14;
        private string _med15;
        private string _dose1;
        private string _dose2;
        private string _dose3;
        private string _dose4;
        private string _dose5;
        private string _dose6;
        private string _dose7;
        private string _dose8;
        private string _dose9;
        private string _dose10;
        private string _dose11;
        private string _dose12;
        private string _dose13;
        private string _dose14;
        private string _dose15;
        private float _total1;
        private float _total2;
        private float _total3;
        private float _total4;
        private float _total5;
        private float _total6;
        private float _total7;
        private float _total8;
        private float _total9;
        private float _total10;
        private float _total11;
        private float _total12;
        private float _total13;
        private float _total14;
        private float _total15;
        private string _chitNumberErrorMessage;
        private string _pharmacyBillErrorMessage;
        private string _admitDateErrorMessage;
        private string _totalCostErrorMessage;
        private string _patientErrorMessage;
        private string _doctorErrorMessage;
        private ViewModelBase _currentChildView;
        private Dictionary<int, string> _doctorList;
        private Dictionary<int, string> _employeeList;
        private Dictionary<int, string> _patientList;
        private Dictionary<int, string> _nurseList;
        private ObservableCollection<KeyValuePair<int, string>> _medicineList;
        private Dictionary<int, string> _fixedMedicineList;
        private bool _updateButtonVisibility = false;
        private bool _createButtonVisibility = false;
        private bool _deleteButtonVisibility = false;
        private bool _resetUpdateButtonsVisibility = false;
        private bool _updateGridVisibility = false;
        private bool _createGridVisibility = false;
        private bool _channelRecordListVisibility = true;

        private IChannelRecordRepository channelRecordRepository;
        private IUserRepository userRepository;
        private IPatientRepository patientRepository;
        private IProductRepository productRepository;
        private IProductMedicalRecordRepository productMedicalRecordRepository;
        private IActivityLogRepository activityLogRepository;
        private UserDetails currentUser;

        #region Properties
        public int SelectedId
        {
            get
            {
                return _selectedId;
            }

            set
            {
                _selectedId = value;
                OnPropertyChanged(nameof(SelectedId));
            }
        }
        public string ChitNumber
        {
            get { return _chitNumber; }
            set
            {
                _chitNumber = value;
                OnPropertyChanged(nameof(ChitNumber));
            }
        }

        public float OPDCharge
        {
            get { return _opdCharge; }
            set
            {
                _opdCharge = value;
                OnPropertyChanged(nameof(OPDCharge));
            }
        }

        public float OtherCharges
        {
            get { return _otherCharges; }
            set
            {
                _otherCharges = value;
                OnPropertyChanged(nameof(OtherCharges));
            }
        }

        public float PharmacyBill
        {
            get { return _pharmacyBill; }
            set
            {
                _pharmacyBill = value;
                OnPropertyChanged(nameof(PharmacyBill));
            }
        }

        public DateTime AdmitDate
        {
            get { return _admitDate; }
            set
            {
                _admitDate = value;
                OnPropertyChanged(nameof(AdmitDate));
            }
        }

        public float TotalBill
        {
            get { return _totalBill; }
            set
            {
                if (_totalBill != value)
                {
                    _totalBill = value;
                    OnPropertyChanged(nameof(TotalBill));
                }
            }
        }

        public string AddedBy
        {
            get { return _addedBy; }
            set
            {
                _addedBy = value;
                OnPropertyChanged(nameof(AddedBy));
            }
        }

        public string Patient
        {
            get
            {
                return _patient;
            }

            set
            {
                _patient = value;
                OnPropertyChanged(nameof(Patient));
            }
        }

        public string Doctor
        {
            get
            {
                return _doctor;
            }

            set
            {
                _doctor = value;
                OnPropertyChanged(nameof(Doctor));
            }
        }
        public float ConsultantFee
        {
            get { return _consultantFee; }
            set
            {
                if (_consultantFee != value)
                {
                    _consultantFee = value;
                    OnPropertyChanged(nameof(ConsultantFee));
                }
            }
        }
        public float DocComm
        {
            get { return _docComm; }
            set
            {
                if (_docComm != value)
                {
                    _docComm = value;
                    OnPropertyChanged(nameof(DocComm));
                }
            }
        }

        public float Nurse1Comm
        {
            get { return _nurse1Comm; }
            set
            {
                if (_nurse1Comm != value)
                {
                    _nurse1Comm = value;
                    OnPropertyChanged(nameof(Nurse1Comm));
                }
            }
        }

        public float Nurse2Comm
        {
            get { return _nurse2Comm; }
            set
            {
                if (_nurse2Comm != value)
                {
                    _nurse2Comm = value;
                    OnPropertyChanged(nameof(Nurse2Comm));
                }
            }
        }

        public string Nurse1
        {
            get { return _nurse1; }
            set
            {
                if (_nurse1 != value)
                {
                    _nurse1 = value;
                    OnPropertyChanged(nameof(Nurse1));
                }
            }
        }

        public string Nurse2
        {
            get { return _nurse2; }
            set
            {
                if (_nurse2 != value)
                {
                    _nurse2 = value;
                    OnPropertyChanged(nameof(Nurse2));
                }
            }
        }

        public string Med1
        {
            get
            {
                return _med1;
            }

            set
            {
                _med1 = value;
                OnPropertyChanged(nameof(Med1));
            }
        }

        public string Med2
        {
            get
            {
                return _med2;
            }

            set
            {
                _med2 = value;
                OnPropertyChanged(nameof(Med2));
            }
        }

        public string Med3
        {
            get
            {
                return _med3;
            }

            set
            {
                _med3 = value;
                OnPropertyChanged(nameof(Med3));
            }
        }

        public string Med4
        {
            get
            {
                return _med4;
            }

            set
            {
                _med4 = value;
                OnPropertyChanged(nameof(Med4));
            }
        }

        public string Med5
        {
            get
            {
                return _med5;
            }

            set
            {
                _med5 = value;
                OnPropertyChanged(nameof(Med5));
            }
        }

        public string Med6
        {
            get
            {
                return _med6;
            }

            set
            {
                _med6 = value;
                OnPropertyChanged(nameof(Med6));
            }
        }

        public string Med7
        {
            get
            {
                return _med7;
            }

            set
            {
                _med7 = value;
                OnPropertyChanged(nameof(Med7));
            }
        }

        public string Med8
        {
            get
            {
                return _med8;
            }

            set
            {
                _med8 = value;
                OnPropertyChanged(nameof(Med8));
            }
        }

        public string Med9
        {
            get
            {
                return _med9;
            }

            set
            {
                _med9 = value;
                OnPropertyChanged(nameof(Med9));
            }
        }

        public string Med10
        {
            get
            {
                return _med10;
            }

            set
            {
                _med10 = value;
                OnPropertyChanged(nameof(Med10));
            }
        }
        public string Med11
        {
            get { return _med11; }
            set
            {
                _med11 = value;
                OnPropertyChanged(nameof(Med11));
            }
        }

        public string Med12
        {
            get { return _med12; }
            set
            {
                _med12 = value;
                OnPropertyChanged(nameof(Med12));
            }
        }

        public string Med13
        {
            get { return _med13; }
            set
            {
                _med13 = value;
                OnPropertyChanged(nameof(Med13));
            }
        }

        public string Med14
        {
            get { return _med14; }
            set
            {
                _med14 = value;
                OnPropertyChanged(nameof(Med14));
            }
        }

        public string Med15
        {
            get { return _med15; }
            set
            {
                _med15 = value;
                OnPropertyChanged(nameof(Med15));
            }
        }
        public string Dose1
        {
            get
            {
                return _dose1;
            }

            set
            {
                _dose1 = value;
                OnPropertyChanged(nameof(Dose1));
            }
        }

        public string Dose2
        {
            get
            {
                return _dose2;
            }

            set
            {
                _dose2 = value;
                OnPropertyChanged(nameof(Dose2));
            }
        }

        public string Dose3
        {
            get
            {
                return _dose3;
            }

            set
            {
                _dose3 = value;
                OnPropertyChanged(nameof(Dose3));
            }
        }

        public string Dose4
        {
            get
            {
                return _dose4;
            }

            set
            {
                _dose4 = value;
                OnPropertyChanged(nameof(Dose4));
            }
        }

        public string Dose5
        {
            get
            {
                return _dose5;
            }

            set
            {
                _dose5 = value;
                OnPropertyChanged(nameof(Dose5));
            }
        }

        public string Dose6
        {
            get
            {
                return _dose6;
            }

            set
            {
                _dose6 = value;
                OnPropertyChanged(nameof(Dose6));
            }
        }

        public string Dose7
        {
            get
            {
                return _dose7;
            }

            set
            {
                _dose7 = value;
                OnPropertyChanged(nameof(Dose7));
            }
        }

        public string Dose8
        {
            get
            {
                return _dose8;
            }

            set
            {
                _dose8 = value;
                OnPropertyChanged(nameof(Dose8));
            }
        }

        public string Dose9
        {
            get
            {
                return _dose9;
            }

            set
            {
                _dose9 = value;
                OnPropertyChanged(nameof(Dose9));
            }
        }

        public string Dose10
        {
            get
            {
                return _dose10;
            }

            set
            {
                _dose10 = value;
                OnPropertyChanged(nameof(Dose10));
            }
        }
        public string Dose11
        {
            get { return _dose11; }
            set
            {
                _dose11 = value;
                OnPropertyChanged(nameof(Dose11));
            }
        }

        public string Dose12
        {
            get { return _dose12; }
            set
            {
                _dose12 = value;
                OnPropertyChanged(nameof(Dose12));
            }
        }

        public string Dose13
        {
            get { return _dose13; }
            set
            {
                _dose13 = value;
                OnPropertyChanged(nameof(Dose13));
            }
        }

        public string Dose14
        {
            get { return _dose14; }
            set
            {
                _dose14 = value;
                OnPropertyChanged(nameof(Dose14));
            }
        }

        public string Dose15
        {
            get { return _dose15; }
            set
            {
                _dose15 = value;
                OnPropertyChanged(nameof(Dose15));
            }
        }

        public float Total1
        {
            get { return _total1; }
            set
            {
                _total1 = value;
                OnPropertyChanged(nameof(Total1));
            }
        }

        public float Total2
        {
            get { return _total2; }
            set
            {
                _total2 = value;
                OnPropertyChanged(nameof(Total2));
            }
        }

        public float Total3
        {
            get { return _total3; }
            set
            {
                _total3 = value;
                OnPropertyChanged(nameof(Total3));
            }
        }

        public float Total4
        {
            get { return _total4; }
            set
            {
                _total4 = value;
                OnPropertyChanged(nameof(Total4));
            }
        }

        public float Total5
        {
            get { return _total5; }
            set
            {
                _total5 = value;
                OnPropertyChanged(nameof(Total5));
            }
        }

        public float Total6
        {
            get { return _total6; }
            set
            {
                _total6 = value;
                OnPropertyChanged(nameof(Total6));
            }
        }

        public float Total7
        {
            get { return _total7; }
            set
            {
                _total7 = value;
                OnPropertyChanged(nameof(Total7));
            }
        }

        public float Total8
        {
            get { return _total8; }
            set
            {
                _total8 = value;
                OnPropertyChanged(nameof(Total8));
            }
        }

        public float Total9
        {
            get { return _total9; }
            set
            {
                _total9 = value;
                OnPropertyChanged(nameof(Total9));
            }
        }

        public float Total10
        {
            get { return _total10; }
            set
            {
                _total10 = value;
                OnPropertyChanged(nameof(Total10));
            }
        }

        public float Total11
        {
            get { return _total11; }
            set
            {
                _total11 = value;
                OnPropertyChanged(nameof(Total11));
            }
        }

        public float Total12
        {
            get { return _total12; }
            set
            {
                _total12 = value;
                OnPropertyChanged(nameof(Total12));
            }
        }

        public float Total13
        {
            get { return _total13; }
            set
            {
                _total13 = value;
                OnPropertyChanged(nameof(Total13));
            }
        }

        public float Total14
        {
            get { return _total14; }
            set
            {
                _total14 = value;
                OnPropertyChanged(nameof(Total14));
            }
        }

        public float Total15
        {
            get { return _total15; }
            set
            {
                _total15 = value;
                OnPropertyChanged(nameof(Total15));
            }
        }

        public string ChitNumberErrorMessage
        {
            get
            {
                return _chitNumberErrorMessage;
            }

            set
            {
                _chitNumberErrorMessage = value;
                OnPropertyChanged(nameof(ChitNumberErrorMessage));
            }
        }

        public string PharmacyBillErrorMessage
        {
            get
            {
                return _pharmacyBillErrorMessage;
            }

            set
            {
                _pharmacyBillErrorMessage = value;
                OnPropertyChanged(nameof(PharmacyBillErrorMessage));
            }
        }

        public string AdmitDateErrorMessage
        {
            get
            {
                return _admitDateErrorMessage;
            }

            set
            {
                _admitDateErrorMessage = value;
                OnPropertyChanged(nameof(AdmitDateErrorMessage));
            }
        }

        public string TotalCostErrorMessage
        {
            get
            {
                return _totalCostErrorMessage;
            }

            set
            {
                _totalCostErrorMessage = value;
                OnPropertyChanged(nameof(TotalCostErrorMessage));
            }
        }

        public string PatientErrorMessage
        {
            get
            {
                return _patientErrorMessage;
            }

            set
            {
                _patientErrorMessage = value;
                OnPropertyChanged(nameof(PatientErrorMessage));
            }
        }

        public string DoctorErrorMessage
        {
            get
            {
                return _doctorErrorMessage;
            }

            set
            {
                _doctorErrorMessage = value;
                OnPropertyChanged(nameof(DoctorErrorMessage));
            }
        }

        public ViewModelBase CurrentChildView
        {
            get
            {
                return _currentChildView;
            }
            set
            {
                _currentChildView = value;
                OnPropertyChanged(nameof(CurrentChildView));
            }
        }

        public bool UpdateButtonVisibility
        {
            get
            {
                return _updateButtonVisibility;
            }

            set
            {
                _updateButtonVisibility = value;
                OnPropertyChanged(nameof(UpdateButtonVisibility));
            }
        }
        public bool CreateButtonVisibility
        {
            get
            {
                return _createButtonVisibility;
            }

            set
            {
                _createButtonVisibility = value;
                OnPropertyChanged(nameof(CreateButtonVisibility));
            }
        }
        public bool DeleteButtonVisibility
        {
            get
            {
                return _deleteButtonVisibility;
            }

            set
            {
                _deleteButtonVisibility = value;
                OnPropertyChanged(nameof(DeleteButtonVisibility));
            }
        }
        public bool ResetUpdateButtonsVisibility
        {
            get
            {
                return _resetUpdateButtonsVisibility;
            }

            set
            {
                _resetUpdateButtonsVisibility = value;
                OnPropertyChanged(nameof(ResetUpdateButtonsVisibility));
            }
        }
        public bool UpdateGridVisibility
        {
            get
            {
                return _updateGridVisibility;
            }

            set
            {
                _updateGridVisibility = value;
                OnPropertyChanged(nameof(UpdateGridVisibility));
            }
        }
        public bool CreateGridVisibility
        {
            get
            {
                return _createGridVisibility;
            }

            set
            {
                _createGridVisibility = value;
                OnPropertyChanged(nameof(CreateGridVisibility));
            }
        }
        public bool ChannelRecordListVisibility
        {
            get
            {
                return _channelRecordListVisibility;
            }

            set
            {
                _channelRecordListVisibility = value;
                OnPropertyChanged(nameof(ChannelRecordListVisibility));
            }
        }

        public IEnumerable<ChannelRecordView> ChannelRecordList
        {
            get
            {
                return _channelRecordList;
            }
            set
            {
                _channelRecordList = value;
                OnPropertyChanged(nameof(ChannelRecordList));
            }
        }
        public string NoResults
        {
            get
            {
                return _noResults;
            }
            set
            {
                _noResults = value;
                OnPropertyChanged(nameof(NoResults));
            }
        }
        public string SearchWord
        {
            get
            {
                return _searchWord;
            }

            set
            {
                _searchWord = value;
                OnPropertyChanged(nameof(SearchWord));
            }
        }
        public Dictionary<int, string> DoctorList
        {
            get
            {
                return _doctorList;
            }
            set
            {
                _doctorList = value;
                OnPropertyChanged(nameof(DoctorList));
            }
        }
        public Dictionary<int, string> NurseList
        {
            get
            {
                return _nurseList;
            }
            set
            {
                _nurseList = value;
                OnPropertyChanged(nameof(NurseList));
            }
        }
        public Dictionary<int, string> EmployeeList
        {
            get
            {
                return _employeeList;
            }
            set
            {
                _employeeList = value;
                OnPropertyChanged(nameof(EmployeeList));
            }
        }
        public Dictionary<int, string> PatientList
        {
            get
            {
                return _patientList;
            }
            set
            {
                _patientList = value;
                OnPropertyChanged(nameof(PatientList));
            }
        }

        public ObservableCollection<KeyValuePair<int, string>> MedicineList
        {
            get => _medicineList;
            set
            {
                _medicineList = value;
                OnPropertyChanged(nameof(MedicineList));
            }
        }
        public Dictionary<int, string> FixedMedicineList
        {
            get
            {
                return _fixedMedicineList;
            }
            set
            {
                _fixedMedicineList = value;
                OnPropertyChanged(nameof(FixedMedicineList));
            }
        }
        #endregion

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }


        //Commands
        public ICommand LoadSearchResults { get; }
        public ICommand UpdateChannelRecordCommand { get; }
        public ICommand CreateChannelRecordCommand { get; }
        public ICommand GetChannelRecordDetails { get; }
        public ICommand ResetCommand { get; }
        public ICommand CancelCommand { get; }
        public ICommand BackToListCommand { get; }
        public ICommand DeleteCommand { get; }

        public ChannelRecordViewModel()
        {
            ChannelRecordList = new List<ChannelRecordView>();
            channelRecordRepository = new ChannelRecordRepository();
            userRepository = new UserRepository();
            patientRepository = new PatientRepository();
            productRepository = new ProductRepository();
            productMedicalRecordRepository = new ProductMedicalRecordRepository();
            LoadChannelRecordList(SearchWord);
            DoctorList = new Dictionary<int, string>();
            NurseList = new Dictionary<int, string>();
            PatientList = new Dictionary<int, string>();
            EmployeeList = new Dictionary<int, string>();
            //MedicineList = new Dictionary<int, string>();
            FixedMedicineList = new Dictionary<int, string>();
            activityLogRepository = new ActivityLogRepository();
            currentUser = userRepository.GetByUsername(Properties.Settings.Default.Username);
            LoadSearchResults = new RelayCommand(ExecuteSearchCommand, CanExecuteForAllUsersCommand);
            GetChannelRecordDetails = new RelayCommand(ExecuteGetChannelRecordDetailsCommand, CanExecuteGetUserDetailsCommand);
            UpdateChannelRecordCommand = new RelayCommand(ExecuteUpdateCommand, CanExecuteForAdminsCommand);
            CreateChannelRecordCommand = new RelayCommand(ExecuteCreateCommand, CanExecuteCreateCommand);
            ResetCommand = new RelayCommand(ExecuteGetChannelRecordDetailsCommand);
            CancelCommand = new RelayCommand(ExecuteCancelCommand);
            DeleteCommand = new RelayCommand(ExecuteDeleteCommand, CanExecuteDeleteUserCommand);
            BackToListCommand = new RelayCommand(ExecuteBackToListCommand);
            ButtonVisibility();
            LoadPatientList();
            LoadDoctorList();
            LoadNurseList();
            LoadEmployeeList();
            LoadMedicineList();
        }

        private void LoadMedicineList()
        {
            var medicines = new ObservableCollection<KeyValuePair<int, string>>();
            var medicineList = productRepository.GetAll("");
            foreach (var medicine in medicineList)
            {
                medicines.Add(new KeyValuePair<int, string>(medicine.Id, medicine.BrandName + " " + medicine.Generic));
            }
            MedicineList = medicines;
            FixedMedicineList = new Dictionary<int, string>(medicines.ToDictionary(m => m.Key, m => m.Value));
        }

        private void LoadNurseList()
        {
            Dictionary<int, string> nurses = new Dictionary<int, string>();
            var nurseList = userRepository.GetAllNurses();
            foreach (var nurse in nurseList)
            {
                nurses.Add(nurse.Id, nurse.FirstName + " " + nurse.LastName);
            }
            NurseList = nurses;
        }
        private void LoadDoctorList()
        {
            Dictionary<int, string> doctors = new Dictionary<int, string>();
            var doctorList = userRepository.GetAllDoctors();
            foreach (var doctor in doctorList)
            {
                doctors.Add(doctor.Id, doctor.FirstName + " " + doctor.LastName);
            }
            DoctorList = doctors;
        }

        private void LoadPatientList()
        {
            Dictionary<int, string> patients = new Dictionary<int, string>();
            var patientList = patientRepository.GetAll("");
            foreach (var patient in patientList)
            {
                patients.Add(patient.Id, patient.FirstName + " " + patient.LastName);
            }
            PatientList = patients;
        }

        private void LoadEmployeeList()
        {
            Dictionary<int, string> employees = new Dictionary<int, string>();
            var employeeList = userRepository.GetAll("");
            foreach (var employee in employeeList)
            {
                employees.Add(employee.Id, employee.FirstName + " " + employee.LastName);
            }
            EmployeeList = employees;
        }

        private void ExecuteDeleteCommand(object obj)
        {
            var oldData = channelRecordRepository.GetByID(SelectedId);
            var oldMedicine = productMedicalRecordRepository.GetAll(SelectedId, RecordTypeEnum.Channel);
            var deleteSuccess = channelRecordRepository.Remove(SelectedId);
            if (deleteSuccess)
            {
                // Transform oldMedicine list to remove unwanted properties
                var filteredOldMedicineData = oldMedicine.Select(m => new
                {
                    m.Id,
                    m.ProductId,
                    m.Units,
                    m.SoldPrice
                }).ToList();
                // Log the activity
                var log = new ActivityLog
                {
                    AffectedEntity = EntitiesEnum.ChannelRecords,
                    AffectedEntityId = SelectedId,
                    ActionType = ActionTypeEnum.Delete,
                    OldValues = JsonConvert.SerializeObject(oldData) + "\n\nMedicine:\n" + JsonConvert.SerializeObject(filteredOldMedicineData), // Serialize the whole object
                    NewValues = "-"
                };
                activityLogRepository.AddLog(log, currentUser);
                LoadChannelRecordList("");
            }
        }

        private void ExecuteBackToListCommand(object obj)
        {
            UpdateGridVisibility = false;
            ChannelRecordListVisibility = true;
            CreateGridVisibility = false;
            LoadChannelRecordList("");
        }

        private void ExecuteCancelCommand(object obj)
        {
            Med1 = string.Empty;
            Med2 = string.Empty;
            Med3 = string.Empty;
            Med4 = string.Empty;
            Med5 = string.Empty;
            Med6 = string.Empty;
            Med7 = string.Empty;
            Med8 = string.Empty;
            Med9 = string.Empty;
            Med10 = string.Empty;
            Med11 = string.Empty;
            Med12 = string.Empty;
            Med13 = string.Empty;
            Med14 = string.Empty;
            Med15 = string.Empty;
            Dose1 = string.Empty;
            Dose2 = string.Empty;
            Dose3 = string.Empty;
            Dose4 = string.Empty;
            Dose5 = string.Empty;
            Dose6 = string.Empty;
            Dose7 = string.Empty;
            Dose8 = string.Empty;
            Dose9 = string.Empty;
            Dose10 = string.Empty;
            Dose11 = string.Empty;
            Dose12 = string.Empty;
            Dose13 = string.Empty;
            Dose14 = string.Empty;
            Dose15 = string.Empty;
            Total1 = 0;
            Total2 = 0;
            Total3 = 0;
            Total4 = 0;
            Total5 = 0;
            Total6 = 0;
            Total7 = 0;
            Total8 = 0;
            Total9 = 0;
            Total10 = 0;
            Total11 = 0;
            Total12 = 0;
            Total13 = 0;
            Total14 = 0;
            Total15 = 0;
            ChitNumber = string.Empty;
            OPDCharge = 0;
            OtherCharges = 0;
            PharmacyBill = 0;
            AdmitDate = DateTime.Now;
            TotalBill = 0;
            AddedBy = string.Empty;
            Patient = string.Empty;
            Doctor = string.Empty;
            ConsultantFee = 0; 
            DocComm = 0;
            Nurse1Comm = 0;
            Nurse2Comm = 0;
            Nurse1 = string.Empty;
            Nurse2 = string.Empty;

            // clear error msgs
            ChitNumberErrorMessage = "";
            PharmacyBillErrorMessage = "";
            TotalCostErrorMessage = "";
            PatientErrorMessage = "";
            DoctorErrorMessage = "";
        }
        private void ExecuteCreateCommand(object obj)
        {
            if (string.IsNullOrWhiteSpace(ChitNumber) || string.IsNullOrWhiteSpace(PharmacyBill.ToString()) || string.IsNullOrWhiteSpace(TotalBill.ToString()) ||
                string.IsNullOrWhiteSpace(Patient) || string.IsNullOrWhiteSpace(Doctor))
            {
                ChitNumberErrorMessage = (string.IsNullOrWhiteSpace(ChitNumber)) ? "Chit number is required." : "";
                PharmacyBillErrorMessage = (string.IsNullOrWhiteSpace(PharmacyBill.ToString())) ? "Pharmacy bill is required." : "";
                TotalCostErrorMessage = (string.IsNullOrWhiteSpace(TotalBill.ToString())) ? "Total bill is required." : "";
                PatientErrorMessage = (string.IsNullOrWhiteSpace(Patient)) ? "Patient name is required." : "";
                DoctorErrorMessage = (string.IsNullOrWhiteSpace(Doctor)) ? "Doctor name is required." : "";
            }
            else
            {
                if (!System.Windows.Application.Current.Properties.Contains("Username"))
                {
                    return;
                }
                string username = (string)System.Windows.Application.Current.Properties["Username"];
                var user = userRepository.GetByUsername(username);
                Dictionary<int, int> medDoseIds = new Dictionary<int, int>();
                var medArray = new[] { Med1, Med2, Med3, Med4, Med5, Med6, Med7, Med8, Med9, Med10, Med11, Med12, Med13, Med14, Med15 };
                var doseArray = new[] { Dose1, Dose2, Dose3, Dose4, Dose5, Dose6, Dose7, Dose8, Dose9, Dose10, Dose11, Dose12, Dose13, Dose14, Dose15 };
                ChannelRecord createChannelRecord = new ChannelRecord()
                {
                    ChitNumber = ChitNumber,
                    PharmacyBill = PharmacyBill,
                    OPDCharge = OPDCharge,
                    OtherCharges = OtherCharges,
                    AdmitDate = DateTime.Now,
                    TotalBill = TotalBill,
                    PatientId = PatientList.FirstOrDefault(x => x.Value == Patient).Key,
                    DoctorId = DoctorList.FirstOrDefault(x => x.Value == Doctor).Key,
                    AddedBy = user.Id,
                    ConsultantFee = ConsultantFee,
                    DocComm = DocComm,
                    Nurse1Id = NurseList.FirstOrDefault(x => x.Value == Nurse1).Key == 0 ? (int?)null : NurseList.FirstOrDefault(x => x.Value == Nurse1).Key,
                    Nurse2Id = NurseList.FirstOrDefault(x => x.Value == Nurse2).Key == 0 ? (int?)null : NurseList.FirstOrDefault(x => x.Value == Nurse2).Key,
                    Nurse1Comm = Nurse1Comm,
                    Nurse2Comm = Nurse2Comm,
                };

                for (int i= 0 ; i < medArray.Count(); i++)
                {
                    if (!string.IsNullOrWhiteSpace(medArray[i]) && !string.IsNullOrWhiteSpace(doseArray[i])) {
                        var medKey = MedicineList.FirstOrDefault(x => x.Value == medArray[i]).Key;

                        if (medDoseIds.ContainsKey(medKey))
                        {
                            // Append the new dose to the existing dose
                            medDoseIds[medKey] += int.Parse(doseArray[i]);
                        }
                        else
                        {
                            // Add a new entry if the key does not exist
                            medDoseIds.Add(medKey, int.Parse(doseArray[i]));
                        }
                    }
                }
                var createSuccess = channelRecordRepository.Add(createChannelRecord, medDoseIds);
                if (createSuccess)
                {
                    UpdateGridVisibility = false;
                    ChannelRecordListVisibility = true;
                    CreateGridVisibility = false;
                    LoadChannelRecordList("");
                }
            }
        }

        private bool CanExecuteGetUserDetailsCommand(object obj)
        {
            if (SelectedId > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private bool CanExecuteDeleteUserCommand(object obj)
        {
            var valideUser = Properties.Settings.Default.Username;
            var validEmployeeType = Properties.Settings.Default.EmployeeType;
            var validStatus = Properties.Settings.Default.Status;

            if (valideUser != null
                && (validEmployeeType.Equals(EmployeeTypeEnum.Admin.ToString()))
                && validStatus.Equals(UserStatusEnum.Active.ToString()) && SelectedId > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private void ExecuteGetChannelRecordDetailsCommand(object obj)
        {
            ChannelRecord channelRecord = channelRecordRepository.GetByID(SelectedId);
            IEnumerable<ProductMedicalRecord> medicalDose = productMedicalRecordRepository.GetAll(SelectedId, RecordTypeEnum.Channel);
            var medArray = new[]
            {
                Med1, Med2, Med3, Med4, Med5, Med6, Med7, Med8, Med9, Med10,
                Med11, Med12, Med13, Med14, Med15
            };
            var doseArray = new[]
            {
                Dose1, Dose2, Dose3, Dose4, Dose5, Dose6, Dose7, Dose8, Dose9, Dose10,
                Dose11, Dose12, Dose13, Dose14, Dose15
            };
            var totalArray = new[]
            {
                Total1, Total2, Total3, Total4, Total5, Total6, Total7, Total8, Total9, Total10,
                Total11, Total12, Total13, Total14, Total15
            };

            ChitNumber = channelRecord.ChitNumber;
            PharmacyBill = channelRecord.PharmacyBill;
            OPDCharge = channelRecord.OPDCharge ?? 0;
            OtherCharges = channelRecord.OtherCharges ?? 0;
            AdmitDate = channelRecord.AdmitDate;
            TotalBill = channelRecord.TotalBill;
            Patient = PatientList[channelRecord.PatientId];
            Doctor = DoctorList[channelRecord.DoctorId];
            AddedBy = EmployeeList[channelRecord.AddedBy];
            ConsultantFee = channelRecord.ConsultantFee ?? 0;
            Nurse1 = channelRecord.Nurse1Id != null ? NurseList[channelRecord.Nurse1Id.Value] : string.Empty;
            Nurse2 = channelRecord.Nurse2Id != null ? NurseList[channelRecord.Nurse2Id.Value] : string.Empty;
            DocComm = channelRecord.DocComm;
            Nurse1Comm = channelRecord.Nurse1Comm;
            Nurse2Comm = channelRecord.Nurse2Comm;
            for (int i=0; i<medArray.Count(); i++)
            {
                if (i < medicalDose.Count()) 
                {
                    medArray[i] = MedicineList.FirstOrDefault(m => m.Key == medicalDose.ElementAt(i).ProductId).Value;
                    doseArray[i] = medicalDose.ElementAt(i).Units.ToString();
                    totalArray[i] = medicalDose.ElementAt(i).SoldPrice;
                } else
                {
                    medArray[i] = null;
                    doseArray[i] = null;
                    totalArray[i] = 0;
                }
            }
            Med1 = medArray[0];
            Med2 = medArray[1];
            Med3 = medArray[2];
            Med4 = medArray[3];
            Med5 = medArray[4];
            Med6 = medArray[5];
            Med7 = medArray[6];
            Med8 = medArray[7];
            Med9 = medArray[8];
            Med10 = medArray[9];
            Med11 = medArray[10];
            Med12 = medArray[11];
            Med13 = medArray[12];
            Med14 = medArray[13];
            Med15 = medArray[14];
            Dose1 = doseArray[0];
            Dose2 = doseArray[1];
            Dose3 = doseArray[2];
            Dose4 = doseArray[3];
            Dose5 = doseArray[4];
            Dose6 = doseArray[5];
            Dose7 = doseArray[6];
            Dose8 = doseArray[7];
            Dose9 = doseArray[8];
            Dose10 = doseArray[9];
            Dose11 = doseArray[10];
            Dose12 = doseArray[11];
            Dose13 = doseArray[12];
            Dose14 = doseArray[13];
            Dose15 = doseArray[14];
            Total1 = totalArray[0];
            Total2 = totalArray[1];
            Total3 = totalArray[2];
            Total4 = totalArray[3];
            Total5 = totalArray[4];
            Total6 = totalArray[5];
            Total7 = totalArray[6];
            Total8 = totalArray[7];
            Total9 = totalArray[8];
            Total10 = totalArray[9];
            Total11 = totalArray[10];
            Total12 = totalArray[11];
            Total13 = totalArray[12];
            Total14 = totalArray[13];
            Total15 = totalArray[14];
        }

        private void ExecuteUpdateCommand(object obj)
        {
            if (string.IsNullOrWhiteSpace(ChitNumber) || string.IsNullOrWhiteSpace(PharmacyBill.ToString()) || string.IsNullOrWhiteSpace(TotalBill.ToString()) ||
                string.IsNullOrWhiteSpace(Patient) || string.IsNullOrWhiteSpace(Doctor) || string.IsNullOrWhiteSpace(AdmitDate.ToString()))
            {
                ChitNumberErrorMessage = (string.IsNullOrWhiteSpace(ChitNumber)) ? "Chit number is required." : "";
                PharmacyBillErrorMessage = (string.IsNullOrWhiteSpace(PharmacyBill.ToString())) ? "Pharmacy bill is required." : "";
                TotalCostErrorMessage = (string.IsNullOrWhiteSpace(TotalBill.ToString())) ? "Total bill is required." : "";
                PatientErrorMessage = (string.IsNullOrWhiteSpace(Patient)) ? "Patient name is required." : "";
                DoctorErrorMessage = (string.IsNullOrWhiteSpace(Doctor)) ? "Doctor name is required." : "";
            }
            else if (AdmitDate > DateTime.Now)
            {
                AdmitDateErrorMessage = "You have selected a future date.";
            }
            else
            {
                if (!System.Windows.Application.Current.Properties.Contains("Username"))
                {
                    return;
                }
                string username = (string)System.Windows.Application.Current.Properties["Username"];
                var user = userRepository.GetByUsername(username);
                Dictionary<int, int> medDoseIds = new Dictionary<int, int>();
                var medArray = new[]
                {
                    Med1, Med2, Med3, Med4, Med5, Med6, Med7, Med8, Med9, Med10,
                    Med11, Med12, Med13, Med14, Med15
                };

                var doseArray = new[]
                {
                    Dose1, Dose2, Dose3, Dose4, Dose5, Dose6, Dose7, Dose8, Dose9, Dose10,
                    Dose11, Dose12, Dose13, Dose14, Dose15
                };
                ChannelRecord updateChannelRecord = new ChannelRecord()
                {
                    Id = SelectedId,
                    ChitNumber = ChitNumber,
                    PharmacyBill = PharmacyBill,
                    OPDCharge = OPDCharge,
                    OtherCharges = OtherCharges,
                    AdmitDate = AdmitDate,
                    TotalBill = TotalBill,
                    PatientId = PatientList.FirstOrDefault(x => x.Value == Patient).Key,
                    DoctorId = DoctorList.FirstOrDefault(x => x.Value == Doctor).Key,
                    AddedBy = user.Id,
                    ConsultantFee = ConsultantFee,
                    DocComm = DocComm,
                    Nurse1Id = NurseList.FirstOrDefault(x => x.Value == Nurse1).Key == 0 ? (int?)null : NurseList.FirstOrDefault(x => x.Value == Nurse1).Key,
                    Nurse2Id = NurseList.FirstOrDefault(x => x.Value == Nurse2).Key == 0 ? (int?)null : NurseList.FirstOrDefault(x => x.Value == Nurse2).Key,
                    Nurse1Comm = Nurse1Comm,
                    Nurse2Comm = Nurse2Comm,
                };

                for (int i = 0; i < medArray.Count(); i++)
                {
                    if (!string.IsNullOrWhiteSpace(medArray[i]) && !string.IsNullOrWhiteSpace(doseArray[i]))
                    {
                        var medKey = MedicineList.FirstOrDefault(x => x.Value == medArray[i]).Key;

                        if (medDoseIds.ContainsKey(medKey))
                        {
                            // Append the new dose to the existing dose
                            medDoseIds[medKey] += int.Parse(doseArray[i]);
                        }
                        else
                        {
                            // Add a new entry if the key does not exist
                            medDoseIds.Add(medKey, int.Parse(doseArray[i]));
                        }
                    }
                }
                var oldData = channelRecordRepository.GetByID(updateChannelRecord.Id);
                var oldMedicine = productMedicalRecordRepository.GetAll(updateChannelRecord.Id, RecordTypeEnum.Channel);
                bool editSuccess = channelRecordRepository.Edit(updateChannelRecord, medDoseIds);
                if (editSuccess)
                {
                    var newMedicine = productMedicalRecordRepository.GetAll(updateChannelRecord.Id, RecordTypeEnum.Channel);
                    // Transform newMedicine list to remove unwanted properties
                    var filteredNewMedicineData = newMedicine.Select(m => new
                    {
                        m.Id,
                        m.ProductId,
                        m.Units,
                        m.SoldPrice
                    }).ToList();

                    // Transform oldMedicine list to remove unwanted properties
                    var filteredOldMedicineData = oldMedicine.Select(m => new
                    {
                        m.Id,
                        m.ProductId,
                        m.Units,
                        m.SoldPrice
                    }).ToList();

                    // Log the activity
                    var log = new ActivityLog
                    {
                        AffectedEntity = EntitiesEnum.ChannelRecords,
                        AffectedEntityId = updateChannelRecord.Id,
                        ActionType = ActionTypeEnum.Update,
                        OldValues = JsonConvert.SerializeObject(oldData) + "\n\nMedicine:\n" + JsonConvert.SerializeObject(filteredOldMedicineData),
                        NewValues = JsonConvert.SerializeObject(updateChannelRecord) + "\n\nMedicine:\n" + JsonConvert.SerializeObject(filteredNewMedicineData),
                    };
                    activityLogRepository.AddLog(log, currentUser);
                    UpdateGridVisibility = false;
                    ChannelRecordListVisibility = true;
                    CreateGridVisibility = false;
                    LoadChannelRecordList("");
                }
            }
        }

        private bool CanExecuteForAdminsCommand(object obj)
        {
            var valideUser = Properties.Settings.Default.Username;
            var validEmployeeType = Properties.Settings.Default.EmployeeType;
            var validStatus = Properties.Settings.Default.Status;

            if (valideUser != null && validEmployeeType.Equals(EmployeeTypeEnum.Admin.ToString()) && validStatus.Equals(UserStatusEnum.Active.ToString()))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private bool CanExecuteCreateCommand(object obj)
        {
            var valideUser = Properties.Settings.Default.Username;
            var validEmployeeType = Properties.Settings.Default.EmployeeType;
            var validStatus = Properties.Settings.Default.Status;

            //Make total = 0 if dose is null
            for (int i = 1; i <= 10; i++)
            {
                var doseProperty = GetType().GetProperty($"Dose{i}");
                var totalProperty = GetType().GetProperty($"Total{i}");

                if (doseProperty != null && totalProperty != null)
                {
                    var doseValue = doseProperty.GetValue(this) as string;
                    if (string.IsNullOrWhiteSpace(doseValue))
                    {
                        totalProperty.SetValue(this, 0);
                    }
                }
            }

            if (!string.IsNullOrWhiteSpace(Med1) && !string.IsNullOrWhiteSpace(Dose1))
            {
                var mediSellingPrice = productRepository.GetByID(MedicineList.FirstOrDefault(x => x.Value == Med1).Key).SellingPrice;
                Total1 = mediSellingPrice * int.Parse(Dose1);
            }

            if (!string.IsNullOrWhiteSpace(Med2) && !string.IsNullOrWhiteSpace(Dose2))
            {
                var mediSellingPrice = productRepository.GetByID(MedicineList.FirstOrDefault(x => x.Value == Med2).Key).SellingPrice;
                Total2 = mediSellingPrice * int.Parse(Dose2);
            }

            if (!string.IsNullOrWhiteSpace(Med3) && !string.IsNullOrWhiteSpace(Dose3))
            {
                var mediSellingPrice = productRepository.GetByID(MedicineList.FirstOrDefault(x => x.Value == Med3).Key).SellingPrice;
                Total3 = mediSellingPrice * int.Parse(Dose3);
            }

            if (!string.IsNullOrWhiteSpace(Med4) && !string.IsNullOrWhiteSpace(Dose4))
            {
                var mediSellingPrice = productRepository.GetByID(MedicineList.FirstOrDefault(x => x.Value == Med4).Key).SellingPrice;
                Total4 = mediSellingPrice * int.Parse(Dose4);
            }

            if (!string.IsNullOrWhiteSpace(Med5) && !string.IsNullOrWhiteSpace(Dose5))
            {
                var mediSellingPrice = productRepository.GetByID(MedicineList.FirstOrDefault(x => x.Value == Med5).Key).SellingPrice;
                Total5 = mediSellingPrice * int.Parse(Dose5);
            }

            if (!string.IsNullOrWhiteSpace(Med6) && !string.IsNullOrWhiteSpace(Dose6))
            {
                var mediSellingPrice = productRepository.GetByID(MedicineList.FirstOrDefault(x => x.Value == Med6).Key).SellingPrice;
                Total6 = mediSellingPrice * int.Parse(Dose6);
            }

            if (!string.IsNullOrWhiteSpace(Med7) && !string.IsNullOrWhiteSpace(Dose7))
            {
                var mediSellingPrice = productRepository.GetByID(MedicineList.FirstOrDefault(x => x.Value == Med7).Key).SellingPrice;
                Total7 = mediSellingPrice * int.Parse(Dose7);
            }

            if (!string.IsNullOrWhiteSpace(Med8) && !string.IsNullOrWhiteSpace(Dose8))
            {
                var mediSellingPrice = productRepository.GetByID(MedicineList.FirstOrDefault(x => x.Value == Med8).Key).SellingPrice;
                Total8 = mediSellingPrice * int.Parse(Dose8);
            }

            if (!string.IsNullOrWhiteSpace(Med9) && !string.IsNullOrWhiteSpace(Dose9))
            {
                var mediSellingPrice = productRepository.GetByID(MedicineList.FirstOrDefault(x => x.Value == Med9).Key).SellingPrice;
                Total9 = mediSellingPrice * int.Parse(Dose9);
            }

            if (!string.IsNullOrWhiteSpace(Med10) && !string.IsNullOrWhiteSpace(Dose10))
            {
                var mediSellingPrice = productRepository.GetByID(MedicineList.FirstOrDefault(x => x.Value == Med10).Key).SellingPrice;
                Total10 = mediSellingPrice * int.Parse(Dose10);
            }

            if (!string.IsNullOrWhiteSpace(Med11) && !string.IsNullOrWhiteSpace(Dose11))
            {
                var mediSellingPrice = productRepository.GetByID(MedicineList.FirstOrDefault(x => x.Value == Med11).Key).SellingPrice;
                Total11 = mediSellingPrice * int.Parse(Dose11);
            }

            if (!string.IsNullOrWhiteSpace(Med12) && !string.IsNullOrWhiteSpace(Dose12))
            {
                var mediSellingPrice = productRepository.GetByID(MedicineList.FirstOrDefault(x => x.Value == Med12).Key).SellingPrice;
                Total12 = mediSellingPrice * int.Parse(Dose12);
            }
            PharmacyBill = Total1 + Total2 + Total3 + Total4 + Total5 + Total6 + Total7 + Total8 + Total9 + Total10 + Total11 + Total12;
            TotalBill = OPDCharge + PharmacyBill + OtherCharges + ConsultantFee;

            if (valideUser != null 
                && (validEmployeeType.Equals(EmployeeTypeEnum.Admin.ToString()) || validEmployeeType.Equals(EmployeeTypeEnum.Manager.ToString()) || validEmployeeType.Equals(EmployeeTypeEnum.Staff.ToString()))
                && validStatus.Equals(UserStatusEnum.Active.ToString()))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private void ButtonVisibility()
        {
            var valideUser = Properties.Settings.Default.Username;
            var validEmployeeType = Properties.Settings.Default.EmployeeType;
            var validStatus = Properties.Settings.Default.Status;

            if (valideUser != null && validEmployeeType.Equals(EmployeeTypeEnum.Admin.ToString()) && validStatus.Equals(UserStatusEnum.Active.ToString()))
            {
                UpdateButtonVisibility = true;
                CreateButtonVisibility = true;
                DeleteButtonVisibility = true;
            }
            else if (valideUser != null && (validEmployeeType.Equals(EmployeeTypeEnum.Manager.ToString()) || validEmployeeType.Equals(EmployeeTypeEnum.Staff.ToString())) && validStatus.Equals(UserStatusEnum.Active.ToString()))
            {
                UpdateButtonVisibility = false;
                CreateButtonVisibility = true;
                DeleteButtonVisibility = false;
            }
            else
            {
                UpdateButtonVisibility = false;
                CreateButtonVisibility = false;
                DeleteButtonVisibility = false;
            }
        }

        private void ExecuteSearchCommand(object obj)
        {
            LoadChannelRecordList(SearchWord);
        }

        private bool CanExecuteForAllUsersCommand(object obj)
        {
            var valideUser = Properties.Settings.Default.Username;

            if (valideUser != null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private void LoadChannelRecordList(string searchWord)
        {
            var channelRecords = channelRecordRepository.GetAll(searchWord);
            ChannelRecordList = channelRecords;
            if (channelRecords.Any())
            {
                NoResults = "Hidden";
            }
            else
            {
                NoResults = "Visible";
            }
        }
    }
}
