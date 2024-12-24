using QWellApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace QWellApp.Repositories
{
    public interface IProductRecordRepository
    {
        bool Add(ProductRecord productModel);
        bool Edit(ProductRecord productModel);
        bool Remove(int id);
        ProductRecord GetByID(int id);
        IEnumerable<ProductRecordView> GetAll(string searchWord);
    }
}
