using ElectronicScale.Models;
using Google.Protobuf.WellKnownTypes;
using Microsoft.VisualBasic;
using Newtonsoft.Json;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Drawing;
using System.IO.Ports;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using thinger.DataConvertLib;
using static Mysqlx.Crud.Order.Types;
using BarTender;
using BtApp = BarTender.Application;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using WinFormsTextBox = System.Windows.Forms.TextBox;
using System.ComponentModel;
using System;
using System.Data;

namespace ElectronicScale
{
    public partial class Form1 : Form
    {
        internal static string JsonConfigPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "config.json");
        private SerialPort sp1;  //电子秤
        private static SerialPort sp2;  //报警器
        private Alarm alarm = new();
        private RingBufferManager rbm = new(1024);
        Alarm.WarnType WarningType = Alarm.WarnType.Reset;  //最后的报警状态
        static int bufferLength = 128; //环形缓冲区长度
        byte[] sbuffer = new byte[bufferLength];
        int head = 0;
        int length = 0;
        int freesize = bufferLength;
        Regex digit = new Regex(@"(\d+)(\.\d+)?");
        Regex jh = new Regex("-+");
        Regex jiahao = new Regex(@"\++");
        Regex ST = new Regex(@"^(ST)+");
        internal static MsSQLDB msdb;
        private Format btFormat;
        private BtApp btApp;
        internal static Dictionary<string, Dictionary<string, string>> APNList;  //APN对应列表
        internal static Dictionary<string, Dictionary<string, string>> LimitList;  //上下限列表
        string tagContext = string.Empty;  //标签模板
        internal static Dictionary<string, Dictionary<string, object>> ConfigInfo = new Dictionary<string, Dictionary<string, object>>();
        MsSQLDB msdbWrite;
        private System.Windows.Forms.Timer autoPrintTimer;
        private bool isScanning = false;
        public Form1(string[] args)
        {
            InitializeComponent();
            LoadConfig();
            toolStripStatusLabel5.Text = $"version:{args[0]}";
        }
        public Form1()
        {
            InitializeComponent();
            LoadConfig();
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            try
            {
                msdb = new MsSQLDB();
                GetBaseInfoByDB();
                msdbWrite = new MsSQLDB($"Server={ConfigInfo["mssql"]["host"]};Database={ConfigInfo["mssql"]["database"]}; User Id={ConfigInfo["mssql"]["user"]};Password={ConfigInfo["mssql"]["password"]};Trusted_Connection=True;integrated security=False");
            }
            catch (Exception es)
            {
                MessageBox.Show($"连接数据库异常\r\n{es.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            autoPrintTimer = new System.Windows.Forms.Timer();
            autoPrintTimer.Interval = 1000;
            autoPrintTimer.Tick += (s, ev) =>
            {
                autoPrintTimer.Stop();
                if (!string.IsNullOrEmpty(textBox1.Text) && !string.IsNullOrEmpty(textBox7.Text))
                {
                    button1.PerformClick();
                }
            };
        }
        private void 系统配置ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Settings childForm = new Settings();
            childForm.ShowDialog();
        }
        private void LoadConfig()
        {
            ConfigInfo = JsonConvert.DeserializeObject<Dictionary<string, Dictionary<string, object?>>>(File.ReadAllText(JsonConfigPath)) ?? ConfigInfo;
            InitTagModel();
        }
        void InitTagModel()
        {
            try
            {
                //throw new Exception("");
                tagContext = File.ReadAllText(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "tag.txt"));
                //if (tagContext.Length == 0) throw new Exception();
            }
            catch
            {
                //TSC
                //tagContext = "SIZE 80 mm,40 mm" +
                //   "GAP 0,0" +
                //   "DIRECTION 1" +
                //   "CLS" +
                //   "TEXT 10,10,\"2\",0,1,1,\"{spec}\"" +
                //   "TEXT 10,40,\"实重: {weight}kg\",0,1,1" +
                //   "BARCODE 10,50,\"TELEPEN\",100,1,0,2,6,\"{apn},{DateTime1}\"" +
                //   "TEXT 10,80,\"标重:{weight1}±0.2kg\",0,1,1" +
                //   "TEXT 10,130,\"日期:{DateTime}\",0,1,1" +
                //   "PRINT {quality}";

                //Zebra
                tagContext = "^XA^CI28" +
                    "^MD20^LL295^PW591" +
                    "^CW1,E:SIMSUN.FNT" +
                    "^A1N,20,20^FO10,10,E:SIMSUN.FNT^FD{spec}^FS" +
                    "^AN,20,20^FO10,40E:SIMSUN.FNT^FD实重:{weight}kg^FS" +
                    "^FO420,5^BQN,2,4^FDLA,{apn},{DateTime1}^FS" +
                    "^A1N,20,20^FO10,80,E:SIMSUN.FNT^FD标重:{weight1}±0.2kg^FS" +
                    "^A1N,20,20^FO10,140,E:SIMSUN.FNT^FD日期:{DateTime}^FS" +
                    "^PQ{quality},0,{quality},Y^XZ";

            }
        }
        internal static void Warning(byte[] b, int v, int length)
        {
            try
            {
                if (sp2 == null)
                {
                    sp2 = new((string)ConfigInfo["alarm"]["port"], Convert.ToInt32(ConfigInfo["alarm"]["baudrate"]));
                    sp2.Open();
                }
                if (sp2.IsOpen)
                {
                    sp2.Write(b, v, length);
                }
            }
            catch (Exception e)
            {
                MessageBox.Show("报警输出错误:" + e.Message, "错误提示:", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btn_scale_Click(object sender, EventArgs e)
        {
            try
            {
                if (sp1 == null)
                {
                    sp1 = new((string)ConfigInfo["scale"]["port"], Convert.ToInt32(ConfigInfo["scale"]["baudrate"]));
                }
                if (sp1.IsOpen == false)
                {
                    sp1.NewLine = "\n";
                    sp1.Open();
                    sp1.DataReceived -= new SerialDataReceivedEventHandler(ScaleDataRecive);
                    sp1.DataReceived += new SerialDataReceivedEventHandler(ScaleDataRecive);
                    timer1.Interval = 500;
                    timer1.Start();
                    btn_scale.Text = "断开电子秤连接";
                    toolStripStatusLabel1.Text = "电子秤通讯正常";
                    toolStripStatusLabel1.BackColor = Color.Green;
                }
                else
                {
                    sp1.Close(); sp1.Dispose(); timer1.Stop(); taskIsRunning = false;
                    btn_scale.Text = "连接电子秤";
                    toolStripStatusLabel1.BackColor = Control.DefaultBackColor;
                    toolStripStatusLabel1.Text = "电子秤通讯已断开";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"打开电子秤通讯端口{ConfigInfo["scale"]["port"]}时出错:\r\n请检查电子秤的通讯接口或重新插拔一下\r\n再重新连接就好了\r\n{ex.Message}", "Error:", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        bool taskIsRunning = false;
        private void timer1_Tick(object sender, EventArgs e)
        {
            if (taskIsRunning == true) return;
            taskIsRunning = true;
            string sn = textBox1.Text;
            Task.Run(() =>
            {
                string apn = GetEEEECode(sn);
                //if (apn == "") return;
                string[] list = StringLib.GetStringFromByteArrayByEncoding(sbuffer, 0, bufferLength, Encoding.ASCII).Split("kg\r\n");
                BeginInvoke(() =>
                {
                    foreach (string str in list)
                    {
                        string tmpValue = digit.Match(str).Value;
                        if (ST.Match(str).Success && tmpValue.Length >= 5)
                        {
                            if (jh.IsMatch(str)) tmpValue = $"-{tmpValue}";
                            textBox7.Text = tmpValue;
                            Decimal tv = Convert.ToDecimal(tmpValue);
                            if (tv <= 0)
                            {
                                //if (ZeroTag == false)
                                //{
                                if (WarningType != Alarm.WarnType.Reset)
                                {
                                    WarningType = Alarm.WarnType.Reset;
                                    if (sp2 != null && sp2.IsOpen)  //发送报警
                                    {
                                        alarm.send_warn_info(WarningType);
                                    }
                                    textBox9.BackColor = Color.White;
                                    textBox9.ForeColor = Control.DefaultForeColor;
                                    textBox9.Text = $"";
                                    CleanText(true);
                                }
                            }
                            // 框内有内容  在列表里 upper不等于空 超上限
                            else if (apn != "" && LimitList.ContainsKey(apn) && tv > Convert.ToDecimal(LimitList[apn]["upper"]))
                            {
                                if (WarningType != Alarm.WarnType.Upper)
                                {
                                    textBox9.BackColor = Color.Red;
                                    textBox9.ForeColor = Color.White;
                                    textBox9.Text = "超上限 - Quá Trọng Lượng";
                                    WarningType = Alarm.WarnType.Upper;
                                    if (sp2 != null && sp2.IsOpen)  //发送报警
                                    {
                                        alarm.send_warn_info(WarningType);
                                    }
                                }
                                //ZeroTag = false;
                            }
                            // 框内有内容  在列表里 upper不等于空 超下限
                            else if (apn != "" && LimitList.ContainsKey(apn) && tv < Convert.ToDecimal(LimitList[apn]["lower"]))
                            {
                                if (WarningType != Alarm.WarnType.Lower)
                                {
                                    textBox9.BackColor = Color.Yellow;
                                    textBox9.ForeColor = Color.Black;
                                    textBox9.Text = "超下限 - Không Đủ Trọng Lượng";
                                    WarningType = Alarm.WarnType.Lower;
                                    if (sp2 != null && sp2.IsOpen)  //发送报警
                                    {
                                        alarm.send_warn_info(WarningType);
                                    }
                                }
                                //ZeroTag = false;
                            }
                            else if (apn != "" && LimitList.ContainsKey(apn))//正常

                            {
                                //ZeroTag = false;
                                if (WarningType != Alarm.WarnType.Normal)
                                {
                                    textBox9.BackColor = Color.Green;
                                    textBox9.ForeColor = Color.White;
                                    textBox9.Text = "正常 - Đúng Trọng Lượng";
                                    WarningType = Alarm.WarnType.Normal;
                                    if (sp2 != null && sp2.IsOpen)  //发送报警
                                    {
                                        alarm.send_warn_info(WarningType);
                                    }
                                    Task.Run(() =>
                                    {
                                        try
                                        {
                                            string now = DateTime.Now.ToString("yyMMddHHmmss");
                                            string datetime = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss");
                                            bool resut = printTag.PrintByTCP(
                                                ConfigInfo["printer"]["ip"]?.ToString() ?? "",
                                                9100,
                                                GetTagContents(
                                                    LimitList[apn]["tag_product_name"]?.ToString() ?? "",
                                                    LimitList[apn]["apn"]?.ToString() ?? "" + ";" + now,
                                                    tmpValue,
                                                    LimitList[apn]["standard"]?.ToString() ?? "",
                                                    ConfigInfo["printer"]["quality"]?.ToString() ?? "1"
                                                ));
                                            if (resut != true)
                                            {
                                                MessageBox.Show($"打印标签时出错:\r\n反正就是错了", "错误提示:", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                            }
                                            else  //写入系统数据库
                                            {
                                                //string sql = $"insert into packing_scale values('{LimitList[apn]["apn"]}','{LimitList[apn]["apn"]};{now}','{sn}','{tmpValue}','{datetime}');";
                                                // 将更改保存到数据库
                                                //msdbWrite.InsertData(sql);
                                            }
                                        }
                                        catch (Exception ex)
                                        {
                                            Debug.WriteLine(ex.Message);
                                            MessageBox.Show($"打印标签时出错:\r\n{ex.Message}", "错误提示:", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                        }
                                    });
                                    //BeginInvoke(() =>
                                    //{
                                    CleanText(true);
                                    //});
                                }
                                //ZeroTag = false;
                            }

                            else
                            {
                                if (textBox9.Text != "")
                                {
                                    textBox9.BackColor = Control.DefaultBackColor;
                                    textBox9.ForeColor = Control.DefaultForeColor;
                                    textBox9.Text = "";
                                }
                            }
                        }
                    }
                });

                taskIsRunning = false;
            });
        }
        void ScaleDataRecive(object sender, EventArgs e)
        {
            SerialPort sp = (SerialPort)sender;
            int l = sp.BytesToRead;
            byte[] buffer = new byte[l];
            sp.Read(buffer, 0, l);
            writeBuffer(buffer);
        }
        void writeBuffer(byte[] stringlist, int startIndex = 0, int dataLength = 0)
        {
            if (stringlist == null || stringlist.Length == 0) return;

            freesize = bufferLength - head;
            dataLength = (dataLength <= 0) ? stringlist.Length : dataLength;


            if (startIndex + dataLength > stringlist.Length)
            {
                dataLength = stringlist.Length - startIndex;
            }

            if (dataLength > freesize)
            {

                int copyLength = Math.Min(freesize, stringlist.Length - startIndex);
                Array.Copy(stringlist, startIndex, sbuffer, head, copyLength);
                head = 0;
                writeBuffer(stringlist, startIndex + copyLength, dataLength - copyLength);
            }
            else
            {
                int copyLength = Math.Min(dataLength, bufferLength - head);
                Array.Copy(stringlist, startIndex, sbuffer, head, copyLength);
                head += copyLength;
                if (head >= bufferLength) head = 0;
            }
        }
        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            if (!isScanning && !string.IsNullOrEmpty(textBox1.Text) && !string.IsNullOrEmpty(textBox7.Text))
            {
                autoPrintTimer.Stop();
                autoPrintTimer.Start();
            }
        }
        private void ApnChanged(object sender, EventArgs e)
        {
            string eeee = GetEEEECode();

            if (eeee == "") { CleanText(true); return; }

            if (APNList == null || LimitList == null || !LimitList.ContainsKey(eeee)) { GetBaseInfoByDB(); }
            if (LimitList.ContainsKey(eeee))
            {
                if (APNList.ContainsKey((string)LimitList[eeee]["apn"]))
                {
                    textBox2.Text = APNList[(string)LimitList[eeee]["apn"]]["project"];
                    textBox3.Text = APNList[(string)LimitList[eeee]["apn"]]["color"];
                    textBox4.Text = APNList[(string)LimitList[eeee]["apn"]]["spec"];
                    textBox5.Text = APNList[(string)LimitList[eeee]["apn"]]["customer"];
                    //textBox5.Text = APNList[apn]["customer"];
                    textBox6.Text = APNList[(string)LimitList[eeee]["apn"]]["num"];
                    textBox8.Text = (string)LimitList[eeee]["standard"];
                    label11.Text = "";
                }
                else
                {
                    CleanText(true);
                }
            }
            else
            {
                CleanText();
            }
        }
        static List<string> eeeeList = new List<string>();
        public static void GetBaseInfoByDB()
        {
            string sql = "SELECT * FROM [dbo].[packing_info] order by id desc;";
            APNList = new Dictionary<string, Dictionary<string, string>>();
            LimitList = new Dictionary<string, Dictionary<string, string>>();
            SqlDataReader result = msdb.ExecuteReader(sql);
            while (result.Read())
            {
                if (result["eeee"] != null && (string)result["eeee"] != "")
                {
                    eeeeList.Add((string)result["eeee"]);
                }
                if (!APNList.ContainsKey(result["apn"].ToString()))
                {
                    Dictionary<string, string> tmpdict = new Dictionary<string, string>();
                    tmpdict.Add("apn", (result["apn"] as string) ?? "");//(string)result["apn"]);
                    tmpdict.Add("eeee", (result["eeee"] as string) ?? "");//(string)result["eeee"]);
                    tmpdict.Add("project", (result["project"] as string) ?? "");//(string)result["project"]);
                    tmpdict.Add("color", (result["color"] as string) ?? "");//(string)result["color"]);
                    tmpdict.Add("spec", (result["spec"] as string) ?? "");//(string)result["spec"]);
                    tmpdict.Add("upper", (result["upper"] as string) ?? "");//(string)result["upper"]);
                    tmpdict.Add("lower", (result["lower"] as string) ?? "");//(string)result["lower"]);
                    tmpdict.Add("standard", (result["standard"] as string) ?? "");//(string)result["standard"]);
                    tmpdict.Add("customer", (result["customer"] as string) ?? "");//(string)result["customer"]);
                    tmpdict.Add("num", Convert.ToString(result["num"]) ?? "");//Convert.ToString(result["num"]));
                    tmpdict.Add("tag_product_name", (result["tag_product_name"] as string) ?? "");//Convert.ToString(result["tag_product_name"]));
                    tmpdict.Add("range", (result["range"] as string) ?? "");//Convert.ToString(result["range"]));
                    tmpdict.Add("peer_nw", (result["peer_nw"] as string) ?? "");//Convert.ToString(result["range"]));
                    tmpdict.Add("peer_interval", (result["peer_interval"] as string) ?? "");//Convert.ToString(result["range"]));
                    tmpdict.Add("packing_material_nw", (result["packing_material_nw"] as string) ?? "");//Convert.ToString(result["range"]));
                    tmpdict.Add("tray_nw", (result["tray_nw"] as string) ?? "");//Convert.ToString(result["range"]));
                    tmpdict.Add("tray_ex_num", Convert.ToString(result["tray_ex_num"]) ?? "");
                    tmpdict.Add("tray_include_num", Convert.ToString(result["tray_include_num"]) ?? "");

                    if (result["apn"] != null && (string)result["apn"] != "") APNList.Add((string)result["apn"], tmpdict);
                    if (result["eeee"] != null && (string)result["eeee"] != "") LimitList.Add((string)result["eeee"], tmpdict);
                }
                ;
            }
            result.Close();
        }
        //private string GetEEEECode(string tbstring = "")
        //{
        //    /***
        //     * 获取EEEE工程代码逻辑
        //     * 两种情况:
        //     * 1.二维码后7位
        //     * 2.二维码12-18位
        //     * 3.二维码 长度17位的,从12开始取4位
        //     * **/
        //    if (string.IsNullOrEmpty(tbstring))
        //    {
        //        if (string.IsNullOrEmpty(textBox1.Text))
        //        {
        //            CleanText(true, "Mã SN trống");
        //            return "";
        //        }
        //        tbstring = textBox1.Text;
        //    }
        //    if (tbstring.Length != 28)
        //    {
        //        CleanText(true, $"Mã SN phải có đúng 28 ký tự, nhận được {tbstring.Length} ký tự");
        //        return "";
        //    }

        //    string[] arr;
        //    if (jiahao.IsMatch(tbstring))
        //    {
        //        arr = tbstring.Split('+');
        //    }
        //    else
        //    {
        //        arr = tbstring.Split(';');
        //    }
        //    string code = arr.Length >= 2? (arr[0].Length > arr[1].Length ? arr[0] : arr[1]): arr.Length == 1 ? arr[0] : "";

        //    if (string.IsNullOrEmpty(code))
        //    {
        //        CleanText(true, "Không thể tách mã SN (dấu + hoặc ; không hợp lệ)");
        //        return "";
        //    }

        //    switch (code.Length)
        //    {
        //        case 17:
        //            string eeee17 = code.Substring(11, 4);
        //            if (eeeeList.Contains(eeee17))
        //            {
        //                return eeee17;
        //            }
        //            CleanText(true, $"Mã EEEE ({eeee17}) chưa được cấu hình");
        //            return "";
        //        case 18:
        //            string eeeeLast7 = code.Substring(code.Length - 7, 7);
        //            string eeeeMid7 = code.Substring(11, 7);
        //            if (eeeeList.Contains(eeeeLast7))
        //            {
        //                return eeeeLast7;
        //            }
        //            else if (eeeeList.Contains(eeeeMid7))
        //            {
        //                return eeeeMid7;
        //            }
        //            CleanText(true, $"Mã EEEE ({eeeeLast7} hoặc {eeeeMid7}) chưa được cấu hình");
        //            return "";
        //        default:
        //            CleanText(true, $"Độ dài mã QR không hợp lệ ({code.Length} ký tự)");
        //            return "";
        //    }
        //}

        private string GetEEEECode(string tbstring = "")
        {
            if (string.IsNullOrEmpty(tbstring))
            {
                if (string.IsNullOrEmpty(textBox1.Text))
                {
                    //label12.Text = "Mã SN trống";
                    return "";
                }
                tbstring = textBox1.Text;
            }
            if (tbstring.Length < 7)
            {
                label12.Text = $"Mã SN quá ngắn, cần ít nhất 7 ký tự, nhận được {tbstring.Length}";
                return "";
            }

            string eeee = tbstring.Substring(tbstring.Length - 7, 7);
            if (eeeeList.Contains(eeee))
            {
                return eeee;
            }

            label12.Text = $"Mã EEEE ({eeee}) chưa được cấu hình";
            return "";
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Task.Run(() =>
            {
                try
                {
                    if (sp2 == null)
                    {
                        sp2 = new SerialPort();
                        sp2.PortName = Convert.ToString(ConfigInfo["alarm"]["port"]);
                        sp2.BaudRate = Convert.ToInt32(ConfigInfo["alarm"]["baudrate"]);
                    }
                    if (sp2.IsOpen == false)
                    {
                        sp2.Open();
                        BeginInvoke(() =>
                        {
                            //ModbusFactory factory = new ModbusFactory();
                            //rtuMaster = factory.CreateRtuMaster((IStreamResource)sp2);
                            button3.Text = "断开报警器连接";
                            toolStripStatusLabel4.Text = "报警器通讯端口正常";
                            toolStripStatusLabel4.BackColor = Color.Green;
                            WarningType = Alarm.WarnType.Reset;
                            alarm.send_warn_info(WarningType);
                        });
                    }
                    else
                    {
                        WarningType = Alarm.WarnType.Off;
                        alarm.send_warn_info(WarningType);
                        sp2.Close(); sp2.Dispose(); BeginInvoke(() =>
                        {
                            button3.Text = "重连报警器";
                            toolStripStatusLabel4.BackColor = Control.DefaultBackColor;
                            toolStripStatusLabel4.Text = "报警器通讯已断开";
                        });

                    }
                }
                catch (Exception ex) { MessageBox.Show($"打开报警器端口{ConfigInfo["alarm"]["port"]}时出错:\r\n请检查报警器的通讯接口或重新插拔一次\r\n再重新连接就好了\r\n:{ex.Message}", "错误^_^", MessageBoxButtons.OK, MessageBoxIcon.Error); }
            });

        }
        private void button4_Click(object sender, EventArgs e)
        {
            WarningType = Alarm.WarnType.Reset;

            alarm.send_warn_info(WarningType);
        }
        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (sp2 != null && sp2.IsOpen)
                alarm.send_warn_info(Alarm.WarnType.Off);
        }
        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                e.Handled = true;
                string sn = textBox1.Text.Trim();

                if (textBox1.TextLength != 28)
                {
                    label12.Text = "Mã SN phải có đúng 28 ký tự (SN码必须正好有28个字符)";
                    label12.Visible = true;
                    CleanText(true);
                    textBox1.Focus();
                    return;
                }

                try
                {
                    string sql = "SELECT TOP 1 1 FROM packing_scale WHERE sn = @sn";
                    SqlParameter snParam = new SqlParameter("@sn", SqlDbType.VarChar, 28) { Value = sn };
                    bool exists = msdbWrite.ExecuteScalar(sql, snParam) != null;

                    if (exists)
                    {
                        label12.Text = "Mã SN đã tồn tại trong hệ thống (SN码已存在)";
                        label12.Visible = true;
                        CleanText(true);
                        textBox1.Focus();
                        return;
                    }
                    label12.Text = "Đang in tem, vui lòng đợi (正在打印标签，请稍候)";
                    label12.Visible = true;
                    isScanning = true;
                    ApnChanged(sender, e);

                    if (!string.IsNullOrEmpty(textBox7.Text))
                    {
                        autoPrintTimer.Stop();
                        autoPrintTimer.Start();
                    }
                    isScanning = false;
                }
                catch (Exception ex)
                {
                    label12.Text = "Lỗi kiểm tra dữ liệu (数据检查错误)";
                    label12.Visible = true;
                    Debug.WriteLine($"Database error: {ex.Message}");
                    CleanText(true);
                    textBox1.Focus();
                    return;
                }
            }
        }
        private void CleanText(bool tag = false, string lable11Text = "")
        {

            BeginInvoke(() =>
            {
                //if (tag) textBox1.SelectAll();
                if (tag) textBox1.Text = "";
                textBox2.Text = "";
                textBox3.Text = "";
                textBox4.Text = "";
                textBox5.Text = "";
                textBox6.Text = "";
                //textBox7.Text = ""; // Thêm: xóa textBox7
                textBox8.Text = "";
                textBox9.Text = ""; // Thêm: xóa textBox9
                //label12.Text = "";
                //label13.Text = "";
                label11.Text = lable11Text;
                textBox1.Focus();
            });
        }
        private PrintTag printTag = new PrintTag();
        private void button1_Click(object sender, EventArgs e)
            {
            string sn = textBox1.Text;
            string now = DateTime.Now.ToString("yyMMddHHmmss");
            string datetime = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss");
            string eeee = GetEEEECode();

            if (string.IsNullOrEmpty(eeee) || !LimitList.ContainsKey(eeee))
            {
                MessageBox.Show("重新打印标签时出错:\r\nAPN可能不正确,\r\n不让印,\r\n浪费纸", "错误提示:", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            decimal standard = (Convert.ToDecimal(textBox7.Text)
                - Convert.ToDecimal(LimitList[eeee]["tray_nw"]) * Convert.ToDecimal(LimitList[eeee]["tray_ex_num"])
                - Convert.ToDecimal(LimitList[eeee]["packing_material_nw"]))
                / (Convert.ToDecimal(LimitList[eeee]["peer_nw"]) * Convert.ToDecimal(LimitList[eeee]["tray_include_num"])
                + Convert.ToDecimal(LimitList[eeee]["tray_nw"]));

            if (!decimal.TryParse(textBox7.Text, out decimal weight))
            {
                label13.Text = "Trọng lượng không hợp lệ!";
                return;
            }
            decimal upperLimit = Convert.ToDecimal(LimitList[eeee]["upper"]);
            decimal lowerLimit = Convert.ToDecimal(LimitList[eeee]["lower"]);
            if (weight > upperLimit)
            {
               
                label13.Text = "Trọng lượng vượt quá giới hạn trên! Không thể in tem.";
                label12.Text = "Vui lòng cân lại";
                CleanText(true);
                return;
            }
            if (weight < lowerLimit)
            {          
                label13.Text = "Trọng lượng không đủ! Không thể in tem.";
                label12.Text = "Vui lòng cân lại";
                CleanText(true);
                return;
            }

            bool result = printTag.PrintByTCP(
                ConfigInfo["printer"]["ip"]?.ToString() ?? "",
                9100,
                GetTagContents(
                    LimitList[eeee]["tag_product_name"]?.ToString() ?? "",
                    $"{LimitList[eeee]["apn"]}", //thừa thời gian ở đây $"{LimitList[eeee]["apn"]};{now}"
                    textBox7.Text,
                    LimitList[eeee]["standard"]?.ToString() ?? "",
                    ConfigInfo["printer"]["quality"]?.ToString() ?? "1"
                ));

            if (result)
            {
                string sql = $"INSERT INTO packing_scale VALUES('{LimitList[eeee]["apn"]}', '{LimitList[eeee]["apn"]};{now}', '{sn}', '{textBox7.Text}', '{datetime}');";
                msdbWrite.InsertData(sql);
            }
            else
            {
                MessageBox.Show("打印标签时出错:\r\n反正就是错了", "错误提示:", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            string bartenderFilePath = "F:\\称重标签.btw";
            FileToBarCodePrint(bartenderFilePath, "");
            label12.Text = "Mời in tiếp thùng mới";
            label13.Text = "Vui lòng đặt thùng mới lên cân";
            label12.Visible = true;
            CleanText(true);
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
        /// <summary>
        /// 获取标签内容
        /// </summary>
        /// <param name="spec"> 品名</param>
        /// <param name="apn">二维码</param>
        /// <param name="weight">实重</param>
        /// <param name="weight1">标重</param>
        /// <returns></returns>
        private string GetTagContents(string spec, string apn, string weight, string weight1, string quality = "2")
        {
            string csvContent = $"spec,weight,apn,datetime1,weight1,datetime\n{spec},{weight},{apn},{DateTime.Now:yyMMddHHmmss},{weight1},{DateTime.Now:yyyy/MM/dd HH:mm:ss}";
            //string tagFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "tag.txt");
            string tagFilePath = Path.Combine("F:", "tag.txt");
            File.WriteAllText(tagFilePath, csvContent);

            try
            {
                File.WriteAllText(tagFilePath, csvContent, Encoding.UTF8);
                if (string.IsNullOrEmpty(tagContext))
                {
                    tagContext = ""; 
                    //"^XA^CI28^MD20^LL295^PW591^CW1,E:SIMSUN.FNT" +
                    //    $"^A1N,20,20^FO2,10,E:SIMSUN.FNT^FD{spec}^FS" +
                    //    $"^A1N,20,20^FO2,40,E:SIMSUN.FNT^FD实重:{weight}kg^FS" +
                    //    $"^FO420,5^BQN,2,4^FDLA,{apn}^FS" +
                    //    $"^A1N,20,20^FO2,80,E:SIMSUN.FNT^FD标重:{weight1}±0.2kg^FS" +
                    //    $"^A1N,20,20^FO2,140,E:SIMSUN.FNT^FD日期:{DateTime.Now:yyyy/MM/dd HH:mm:ss}^FS" +
                    //    $"^PQ{quality},0,{quality},Y^XZ";
                }

                return tagContext;
            }
            catch (Exception ex)
            {
                string logPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "print_error.log");
                File.AppendAllText(logPath, $"[{DateTime.Now}] Error writing to tag.txt: {ex.Message}\n");

                return string.Empty;
            }
        }
        private void 数据修改ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string input = Interaction.InputBox("请输入管理密码", "不能随便改", "密码");
            if (!string.IsNullOrEmpty(input))
            {
                string dt = DateTime.Now.ToString("HHmm");
                if (input != DateTime.Now.ToString("HHmm"))
                {
                    MessageBox.Show("密码输入错误\r\n请重试", "密码错误:", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else
                {
                    EditBaseData editData = new();
                    editData.ShowDialog();
                }
            }
            else
            {
                MessageBox.Show("不输密码不得行", "密码错误:", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void button2_Click(object sender, EventArgs e)
        {
            CleanText(true, "");
        }
        private void button5_Click(object sender, EventArgs e)
        {
            ModelContext modelContext = new ModelContext();
            var packingInfo = modelContext.PackingInfo.ToList();
            //List<PackingInfo> packings = new();
            //foreach (var i in packingInfo)
            //{
            //    packings.Add(i as PackingInfo);
            //}
            var xx = (from i in packingInfo where i.id > 100 select i).ToArray();
            Debug.WriteLine(xx);
        }

        private void 已称重ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            weigh childForm = new weigh();
            childForm.ShowDialog();
        }
    }
}

