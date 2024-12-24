using iText.Kernel.XMP.Impl;
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
    public class ProductMedicalRecordRepository : BaseRepository, IProductMedicalRecordRepository
    {
        public IProductRepository productRepository;

        public ProductMedicalRecordRepository()
        {
            productRepository = new ProductRepository();
        }

        public bool Add(ProductMedicalRecord productMedicalRecordModel)
        {
            try
            {
                using (AppDataContext context = new AppDataContext())
                {
                    // Check if the Product exists
                    var productExists = context.Products.Any(p => p.Id == productMedicalRecordModel.ProductId);
                    if (!productExists)
                    {
                        MessageBox.Show("Product does not exist. Please check the Product ID.");
                        return false;
                    }

                    var recordExists = false;

                    // Check if the MedicalRecord exists
                    if (productMedicalRecordModel.RecordTypeId == (int)RecordTypeEnum.Medical) {
                        recordExists = context.MedicalRecords.Any(mr => mr.Id == productMedicalRecordModel.MedicalRecordId);
                    }
                    // Check if the LabRecord exists
                    if (productMedicalRecordModel.RecordTypeId == (int)RecordTypeEnum.Lab)
                    {
                        recordExists = context.LabRecords.Any(mr => mr.Id == productMedicalRecordModel.LabRecordId);
                    }
                    // Check if the ProcedureRecord exists
                    if (productMedicalRecordModel.RecordTypeId == (int)RecordTypeEnum.Procedure)
                    {
                        recordExists = context.ProcedureRecords.Any(mr => mr.Id == productMedicalRecordModel.ProcedureRecordId);
                    }

                    if (!recordExists)
                    {
                        MessageBox.Show("Record does not exist. Please check the Record ID.");
                        return false;
                    }

                    // Create a new ProductMedicalRecord object
                    var newProductMedicalRecord = new ProductMedicalRecord
                    {
                        ProductId = productMedicalRecordModel.ProductId,
                        AdmitDate = productMedicalRecordModel.AdmitDate,
                        Units = productMedicalRecordModel.Units,
                        SoldPrice = productMedicalRecordModel.SoldPrice,
                        MedicalRecordId = productMedicalRecordModel.MedicalRecordId,
                        LabRecordId = productMedicalRecordModel.LabRecordId,
                        ProcedureRecordId = productMedicalRecordModel.ProcedureRecordId,
                        RecordType = productMedicalRecordModel.RecordType,
                        RecordTypeId = productMedicalRecordModel.RecordTypeId
                    };

                    // Add the new record to the context
                    context.ProductMedicalRecords.Add(newProductMedicalRecord);
                    context.SaveChanges();
                    productRepository.EditCurrentQuantityOnly(productMedicalRecordModel.ProductId, -productMedicalRecordModel.Units);
                    //MessageBox.Show("Record added successfully!");
                    return true;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}");
                return false;
            }
        }


        public bool Edit(ProductMedicalRecord productMedicalRecordModel)
        {
            try
            {
                using (AppDataContext context = new AppDataContext())
                {
                    // Retrieve the existing medical dose record
                    var productMedicalRecord = context.ProductMedicalRecords.FirstOrDefault(x => x.Id == productMedicalRecordModel.Id && x.RecordTypeId == productMedicalRecordModel.RecordTypeId);
                    if (productMedicalRecord == null)
                    {
                        MessageBox.Show("Record not found!");
                        return false;
                    }

                    // Check if the associated Product exists
                    var productExists = context.Products.Any(p => p.Id == productMedicalRecordModel.ProductId);
                    if (!productExists)
                    {
                        MessageBox.Show("Product does not exist. Please check the Product ID.");
                        return false;
                    }

                    var recordExists = false;
                    // Check if the MedicalRecord exists
                    if (productMedicalRecordModel.RecordTypeId == (int)RecordTypeEnum.Medical)
                    {
                        recordExists = context.MedicalRecords.Any(mr => mr.Id == productMedicalRecordModel.MedicalRecordId);
                    }
                    // Check if the LabRecord exists
                    if (productMedicalRecordModel.RecordTypeId == (int)RecordTypeEnum.Lab)
                    {
                        recordExists = context.LabRecords.Any(mr => mr.Id == productMedicalRecordModel.LabRecordId);
                    }
                    // Check if the ProcedureRecord exists
                    if (productMedicalRecordModel.RecordTypeId == (int)RecordTypeEnum.Procedure)
                    {
                        recordExists = context.ProcedureRecords.Any(mr => mr.Id == productMedicalRecordModel.ProcedureRecordId);
                    }

                    if (!recordExists)
                    {
                        MessageBox.Show("Record does not exist. Please check the Record ID.");
                        return false;
                    }

                    // Update the fields
                    productMedicalRecord.ProductId = productMedicalRecordModel.ProductId; // Assuming ProductName is the correct property
                    productMedicalRecord.AdmitDate = productMedicalRecordModel.AdmitDate;
                    productMedicalRecord.Units = productMedicalRecordModel.Units;
                    productMedicalRecord.SoldPrice = productMedicalRecordModel.SoldPrice;
                    productMedicalRecord.MedicalRecordId = productMedicalRecordModel.MedicalRecordId;
                    productMedicalRecord.LabRecordId = productMedicalRecordModel.LabRecordId;
                    productMedicalRecord.ProcedureRecordId = productMedicalRecordModel.ProcedureRecordId;
                    productMedicalRecord.RecordTypeId = productMedicalRecordModel.RecordTypeId;

                    // Mark the entity as modified and save changes
                    context.Entry(productMedicalRecord).State = EntityState.Modified;
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


        public IEnumerable<ProductMedicalRecord> GetAll(int recordId, RecordTypeEnum recordTypeId)
        {
            List<ProductMedicalRecord> productMedicalRecords = new List<ProductMedicalRecord>();
            try
            {
                using (AppDataContext context = new AppDataContext())
                {
                    // Check if the MedicalRecord exists
                    if (recordTypeId == RecordTypeEnum.Medical)
                    {
                        // Fetch medical doses related to the specified medical record ID
                        productMedicalRecords = context.ProductMedicalRecords
                            .Where(m => m.MedicalRecordId == recordId && m.RecordTypeId == (int)recordTypeId)
                            .Include(m => m.Product)  // Include related Product if necessary
                            .ToList();
                    }
                    // Check if the LabRecord exists
                    if (recordTypeId == RecordTypeEnum.Lab)
                    {
                        // Fetch medical doses related to the specified lab record ID
                        productMedicalRecords = context.ProductMedicalRecords
                            .Where(m => m.LabRecordId == recordId && m.RecordTypeId == (int)recordTypeId)
                            .Include(m => m.Product)  // Include related Product if necessary
                            .ToList();
                    }
                    // Check if the ProcedureRecord exists
                    if (recordTypeId == RecordTypeEnum.Procedure)
                    {
                        // Fetch medical doses related to the specified procedure record ID
                        productMedicalRecords = context.ProductMedicalRecords
                            .Where(m => m.ProcedureRecordId == recordId && m.RecordTypeId == (int)recordTypeId)
                            .Include(m => m.Product)  // Include related Product if necessary
                            .ToList();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}");
            }
            return productMedicalRecords;
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

        public bool RemoveMedicalRecord(int id)
        {
            try
            {
                using (AppDataContext context = new AppDataContext())
                {
                    var records = context.ProductMedicalRecords.Where(x => x.MedicalRecordId.Equals(id));
                    if (records == null)
                    {
                        MessageBox.Show("Medical record does not exist. Please check the ID.");
                        return false;
                    }

                    foreach (var record in records)
                    {
                        context.ProductMedicalRecords.Remove(record);

                        //Add product current quantities
                        productRepository.EditCurrentQuantityOnly(record.ProductId, record.Units);
                    }
                    context.SaveChanges();
                    //MessageBox.Show("Deleted Successfully!");
                    return true;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}");
                return false;
            }
        }

        public bool RemoveProcedureRecord(int id)
        {
            try
            {
                using (AppDataContext context = new AppDataContext())
                {
                    var records = context.ProductMedicalRecords.Where(x => x.ProcedureRecordId.Equals(id));
                    if (records == null)
                    {
                        MessageBox.Show("Procedure record does not exist. Please check the ID.");
                        return false;
                    }
                    foreach (var record in records)
                    {
                        context.ProductMedicalRecords.Remove(record);

                        //Add product current quantities
                        productRepository.EditCurrentQuantityOnly(record.ProductId, record.Units);
                    }
                    context.SaveChanges();
                    //MessageBox.Show("Deleted Successfully!");
                    return true;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}");
                return false;
            }
        }

        public bool RemoveLabRecord(int id)
        {
            try
            {
                using (AppDataContext context = new AppDataContext())
                {
                    var records = context.ProductMedicalRecords.Where(x => x.LabRecordId.Equals(id));
                    if (records == null)
                    {
                        MessageBox.Show("Lab record does not exist. Please check the ID.");
                        return false;
                    }
                    foreach (var record in records)
                    {
                        context.ProductMedicalRecords.Remove(record);

                        //Add product current quantities
                        productRepository.EditCurrentQuantityOnly(record.ProductId, record.Units);
                    }
                    context.SaveChanges();
                    //MessageBox.Show("Deleted Successfully!");
                    return true;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}");
                return false;
            }
        }
        /*
        MedicineId = ,
        TimePeriod = ,
        MedicalDoseId = 
         */
    }
}
