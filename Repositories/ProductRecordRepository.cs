using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using QWellApp.DBConnection;
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
    public class ProductRecordRepository : BaseRepository, IProductRecordRepository
    {
        public ISupplierRepository supplierRepository;
        public IProductRepository productRepository;

        public ProductRecordRepository()
        {
            supplierRepository = new SupplierRepository();
            productRepository = new ProductRepository();
        }

        public bool Add(ProductRecord productModel)
        {
            try
            {
                using (AppDataContext context = new AppDataContext())
                {
                    // Check if a record with the same ProductId and SupplierId already exists
                    //bool productFound = context.ProductRecords
                    //    .Any(record => record.ProductId == productModel.ProductId && record.SupplierId == productModel.SupplierId);

                    //if (productFound)
                    //{
                    //    MessageBox.Show("This product record with the same supplier is already found. Try a different combination!");
                    //    return false;
                    //}
                    //else
                    //{
                        var newProductRecord = new ProductRecord
                        {
                            Barcode = productModel.Barcode,
                            ProductId = productModel.ProductId,
                            SupplierPrice = productModel.SupplierPrice,
                            SellingPrice = productModel.SellingPrice,
                            OrderedQuantity = productModel.OrderedQuantity,
                            ExpDate = productModel.ExpDate,
                            ReceivedDate = productModel.ReceivedDate,
                            SupplierId = productModel.SupplierId,
                            UserId = productModel.UserId, // The User who added the record
                            Supplier = productModel.Supplier,
                            Product = productModel.Product,
                        };

                        context.ProductRecords.Add(newProductRecord);
                        context.SaveChanges();

                        productRepository.EditCurrentQuantityOnly(productModel.ProductId, productModel.OrderedQuantity);
                        MessageBox.Show("Product record created successfully!");
                        return true;
                    //}
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}");
                return false;
            }
        }


        public bool Edit(ProductRecord productModel)
        {
            try
            {
                using (AppDataContext context = new AppDataContext())
                {
                    // Find the existing product record by its ID
                    var productRecord = context.ProductRecords.FirstOrDefault(x => x.Id.Equals(productModel.Id));
                    if (productRecord != null)
                    {
                        // Check if there is another record with the same ProductId and SupplierId (excluding the current one)
                        //bool productFound = context.ProductRecords.Any(record => record.ProductId == productModel.ProductId
                        //    && record.SupplierId == productModel.SupplierId && record.Id != productModel.Id);

                        //if (productFound)
                        //{
                        //    MessageBox.Show("A product record with the same product and supplier already exists. Try a different combination!");
                        //    return false;
                        //

                        var balance = productModel.OrderedQuantity - productRecord.OrderedQuantity;

                        // Update fields of the product record with new values from the productModel
                        productRecord.Barcode = productModel.Barcode;
                        productRecord.ProductId = productModel.ProductId;
                        productRecord.SupplierPrice = productModel.SupplierPrice;
                        productRecord.SellingPrice = productModel.SellingPrice;
                        productRecord.OrderedQuantity = productModel.OrderedQuantity;
                        productRecord.ExpDate = productModel.ExpDate;
                        productRecord.ReceivedDate = productModel.ReceivedDate;
                        productRecord.SupplierId = productModel.SupplierId;
                        productRecord.UserId = productModel.UserId;  // User who made the change
                        productRecord.Supplier = productModel.Supplier;
                        productRecord.Product = productModel.Product;

                        // Mark the entity as modified and save changes
                        context.Entry(productRecord).State = EntityState.Modified;
                        context.SaveChanges();

                        productRepository.EditCurrentQuantityOnly(productModel.ProductId, balance);
                        MessageBox.Show("Product record updated successfully!");
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


        public IEnumerable<ProductRecordView> GetAll(string searchWord)
        {
            List<ProductRecordView> productRecords = new List<ProductRecordView>();
            try
            {
                using (AppDataContext context = new AppDataContext())
                {
                    // Normalize the search term for case-insensitive search
                    string normalizedSearchWord = searchWord.Trim().ToLower();

                    // Fetch ProductRecords and include related entities like Product and Supplier for the search criteria
                    var productRecordList = context.ProductRecords
                        .Include(pr => pr.Product)   // Include the Product details
                        .Include(pr => pr.Supplier)  // Include the Supplier details
                        .Include(pr => pr.User)      // Include the User details (AddedBy)
                        .Where(pr =>
                            (pr.Barcode.Contains(normalizedSearchWord)) || (pr.Product.BrandName.Contains(normalizedSearchWord)) || (pr.Product.Generic.Contains(normalizedSearchWord)) ||
                            (pr.Supplier.CompanyName.Contains(normalizedSearchWord)) || (pr.User.FirstName.Contains(normalizedSearchWord)) || (pr.User.LastName.Contains(normalizedSearchWord)) ||
                            (pr.ReceivedDate.ToString().Contains(normalizedSearchWord)) || (pr.ExpDate.ToString().Contains(normalizedSearchWord))
                        )
                        .OrderByDescending(p => p.Id) // Sort by id in descending order. recent up
                        .ToList();

                    // Map each ProductRecord to ProductRecordView
                    foreach (var productRecord in productRecordList)
                    {
                        ProductRecordView singleProductRecord = new ProductRecordView()
                        {
                            Id = productRecord.Id,
                            Barcode = productRecord.Barcode,
                            BrandName = productRecord.Product.BrandName,
                            Generic = productRecord.Product.Generic,
                            ReceivedDate = productRecord.ReceivedDate.ToString("dd-MMM-yyyy HH:mm"),
                            OrderedQty = productRecord.OrderedQuantity,
                            SupplierName = productRecord.Supplier.CompanyName,
                            AddedBy = $"{productRecord.User.FirstName} {productRecord.User.LastName}",
                        };

                        productRecords.Add(singleProductRecord);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}");
            }
            return productRecords;
        }


        public ProductRecord GetByID(int id)
        {
            ProductRecord productRecord = null;
            using (var connection = GetConnection())
            using (var command = new SqlCommand())
            {
                try
                {
                    // Open the database connection
                    connection.Open();
                    command.Connection = connection;

                    // Define the SQL query to fetch data from ProductRecords based on the ProductRecord ID
                    command.CommandText = @"SELECT pr.Id, pr.Barcode, pr.ProductId, p.BrandName, p.Generic, pr.SupplierPrice, 
                                    pr.SellingPrice, pr.OrderedQuantity, pr.ExpDate, pr.ReceivedDate, 
                                    pr.SupplierId, s.CompanyName AS SupplierName, pr.UserId, u.FirstName + ' ' + u.LastName AS AddedBy 
                                    FROM ProductRecords pr
                                    INNER JOIN Products p ON pr.ProductId = p.Id
                                    INNER JOIN Suppliers s ON pr.SupplierId = s.Id
                                    INNER JOIN Users u ON pr.UserId = u.Id
                                    WHERE pr.Id = @id";
                    command.Parameters.Add("@id", SqlDbType.Int).Value = id;

                    // Execute the query and process the results
                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            productRecord = new ProductRecord()
                            {
                                Id = reader.GetInt32(reader.GetOrdinal("Id")),
                                Barcode = reader.IsDBNull(reader.GetOrdinal("Barcode")) ? null : reader.GetString(reader.GetOrdinal("Barcode")),
                                ProductId = reader.GetInt32(reader.GetOrdinal("ProductId")),
                                SupplierPrice = reader.IsDBNull(reader.GetOrdinal("SupplierPrice")) ? 0 : reader.GetFloat(reader.GetOrdinal("SupplierPrice")),
                                SellingPrice = reader.IsDBNull(reader.GetOrdinal("SellingPrice")) ? 0 : reader.GetFloat(reader.GetOrdinal("SellingPrice")),
                                OrderedQuantity = reader.IsDBNull(reader.GetOrdinal("OrderedQuantity")) ? 0 : reader.GetInt32(reader.GetOrdinal("OrderedQuantity")),
                                ExpDate = reader.IsDBNull(reader.GetOrdinal("ExpDate")) ? (DateTime?)null : reader.GetDateTime(reader.GetOrdinal("ExpDate")),
                                ReceivedDate = reader.GetDateTime(reader.GetOrdinal("ReceivedDate")),
                                SupplierId = reader.GetInt32(reader.GetOrdinal("SupplierId")),
                                Supplier = new Supplier
                                {
                                    Id = reader.GetInt32(reader.GetOrdinal("Id")),
                                    CompanyName = reader.IsDBNull(reader.GetOrdinal("SupplierName")) ? null : reader.GetString(reader.GetOrdinal("SupplierName"))
                                },
                                UserId = reader.GetInt32(reader.GetOrdinal("UserId")),
                                User = new User
                                {
                                    Id = reader.GetInt32(reader.GetOrdinal("Id")),
                                    FirstName = reader.IsDBNull(reader.GetOrdinal("AddedBy")) ? null : reader.GetString(reader.GetOrdinal("AddedBy"))
                                },
                                Product = new Product
                                {
                                    Id = reader.GetInt32(reader.GetOrdinal("Id")),
                                    BrandName = reader.IsDBNull(reader.GetOrdinal("BrandName")) ? null : reader.GetString(reader.GetOrdinal("BrandName")),
                                    Generic = reader.IsDBNull(reader.GetOrdinal("Generic")) ? null : reader.GetString(reader.GetOrdinal("Generic"))
                                }
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
            return productRecord;
        }


        public bool Remove(int id)
        {
            try
            {
                using (AppDataContext context = new AppDataContext())
                {
                    var product = context.ProductRecords.FirstOrDefault(x => x.Id.Equals(id));
                    if (product != null)
                    {
                        context.ProductRecords.Remove(product);
                        context.SaveChanges();

                        //substract product current quantities
                        productRepository.EditCurrentQuantityOnly(product.ProductId, -product.OrderedQuantity);
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
            /*
            Barcode = ,
            BrandName = ,
            Generic = ,
            Cost = ,
            Price = ,
            Margin = ,
            MarginPercent = ,
            Stock = ,
            Unit = ,
            ExpDate = ,
            SupplierId = ,
            Location = ,
            OrderedQty = ,
            SoldQty = ,
            ColoredExp = ,
            */
        }
    }
}
