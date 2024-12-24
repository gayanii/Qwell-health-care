using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic.ApplicationServices;
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
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace QWellApp.Repositories
{
    public class ProductRepository : BaseRepository, IProductRepository
    {
        public ISupplierRepository supplierRepository;

        public ProductRepository()
        {
            supplierRepository = new SupplierRepository();
        }

        public bool Add(Product productModel)
        {
            try
            {
                using (AppDataContext context = new AppDataContext())
                {
                    // Check if a product with the same BrandName already exists
                    bool productFound = context.Products.Any(prod => prod.BrandName == productModel.BrandName && prod.Generic == productModel.Generic);

                    if (productFound)
                    {
                        MessageBox.Show("This product is already found. Try a different brand name and generic!");
                        return false;
                    }
                    else
                    {
                        // Create a new product using all fields
                        var newProduct = new Product
                        {
                            BrandName = productModel.BrandName,
                            Generic = productModel.Generic,
                            CurrentQuantity = productModel.CurrentQuantity, 
                            SellingPrice = productModel.SellingPrice,
                            Status = UserStatusEnum.Active.ToString()
                        };

                        // Add the new product to the context and save changes
                        context.Products.Add(newProduct);
                        context.SaveChanges();
                        MessageBox.Show("Product created Successfully!");
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


        public bool Edit(Product productModel)
        {
            try
            {
                using (AppDataContext context = new AppDataContext())
                {
                    // Retrieve the product to be edited based on its ID
                    var product = context.Products.FirstOrDefault(x => x.Id.Equals(productModel.Id));
                    if (product != null)
                    {
                        // Check if there is another product with the same BrandName, excluding the current product
                        bool productFound = context.Products.Any(prod => prod.BrandName == productModel.BrandName && !prod.Id.Equals(productModel.Id));

                        if (productFound)
                        {
                            MessageBox.Show("This brand name is already found. Try a different brand name!");
                            return false;
                        }
                        else
                        {
                            // Update the fields with the values from the product model
                            product.BrandName = productModel.BrandName;
                            product.Generic = productModel.Generic;
                            product.CurrentQuantity = productModel.CurrentQuantity; 
                            product.SellingPrice = productModel.SellingPrice;
                            product.Status = productModel.Status;

                            // Mark the product as modified and save changes
                            context.Entry(product).State = EntityState.Modified;
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

        public bool EditCurrentQuantityOnly(int id, int quantity)
        {
            try
            {
                using (AppDataContext context = new AppDataContext())
                {
                    // Retrieve the product to be edited based on its ID
                    var product = context.Products.FirstOrDefault(x => x.Id.Equals(id));
                    if (product != null)
                    {
                        product.CurrentQuantity = product.CurrentQuantity + quantity;

                        // Mark the product as modified and save changes
                        context.Entry(product).State = EntityState.Modified;
                        context.SaveChanges();
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


        public IEnumerable<ProductView> GetAll(string searchWord)
        {
            List<ProductView> products = new List<ProductView>();
            try
            {
                using (AppDataContext context = new AppDataContext())
                {
                    string normalizedSearchWord = searchWord.Trim().ToLower();

                    // Retrieve the list of products that match the search criteria based on relevant fields
                    var productList = context.Products
                        .Where(p => p.BrandName.ToLower().Contains(normalizedSearchWord) || p.Generic.ToLower().Contains(normalizedSearchWord) || p.Status.ToLower().Contains(normalizedSearchWord) ||
                        p.CurrentQuantity.ToString().Contains(normalizedSearchWord) || p.SellingPrice.ToString().Contains(normalizedSearchWord))
                        .OrderBy(p => p.BrandName) // Sort by brand name in descending order. recent up
                        .ToList();

                    // Convert each product in the filtered list to the ProductView object
                    foreach (var product in productList)
                    {
                        ProductView singleProduct = new ProductView()
                        {
                            Id = product.Id,
                            BrandName = product.BrandName,
                            Generic = product.Generic,
                            CurrentQuantity = product.CurrentQuantity,
                            SellingPrice = product.SellingPrice,
                            Status = product.Status,
                        };
                        products.Add(singleProduct);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}");
            }
            return products;
        }


        public Product GetByID(int id)
        {
            Product product = null;
            using (var connection = GetConnection())
            using (var command = new SqlCommand())
            {
                try
                {
                    connection.Open(); // Open the connection only once
                    command.Connection = connection;
                    command.CommandText = "SELECT * FROM [Products] WHERE Id = @id";
                    command.Parameters.Add("@id", SqlDbType.Int).Value = id; // Use SqlDbType.Int for ProductId

                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            product = new Product()
                            {
                                // Populate fields using the reader and handle DBNull
                                Id = reader.GetInt32(reader.GetOrdinal("Id")),
                                BrandName = reader.IsDBNull(reader.GetOrdinal("BrandName")) ? null : reader.GetString(reader.GetOrdinal("BrandName")),
                                Generic = reader.IsDBNull(reader.GetOrdinal("Generic")) ? null : reader.GetString(reader.GetOrdinal("Generic")),
                                CurrentQuantity = reader.IsDBNull(reader.GetOrdinal("CurrentQuantity")) ? 0 : reader.GetInt32(reader.GetOrdinal("CurrentQuantity")),
                                SellingPrice = reader.IsDBNull(reader.GetOrdinal("SellingPrice")) ? 0 : reader.GetFloat(reader.GetOrdinal("SellingPrice")),
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
            return product;
        }


        public bool Remove(int id)
        {
            try
            {
                using (AppDataContext context = new AppDataContext())
                {
                    var product = context.Products.FirstOrDefault(x => x.Id.Equals(id));
                    if (product != null)
                    {
                        product.Status = UserStatusEnum.Inactive.ToString();
                        context.Entry(product).State = EntityState.Modified;
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
