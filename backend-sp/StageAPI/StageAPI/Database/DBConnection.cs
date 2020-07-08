using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace StageAPI.Database
{
    public class DBConnection
    {
        public static MySqlConnection Connection;

        public static void Initialize()
        {
            Connection = new MySqlConnection($"server={Constants.SQL_SERVER}; database={Constants.SQL_DB}; uid={Constants.SQL_USER}; pwd={Constants.SQL_PASS}");
            Connection.Open();
        }
    }
}
