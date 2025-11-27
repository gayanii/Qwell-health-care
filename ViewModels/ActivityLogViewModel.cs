using Azure.Core.GeoJson;
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
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;
using static System.Runtime.InteropServices.JavaScript.JSType;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ListView;

namespace QWellApp.ViewModels
{
    public class ActivityLogViewModel : ViewModelBase
    {
        // Fields
        private IEnumerable<ActivityLogView> _activityLogList;
        private string _searchWord = "";
        private string _noResults;
        private bool _activityLogListVisibility = true;

        private readonly IActivityLogRepository activityLogRepository;

        // Properties
        public IEnumerable<ActivityLogView> ActivityLogList
        {
            get
            {
                return _activityLogList;
            }
            set
            {
                _activityLogList = value;
                OnPropertyChanged(nameof(ActivityLogList));
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
        
        public string NoResults
        {
            get => _noResults;
            set { _noResults = value; OnPropertyChanged(nameof(NoResults)); }
        }

        public bool ActivityLogListVisibility
        {
            get => _activityLogListVisibility;
            set { _activityLogListVisibility = value; OnPropertyChanged(nameof(ActivityLogListVisibility)); }
        }

        // Commands
        public ICommand LoadActivityLogResults { get; }

        // Constructor
        public ActivityLogViewModel()
        {
            activityLogRepository = new ActivityLogRepository();
            ActivityLogList = new List<ActivityLogView>();
            LoadActivityLogResults = new RelayCommand(ExecuteSearchCommand, CanExecuteForAdminsCommand);
            ButtonVisibility();
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

        private async void LoadActivityLogList(string SearchWord)
        {
            //Delete old logs
            activityLogRepository.CleanupOldActivityLogs();
            IEnumerable<ActivityLogView> activityLogs = await activityLogRepository.GetActivityLogs(SearchWord);
            ActivityLogList = activityLogs;

            if (ActivityLogList.Any())
            {
                NoResults = "Hidden";
            }
            else
            {
                NoResults = "Visible";
            }
        }
        private void ExecuteSearchCommand(object obj)
        {
            LoadActivityLogList(SearchWord);
        }

        private void ButtonVisibility()
        {
            var valideUser = Properties.Settings.Default.Username;
            var validEmployeeType = Properties.Settings.Default.EmployeeType;
            var validStatus = Properties.Settings.Default.Status;

            if (valideUser != null && (validEmployeeType.Equals(EmployeeTypeEnum.Admin.ToString()) || validEmployeeType.Equals(EmployeeTypeEnum.Manager.ToString())) && validStatus.Equals(UserStatusEnum.Active.ToString()))
            {
                ActivityLogListVisibility = true;
            }
            else
            {
                ActivityLogListVisibility = false;
            }
        }
    }
}
