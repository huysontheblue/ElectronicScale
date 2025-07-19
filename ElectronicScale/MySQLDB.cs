using MySql.Data.MySqlClient;

namespace ElectronicScale
{
    public class MySQLDB
    {
        string connectionString = $"server={Form1.ConfigInfo["mysql"]["host"]};user={Form1.ConfigInfo["mysql"]["user"]};" +
            $"port={Form1.ConfigInfo["mysql"]["port"]};database={Form1.ConfigInfo["mysql"]["database"]};password={Form1.ConfigInfo["mysql"]["assword"]}";
        public MySqlConnection mysqlConnction;
        public bool IsConnected()
        {
            if (mysqlConnction == null)
                mysqlConnction = new MySqlConnection(connectionString);
            try
            {
                mysqlConnction.Open();
                return true;
            }
            catch (Exception e) { return false; }

        }

        public string GetLastId()
        {
            if (IsConnected())
            {
                string sql = "select max(id) from xx;";
                MySqlCommand cmd = new(sql, mysqlConnction);
                using MySqlDataReader reader = cmd.ExecuteReader();
            }
            return "a";
        }

        public bool InsertData(string sql)
        {
            if (sql == "") return false;
            try
            {
                using (MySqlConnection a = new MySqlConnection(connectionString))
                {
                    a.Open();
                    MySqlCommand command = new(sql, a);
                    MySqlDataReader reader = command.ExecuteReader();
                    reader.Close();
                    a.Close();
                    a.Dispose();
                    return true;

                };

            }
            catch (Exception ex) { throw new Exception($"执行MYSQL操作时失败\r\n{ex.Message}"); }
        }

    }
}

