using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using QWellApp.Enums;
using QWellApp.Models;
using QWellApp.Repositories;
using QWellApp.ViewModels.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Input;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ListView;

namespace QWellApp.ViewModels
{
    public class LabRecordViewModel: ViewModelBase
    {
        //Fields
        private IEnumerable<LabRecordView> _labRecordList;
        private string _noResults;
        private string _searchWord = "";
        public int _selectedId;
        private string _patient;
        private string _chitNumber;
        private DateTime _admitDate = DateTime.Today;
        private float _totalBill;
        private float _totalLabPaidCost;
        private string _addedBy;
        private float _otherCharges;
        private float _consultantFee;
        private float _labBill;
        private string _doctor;
        private float _docComm;
        private float _nurse1Comm;
        private float _nurse2Comm;
        private string _nurse1;
        private string _nurse2;
        private string _lab1;
        private string _lab2;
        private string _lab3;
        private string _lab4;
        private string _lab5;
        private string _lab6;
        private string _lab7;
        private string _lab8;
        private string _lab9;
        private string _lab10;
        private float _labTotal1;
        private float _labTotal2;
        private float _labTotal3;
        private float _labTotal4;
        private float _labTotal5;
        private float _labTotal6;
        private float _labTotal7;
        private float _labTotal8;
        private float _labTotal9;
        private float _labTotal10;
        private float _labPaid1;
        private float _labPaid2;
        private float _labPaid3;
        private float _labPaid4;
        private float _labPaid5;
        private float _labPaid6;
        private float _labPaid7;
        private float _labPaid8;
        private float _labPaid9;
        private float _labPaid10;
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
        private string _admitDateErrorMessage;
        private string _totalCostErrorMessage;
        private string _totalLabPaidCostErrorMessage;
        private string _patientErrorMessage;
        private ViewModelBase _currentChildView;
        private Dictionary<int, string> _employeeList;
        private Dictionary<int, string> _patientList;
        private Dictionary<int, string> _nurseList;
        private Dictionary<int, string> _doctorList;
        private Dictionary<int, string> _medicineList;
        private Dictionary<int, string> _labTests;
        private List<int> _labTestList;
        private bool _updateButtonVisibility = false;
        private bool _createButtonVisibility = false;
        private bool _deleteButtonVisibility = false;
        private bool _resetUpdateButtonsVisibility = false;
        private bool _updateGridVisibility = false;
        private bool _createGridVisibility = false;
        private bool _labRecordListVisibility = true;

        private ILabRecordRepository labRecordRepository;
        private IUserRepository userRepository;
        private IPatientRepository patientRepository;
        private ILabTestRepository labTestRepository;
        private IProductRepository productRepository;
        private ILabRecordTestRepository labRecordTestRepository;
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
        public string Patient
        {
            get { return _patient; }
            set
            {
                _patient = value;
                OnPropertyChanged(nameof(Patient));
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
                _totalBill = value;
                OnPropertyChanged(nameof(TotalBill));
            }
        }

        public float TotalLabPaidCost
        {
            get { return _totalLabPaidCost; }
            set
            {
                _totalLabPaidCost = value;
                OnPropertyChanged(nameof(TotalLabPaidCost));
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
        public float OtherCharges
        {
            get { return _otherCharges; }
            set
            {
                _otherCharges = value;
                OnPropertyChanged(nameof(OtherCharges));
            }
        }
        public float LabBill
        {
            get { return _labBill; }
            set
            {
                _labBill = value;
                OnPropertyChanged(nameof(LabBill));
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
        public string Lab1
        {
            get { return _lab1; }
            set
            {
                _lab1 = value;
                OnPropertyChanged(nameof(Lab1));
            }
        }

        public string Lab2
        {
            get { return _lab2; }
            set
            {
                _lab2 = value;
                OnPropertyChanged(nameof(Lab2));
            }
        }

        public string Lab3
        {
            get { return _lab3; }
            set
            {
                _lab3 = value;
                OnPropertyChanged(nameof(Lab3));
            }
        }

        public string Lab4
        {
            get { return _lab4; }
            set
            {
                _lab4 = value;
                OnPropertyChanged(nameof(Lab4));
            }
        }

        public string Lab5
        {
            get { return _lab5; }
            set
            {
                _lab5 = value;
                OnPropertyChanged(nameof(Lab5));
            }
        }

        public string Lab6
        {
            get { return _lab6; }
            set
            {
                _lab6 = value;
                OnPropertyChanged(nameof(Lab6));
            }
        }

        public string Lab7
        {
            get { return _lab7; }
            set
            {
                _lab7 = value;
                OnPropertyChanged(nameof(Lab7));
            }
        }

        public string Lab8
        {
            get { return _lab8; }
            set
            {
                _lab8 = value;
                OnPropertyChanged(nameof(Lab8));
            }
        }

        public string Lab9
        {
            get { return _lab9; }
            set
            {
                _lab9 = value;
                OnPropertyChanged(nameof(Lab9));
            }
        }

        public string Lab10
        {
            get { return _lab10; }
            set
            {
                _lab10 = value;
                OnPropertyChanged(nameof(Lab10));
            }
        }

        public float LabTotal1
        {
            get { return _labTotal1; }
            set
            {
                _labTotal1 = value;
                OnPropertyChanged(nameof(LabTotal1));
            }
        }

        public float LabTotal2
        {
            get { return _labTotal2; }
            set
            {
                _labTotal2 = value;
                OnPropertyChanged(nameof(LabTotal2));
            }
        }

        public float LabTotal3
        {
            get { return _labTotal3; }
            set
            {
                _labTotal3 = value;
                OnPropertyChanged(nameof(LabTotal3));
            }
        }

        public float LabTotal4
        {
            get { return _labTotal4; }
            set
            {
                _labTotal4 = value;
                OnPropertyChanged(nameof(LabTotal4));
            }
        }

        public float LabTotal5
        {
            get { return _labTotal5; }
            set
            {
                _labTotal5 = value;
                OnPropertyChanged(nameof(LabTotal5));
            }
        }

        public float LabTotal6
        {
            get { return _labTotal6; }
            set
            {
                _labTotal6 = value;
                OnPropertyChanged(nameof(LabTotal6));
            }
        }

        public float LabTotal7
        {
            get { return _labTotal7; }
            set
            {
                _labTotal7 = value;
                OnPropertyChanged(nameof(LabTotal7));
            }
        }

        public float LabTotal8
        {
            get { return _labTotal8; }
            set
            {
                _labTotal8 = value;
                OnPropertyChanged(nameof(LabTotal8));
            }
        }

        public float LabTotal9
        {
            get { return _labTotal9; }
            set
            {
                _labTotal9 = value;
                OnPropertyChanged(nameof(LabTotal9));
            }
        }

        public float LabTotal10
        {
            get { return _labTotal10; }
            set
            {
                _labTotal10 = value;
                OnPropertyChanged(nameof(LabTotal10));
            }
        }

        public float LabPaid1
        {
            get { return _labPaid1; }
            set
            {
                _labPaid1 = value;
                OnPropertyChanged(nameof(LabPaid1));
            }
        }

        public float LabPaid2
        {
            get { return _labPaid2; }
            set
            {
                _labPaid2 = value;
                OnPropertyChanged(nameof(LabPaid2));
            }
        }

        public float LabPaid3
        {
            get { return _labPaid3; }
            set
            {
                _labPaid3 = value;
                OnPropertyChanged(nameof(LabPaid3));
            }
        }

        public float LabPaid4
        {
            get { return _labPaid4; }
            set
            {
                _labPaid4 = value;
                OnPropertyChanged(nameof(LabPaid4));
            }
        }

        public float LabPaid5
        {
            get { return _labPaid5; }
            set
            {
                _labPaid5 = value;
                OnPropertyChanged(nameof(LabPaid5));
            }
        }

        public float LabPaid6
        {
            get { return _labPaid6; }
            set
            {
                _labPaid6 = value;
                OnPropertyChanged(nameof(LabPaid6));
            }
        }

        public float LabPaid7
        {
            get { return _labPaid7; }
            set
            {
                _labPaid7 = value;
                OnPropertyChanged(nameof(LabPaid7));
            }
        }

        public float LabPaid8
        {
            get { return _labPaid8; }
            set
            {
                _labPaid8 = value;
                OnPropertyChanged(nameof(LabPaid8));
            }
        }

        public float LabPaid9
        {
            get { return _labPaid9; }
            set
            {
                _labPaid9 = value;
                OnPropertyChanged(nameof(LabPaid9));
            }
        }

        public float LabPaid10
        {
            get { return _labPaid10; }
            set
            {
                _labPaid10 = value;
                OnPropertyChanged(nameof(LabPaid10));
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
            get { return _chitNumberErrorMessage; }
            set
            {
                _chitNumberErrorMessage = value;
                OnPropertyChanged(nameof(ChitNumberErrorMessage));
            }
        }

        public string AdmitDateErrorMessage
        {
            get { return _admitDateErrorMessage; }
            set
            {
                _admitDateErrorMessage = value;
                OnPropertyChanged(nameof(AdmitDateErrorMessage));
            }
        }

        public string TotalCostErrorMessage
        {
            get { return _totalCostErrorMessage; }
            set
            {
                _totalCostErrorMessage = value;
                OnPropertyChanged(nameof(TotalCostErrorMessage));
            }
        }

        public string TotalLabPaidCostErrorMessage
        {
            get { return _totalLabPaidCostErrorMessage; }
            set
            {
                _totalLabPaidCostErrorMessage = value;
                OnPropertyChanged(nameof(TotalLabPaidCostErrorMessage));
            }
        }

        public string PatientErrorMessage
        {
            get { return _patientErrorMessage; }
            set
            {
                _patientErrorMessage = value;
                OnPropertyChanged(nameof(PatientErrorMessage));
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
        public bool LabRecordListVisibility
        {
            get
            {
                return _labRecordListVisibility;
            }

            set
            {
                _labRecordListVisibility = value;
                OnPropertyChanged(nameof(LabRecordListVisibility));
            }
        }

        public IEnumerable<LabRecordView> LabRecordList
        {
            get
            {
                return _labRecordList;
            }
            set
            {
                _labRecordList = value;
                OnPropertyChanged(nameof(LabRecordList));
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
        public Dictionary<int, string> MedicineList
        {
            get
            {
                return _medicineList;
            }
            set
            {
                _medicineList = value;
                OnPropertyChanged(nameof(MedicineList));
            }
        }
        public Dictionary<int, string> LabTests
        {
            get
            {
                return _labTests;
            }
            set
            {
                _labTests = value;
                OnPropertyChanged(nameof(LabTests));
            }
        }

        public List<int> LabTestList
        {
            get
            {
                return _labTestList;
            }
            set
            {
                _labTestList = value;
                OnPropertyChanged(nameof(LabTestList));
            }
        }

        #endregion

        //Commands
        public ICommand LoadSearchResults { get; }
        public ICommand UpdateLabRecordCommand { get; }
        public ICommand CreateLabRecordCommand { get; }
        public ICommand GetLabRecordDetails { get; }
        public ICommand ResetCommand { get; }
        public ICommand CancelCommand { get; }
        public ICommand BackToListCommand { get; }
        public ICommand DeleteCommand { get; }

        public LabRecordViewModel()
        {
            LabRecordList = new List<LabRecordView>();
            labRecordRepository = new LabRecordRepository();
            userRepository = new UserRepository();
            patientRepository = new PatientRepository();
            labTestRepository = new LabTestRepository();
            productRepository = new ProductRepository();
            productMedicalRecordRepository = new ProductMedicalRecordRepository();
            labRecordTestRepository = new LabRecordTestRepository();
            LoadLabRecordList(SearchWord);
            PatientList = new Dictionary<int, string>();
            EmployeeList = new Dictionary<int, string>();
            DoctorList = new Dictionary<int, string>();
            NurseList = new Dictionary<int, string>();
            MedicineList = new Dictionary<int, string>();
            activityLogRepository = new ActivityLogRepository();
            currentUser = userRepository.GetByUsername(Properties.Settings.Default.Username);
            LabTestList = new List<int>();
            LabTests = new Dictionary<int, string>();
            LoadSearchResults = new RelayCommand(ExecuteSearchCommand, CanExecuteForAllUsersCommand);
            GetLabRecordDetails = new RelayCommand(ExecuteGetLabRecordDetailsCommand, CanExecuteGetLabRecordDetailsCommand);
            UpdateLabRecordCommand = new RelayCommand(ExecuteUpdateCommand, CanExecuteForAdminsCommand);
            CreateLabRecordCommand = new RelayCommand(ExecuteCreateCommand, CanExecuteCreateCommand);
            ResetCommand = new RelayCommand(ExecuteGetLabRecordDetailsCommand);
            CancelCommand = new RelayCommand(ExecuteCancelCommand);
            DeleteCommand = new RelayCommand(ExecuteDeleteCommand, CanExecuteDeleteUserCommand);
            BackToListCommand = new RelayCommand(ExecuteBackToListCommand);
            ButtonVisibility();
            LoadPatientList();
            LoadDoctorList();
            LoadNurseList();
            LoadEmployeeList();
            LoadLabTestList();
            LoadMedicineList();
        }

        private void LoadMedicineList()
        {
            Dictionary<int, string> medicines = new Dictionary<int, string>();
            var medicineList = productRepository.GetAll("");
            foreach (var medicine in medicineList)
            {
                medicines.Add(medicine.Id, medicine.BrandName + " " + medicine.Generic);
            }
            MedicineList = medicines;
        }

        private void LoadLabTestList()
        {
            List<int> labTests = new List<int>();
            Dictionary<int, string> tests = new Dictionary<int, string>();
            var labTestList = labTestRepository.GetAll("");
            foreach (var labTest in labTestList)
            {
                labTests.Add(labTest.Id);
                tests.Add(labTest.Id, labTest.HospitalName + "-" + labTest.TestName);
            }
            LabTestList = labTests;
            LabTests = tests;
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
            var oldData = labRecordRepository.GetByID(SelectedId);
            var oldMedicine = productMedicalRecordRepository.GetAll(SelectedId, RecordTypeEnum.Lab);
            var deleteSuccess = labRecordRepository.Remove(SelectedId);
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
                    AffectedEntity = EntitiesEnum.LabRecords,
                    AffectedEntityId = SelectedId,
                    ActionType = ActionTypeEnum.Delete,
                    OldValues = JsonConvert.SerializeObject(oldData) + "\n\nMedicine:\n" + JsonConvert.SerializeObject(filteredOldMedicineData), // Serialize the whole object
                    NewValues = "-"
                };
                activityLogRepository.AddLog(log, currentUser);
                LoadLabRecordList("");
            }
        }

        private void ExecuteBackToListCommand(object obj)
        {
            UpdateGridVisibility = false;
            LabRecordListVisibility = true;
            CreateGridVisibility = false;
            LoadLabRecordList("");
        }

        private void ExecuteCancelCommand(object obj)
        {
            Patient = string.Empty;
            ChitNumber = string.Empty;
            AdmitDate = DateTime.Now;
            TotalBill = 0;
            TotalLabPaidCost = 0;
            AddedBy = string.Empty;
            ConsultantFee = 0;
            LabBill = 0;
            OtherCharges = 0;
            Doctor = string.Empty;
            DocComm = 0;
            Nurse1Comm = 0;
            Nurse2Comm = 0;
            Nurse1 = string.Empty;
            Nurse2 = string.Empty;
            Lab1 = string.Empty;
            Lab2 = string.Empty;
            Lab3 = string.Empty;
            Lab4 = string.Empty;
            Lab5 = string.Empty;
            Lab6 = string.Empty;
            Lab7 = string.Empty;
            Lab8 = string.Empty;
            Lab9 = string.Empty;
            Lab10 = string.Empty;
            LabTotal1 = 0;
            LabTotal2 = 0;
            LabTotal3 = 0;
            LabTotal4 = 0;
            LabTotal5 = 0;
            LabTotal6 = 0;
            LabTotal7 = 0;
            LabTotal8 = 0;
            LabTotal9 = 0;
            LabTotal10 = 0;
            LabPaid1 = 0;
            LabPaid2 = 0;
            LabPaid3 = 0;
            LabPaid4 = 0;
            LabPaid5 = 0;
            LabPaid6 = 0;
            LabPaid7 = 0;
            LabPaid8 = 0;
            LabPaid9 = 0;
            LabPaid10 = 0;
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

            // clear error msgs
            ChitNumberErrorMessage = string.Empty;
            AdmitDateErrorMessage = string.Empty;
            TotalCostErrorMessage = string.Empty;
            TotalLabPaidCostErrorMessage = string.Empty;
            PatientErrorMessage = string.Empty;
        }
        private void ExecuteCreateCommand(object obj)
        {
            if (string.IsNullOrWhiteSpace(ChitNumber) || string.IsNullOrWhiteSpace(TotalBill.ToString()) || string.IsNullOrWhiteSpace(TotalLabPaidCost.ToString()) ||
                string.IsNullOrWhiteSpace(Patient))
            {
                ChitNumberErrorMessage = (string.IsNullOrWhiteSpace(ChitNumber)) ? "Chit number is required." : "";
                TotalLabPaidCostErrorMessage = (string.IsNullOrWhiteSpace(TotalLabPaidCost.ToString())) ? "Total lab paid cost is required." : "";
                TotalCostErrorMessage = (string.IsNullOrWhiteSpace(TotalBill.ToString())) ? "Total bill is required." : "";
                PatientErrorMessage = (string.IsNullOrWhiteSpace(Patient)) ? "Patient name is required." : "";
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
                List<int> labRecordTestIds = new List<int>();
                var labArray = new[] { Lab1, Lab2, Lab3, Lab4, Lab5, Lab6, Lab7, Lab8, Lab9, Lab10 };
                LabRecord createLabRecord = new LabRecord()
                {
                    PatientId = PatientList.FirstOrDefault(x => x.Value == Patient).Key,
                    ChitNumber = ChitNumber,
                    AdmitDate = DateTime.Now,
                    TotalBill = TotalBill,
                    OtherCharges = OtherCharges,
                    ConsultantFee = ConsultantFee,
                    LabBill = LabBill,
                    TotalLabPaidCost = TotalLabPaidCost,
                    DoctorId = DoctorList.FirstOrDefault(x => x.Value == Doctor).Key == 0 ? (int?)null : DoctorList.FirstOrDefault(x => x.Value == Doctor).Key,
                    AddedBy = user.Id,
                    DocComm = DocComm,
                    Nurse1Id = NurseList.FirstOrDefault(x => x.Value == Nurse1).Key == 0 ? (int?)null : NurseList.FirstOrDefault(x => x.Value == Nurse1).Key,
                    Nurse2Id = NurseList.FirstOrDefault(x => x.Value == Nurse2).Key == 0 ? (int?)null : NurseList.FirstOrDefault(x => x.Value == Nurse2).Key,
                    Nurse1Comm = Nurse1Comm,
                    Nurse2Comm = Nurse2Comm,
                };

                for (int i= 0 ; i < labArray.Count(); i++)
                {
                    if (!string.IsNullOrWhiteSpace(labArray[i])) {
                        if (labRecordTestIds.Contains(LabTests.FirstOrDefault(x => x.Value == labArray[i]).Key))
                        {
                            labRecordTestIds.Remove(LabTests.FirstOrDefault(x => x.Value == labArray[i]).Key);
                        }
                        labRecordTestIds.Add(LabTests.FirstOrDefault(x => x.Value == labArray[i]).Key);
                    }
                }
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
                var createSuccess = labRecordRepository.Add(createLabRecord, labRecordTestIds, medDoseIds);
                if (createSuccess)
                {
                    UpdateGridVisibility = false;
                    LabRecordListVisibility = true;
                    CreateGridVisibility = false;
                    LoadLabRecordList("");
                }
            }
        }

        private bool CanExecuteGetLabRecordDetailsCommand(object obj)
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

        private void ExecuteGetLabRecordDetailsCommand(object obj)
        {
            LabRecord labRecord = labRecordRepository.GetByID(SelectedId);
            IEnumerable<LabRecordTest> labRecordTest = labRecordTestRepository.GetAll(SelectedId);
            IEnumerable<ProductMedicalRecord> medicalDose = productMedicalRecordRepository.GetAll(SelectedId, RecordTypeEnum.Lab);
            var labArray = new[] { Lab1, Lab2, Lab3, Lab4, Lab5, Lab6, Lab7, Lab8, Lab9, Lab10 };
            var totalLabArray = new[] {LabTotal1, LabTotal2, LabTotal3, LabTotal4, LabTotal5, LabTotal6, LabTotal7, LabTotal8, LabTotal9, LabTotal10 };
            var labPaidArray = new[] { LabPaid1, LabPaid2, LabPaid3, LabPaid4, LabPaid5, LabPaid6, LabPaid7, LabPaid8, LabPaid9, LabPaid10 };
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

            Patient = PatientList[labRecord.PatientId];
            ChitNumber = labRecord.ChitNumber;
            AdmitDate = labRecord.AdmitDate;
            TotalBill = labRecord.TotalBill;
            TotalLabPaidCost = labRecord.TotalLabPaidCost;
            OtherCharges = labRecord.OtherCharges ?? 0;
            ConsultantFee = labRecord.ConsultantFee ?? 0;
            LabBill = labRecord.LabBill;
            Doctor = labRecord.DoctorId != null ? DoctorList[labRecord.DoctorId.Value] : string.Empty; ;
            AddedBy = EmployeeList[labRecord.AddedBy];
            Nurse1 = labRecord.Nurse1Id != null ? NurseList[labRecord.Nurse1Id.Value] : string.Empty;
            Nurse2 = labRecord.Nurse2Id != null ? NurseList[labRecord.Nurse2Id.Value] : string.Empty;
            DocComm = labRecord.DocComm;
            Nurse1Comm = labRecord.Nurse1Comm;
            Nurse2Comm = labRecord.Nurse2Comm;

            for (int i=0; i<labArray.Count(); i++)
            {
                if (i < labRecordTest.Count()) 
                {
                    labArray[i] = LabTests[labRecordTest.ElementAt(i).LabTestId];
                    var labTestCost = labTestRepository.GetByID(labRecordTest.ElementAt(i).LabTestId).Cost;
                    totalLabArray[i] = labTestCost;
                    labPaidArray[i] = float.Parse(labTestRepository.GetByID(labRecordTest.ElementAt(i).LabTestId).LabPaid);
                } else
                {
                    labArray[i] = null;
                    totalLabArray[i] = 0;
                    labPaidArray[i] = 0;
                }
            }

            for (int i = 0; i < medArray.Count(); i++)
            {
                if (i < medicalDose.Count())
                {
                    medArray[i] = MedicineList[medicalDose.ElementAt(i).ProductId];
                    doseArray[i] = medicalDose.ElementAt(i).Units.ToString();
                    totalArray[i] = medicalDose.ElementAt(i).SoldPrice;
                }
                else
                {
                    medArray[i] = null;
                    doseArray[i] = null;
                    totalArray[i] = 0;
                }
            }
            Lab1 = labArray[0];
            Lab2 = labArray[1];
            Lab3 = labArray[2];
            Lab4 = labArray[3];
            Lab5 = labArray[4];
            Lab6 = labArray[5];
            Lab7 = labArray[6];
            Lab8 = labArray[7];
            Lab9 = labArray[8];
            Lab10 = labArray[9];
            LabTotal1 = totalLabArray[0];
            LabTotal2 = totalLabArray[1];
            LabTotal3 = totalLabArray[2];
            LabTotal4 = totalLabArray[3];
            LabTotal5 = totalLabArray[4];
            LabTotal6 = totalLabArray[5];
            LabTotal7 = totalLabArray[6];
            LabTotal8 = totalLabArray[7];
            LabTotal9 = totalLabArray[8];
            LabTotal10 = totalLabArray[9];
            LabPaid1 = labPaidArray[0];
            LabPaid2 = labPaidArray[1];
            LabPaid3 = labPaidArray[2];
            LabPaid4 = labPaidArray[3];
            LabPaid5 = labPaidArray[4];
            LabPaid6 = labPaidArray[5];
            LabPaid7 = labPaidArray[6];
            LabPaid8 = labPaidArray[7];
            LabPaid9 = labPaidArray[8];
            LabPaid10 = labPaidArray[9];

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
            if (string.IsNullOrWhiteSpace(ChitNumber) || string.IsNullOrWhiteSpace(TotalBill.ToString()) || string.IsNullOrWhiteSpace(TotalLabPaidCost.ToString()) ||
                string.IsNullOrWhiteSpace(Patient))
            {
                ChitNumberErrorMessage = (string.IsNullOrWhiteSpace(ChitNumber)) ? "Chit number is required." : "";
                TotalLabPaidCostErrorMessage = (string.IsNullOrWhiteSpace(TotalLabPaidCost.ToString())) ? "Total lab paid cost is required." : "";
                TotalCostErrorMessage = (string.IsNullOrWhiteSpace(TotalBill.ToString())) ? "Total bill is required." : "";
                PatientErrorMessage = (string.IsNullOrWhiteSpace(Patient)) ? "Patient name is required." : "";
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
                List<int> labRecordTestIds = new List<int>();
                var labArray = new[] { Lab1, Lab2, Lab3, Lab4, Lab5, Lab6, Lab7, Lab8, Lab9, Lab10 };
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
                LabRecord updateLabRecord = new LabRecord()
                {
                    Id = SelectedId,
                    PatientId = PatientList.FirstOrDefault(x => x.Value == Patient).Key,
                    ChitNumber = ChitNumber,
                    AdmitDate = DateTime.Now,
                    TotalBill = TotalBill,
                    OtherCharges = OtherCharges,
                    ConsultantFee = ConsultantFee,
                    LabBill = LabBill,
                    TotalLabPaidCost = TotalLabPaidCost,
                    DoctorId = DoctorList.FirstOrDefault(x => x.Value == Doctor).Key == 0 ? (int?)null : DoctorList.FirstOrDefault(x => x.Value == Doctor).Key,
                    AddedBy = user.Id,
                    DocComm = DocComm,
                    Nurse1Id = NurseList.FirstOrDefault(x => x.Value == Nurse1).Key == 0 ? (int?)null : NurseList.FirstOrDefault(x => x.Value == Nurse1).Key,
                    Nurse2Id = NurseList.FirstOrDefault(x => x.Value == Nurse2).Key == 0 ? (int?)null : NurseList.FirstOrDefault(x => x.Value == Nurse2).Key,
                    Nurse1Comm = Nurse1Comm,
                    Nurse2Comm = Nurse2Comm,
                };

                for (int i = 0; i < labArray.Count(); i++)
                {
                    if (!string.IsNullOrWhiteSpace(labArray[i]))
                    {
                        if (labRecordTestIds.Contains(LabTests.FirstOrDefault(x => x.Value == labArray[i]).Key))
                        {
                            labRecordTestIds.Remove(LabTests.FirstOrDefault(x => x.Value == labArray[i]).Key);
                        }
                        labRecordTestIds.Add(LabTests.FirstOrDefault(x => x.Value == labArray[i]).Key);
                        
                    }
                }

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
                var oldData = labRecordRepository.GetByID(updateLabRecord.Id);
                var oldMedicine = productMedicalRecordRepository.GetAll(updateLabRecord.Id, RecordTypeEnum.Lab);
                bool editSuccess = labRecordRepository.Edit(updateLabRecord, labRecordTestIds, medDoseIds);
                if (editSuccess)
                {
                    var newMedicine = productMedicalRecordRepository.GetAll(updateLabRecord.Id, RecordTypeEnum.Lab);
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
                        AffectedEntity = EntitiesEnum.LabRecords,
                        AffectedEntityId = updateLabRecord.Id,
                        ActionType = ActionTypeEnum.Update,
                        OldValues = JsonConvert.SerializeObject(oldData) + "\n\nMedicine:\n" + JsonConvert.SerializeObject(filteredOldMedicineData),
                        NewValues = JsonConvert.SerializeObject(updateLabRecord) + "\n\nMedicine:\n" + JsonConvert.SerializeObject(filteredNewMedicineData),
                    };
                    activityLogRepository.AddLog(log, currentUser);
                    UpdateGridVisibility = false;
                    LabRecordListVisibility = true;
                    CreateGridVisibility = false;
                    LoadLabRecordList("");
                }
            }
        }

        private bool CanExecuteForAdminsCommand(object obj)
        {
            var valideUser = Properties.Settings.Default.Username;
            var validEmployeeType = Properties.Settings.Default.EmployeeType;
            var validStatus = Properties.Settings.Default.Status;

            if (valideUser != null && (validEmployeeType.Equals(EmployeeTypeEnum.Admin.ToString())) && validStatus.Equals(UserStatusEnum.Active.ToString()))
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

            List<int> labRecordTestIds = new List<int>();

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

            if (!string.IsNullOrWhiteSpace(Lab1))
            {
                var labCost = labTestRepository.GetByID(LabTests.FirstOrDefault(x => x.Value == Lab1).Key).Cost;
                var labPaidCost = labTestRepository.GetByID(LabTests.FirstOrDefault(x => x.Value == Lab1).Key).LabPaid;
                if (!labRecordTestIds.Contains(LabTests.FirstOrDefault(x => x.Value == Lab1).Key))
                {
                    labRecordTestIds.Add(LabTests.FirstOrDefault(x => x.Value == Lab1).Key);
                    LabTotal1 = labCost;
                    LabPaid1 = float.Parse(labPaidCost);
                }
            }

            if (!string.IsNullOrWhiteSpace(Lab2))
            {
                var labCost = labTestRepository.GetByID(LabTests.FirstOrDefault(x => x.Value == Lab2).Key).Cost;
                var labPaidCost = labTestRepository.GetByID(LabTests.FirstOrDefault(x => x.Value == Lab2).Key).LabPaid;
                if (!labRecordTestIds.Contains(LabTests.FirstOrDefault(x => x.Value == Lab2).Key))
                {
                    labRecordTestIds.Add(LabTests.FirstOrDefault(x => x.Value == Lab2).Key);
                    LabTotal2 = labCost;
                    LabPaid2 = float.Parse(labPaidCost);
                }
            }

            if (!string.IsNullOrWhiteSpace(Lab3))
            {
                var labCost = labTestRepository.GetByID(LabTests.FirstOrDefault(x => x.Value == Lab3).Key).Cost;
                var labPaidCost = labTestRepository.GetByID(LabTests.FirstOrDefault(x => x.Value == Lab3).Key).LabPaid;
                if (!labRecordTestIds.Contains(LabTests.FirstOrDefault(x => x.Value == Lab3).Key))
                {
                    labRecordTestIds.Add(LabTests.FirstOrDefault(x => x.Value == Lab3).Key);
                    LabTotal3 = labCost;
                    LabPaid3 = float.Parse(labPaidCost);
                }
            }

            if (!string.IsNullOrWhiteSpace(Lab4))
            {
                var labCost = labTestRepository.GetByID(LabTests.FirstOrDefault(x => x.Value == Lab4).Key).Cost;
                var labPaidCost = labTestRepository.GetByID(LabTests.FirstOrDefault(x => x.Value == Lab4).Key).LabPaid;
                if (!labRecordTestIds.Contains(LabTests.FirstOrDefault(x => x.Value == Lab4).Key))
                {
                    labRecordTestIds.Add(LabTests.FirstOrDefault(x => x.Value == Lab4).Key);
                    LabTotal4 = labCost;
                    LabPaid4 = float.Parse(labPaidCost);
                }
            }

            if (!string.IsNullOrWhiteSpace(Lab5))
            {
                var labCost = labTestRepository.GetByID(LabTests.FirstOrDefault(x => x.Value == Lab5).Key).Cost;
                var labPaidCost = labTestRepository.GetByID(LabTests.FirstOrDefault(x => x.Value == Lab5).Key).LabPaid;
                if (!labRecordTestIds.Contains(LabTests.FirstOrDefault(x => x.Value == Lab5).Key))
                {
                    labRecordTestIds.Add(LabTests.FirstOrDefault(x => x.Value == Lab5).Key);
                    LabTotal5 = labCost;
                    LabPaid5 = float.Parse(labPaidCost);
                }
            }

            if (!string.IsNullOrWhiteSpace(Lab6))
            {
                var labCost = labTestRepository.GetByID(LabTests.FirstOrDefault(x => x.Value == Lab6).Key).Cost;
                var labPaidCost = labTestRepository.GetByID(LabTests.FirstOrDefault(x => x.Value == Lab6).Key).LabPaid;
                if (!labRecordTestIds.Contains(LabTests.FirstOrDefault(x => x.Value == Lab6).Key))
                {
                    labRecordTestIds.Add(LabTests.FirstOrDefault(x => x.Value == Lab6).Key);
                    LabTotal6 = labCost;
                    LabPaid6 = float.Parse(labPaidCost);
                }
            }

            if (!string.IsNullOrWhiteSpace(Lab7))
            {
                var labCost = labTestRepository.GetByID(LabTests.FirstOrDefault(x => x.Value == Lab7).Key).Cost;
                var labPaidCost = labTestRepository.GetByID(LabTests.FirstOrDefault(x => x.Value == Lab7).Key).LabPaid;
                if (!labRecordTestIds.Contains(LabTests.FirstOrDefault(x => x.Value == Lab7).Key))
                {
                    labRecordTestIds.Add(LabTests.FirstOrDefault(x => x.Value == Lab7).Key);
                    LabTotal7 = labCost;
                    LabPaid7 = float.Parse(labPaidCost);
                }
            }

            if (!string.IsNullOrWhiteSpace(Lab8))
            {
                var labCost = labTestRepository.GetByID(LabTests.FirstOrDefault(x => x.Value == Lab8).Key).Cost;
                var labPaidCost = labTestRepository.GetByID(LabTests.FirstOrDefault(x => x.Value == Lab8).Key).LabPaid;
                if (!labRecordTestIds.Contains(LabTests.FirstOrDefault(x => x.Value == Lab8).Key))
                {
                    labRecordTestIds.Add(LabTests.FirstOrDefault(x => x.Value == Lab8).Key);
                    LabTotal8 = labCost;
                    LabPaid8 = float.Parse(labPaidCost);
                }
            }

            if (!string.IsNullOrWhiteSpace(Lab9))
            {
                var labCost = labTestRepository.GetByID(LabTests.FirstOrDefault(x => x.Value == Lab9).Key).Cost;
                var labPaidCost = labTestRepository.GetByID(LabTests.FirstOrDefault(x => x.Value == Lab9).Key).LabPaid;
                if (!labRecordTestIds.Contains(LabTests.FirstOrDefault(x => x.Value == Lab9).Key))
                {
                    labRecordTestIds.Add(LabTests.FirstOrDefault(x => x.Value == Lab9).Key);
                    LabTotal9 = labCost;
                    LabPaid9 = float.Parse(labPaidCost);
                }
            }

            if (!string.IsNullOrWhiteSpace(Lab10))
            {
                var labCost = labTestRepository.GetByID(LabTests.FirstOrDefault(x => x.Value == Lab10).Key).Cost;
                var labPaidCost = labTestRepository.GetByID(LabTests.FirstOrDefault(x => x.Value == Lab10).Key).LabPaid;
                if (!labRecordTestIds.Contains(LabTests.FirstOrDefault(x => x.Value == Lab10).Key))
                {
                    labRecordTestIds.Add(LabTests.FirstOrDefault(x => x.Value == Lab10).Key);
                    LabTotal10 = labCost;
                    LabPaid10 = float.Parse(labPaidCost);
                }
            }

            // med
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

            LabBill = LabTotal1 + LabTotal2 + LabTotal3 + LabTotal4 + LabTotal5 + LabTotal6 + LabTotal7 + LabTotal8 + LabTotal9 + LabTotal10;
            TotalLabPaidCost = LabPaid1 + LabPaid2 + LabPaid3 + LabPaid4 + LabPaid5 + LabPaid6 + LabPaid7 + LabPaid8 + LabPaid9 + LabPaid10;
            OtherCharges = Total1 + Total2 + Total3 + Total4 + Total5 + Total6 + Total7 + Total8 + Total9 + Total10;
            TotalBill = LabBill + OtherCharges + ConsultantFee;

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
            LoadLabRecordList(SearchWord);
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

        private void LoadLabRecordList(string searchWord)
        {
            var labRecords = labRecordRepository.GetAll(searchWord);
            LabRecordList = labRecords;
            if (labRecords.Any())
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
