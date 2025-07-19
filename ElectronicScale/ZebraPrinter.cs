using System.Net.Sockets;
using System.Text;

namespace ElectronicScale
{
    internal class ZebraPrinter
    {
        internal static bool PrintByTCP(string ipAddress, int port, string command)
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
                Console.WriteLine(ex);
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
        //    internal static bool PrintByEPL(string text, int printerIndex = 0)
        //    {
        //        var printers = UsbDiscoverer.GetZebraUsbPrinters();
        //        if (printers.Count == 0) { MessageBox.Show("斑马打印机未找到", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error); return false; }
        //        if (printerIndex > printers.Count)
        //        {
        //            printerIndex = 0;
        //        }
        //        string printerAddress = printers[printerIndex].Address;
        //        UsbConnection usbConnection = new UsbConnection(printerAddress);
        //        usbConnection.Open();

        //        // 获取打印机实例
        //        var printer = ZebraPrinterFactory.GetInstance(usbConnection);
        //        //PrinterStatus printerStatus = printer.GetCurrentStatus();  检测打印机状态
        //        //if (printerStatus.isPaused)
        //        //{
        //        //    MessageBox.Show("打印机已暂停", "错误", MessageBoxButton.OK, MessageBoxImage.Error); return false;
        //        //}else if (printerStatus.isPaperOut)
        //        //{
        //        //    MessageBox.Show("打印纸已用完", "错误", MessageBoxButton.OK, MessageBoxImage.Error); return false;
        //        //}else if (printerStatus.isHeadTooHot)
        //        //{
        //        //    MessageBox.Show("打印头过热", "错误", MessageBoxButton.OK, MessageBoxImage.Error); return false;
        //        //}else if (printerStatus.isHeadOpen)
        //        //{
        //        //    MessageBox.Show("盖子未关闭", "错误", MessageBoxButton.OK, MessageBoxImage.Error); return false;
        //        //}else if (printerStatus.isRibbonOut)
        //        //{
        //        //    MessageBox.Show("碳带已用完", "错误", MessageBoxButton.OK, MessageBoxImage.Error); return false;
        //        //}
        //        // 设置ZPL命令
        //        string zplCommand = text;

        //        // 发送命令到打印机
        //        printer.SendCommand(zplCommand);
        //        usbConnection.Close();
        //        return true;
        //    }
    }

}
