using Microsoft.VisualBasic.ApplicationServices;
using Newtonsoft.Json;
using QWellApp.Enums;
using QWellApp.Models;
using QWellApp.Repositories;
using QWellApp.ViewModels.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Window;

namespace QWellApp.ViewModels
{
    public class LabTestViewModel : ViewModelBase
    {
        private IEnumerable<LabTestView> _labTestList;
        private string _noResults;
        private string _searchWord = "";
        public int _selectedId;
        private string _hospitalName;
        private string _testName;
        private float _cost;
        private float? _discount;
        private string? _labPaid;
        private string _status;
        private IEnumerable<string> _statusList;
        private string _hospitalNameErrorMessage;
        private string _testNameErrorMessage;
        private string _costErrorMessage;
        private ViewModelBase _currentChildView;
        private bool _updateButtonVisibility = false;
        private bool _createButtonVisibility = false;
        private bool _deleteButtonVisibility = false;
        private bool _resetUpdateButtonsVisibility = false;
        private bool _updateGridVisibility = false;
        private bool _createGridVisibility = false;
        private bool _labTestListVisibility = true;

        private IUserRepository userRepository;
        private ILabTestRepository labTestRepository;
        private IActivityLogRepository activityLogRepository;
        private UserDetails currentUser;

        //Properties 
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

        public string HospitalName
        {
            get { return _hospitalName; }
            set
            {
                _hospitalName = value;
                OnPropertyChanged(nameof(HospitalName));
            }
        }

        public string TestName
        {
            get { return _testName; }
            set
            {
                _testName = value;
                OnPropertyChanged(nameof(TestName));
            }
        }

        public float Cost
        {
            get { return _cost; }
            set
            {
                _cost = value;
                OnPropertyChanged(nameof(Cost));
            }
        }

        public float? Discount
        {
            get { return _discount; }
            set
            {
                _discount = value;
                OnPropertyChanged(nameof(Discount));
            }
        }

        public string? LabPaid
        {
            get { return _labPaid; }
            set
            {
                _labPaid = value;
                OnPropertyChanged(nameof(LabPaid));
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

        public string HospitalNameErrorMessage
        {
            get { return _hospitalNameErrorMessage; }
            set
            {
                _hospitalNameErrorMessage = value;
                OnPropertyChanged(nameof(HospitalNameErrorMessage));
            }
        }

        public string TestNameErrorMessage
        {
            get { return _testNameErrorMessage; }
            set
            {
                _testNameErrorMessage = value;
                OnPropertyChanged(nameof(TestNameErrorMessage));
            }
        }

        public string CostErrorMessage
        {
            get { return _costErrorMessage; }
            set
            {
                _costErrorMessage = value;
                OnPropertyChanged(nameof(CostErrorMessage));
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
        public bool LabTestListVisibility
        {
            get
            {
                return _labTestListVisibility;
            }

            set
            {
                _labTestListVisibility = value;
                OnPropertyChanged(nameof(LabTestListVisibility));
            }
        }

        public IEnumerable<LabTestView> LabTestList
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

        //Commands
        public ICommand LoadSearchResults { get; }
        public ICommand UpdateLabTestCommand { get; }
        public ICommand CreateLabTestCommand { get; }
        public ICommand GetLabTestDetails { get; }
        public ICommand ResetCommand { get; }
        public ICommand CancelCommand { get; }
        public ICommand BackToListCommand { get; }
        public ICommand DeleteCommand { get; }

        public LabTestViewModel()
        {
            LabTestList = new List<LabTestView>();
            labTestRepository = new LabTestRepository();
            userRepository = new UserRepository();
            StatusList = new List<string>() { UserStatusEnum.Active.ToString(), UserStatusEnum.Inactive.ToString() };
            LoadLabTestList(SearchWord);
            activityLogRepository = new ActivityLogRepository();
            currentUser = userRepository.GetByUsername(Properties.Settings.Default.Username);
            LoadSearchResults = new RelayCommand(ExecuteSearchCommand, CanExecuteForAllUsersCommand);
            GetLabTestDetails = new RelayCommand(ExecuteGetLabTestDetailsCommand, CanExecuteGetLabTestDetailsCommand);
            UpdateLabTestCommand = new RelayCommand(ExecuteUpdateCommand, CanExecuteForAdminsCommand);
            CreateLabTestCommand = new RelayCommand(ExecuteCreateCommand, CanExecuteCreateCommand);
            ResetCommand = new RelayCommand(ExecuteGetLabTestDetailsCommand);
            CancelCommand = new RelayCommand(ExecuteCancelCommand);
            DeleteCommand = new RelayCommand(ExecuteDeleteCommand, CanExecuteDeleteUserCommand);
            BackToListCommand = new RelayCommand(ExecuteBackToListCommand);
            ButtonVisibility();
        }

        private void ExecuteDeleteCommand(object obj)
        {
            var oldData = labTestRepository.GetByID(SelectedId);
            var deleteSuccess = labTestRepository.Remove(SelectedId);
            if (deleteSuccess)
            {
                // Log the activity
                var log = new ActivityLog
                {
                    AffectedEntity = EntitiesEnum.LabTests,
                    AffectedEntityId = SelectedId,
                    ActionType = ActionTypeEnum.Delete,
                    OldValues = JsonConvert.SerializeObject(oldData), // Serialize the whole object
                    NewValues = "-"
                };
                activityLogRepository.AddLog(log, currentUser);
                LoadLabTestList("");
            }
        }

        private void ExecuteBackToListCommand(object obj)
        {
            UpdateGridVisibility = false;
            LabTestListVisibility = true;
            CreateGridVisibility = false;
            LoadLabTestList("");
        }

        private void ExecuteCancelCommand(object obj)
        {
            HospitalName = string.Empty;
            TestName = string.Empty;
            Cost = 0;
            Discount = null;
            LabPaid = string.Empty;
            Status = string.Empty;

            //clear error msgs
            HospitalNameErrorMessage = "";
            TestNameErrorMessage = "";
            CostErrorMessage = "";
        }

        private void ExecuteCreateCommand(object obj)
        {
            if (string.IsNullOrWhiteSpace(HospitalName) || string.IsNullOrWhiteSpace(TestName) || string.IsNullOrWhiteSpace(Cost.ToString()))
            {
                HospitalNameErrorMessage = string.IsNullOrWhiteSpace(HospitalName) ? "Hospital name is required." : "";
                TestNameErrorMessage = string.IsNullOrWhiteSpace(TestName) ? "Test name is required." : "";
                CostErrorMessage = (Cost <= 0) ? "Cost must be a positive value." : "";

            }
            else
            {
                LabTest createLabTest = new LabTest()
                {
                    HospitalName = HospitalName,
                    TestName = TestName,
                    Cost = Cost,
                    Discount = Discount,
                    LabPaid = LabPaid,
                };
                var createSuccess = labTestRepository.Add(createLabTest);
                if (createSuccess)
                {
                    // Log the activity
                    var log = new ActivityLog
                    {
                        AffectedEntity = EntitiesEnum.LabTests,
                        AffectedEntityId = createLabTest.Id,
                        ActionType = ActionTypeEnum.Add,
                        OldValues = "-",
                        NewValues = JsonConvert.SerializeObject(createLabTest)
                    };
                    activityLogRepository.AddLog(log, currentUser);
                    UpdateGridVisibility = false;
                    LabTestListVisibility = true;
                    CreateGridVisibility = false;
                    LoadLabTestList("");
                }
            }
        }

        private bool CanExecuteGetLabTestDetailsCommand(object obj)
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

        private void ExecuteGetLabTestDetailsCommand(object obj)
        {
            LabTest labTest = labTestRepository.GetByID(SelectedId);
            HospitalName = labTest.HospitalName;
            TestName = labTest.TestName;
            Cost = labTest.Cost;
            Discount = labTest.Discount;
            LabPaid = labTest.LabPaid;
            Status = labTest.Status;
        }

        private void ExecuteUpdateCommand(object obj)
        {
            if (string.IsNullOrWhiteSpace(HospitalName) || string.IsNullOrWhiteSpace(TestName) || string.IsNullOrWhiteSpace(Cost.ToString()))
            {
                HospitalNameErrorMessage = string.IsNullOrWhiteSpace(HospitalName) ? "Hospital name is required." : "";
                TestNameErrorMessage = string.IsNullOrWhiteSpace(TestName) ? "Test name is required." : "";
                CostErrorMessage = (Cost <= 0) ? "Cost must be a positive value." : "";

            }
            else
            {
                LabTest updateLabTest = new LabTest()
                {
                    Id = SelectedId,
                    HospitalName = HospitalName,
                    TestName = TestName,
                    Cost = Cost,
                    Discount = Discount,
                    LabPaid = LabPaid,
                    Status = (Status == "") ? UserStatusEnum.Active.ToString() : Status,
                };
                var oldData = labTestRepository.GetByID(updateLabTest.Id);
                bool editSuccess = labTestRepository.Edit(updateLabTest);
                if (editSuccess)
                {
                    // Log the activity
                    var log = new ActivityLog
                    {
                        AffectedEntity = EntitiesEnum.LabTests,
                        AffectedEntityId = updateLabTest.Id,
                        ActionType = ActionTypeEnum.Update,
                        OldValues = JsonConvert.SerializeObject(oldData), // Serialize the whole object
                        NewValues = JsonConvert.SerializeObject(updateLabTest) // Serialize the whole object
                    };
                    activityLogRepository.AddLog(log, currentUser);
                    UpdateGridVisibility = false;
                    LabTestListVisibility = true;
                    CreateGridVisibility = false;
                    LoadLabTestList("");
                }
            }
        }

        private bool CanExecuteCreateCommand(object obj)
        {
            var valideUser = Properties.Settings.Default.Username;
            var validEmployeeType = Properties.Settings.Default.EmployeeType;
            var validStatus = Properties.Settings.Default.Status;
            LabPaid = (Cost - ((Cost*Discount)/100)).ToString();

            if (valideUser != null && (validEmployeeType.Equals(EmployeeTypeEnum.Admin.ToString()) || validEmployeeType.Equals(EmployeeTypeEnum.Manager.ToString())) && validStatus.Equals(UserStatusEnum.Active.ToString()))
            {
                return true;
            }
            else
            {
                return false;
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
            else
            {
                UpdateButtonVisibility = false;
                CreateButtonVisibility = false;
                DeleteButtonVisibility = false;
            }
        }

        private void ExecuteSearchCommand(object obj)
        {
            LoadLabTestList(SearchWord);
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

        private void LoadLabTestList(string searchWord)
        {
            var labTests = labTestRepository.GetAll(searchWord);
            LabTestList = labTests;
            if (labTests.Any())
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
