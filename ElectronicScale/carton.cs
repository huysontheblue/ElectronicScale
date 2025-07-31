using OfficeOpenXml;
using System.Data.SqlClient;
using Excel = OfficeOpenXml;

namespace ElectronicScale
{
    public partial class carton : Form
    {
        string connectionString = $"Server={Form1.ConfigInfo["mssql"]["host"]};Database={Form1.ConfigInfo["mssql"]["database"]}; User Id={Form1.ConfigInfo["mssql"]["user"]};Password={Form1.ConfigInfo["mssql"]["password"]};Trusted_Connection=True;integrated security=False";
        public carton()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            DateTime fromDate = date1.Value;
            DateTime toDate = date2.Value;
            if (fromDate >= toDate)
            {
                MessageBox.Show("结束日期必须大于开始日期");
                return;
            }
            if (string.IsNullOrEmpty(txtApn.Text))
            {
                LoadAllData(fromDate, toDate);
            }
            else
            {
                LoadDataApn(fromDate, toDate);
            }
        }

        private void LoadDataApn(DateTime? fromDate = null, DateTime? toDate = null)
        {
            try
            {
                string query = @"SELECT apn,color,spec,lag,erweima,NW,GW,create_time from packing_carton_small where apn = @apn";
                if (fromDate != null)
                {
                    query += " AND create_time >= @fromDate";
                }
                if (toDate != null)
                {
                    query += " AND create_time <= @toDate";
                }

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    SqlCommand command = new SqlCommand(query, connection);
                    command.Parameters.AddWithValue("@apn", txtApn.Text.Trim());
                    if (fromDate != null)
                    {
                        command.Parameters.AddWithValue("@fromDate", fromDate.Value);
                    }
                    if (toDate != null)
                    {
                        command.Parameters.AddWithValue("@toDate", toDate.Value);
                    }
                    SqlDataAdapter adapter = new SqlDataAdapter(command);
                    System.Data.DataTable dataTable = new System.Data.DataTable();
                    adapter.Fill(dataTable);
                    dataGridView1.DataSource = dataTable;
                    ConfigureDataGridView();
                    UpdateRowCount();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi tải dữ liệu: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LoadAllData(DateTime? fromDate = null, DateTime? toDate = null)
        {
            try
            {
                string query = @"SELECT apn,color,spec,lag,erweima,NW,GW,create_time from packing_carton_small where 1=1";
                if (fromDate != null)
                {
                    query += " AND create_time >= @fromDate";
                }
                if (toDate != null)
                {
                    query += " AND create_time <= @toDate";
                }

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    SqlCommand command = new SqlCommand(query, connection);

                    if (fromDate != null)
                    {
                        command.Parameters.AddWithValue("@fromDate", fromDate.Value);
                    }
                    if (toDate != null)
                    {
                        command.Parameters.AddWithValue("@toDate", toDate.Value);
                    }

                    SqlDataAdapter adapter = new SqlDataAdapter(command);
                    System.Data.DataTable dataTable = new System.Data.DataTable();
                    //DataTable dataTable = new DataTable();
                    adapter.Fill(dataTable);
                    dataGridView1.DataSource = dataTable;
                    ConfigureDataGridView();
                    UpdateRowCount();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi tải dữ liệu: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void UpdateRowCount()
        {
            int rowCount = 0;
            if (dataGridView1 != null && dataGridView1.Rows != null)
            {
                if (dataGridView1.DataSource is System.Data.DataTable)
                {
                    rowCount = dataGridView1.Rows.Count;
                }
            }
            if (label3 != null)
            {
                label3.Text = rowCount.ToString();
            }
        }

        private void ConfigureDataGridView()
        {
            if (dataGridView1.Columns.Count == 0) return;
            dataGridView1.AutoGenerateColumns = false;
            dataGridView1.Columns.Clear();
            DataGridViewTextBoxColumn sttColumn = new DataGridViewTextBoxColumn(); sttColumn.Name = "STT"; sttColumn.HeaderText = "STT"; sttColumn.Width = 50; sttColumn.ReadOnly = true; dataGridView1.Columns.Insert(0, sttColumn);
            dataGridView1.Columns.Add(new DataGridViewTextBoxColumn() { Name = "apn", DataPropertyName = "apn", HeaderText = "apn", Width = 150, ReadOnly = true });
            dataGridView1.Columns.Add(new DataGridViewTextBoxColumn() { Name = "color", DataPropertyName = "color", HeaderText = "color", Width = 100, ReadOnly = true, DefaultCellStyle = new DataGridViewCellStyle() { Alignment = DataGridViewContentAlignment.MiddleCenter } });
            dataGridView1.Columns.Add(new DataGridViewTextBoxColumn() { Name = "spec", DataPropertyName = "spec", HeaderText = "spec", Width = 100, ReadOnly = true });
            dataGridView1.Columns.Add(new DataGridViewTextBoxColumn() { Name = "lag", DataPropertyName = "lag", HeaderText = "lag", Width = 100, ReadOnly = true });
            dataGridView1.Columns.Add(new DataGridViewTextBoxColumn() { Name = "erweima", DataPropertyName = "erweima", HeaderText = "erweima", Width = 300, ReadOnly = true });
            dataGridView1.Columns.Add(new DataGridViewTextBoxColumn() { Name = "NW", DataPropertyName = "NW", HeaderText = "NW", Width = 70, ReadOnly = true });
            dataGridView1.Columns.Add(new DataGridViewTextBoxColumn() { Name = "GW", DataPropertyName = "GW", HeaderText = "GW", Width = 70, ReadOnly = true });
            dataGridView1.Columns.Add(new DataGridViewTextBoxColumn() { Name = "create_time", DataPropertyName = "create_time", HeaderText = "create_time", Width = 200, ReadOnly = true, DefaultCellStyle = new DataGridViewCellStyle() { Alignment = DataGridViewContentAlignment.MiddleCenter } });
            UpdateRowNumbers();
            dataGridView1.EnableHeadersVisualStyles = false;
            dataGridView1.ColumnHeadersDefaultCellStyle.BackColor = Color.LightBlue;
            dataGridView1.ColumnHeadersDefaultCellStyle.Font = new System.Drawing.Font("Arial", 9, System.Drawing.FontStyle.Bold);
            //dataGridView1.ColumnHeadersDefaultCellStyle.Font = new Font("Arial", 9, FontStyle.Bold);
            dataGridView1.RowHeadersVisible = false;
            dataGridView1.AllowUserToAddRows = false;
            dataGridView1.AllowUserToDeleteRows = false;
            dataGridView1.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.None;
            dataGridView1.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            //dataGridView1.ScrollBars = ScrollBars.Both;
            dataGridView1.Refresh();
        }

        private void UpdateRowNumbers()
        {
            foreach (DataGridViewRow row in dataGridView1.Rows)
            {
                if (!row.IsNewRow)
                {
                    row.Cells["STT"].Value = row.Index + 1;
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                if (dataGridView1.Rows.Count <= 1)
                {
                    MessageBox.Show("Không có dữ liệu để xuất!", "Thông báo");
                    return;
                }
                ExcelPackage.LicenseContext = Excel.LicenseContext.NonCommercial;
                using (var package = new ExcelPackage())
                {
                    var worksheet = package.Workbook.Worksheets.Add("Sheet1");
                    for (int i = 0; i < dataGridView1.Columns.Count; i++)
                    {
                        worksheet.Cells[1, i + 1].Value = dataGridView1.Columns[i].HeaderText;
                        worksheet.Cells[1, i + 1].Style.Font.Bold = true;
                        worksheet.Cells[1, i + 1].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                        worksheet.Cells[1, i + 1].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.LightGray);
                    }
                    for (int i = 0; i < dataGridView1.Rows.Count; i++)
                    {
                        for (int j = 0; j < dataGridView1.Columns.Count; j++)
                        {
                            object cellValue = dataGridView1[j, i].Value;
                            worksheet.Cells[i + 2, j + 1].Value = cellValue != null ? cellValue.ToString() : "";
                        }
                    }
                    worksheet.Cells.AutoFitColumns();
                    SaveFileDialog saveFileDialog = new SaveFileDialog();
                    saveFileDialog.Filter = "Excel Files|*.xlsx";
                    saveFileDialog.Title = "Lưu file Excel";
                    saveFileDialog.FileName = "Export_" + DateTime.Now.ToString("yyyyMMdd_HHmmss");
                    if (saveFileDialog.ShowDialog() == DialogResult.OK)
                    {
                        File.WriteAllBytes(saveFileDialog.FileName, package.GetAsByteArray());
                        MessageBox.Show("Xuất Excel thành công!", "Thông báo");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi xuất Excel: {ex.Message}\n\nChi tiết: {ex.ToString()}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        //private void btnDelete_Click(object sender, EventArgs e)
        //{
        //    try
        //    {
        //        if (dataGridView1.SelectedRows.Count == 0)
        //        {
        //            MessageBox.Show("Vui lòng chọn ít nhất một dòng để xóa!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        //            return;
        //        }
        //        string input = Interaction.InputBox("Mời nhập mật khẩu", "Nhập mật khẩu để xóa dữ liệu", "");
        //        if (string.IsNullOrEmpty(input))
        //        {
        //            //MessageBox.Show("Mật khẩu không được để trống hoặc đã nhấn Cancel!", "密码错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
        //            return;
        //        }

        //        string dt = DateTime.Now.ToString("HHmm");
        //        if (input != dt)
        //        {
        //            MessageBox.Show("Mật khẩu không đúng, vui lòng thử lại!\r\n请重试", "Mật khẩu sai", MessageBoxButtons.OK, MessageBoxIcon.Error);
        //            return;
        //        }

        //        if (MessageBox.Show("Bạn có chắc chắn muốn xóa?", "Xác nhận xóa", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
        //        {
        //            return;
        //        }


        //        using (SqlConnection connection = new SqlConnection(connectionString))
        //        {
        //            connection.Open();
        //            using (SqlTransaction transaction = connection.BeginTransaction())
        //            {
        //                try
        //                {
        //                    using (SqlCommand logCommand = new SqlCommand(
        //                        @"INSERT INTO packing_scale_log (apn, code, sn, weight, create_time)
        //                        VALUES (@apn, @code, @sn, @weight, @create_time)", connection, transaction))
        //                    {
        //                        using (SqlCommand deleteCommand = new SqlCommand(
        //                            "DELETE FROM packing_scale WHERE code = @code", connection, transaction))
        //                        {
        //                            foreach (DataGridViewRow row in dataGridView1.SelectedRows)
        //                            {
        //                                if (!row.IsNewRow)
        //                                {
        //                                    string code = row.Cells["code"].Value?.ToString();
        //                                    if (!string.IsNullOrEmpty(code))
        //                                    {
        //                                        string apn = row.Cells["apn"].Value?.ToString();
        //                                        string sn = row.Cells["sn"].Value?.ToString();
        //                                        string weightStr = row.Cells["weight"].Value?.ToString().Replace(" KG", "");
        //                                        decimal? weight = decimal.TryParse(weightStr, out decimal w) ? w : (decimal?)null;
        //                                        DateTime? createTime = row.Cells["create_time"].Value != null
        //                                            ? Convert.ToDateTime(row.Cells["create_time"].Value) : (DateTime?)null;

        //                                        logCommand.Parameters.Clear();
        //                                        logCommand.Parameters.AddWithValue("@apn", (object)apn ?? DBNull.Value);
        //                                        logCommand.Parameters.AddWithValue("@code", code);
        //                                        logCommand.Parameters.AddWithValue("@sn", (object)sn ?? DBNull.Value);
        //                                        logCommand.Parameters.AddWithValue("@weight", (object)weight ?? DBNull.Value);
        //                                        logCommand.Parameters.AddWithValue("@create_time", (object)createTime ?? DBNull.Value);
        //                                        logCommand.ExecuteNonQuery();

        //                                        deleteCommand.Parameters.Clear();
        //                                        deleteCommand.Parameters.AddWithValue("@code", code);
        //                                        deleteCommand.ExecuteNonQuery();
        //                                    }
        //                                }
        //                            }
        //                        }
        //                    }
        //                    transaction.Commit();
        //                }
        //                catch
        //                {
        //                    transaction.Rollback();
        //                    throw;
        //                }
        //            }
        //        }

        //        if (string.IsNullOrEmpty(txtApn.Text))
        //        {
        //            LoadAllData(date1.Value, date2.Value);
        //        }
        //        else
        //        {
        //            LoadDataApn(date1.Value, date2.Value);
        //        }

        //        MessageBox.Show("Xóa dữ liệu thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
        //    }
        //    catch (Exception ex)
        //    {
        //        MessageBox.Show("Lỗi khi xóa dữ liệu: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
        //    }
        //}
    }
}
