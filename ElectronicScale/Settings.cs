using Newtonsoft.Json;
using System.IO.Ports;

namespace ElectronicScale
{
    public partial class Settings : Form
    {
        private int[] BaudRateList = { 110, 300, 600, 1200, 2400, 4800, 9600, 14400, 19200, 38400, 56000, 57600, 115200, 128000, 256000 };
        private int[] BaudRateList1 = { 110, 300, 600, 1200, 2400, 4800, 9600, 14400, 19200, 38400, 56000, 57600, 115200, 128000, 256000 };
        public Settings()
        {
            InitializeComponent();
            SettingsFormInit();
        }
        /// <summary>
        /// 初始化设置
        /// </summary>
        public void SettingsFormInit()
        {
            var comList = SerialPort.GetPortNames();
            var comList1 = SerialPort.GetPortNames();

            if (comList != null)
            {
                cb_com_1.DataSource = comList;
                if (comList.Contains(Form1.ConfigInfo["scale"]["port"]))
                {
                    cb_com_1.SelectedItem = Form1.ConfigInfo["scale"]["port"];
                }
                cb_com_2.DataSource = comList1;
                if (comList.Contains(Form1.ConfigInfo["alarm"]["port"]))
                {
                    cb_com_2.SelectedItem = Form1.ConfigInfo["alarm"]["port"];
                }
            }

            cb_baudrate_1.DataSource = BaudRateList;
            if (BaudRateList.Contains(Convert.ToInt32(Form1.ConfigInfo["scale"]["baudrate"])))
            {
                cb_baudrate_1.SelectedItem = Convert.ToInt32(Form1.ConfigInfo["scale"]["baudrate"]);
            }
            cb_baudrate_2.DataSource = BaudRateList1;
            if (BaudRateList1.Contains(Convert.ToInt32(Form1.ConfigInfo["alarm"]["baudrate"])))
            {
                cb_baudrate_2.SelectedItem = Convert.ToInt32(Form1.ConfigInfo["alarm"]["baudrate"]);
            }

            tb_filepath.Text = (string)Form1.ConfigInfo["fileinfo"]["path"];

            if (Convert.ToBoolean(Form1.ConfigInfo["alarm"]["voicestatus"]))
            {
                rb_voice_on.Checked = true;
            }
            else { rb_voice_off.Checked = true; }

            nud_ualarm_time.Value = Convert.ToDecimal(Form1.ConfigInfo["alarm"]["alarmupper"]);
            nud_dalarm_time.Value = Convert.ToDecimal(Form1.ConfigInfo["alarm"]["alarmlower"]);

            //添加打印机列表
            //new PrintTag().GetZebraPrinterList();
            List<string> printerList = new PrintTag().GetPrinterList();
            comboBox1.DataSource = printerList;
            //if (printerList.Contains((string)Form1.ConfigInfo["printer"]["printer"]))
            //{
            //    comboBox1.SelectedItem = (string)Form1.ConfigInfo["printer"]["printer"];
            //}
            //textBox1.Text = (string)Form1.ConfigInfo["printer"]["ip"];
        }

        private void cb_com_1_SelectionChangeCommitted(object sender, EventArgs e)
        {
            if (cb_com_1.SelectedItem == null) return;
            Form1.ConfigInfo["scale"]["port"] = cb_com_1.SelectedItem.ToString();
        }

        private void cb_baudrate_1_SelectionChangeCommitted(object sender, EventArgs e)
        {
            if (cb_baudrate_1.SelectedItem == null) return;
            Form1.ConfigInfo["scale"]["baudrate"] = Convert.ToInt32(cb_baudrate_1.SelectedItem);
        }
        private void cb_com_2_SelectionChangeCommitted(object sender, EventArgs e)
        {
            if (cb_com_2.SelectedItem == null) return;
            Form1.ConfigInfo["alarm"]["port"] = cb_com_2.SelectedItem.ToString();
        }

        private void cb_baudrate_2_SelectionChangeCommitted(object sender, EventArgs e)
        {
            if (cb_baudrate_2.SelectedItem == null) return;
            Form1.ConfigInfo["alarm"]["baudrate"] = Convert.ToInt32(cb_baudrate_2.SelectedItem);
        }

        private void tb_filepath_DoubleClick(object sender, EventArgs e)
        {
            FolderBrowserDialog folderBrowserDialog = new FolderBrowserDialog();
            DialogResult result = folderBrowserDialog.ShowDialog();
            if (result == DialogResult.OK)
            {
                Form1.ConfigInfo["fileinfo"]["path"] = folderBrowserDialog.SelectedPath;
                tb_filepath.Text = folderBrowserDialog.SelectedPath;
            }
        }


        private void rb_voice_on_CheckedChanged(object sender, EventArgs e)
        {
            Form1.ConfigInfo["alarm"]["voicestatus"] = true;
        }

        private void rb_voice_off_CheckedChanged(object sender, EventArgs e)
        {
            Form1.ConfigInfo["alarm"]["voicestatus"] = false;
        }

        private void nud_ualarm_time_ValueChanged(object sender, EventArgs e)
        {
            Form1.ConfigInfo["alarm"]["alarmupper"] = nud_ualarm_time.Value;
        }

        private void nud_dalarm_time_ValueChanged(object sender, EventArgs e)
        {
            Form1.ConfigInfo["alarm"]["alarmlower"] = nud_dalarm_time.Value;
        }
        private void bt_save_Click(object sender, EventArgs e)
        {
            this.SaveConfig();
            this.Close();
        }

        private void SaveConfig()
        {
            Task.Run(() =>
            {
                File.WriteAllTextAsync(Form1.JsonConfigPath, JsonConvert.SerializeObject(Form1.ConfigInfo, Formatting.Indented));
            });
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            Form1.ConfigInfo["printer"]["printer"] = comboBox1.SelectedItem.ToString();
        }

        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                Form1.ConfigInfo["printer"]["ip"] = textBox1.Text;
            }
        }

        private void textBox1_Leave(object sender, EventArgs e)
        {
            Form1.ConfigInfo["printer"]["ip"]=textBox1.Text;
        }
    }

}
