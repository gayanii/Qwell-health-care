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
    public class LabRecordRepository : BaseRepository, ILabRecordRepository
    {
        public IUserRepository userRepository;
        public IProductRepository productRepository;
        public ILabRecordTestRepository labRecordTestRepository;
        public IProductMedicalRecordRepository productMedicalRepository;
        public IActivityLogRepository activityLogRepository;
        public Validation validator;

        public LabRecordRepository()
        {
            userRepository = new UserRepository();
            productRepository = new ProductRepository();
            labRecordTestRepository = new LabRecordTestRepository();
            productMedicalRepository = new ProductMedicalRecordRepository();
            activityLogRepository = new ActivityLogRepository();
            validator = new Validation();
        }

        public bool Add(LabRecord labRecordModel, List<int> labRecordTestData, Dictionary<int, int> labData)
        {
            try
            {
                using (AppDataContext context = new AppDataContext())
                {
                    // Check if a record with the same ChitNumber exists for the given day
                    if (!validator.IsChitNumberUnique(context, labRecordModel.ChitNumber, labRecordModel.AdmitDate.Date))
                    {
                        MessageBox.Show("A record with the same chit number already exists for the specified day. Try a different chit number.");
                        return false;
                    }

                    var newLabRecord = new LabRecord
                    {
                        ChitNumber = labRecordModel.ChitNumber,
                        AdmitDate = labRecordModel.AdmitDate,
                        HospitalName = labRecordModel.HospitalName,
                        TotalBill = labRecordModel.TotalBill,
                        PatientId = labRecordModel.PatientId,
                        AddedBy = labRecordModel.AddedBy,
                        TotalLabPaidCost = labRecordModel.TotalLabPaidCost,
                        QwellCommission = labRecordModel.QwellCommission,
                        Doctor = labRecordModel.Doctor,
                        DoctorId = labRecordModel.DoctorId,
                        ConsultantFee = labRecordModel.ConsultantFee,
                        OtherCharges = labRecordModel.OtherCharges,
                        LabBill = labRecordModel.LabBill,
                        DocComm = labRecordModel.DocComm,
                        Nurse1Comm = labRecordModel.Nurse1Comm,
                        Nurse2Comm = labRecordModel.Nurse2Comm,
                        Nurse1Id = labRecordModel.Nurse1Id,
                        Nurse2Id = labRecordModel.Nurse2Id,
                        Nurse1 = labRecordModel.Nurse1,
                        Nurse2 = labRecordModel.Nurse2
                    };

                    context.LabRecords.Add(newLabRecord);
                    context.SaveChanges();

                    for (int i = 0; i < labRecordTestData.Count(); i++)
                    {
                        int labTestId = labRecordTestData[i];

                        // Fetch the lab test data
                        var labTest = context.LabTests.Find(labTestId);
                        if (labTest == null)
                        {
                            MessageBox.Show($"Lab test with ID {labTestId} not found.");
                            return false;
                        }

                        var newLabRecordTest = new LabRecordTest
                        {
                            LabRecordId = newLabRecord.Id,
                            LabTestId = labTestId
                        };

                        labRecordTestRepository.Add(newLabRecordTest);
                    }
                    foreach (var entry in labData)
                    {
                        int productId = entry.Key;
                        int units = entry.Value;

                        var product = context.Products.Find(productId);
                        if (product == null)
                        {
                            MessageBox.Show($"Product with ID {productId} not found.");
                            return false;
                        }

                        var newLabDose = new ProductMedicalRecord
                        {
                            LabRecordId = newLabRecord.Id,
                            AdmitDate = newLabRecord.AdmitDate,
                            RecordTypeId = (int)RecordTypeEnum.Lab,
                            ProductId = productId,
                            Units = units,
                            SoldPrice = product.SellingPrice * units
                        };

                        productMedicalRepository.Add(newLabDose);
                    }
                    context.SaveChanges(); // Save changes after adding all doses
                    MessageBox.Show("Lab Record created Successfully!");

                    var newMedicine = productMedicalRepository.GetAll(newLabRecord.Id, RecordTypeEnum.Lab);
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
                        AffectedEntity = EntitiesEnum.LabRecords,
                        AffectedEntityId = labRecordModel.Id,
                        ActionType = ActionTypeEnum.Add,
                        OldValues = "-",
                        NewValues = JsonConvert.SerializeObject(labRecordModel) + "\n\nMedicine:\n" + JsonConvert.SerializeObject(filteredNewMedicineData),
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

        public bool Edit(LabRecord labRecordModel, List<int> labRecordTestData, Dictionary<int, int> labData)
        {
            try
            {
                using (AppDataContext context = new AppDataContext())
                {
                    // Fetch the existing lab record
                    var labRecord = context.LabRecords
                        .FirstOrDefault(x => x.Id.Equals(labRecordModel.Id));

                    if (labRecord != null)
                    {
                        // Check if a record with the same ChitNumber exists for the given day
                        if (!validator.IsChitNumberUnique(context, labRecordModel.ChitNumber, labRecordModel.AdmitDate.Date)
                            && labRecordModel.ChitNumber != labRecord.ChitNumber)
                        {
                            MessageBox.Show("A record with the same chit number already exists for the specified day. Try a different chit number.");
                            return false;
                        }

                        // Update the necessary fields
                        labRecord.ChitNumber = labRecordModel.ChitNumber;
                        labRecord.AdmitDate = labRecordModel.AdmitDate;
                        labRecord.HospitalName = labRecordModel.HospitalName;
                        labRecord.TotalBill = labRecordModel.TotalBill;
                        labRecord.PatientId = labRecordModel.PatientId;
                        labRecord.AddedBy = labRecordModel.AddedBy;
                        labRecord.TotalLabPaidCost = labRecordModel.TotalLabPaidCost;
                        labRecord.QwellCommission = labRecordModel.QwellCommission;
                        labRecord.DoctorId = labRecordModel.DoctorId;
                        labRecord.ConsultantFee = labRecordModel.ConsultantFee;
                        labRecord.LabBill = labRecordModel.LabBill;
                        labRecord.OtherCharges = labRecordModel.OtherCharges;
                        labRecord.DocComm = labRecordModel.DocComm;
                        labRecord.Nurse1Comm = labRecordModel.Nurse1Comm;
                        labRecord.Nurse2Comm = labRecordModel.Nurse2Comm;
                        labRecord.Nurse1Id = labRecordModel.Nurse1Id;
                        labRecord.Nurse2Id = labRecordModel.Nurse2Id;
                        labRecord.Nurse1 = labRecordModel.Nurse1;
                        labRecord.Nurse2 = labRecordModel.Nurse2;

                        context.Entry(labRecord).State = EntityState.Modified;
                        context.SaveChanges();

                        // Remove existing records for the lab record
                        var labRecordTestListForLabRecord = context.LabRecordTests
                            .Where(p => p.LabRecordId.Equals(labRecordModel.Id)).ToList();

                        foreach (var labRecordTest in labRecordTestListForLabRecord)
                        {
                            context.LabRecordTests.Remove(labRecordTest);
                        }

                        var labDoseList = context.ProductMedicalRecords
                            .Where(p => p.LabRecordId.Equals(labRecordModel.Id) && p.RecordTypeId.Equals((int)RecordTypeEnum.Lab)).ToList();

                        foreach (var dose in labDoseList)
                        {
                            context.ProductMedicalRecords.Remove(dose);

                            //Add product current quantities
                            productRepository.EditCurrentQuantityOnly(dose.ProductId, dose.Units);
                        }
                        context.SaveChanges(); // Save changes after removing old records

                        // Add new records
                        for (int i = 0; i < labRecordTestData.Count(); i++)
                        {
                            int labTestId = labRecordTestData[i];

                            // Fetch the lab test data
                            var labTest = context.LabTests.Find(labTestId);
                            if (labTest == null)
                            {
                                MessageBox.Show($"Lab test with ID {labTestId} not found.");
                                return false;
                            }

                            var newLabRecordTest = new LabRecordTest
                            {
                                LabRecordId = labRecord.Id,
                                LabTestId = labTestId
                            };

                            labRecordTestRepository.Add(newLabRecordTest);
                        }

                        foreach (var entry in labData)
                        {
                            int productId = entry.Key;
                            int units = entry.Value;

                            var product = context.Products.Find(productId);
                            if (product == null)
                            {
                                MessageBox.Show($"Product with ID {productId} not found.");
                                return false;
                            }

                            var newLabDose = new ProductMedicalRecord
                            {
                                LabRecordId = labRecordModel.Id,
                                AdmitDate = labRecordModel.AdmitDate,
                                RecordTypeId = (int)RecordTypeEnum.Lab,
                                ProductId = productId,
                                Units = units,
                                SoldPrice = product.SellingPrice * units
                            };

                            productMedicalRepository.Add(newLabDose);
                        }
                        context.SaveChanges(); // Save changes after adding all records
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


        public IEnumerable<LabRecordView> GetAll(string searchWord)
        {
            List<LabRecordView> labRecords = new List<LabRecordView>();
            try
            {
                using (AppDataContext context = new AppDataContext())
                {
                    var labRecordsList = context.LabRecords
                        .Include(pr => pr.Doctor)
                        .Where(p => p.Patient.FirstName.Contains(searchWord) || p.Patient.LastName.Contains(searchWord) ||
                        p.ChitNumber.Contains(searchWord) || p.AdmitDate.Date.ToString().Contains(searchWord))
                        .OrderByDescending(p => p.AdmitDate) // Sort by AdmitDate in descending order. recent up
                        .Select(lr => new LabRecordView
                        {
                            Id = lr.Id,
                            ChitNumber = lr.ChitNumber,
                            HospitalName = lr.HospitalName,
                            TotalBill = lr.TotalBill,
                            PatientName = lr.Patient.FirstName + " " + lr.Patient.LastName,
                            AdmitDate = lr.AdmitDate.ToString("dd-MMM-yyyy HH:mm"),
                            AddedBy = $"{userRepository.GetByID(lr.AddedBy).FirstName} {userRepository.GetByID(lr.AddedBy).LastName}",
                        })
                        .ToList();

                    labRecords.AddRange(labRecordsList);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}");
            }
            return labRecords;
        }

        public LabRecord GetByID(int id)
        {
            LabRecord labRecord = null;
            using (var connection = GetConnection())
            using (var command = new SqlCommand())
            {
                try
                {
                    connection.Open();
                    command.Connection = connection;
                    command.CommandText = "SELECT * FROM [LabRecords] WHERE id=@id";
                    command.Parameters.Add("@id", SqlDbType.Int).Value = id;

                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            labRecord = new LabRecord()
                            {
                                Id = reader.GetInt32(reader.GetOrdinal("Id")),
                                ChitNumber = reader.IsDBNull(reader.GetOrdinal("ChitNumber")) ? null : reader.GetString(reader.GetOrdinal("ChitNumber")),
                                AdmitDate = reader.IsDBNull(reader.GetOrdinal("AdmitDate")) ? DateTime.Now : reader.GetDateTime(reader.GetOrdinal("AdmitDate")),
                                TotalBill = reader.IsDBNull(reader.GetOrdinal("TotalBill")) ? 0 : reader.GetFloat(reader.GetOrdinal("TotalBill")),
                                PatientId = reader.IsDBNull(reader.GetOrdinal("PatientId")) ? 0 : reader.GetInt32(reader.GetOrdinal("PatientId")),
                                ConsultantFee = reader.IsDBNull(reader.GetOrdinal("ConsultantFee")) ? 0 : reader.GetFloat(reader.GetOrdinal("ConsultantFee")),
                                OtherCharges = reader.IsDBNull(reader.GetOrdinal("OtherCharges")) ? 0 : reader.GetFloat(reader.GetOrdinal("OtherCharges")),
                                LabBill = reader.IsDBNull(reader.GetOrdinal("LabBill")) ? 0 : reader.GetFloat(reader.GetOrdinal("LabBill")),
                                DoctorId = reader.IsDBNull(reader.GetOrdinal("DoctorId")) ? null : reader.GetInt32(reader.GetOrdinal("DoctorId")),
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
            return labRecord;
        }


        public bool Remove(int id)
        {
            try
            {
                using (AppDataContext context = new AppDataContext())
                {
                    productMedicalRepository.RemoveLabRecord(id);
                    var labRecord = context.LabRecords.FirstOrDefault(x => x.Id.Equals(id));
                    if (labRecord != null)
                    {
                        context.LabRecords.Remove(labRecord);
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
