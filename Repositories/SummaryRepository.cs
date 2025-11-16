using iText.Kernel.Pdf.Canvas.Parser.ClipperLib;
using iText.StyledXmlParser.Node;
using Microsoft.EntityFrameworkCore;
using QWellApp.DBConnection;
using QWellApp.Enums;
using QWellApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace QWellApp.Repositories
{
    public class SummaryRepository : BaseRepository, ISummaryRepository
    {
        public IPatientRepository patientRepository;
        public IUserRepository userRepository;
        public IProductRepository productRepository;
        public IProductMedicalRecordRepository productMedicalRepository;
        public ICommissionRepository commissionRepository;

        public SummaryRepository()
        {
            patientRepository = new PatientRepository();
            userRepository = new UserRepository();
            productRepository = new ProductRepository();
            productMedicalRepository = new ProductMedicalRecordRepository();
            commissionRepository = new CommissionRepository();
        }

        // common template to display individual summary records
        public async Task<IEnumerable<TSummary>> GetSummaryAsync<TRecord, TSummary>(
            DateTime SumStartDate, DateTime SumEndDate, 
            Func<TRecord, TSummary> createSummary,
            IQueryable<TRecord> recordQuery)
            where TRecord : class, IAdmitDateRecord  // Add constraint
            where TSummary : class
        {
            try
            {
                using (AppDataContext context = new AppDataContext())
                {
                    // Filter records based on admit date
                    var records = await recordQuery
                        .Where(x => x.AdmitDate >= SumStartDate && x.AdmitDate < SumEndDate)
                        .OrderByDescending(p => p.AdmitDate) // Sort by AdmitDate in descending order. recent up
                        .ToListAsync();

                    // Generate summaries
                    int count = 0;
                    var summaryList = records.Select(record =>
                    {
                        count++;
                        return createSummary(record);
                    }).ToList();

                    return summaryList;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}");
                throw;
            }
        }

        // Wrapper methods for each summary type
        // Get medical records list to show in the UI and in the downloaded pdf 1st table
        public async Task<IEnumerable<MedicalSummary>> GetMedicalSummary(DateTime sumStartDate, DateTime sumEndDate)
        {
            try
            {
                using (AppDataContext context = new AppDataContext())
                {
                    return await GetSummaryAsync<MedicalRecord, MedicalSummary>(
                sumStartDate, sumEndDate,
                record =>
                {
                    float totCom = CalculateTotalCommissions(record.DoctorId, record.DocComm, record.Nurse1Id, record.Nurse1Comm, record.Nurse2Id, record.Nurse2Comm);
                    return new MedicalSummary
                    {
                        Id = record.Id,
                        ChitNumber = record.ChitNumber,
                        AdmitDate = record.AdmitDate.ToString("dd-MMM-yyyy HH:mm"),
                        OPDCharge = record.OPDCharge ?? 0,
                        PharmacyBill = record.PharmacyBill,
                        ConsultantFee = record.ConsultantFee ?? 0,
                        OtherCharges = record.OtherCharges ?? 0,
                        TotalCommisions = totCom,
                        TotalBill = (record.PharmacyBill) + (record.OPDCharge ?? 0) + (record.ConsultantFee ?? 0) + (record.OtherCharges ?? 0)
                    };
                },
                context.MedicalRecords.AsQueryable());
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}");
                throw;
            }
        }

        // Get procedure records list to show in the UI and in the downloaded pdf 1st table
        public async Task<IEnumerable<ProcedureSummary>> GetProcedureSummary(DateTime sumStartDate, DateTime sumEndDate)
        {
            try
            {
                using (AppDataContext context = new AppDataContext())
                {
                    return await GetSummaryAsync<ProcedureRecord, ProcedureSummary>(
                sumStartDate, sumEndDate,
                record =>
                {
                    float totCom = CalculateTotalCommissions(record.DoctorId, record.DocComm, record.Nurse1Id, record.Nurse1Comm, record.Nurse2Id, record.Nurse2Comm);
                    return new ProcedureSummary
                    {
                        Id = record.Id,
                        ChitNumber = record.ChitNumber,
                        AdmitDate = record.AdmitDate.ToString("dd-MMM-yyyy HH:mm"),
                        OPDCharge = record.OPDCharge ?? 0,
                        ProcedureBill = record.ProcedureBill,
                        ConsultantFee = record.ConsultantFee ?? 0,
                        OtherCharges = record.OtherCharges ?? 0,
                        TotalCommisions = totCom,
                        TotalBill = (record.ProcedureBill) + (record.OPDCharge ?? 0) + (record.ConsultantFee ?? 0) + (record.OtherCharges ?? 0)
                    };
                },
                context.ProcedureRecords.AsQueryable());
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}");
                throw;
            }
        }

        // Get lab records list to show in the UI and in the downloaded pdf 1st table
        public async Task<IEnumerable<LabSummary>> GetLabSummary(DateTime sumStartDate, DateTime sumEndDate)
        {
            try
            {
                using (AppDataContext context = new AppDataContext())
                {
                    return await GetSummaryAsync<LabRecord, LabSummary>(
                sumStartDate, sumEndDate,
                record =>
                {
                    int? ab = record.DoctorId;
                    float totCom = CalculateTotalCommissions(record.DoctorId, record.DocComm, record.Nurse1Id, record.Nurse1Comm, record.Nurse2Id, record.Nurse2Comm);
                    return new LabSummary
                    {
                        Id = record.Id,
                        ChitNumber = record.ChitNumber,
                        AdmitDate = record.AdmitDate.ToString("dd-MMM-yyyy HH:mm"),
                        HospitalName = record.HospitalName,
                        LabBill = record.LabBill,
                        LabPaidCost = record.TotalLabPaidCost,
                        QwellCommission = record.QwellCommission,
                        ConsultantFee = record.ConsultantFee ?? 0,
                        ConsumableBill = record.OtherCharges ?? 0,
                        TotalCommisions = totCom,
                        TotalBill = (record.LabBill) + (record.ConsultantFee ?? 0) + (record.OtherCharges ?? 0)
                    };
                },
                context.LabRecords.AsQueryable());
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}");
                throw;
            }
        }

        // Get channel records list to show in the UI and in the downloaded pdf 1st table
        public async Task<IEnumerable<ChannelSummary>> GetChannelSummary(DateTime sumStartDate, DateTime sumEndDate)
        {
            try
            {
                using (AppDataContext context = new AppDataContext())
                {
                    return await GetSummaryAsync<ChannelRecord, ChannelSummary>(
                sumStartDate, sumEndDate,
                record =>
                {
                    float totCom = CalculateTotalCommissions(record.DoctorId, record.DocComm, record.Nurse1Id, record.Nurse1Comm, record.Nurse2Id, record.Nurse2Comm);
                    return new ChannelSummary
                    {
                        Id = record.Id,
                        ChitNumber = record.ChitNumber,
                        AdmitDate = record.AdmitDate.ToString("dd-MMM-yyyy HH:mm"),
                        OPDCharge = record.OPDCharge ?? 0,
                        PharmacyBill = record.PharmacyBill,
                        ConsultantFee = record.ConsultantFee ?? 0,
                        OtherCharges = record.OtherCharges ?? 0,
                        TotalCommisions = totCom,
                        TotalBill = (record.PharmacyBill) + (record.OPDCharge ?? 0) + (record.ConsultantFee ?? 0) + (record.OtherCharges ?? 0)
                    };
                },
                context.ChannelRecords.AsQueryable());
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}");
                throw;
            }
        }

        // Helper method to calculate total commissions
        private float CalculateTotalCommissions(int? doctorId, float? docComm, int? nurse1Id, float? nurse1Comm, int? nurse2Id, float? nurse2Comm)
        {
            float totCom = 0;
            if (doctorId != null) totCom += docComm ?? 0;
            if (nurse1Id != null) totCom += nurse1Comm ?? 0;
            if (nurse2Id != null) totCom += nurse2Comm ?? 0;
            return totCom;
        }

        // Get individual record summary to show in the downloaded pdf 2nd table
        public async Task<Report> GenerateReport<T>(IEnumerable<T> summary, DateTime startDate, DateTime endDate) where T : class
        {
            try
            {
                using (AppDataContext context = new AppDataContext())
                {
                    float? income = 0;
                    float? lapPaid = 0;
                    float? qwellCommission = 0;
                    float? commissionTot = 0;

                    IEnumerable<Commission> commissions = [];
                    // Check if medical summary report
                    if (typeof(T) == typeof(MedicalSummary)) {
                        commissions = await commissionRepository.GetMedicalCommissions(startDate, endDate);
                    }
                    // Check if procedure summary report
                    if (typeof(T) == typeof(ProcedureSummary))
                    {
                        commissions = await commissionRepository.GetProcedureCommissions(startDate, endDate);
                    }
                    // Check if lab summary report
                    if (typeof(T) == typeof(LabSummary))
                    {
                        commissions = await commissionRepository.GetLabCommissions(startDate, endDate);
                    }
                    // Check if channel summary report
                    if (typeof(T) == typeof(ChannelSummary))
                    {
                        commissions = await commissionRepository.GetChannelCommissions(startDate, endDate);
                    }

                    foreach (var commission in commissions)
                    {
                        commissionTot += commission.TotalCommisssion;
                    }
                    foreach (var summaryRecord in summary)
                    {
                        // Use reflection to check if the property 'LabPaid' exists
                        var totalBillProperty = summaryRecord.GetType().GetProperty("TotalBill");
                        var labPaidProperty = summaryRecord.GetType().GetProperty("LabPaidCost");
                        var qwellCommissionProperty = summaryRecord.GetType().GetProperty("QwellCommission");

                        income += totalBillProperty != null ? totalBillProperty.GetValue(summaryRecord) as float? : 0;
                        lapPaid += labPaidProperty != null ? labPaidProperty.GetValue(summaryRecord) as float? : 0;
                        qwellCommission += qwellCommissionProperty != null ? qwellCommissionProperty.GetValue(summaryRecord) as float? : 0;
                    }
                    float? balance = (income ?? 0) - (lapPaid ?? 0) - (commissionTot ?? 0);
                    Report report = new Report()
                    {
                        TotalIncome = income,
                        TotalLabPaid = lapPaid,
                        QwellCommission = qwellCommission,
                        TotalCommissions = commissionTot,
                        Balance = (float)Math.Round(balance ?? 0, 2),
                    };

                    return report;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}");
                throw new NotImplementedException();
            }
        }

        // Get medical,procedure,lab,channel summary to show in the downloaded pdf 2nd table (in the 'download full summary' pdf)
        public Report GenerateFullReport(Report medicalReport, Report procedureReport, Report labReport, Report channelReport)
        {
            Report report = new Report()
            {
                TotalIncome = medicalReport.TotalIncome + procedureReport.TotalIncome + labReport.TotalIncome + channelReport.TotalIncome,
                TotalLabPaid = medicalReport.TotalLabPaid + procedureReport.TotalLabPaid + labReport.TotalLabPaid + channelReport.TotalLabPaid,
                QwellCommission = medicalReport.QwellCommission + procedureReport.QwellCommission + labReport.QwellCommission + channelReport.QwellCommission,
                TotalCommissions = medicalReport.TotalCommissions + procedureReport.TotalCommissions + labReport.TotalCommissions + channelReport.TotalCommissions
            };

            report.Balance = (float)Math.Round((report.TotalIncome - report.TotalLabPaid - report.TotalCommissions) ?? 0, 2);

            return report;
        }
    }
}
