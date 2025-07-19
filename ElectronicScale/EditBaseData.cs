using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ElectronicScale
{
    public partial class EditBaseData : Form
    {
        public EditBaseData()
        {
            InitializeComponent();
            LoadData();
        }

        private void EditBaseData_Load(object sender, EventArgs e)
        {

        }
        // 连接字符串
        string connectionString = $"Server={Form1.ConfigInfo["mssql"]["host"]};Database={Form1.ConfigInfo["mssql"]["database"]}; User Id={Form1.ConfigInfo["mssql"]["user"]};Password={Form1.ConfigInfo["mssql"]["password"]};Trusted_Connection=True;integrated security=False";

        Dictionary<string, string> title = new Dictionary<string, string>()
            {
                {"id","ID" },
                {"apn","APN" },
                {"project","专案" },     //Dự án
                {"color","颜色" },
                {"spec","规格/型号" },
                {"lag","外购件" },
                {"num","标准件装量" },
                {"upper","称重上限" },    //Giới hạn trên
                {"lower","称重下限" },    //Giới hạn dưới   
                {"standard","标准重量" }, //Trọng lượng chuẩn
                {"customer","出货客户" }, 
                {"mes_code","MES料号" },
                {"nw","净重(外箱标识)" },
                {"gw","毛重(外箱标识)" },
                {"range","取工程代码参考" },
                {"tag_product_name","称重标签品名" },
                {"eeee","工程代码" },
            };
        // 查询数据库并设置数据源
        private void LoadData()
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                SqlDataAdapter adapter = new SqlDataAdapter("SELECT * FROM packing_info order by id desc", conn);
                //SqlDataAdapter adapter = new SqlDataAdapter("select apn, spec, lag, num, upper, lower, standard, eeee, tag_product_name FROM packing_info order by id desc", conn);
                DataTable dataTable = new DataTable();
                adapter.Fill(dataTable);
                dataGridView1.DataSource = dataTable;
            }
            //foreach (var i in title)
            //{
            //    if (i.Key != "") dataGridView1.Columns[i.Key].HeaderText = i.Value;
            //}

            if (dataGridView1.Columns != null)
            {
                foreach (var i in title)
                {
                    if (!string.IsNullOrEmpty(i.Key) && dataGridView1.Columns.Contains(i.Key))
                    {
                        dataGridView1.Columns[i.Key].HeaderText = i.Value;
                    }
                }
            }
        }

        // 添加数据
        private void AddData(object sender, EventArgs e)
        {
            // 将更改保存到数据库
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    SqlCommand command = new SqlCommand("INSERT INTO packing_info (apn) VALUES ('');select scope_identity() as newID", conn);
                    conn.Open();
                    var sqlData = command.ExecuteScalar();
                    DataRow row = ((DataTable)dataGridView1.DataSource).NewRow();
                    row["id"] = sqlData;
                    ((DataTable)dataGridView1.DataSource).Rows.InsertAt(row, 0);
                }
            }
            catch (Exception ex) { MessageBox.Show(this, $"新增数据时出错:\r\n{ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error); }
        }

        // 删除数据
        private void DeleteData(object sender, EventArgs e)
        {
            try
            {
                // 假设已经选中DataGridView中的行
                DataGridViewRow i = dataGridView1.CurrentRow;
                DataRow row = ((DataTable)dataGridView1.DataSource).Rows[i.Index];
                // 将更改保存到数据库
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    SqlCommand command = new SqlCommand($"DELETE FROM packing_info WHERE id = @id", conn);
                    command.Parameters.AddWithValue("@id", row.ItemArray[0]);
                    conn.Open();
                    command.ExecuteNonQuery();
                    //row.Delete();
                    ((DataTable)dataGridView1.DataSource).Rows.RemoveAt(i.Index);
                }
                Form1.GetBaseInfoByDB(); //更新前台数据
            }
            catch (Exception ex) { MessageBox.Show(this, $"删除数据时出错:\r\n{ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error); }
        }

        // 更新数据
        private void UpdateData(object sender, DataGridViewCellEventArgs e)
        {
            // 假设已经在DataGridView中编辑数据
            DataRow row = ((DataTable)dataGridView1.DataSource).Rows[e.RowIndex];
            try
            {
                // 将更改保存到数据库
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    SqlCommand command = new SqlCommand($"UPDATE packing_info SET {dataGridView1.Columns[e.ColumnIndex].Name} = N'{row.ItemArray[e.ColumnIndex]}' WHERE id = {row.ItemArray[0]}", conn);
                    conn.Open();
                    command.ExecuteNonQuery();

                }
                Form1.GetBaseInfoByDB(); //更新前台数据
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, $"更新数据时出错:\r\n{ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            string searchText = textBox1.Text.Trim();
            if (searchText.Length == 0) { return; }
            foreach (DataGridViewRow row in dataGridView1.Rows)
            {
                for (int i = 0; i < dataGridView1.Columns.Count; i++)
                {
                    if (row.Cells[i].Value != null && row.Cells[i].Value.ToString().Contains(searchText))
                    {
                        // 找到匹配项，选中该行并将视图定位到该行
                        dataGridView1.ClearSelection();
                        row.Selected = true;
                        dataGridView1.CurrentCell = row.Cells[i];
                        break;
                    }
                }
            }
        }


    }
}
