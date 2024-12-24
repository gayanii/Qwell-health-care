using QWellApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace QWellApp.Repositories
{
    public interface IProductRepository
    {
        bool Add(Product productModel);
        bool Edit(Product productModel);
        bool EditCurrentQuantityOnly(int id, int quantity);
        bool Remove(int id);
        Product GetByID(int id);
        IEnumerable<ProductView> GetAll(string searchWord);
    }
}
