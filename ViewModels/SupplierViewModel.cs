using Microsoft.VisualBasic.ApplicationServices;
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
    public class SupplierViewModel: ViewModelBase
    {
        //Fields
        private IEnumerable<SupplierView> _supplierList;
        private string _noResults;
        private string _searchWord = "";
        public int _selectedId;
        private string _company;
        private string _email;
        private string _address;
        private string _telephone;
        private string _status;
        private IEnumerable<string> _statusList;
        private string _companyErrorMessage;
        private string _emailErrorMessage;
        private string _addressErrorMessage;
        private string _telephoneNumErrorMessage;
        private ViewModelBase _currentChildView;
        private bool _updateButtonVisibility = false;
        private bool _createButtonVisibility = false;
        private bool _deleteButtonVisibility = false;
        private bool _resetUpdateButtonsVisibility = false;
        private bool _updateGridVisibility = false;
        private bool _createGridVisibility = false;
        private bool _supplierListVisibility = true;

        private ISupplierRepository supplierRepository;
        private IUserRepository userRepository;

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
        public string Company
        {
            get
            {
                return _company;
            }
            set
            {
                _company = value;
                OnPropertyChanged(nameof(Company));
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
        public string Address
        {
            get
            {
                return _address;
            }
            set
            {
                _address = value;
                OnPropertyChanged(nameof(Address));
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
        public string CompanyErrorMessage
        {
            get
            {
                return _companyErrorMessage;
            }

            set
            {
                _companyErrorMessage = value;
                OnPropertyChanged(nameof(CompanyErrorMessage));
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
        public string TelephoneNumErrorMessage
        {
            get
            {
                return _telephoneNumErrorMessage;
            }

            set
            {
                _telephoneNumErrorMessage = value;
                OnPropertyChanged(nameof(TelephoneNumErrorMessage));
            }
        }
        public string AddressErrorMessage
        {
            get
            {
                return _addressErrorMessage;
            }

            set
            {
                _addressErrorMessage = value;
                OnPropertyChanged(nameof(AddressErrorMessage));
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
        public bool SupplierListVisibility
        {
            get
            {
                return _supplierListVisibility;
            }

            set
            {
                _supplierListVisibility = value;
                OnPropertyChanged(nameof(SupplierListVisibility));
            }
        }
        public IEnumerable<SupplierView> SupplierList
        {
            get
            {
                return _supplierList;
            }
            set
            {
                _supplierList = value;
                OnPropertyChanged(nameof(SupplierList));
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
        public ICommand UpdateSupplierCommand { get; }
        public ICommand CreateSupplierCommand { get; }
        public ICommand GetSupplierDetails { get; }
        public ICommand ResetCommand { get; }
        public ICommand CancelCommand { get; }
        public ICommand BackToListCommand { get; }
        public ICommand DeleteCommand { get; }

        public SupplierViewModel()
        {
            SupplierList = new List<SupplierView>();
            supplierRepository = new SupplierRepository();
            StatusList = new List<string>() { UserStatusEnum.Active.ToString(), UserStatusEnum.Inactive.ToString() };
            LoadSupplierList(SearchWord);
            userRepository = new UserRepository();
            LoadSearchResults = new RelayCommand(ExecuteSearchCommand, CanExecuteForAllUsersCommand);
            GetSupplierDetails = new RelayCommand(ExecuteGetUserDetailsCommand, CanExecuteGetUserDetailsCommand);
            UpdateSupplierCommand = new RelayCommand(ExecuteUpdateCommand, CanExecuteForAdminsCommand);
            CreateSupplierCommand = new RelayCommand(ExecuteCreateCommand, CanExecuteForAdminsCommand);
            ResetCommand = new RelayCommand(ExecuteGetUserDetailsCommand);
            CancelCommand = new RelayCommand(ExecuteCancelCommand);
            DeleteCommand = new RelayCommand(ExecuteDeleteCommand, CanExecuteDeleteUserCommand);
            BackToListCommand = new RelayCommand(ExecuteBackToListCommand);
            ButtonVisibility();
        }

        private void ExecuteDeleteCommand(object obj)
        {
            var deleteSuccess = supplierRepository.Remove(SelectedId);
            if (deleteSuccess)
            {
                LoadSupplierList("");
            }
        }

        private void ExecuteBackToListCommand(object obj)
        {
            UpdateGridVisibility = false;
            SupplierListVisibility = true;
            CreateGridVisibility = false;
            LoadSupplierList("");
        }

        private void ExecuteCancelCommand(object obj)
        {
            Company = string.Empty;
            Email = string.Empty;
            Address = string.Empty;
            Telephone = string.Empty;
            Status = string.Empty;

            //clear error msgs
            CompanyErrorMessage = "";
            EmailErrorMessage = "";
            AddressErrorMessage = "";
            TelephoneNumErrorMessage = "";

        }

        private void ExecuteCreateCommand(object obj)
        {
            if (string.IsNullOrWhiteSpace(Company) || string.IsNullOrWhiteSpace(Address) || 
                string.IsNullOrWhiteSpace(Email) || string.IsNullOrWhiteSpace(Telephone) || Telephone.Length != 10)
            {
                CompanyErrorMessage = (string.IsNullOrWhiteSpace(Company)) ? "Company is required." : "";
                EmailErrorMessage = (string.IsNullOrWhiteSpace(Email)) ? "Email address is required." : "";
                AddressErrorMessage = (string.IsNullOrWhiteSpace(Address)) ? "Address is required." : "";
                TelephoneNumErrorMessage = (string.IsNullOrWhiteSpace(Telephone)) ? "Telephone number is required." : (Telephone.Length != 10) ? "Telephone number should have 10 characters." : "";
            }
            else
            {
                Supplier createSupplier = new Supplier()
                {
                    CompanyName = Company,
                    Email = Email,
                    Address = Address,
                    TelephoneNum = Telephone
                };
                var createSuccess = supplierRepository.Add(createSupplier);
                if (createSuccess)
                {
                    UpdateGridVisibility = false;
                    SupplierListVisibility = true;
                    CreateGridVisibility = false;
                    LoadSupplierList("");
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
            Supplier supplier = supplierRepository.GetByID(SelectedId);
            Company = supplier.CompanyName;
            Email = supplier.Email;
            Address = supplier.Address;
            Telephone = supplier.TelephoneNum;
            Status = supplier.Status;
        }

        private void ExecuteUpdateCommand(object obj)
        {
            if (string.IsNullOrWhiteSpace(Company) || string.IsNullOrWhiteSpace(Address) ||
                string.IsNullOrWhiteSpace(Email) || string.IsNullOrWhiteSpace(Telephone) || Telephone.Length != 10)
            {
                CompanyErrorMessage = (string.IsNullOrWhiteSpace(Company)) ? "Company is required." : "";
                EmailErrorMessage = (string.IsNullOrWhiteSpace(Email)) ? "Email address is required." : "";
                AddressErrorMessage = (string.IsNullOrWhiteSpace(Address)) ? "Address is required." : "";
                TelephoneNumErrorMessage = (string.IsNullOrWhiteSpace(Telephone)) ? "Telephone number is required." : (Telephone.Length != 10) ? "Telephone number should have 10 characters." : "";
            }
            else
            {
                Supplier updateSupplier = new Supplier()
                {
                    Id = SelectedId,
                    CompanyName = Company,
                    Email = Email,
                    Address = Address,
                    TelephoneNum = Telephone,
                    Status = (Status == "") ? UserStatusEnum.Active.ToString() : Status,
                };
                bool editSuccess = supplierRepository.Edit(updateSupplier);
                if (editSuccess)
                {
                    UpdateGridVisibility = false;
                    SupplierListVisibility = true;
                    CreateGridVisibility = false;
                    LoadSupplierList("");
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
            LoadSupplierList(SearchWord);
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
        private void LoadSupplierList(string searchWord)
        {
            var suppliers = supplierRepository.GetAll(searchWord);
            SupplierList = suppliers;
            if (suppliers.Any())
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
