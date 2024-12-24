using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic.ApplicationServices;
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
    public class StockRepository : BaseRepository, IStockRepository
    {
        public IProductMedicalRecordRepository productMedicalRecordRepository;

        public StockRepository()
        {
            productMedicalRecordRepository = new ProductMedicalRecordRepository();
        }

        public async Task<IEnumerable<Stock>> GetStocks(DateTime startDate, DateTime endDate)
        {
            try
            {
                using (AppDataContext context = new AppDataContext())
                {
                    // Calculate the start and end dates based on the summary date
                    DateTime StockStartDate = startDate.AddHours(7);  // 10/20/2024 7:00:00 AM
                    DateTime StockEndDate = endDate.AddDays(1).AddHours(6).AddMinutes(59); // 10/21/2024 6:59:00 AM

                    List<ProductRecord> productRecords = context.ProductRecords
                        .Include(x => x.Product)
                        .Where(x => x.ReceivedDate >= StockStartDate && x.ReceivedDate < StockEndDate)
                        .OrderByDescending(p => p.Id) // Sort by id in descending order. recent up
                        .ToList();
                    List<ProductMedicalRecord> productMedicalRecords = context.ProductMedicalRecords
                        .Include(x => x.Product)
                        .Where(x => x.AdmitDate >= StockStartDate && x.AdmitDate < StockEndDate)
                        .ToList();
                    List<Stock> StockList = new List<Stock>();
                    var count = 1;

                    // Process each product record
                    foreach (var productRecord in productRecords)
                    {
                        var existingStock= StockList.FirstOrDefault(c => c.ProductId == productRecord.ProductId);
                        int collectedItems = productRecord.OrderedQuantity;
                        // Filter records that match the ProductId condition
                        var matchingRecords = productMedicalRecords.Where(x => x.ProductId == productRecord.ProductId);
                        // Sum up the 'Units' property for the filtered records
                        int totalUnits = matchingRecords.Sum(x => x.Units);

                        if (existingStock != null)
                        {
                            existingStock.CollectedStock += collectedItems;
                            existingStock.Balance = existingStock.CollectedStock - totalUnits;
                        }
                        else
                        {
                            StockList.Add(new Stock
                            {
                                Id = count,
                                ProductId = productRecord.Product.Id,
                                BrandName = productRecord.Product.BrandName,
                                Generic = productRecord.Product.Generic,
                                CollectedStock = productRecord.OrderedQuantity,
                                SoldStock = totalUnits,
                                Balance = productRecord.OrderedQuantity - totalUnits
                            });
                            count++;
                        }
                    }


                    return StockList;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}");
                throw;
            }
        }
    }
}
