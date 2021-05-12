using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DapperExample.Database
{
    public static class Db
    {
        //Connection Dönen Metot
        public static SqlConnection GetConnection()
        {
            //SQL Client Instance 'ı alındı
            var con = new SqlConnection(); //MSSQL Connection
            con.ConnectionString = @"Server=(localdb)\MSSQLLocalDB;Database=DapperDb;Trusted_Connection=true;";

            return con;
        }
    }
}
