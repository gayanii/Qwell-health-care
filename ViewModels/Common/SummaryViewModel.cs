using QWellApp.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QWellApp.ViewModels.Common
{
    public interface SummaryViewModel
    {
        DateTime StartDate { get; set; }
        DateTime EndDate { get; set; }

        IEnumerable<MedicalSummary> MedicalSummaryList { get; }
        IEnumerable<ProcedureSummary> ProcedureSummaryList { get; }
        IEnumerable<LabSummary> LabSummaryList { get; }
        IEnumerable<ChannelSummary> ChannelSummaryList { get; }
        Report FullReportSummary { get; }

        //private IEnumerable<MedicalSummary> _medicalSummaryList;
        //private IEnumerable<ProcedureSummary> _procedureSummaryList;
        //private IEnumerable<LabSummary> _labSummaryList;
        //private IEnumerable<ChannelSummary> _channelSummaryList;
    }
}
