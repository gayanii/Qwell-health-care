using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QWellApp.Repositories
{
    public abstract class BaseRepository
    {
        private readonly string DefaultConnectionString;

        public BaseRepository()
        {
            //DefaultConnectionString = @"Data Source=DESKTOP-KKFTKU6\MSSQLLOCALDB;Initial Catalog=Qwell;Integrated Security=True;TrustServerCertificate=True;Max Pool Size=1000;";
            DefaultConnectionString = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=Qwell;Integrated Security=True;TrustServerCertificate=True;Max Pool Size=1000;";
            //DefaultConnectionString = @"Data Source=tcp:qwellappdbserver.database.windows.net,1433;Initial Catalog=Qwell;User Id=CloudSAf50139bf@qwellappdbserver;Password=Gayani@321";
        }

        protected SqlConnection GetConnection()
        {
            return new SqlConnection(DefaultConnectionString);
        }
    }
}
