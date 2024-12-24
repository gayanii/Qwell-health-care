using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
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
    public class MedicalRecordRepository : BaseRepository, IMedicalRecordRepository
    {
        public IUserRepository userRepository;
        public IProductRepository productRepository;
        public IProductMedicalRecordRepository productMedicalRepository;
        public Validation validator;

        public MedicalRecordRepository()
        {
            userRepository = new UserRepository();
            productRepository = new ProductRepository();
            productMedicalRepository = new ProductMedicalRecordRepository();
            validator = new Validation();
        }

        public bool Add(MedicalRecord medicalRecordModel, Dictionary<int, int> medicalDoseData)
        {
            try
            {
                using (AppDataContext context = new AppDataContext())
                {
                    if (!validator.IsChitNumberUnique(context, medicalRecordModel.ChitNumber, medicalRecordModel.AdmitDate.Date))
                    {
                        MessageBox.Show("A record with the same chit number already exists for the specified day. Try a different chit number.");
                        return false;
                    }

                    var newMedicalRecord = new MedicalRecord
                    {
                        ChitNumber = medicalRecordModel.ChitNumber,
                        OPDCharge = medicalRecordModel.OPDCharge,
                        OtherCharges = medicalRecordModel.OtherCharges,
                        PharmacyBill = medicalRecordModel.PharmacyBill,
                        AdmitDate = medicalRecordModel.AdmitDate,
                        TotalBill = medicalRecordModel.TotalBill,
                        PatientId = medicalRecordModel.PatientId,
                        DoctorId = medicalRecordModel.DoctorId, // Include DoctorId if needed
                        AddedBy = medicalRecordModel.AddedBy, // Assuming this is provided
                        Doctor = medicalRecordModel.Doctor,
                        Patient = medicalRecordModel.Patient,
                        DocComm = medicalRecordModel.DocComm,
                        Nurse1Comm = medicalRecordModel.Nurse1Comm,
                        Nurse2Comm = medicalRecordModel.Nurse2Comm,
                        Nurse1Id = medicalRecordModel.Nurse1Id,
                        Nurse2Id = medicalRecordModel.Nurse2Id,
                        Nurse1 = medicalRecordModel.Nurse1,
                        Nurse2 = medicalRecordModel.Nurse2,
                        ConsultantFee = medicalRecordModel.ConsultantFee,
                    };

                    context.MedicalRecords.Add(newMedicalRecord);
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

                        var newMedicalDose = new ProductMedicalRecord
                        {
                            MedicalRecordId = newMedicalRecord.Id,
                            AdmitDate = medicalRecordModel.AdmitDate,
                            ProductId = productId,
                            Units = units,
                            SoldPrice = product.SellingPrice * units, // Calculate SoldPrice
                            RecordTypeId = (int)RecordTypeEnum.Medical,
                        };

                        productMedicalRepository.Add(newMedicalDose);
                    }
                    context.SaveChanges(); // Save changes after adding all doses
                    MessageBox.Show("Medical Record created Successfully!");
                    return true;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}");
                return false;
            }
        }

        public bool Edit(MedicalRecord medicalRecordModel, Dictionary<int, int> medicalDoseData)
        {
            try
            {
                using (AppDataContext context = new AppDataContext())
                {
                    // Fetch the existing medical record
                    var medicalRecord = context.MedicalRecords
                        .FirstOrDefault(x => x.Id.Equals(medicalRecordModel.Id)); // Assuming MedicalRecordId is Id

                    if (medicalRecord != null)
                    {
                        // Check if a record with the same ChitNumber exists for the given day
                        if (!validator.IsChitNumberUnique(context, medicalRecordModel.ChitNumber, medicalRecordModel.AdmitDate.Date) 
                            && medicalRecordModel.ChitNumber != medicalRecord.ChitNumber)
                        {
                            MessageBox.Show("A record with the same chit number already exists for the specified day. Try a different chit number.");
                            return false;
                        }

                        // Update the necessary fields
                        medicalRecord.ChitNumber = medicalRecordModel.ChitNumber;
                        medicalRecord.OPDCharge = medicalRecordModel.OPDCharge;
                        medicalRecord.OtherCharges = medicalRecordModel.OtherCharges;
                        medicalRecord.PharmacyBill = medicalRecordModel.PharmacyBill;
                        medicalRecord.AdmitDate = medicalRecordModel.AdmitDate;
                        medicalRecord.TotalBill = medicalRecordModel.TotalBill;
                        medicalRecord.PatientId = medicalRecordModel.PatientId;
                        medicalRecord.DoctorId = medicalRecordModel.DoctorId;
                        medicalRecord.AddedBy = medicalRecordModel.AddedBy;
                        medicalRecord.DocComm = medicalRecordModel.DocComm;
                        medicalRecord.Nurse1Comm = medicalRecordModel.Nurse1Comm;
                        medicalRecord.Nurse2Comm = medicalRecordModel.Nurse2Comm;
                        medicalRecord.Nurse1Id = medicalRecordModel.Nurse1Id;
                        medicalRecord.Nurse2Id = medicalRecordModel.Nurse2Id;
                        medicalRecord.Nurse1 = medicalRecordModel.Nurse1;
                        medicalRecord.Nurse2 = medicalRecordModel.Nurse2;
                        medicalRecord.ConsultantFee = medicalRecordModel.ConsultantFee;

                        context.Entry(medicalRecord).State = EntityState.Modified;
                        context.SaveChanges();

                        // Remove existing doses for the medical record
                        var medicalDoseListForMedicalRecord = context.ProductMedicalRecords
                            .Where(p => p.MedicalRecordId.Equals(medicalRecordModel.Id) && p.RecordTypeId.Equals((int)RecordTypeEnum.Medical)).ToList();

                        foreach (var medicalDose in medicalDoseListForMedicalRecord)
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

                            var newMedicalDose = new ProductMedicalRecord
                            {
                                MedicalRecordId = medicalRecordModel.Id,
                                AdmitDate = medicalRecordModel.AdmitDate,
                                ProductId = productId,
                                Units = units,
                                SoldPrice = product.SellingPrice * units, // Calculate SoldPrice
                                RecordTypeId = (int)RecordTypeEnum.Medical,
                            };

                            productMedicalRepository.Add(newMedicalDose);
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


        public IEnumerable<MedicalRecordView> GetAll(string searchWord)
        {
            List<MedicalRecordView> medicalRecords = new List<MedicalRecordView>();
            try
            {
                using (AppDataContext context = new AppDataContext())
                {
                    var medicalRecordsList = context.MedicalRecords
                        .Include(pr => pr.Doctor)
                        .Where(p => p.Patient.FirstName.Contains(searchWord) || p.Patient.LastName.Contains(searchWord) ||
                        p.Doctor.FirstName.Contains(searchWord) || p.Doctor.LastName.Contains(searchWord) || p.ChitNumber.Contains(searchWord) || 
                        p.AdmitDate.Date.ToString().Contains(searchWord))
                        .OrderByDescending(p => p.AdmitDate) // Sort by AdmitDate in descending order. recent up
                        .Select(mr => new MedicalRecordView
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

                    medicalRecords.AddRange(medicalRecordsList);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}");
            }
            return medicalRecords;
        }

        public MedicalRecord GetByID(int id)
        {
            MedicalRecord medicalRecord = null;
            using (var connection = GetConnection())
            using (var command = new SqlCommand())
            {
                try
                {
                    connection.Open();
                    command.Connection = connection;
                    command.CommandText = "SELECT * FROM [MedicalRecords] WHERE id=@id";
                    command.Parameters.Add("@id", SqlDbType.Int).Value = id;

                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            medicalRecord = new MedicalRecord()
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
            return medicalRecord;
        }


        public bool Remove(int id)
        {
            try
            {
                using (AppDataContext context = new AppDataContext())
                {
                    productMedicalRepository.RemoveMedicalRecord(id);
                    var medicalRecord = context.MedicalRecords.FirstOrDefault(x => x.Id.Equals(id));
                    if (medicalRecord != null)
                    {
                        context.MedicalRecords.Remove(medicalRecord);
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
        /*
        ShortDescription = ,
        Symptoms = ,
        AdmitDate = ,
        TotalCost = ,
        PatientId = ,
        DoctorId = ,
         */
    }
}
