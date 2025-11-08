using Azure.Core.GeoJson;
using QWellApp.Enums;
using QWellApp.Helpers;
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
    public class MedicalSummaryViewModel : BaseSummaryViewModel
    {
        // Commands
        public ICommand LoadReportResults { get; }


        // Constructor
        public MedicalSummaryViewModel()
        {
            LoadReportResults = new RelayCommand(async _ => await LoadAllSummaries(), CanExecuteForAdminsCommand);
        }

    }
}
