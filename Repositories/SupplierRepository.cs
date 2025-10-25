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
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ListView;

namespace QWellApp.Repositories
{
    public class SupplierRepository : BaseRepository, ISupplierRepository
    {
        public bool Add(Supplier supplierModel)
        {
            try
            {
                using (AppDataContext context = new AppDataContext())
                {
                    bool supplierFound = context.Suppliers.Any(sup => sup.CompanyName == supplierModel.CompanyName);

                    if (supplierFound)
                    {
                        MessageBox.Show("This supplier company name already found. Try a different name!");
                        return false;
                    }
                    else
                    {
                        var newSupplier = new Supplier
                        {
                            CompanyName = supplierModel.CompanyName,
                            Address = supplierModel.Address,
                            TelephoneNum = supplierModel.TelephoneNum,
                            Email = supplierModel.Email,
                            Status = UserStatusEnum.Active.ToString()
                        };

                        context.Suppliers.Add(newSupplier);
                        context.SaveChanges();
                        MessageBox.Show("Supplier created Successfully!");
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

        public bool Edit(Supplier supplierModel)
        {
            try
            {
                using (AppDataContext context = new AppDataContext())
                {
                    var supplier = context.Suppliers.FirstOrDefault(x => x.Id.Equals(supplierModel.Id));
                    if (supplier != null)
                    {
                        bool supplierFound = context.Suppliers.Any(sup => sup.CompanyName == supplierModel.CompanyName && !sup.Id.Equals(supplierModel.Id));

                        if (supplierFound)
                        {
                            MessageBox.Show("This supplier company name already found. Try a different name!");
                            return false;
                        }
                        else
                        {
                            supplier.CompanyName = supplierModel.CompanyName;
                            supplier.Address = supplierModel.Address;
                            supplier.TelephoneNum = supplierModel.TelephoneNum;
                            supplier.Email = supplierModel.Email;
                            supplier.Status = supplierModel.Status;

                            context.Entry(supplier).State = EntityState.Modified;
                            context.SaveChanges();
                            MessageBox.Show("Updated Successfully!");
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

        public IEnumerable<SupplierView> GetAll(string searchWord)
        {
            List<SupplierView> suppliers = new List<SupplierView>();
            try
            {
                using (AppDataContext context = new AppDataContext())
                {
                    // Normalize the search term for case-insensitive search
                    string normalizedSearchWord = searchWord.Trim().ToLower();

                    var supplierList = context.Suppliers
                        .Where(p => p.CompanyName.ToLower().Contains(normalizedSearchWord) || p.Status.ToLower().Contains(normalizedSearchWord) ||
                        p.Address.ToLower().Contains(normalizedSearchWord) || p.TelephoneNum.Contains(normalizedSearchWord) ||
                        p.Email.Contains(normalizedSearchWord))
                        .OrderBy(p => p.CompanyName) // Sort companyname in ascending order
                        .ToList();
                    foreach (var supplier in supplierList)
                    {
                        SupplierView singleSupplier = new SupplierView()
                        {
                            Id = supplier.Id,
                            CompanyName = supplier.CompanyName,
                            Address = supplier.Address,
                            TelephoneNum = supplier.TelephoneNum,
                            Email = supplier.Email,
                            Status = supplier.Status,
                        };
                        suppliers.Add(singleSupplier);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}");
            }
            return suppliers;
        }

        public Supplier GetByID(int id)
        {
            Supplier supplier = null;
            using (var connection = GetConnection())
            using (var command = new SqlCommand())
            {
                try
                {
                    GetConnection().Open();
                    connection.Open();
                    command.Connection = connection;
                    command.CommandText = "select * from [Suppliers] where Id=@id";
                    command.Parameters.Add("@id", SqlDbType.VarChar).Value = id;
                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            supplier = new Supplier()
                            {
                                Id = reader.GetInt32(reader.GetOrdinal("Id")),
                                CompanyName = (reader.IsDBNull("CompanyName")) ? null : reader.GetString(reader.GetOrdinal("CompanyName")),
                                Address = (reader.IsDBNull("Address")) ? null : reader.GetString(reader.GetOrdinal("Address")),
                                TelephoneNum = (reader.IsDBNull("TelephoneNum")) ? null : reader.GetString(reader.GetOrdinal("TelephoneNum")),
                                Email = (reader.IsDBNull("Email")) ? null : reader.GetString(reader.GetOrdinal("Email")),
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
            return supplier;
        }

        public bool Remove(int id)
        {
            try
            {
                using (AppDataContext context = new AppDataContext())
                {
                    var supplier = context.Suppliers.FirstOrDefault(x => x.Id.Equals(id));
                    if (supplier != null)
                    {
                        supplier.Status = UserStatusEnum.Inactive.ToString();
                        context.Entry(supplier).State = EntityState.Modified;
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
