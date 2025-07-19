using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using thinger.DataConvertLib;

namespace ElectronicScale
{
    internal class Alarm
    {
        //报警
        internal async Task<bool> send_warn_info(WarnType warnType)
        {
            string command = "";
            switch (warnType)
            {
                case WarnType.Lower:
                    if (Convert.ToBoolean(Form1.ConfigInfo["alarm"]["voicestatus"])==true)
                        command = send_warn_dict["lower"];
                    else
                        command = send_warn_dict["lower1"];
                    break;
                case WarnType.Upper:
                    if (Convert.ToBoolean(Form1.ConfigInfo["alarm"]["voicestatus"])==true)
                        command = send_warn_dict["upper"];
                    else
                        command = send_warn_dict["upper1"];
                    break;
                case WarnType.Normal:
                case WarnType.Reset:
                    command = send_warn_dict["reset"];
                    break;
                case WarnType.Off:
                    command = send_warn_dict["off"];
                    break;
            }

            try
            {
                //Debug.WriteLine(command);
                byte[] b = HexStringToByteArray(command);
                Form1.Warning(b, 0, b.Length);
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }

        }
        private byte[] HexStringToByteArray(string hex)
        {
            if (hex.Length % 2 != 0)
                throw new ArgumentException(hex + " is not a valid hex string!");

            byte[] result = new byte[hex.Length / 2];
            for (int i = 0; i < result.Length; i++)
            {
                result[i] = Convert.ToByte(hex.Substring(i * 2, 2), 16);
            }
            return result;
        }
        internal enum WarnType
        {
            Lower, //下限
            Upper, //上限
            Reset, //复位
            Off,
            Normal //正常
        }
        Dictionary<string, string> send_warn_dict = new Dictionary<string, string> {
        {"reset","010F00000004020100E640"},  //绿灯
        {"off","010F00000004020000E7D0"},  //绿灯
        //010F00000004020000E7D0  //清除所有
        //010F00000004020100E640  //保留第一路开启
        //010F00000004020700E5E0  //下限 1.2.3开启
        //010F00000004020600E470  //下限 2.3开启
        //010F00000004020400E510  //下限 3开启 无声音
        //010F00000004020A00E170  //上限 2.4开启
        //010F00000004020800E010  //上限 4开启 无声音
        {"lower","010F00000004020600E470"},  //黄灯+声音
        {"upper","010F00000004020A00E170"},  //红灯+声音
        {"lower1","010F00000004020400E510"},  //黄灯+无声音
        {"upper1","010F00000004020800E010"},  //红灯+无声音
        };

    }
}
