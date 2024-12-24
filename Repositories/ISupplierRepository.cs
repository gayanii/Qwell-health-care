using QWellApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QWellApp.Repositories
{
    public interface ISupplierRepository
    {
        bool Add(Supplier supplierModel);
        bool Edit(Supplier supplierModel);
        bool Remove(int id);
        Supplier GetByID(int id);
        IEnumerable<SupplierView> GetAll(string searchWord);
    }
}
