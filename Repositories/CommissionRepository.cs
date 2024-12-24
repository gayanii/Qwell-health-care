using Microsoft.EntityFrameworkCore;
using QWellApp.DBConnection;
using QWellApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Xml.Linq;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace QWellApp.Repositories
{
    public class CommissionRepository : BaseRepository, ICommissionRepository
    {
        public async Task<IEnumerable<Commission>> GetMedicalCommissions(DateTime startDate, DateTime endDate)
        {
            try
            {
                using (AppDataContext context = new AppDataContext())
                {
                    // Calculate the start and end dates based on the summary date
                    DateTime ComStartDate = startDate.AddHours(7);  // 10/20/2024 7:00:00 AM
                    DateTime ComEndDate = endDate.AddDays(1).AddHours(6).AddMinutes(59); // 10/21/2024 6:59:00 AM

                    List<MedicalRecord> medicalRecords = context.MedicalRecords
                        .Include(x => x.Doctor)
                        .Include(x => x.Doctor.Role)
                        .Include(x => x.Nurse1)
                        .Include(x => x.Nurse1!.Role)
                        .Include(x => x.Nurse2)
                        .Include(x => x.Nurse2!.Role)
                        .Where(x => x.AdmitDate >= ComStartDate && x.AdmitDate < ComEndDate)
                        .OrderByDescending(p => p.AdmitDate) // Sort by AdmitDate in descending order. recent up
                        .ToList();
                    List<Commission> CommissionList = new List<Commission>();


                    // Process each medical record
                    foreach (var medicalRecord in medicalRecords)
                    {
                        AddOrUpdateCommission(medicalRecord.Doctor, medicalRecord.DocComm, medicalRecord.ChitNumber, medicalRecord.AdmitDate, CommissionList);
                        AddOrUpdateCommission(medicalRecord.Nurse1, medicalRecord.Nurse1Comm, medicalRecord.ChitNumber, medicalRecord.AdmitDate, CommissionList);
                        AddOrUpdateCommission(medicalRecord.Nurse2, medicalRecord.Nurse2Comm, medicalRecord.ChitNumber, medicalRecord.AdmitDate, CommissionList);
                    }

                    return CommissionList;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}");
                throw;
            }
        }

        public async Task<IEnumerable<Commission>> GetLabCommissions(DateTime startDate, DateTime endDate)
        {
            try
            {
                using (AppDataContext context = new AppDataContext())
                {
                    // Calculate the start and end dates based on the summary date
                    DateTime ComStartDate = startDate.AddHours(7);  // 10/20/2024 7:00:00 AM
                    DateTime ComEndDate = endDate.AddDays(1).AddHours(6).AddMinutes(59); // 10/21/2024 6:59:00 AM

                    List<LabRecord> labRecords = context.LabRecords
                        .Include(x => x.Doctor)
                        .Include(x => x.Doctor!.Role)
                        .Include(x => x.Nurse1)
                        .Include(x => x.Nurse1!.Role)
                        .Include(x => x.Nurse2)
                        .Include(x => x.Nurse2!.Role)
                        .Where(x => x.AdmitDate >= ComStartDate && x.AdmitDate < ComEndDate)
                        .OrderByDescending(p => p.AdmitDate) // Sort by AdmitDate in descending order. recent up
                        .ToList();
                    List<Commission> CommissionList = new List<Commission>();


                    // Process each lab record
                    foreach (var labRecord in labRecords)
                    {
                        AddOrUpdateCommission(labRecord.Doctor, labRecord.DocComm, labRecord.ChitNumber, labRecord.AdmitDate, CommissionList);
                        AddOrUpdateCommission(labRecord.Nurse1, labRecord.Nurse1Comm, labRecord.ChitNumber, labRecord.AdmitDate, CommissionList);
                        AddOrUpdateCommission(labRecord.Nurse2, labRecord.Nurse2Comm, labRecord.ChitNumber, labRecord.AdmitDate, CommissionList);
                    }

                    return CommissionList;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}");
                throw;
            }
        }

        public async Task<IEnumerable<Commission>> GetProcedureCommissions(DateTime startDate, DateTime endDate)
        {
            try
            {
                using (AppDataContext context = new AppDataContext())
                {
                    // Calculate the start and end dates based on the summary date
                    DateTime ComStartDate = startDate.AddHours(7);  // 10/20/2024 7:00:00 AM
                    DateTime ComEndDate = endDate.AddDays(1).AddHours(6).AddMinutes(59); // 10/21/2024 6:59:00 AM

                    List<ProcedureRecord> procedureRecords = context.ProcedureRecords
                        .Include(x => x.Doctor)
                        .Include(x => x.Doctor.Role)
                        .Include(x => x.Nurse1)
                        .Include(x => x.Nurse1!.Role)
                        .Include(x => x.Nurse2)
                        .Include(x => x.Nurse2!.Role)
                        .Where(x => x.AdmitDate >= ComStartDate && x.AdmitDate < ComEndDate)
                        .OrderByDescending(p => p.AdmitDate) // Sort by AdmitDate in descending order. recent up
                        .ToList();
                    List<Commission> CommissionList = new List<Commission>();


                    // Process each procedure record
                    foreach (var procedureRecord in procedureRecords)
                    {
                        AddOrUpdateCommission(procedureRecord.Doctor, procedureRecord.DocComm, procedureRecord.ChitNumber, procedureRecord.AdmitDate, CommissionList);
                        AddOrUpdateCommission(procedureRecord.Nurse1, procedureRecord.Nurse1Comm, procedureRecord.ChitNumber, procedureRecord.AdmitDate, CommissionList);
                        AddOrUpdateCommission(procedureRecord.Nurse2, procedureRecord.Nurse2Comm, procedureRecord.ChitNumber, procedureRecord.AdmitDate, CommissionList);
                    }

                    return CommissionList;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}");
                throw;
            }
        }

        // Helper method to add or update commission entries
        void AddOrUpdateCommission(User? user, float commissionAmount, string chitNumber, DateTime admitDate, List<Commission> CommissionList)
        {
            if (user == null) return;

            var existingCommission = CommissionList.FirstOrDefault(c => c.UserId == user.Id);
            string formattedDate = admitDate.ToString("dd-MMM-yyyy HH:mm");
            string formattedChit = $"{chitNumber}: {commissionAmount}";

            if (existingCommission != null)
            {
                existingCommission.Date.Add(formattedDate);
                existingCommission.ChitNumber.Add(formattedChit);
                existingCommission.TotalCommisssion += commissionAmount;
            }
            else
            {
                CommissionList.Add(new Commission
                {
                    UserId = user.Id,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    Role = user.Role.RoleName,
                    Date = new List<string> { formattedDate },
                    ChitNumber = new List<string> { formattedChit },
                    TotalCommisssion = commissionAmount
                });
            }
        }
    }
}
