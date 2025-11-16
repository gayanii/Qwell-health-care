using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic.ApplicationServices;
using QWellApp.DBConnection;
using QWellApp.Enums;
using QWellApp.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Runtime.ExceptionServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace QWellApp.Repositories
{
    public class PatientRepository : BaseRepository, IPatientRepository
    {
        public IMedicalRecordRepository medicalRecordRepository;
        public IProcedureRecordRepository procedureRecordRepository;
        public ILabRecordRepository labRecordRepository;
        public PatientRepository()
        {
            medicalRecordRepository = new MedicalRecordRepository();
            procedureRecordRepository = new ProcedureRecordRepository();
            labRecordRepository = new LabRecordRepository();
        }

        public bool Add(Patient patientModel)
        {
            try
            {
                using (AppDataContext context = new AppDataContext())
                {
                    // Normalize case for patient search
                    string normalizedNIC = (patientModel.NIC ?? "").Trim().ToLower();

                    // Check if patient already exists
                    bool patientFound = context.Patients.Any(patient =>
                        patient.NIC.Trim().ToLower() == normalizedNIC);

                    if (patientFound)
                    {
                        MessageBox.Show("A patient with the same NIC already exists. Please try a different NIC.");
                        return false;
                    }

                    // Create new patient
                    var newPatient = new Patient
                    {
                        FirstName = patientModel.FirstName,
                        LastName = patientModel.LastName,
                        Gender = patientModel.Gender,
                        MobileNum = patientModel.MobileNum,
                        TelephoneNum = patientModel.TelephoneNum,
                        Age = patientModel.Age,
                        AllergicHistory = patientModel.AllergicHistory, 
                        Weight = patientModel.Weight,
                        NIC = patientModel.NIC,
                        Status = UserStatusEnum.Active.ToString()
                    };

                    // Add new patient to the context
                    context.Patients.Add(newPatient);
                    context.SaveChanges();

                    MessageBox.Show("Patient created successfully!");
                    return true;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred while adding the patient: {ex.Message}");
                return false;
            }
        }


        public bool Edit(Patient patientModel)
        {
            try
            {
                using (AppDataContext context = new AppDataContext())
                {
                    // Retrieve the existing patient
                    var patient = context.Patients.FirstOrDefault(x => x.Id == patientModel.Id);
                    if (patient != null)
                    {
                        // Normalize input nic for search
                        string normalizedNIC = (patientModel.NIC ?? "").Trim().ToLower();

                        // Check if another patient with the same name exists (excluding the current one)
                        bool patientFound = context.Patients.Any(p =>
                            p.NIC.Trim().ToLower() == normalizedNIC && 
                            p.Id != patientModel.Id);

                        if (patientFound)
                        {
                            MessageBox.Show("A patient with the same NIC already exists. Please try a different NIC.");
                            return false;
                        }
                        else
                        {
                            // Update the patient's details
                            patient.FirstName = patientModel.FirstName;
                            patient.LastName = patientModel.LastName;
                            patient.MobileNum = patientModel.MobileNum;
                            patient.TelephoneNum = patientModel.TelephoneNum;
                            patient.Gender = patientModel.Gender;
                            patient.Age = patientModel.Age;
                            patient.AllergicHistory = patientModel.AllergicHistory; 
                            patient.Weight = patientModel.Weight; 
                            patient.NIC = patientModel.NIC;
                            patient.Status = patientModel.Status;

                            context.Entry(patient).State = EntityState.Modified;
                            context.SaveChanges();
                            MessageBox.Show("Patient updated successfully!");
                            return true;
                        }
                    }
                    MessageBox.Show("Failed to update: Patient not found.");
                    return false;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred while updating the patient: {ex.Message}");
                return false;
            }
        }


        public IEnumerable<PatientView> GetAll(string searchWord)
        {
            List<PatientView> patients = new List<PatientView>();
            try
            {
                using (AppDataContext context = new AppDataContext())
                {
                    // Normalize the search term for case-insensitive search
                    string normalizedSearchWord = searchWord.Trim().ToLower();

                    var patientList = context.Patients
                        .Where(p =>
                            p.FirstName.ToLower().Contains(normalizedSearchWord) ||
                            p.LastName.ToLower().Contains(normalizedSearchWord) ||
                            p.MobileNum.Contains(normalizedSearchWord) ||
                            p.TelephoneNum.Contains(normalizedSearchWord) ||
                            p.Age.Contains(normalizedSearchWord) ||
                            p.AllergicHistory.Contains(normalizedSearchWord) ||
                            p.Weight.Contains(normalizedSearchWord) ||
                            p.NIC.Contains(normalizedSearchWord) || 
                            p.Status.ToLower().Contains(normalizedSearchWord) || 
                            p.Gender.ToLower().Contains(normalizedSearchWord))
                        .OrderBy(p => p.FirstName) // Sort FirstName in ascending order
                        .Select(p => new PatientView // Projecting directly to PatientView
                        {
                            Id = p.Id,
                            FirstName = p.FirstName,
                            LastName = p.LastName,
                            MobileNum = p.MobileNum,
                            Age = p.Age,
                            NIC = p.NIC,
                            Status = p.Status.ToString(),
                        })
                        .ToList();

                    patients.AddRange(patientList); // Add all projected patients to the list
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred while fetching patients: {ex.Message}");
            }

            return patients;
        }


        public Patient GetByID(int id)
        {
            Patient patient = null;

            using (var connection = GetConnection())
            using (var command = new SqlCommand())
            {
                try
                {
                    connection.Open(); // Open connection
                    command.Connection = connection;
                    command.CommandText = "SELECT * FROM [Patients] WHERE Id = @id";
                    command.Parameters.Add("@id", SqlDbType.Int).Value = id; // Use SqlDbType.Int for PatientId

                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            patient = new Patient
                            {
                                Id = reader.GetInt32(reader.GetOrdinal("Id")),
                                FirstName = reader.IsDBNull(reader.GetOrdinal("FirstName")) ? null : reader.GetString(reader.GetOrdinal("FirstName")),
                                LastName = reader.IsDBNull(reader.GetOrdinal("LastName")) ? null : reader.GetString(reader.GetOrdinal("LastName")),
                                MobileNum = reader.IsDBNull(reader.GetOrdinal("MobileNum")) ? null : reader.GetString(reader.GetOrdinal("MobileNum")),
                                TelephoneNum = reader.IsDBNull(reader.GetOrdinal("TelephoneNum")) ? null : reader.GetString(reader.GetOrdinal("TelephoneNum")),
                                Gender = reader.IsDBNull(reader.GetOrdinal("Gender")) ? null : reader.GetString(reader.GetOrdinal("Gender")),
                                Age = reader.IsDBNull(reader.GetOrdinal("Age")) ? null : reader.GetString(reader.GetOrdinal("Age")),
                                AllergicHistory = reader.IsDBNull(reader.GetOrdinal("AllergicHistory")) ? null : reader.GetString(reader.GetOrdinal("AllergicHistory")),
                                Weight = reader.IsDBNull(reader.GetOrdinal("Weight")) ? null : reader.GetString(reader.GetOrdinal("Weight")),
                                NIC = reader.IsDBNull(reader.GetOrdinal("NIC")) ? null : reader.GetString(reader.GetOrdinal("NIC")),
                                Status = reader.IsDBNull(reader.GetOrdinal("Status")) ? null : reader.GetString(reader.GetOrdinal("Status")),
                            };
                        }
                        else
                        {
                            MessageBox.Show($"No patient found with ID: {id}");
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error retrieving patient: {ex.Message}");
                }
                finally
                {
                    connection.Close();
                }
                // No need to explicitly close the connection; it's handled by the using statement
            }

            return patient;
        }


        public bool Remove(int id)
        {
            try
            {
                using (AppDataContext context = new AppDataContext())
                {
                    // Fetch all related records
                    //var medicalRecords = context.MedicalRecords.Where(x => x.PatientId == id).ToList();
                    //var procedureRecords = context.ProcedureRecords.Where(x => x.PatientId == id).ToList();
                    //var labRecords = context.LabRecords.Where(x => x.PatientId == id).ToList();

                    // Delete related medical records
                    //if (medicalRecords.Any())
                    //{
                    //    context.MedicalRecords.RemoveRange(medicalRecords);
                    //    foreach (var record in medicalRecords)
                    //    {
                    //        var productMedicalRecords = context.ProductMedicalRecords.Where(x => x.MedicalRecordId == record.Id).ToList();
                    //        // Remove all ProductMedicalRecords in a single operation
                    //        context.ProductMedicalRecords.RemoveRange(productMedicalRecords);
                    //    }
                    //}

                    //// Delete related procedure records
                    //if (procedureRecords.Any())
                    //{
                    //    context.ProcedureRecords.RemoveRange(procedureRecords);
                    //    foreach (var record in procedureRecords)
                    //    {
                    //        var productMedicalRecords = context.ProductMedicalRecords.Where(x => x.ProcedureRecordId == record.Id).ToList();
                    //        // Remove all ProductMedicalRecords in a single operation
                    //        context.ProductMedicalRecords.RemoveRange(productMedicalRecords);
                    //    }
                    //}

                    //// Delete related lab records
                    //if (labRecords.Any())
                    //{
                    //    context.LabRecords.RemoveRange(labRecords);
                    //    foreach (var record in labRecords)
                    //    {
                    //        var productMedicalRecords = context.ProductMedicalRecords.Where(x => x.LabRecordId == record.Id).ToList();
                    //        // Remove all ProductMedicalRecords in a single operation
                    //        context.ProductMedicalRecords.RemoveRange(productMedicalRecords);
                    //    }
                    //}

                    // Find and delete the patient
                    var patient = context.Patients.FirstOrDefault(x => x.Id == id);
                    if (patient != null)
                    {
                        patient.Status = UserStatusEnum.Inactive.ToString();
                        context.Entry(patient).State = EntityState.Modified;
                        context.SaveChanges();
                        MessageBox.Show("Status Changed Successfully!");
                        return true;
                    }
                    else
                    {
                        MessageBox.Show("Patient not found. Nothing to delete.");
                        return false;
                    }
                }
            }
            catch (DbUpdateConcurrencyException ex)
            {
                MessageBox.Show("Data has been modified or deleted by another process.");
                return false;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}");
                return false;
            }
        }
    }
}
