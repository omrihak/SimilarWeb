using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimilarWebAPI.Manager
{
    public class SqlServerConnectionHelper
    {  
        //Azure sql server
        //public static SqlConnection getConnection()
        //{
        //    string connectionString = "Data Source=omrihakimi.database.windows.net;Persist Security Info=True;User ID=omrih;Password=abc1234!;Database=Hakimi";
        //    SqlConnection conn = new SqlConnection(connectionString);
        //    return conn;
        //}

        // local sql server
        public static SqlConnection getConnection()
        {
            string connectionString = "Data Source=.\\SQLEXPRESS;Persist Security Info=True;User ID=omrih;Password=abcd1234;Database=SimilarWeb";
            SqlConnection conn = new SqlConnection(connectionString);
            return conn;
        }
    }
}
