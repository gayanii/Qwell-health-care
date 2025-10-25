using QWellApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace QWellApp.Repositories
{
    public interface IUserRepository
    {
        bool AuthenticateUser(NetworkCredential credential);
        bool ChangePassword(NetworkCredential changePasswordModel);
        bool Add(UserDetails userModel);
        bool Edit(UserDetails userModel);
        bool Remove(int id);
        UserDetails GetByID(int id);
        UserDetails GetByUsername(string username);
        IEnumerable<UserView> GetAll(string searchWord);
        IEnumerable<UserView> GetAllDoctors();
        IEnumerable<UserView> GetAllNurses();
    }
}
