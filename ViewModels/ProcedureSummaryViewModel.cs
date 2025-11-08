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

namespace QWellApp.ViewModels
{
    public class ProcedureSummaryViewModel : BaseSummaryViewModel
    {

        // Commands
        public ICommand LoadReportResults { get; }

        // Constructor
        public ProcedureSummaryViewModel()
        {
            LoadReportResults = new RelayCommand(async _ => await LoadAllSummaries(), CanExecuteForAdminsCommand);
        }
    }
}
