using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using QWellApp.DBConnection;
using QWellApp.Enums;
using QWellApp.Helpers;
using QWellApp.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace QWellApp.Repositories
{
    public class ProcedureRecordRepository : BaseRepository, IProcedureRecordRepository
    {
        public IUserRepository userRepository;
        public IProductRepository productRepository;
        public IProductMedicalRecordRepository productMedicalRepository;
        public IActivityLogRepository activityLogRepository;
        public Validation validator;

        public ProcedureRecordRepository()
        {
            userRepository = new UserRepository();
            productRepository = new ProductRepository();
            productMedicalRepository = new ProductMedicalRecordRepository();
            activityLogRepository = new ActivityLogRepository();
            validator = new Validation();
        }

        public bool Add(ProcedureRecord procedureRecordModel, Dictionary<int, int> procedureData)
        {
            try
            {
                using (AppDataContext context = new AppDataContext())
                {
                    // Check if a record with the same ChitNumber exists for the given day
                    if (!validator.IsChitNumberUnique(context, procedureRecordModel.ChitNumber, procedureRecordModel.AdmitDate.Date))
                    {
                        MessageBox.Show("A record with the same chit number already exists for the specified day. Try a different chit number.");
                        return false;
                    }

                    var newProcedureRecord = new ProcedureRecord
                    {
                        ChitNumber = procedureRecordModel.ChitNumber,
                        OPDCharge = procedureRecordModel.OPDCharge,
                        OtherCharges = procedureRecordModel.OtherCharges,
                        ProcedureBill = procedureRecordModel.ProcedureBill,
                        AdmitDate = procedureRecordModel.AdmitDate,
                        TotalBill = procedureRecordModel.TotalBill,
                        ConsultantFee = procedureRecordModel.ConsultantFee,
                        PatientId = procedureRecordModel.PatientId,
                        DoctorId = procedureRecordModel.DoctorId,
                        AddedBy = procedureRecordModel.AddedBy,
                        Doctor = procedureRecordModel.Doctor,
                        Patient = procedureRecordModel.Patient,
                        DocComm = procedureRecordModel.DocComm,
                        Nurse1Comm = procedureRecordModel.Nurse1Comm,
                        Nurse2Comm = procedureRecordModel.Nurse2Comm,
                        Nurse1Id = procedureRecordModel.Nurse1Id,
                        Nurse2Id = procedureRecordModel.Nurse2Id,
                        Nurse1 = procedureRecordModel.Nurse1,
                        Nurse2 = procedureRecordModel.Nurse2
                    };

                    context.ProcedureRecords.Add(newProcedureRecord);
                    context.SaveChanges();

                    foreach (var entry in procedureData)
                    {
                        int productId = entry.Key;
                        int units = entry.Value;

                        var product = context.Products.Find(productId);
                        if (product == null)
                        {
                            MessageBox.Show($"Product with ID {productId} not found.");
                            return false;
                        }

                        var newProcedureDose = new ProductMedicalRecord
                        {
                            ProcedureRecordId = newProcedureRecord.Id,
                            AdmitDate = procedureRecordModel.AdmitDate,
                            RecordTypeId = (int)RecordTypeEnum.Procedure,
                            ProductId = productId,
                            Units = units,
                            SoldPrice = product.SellingPrice * units
                        };

                        productMedicalRepository.Add(newProcedureDose);
                    }
                    context.SaveChanges();
                    MessageBox.Show("Procedure Record created Successfully!");

                    var newMedicine = productMedicalRepository.GetAll(newProcedureRecord.Id, RecordTypeEnum.Procedure);
                    var currentUser = userRepository.GetByUsername(Properties.Settings.Default.Username);
                    // Transform newMedicine list to remove unwanted properties
                    var filteredNewMedicineData = newMedicine.Select(m => new
                    {
                        m.Id,
                        m.ProductId,
                        m.Units,
                        m.SoldPrice
                    }).ToList();

                    // Log the activity
                    var log = new ActivityLog
                    {
                        AffectedEntity = EntitiesEnum.ProcedureRecords,
                        AffectedEntityId = procedureRecordModel.Id,
                        ActionType = ActionTypeEnum.Add,
                        OldValues = "-",
                        NewValues = JsonConvert.SerializeObject(procedureRecordModel) + "\n\nMedicine:\n" + JsonConvert.SerializeObject(filteredNewMedicineData),
                    };
                    activityLogRepository.AddLog(log, currentUser);
                    return true;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}");
                return false;
            }
        }

        public bool Edit(ProcedureRecord procedureRecordModel, Dictionary<int, int> procedureData)
        {
            try
            {
                using (AppDataContext context = new AppDataContext())
                {
                    var procedureRecord = context.ProcedureRecords
                        .FirstOrDefault(x => x.Id.Equals(procedureRecordModel.Id));

                    if (procedureRecord != null)
                    {
                        // Check if a record with the same ChitNumber exists for the given day
                        if (!validator.IsChitNumberUnique(context, procedureRecordModel.ChitNumber, procedureRecordModel.AdmitDate.Date)
                            && procedureRecordModel.ChitNumber != procedureRecord.ChitNumber)
                        {
                            MessageBox.Show("A record with the same chit number already exists for the specified day. Try a different chit number.");
                            return false;
                        }

                        procedureRecord.ChitNumber = procedureRecordModel.ChitNumber;
                        procedureRecord.OPDCharge = procedureRecordModel.OPDCharge;
                        procedureRecord.OtherCharges = procedureRecordModel.OtherCharges;
                        procedureRecord.ProcedureBill = procedureRecordModel.ProcedureBill;
                        procedureRecord.TotalBill = procedureRecordModel.TotalBill;
                        procedureRecord.ConsultantFee = procedureRecordModel.ConsultantFee;
                        procedureRecord.PatientId = procedureRecordModel.PatientId;
                        procedureRecord.DoctorId = procedureRecordModel.DoctorId;
                        procedureRecord.AddedBy = procedureRecordModel.AddedBy;
                        procedureRecord.DocComm = procedureRecordModel.DocComm;
                        procedureRecord.Nurse1Comm = procedureRecordModel.Nurse1Comm;
                        procedureRecord.Nurse2Comm = procedureRecordModel.Nurse2Comm;
                        procedureRecord.Nurse1Id = procedureRecordModel.Nurse1Id;
                        procedureRecord.Nurse2Id = procedureRecordModel.Nurse2Id;
                        procedureRecord.Nurse1 = procedureRecordModel.Nurse1;
                        procedureRecord.Nurse2 = procedureRecordModel.Nurse2;

                        context.Entry(procedureRecord).State = EntityState.Modified;
                        context.SaveChanges();

                        var procedureDoseList = context.ProductMedicalRecords
                            .Where(p => p.ProcedureRecordId.Equals(procedureRecordModel.Id) && p.RecordTypeId.Equals((int)RecordTypeEnum.Procedure)).ToList();

                        foreach (var dose in procedureDoseList)
                        {
                            context.ProductMedicalRecords.Remove(dose);

                            //Add product current quantities
                            productRepository.EditCurrentQuantityOnly(dose.ProductId, dose.Units);
                        }
                        context.SaveChanges();

                        foreach (var entry in procedureData)
                        {
                            int productId = entry.Key;
                            int units = entry.Value;

                            var product = context.Products.Find(productId);
                            if (product == null)
                            {
                                MessageBox.Show($"Product with ID {productId} not found.");
                                return false;
                            }

                            var newProcedureDose = new ProductMedicalRecord
                            {
                                ProcedureRecordId = procedureRecordModel.Id,
                                AdmitDate = procedureRecordModel.AdmitDate,
                                RecordTypeId = (int)RecordTypeEnum.Procedure,
                                ProductId = productId,
                                Units = units,
                                SoldPrice = product.SellingPrice * units
                            };

                            productMedicalRepository.Add(newProcedureDose);
                        }
                        context.SaveChanges();
                        MessageBox.Show("Updated Successfully!");
                        return true;
                    }
                    MessageBox.Show("Failed to update!");
                    return false;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}");
                return false;
            }
        }

        public IEnumerable<ProcedureRecordView> GetAll(string searchWord)
        {
            List<ProcedureRecordView> procedureRecords = new List<ProcedureRecordView>();
            try
            {
                using (AppDataContext context = new AppDataContext())
                {
                    var procedureRecordsList = context.ProcedureRecords
                        .Include(pr => pr.Doctor)
                        .Where(p => p.Patient.FirstName.Contains(searchWord) || p.Patient.LastName.Contains(searchWord) ||
                        p.Doctor.FirstName.Contains(searchWord) || p.Doctor.LastName.Contains(searchWord) || p.ChitNumber.Contains(searchWord) ||
                        p.AdmitDate.Date.ToString().Contains(searchWord))
                        .OrderByDescending(p => p.AdmitDate) // Sort by AdmitDate in descending order. recent up
                        .Select(pr => new ProcedureRecordView
                        {
                            Id = pr.Id,
                            ChitNumber = pr.ChitNumber,
                            TotalBill = pr.TotalBill,
                            PatientName = pr.Patient.FirstName + " " + pr.Patient.LastName,
                            DoctorName = pr.Doctor.FirstName + " " + pr.Doctor.LastName,
                            AdmitDate = pr.AdmitDate.ToString("dd-MMM-yyyy HH:mm"),
                            AddedBy = $"{userRepository.GetByID(pr.AddedBy).FirstName} {userRepository.GetByID(pr.AddedBy).LastName}",
                        })
                        .ToList();

                    procedureRecords.AddRange(procedureRecordsList);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}");
            }
            return procedureRecords;
        }

        public ProcedureRecord GetByID(int id)
        {
            ProcedureRecord procedureRecord = null;
            using (var connection = GetConnection())
            using (var command = new SqlCommand())
            {
                try
                {
                    connection.Open();
                    command.Connection = connection;
                    command.CommandText = "SELECT * FROM [ProcedureRecords] WHERE id=@id";
                    command.Parameters.Add("@id", SqlDbType.Int).Value = id;

                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            procedureRecord = new ProcedureRecord()
                            {
                                Id = reader.GetInt32(reader.GetOrdinal("Id")),
                                ChitNumber = reader.IsDBNull(reader.GetOrdinal("ChitNumber")) ? null : reader.GetString(reader.GetOrdinal("ChitNumber")),
                                AdmitDate = reader.IsDBNull(reader.GetOrdinal("AdmitDate")) ? DateTime.Now : reader.GetDateTime(reader.GetOrdinal("AdmitDate")),
                                OtherCharges = reader.IsDBNull(reader.GetOrdinal("OtherCharges")) ? 0 : reader.GetFloat(reader.GetOrdinal("OtherCharges")),
                                OPDCharge = reader.IsDBNull(reader.GetOrdinal("OPDCharge")) ? 0 : reader.GetFloat(reader.GetOrdinal("OPDCharge")),
                                ProcedureBill = reader.IsDBNull(reader.GetOrdinal("ProcedureBill")) ? 0 : reader.GetFloat(reader.GetOrdinal("ProcedureBill")),
                                TotalBill = reader.IsDBNull(reader.GetOrdinal("TotalBill")) ? 0 : reader.GetFloat(reader.GetOrdinal("TotalBill")),
                                ConsultantFee = reader.IsDBNull(reader.GetOrdinal("ConsultantFee")) ? 0 : reader.GetFloat(reader.GetOrdinal("ConsultantFee")),
                                PatientId = reader.IsDBNull(reader.GetOrdinal("PatientId")) ? 0 : reader.GetInt32(reader.GetOrdinal("PatientId")),
                                DoctorId = reader.IsDBNull(reader.GetOrdinal("DoctorId")) ? 0 : reader.GetInt32(reader.GetOrdinal("DoctorId")),
                                AddedBy = reader.IsDBNull(reader.GetOrdinal("AddedBy")) ? 0 : reader.GetInt32(reader.GetOrdinal("AddedBy")),
                                DocComm = reader.IsDBNull(reader.GetOrdinal("DocComm")) ? 0 : reader.GetFloat(reader.GetOrdinal("DocComm")),
                                Nurse1Comm = reader.IsDBNull(reader.GetOrdinal("Nurse1Comm")) ? 0 : reader.GetFloat(reader.GetOrdinal("Nurse1Comm")),
                                Nurse2Comm = reader.IsDBNull(reader.GetOrdinal("Nurse2Comm")) ? 0 : reader.GetFloat(reader.GetOrdinal("Nurse2Comm")),
                                Nurse1Id = reader.IsDBNull(reader.GetOrdinal("Nurse1Id")) ? null : reader.GetInt32(reader.GetOrdinal("Nurse1Id")),
                                Nurse2Id = reader.IsDBNull(reader.GetOrdinal("Nurse2Id")) ? null : reader.GetInt32(reader.GetOrdinal("Nurse2Id"))
                            };
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error: {ex.Message}");
                }
                finally
                {
                    connection.Close();
                }
            }
            return procedureRecord;
        }

        public bool Remove(int id)
        {
            try
            {
                using (AppDataContext context = new AppDataContext())
                {
                    productMedicalRepository.RemoveProcedureRecord(id);
                    var procedureRecord = context.ProcedureRecords.FirstOrDefault(x => x.Id.Equals(id));
                    if (procedureRecord != null)
                    {
                        context.ProcedureRecords.Remove(procedureRecord);
                        context.SaveChanges();
                        MessageBox.Show("Deleted Successfully!");
                        return true;
                    }
                    else
                    {
                        MessageBox.Show("Failed to Delete!");
                        return false;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}");
                return false;
            }
        }
    }
}
