using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

namespace LW3
{
    internal class DBMySQLUtils
    {
        public static MySqlConnection 
        GetDbConnection(string host, int port, string database,string username,string password)
        {
            string Conn = "Server=" + host + ";Database=" + database + ";port=" + port.ToString() + ";User Id =" + username + ";password=" + password;
            return new MySqlConnection(Conn);
        }
    }
}
