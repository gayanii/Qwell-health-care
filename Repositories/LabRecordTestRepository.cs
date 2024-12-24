using Microsoft.EntityFrameworkCore;
using QWellApp.DBConnection;
using QWellApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace QWellApp.Repositories
{
    public class LabRecordTestRepository : BaseRepository, ILabRecordTestRepository
    {
        public bool Add(LabRecordTest labRecordTestModel)
        {
            try
            {
                using (AppDataContext context = new AppDataContext())
                {
                    // Check if the lab test exists
                    var labTestExists = context.LabTests.Any(p => p.Id == labRecordTestModel.LabTestId);
                    if (!labTestExists)
                    {
                        MessageBox.Show("Lab test does not exist. Please check the lab test ID.");
                        return false;
                    }

                    // Check if the lab record exists
                    var labRecordExists = context.LabRecords.Any(mr => mr.Id == labRecordTestModel.LabRecordId);
                    if (!labRecordExists)
                    {
                        MessageBox.Show("Lab record does not exist. Please check the Lab Record ID.");
                        return false;
                    }

                    // Create a new LabRecordTest object
                    var newLabRecordTest = new LabRecordTest
                    {
                        LabTestId = labRecordTestModel.LabTestId,
                        LabRecordId = labRecordTestModel.LabRecordId,
                    };

                    // Add the new record to the context
                    context.LabRecordTests.Add(newLabRecordTest);
                    context.SaveChanges();
                    //MessageBox.Show("Lab test record added successfully!");
                    return true;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}");
                return false;
            }
        }


        public bool Edit(LabRecordTest labRecordTestModel)
        {
            try
            {
                using (AppDataContext context = new AppDataContext())
                {
                    // Retrieve the existing lab test record
                    var labRecordTest = context.LabRecordTests.FirstOrDefault(x => x.Id == labRecordTestModel.Id);
                    if (labRecordTest == null)
                    {
                        MessageBox.Show("Lab test record not found!");
                        return false;
                    }

                    // Check if the associated lab test exists
                    var labTestExists = context.LabTests.Any(p => p.Id == labRecordTestModel.LabTestId);
                    if (!labTestExists)
                    {
                        MessageBox.Show("Lab test does not exist. Please check the lab test ID.");
                        return false;
                    }

                    // Check if the associated labRecord exists
                    var labRecordExists = context.LabRecords.Any(lr => lr.Id == labRecordTestModel.LabRecordId);
                    if (!labRecordExists)
                    {
                        MessageBox.Show("Lab record does not exist. Please check the Lab Record ID.");
                        return false;
                    }

                    // Update the fields
                    labRecordTest.LabTestId = labRecordTestModel.LabTestId;
                    labRecordTest.LabRecordId = labRecordTestModel.LabRecordId;

                    // Mark the entity as modified and save changes
                    context.Entry(labRecordTest).State = EntityState.Modified;
                    context.SaveChanges();
                    MessageBox.Show("Updated Successfully!");
                    return true;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}");
                return false;
            }
        }


        public IEnumerable<LabRecordTest> GetAll(int labRecordId)
        {
            List<LabRecordTest> labRecordTests = new List<LabRecordTest>();
            try
            {
                using (AppDataContext context = new AppDataContext())
                {
                    // Fetch lab record tests related to the specified lab record ID
                    var labRecordTestList = context.LabRecordTests
                        .Where(m => m.LabRecordId == labRecordId)
                        .Include(m => m.LabTest)  // Include related lab test if necessary
                        .ToList();

                    // Use LINQ to project the results into the lab record test list
                    labRecordTests = labRecordTestList.Select(labRecordTest => new LabRecordTest
                    {
                        Id = labRecordTest.Id,
                        LabTestId = labRecordTest.LabTestId,
                        LabRecordId = labRecordTest.LabRecordId
                    }).ToList();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}");
            }
            return labRecordTests;
        }


        //public IEnumerable<ProductMedicalRecord> GetByID(int medicalRecordId)
        //{
        //    List<ProductMedicalRecord> productMedicalRecords = new List<ProductMedicalRecord>();
        //    try
        //    {
        //        using (AppDataContext context = new AppDataContext())
        //        {
        //            // Retrieve medical doses related to the specified medical record ID
        //            var medicalDosesList = context.ProductMedicalRecords
        //                .Where(m => m.MedicalRecordId == medicalRecordId)
        //                .Include(m => m.Product) // Include related Product if necessary
        //                .ToList();

        //            // Use LINQ to project results into the list of ProductMedicalRecord
        //            productMedicalRecords = medicalDosesList.Select(medicalDose => new ProductMedicalRecord
        //            {
        //                Id = medicalDose.Id,
        //                ProductId = medicalDose.ProductId,
        //                Units = medicalDose.Units,
        //                SoldPrice = medicalDose.SoldPrice,
        //                MedicalRecordId = medicalDose.MedicalRecordId
        //            }).ToList();
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        MessageBox.Show($"Error: {ex.Message}");
        //    }
        //    return productMedicalRecords;
        //}

        public bool Remove(int id)
        {
            throw new NotImplementedException();
        }
        /*
        MedicineId = ,
        TimePeriod = ,
        MedicalDoseId = 
         */
    }
}
