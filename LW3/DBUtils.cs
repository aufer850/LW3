using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

namespace LW3
{
    class DBUtils
    {
        public static MySqlConnection GetDBConnection()
        {
            return DBMySQLUtils.GetDbConnection("26.145.209.152", 3306, "milkfactory","lab","lab_pass");
        }
    }
}
