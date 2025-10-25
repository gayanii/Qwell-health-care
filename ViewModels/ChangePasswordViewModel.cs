using Azure.Core.GeoJson;
using QWellApp.Enums;
using QWellApp.Models;
using QWellApp.Repositories;
using QWellApp.ViewModels.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Security;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Navigation;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;
using static System.Runtime.InteropServices.JavaScript.JSType;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ListView;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;

namespace QWellApp.ViewModels
{
    public class ChangePasswordViewModel : ViewModelBase
    {
        // Fields
        private SecureString _oldPassword;
        private SecureString _newPassword;
        private SecureString _confirmPassword;
        private string _oldPasswordErrorMessage;
        private string _newPasswordErrorMessage;
        private string _confirmPasswordErrorMessage;

        private IUserRepository userRepository;
        private readonly MainViewModel mainViewModel;

        // Properties
        public SecureString OldPassword
        {
            get
            {
                return _oldPassword;
            }
            set
            {
                _oldPassword = value;
                OnPropertyChanged(nameof(OldPassword));
            }
        }
        public SecureString NewPassword
        {
            get
            {
                return _newPassword;
            }
            set
            {
                _newPassword = value;
                OnPropertyChanged(nameof(NewPassword));
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
        public string OldPasswordErrorMessage
        {
            get
            {
                return _oldPasswordErrorMessage;
            }

            set
            {
                _oldPasswordErrorMessage = value;
                OnPropertyChanged(nameof(OldPasswordErrorMessage));
            }
        }
        public string NewPasswordErrorMessage
        {
            get
            {
                return _newPasswordErrorMessage;
            }

            set
            {
                _newPasswordErrorMessage = value;
                OnPropertyChanged(nameof(NewPasswordErrorMessage));
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

        // Commands
        public ICommand ChangePasswordCommand { get; }
        public ICommand ResetPasswordCommand { get; }

        // Constructor
        public ChangePasswordViewModel()
        {
            userRepository = new UserRepository();
            mainViewModel = new MainViewModel();
            ChangePasswordCommand = new RelayCommand(ExecuteChangePasswordCommand, CanExecuteChangePasswordCommand);
            ResetPasswordCommand = new RelayCommand(ExecuteResetPasswordCommand);
        }
        private void ExecuteChangePasswordCommand(object obj)
        {
            if (string.IsNullOrWhiteSpace(new System.Net.NetworkCredential(string.Empty, OldPassword).Password) || string.IsNullOrWhiteSpace(new System.Net.NetworkCredential(string.Empty, NewPassword).Password) || 
                string.IsNullOrWhiteSpace(new System.Net.NetworkCredential(string.Empty, ConfirmPassword).Password) || NewPassword.Length < 3 ||
                new System.Net.NetworkCredential(string.Empty, NewPassword).Password != new System.Net.NetworkCredential(string.Empty, ConfirmPassword).Password)
            {
                OldPasswordErrorMessage = (string.IsNullOrWhiteSpace(new System.Net.NetworkCredential(string.Empty, OldPassword).Password)) ? "Old password is required." : "";
                NewPasswordErrorMessage = (string.IsNullOrWhiteSpace(new System.Net.NetworkCredential(string.Empty, NewPassword).Password)) ? "New password is required." : (NewPassword.Length < 3) ? "Password should have atleast 3 characters." : "";
                ConfirmPasswordErrorMessage = (string.IsNullOrWhiteSpace(new System.Net.NetworkCredential(string.Empty, ConfirmPassword).Password)) ? "Confirm password is required." : (new System.Net.NetworkCredential(string.Empty, NewPassword).Password != new System.Net.NetworkCredential(string.Empty, ConfirmPassword).Password) ? "New password and confirm password should be same." : "";
            }
            else
            {
                OldPasswordErrorMessage = "";
                NewPasswordErrorMessage = "";
                ConfirmPasswordErrorMessage = "";

                var username = Properties.Settings.Default.Username;
                var isValidUser = userRepository.AuthenticateUser(new NetworkCredential(username, OldPassword));
                if (isValidUser)
                {
                    var changeSuccess = userRepository.ChangePassword(new NetworkCredential(username, NewPassword));
                    if (changeSuccess)
                    {
                        mainViewModel.CurrentChildView = new ProductViewModel();
                    }
                }
                else
                {
                    MessageBox.Show("The old password is incorrect");
                }
            }
        }

        private void ExecuteResetPasswordCommand(object obj)
        {
            ClearSecureString(OldPassword);
            ClearSecureString(NewPassword);
            ClearSecureString(ConfirmPassword);
        }

        private string ConvertToUnsecureString(SecureString secureString)
        {
            if (secureString == null) return string.Empty;
            IntPtr ptr = Marshal.SecureStringToBSTR(secureString);
            try
            {
                return Marshal.PtrToStringBSTR(ptr);
            }
            finally
            {
                Marshal.ZeroFreeBSTR(ptr);
            }
        }

        private void ClearSecureString(SecureString secureString)
        {
            if (secureString != null)
            {
                secureString.Clear();
                //secureString.Dispose();
                //secureString = null;
            }
        }

        private bool CanExecuteChangePasswordCommand(object obj)
        {
            var valideUser = Properties.Settings.Default.Username;
            var validEmployeeType = Properties.Settings.Default.EmployeeType;
            var validStatus = Properties.Settings.Default.Status;

            if (valideUser != null && validStatus.Equals(UserStatusEnum.Active.ToString()))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
