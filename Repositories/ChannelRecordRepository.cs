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
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace QWellApp.Repositories
{
    public class ChannelRecordRepository : BaseRepository, IChannelRecordRepository
    {
        public IUserRepository userRepository;
        public IProductRepository productRepository;
        public IProductMedicalRecordRepository productMedicalRepository;
        public IActivityLogRepository activityLogRepository;
        public Validation validator;

        public ChannelRecordRepository()
        {
            userRepository = new UserRepository();
            productRepository = new ProductRepository();
            productMedicalRepository = new ProductMedicalRecordRepository();
            activityLogRepository = new ActivityLogRepository();
            validator = new Validation();
        }

        public bool Add(ChannelRecord channelRecordModel, Dictionary<int, int> medicalDoseData)
        {
            try
            {
                using (AppDataContext context = new AppDataContext())
                {
                    if (!validator.IsChitNumberUnique(context, channelRecordModel.ChitNumber, channelRecordModel.AdmitDate.Date))
                    {
                        MessageBox.Show("A record with the same chit number already exists for the specified day. Try a different chit number.");
                        return false;
                    }

                    var newChannelRecord = new ChannelRecord
                    {
                        ChitNumber = channelRecordModel.ChitNumber,
                        OPDCharge = channelRecordModel.OPDCharge,
                        OtherCharges = channelRecordModel.OtherCharges,
                        PharmacyBill = channelRecordModel.PharmacyBill,
                        AdmitDate = channelRecordModel.AdmitDate,
                        TotalBill = channelRecordModel.TotalBill,
                        PatientId = channelRecordModel.PatientId,
                        DoctorId = channelRecordModel.DoctorId, // Include DoctorId if needed
                        AddedBy = channelRecordModel.AddedBy, // Assuming this is provided
                        Doctor = channelRecordModel.Doctor,
                        Patient = channelRecordModel.Patient,
                        DocComm = channelRecordModel.DocComm,
                        Nurse1Comm = channelRecordModel.Nurse1Comm,
                        Nurse2Comm = channelRecordModel.Nurse2Comm,
                        Nurse1Id = channelRecordModel.Nurse1Id,
                        Nurse2Id = channelRecordModel.Nurse2Id,
                        Nurse1 = channelRecordModel.Nurse1,
                        Nurse2 = channelRecordModel.Nurse2,
                        ConsultantFee = channelRecordModel.ConsultantFee,
                    };

                    context.ChannelRecords.Add(newChannelRecord);
                    context.SaveChanges();

                    for (int i = 0; i < medicalDoseData.Count(); i++)
                    {
                        int productId = medicalDoseData.Keys.ElementAt(i);
                        int units = medicalDoseData.Values.ElementAt(i);

                        // Fetch the product to get the selling price
                        var product = context.Products.Find(productId);
                        if (product == null)
                        {
                            MessageBox.Show($"Product with ID {productId} not found.");
                            return false;
                        }

                        var newChannelDose = new ProductMedicalRecord
                        {
                            ChannelRecordId = newChannelRecord.Id,
                            AdmitDate = newChannelRecord.AdmitDate,
                            ProductId = productId,
                            Units = units,
                            SoldPrice = product.SellingPrice * units, // Calculate SoldPrice
                            RecordTypeId = (int)RecordTypeEnum.Channel,
                        };

                        productMedicalRepository.Add(newChannelDose);
                    }
                    context.SaveChanges(); // Save changes after adding all doses
                    MessageBox.Show("Channel Record created Successfully!");

                    var newMedicine = productMedicalRepository.GetAll(newChannelRecord.Id, RecordTypeEnum.Channel);
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
                        AffectedEntity = EntitiesEnum.ChannelRecords,
                        AffectedEntityId = channelRecordModel.Id,
                        ActionType = ActionTypeEnum.Add,
                        OldValues = "-",
                        NewValues = JsonConvert.SerializeObject(channelRecordModel) + "\n\nMedicine:\n" + JsonConvert.SerializeObject(filteredNewMedicineData),
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

        public bool Edit(ChannelRecord channelRecordModel, Dictionary<int, int> medicalDoseData)
        {
            try
            {
                using (AppDataContext context = new AppDataContext())
                {
                    // Fetch the existing medical record
                    var channelRecord = context.ChannelRecords
                        .FirstOrDefault(x => x.Id.Equals(channelRecordModel.Id)); // Assuming ChannelRecordId is Id

                    if (channelRecord != null)
                    {
                        // Check if a record with the same ChitNumber exists for the given day
                        if (!validator.IsChitNumberUnique(context, channelRecordModel.ChitNumber, channelRecordModel.AdmitDate.Date) 
                            && channelRecordModel.ChitNumber != channelRecord.ChitNumber)
                        {
                            MessageBox.Show("A record with the same chit number already exists for the specified day. Try a different chit number.");
                            return false;
                        }

                        // Update the necessary fields
                        channelRecord.ChitNumber = channelRecordModel.ChitNumber;
                        channelRecord.OPDCharge = channelRecordModel.OPDCharge;
                        channelRecord.OtherCharges = channelRecordModel.OtherCharges;
                        channelRecord.PharmacyBill = channelRecordModel.PharmacyBill;
                        channelRecord.TotalBill = channelRecordModel.TotalBill;
                        channelRecord.PatientId = channelRecordModel.PatientId;
                        channelRecord.DoctorId = channelRecordModel.DoctorId;
                        channelRecord.AddedBy = channelRecordModel.AddedBy;
                        channelRecord.DocComm = channelRecordModel.DocComm;
                        channelRecord.Nurse1Comm = channelRecordModel.Nurse1Comm;
                        channelRecord.Nurse2Comm = channelRecordModel.Nurse2Comm;
                        channelRecord.Nurse1Id = channelRecordModel.Nurse1Id;
                        channelRecord.Nurse2Id = channelRecordModel.Nurse2Id;
                        channelRecord.Nurse1 = channelRecordModel.Nurse1;
                        channelRecord.Nurse2 = channelRecordModel.Nurse2;
                        channelRecord.ConsultantFee = channelRecordModel.ConsultantFee;

                        context.Entry(channelRecord).State = EntityState.Modified;
                        context.SaveChanges();

                        // Remove existing doses for the channel record
                        var medicalDoseListForChannelRecord = context.ProductMedicalRecords
                            .Where(p => p.ChannelRecordId.Equals(channelRecordModel.Id) && p.RecordTypeId.Equals((int)RecordTypeEnum.Channel)).ToList();

                        foreach (var medicalDose in medicalDoseListForChannelRecord)
                        {
                            context.ProductMedicalRecords.Remove(medicalDose);

                            //Add product current quantities
                            productRepository.EditCurrentQuantityOnly(medicalDose.ProductId, medicalDose.Units);
                        }
                        context.SaveChanges(); // Save changes after removing old doses

                        // Add new doses
                        for (int i = 0; i < medicalDoseData.Count(); i++)
                        {
                            int productId = medicalDoseData.Keys.ElementAt(i);
                            int units = medicalDoseData.Values.ElementAt(i);

                            // Fetch the product to get the selling price
                            var product = context.Products.Find(productId);
                            if (product == null)
                            {
                                MessageBox.Show($"Product with ID {productId} not found.");
                                return false;
                            }

                            var newChannelDose = new ProductMedicalRecord
                            {
                                ChannelRecordId = channelRecordModel.Id,
                                AdmitDate = channelRecordModel.AdmitDate,
                                ProductId = productId,
                                Units = units,
                                SoldPrice = product.SellingPrice * units, // Calculate SoldPrice
                                RecordTypeId = (int)RecordTypeEnum.Channel,
                            };

                            productMedicalRepository.Add(newChannelDose);
                        }
                        context.SaveChanges(); // Save changes after adding all doses
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


        public IEnumerable<ChannelRecordView> GetAll(string searchWord)
        {
            List<ChannelRecordView> channelRecords = new List<ChannelRecordView>();
            try
            {
                using (AppDataContext context = new AppDataContext())
                {
                    var channelRecordsList = context.ChannelRecords
                        .Include(pr => pr.Doctor)
                        .Where(p => p.Patient.FirstName.Contains(searchWord) || p.Patient.LastName.Contains(searchWord) ||
                        p.Doctor.FirstName.Contains(searchWord) || p.Doctor.LastName.Contains(searchWord) || p.ChitNumber.Contains(searchWord) || 
                        p.AdmitDate.Date.ToString().Contains(searchWord))
                        .OrderByDescending(p => p.AdmitDate) // Sort by AdmitDate in descending order. recent up
                        .Select(mr => new ChannelRecordView
                        {
                            Id = mr.Id,
                            ChitNumber = mr.ChitNumber,
                            TotalBill = mr.TotalBill,
                            PatientName = mr.Patient.FirstName + " " + mr.Patient.LastName,
                            DoctorName = mr.Doctor.FirstName + " " + mr.Doctor.LastName,
                            AdmitDate = mr.AdmitDate.ToString("dd-MMM-yyyy HH:mm"),
                            AddedBy = $"{userRepository.GetByID(mr.AddedBy).FirstName} {userRepository.GetByID(mr.AddedBy).LastName}",
                        })
                        .ToList();

                    channelRecords.AddRange(channelRecordsList);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}");
            }
            return channelRecords;
        }

        public ChannelRecord GetByID(int id)
        {
            ChannelRecord channelRecord = null;
            using (var connection = GetConnection())
            using (var command = new SqlCommand())
            {
                try
                {
                    connection.Open();
                    command.Connection = connection;
                    command.CommandText = "SELECT * FROM [ChannelRecords] WHERE id=@id";
                    command.Parameters.Add("@id", SqlDbType.Int).Value = id;

                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            channelRecord = new ChannelRecord()
                            {
                                Id = reader.GetInt32(reader.GetOrdinal("Id")),
                                ChitNumber = reader.IsDBNull(reader.GetOrdinal("ChitNumber")) ? null : reader.GetString(reader.GetOrdinal("ChitNumber")),
                                AdmitDate = reader.IsDBNull(reader.GetOrdinal("AdmitDate")) ? DateTime.Now : reader.GetDateTime(reader.GetOrdinal("AdmitDate")),
                                OtherCharges = reader.IsDBNull(reader.GetOrdinal("OtherCharges")) ? 0 : reader.GetFloat(reader.GetOrdinal("OtherCharges")),
                                OPDCharge = reader.IsDBNull(reader.GetOrdinal("OPDCharge")) ? 0 : reader.GetFloat(reader.GetOrdinal("OPDCharge")),
                                PharmacyBill = reader.IsDBNull(reader.GetOrdinal("PharmacyBill")) ? 0 : reader.GetFloat(reader.GetOrdinal("PharmacyBill")),
                                TotalBill = reader.IsDBNull(reader.GetOrdinal("TotalBill")) ? 0 : reader.GetFloat(reader.GetOrdinal("TotalBill")),
                                PatientId = reader.IsDBNull(reader.GetOrdinal("PatientId")) ? 0 : reader.GetInt32(reader.GetOrdinal("PatientId")),
                                DoctorId = reader.IsDBNull(reader.GetOrdinal("DoctorId")) ? 0 : reader.GetInt32(reader.GetOrdinal("DoctorId")),
                                AddedBy = reader.IsDBNull(reader.GetOrdinal("AddedBy")) ? 0 : reader.GetInt32(reader.GetOrdinal("AddedBy")),
                                DocComm = reader.IsDBNull(reader.GetOrdinal("DocComm")) ? 0 : reader.GetFloat(reader.GetOrdinal("DocComm")),
                                Nurse1Comm = reader.IsDBNull(reader.GetOrdinal("Nurse1Comm")) ? 0 : reader.GetFloat(reader.GetOrdinal("Nurse1Comm")),
                                Nurse2Comm = reader.IsDBNull(reader.GetOrdinal("Nurse2Comm")) ? 0 : reader.GetFloat(reader.GetOrdinal("Nurse2Comm")),
                                Nurse1Id = reader.IsDBNull(reader.GetOrdinal("Nurse1Id")) ? null : reader.GetInt32(reader.GetOrdinal("Nurse1Id")),
                                Nurse2Id = reader.IsDBNull(reader.GetOrdinal("Nurse2Id")) ? null : reader.GetInt32(reader.GetOrdinal("Nurse2Id")),
                                ConsultantFee = reader.IsDBNull(reader.GetOrdinal("ConsultantFee")) ? 0 : reader.GetFloat(reader.GetOrdinal("ConsultantFee")),
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
            return channelRecord;
        }


        public bool Remove(int id)
        {
            try
            {
                using (AppDataContext context = new AppDataContext())
                {
                    productMedicalRepository.RemoveChannelRecord(id);
                    var channelRecord = context.ChannelRecords.FirstOrDefault(x => x.Id.Equals(id));
                    if (channelRecord != null)
                    {
                        context.ChannelRecords.Remove(channelRecord);
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
