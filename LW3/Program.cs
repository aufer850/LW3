using MySql.Data.MySqlClient;
using Mysqlx.Expr;
using System.Data.Common;

namespace LW3
{
    internal class Program
    {
        private static void PrintDatabase(MySqlConnection conn)
        {
            string id, productname, orderamount;
            string sqlcomm = "select order_id,productname,orderamount from milkfactory.order";
            MySqlCommand cmd = new MySqlCommand();
            cmd.Connection = conn;
            cmd.CommandText = sqlcomm;
            using (DbDataReader reader = cmd.ExecuteReader())
            {
                if (!reader.HasRows) return;
                while (reader.Read())
                {
                    id = reader["order_id"].ToString();
                    productname = reader["productname"].ToString();
                    orderamount = reader["orderamount"].ToString();
                    Console.WriteLine("Id:{0}; Product:{1}; amount:{2};",id,productname,orderamount);
                }
            }
        }

        private static void CreateOrder(MySqlConnection conn)
        {
            string startdate = "0", productname = "0";
            int clientcode = 0, productcode = 0, orderamount = 0;
            List<string> strings = new List<string> { "startdate", "productcode", "clientcode", "productname", "orderamount" };
            for(int i = 0; i < strings.Count; i++ )
            {
                Console.WriteLine("Enter " + strings[i]);
                string Text = Console.ReadLine();
                switch(i)
                {
                    case 0: startdate = Text;              break;
                    case 1: productcode = int.Parse(Text); break;
                    case 2: clientcode = int.Parse(Text);  break;
                    case 3: productname = Text;            break;
                    case 5: orderamount = int.Parse(Text); break;
                }
            }
            string sqlcomm = string.Format("insert into milkfactory.order(startdate,clientcode,productname,productcode,orderamount,delivered,deliverydate) values(\"{0}\",{1},\"{2}\",{3},{4},0,\"{0}\")", startdate,clientcode,productname,productcode,orderamount);
            MySqlCommand cmd = new MySqlCommand();
            cmd.Connection = conn;
            cmd.CommandText = sqlcomm;
            DbDataReader reader = cmd.ExecuteReader();
        }

        private static void DeleteOrder(MySqlConnection conn)
        {
            int idtodelete = 0;
            Console.WriteLine("Enter ID of the order: ");
            idtodelete = GetNumber();
            string sqlcomm = string.Format("delete from milkfactory.order where milkfactory.order.order_id = " + idtodelete);
            MySqlCommand cmd = new MySqlCommand();
            cmd.Connection = conn;
            cmd.CommandText = sqlcomm;
            DbDataReader reader = cmd.ExecuteReader();
            Console.WriteLine("Succesfully deleted order #" + idtodelete);
        }

        private static void ChangeOrder(MySqlConnection conn)
        {
            int idtodelete = 0;
            Console.WriteLine("Enter ID of the order: ");
            idtodelete = GetNumber();
            List<string> VarList = new List<string>{"startdate", "clientcode", "productname", "productcode", "orderamount","delivered","deliverydate"};
            
            for (int i = 0; i < VarList.Count; i++)
            {
                Console.WriteLine("Enter " + VarList[i]);
                string Text = Console.ReadLine();
                if (string.Equals(Text, "") == true) { continue; } // якщо пусто, залишаємо так само
                if (int.TryParse(Text, out int n) == true) { VarList[i] = Text; } // якщо число, пишимо без змін
                else { VarList[i] = string.Format("\"{0}\"", Text); } // якщо текст, додаємо лапки для розуміння SQL
            }

            // дуже велика команда
            string sqlcomm = string.Format("update milkfactory.order set startdate ={0},clientcode ={1},productname ={2},productcode = {3}," +
                "orderamount = {4},delivered = {5},deliverydate = {6} where order_id = {7}", 
                VarList[0], VarList[1], VarList[2], VarList[3], VarList[4], VarList[5], VarList[6], idtodelete);
            //
            Console.WriteLine(sqlcomm);
            MySqlCommand cmd = new MySqlCommand();
            cmd.Connection = conn;
            cmd.CommandText = sqlcomm;
            DbDataReader reader = cmd.ExecuteReader();
            Console.WriteLine("Succesfully changed order #" + idtodelete);
        }


        static int GetNumber()
        { 
            while (true)
            {
                try
                {
                    return Convert.ToInt32(Console.ReadLine());
                }
                catch { Console.WriteLine("Somthing went wrong! try again"); }
            
            }
        
        }

        static void Main(string[] args)
        {
            Console.WriteLine("Getting connection!");
            MySqlConnection conn = DBUtils.GetDBConnection();
            try
            {  
                while (true)
                {
                    Console.WriteLine("Opening connection...");
                    conn.Open();
                    Console.WriteLine("Connection success!");

                    Console.Clear();
                    Console.WriteLine("1 - add order \n2 - change order \n3 - delete order \n0 - close");
                    int num = GetNumber();
                    switch(num)
                    {
                        case 0: Environment.Exit(0); break; 
                        case 1: CreateOrder(conn); break;
                        case 2: ChangeOrder(conn); break;
                        case 3: DeleteOrder(conn); break;
                        default: { Console.WriteLine("Wrong number!"); Console.ReadLine(); continue; }; break;
                    }
                    conn.Close();
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine("Something went wrong: " + ex.ToString());
            }
            finally
            {
                conn.Close();
                Console.WriteLine("Program ended, press any button to exit!");
                Console.ReadLine();
            }
        }
    }
}
