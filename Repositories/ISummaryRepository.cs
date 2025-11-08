using QWellApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QWellApp.Repositories
{
    public interface ISummaryRepository
    {
        Task<IEnumerable<MedicalSummary>> GetMedicalSummary(DateTime startDateTime, DateTime endDateTime);
        Task<IEnumerable<ProcedureSummary>> GetProcedureSummary(DateTime startDateTime, DateTime endDateTime);
        Task<IEnumerable<LabSummary>> GetLabSummary(DateTime startDateTime, DateTime endDateTime);
        Task<IEnumerable<ChannelSummary>> GetChannelSummary(DateTime startDateTime, DateTime endDateTime);
        Task<Report> GenerateReport<T>(IEnumerable<T> summary, DateTime startDateTime, DateTime endDateTime) where T : class;
        Report GenerateFullReport(Report medicalReport, Report procedureReport, Report labReport, Report channelReport);
    }
}
