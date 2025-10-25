using Newtonsoft.Json;
using QWellApp.Enums;
using QWellApp.Models;
using QWellApp.Repositories;
using QWellApp.UserControls;
using QWellApp.ViewModels.Common;
using QWellApp.Views.UserControls;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net;
using System.Security;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;

namespace QWellApp.ViewModels
{
    public class EmployeeViewModel: ViewModelBase
    {
        //Fields
        private string _searchWord = "";
        public int _selectedId;
        private string _firstName;
        private string _lastName;
        private string _email;
        private string _mobile;
        private string _telephone;
        private string _username;
        private string _nic;
        private string _empType = EmployeeTypeEnum.Staff.ToString();
        private string _role = "Intern Nurse";
        private string _gender;
        private string _employeeStatus;
        private string _firstNameErrorMessage;
        private string _lastNameErrorMessage;
        private string _usernameErrorMessage;
        private string _emailErrorMessage;
        private string _genderErrorMessage;
        private string _mobileNumErrorMessage;
        private string _passwordErrorMessage;
        private string _confirmPasswordErrorMessage;
        private string _noResults;
        private SecureString _password;
        private SecureString _confirmPassword;
        private ViewModelBase _currentChildView;
        private IEnumerable<UserView> _userList;
        private IEnumerable<string> _employeeTypeList;
        private Dictionary<int, string> _roleList;
        private IEnumerable<string> _statusList;
        private bool _updateButtonVisibility = false;
        private bool _createButtonVisibility = false;
        private bool _deleteButtonVisibility = false;
        private bool _resetUpdateButtonsVisibility = false;
        private bool _updateGridVisibility = false;
        private bool _createGridVisibility = false;
        private bool _userListVisibility = true;
        private bool _refresh = false;

        private IUserRepository userRepository;
        private IActivityLogRepository activityLogRepository;
        private IRoleRepository roleRepository;
        private UserDetails currentUser;

        //Properties 
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
        public string Email
        {
            get
            {
                return _email;
            }
            set
            {
                _email = value;
                OnPropertyChanged(nameof(Email));
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

        public string Username
        {
            get
            {
                return _username;
            }

            set
            {
                _username = value;
                OnPropertyChanged(nameof(Username));
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

        public string EmployeeType
        {
            get
            {
                return _empType;
            }

            set
            {
                _empType = value;
                OnPropertyChanged(nameof(EmployeeType));
            }
        }
        public string Role
        {
            get
            {
                return _role;
            }

            set
            {
                _role = value;
                OnPropertyChanged(nameof(Role));
            }
        }
        public string EmployeeStatus
        {
            get
            {
                return _employeeStatus;
            }

            set
            {
                _employeeStatus = value;
                OnPropertyChanged(nameof(EmployeeStatus));
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
        public string UsernameErrorMessage
        {
            get
            {
                return _usernameErrorMessage;
            }

            set
            {
                _usernameErrorMessage = value;
                OnPropertyChanged(nameof(UsernameErrorMessage));
            }
        }

        public string EmailErrorMessage
        {
            get
            {
                return _emailErrorMessage;
            }

            set
            {
                _emailErrorMessage = value;
                OnPropertyChanged(nameof(EmailErrorMessage));
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
        public string PasswordErrorMessage
        {
            get
            {
                return _passwordErrorMessage;
            }

            set
            {
                _passwordErrorMessage = value;
                OnPropertyChanged(nameof(PasswordErrorMessage));
            }
        }
        public string ConfirmPasswordErrorMessage
        {
            get
            {
                return _confirmPasswordErrorMessage;
            }

            set
            {
                _confirmPasswordErrorMessage = value;
                OnPropertyChanged(nameof(ConfirmPasswordErrorMessage));
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

        public IEnumerable<UserView> UserList
        {
            get
            {
                return _userList;
            }
            set
            {
                _userList = value;
                OnPropertyChanged(nameof(UserList));
            }
        }

        public IEnumerable<string> EmployeeTypeList
        {
            get
            {
                return _employeeTypeList;
            }
            set
            {
                _employeeTypeList = value;
                OnPropertyChanged(nameof(EmployeeTypeList));
            }
        }
        public Dictionary<int, string> RoleList
        {
            get
            {
                return _roleList;
            }
            set
            {
                _roleList = value;
                OnPropertyChanged(nameof(RoleList));
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
        public SecureString Password
        {
            get
            {
                return _password;
            }
            set
            {
                _password = value;
                OnPropertyChanged(nameof(Password));
            }
        }
        public SecureString ConfirmPassword
        {
            get
            {
                return _confirmPassword;
            }
            set
            {
                _confirmPassword = value;
                OnPropertyChanged(nameof(ConfirmPassword));
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
        public bool UserListVisibility
        {
            get
            {
                return _userListVisibility;
            }

            set
            {
                _userListVisibility = value;
                OnPropertyChanged(nameof(UserListVisibility));
            }
        }

        public bool Refresh
        {
            get
            {
                return _refresh;
            }

            set
            {
                _refresh = value;
                OnPropertyChanged(nameof(Refresh));
            }
        }

        //Commands
        public ICommand LoadSearchResults { get; }
        public ICommand UpdateUserCommand { get; }
        public ICommand CreateUserCommand { get; }
        public ICommand GetUserDetails { get; }
        public ICommand ResetCommand { get; }
        public ICommand CancelCommand { get; }
        public ICommand BackToListCommand { get; }
        public ICommand DeleteCommand { get; }

        public EmployeeViewModel()
        {
            UserList = new List<UserView>();
            EmployeeTypeList = new List<string>() { EmployeeTypeEnum.Admin.ToString(), EmployeeTypeEnum.Manager.ToString(), EmployeeTypeEnum.Staff.ToString() };
            RoleList = new Dictionary<int, string> ();
            StatusList = new List<string>() { UserStatusEnum.Active.ToString(), UserStatusEnum.Inactive.ToString() };
            userRepository = new UserRepository();
            roleRepository = new RoleRepository();
            activityLogRepository = new ActivityLogRepository();
            LoadSearchResults = new RelayCommand(ExecuteSearchCommand, CanExecuteForAllUsersCommand);
            GetUserDetails = new RelayCommand(ExecuteGetUserDetailsCommand, CanExecuteGetUserDetailsCommand);
            UpdateUserCommand = new RelayCommand(ExecuteUpdateCommand, CanExecuteForAdminsCommand);
            CreateUserCommand = new RelayCommand(ExecuteCreateCommand, CanExecuteCreateCommand);
            ResetCommand = new RelayCommand(ExecuteGetUserDetailsCommand);
            CancelCommand = new RelayCommand(ExecuteCancelCommand);
            DeleteCommand = new RelayCommand(ExecuteDeleteCommand, CanExecuteDeleteUserCommand);
            BackToListCommand = new RelayCommand(ExecuteBackToListCommand);
            //LoadUserList(SearchWord);
            LoadPositionList();
            ButtonVisibility();
            currentUser = userRepository.GetByUsername(Properties.Settings.Default.Username);
        }

        private void ExecuteDeleteCommand(object obj)
        {
            var oldData = userRepository.GetByID(SelectedId);
            var deleteSuccess = userRepository.Remove(SelectedId);
            if (deleteSuccess)
            {
                // Log the activity
                var log = new ActivityLog
                {
                    AffectedEntity = EntitiesEnum.Employees,
                    AffectedEntityId = SelectedId,
                    ActionType = ActionTypeEnum.Delete,
                    OldValues = JsonConvert.SerializeObject(oldData), // Serialize the whole object
                    NewValues = "-"
                };
                activityLogRepository.AddLog(log, currentUser);
                LoadUserList("");
            }
        }

        private void ExecuteBackToListCommand(object obj)
        {
            UpdateGridVisibility = false;
            UserListVisibility = true;
            CreateGridVisibility = false;
            LoadUserList("");
        }

        private void ExecuteCancelCommand(object obj)
        {
            FirstName = string.Empty;
            LastName = string.Empty;
            Email = string.Empty;
            Mobile = string.Empty;
            Telephone = string.Empty;
            Username = string.Empty;
            NIC = string.Empty;
            EmployeeType = string.Empty;
            Role = string.Empty;
            Gender = string.Empty;
            EmployeeStatus = string.Empty;
            new System.Net.NetworkCredential(string.Empty, Password).Password = null;
            Password = null;
            PasswordUserControl xx = new PasswordUserControl();
            xx.txtPassword.Clear();
            xx.txtPassword.Clear();

            //clear error msgs
            FirstNameErrorMessage = "";
            LastNameErrorMessage = "";
            UsernameErrorMessage = "";
            EmailErrorMessage = "";
            GenderErrorMessage = "";
            MobileNumErrorMessage = "";
            PasswordErrorMessage = "";
            ConfirmPasswordErrorMessage = "";

        }

        private bool CanExecuteCreateCommand(object obj)
        {
            var valideUser = Properties.Settings.Default.Username;
            var validEmployeeType = Properties.Settings.Default.EmployeeType;
            var validStatus = Properties.Settings.Default.Status;

            if (valideUser != null && (validEmployeeType.Equals(EmployeeTypeEnum.Admin.ToString()) || validEmployeeType.Equals(EmployeeTypeEnum.Manager.ToString())) && validStatus.Equals(UserStatusEnum.Active.ToString()))
            {
                UsernameErrorMessage = (!string.IsNullOrWhiteSpace(Username) && Username.Length < 3) ? "Username should have atleast 3 characters." : "";
                PasswordErrorMessage = (!string.IsNullOrWhiteSpace(new System.Net.NetworkCredential(string.Empty, Password).Password) && Password.Length < 3) ? "Password should have atleast 3 characters." : "";
                ConfirmPasswordErrorMessage = (!string.IsNullOrWhiteSpace(new System.Net.NetworkCredential(string.Empty, ConfirmPassword).Password) &&
                    new System.Net.NetworkCredential(string.Empty, Password).Password != new System.Net.NetworkCredential(string.Empty, ConfirmPassword).Password) ? "Password and Confirm Password should be same." : "";

                return true;
            }
            else
            {
                return false;
            }
        }

        private void ExecuteCreateCommand(object obj)
        {
            string emailPattern = @"^[^@\s]+@[^@\s]+\.[^@\s]+$"; // Regex for email validation

            if (string.IsNullOrWhiteSpace(FirstName) || string.IsNullOrWhiteSpace(LastName) || string.IsNullOrWhiteSpace(Username) || string.IsNullOrWhiteSpace(Email) ||
                !Regex.IsMatch(Email, emailPattern) || string.IsNullOrWhiteSpace(Gender) || string.IsNullOrWhiteSpace(Mobile) || Mobile.Length != 10 || 
                string.IsNullOrWhiteSpace(new System.Net.NetworkCredential(string.Empty, Password).Password) ||
                string.IsNullOrWhiteSpace(new System.Net.NetworkCredential(string.Empty, ConfirmPassword).Password))
            {
                FirstNameErrorMessage = (string.IsNullOrWhiteSpace(FirstName)) ? "First name is required." : "";
                LastNameErrorMessage = (string.IsNullOrWhiteSpace(LastName)) ? "Last name is required." : "";
                UsernameErrorMessage = (string.IsNullOrWhiteSpace(Username)) ? "Username is required." : (Username.Length < 3) ? "Username should have atleast 3 characters." : "";
                EmailErrorMessage = string.IsNullOrWhiteSpace(Email) ? "Email is required." : (!Regex.IsMatch(Email, emailPattern) ? "Invalid email format." : ""); // Validate email format
                GenderErrorMessage = (string.IsNullOrWhiteSpace(Gender)) ? "Gender is required." : "";
                MobileNumErrorMessage = (string.IsNullOrWhiteSpace(Mobile)) ? "Mobile number is required." : (Mobile.Length != 10) ? "Mobile number should have 10 characters." : "";
                PasswordErrorMessage = (string.IsNullOrWhiteSpace(new System.Net.NetworkCredential(string.Empty, Password).Password)) ? "Password is required." : (Password.Length < 3) ? "Password should have atleast 3 characters." : "";
                ConfirmPasswordErrorMessage = (string.IsNullOrWhiteSpace(new System.Net.NetworkCredential(string.Empty, ConfirmPassword).Password)) ? "Confirm password is required." : (new System.Net.NetworkCredential(string.Empty, Password).Password != new System.Net.NetworkCredential(string.Empty, ConfirmPassword).Password) ? "Password and Confirm Password should be same." : "";
            }
            else
            {
                UserDetails createUser = new UserDetails()
                {
                    FirstName = FirstName,
                    LastName = LastName,
                    Email = Email,
                    MobileNum = Mobile,
                    TelephoneNum = Telephone,
                    Username = Username,
                    NIC = NIC,
                    Password = new System.Net.NetworkCredential(string.Empty, Password).Password,
                    EmployeeType = EmployeeType,
                    RoleId = RoleList.FirstOrDefault(x => x.Value == Role).Key,
                    Gender = Gender
                };
                if (Role == null)
                {
                    createUser.RoleId = RoleList.FirstOrDefault(x => x.Value == "Intern Nurse").Key;
                }
                if (EmployeeType == null)
                {
                    createUser.EmployeeType = EmployeeTypeEnum.Staff.ToString();
                }
                var createSuccess = userRepository.Add(createUser);
                if (createSuccess)
                {
                    // Log the activity
                    var log = new ActivityLog
                    {
                        AffectedEntity = EntitiesEnum.Employees,
                        AffectedEntityId = createUser.Id,
                        ActionType = ActionTypeEnum.Add,
                        OldValues = "-",
                        NewValues = JsonConvert.SerializeObject(new
                        {
                            createUser.FirstName,
                            createUser.LastName,
                            createUser.Email,
                            createUser.MobileNum,
                            createUser.TelephoneNum,
                            createUser.Username,
                            createUser.NIC,
                            createUser.EmployeeType,
                            createUser.RoleId,
                            createUser.Gender
                        }) // Excludes Password manually
                    };
                    activityLogRepository.AddLog(log, currentUser);
                    UpdateGridVisibility = false;
                    UserListVisibility = true;
                    CreateGridVisibility = false;
                    LoadUserList("");
                    Password = null;
                    ConfirmPassword = null;
                }
            }
        }

        private bool CanExecuteGetUserDetailsCommand(object obj)
        {
            if (SelectedId > 0)
            {
                return true;
            } else
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
            UserDetails user = userRepository.GetByID(SelectedId);
            FirstName = user.FirstName;
            LastName = user.LastName;
            Email = user.Email;
            Mobile = user.MobileNum;
            Telephone = user.TelephoneNum;
            Username = user.Username;
            NIC = user.NIC;
            EmployeeType = user.EmployeeType;
            Role = RoleList[user.RoleId];
            Gender = user.Gender;
            EmployeeStatus = user.Status;
        }

        private void LoadPositionList()
        {
            Dictionary<int, string> roles = new Dictionary<int, string>();
            var roleList = roleRepository.GetAll();
            foreach ( var role in roleList )
            {
                roles.Add(role.Id, role.RoleName);
            }
            RoleList = roles;
        }

        private void ExecuteUpdateCommand(object obj)
        {
            string emailPattern = @"^[^@\s]+@[^@\s]+\.[^@\s]+$"; // Regex for email validation

            if (string.IsNullOrWhiteSpace(FirstName) || string.IsNullOrWhiteSpace(LastName) || string.IsNullOrWhiteSpace(Username) || string.IsNullOrWhiteSpace(Email) ||
                !Regex.IsMatch(Email, emailPattern) || string.IsNullOrWhiteSpace(Gender) || string.IsNullOrWhiteSpace(Mobile) || Mobile.Length != 10)
            {
                FirstNameErrorMessage = (string.IsNullOrWhiteSpace(FirstName)) ? "First name is required." : "";
                LastNameErrorMessage = (string.IsNullOrWhiteSpace(LastName)) ? "Last name is required." : "";
                UsernameErrorMessage = (string.IsNullOrWhiteSpace(Username)) ? "Username is required." : (Username.Length < 3) ? "Username should have atleast 3 characters." : "";
                EmailErrorMessage = string.IsNullOrWhiteSpace(Email) ? "Email is required." : (!Regex.IsMatch(Email, emailPattern) ? "Invalid email format." : ""); // Validate email format
                GenderErrorMessage = (string.IsNullOrWhiteSpace(Gender)) ? "Gender is required." : "";
                MobileNumErrorMessage = (string.IsNullOrWhiteSpace(Mobile)) ? "Mobile number is required." : (Mobile.Length != 10) ? "Mobile number should have 10 characters." : "";
            }
            else
            {
                UserDetails updateUser = new UserDetails()
                {
                    Id = SelectedId,
                    FirstName = (FirstName == "") ? null : FirstName,
                    LastName = (LastName == "") ? null : LastName,
                    Email = (Email == "") ? null : Email,
                    MobileNum = (Mobile == "") ? null : Mobile,
                    TelephoneNum = (Telephone == "") ? null : Telephone,
                    Username = (Username == "") ? null : Username,
                    NIC = (NIC == "") ? null : NIC,
                    EmployeeType = (EmployeeType == null) ? EmployeeTypeEnum.Staff.ToString() : EmployeeType,
                    RoleId = (Role == null) ? RoleList.FirstOrDefault(x => x.Value == "Intern Nurse").Key : RoleList.FirstOrDefault(x => x.Value == Role).Key,
                    Gender = (Gender == "") ? null : Gender,
                    Status = (EmployeeStatus == "") ? UserStatusEnum.Active.ToString() : EmployeeStatus,
                };
                var oldData = userRepository.GetByID(updateUser.Id);
                bool editSuccess = userRepository.Edit(updateUser);
                if (editSuccess)
                {
                    // Log the activity
                    var log = new ActivityLog
                    {
                        AffectedEntity = EntitiesEnum.Employees,
                        AffectedEntityId = updateUser.Id,
                        ActionType = ActionTypeEnum.Update,
                        OldValues = JsonConvert.SerializeObject(oldData), // Serialize the whole object
                        NewValues = JsonConvert.SerializeObject(updateUser) // Serialize the whole object
                    };
                    activityLogRepository.AddLog(log, currentUser);

                    UpdateGridVisibility = false;
                    UserListVisibility = true;
                    CreateGridVisibility = false;
                    LoadUserList("");
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
            LoadUserList(SearchWord);
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

        public void LoadUserList(string SearchWord)
        {
            var users = userRepository.GetAll(SearchWord);
            UserList = users;
            if (users.Any())
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
