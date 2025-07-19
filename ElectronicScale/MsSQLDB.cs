using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Windows.Forms;

namespace ElectronicScale
{
    public class MsSQLDB
    {
        public static SqlConnection connection;

        public MsSQLDB(string connectionString = "")
        {
            if (connectionString != "")
            {
                ConnectDb(connectionString);
            }
            else
            {
                ConnectDb();
            }
        }

        public SqlDataReader ExecuteReader(string sql)
        {

            try
            {
                using SqlCommand command = new SqlCommand(sql, connection);
                SqlDataReader reader = command.ExecuteReader();
                return reader;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"执行MSSQL操作时失败\r\n{ex.Message}", "错误提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                throw new Exception($"执行MSSQL操作时失败\r\n{ex.Message}");
            }
        }
        public bool InsertData(string sql)
        {
            try
            {
                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    if (connection.State != ConnectionState.Open)
                    {
                        connection.Open();
                    }
                    command.ExecuteNonQuery();
                    return true;
                };
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                throw new Exception($"执行MSSQL写入操作时失败\r\n{ex.Message}");
            }

        }

        public int InsertDataReturnID(string sql)
        {
            using SqlCommand command = new($"{sql};select scope_identity();", connection);
            return Convert.ToInt32(command.ExecuteScalar());
        }
        public bool MssqlIsConnected()
        {
            if (connection == null)
            {
                ConnectDb(); return true;
            }
            else { return true; }
        }

        private void ConnectDb(string connectionString = "")
        {
            try
            {
                if (connectionString != "")
                {
                    connection = new SqlConnection(connectionString);
                }
                else
                {
                    connection = new SqlConnection($"Server={Form1.ConfigInfo["mssql"]["host"]};Database={Form1.ConfigInfo["mssql"]["database"]};" +
                        $"User Id={Form1.ConfigInfo["mssql"]["user"]};Password={Form1.ConfigInfo["mssql"]["password"]};Trusted_Connection=True;integrated security=False");
                }
                connection.Open();
            }
            catch (Exception ex) { throw new Exception($"连接MSSQL出错\r\n{ex.Message}"); }
        }

        public object ExecuteScalar(string sql)
        {
            try
            {
                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    if (connection.State != ConnectionState.Open)
                    {
                        connection.Open();
                    }
                    return command.ExecuteScalar();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"执行MSSQL操作时失败\r\n{ex.Message}", "错误提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                throw new Exception($"执行MSSQL操作时失败\r\n{ex.Message}");
            }
        }

        public object ExecuteScalar(string sql, params SqlParameter[] parameters)
        {
            try
            {
                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    if (connection.State != ConnectionState.Open)
                    {
                        connection.Open();
                    }
                    if (parameters != null && parameters.Length > 0)
                    {
                        command.Parameters.AddRange(parameters);
                    }
                    return command.ExecuteScalar();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"执行MSSQL操作时失败\r\n{ex.Message}", "错误提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                throw new Exception($"执行MSSQL操作时失败\r\n{ex.Message}");
            }
        }
    }
}
