using QWellApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace QWellApp.Repositories
{
    public interface ILabTestRepository
    {
        bool Add(LabTest labTestModel);
        bool Edit(LabTest labTestModel);
        bool Remove(int id);
        LabTest GetByID(int id);
        IEnumerable<LabTestView> GetAll(string searchWord);
    }
}
