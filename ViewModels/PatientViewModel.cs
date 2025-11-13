using Microsoft.VisualBasic.ApplicationServices;
using Newtonsoft.Json;
using QWellApp.Enums;
using QWellApp.Models;
using QWellApp.Repositories;
using QWellApp.ViewModels.Common;
using System;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.Linq;
using System.Security;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;

namespace QWellApp.ViewModels
{
    public class PatientViewModel: ViewModelBase
    {
        //Fields
        private IEnumerable<PatientView> _patientList;
        private string _noResults;
        private string _searchWord = "";
        public int _selectedId;
        private string _firstName;
        private string _lastName;
        private string _mobile;
        private string _telephone;
        private string _gender;
        private string _age;
        private string _nic;
        private string _status;
        private IEnumerable<string> _statusList;
        private string _allergicHistory;
        private string _weight;
        private string _firstNameErrorMessage;
        private string _lastNameErrorMessage;
        private string _mobileNumErrorMessage;
        private string _ageErrorMessage;
        private string _genderErrorMessage;
        private string _nicErrorMessage;
        private ViewModelBase _currentChildView;
        private bool _updateButtonVisibility = false;
        private bool _createButtonVisibility = false;
        private bool _deleteButtonVisibility = false;
        private bool _resetUpdateButtonsVisibility = false;
        private bool _updateGridVisibility = false;
        private bool _createGridVisibility = false;
        private bool _patientListVisibility = true;

        private IPatientRepository patientRepository;
        private IUserRepository userRepository;
        private IActivityLogRepository activityLogRepository;
        private UserDetails currentUser;

        public IEnumerable<PatientView> PatientList
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

        public string FirstName
        {
            get
            {
                return _firstName;
            }
            set
            {
                _firstName = value;
                OnPropertyChanged(nameof(FirstName));
            }
        }

        public string LastName
        {
            get
            {
                return _lastName;
            }
            set
            {
                _lastName = value;
                OnPropertyChanged(nameof(LastName));
            }
        }
        public string Mobile
        {
            get
            {
                return _mobile;
            }
            set
            {
                _mobile = value;
                OnPropertyChanged(nameof(Mobile));
            }
        }
        public string Telephone
        {
            get
            {
                return _telephone;
            }
            set
            {
                _telephone = value;
                OnPropertyChanged(nameof(Telephone));
            }
        }
        public string Age
        {
            get
            {
                return _age;
            }
            set
            {
                _age = value;
                OnPropertyChanged(nameof(Age));
            }
        }

        public string AllergicHistory
        {
            get
            {
                return _allergicHistory;
            }
            set
            {
                _allergicHistory = value;
                OnPropertyChanged(nameof(AllergicHistory));
            }
        }

        public string Weight
        {
            get
            {
                return _weight;
            }
            set
            {
                _weight = value;
                OnPropertyChanged(nameof(Weight));
            }
        }

        public string NIC
        {
            get
            {
                return _nic;
            }

            set
            {
                _nic = value;
                OnPropertyChanged(nameof(NIC));
            }
        }

        public string Status
        {
            get
            {
                return _status;
            }

            set
            {
                _status = value;
                OnPropertyChanged(nameof(Status));
            }
        }
        public IEnumerable<string> StatusList
        {
            get
            {
                return _statusList;
            }
            set
            {
                _statusList = value;
                OnPropertyChanged(nameof(StatusList));
            }
        }
        public string NICErrorMessage
        {
            get
            {
                return _nicErrorMessage;
            }

            set
            {
                _nicErrorMessage = value;
                OnPropertyChanged(nameof(NICErrorMessage));
            }
        }
        public string FirstNameErrorMessage
        {
            get
            {
                return _firstNameErrorMessage;
            }

            set
            {
                _firstNameErrorMessage = value;
                OnPropertyChanged(nameof(FirstNameErrorMessage));
            }
        }
        public string LastNameErrorMessage
        {
            get
            {
                return _lastNameErrorMessage;
            }

            set
            {
                _lastNameErrorMessage = value;
                OnPropertyChanged(nameof(LastNameErrorMessage));
            }
        }
        public string AgeErrorMessage
        {
            get
            {
                return _ageErrorMessage;
            }

            set
            {
                _ageErrorMessage = value;
                OnPropertyChanged(nameof(AgeErrorMessage));
            }
        }
        public string GenderErrorMessage
        {
            get
            {
                return _genderErrorMessage;
            }

            set
            {
                _genderErrorMessage = value;
                OnPropertyChanged(nameof(GenderErrorMessage));
            }
        }
        public string MobileNumErrorMessage
        {
            get
            {
                return _mobileNumErrorMessage;
            }

            set
            {
                _mobileNumErrorMessage = value;
                OnPropertyChanged(nameof(MobileNumErrorMessage));
            }
        }
        public string Gender
        {
            get
            {
                return _gender;
            }

            set
            {
                _gender = value;
                OnPropertyChanged(nameof(Gender));
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
        public bool PatientListVisibility
        {
            get
            {
                return _patientListVisibility;
            }

            set
            {
                _patientListVisibility = value;
                OnPropertyChanged(nameof(PatientListVisibility));
            }
        }

        //Commands
        public ICommand LoadSearchResults { get; }
        public ICommand UpdatePatientCommand { get; }
        public ICommand CreatePatientCommand { get; }
        public ICommand GetPatientDetails { get; }
        public ICommand ResetCommand { get; }
        public ICommand CancelCommand { get; }
        public ICommand BackToListCommand { get; }
        public ICommand DeleteCommand { get; }

        public PatientViewModel()
        {
            PatientList = new List<PatientView>();
            patientRepository = new PatientRepository();
            userRepository = new UserRepository();
            StatusList = new List<string>() { UserStatusEnum.Active.ToString(), UserStatusEnum.Inactive.ToString() };
            LoadPatientList(SearchWord);
            activityLogRepository = new ActivityLogRepository();
            currentUser = userRepository.GetByUsername(Properties.Settings.Default.Username);
            LoadSearchResults = new RelayCommand(ExecuteSearchCommand, CanExecuteForAllUsersCommand);
            GetPatientDetails = new RelayCommand(ExecuteGetUserDetailsCommand, CanExecuteGetUserDetailsCommand);
            UpdatePatientCommand = new RelayCommand(ExecuteUpdateCommand, CanExecuteForAdminsCommand);
            CreatePatientCommand = new RelayCommand(ExecuteCreateCommand, CanExecuteCreateCommand);
            ResetCommand = new RelayCommand(ExecuteGetUserDetailsCommand);
            CancelCommand = new RelayCommand(ExecuteCancelCommand);
            DeleteCommand = new RelayCommand(ExecuteDeleteCommand, CanExecuteDeleteUserCommand);
            BackToListCommand = new RelayCommand(ExecuteBackToListCommand);
            ButtonVisibility();
        }

        private void ExecuteDeleteCommand(object obj)
        {
            var oldData = patientRepository.GetByID(SelectedId);
            var deleteSuccess = patientRepository.Remove(SelectedId);
            if (deleteSuccess)
            {
                // Log the activity
                var log = new ActivityLog
                {
                    AffectedEntity = EntitiesEnum.Patients,
                    AffectedEntityId = SelectedId,
                    ActionType = ActionTypeEnum.Delete,
                    OldValues = JsonConvert.SerializeObject(oldData), // Serialize the whole object
                    NewValues = "-"
                };
                activityLogRepository.AddLog(log, currentUser);

                LoadPatientList("");
            }
        }

        private void ExecuteBackToListCommand(object obj)
        {
            UpdateGridVisibility = false;
            PatientListVisibility = true;
            CreateGridVisibility = false;
            LoadPatientList("");
        }

        private void ExecuteCancelCommand(object obj)
        {
            FirstName = string.Empty;
            LastName = string.Empty;
            Mobile = string.Empty;
            Telephone = string.Empty;
            Gender = string.Empty;
            Age = string.Empty;
            AllergicHistory = string.Empty;
            Weight = string.Empty;
            NIC = string.Empty;
            Status = string.Empty;

            //clear error msgs
            FirstNameErrorMessage = "";
            LastNameErrorMessage = "";
            AgeErrorMessage = "";
            MobileNumErrorMessage = "";
            GenderErrorMessage = "";
            NICErrorMessage = "";
        }

        private void ExecuteCreateCommand(object obj)
        {
            if (string.IsNullOrWhiteSpace(FirstName) || string.IsNullOrWhiteSpace(LastName) || string.IsNullOrWhiteSpace(Gender) ||
                string.IsNullOrWhiteSpace(NIC) || string.IsNullOrWhiteSpace(Age) || string.IsNullOrWhiteSpace(Mobile) || Mobile.Length != 10)
            {
                FirstNameErrorMessage = (string.IsNullOrWhiteSpace(FirstName)) ? "First name is required." : "";
                LastNameErrorMessage = (string.IsNullOrWhiteSpace(LastName)) ? "Last name is required." : "";
                AgeErrorMessage = (string.IsNullOrWhiteSpace(Age)) ? "Age is required." : "";
                GenderErrorMessage = (string.IsNullOrWhiteSpace(Gender)) ? "Gender is required." : "";
                NICErrorMessage = (string.IsNullOrWhiteSpace(NIC)) ? "NIC is required." : "";
                MobileNumErrorMessage = (string.IsNullOrWhiteSpace(Mobile)) ? "Mobile number is required." : (Mobile.Length != 10) ? "Should have 10 characters." : "";
            }
            else
            {
                Patient createPatient = new Patient()
                {
                    FirstName = FirstName,
                    LastName = LastName,
                    MobileNum = Mobile,
                    TelephoneNum = Telephone,
                    Gender = Gender,
                    Age = Age,
                    NIC = NIC ?? null,
                    AllergicHistory = AllergicHistory,
                    Weight = Weight,
                };
                var createSuccess = patientRepository.Add(createPatient);
                if (createSuccess)
                {
                    // Log the activity
                    var log = new ActivityLog
                    {
                        AffectedEntity = EntitiesEnum.Patients,
                        AffectedEntityId = createPatient.Id,
                        ActionType = ActionTypeEnum.Add,
                        OldValues = "-",
                        NewValues = JsonConvert.SerializeObject(createPatient)
                    };
                    activityLogRepository.AddLog(log, currentUser);
                    UpdateGridVisibility = false;
                    PatientListVisibility = true;
                    CreateGridVisibility = false;
                    LoadPatientList("");
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

        private void ExecuteGetUserDetailsCommand(object obj)
        {
            Gender = string.Empty;
            Patient patient = patientRepository.GetByID(SelectedId);
            FirstName = patient.FirstName;
            LastName = patient.LastName;
            Mobile = patient.MobileNum;
            Telephone = patient.TelephoneNum;
            Gender = patient.Gender;
            Age = patient.Age;
            NIC = patient.NIC;
            AllergicHistory = patient.AllergicHistory;
            Weight = patient.Weight;
            Status = patient.Status;
        }

        private void ExecuteUpdateCommand(object obj)
        {
            if (string.IsNullOrWhiteSpace(FirstName) || string.IsNullOrWhiteSpace(LastName) || string.IsNullOrWhiteSpace(Gender) ||
                string.IsNullOrWhiteSpace(NIC) || string.IsNullOrWhiteSpace(Age) || string.IsNullOrWhiteSpace(Mobile) || Mobile.Length != 10)
            {
                FirstNameErrorMessage = (string.IsNullOrWhiteSpace(FirstName)) ? "First name is required." : "";
                LastNameErrorMessage = (string.IsNullOrWhiteSpace(LastName)) ? "Last name is required." : "";
                AgeErrorMessage = (string.IsNullOrWhiteSpace(Age)) ? "Age is required." : "";
                GenderErrorMessage = (string.IsNullOrWhiteSpace(Gender)) ? "Gender is required." : "";
                NICErrorMessage = (string.IsNullOrWhiteSpace(NIC)) ? "NIC is required." : "";
                MobileNumErrorMessage = (string.IsNullOrWhiteSpace(Mobile)) ? "Mobile number is required." : (Mobile.Length != 10) ? "Should have 10 characters." : "";
            }
            else
            {
                Patient updatePatient = new Patient()
                {
                    Id = SelectedId,
                    FirstName = FirstName,
                    LastName = LastName,
                    MobileNum = Mobile,
                    TelephoneNum = (Telephone == "") ? null : Telephone,
                    Gender = Gender,
                    Age = Age,
                    NIC = NIC ?? null,
                    AllergicHistory = (AllergicHistory == "") ? null : AllergicHistory,
                    Weight = (Weight == "") ? null : Weight,
                    Status = (Status == "") ? UserStatusEnum.Active.ToString() : Status,
                };
                var oldData = patientRepository.GetByID(updatePatient.Id);
                bool editSuccess = patientRepository.Edit(updatePatient);
                if (editSuccess)
                {
                    // Log the activity
                    var log = new ActivityLog
                    {
                        AffectedEntity = EntitiesEnum.Patients,
                        AffectedEntityId = updatePatient.Id,
                        ActionType = ActionTypeEnum.Update,
                        OldValues = JsonConvert.SerializeObject(oldData), // Serialize the whole object
                        NewValues = JsonConvert.SerializeObject(updatePatient) // Serialize the whole object
                    };
                    activityLogRepository.AddLog(log, currentUser);

                    UpdateGridVisibility = false;
                    PatientListVisibility = true;
                    CreateGridVisibility = false;
                    LoadPatientList("");
                }
            }
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

        private bool CanExecuteCreateCommand(object obj)
        {
            var valideUser = Properties.Settings.Default.Username;
            var validEmployeeType = Properties.Settings.Default.EmployeeType;
            var validStatus = Properties.Settings.Default.Status;

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
            else if (valideUser != null && validEmployeeType.Equals(EmployeeTypeEnum.Manager.ToString()) && validStatus.Equals(UserStatusEnum.Active.ToString()))
            {
                UpdateButtonVisibility = true;
                CreateButtonVisibility = true;
                DeleteButtonVisibility = false;
            }
            else if (valideUser != null && validEmployeeType.Equals(EmployeeTypeEnum.Staff.ToString()) && validStatus.Equals(UserStatusEnum.Active.ToString()))
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
            LoadPatientList(SearchWord);
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

        private void LoadPatientList(string searchWord)
        {
            var patients = patientRepository.GetAll(searchWord);
            PatientList = patients;
            if (patients.Any())
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
