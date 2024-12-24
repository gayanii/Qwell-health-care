using QWellApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QWellApp.Repositories
{
    public interface IStockRepository
    {
        Task<IEnumerable<Stock>> GetStocks(DateTime startDate, DateTime endDate);
    }
}
