using QWell;
using QWellApp.Repositories;
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

namespace QWellApp.ViewModels
{
    public class LoginViewModel: ViewModelBase
    {
        //Fields
        private string _username;
        private SecureString _password;
        private string _errorMessage;
        private bool _isLoginViewVisible = false;
        private bool _loadingGridVisibility;

        private IUserRepository userRepository;

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

        //Constructors
        public LoginViewModel()
        {
            userRepository = new UserRepository();
            LoginCommand = new RelayCommand(ExecuteLoginCommand, CanExecuteLoginCommand);
            RecoverPasswordCommand = new RelayCommand(p => ExecutedRecoverPassCommand("", ""));
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
                if (string.IsNullOrWhiteSpace(Username) || Username.Length < 3 ||
                    string.IsNullOrWhiteSpace(Password.ToString()) || Password.Length < 3)
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
                    ErrorMessage = "Invalid username or password.";
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

        private void ExecutedRecoverPassCommand(string username, string email)
        {
            throw new NotImplementedException();
        }
    }
}
