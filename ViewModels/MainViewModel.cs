using FontAwesome.Sharp;
using QWell;
using QWellApp.Models;
using QWellApp.Repositories;
using QWellApp.ViewModels.Common;
using QWellApp.Views.Pages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;

namespace QWellApp.ViewModels
{
    public class MainViewModel: ViewModelBase
    {
        //Fields
        private UserAccount _currentUserAccount;
        private ViewModelBase _currentChildView;
        private string _caption;
        private IconChar _icon;
        private bool _isMainViewVisible = false;

        private IUserRepository userRepository;

        //Properties
        public UserAccount CurrentUserAccount
        {
            get 
            { 
                return _currentUserAccount; 
            }
            set
            {
                _currentUserAccount = value;
                OnPropertyChanged(nameof(CurrentUserAccount));
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
        public string Caption
        {
            get 
            { 
                return _caption; 
            }
            set
            {
                _caption = value;
                OnPropertyChanged(nameof(Caption));
            }
        }
        public IconChar Icon
        {
            get 
            { 
                return _icon; 
            }
            set
            {
                _icon = value;
                OnPropertyChanged(nameof(Icon));
            }
        }
        public bool IsMainViewVisible
        {
            get
            {
                return _isMainViewVisible;
            }

            set
            {
                _isMainViewVisible = value;
                OnPropertyChanged(nameof(IsMainViewVisible));
            }
        }

        //Commands
        public ICommand ShowProductViewCommand { get; }
        public ICommand ShowProductRecordViewCommand { get; }
        public ICommand ShowStockViewCommand { get; }
        public ICommand ShowPatientViewCommand { get; }
        public ICommand ShowEmployeeViewCommand { get; }
        public ICommand ShowSupplierViewCommand { get; }
        public ICommand ShowMedicalRecordViewCommand { get; }
        public ICommand ShowProcedureRecordViewCommand { get; }
        public ICommand ShowLabTestViewCommand { get; }
        public ICommand ShowLabRecordViewCommand { get; }
        public ICommand ShowChannelRecordViewCommand { get; }
        public ICommand ShowMedicalCommissionViewCommand { get; }
        public ICommand ShowLabCommissionViewCommand { get; }
        public ICommand ShowProcedureCommissionViewCommand { get; }
        public ICommand ShowChanellingCommissionViewCommand { get; }
        public ICommand ShowMedicalReportViewCommand { get; }
        public ICommand ShowProcedureReportViewCommand { get; }
        public ICommand ShowLabReportViewCommand { get; }
        public ICommand ShowChanellingReportViewCommand { get; }
        public ICommand ShowActivityLogsViewCommand { get; }
        public ICommand LogoutCommand { get; }
        public ICommand ShowChangePasswordViewCommand { get; }

        public MainViewModel()
        {
            userRepository = new UserRepository();
            CurrentUserAccount = new UserAccount();

            //Initialize commands
            ShowProductViewCommand = new RelayCommand(ExecuteShowProductViewCommand);
            ShowProductRecordViewCommand = new RelayCommand(ExecuteShowProductRecordViewCommand);
            ShowStockViewCommand = new RelayCommand(ExecuteShowStockViewCommand);
            ShowPatientViewCommand = new RelayCommand(ExecuteShowPatientViewCommand);
            ShowEmployeeViewCommand = new RelayCommand(ExecuteShowEmployeeViewCommand);
            ShowSupplierViewCommand = new RelayCommand(ExecuteShowSupplierViewCommand);
            ShowMedicalRecordViewCommand = new RelayCommand(ExecuteShowMedicalRecordViewCommand);
            ShowProcedureRecordViewCommand = new RelayCommand(ExecuteShowProcedureRecordViewCommand);
            ShowLabTestViewCommand = new RelayCommand(ExecuteShowLabTestViewCommand);
            ShowLabRecordViewCommand = new RelayCommand(ExecuteShowLabRecordViewCommand);
            ShowChannelRecordViewCommand = new RelayCommand(ExecuteShowChannelRecordViewCommand);
            ShowMedicalCommissionViewCommand = new RelayCommand(ExecuteShowMedicalCommissionViewCommand);
            ShowLabCommissionViewCommand = new RelayCommand(ExecuteShowLabCommissionViewCommand);
            ShowProcedureCommissionViewCommand = new RelayCommand(ExecuteShowProcedureCommissionViewCommand);
            ShowChanellingCommissionViewCommand = new RelayCommand(ExecuteShowChanellingCommissionViewCommand);
            ShowMedicalReportViewCommand = new RelayCommand(ExecuteShowMedicalReportViewCommand);
            ShowProcedureReportViewCommand = new RelayCommand(ExecuteShowProcedureReportViewCommand);
            ShowLabReportViewCommand = new RelayCommand(ExecuteShowLabReportViewCommand);
            ShowChanellingReportViewCommand = new RelayCommand(ExecuteShowChanellingReportViewCommand);
            ShowActivityLogsViewCommand = new RelayCommand(ExecuteShowActivityLogsViewCommand);
            LogoutCommand = new RelayCommand(ExecuteLogoutCommand);
            ShowChangePasswordViewCommand = new RelayCommand(ExecuteShowChangePasswordViewCommand);

            //Default view
            ExecuteShowProductViewCommand(null);

            LoadCurrentUserData();
        }

        private void ExecuteShowMedicalRecordViewCommand(object obj)
        {
            CurrentChildView = new MedicalRecordViewModel();
            Caption = "Medical Records";
            Icon = IconChar.FileMedical;
            Application.Current.Properties["PageName"] = Caption;
        }

        private void ExecuteShowProcedureRecordViewCommand(object obj)
        {
            CurrentChildView = new ProcedureRecordViewModel();
            Caption = "Procedure Records";
            Icon = IconChar.FileMedical;
            Application.Current.Properties["PageName"] = Caption;
        }
        private void ExecuteShowStockViewCommand(object obj)
        {
            CurrentChildView = new StockViewModel();
            Caption = "Stocks";
            Icon = IconChar.ArrowTrendUp;
            Application.Current.Properties["PageName"] = Caption;
        }

        private void ExecuteShowSupplierViewCommand(object obj)
        {
            CurrentChildView = new SupplierViewModel();
            Caption = "Suppliers";
            Icon = IconChar.TruckMedical;
            Application.Current.Properties["PageName"] = Caption;
        }

        private void ExecuteLogoutCommand(object obj)
        {
            IsMainViewVisible = false;

            var mainWindow = new MainWindow();
            if (mainWindow.IsVisible == false)
            {
                var loginWindow = new LoginWindow();
                loginWindow.Show();
            }
            //new System.Net.NetworkCredential().UserName = null;
            //new System.Net.NetworkCredential().Password = null;
            //Thread.CurrentPrincipal = null;
        }

        private void ExecuteShowEmployeeViewCommand(object obj)
        {
            CurrentChildView = new EmployeeViewModel();
            Caption = "Employees";
            Icon = IconChar.HospitalUser;
            Application.Current.Properties["PageName"] = Caption;
        }

        private void ExecuteShowPatientViewCommand(object obj)
        {
            CurrentChildView = new PatientViewModel();
            Caption = "Patients";
            Icon = IconChar.Users;
            Application.Current.Properties["PageName"] = Caption;
        }

        private void ExecuteShowProductViewCommand(object obj)
        {
            CurrentChildView = new ProductViewModel();
            Caption = "Products";
            Icon = IconChar.Pills;
            Application.Current.Properties["PageName"] = Caption;
        }

        private void ExecuteShowProductRecordViewCommand(object obj)
        {
            CurrentChildView = new ProductRecordViewModel();
            Caption = "Product Records";
            Icon = IconChar.Capsules;
            Application.Current.Properties["PageName"] = Caption;
        }

        private void ExecuteShowLabTestViewCommand(object obj)
        {
            CurrentChildView = new LabTestViewModel();
            Caption = "Lab Tests";
            Icon = IconChar.Vials;
            Application.Current.Properties["PageName"] = Caption;
        }

        private void ExecuteShowLabRecordViewCommand(object obj)
        {
            CurrentChildView = new LabRecordViewModel();
            Caption = "Lab Records";
            Icon = IconChar.FileMedical;
            Application.Current.Properties["PageName"] = Caption;
        }

        private void ExecuteShowChannelRecordViewCommand(object obj)
        {
            CurrentChildView = new ChannelRecordViewModel();
            Caption = "Channel Records";
            Icon = IconChar.FileMedical;
            Application.Current.Properties["PageName"] = Caption;
        }

        private void ExecuteShowMedicalCommissionViewCommand(object obj)
        {
            CurrentChildView = new CommissionViewModel();
            Caption = "Medical Commissions";
            Icon = IconChar.SackDollar;
            Application.Current.Properties["PageName"] = Caption;
        }

        private void ExecuteShowLabCommissionViewCommand(object obj)
        {
            CurrentChildView = new CommissionViewModel();
            Caption = "Lab Commissions";
            Icon = IconChar.SackDollar;
            Application.Current.Properties["PageName"] = Caption;
        }

        private void ExecuteShowProcedureCommissionViewCommand(object obj)
        {
            CurrentChildView = new CommissionViewModel();
            Caption = "Procedure Commissions";
            Icon = IconChar.SackDollar;
            Application.Current.Properties["PageName"] = Caption;
        }

        private void ExecuteShowChanellingCommissionViewCommand(object obj)
        {
            CurrentChildView = new CommissionViewModel();
            Caption = "Chanelling Commissions";
            Icon = IconChar.SackDollar;
            Application.Current.Properties["PageName"] = Caption;
        }

        private void ExecuteShowMedicalReportViewCommand(object obj)
        {
            CurrentChildView = new MedicalSummaryViewModel();
            Caption = "Medical Reports";
            Icon = IconChar.ClipboardList;
            Application.Current.Properties["PageName"] = Caption;
        }

        private void ExecuteShowProcedureReportViewCommand(object obj)
        {
            CurrentChildView = new ProcedureSummaryViewModel();
            Caption = "Procedure Reports";
            Icon = IconChar.ClipboardList;
            Application.Current.Properties["PageName"] = Caption;
        }

        private void ExecuteShowLabReportViewCommand(object obj)
        {
            CurrentChildView = new LabSummaryViewModel();
            Caption = "Lab Reports";
            Icon = IconChar.ClipboardList;
            Application.Current.Properties["PageName"] = Caption;
        }

        private void ExecuteShowChanellingReportViewCommand(object obj)
        {
            CurrentChildView = new ChanellingSummaryViewModel();
            Caption = "Chanelling Reports";
            Icon = IconChar.ClipboardList;
            Application.Current.Properties["PageName"] = Caption;
        }

        private void ExecuteShowActivityLogsViewCommand(object obj)
        {
            CurrentChildView = new ActivityLogViewModel();
            Caption = "Activity Logs";
            Icon = IconChar.ListCheck;
            Application.Current.Properties["PageName"] = Caption;
        }

        private void ExecuteShowChangePasswordViewCommand(object obj)
        {
            CurrentChildView = new ChangePasswordViewModel();
            Caption = "Change Password";
            Icon = IconChar.RightLeft;
            Application.Current.Properties["PageName"] = Caption;
        }

        private void LoadCurrentUserData()
        {
            if (Thread.CurrentPrincipal != null)
            {
                var user = userRepository.GetByUsername(Thread.CurrentPrincipal.Identity.Name);
                Thread.CurrentPrincipal = new GenericPrincipal(new GenericIdentity(Thread.CurrentPrincipal.Identity.Name), null);
                if (user != null)
                {
                    {
                        CurrentUserAccount.Username = "Welcome " + user.Username;
                        Properties.Settings.Default.Status = user.Status;
                        Properties.Settings.Default.EmployeeType = user.EmployeeType;
                        Properties.Settings.Default.Username = user.Username;
                        Properties.Settings.Default.Save();
                    };
                }
                else
                {
                    CurrentUserAccount.Username = ("Invalid user, not logged in");
                    //Hide child views
                }
            }
        }
    }
}
