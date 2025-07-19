using Google.Protobuf.WellKnownTypes;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Management;
using System.Net.Sockets;
using System.Runtime.CompilerServices;
using System.Runtime.Intrinsics.X86;
using System.Text;
using System.Threading.Tasks;
//using Zebra.Sdk.Comm;
using static Mysqlx.Notice.Warning.Types;
using static System.Windows.Forms.LinkLabel;
using BarTender;

namespace ElectronicScale
{
    internal class PrintTag
    {
        private string TagContents; //标签内容,
        internal List<string> GetPrinterList()
        {
            List<string> printerList = new();
            ManagementObjectSearcher searcher = new("SELECT Name FROM Win32_Printer");
            ManagementObjectCollection printerCollection = searcher.Get();
            foreach (var printer in printerCollection)
            {
                printerList.Add((string)printer["Name"]);
            }
            return printerList;
        }
        internal List<string> GetZebraPrinterList()
        {
            List<string> printerList = new();
            //foreach (var printer in printerFilter)
            //{
            //    printerList.Add((string)printer["Name"]);
            //} 
            return printerList;
        }

        internal string GetTag(string spec = "XXOO", string standard = "0", string weight = "0")
        {
            string tagText = "";
            try
            {//获取标签模板内容
                if (TagContents == null || TagContents.Length <= 0)
                {
                    string tagFile = Directory.GetCurrentDirectory() + "\\tag.txt";
                    using (var file = File.Open(tagFile, FileMode.Open))
                    {
                        byte[] tmp = new byte[file.Length];
                        int totalSize = file.Read(tmp, 0, tmp.Length);
                        TagContents = Encoding.UTF8.GetString(tmp);
                    };
                    tagText = TagContents;
                    tagText.Replace("spec", spec);
                    tagText.Replace("standard", standard);
                    tagText.Replace("weight", weight);
                }
                return tagText;
            }
            catch (Exception ex)
            {
                Console.WriteLine("获取标签时出错:" + ex.Message);
                tagText =
                    $"^XA^CI28" +
                    $"^MD15 #浓度" +
                    $"^LL40" + //标签长/高度
                    $"^PW20" + //标签宽度
                    $"^LRY " + //是否反转(Y/N)
                    $"^CW1,E:SIMSUN.FNT" +  //中文字体
                    $"^ FO38,63 ^ A1N,50,50 ^ ^FD{spec} ^ FS" + //品名
                    $"^ FO38,133 ^ A1N,50,50 ^ ^FD标准重量: ^ FS" + //标重固定字符
                    $"^ FO263,138 ^ A0N,50,50 ^ ^FD{standard}±0.2Kg ^ FS" + //标重数值
                    $"^ FO38,204 ^ A1N,50,50 ^ ^FD实际重量: ^ FS" +  //称重固定字符
                    $"^ FO263,202 ^ A0N,50,50 ^ ^FD{weight}Kg ^ FS" + //称重数值
                    $"^ FO38,274 ^ A1N,50,50 ^ ^FD日期: ^ FS" +  //日期
                    $"^ FO138,274 ^ A0N,50,50 ^ ^FD{DateTime.Now.ToString("yyyy/MM/dd H:m:s")} ^ FS" +  //日期
                    $"^ PQ1,0,1,Y ^ XZ";
                //tagText = 
                //    $"SIZE 80 mm,40 mm\n" +
                //    $"GAP 0,0\n" +
                //    $"DIRECTION 1\n" +
                //    $"CLS\n" +
                //    $"TEXT 10,10,\"2\",0,1,1,\"{spec}\"\n" +
                //    $"BARCODE 10,50,\"128\",100,1,0,2,6,\"{apn}\"\n" +
                //    $"PRINT 1";

            }
            return tagText;
        }
        public bool PrintByTCP(string ipAddress, int port, string command)
        {
            TcpClient tcpClient = new TcpClient(ipAddress, port);
            NetworkStream stream = tcpClient.GetStream();
            try
            {
                byte[] buffer = Encoding.UTF8.GetBytes(command);
                Console.WriteLine(buffer.Length);
                stream.Write(buffer, 0, buffer.Length);
                stream.Flush();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"错误信息如下:\r\n{ex.Message}", "打印出错:", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            finally
            {
                if (stream != null)
                {
                    stream.Close();
                    stream = null;
                }
                if (tcpClient != null)
                {
                    tcpClient.Close();
                    tcpClient = null;
                }
            }
            return true;
        }
        internal bool PrintData(string data)
        {
            return false;
        }
        /// <summary>
        /// 返回TSCcode代码
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        internal string get_TSPL_code(string data) {
            // 200dpi:1mm=8dots  300dpi:1mm=11.8dots
            //size m,n  (inch) m纸张宽 n纸张高
            //size m mm,n mm  (mm) m纸张宽 n纸张高
            //size m dot,n dot (Dot) m纸张宽 n纸张高 // 以 Dot 为单位 此条指令仅在 V6.27 及以后版本 Firmware 中支持。

            //GAP 定义两张标签纸中间的间隙高度 参数同Size
            //gap m,n   m=两张纸中间的间隙 n=单张纸间隙偏移量,如果是整张为0

            //DENSITY 浓度0-15 tsc tx310 支持范围:1.5,2,3,4,5,6,7


            return data;
        }
    }

    //public class XxPrinter : Zebra.Sdk.Printer.Discovery.DiscoveredPrinter
    //{
    //    //public 
    //    public override Connection GetConnection()
    //    {
    //        throw new NotImplementedException();
    //    }
    //}

}
