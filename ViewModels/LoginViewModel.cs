using QWell;
using QWellApp.Helpers;
using QWellApp.Repositories;
using QWellApp.Services;
using QWellApp.ViewModels.Common;
using QWellApp.Views.Pages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security;
using System.Security.Principal;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;

namespace QWellApp.ViewModels
{
    public class LoginViewModel: ViewModelBase
    {
        //Fields
        private string _username;
        private SecureString _password;
        private bool _isForgotMode;
        private string _errorMessage;
        private bool _isLoginViewVisible = false;
        private bool _loadingGridVisibility;

        private IUserRepository userRepository;
        protected readonly Validation validator;

        //Properties 
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

        public bool IsForgotMode
        {
            get => _isForgotMode;
            set
            {
                if (_isForgotMode != value)
                {
                    _isForgotMode = value;
                    OnPropertyChanged(nameof(IsForgotMode));

                    // Notify dependent properties
                    OnPropertyChanged(nameof(ForgotPasswordButtonLabel));
                    OnPropertyChanged(nameof(SignInLabel));
                }
            }
        }

        public string ErrorMessage
        {
            get
            {
                return _errorMessage;
            }

            set
            {
                _errorMessage = value;
                OnPropertyChanged(nameof(ErrorMessage));
            }
        }
        public bool IsLoginViewVisible
        {
            get
            {
                return _isLoginViewVisible;
            }

            set
            {
                _isLoginViewVisible = value;
                OnPropertyChanged(nameof(IsLoginViewVisible));
            }
        }

        public bool LoadingGridVisibility
        {
            get
            {
                return _loadingGridVisibility;
            }

            set
            {
                _loadingGridVisibility = value;
                OnPropertyChanged(nameof(LoadingGridVisibility));
            }
        }

        //Commands
        public ICommand LoginCommand { get; }
        public ICommand RecoverPasswordCommand { get; }
        public ICommand ShowPasswordCommand { get; }
        public ICommand RememberPasswordCommand { get; }
        public ICommand ForgotPasswordCommand { get; }
        public ICommand SendEmailCommand { get; }
        public string ForgotPasswordButtonLabel => IsForgotMode ? "Cancel" : "Forgot Password?";
        public string SignInLabel => IsForgotMode ? "Forgot Password" : "Sign In";

        //Constructors
        public LoginViewModel()
        {
            userRepository = new UserRepository();
            validator = new Validation();
            LoginCommand = new RelayCommand(ExecuteLoginCommand, CanExecuteLoginCommand);
            RecoverPasswordCommand = new RelayCommand(p => ExecutedRecoverPassCommand("", ""));
            ForgotPasswordCommand = new RelayCommand(a => IsForgotMode = !IsForgotMode);
            SendEmailCommand = new RelayCommand(ExecuteSendEmailCommand, CanExecuteSendEmailCommand);
        }

        private bool CanExecuteLoginCommand(object obj)
        {
            /*bool validData;
            if (string.IsNullOrWhiteSpace(Username) || Username.Length < 3 || 
                Password == null || Password.Length <3)
            {
                validData = false;
            } else
            {
                validData = true;
            }
            return validData;*/
            return true;
        }

        private async void ExecuteLoginCommand(object obj)
        {
            ErrorMessage = "";
            LoadingGridVisibility = true;
            try
            {
                if (string.IsNullOrWhiteSpace(Username) || Username.Length < 3 || Password == null || Password.Length < 3)
                {
                    ErrorMessage = "Invalid username or password.";
                    return;
                }

                var isValidUser = await Task.Run(() => userRepository.AuthenticateUser(new NetworkCredential(Username, Password)));
                if (isValidUser)
                {
                    Thread.CurrentPrincipal = new GenericPrincipal(new GenericIdentity(Username), null);
                    Application.Current.Properties["Username"] = Username;
                    IsLoginViewVisible = false;

                    var mainWindow = new MainWindow();
                    mainWindow.Show();
                }
                else
                {
                    MessageBox.Show("Invalid username or password.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred: {ex.Message}");
            }
            finally
            {
                LoadingGridVisibility = false;
            }
        }

        private async void ExecuteSendEmailCommand(object obj)
        {
            if (string.IsNullOrWhiteSpace(Username))
            {
                ErrorMessage = "Please enter your email.";
                return;
            }

            var user = userRepository.GetByUsername(Username);

            if (user == null)
            {
                MessageBox.Show("This username is not registered.");
                return;
            }

            if (string.IsNullOrEmpty(user.Email))
            {
                MessageBox.Show("No email is registered under this username.");
                return;
            }

            // Generate random password
            string newPassword = validator.GenerateRandomPassword();

            // Save new password
            bool updated = userRepository.ChangePassword(new NetworkCredential(Username, newPassword));

            if (!updated)
            {
                MessageBox.Show("Failed to reset password.");
                return;
            }

            // Send email
            bool emailSent = await EmailService.SendPasswordResetEmail(user.Email, newPassword);

            if (emailSent)
                MessageBox.Show("A reset email has been sent.");
            else
                MessageBox.Show("Failed to send email.");
        }

        private bool CanExecuteSendEmailCommand(object obj)
        {
            return IsForgotMode;   // only enabled in forgot mode
        }

        private void ExecutedRecoverPassCommand(string username, string email)
        {
            throw new NotImplementedException();
        }
    }
}
