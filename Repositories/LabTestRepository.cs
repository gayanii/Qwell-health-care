using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using QWellApp.DBConnection;
using QWellApp.Enums;
using QWellApp.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Drawing.Printing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace QWellApp.Repositories
{
    public class LabTestRepository : BaseRepository, ILabTestRepository
    {

        public LabTestRepository()
        {
        }

        public bool Add(LabTest labTestModel)
        {
            try
            {
                using (AppDataContext context = new AppDataContext())
                {
                    // Check if a record with the same hospital name and test name already exists
                    bool labTestFound = context.LabTests
                        .Any(record => record.HospitalName == labTestModel.HospitalName && record.TestName == labTestModel.TestName);

                    if (labTestFound)
                    {
                        MessageBox.Show("This lab test with the same hospital is already found. Try a different combination!");
                        return false;
                    }
                    else
                    {
                        var newLabTest = new LabTest
                        {
                            HospitalName = labTestModel.HospitalName,
                            TestName = labTestModel.TestName,
                            Cost = labTestModel.Cost,
                            Discount = labTestModel.Discount,
                            LabPaid = labTestModel.LabPaid,
                            Status = UserStatusEnum.Active.ToString()
                        };

                        context.LabTests.Add(newLabTest);
                        context.SaveChanges();
                        MessageBox.Show("Lab test created successfully!");
                        return true;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}");
                return false;
            }
        }


        public bool Edit(LabTest labTestModel)
        {
            try
            {
                using (AppDataContext context = new AppDataContext())
                {
                    // Find the existing lab test by its ID
                    var labTest = context.LabTests.FirstOrDefault(x => x.Id.Equals(labTestModel.Id));
                    if (labTest != null)
                    {
                        // Check if there is another record with the same hospital name and test name (excluding the current one)
                        bool labTestFound = context.LabTests.Any(record => record.HospitalName == labTestModel.HospitalName
                            && record.TestName == labTestModel.TestName && record.Id != labTestModel.Id);

                        if (labTestFound)
                        {
                            MessageBox.Show("A lab test with the same hospital name and test name already exists. Try a different combination!");
                            return false;
                        }
                        else
                        {
                            // Update fields of the lab test with new values from the labTestModel
                            labTest.HospitalName = labTestModel.HospitalName;
                            labTest.TestName = labTestModel.TestName;
                            labTest.Cost = labTestModel.Cost;
                            labTest.Discount = labTestModel.Discount;
                            labTest.LabPaid = labTestModel.LabPaid;
                            labTest.Status = labTestModel.Status;

                            // Mark the entity as modified and save changes
                            context.Entry(labTest).State = EntityState.Modified;
                            context.SaveChanges();
                            MessageBox.Show("Lab test updated successfully!");
                            return true;
                        }
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


        public IEnumerable<LabTestView> GetAll(string searchWord)
        {
            List<LabTestView> LabTests = new List<LabTestView>();
            try
            {
                using (AppDataContext context = new AppDataContext())
                {
                    // Normalize the search term for case-insensitive search
                    string normalizedSearchWord = searchWord.Trim().ToLower();

                    var labTestList = context.LabTests
                        .Where(lt => lt.HospitalName.ToLower().Contains(normalizedSearchWord) || lt.Status.ToLower().Contains(normalizedSearchWord) ||
                            lt.TestName.ToLower().Contains(normalizedSearchWord) || lt.Cost.ToString().Contains(normalizedSearchWord) ||
                            lt.Discount.ToString().Contains(normalizedSearchWord) || lt.LabPaid.Contains(normalizedSearchWord))
                        .OrderByDescending(lt => lt.Id) // Sort by id in descending order. recent up
                        .ToList();

                    // Map each LabTests to LabTestViews
                    foreach (var labTest in labTestList)
                    {
                        LabTestView singleLabTest = new LabTestView()
                        {
                            Id = labTest.Id,
                            HospitalName = labTest.HospitalName, 
                            TestName = labTest.TestName,
                            Cost = labTest.Cost,
                            Discount = labTest.Discount,
                            LabPaid = labTest.LabPaid,
                            Status = labTest.Status,
                        };

                        LabTests.Add(singleLabTest);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}");
            }
            return LabTests;
        }


        public LabTest GetByID(int id)
        {
            LabTest labTest = null;
            using (var connection = GetConnection())
            using (var command = new SqlCommand())
            {
                try
                {
                    // Open the database connection
                    connection.Open();
                    command.Connection = connection;

                    // Define the SQL query to fetch data from LabTests based on the LabTest ID
                    command.CommandText = "select * from [LabTests] where Id=@id";
                    command.Parameters.Add("@id", SqlDbType.Int).Value = id;

                    // Execute the query and process the results
                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            labTest = new LabTest()
                            {
                                Id = reader.GetInt32(reader.GetOrdinal("Id")),
                                HospitalName = reader.IsDBNull(reader.GetOrdinal("HospitalName")) ? null : reader.GetString(reader.GetOrdinal("HospitalName")),
                                TestName = reader.IsDBNull(reader.GetOrdinal("TestName")) ? null : reader.GetString(reader.GetOrdinal("TestName")),
                                Cost = reader.IsDBNull(reader.GetOrdinal("Cost")) ? 0 : reader.GetFloat(reader.GetOrdinal("Cost")),
                                Discount = reader.IsDBNull(reader.GetOrdinal("Discount")) ? (float?)null : reader.GetFloat(reader.GetOrdinal("Discount")),
                                LabPaid = reader.IsDBNull(reader.GetOrdinal("LabPaid")) ? null : reader.GetString(reader.GetOrdinal("LabPaid")),
                                Status = reader.IsDBNull(reader.GetOrdinal("Status")) ? null : reader.GetString(reader.GetOrdinal("Status")),
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
            return labTest;
        }


        public bool Remove(int id)
        {
            try
            {
                using (AppDataContext context = new AppDataContext())
                {
                    var labTest = context.LabTests.FirstOrDefault(x => x.Id.Equals(id));
                    if (labTest != null)
                    {
                        labTest.Status = UserStatusEnum.Inactive.ToString();
                        context.Entry(labTest).State = EntityState.Modified;
                        context.SaveChanges();
                        MessageBox.Show("Status Changed Successfully!");
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
