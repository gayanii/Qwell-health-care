using Microsoft.Data.SqlClient;
using QWellApp.DBConnection;
using QWellApp.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace QWellApp.Repositories
{
    class RoleRepository : BaseRepository, IRoleRepository
    {
        public IEnumerable<Role> GetAll()
        {
            List<Role> positions = new List<Role>();
            try
            {
                using (AppDataContext context = new AppDataContext())
                {
                    positions = context.Roles.ToList();
                }
            } catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}");
            }
            
            return positions;
        }

        public Role GetByID(int id)
        {
            Role position = null;
            using (var connection = GetConnection())
            using (var command = new SqlCommand())
            {
                try
                {
                    GetConnection().Open();
                    connection.Open();
                    command.Connection = connection;
                    command.CommandText = "select * from [Roles] where id=@id";
                    command.Parameters.Add("@id", SqlDbType.VarChar).Value = id;
                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            position = new Role()
                            {
                                RoleName = (reader.IsDBNull("RoleName")) ? null : reader.GetString(reader.GetOrdinal("RoleName"))
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
            return position;
        }
    }
}
