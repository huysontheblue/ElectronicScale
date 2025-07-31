using BarTender;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using BtApp = BarTender.Application;

namespace ElectronicScale
{
    public partial class PrintCarton : Form
    {
        internal static string JsonConfigPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "config.json");
        private Format btFormat;
        private BtApp btApp;
        public PrintCarton()
        {
            InitializeComponent();
        }

        private void txtSN_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                e.Handled = true;
                string input = txtSN.Text.Trim();
                if (string.IsNullOrWhiteSpace(input))
                {
                    txtSN.SelectAll();
                    txtSN.Focus();
                    return;
                }
                if (!input.Contains(","))
                {
                    label2.Text = "Dữ liệu k hợp lệ";
                    txtSN.SelectAll();
                    txtSN.Focus();
                    return;
                }

                string[] parts = input.Split(',');
                string apn = parts[0];
                string sn1 = apn;
                string sn2 = parts.Length > 1 ? parts[1] : "";
                string erweima = input;
                string filePath = @"D:\packing_info.txt";
                string bartenderFilePath = @"D:\packing_info.btw";
                string header = "apn,color,spec,lag,nw,gw,sn1,sn2";
                string data = "";
                string color = "N/A";
                string spec = "N/A";
                string lag = "N/A";
                string nw = "N/A";
                string gw = "N/A";
                DateTime createTime = DateTime.Now;

                string connectionString;
                try
                {
                    string configPath = Path.Combine(System.Windows.Forms.Application.StartupPath, "config.json");
                    string jsonContent = File.ReadAllText(configPath);
                    using (JsonDocument doc = JsonDocument.Parse(jsonContent))
                    {
                        JsonElement root = doc.RootElement;
                        JsonElement mssql = root.GetProperty("mssql");
                        string host = mssql.GetProperty("host").GetString();
                        int port = mssql.GetProperty("port").GetInt32();
                        string database = mssql.GetProperty("database").GetString();
                        string user = mssql.GetProperty("user").GetString();
                        string password = mssql.GetProperty("password").GetString();
                        connectionString = $"Server={host},{port};Database={database};User Id={user};Password={password};";
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi khi đọc file config.json: " + ex.Message, "Lỗi hệ thống", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    BeginInvoke(() =>
                    {
                        this.Activate();
                        txtSN.SelectAll();
                        txtSN.Focus();
                    });
                    return;
                }

                try
                {
                    using (SqlConnection connection = new SqlConnection(connectionString))
                    {
                        connection.Open();
                        string checkQuery = "SELECT apn, color, spec, lag, erweima, NW, GW FROM packing_carton_small WHERE erweima = @erweima";
                        using (SqlCommand checkCommand = new SqlCommand(checkQuery, connection))
                        {
                            checkCommand.Parameters.AddWithValue("@erweima", erweima);
                            using (SqlDataReader reader = checkCommand.ExecuteReader())
                            {
                                if (reader.Read())
                                {
                                    apn = reader["apn"].ToString();
                                    color = reader["color"].ToString();
                                    spec = reader["spec"].ToString();
                                    lag = reader["lag"].ToString();
                                    nw = reader["NW"].ToString();
                                    gw = reader["GW"].ToString();
                                    erweima = reader["erweima"].ToString();
                                    parts = erweima.Split(',');
                                    sn1 = parts[0];
                                    sn2 = parts.Length > 1 ? parts[1] : "";

                                    data = string.Join(",", new string[] { apn, color, spec, lag, nw, gw, sn1, sn2 });

                                    try
                                    {
                                        string logDirectory = Path.GetDirectoryName(filePath);
                                        if (!Directory.Exists(logDirectory))
                                        {
                                            Directory.CreateDirectory(logDirectory);
                                        }
                                        File.WriteAllText(filePath, header + Environment.NewLine + data + Environment.NewLine, Encoding.UTF8);
                                    }
                                    catch (Exception ex)
                                    {
                                        MessageBox.Show("Lỗi khi ghi file D:\\packing_info.txt: " + ex.Message, "Lỗi hệ thống", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                    }
                                    FileToBarCodePrint(bartenderFilePath, "");
                                    BeginInvoke(() =>
                                    {
                                        this.Activate();
                                        txtSN.SelectAll();
                                        txtSN.Focus();
                                        Debug.WriteLine("Focus set to txtSN, ActiveControl: " + this.ActiveControl.Name);
                                    });
                                    return;
                                }
                            }
                        }

                        label2.Text = "Không tìm thấy dữ liệu cho mã SN: " + erweima;
                        BeginInvoke(() =>
                        {
                            this.Activate();
                            txtSN.SelectAll();
                            txtSN.Focus();
                            Debug.WriteLine("Focus set to txtSN, ActiveControl: " + this.ActiveControl.Name);
                        });
                        return;
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi khi kiểm tra mã SN trong cơ sở dữ liệu: " + ex.Message, "Lỗi hệ thống", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    BeginInvoke(() =>
                    {
                        this.Activate();
                        txtSN.SelectAll();
                        txtSN.Focus();
                        Debug.WriteLine("Focus set to txtSN, ActiveControl: " + this.ActiveControl.Name);
                    });
                    return;
                }
            }
        }
        private void FileToBarCodePrint(string pFilePath, string printName)
        {
            if (btApp == null)
            {
                btApp = new BtApp();
            }
            try
            {
                btFormat = btApp.Formats.Open(pFilePath, false, "");
                //btFormat.PrintSetup.IdenticalCopiesOfLabel = Convert.ToInt32(TextBox1.Text);
                btFormat.PrintOut(false, false);
                btFormat.Close(BtSaveOptions.btDoNotSaveChanges);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi in tem - Liên hệ IT: " + ex.Message);
            }
        }

    }
}
