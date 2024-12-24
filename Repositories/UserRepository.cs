using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client.Platforms.Features.DesktopOs.Kerberos;
using QWellApp.DBConnection;
using QWellApp.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ListView;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;
using QWellApp.Views.UserControls;
using QWellApp.ViewModels;
using System.Windows.Media.Animation;
using System.Linq.Expressions;
using System.Configuration;
using System.Numerics;
using System.Threading;
using QWellApp.Helpers;
using QWellApp.Enums;

namespace QWellApp.Repositories
{
    public class UserRepository : BaseRepository, IUserRepository
    {
        public IRoleRepository roleRepository;

        public UserRepository()
        {
            roleRepository = new RoleRepository();
        }
        public bool Add(UserDetails userModel)
        {
            try
            {
                using (var context = new AppDataContext())
                {
                    if (context.Users.Any(user => user.Username == userModel.Username))
                    {
                        MessageBox.Show("This username is already in use. Try a different username.");
                        return false;
                    }

                    var salt = PasswordHelper.GenerateSalt();
                    var passwordHash = PasswordHelper.HashPassword(userModel.Password, salt);

                    var newUser = new User
                    {
                        FirstName = userModel.FirstName,
                        LastName = userModel.LastName,
                        Username = userModel.Username,
                        Email = userModel.Email,
                        NIC = userModel.NIC,
                        Gender = userModel.Gender,
                        MobileNum = userModel.MobileNum,
                        TelephoneNum = userModel.TelephoneNum,
                        EmployeeType = userModel.EmployeeType,
                        RoleId = userModel.RoleId,
                        PasswordHash = passwordHash,
                        PasswordSalt = salt,
                        Status = UserStatusEnum.Active.ToString()
                    };

                    context.Users.Add(newUser);
                    context.SaveChanges();
                    MessageBox.Show("User created successfully!");
                    return true;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}");
                return false;
            }
        }

        public bool AuthenticateUser(NetworkCredential credential)
        {
            try
            {
                using (var context = new AppDataContext())
                {
                    //UserDetails newUser = new UserDetails
                    //{
                    //    FirstName = "Gayani",
                    //    LastName = "Silva",
                    //    Username = "gaya",
                    //    Email = "",
                    //    NIC = "985931402v",
                    //    Gender = "Female",
                    //    MobileNum = "0773582898",
                    //    TelephoneNum = "0773582898",
                    //    EmployeeType = "Admin",
                    //    RoleId = 1,
                    //    Password = "123",
                    //    Status = "Active"
                    //};
                    //Add(newUser);

                    var user = context.Users.FirstOrDefault(u => u.Username == credential.UserName && u.Status == UserStatusEnum.Active.ToString());
                    if (user == null) return false;

                    var hashedPassword = PasswordHelper.HashPassword(credential.Password, user.PasswordSalt);
                    return hashedPassword == user.PasswordHash;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}");
                return false;
            }
        }

        public bool Edit(UserDetails userModel)
        {
            try
            {
                using (AppDataContext context = new AppDataContext())
                {
                    var user = context.Users.FirstOrDefault(x => x.Id.Equals(userModel.Id));
                    var usernamefound = context.Users.Any(x => x.Username.Equals(userModel.Username) && !x.Id.Equals(userModel.Id));
                    if (user != null)
                    {
                        if (!usernamefound)
                        {
                            user.FirstName = userModel.FirstName;
                            user.LastName = userModel.LastName;
                            user.Email = userModel.Email;
                            user.MobileNum = userModel.MobileNum;
                            user.TelephoneNum = userModel.TelephoneNum;
                            user.NIC = userModel.NIC;
                            user.EmployeeType = userModel.EmployeeType;
                            user.Username = userModel.Username;
                            user.RoleId = userModel.RoleId;
                            user.Gender = userModel.Gender;
                            user.Status = userModel.Status;

                            context.Entry(user).State = EntityState.Modified;
                            context.SaveChanges();
                            MessageBox.Show("Updated Successfully!");
                            return true;
                        } else
                        {
                            MessageBox.Show("Username already found");
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

        public IEnumerable<UserView> GetAll(string searchWord)
        {
            List<UserView> users = new List<UserView>();
            try
            {
                using (AppDataContext context = new AppDataContext())
                {
                    // Normalize the search term for case-insensitive search
                    string normalizedSearchWord = searchWord.Trim().ToLower();

                    var userList = context.Users
                        .Where(p => p.FirstName.ToLower().Contains(normalizedSearchWord) || p.LastName.ToLower().Contains(normalizedSearchWord) || 
                        p.Email.ToLower().Contains(normalizedSearchWord) || p.MobileNum.Contains(normalizedSearchWord) || p.NIC.Contains(normalizedSearchWord) ||
                        p.TelephoneNum.Contains(normalizedSearchWord) || p.Status.ToLower().Contains(normalizedSearchWord) || p.Role.RoleName.ToLower().Contains(normalizedSearchWord) ||
                        p.EmployeeType.ToLower().Contains(normalizedSearchWord) || p.Username.ToLower().Contains(normalizedSearchWord))
                        .OrderBy(p => p.FirstName) // Sort FirstName id in descending order. recent up
                        .ToList();
                    foreach (var user in userList)
                    {
                        UserView singleUser = new UserView()
                        {
                            Id = user.Id,
                            FirstName = user.FirstName,
                            LastName = user.LastName,
                            Status = user.Status,
                            MobileNum = user.MobileNum,
                            EmployeeType = user.EmployeeType,
                            Role = roleRepository.GetByID(user.RoleId).RoleName
                        };
                        users.Add(singleUser);
                    }
                }
            } catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}");
            }
            return users;
        }

        public IEnumerable<UserView> GetAllDoctors()
        {
            List<UserView> doctors = new List<UserView>();
            try
            {
                using (AppDataContext context = new AppDataContext())
                {
                    var doctorList = context.Users
                        .Where(p => p.RoleId.Equals(1) && p.Status.Equals(UserStatusEnum.Active.ToString()))
                        .OrderBy(p => p.FirstName) // Sort by FirstName in descending order. recent up
                        .ToList();
                    foreach (var doctor in doctorList)
                    {
                        UserView singleDoctor = new UserView()
                        {
                            Id = doctor.Id,
                            FirstName = doctor.FirstName,
                            LastName = doctor.LastName,
                            Status = doctor.Status,
                            MobileNum = doctor.MobileNum,
                            EmployeeType = doctor.EmployeeType,
                            Role = roleRepository.GetByID(doctor.RoleId).RoleName
                        };
                        doctors.Add(singleDoctor);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}");
            }
            return doctors;
        }

        public IEnumerable<UserView> GetAllNurses()
        {
            List<UserView> doctors = new List<UserView>();
            try
            {
                using (AppDataContext context = new AppDataContext())
                {
                    var nurseList = context.Users
                        .Where(p => new[] { 2, 3, 4, 5, 6, 7, 8 }.Contains(p.RoleId) && p.Status.Equals(UserStatusEnum.Active.ToString()))
                        .OrderBy(p => p.FirstName) // Sort by FirstName in descending order. recent up
                        .ToList();
                    foreach (var nurse in nurseList)
                    {
                        UserView singleNurse = new UserView()
                        {
                            Id = nurse.Id,
                            FirstName = nurse.FirstName,
                            LastName = nurse.LastName,
                            Status = nurse.Status,
                            MobileNum = nurse.MobileNum,
                            EmployeeType = nurse.EmployeeType,
                            Role = roleRepository.GetByID(nurse.RoleId).RoleName
                        };
                        doctors.Add(singleNurse);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}");
            }
            return doctors;
        }

        public UserDetails GetByID(int id)
        {
            UserDetails user = null;
            using (var connection = GetConnection())
            using (var command = new SqlCommand())
            {
                try
                {
                    GetConnection().Open();
                    connection.Open();
                    command.Connection = connection;
                    command.CommandText = "select * from [Users] where Id=@id";
                    command.Parameters.Add("@id", SqlDbType.VarChar).Value = id;
                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            user = new UserDetails()
                            {
                                Id = reader.GetInt32(reader.GetOrdinal("Id")),
                                FirstName = reader.IsDBNull(reader.GetOrdinal("FirstName")) ? null : reader.GetString(reader.GetOrdinal("FirstName")),
                                LastName = reader.IsDBNull(reader.GetOrdinal("LastName")) ? null : reader.GetString(reader.GetOrdinal("LastName")),
                                Email = reader.IsDBNull(reader.GetOrdinal("Email")) ? null : reader.GetString(reader.GetOrdinal("Email")),
                                MobileNum = reader.IsDBNull(reader.GetOrdinal("MobileNum")) ? null : reader.GetString(reader.GetOrdinal("MobileNum")),
                                TelephoneNum = reader.IsDBNull(reader.GetOrdinal("TelephoneNum")) ? null : reader.GetString(reader.GetOrdinal("TelephoneNum")),
                                Gender = reader.IsDBNull(reader.GetOrdinal("Gender")) ? null : reader.GetString(reader.GetOrdinal("Gender")),
                                NIC = reader.IsDBNull(reader.GetOrdinal("NIC")) ? null : reader.GetString(reader.GetOrdinal("NIC")),
                                EmployeeType = reader.IsDBNull(reader.GetOrdinal("EmployeeType")) ? null : reader.GetString(reader.GetOrdinal("EmployeeType")),
                                RoleId = reader.IsDBNull(reader.GetOrdinal("RoleId")) ? 3 : reader.GetInt32(reader.GetOrdinal("RoleId")),
                                Username = reader.IsDBNull(reader.GetOrdinal("Username")) ? null : reader.GetString(reader.GetOrdinal("Username")),
                                Password = string.Empty,  // Password is not retrieved from the database; set to an empty string
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
            return user;
        }

        public UserDetails GetByUsername(string username)
        {
            UserDetails user = null;
            using (var connection = GetConnection())
            using (var command = new SqlCommand())
            {
                try
                {
                    GetConnection().Open();
                    connection.Open();
                    command.Connection = connection;
                    command.CommandText = "select * from [Users] where username=@username";
                    command.Parameters.Add("@username", SqlDbType.VarChar).Value = username;
                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            user = new UserDetails()
                            {
                                Id = reader.GetInt32(reader.GetOrdinal("Id")),
                                FirstName = reader.IsDBNull(reader.GetOrdinal("FirstName")) ? null : reader.GetString(reader.GetOrdinal("FirstName")),
                                LastName = reader.IsDBNull(reader.GetOrdinal("LastName")) ? null : reader.GetString(reader.GetOrdinal("LastName")),
                                Email = reader.IsDBNull(reader.GetOrdinal("Email")) ? null : reader.GetString(reader.GetOrdinal("Email")),
                                MobileNum = reader.IsDBNull(reader.GetOrdinal("MobileNum")) ? null : reader.GetString(reader.GetOrdinal("MobileNum")),
                                TelephoneNum = reader.IsDBNull(reader.GetOrdinal("TelephoneNum")) ? null : reader.GetString(reader.GetOrdinal("TelephoneNum")),
                                Gender = reader.IsDBNull(reader.GetOrdinal("Gender")) ? null : reader.GetString(reader.GetOrdinal("Gender")),
                                NIC = reader.IsDBNull(reader.GetOrdinal("NIC")) ? null : reader.GetString(reader.GetOrdinal("NIC")),
                                EmployeeType = reader.IsDBNull(reader.GetOrdinal("EmployeeType")) ? null : reader.GetString(reader.GetOrdinal("EmployeeType")),
                                RoleId = reader.IsDBNull(reader.GetOrdinal("RoleId")) ? 3 : reader.GetInt32(reader.GetOrdinal("RoleId")),
                                Username = reader.IsDBNull(reader.GetOrdinal("Username")) ? null : reader.GetString(reader.GetOrdinal("Username")),
                                Password = string.Empty,  // Password is not retrieved from the database; set to an empty string
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
            return user;
        }

        public bool Remove(int id)
        {
            try
            {
                using (AppDataContext context = new AppDataContext())
                {
                    var user = context.Users.FirstOrDefault(x => x.Id.Equals(id));
                    var currentUser = Properties.Settings.Default.Username;
                    if (user != null && user.Username != currentUser)
                    {
                        user.Status = UserStatusEnum.Inactive.ToString();
                        context.Entry(user).State = EntityState.Modified;
                        context.SaveChanges();
                        MessageBox.Show("Status Changed Successfully!");
                        return true;
                    } else
                    {
                        MessageBox.Show("Failed to Delete!");
                        return false;
                    }
                }
            } catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}");
                return false;
            }
        }
    }
}
