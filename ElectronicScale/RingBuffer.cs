using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElectronicScale
{
    /// <summary>
    /// 环形缓冲区管理器
    /// </summary>
    public class RingBufferManager
    {
        /// <summary>
        /// 存放字节数组
        /// </summary>
        public byte[] Buffer { get; set; }
        /// <summary>
        /// 写入数据长度
        /// </summary>
        public int DataCount { get; set; } = 0;
        /// <summary>
        /// 数据开始索引
        /// </summary>
        public int DataStart { get; set; } = 0;
        /// <summary>
        /// 数据结束索引
        /// </summary>
        public int DataEnd { get; set; } = 0;


        public RingBufferManager(int size)
        {
            Buffer = new byte[size];
        }
        /// <summary>
        /// 写入数据到缓冲区
        /// </summary>
        /// <param name="buffer"></param>
        /// <param name="offset"></param>
        /// <param name="count"></param>
        public void WriteBuffer(byte[] buffer, int offset, int count)
        {
            int reserveCount = Buffer.Length - DataCount; //计算可用空间
            if (reserveCount >= count)
            {
                if (DataEnd + count < Buffer.Length)  //数据没到结束
                {
                    Array.Copy(buffer, offset, Buffer, DataEnd, count);
                    DataEnd += count;
                    DataCount += count;
                }
                else //数据超过结束
                {
                    int overflowIndexLength = (DataEnd + count) - Buffer.Length; //计算超过索引的长度
                    int endPushIndexLength = count - overflowIndexLength; //填充在末尾的数据长度
                    Array.Copy(buffer, offset, Buffer, DataEnd, endPushIndexLength);
                    DataEnd = 0;
                    offset += endPushIndexLength;
                    DataCount += endPushIndexLength;
                    if (overflowIndexLength != 0)
                    {
                        Array.Copy(buffer, offset, Buffer, DataEnd, overflowIndexLength);
                    }
                    DataEnd += overflowIndexLength;
                    DataCount += overflowIndexLength;
                }
            }
            else
            {
                MessageBox.Show("接收缓存溢出!", "溢出提示:", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        /// <summary>
        /// 从缓冲区读取数据
        /// </summary>
        /// <param name="buffer"></param>
        /// <param name="offset"></param>
        /// <param name="count"></param>
        /// <exception cref="Exception"></exception>
        public void ReadBuffer(byte[] buffer, int offset, int count)
        {
            if (count > DataCount)
            {
                throw new Exception("读取的数据长度大于缓冲区长度");
            }
            if (DataStart + count < Buffer.Length)
            {
                Array.Copy(Buffer, DataStart, buffer, offset, count);
            }
            else
            {
                int overflowIndexLength = (DataStart + count) - Buffer.Length; //超出索引长度
                int endPushIndexLength = count - overflowIndexLength;
                Array.Copy(Buffer, DataStart, buffer, offset, endPushIndexLength);
                offset += endPushIndexLength;
                if (overflowIndexLength != 0)
                {
                    Array.Copy(Buffer, 0, buffer, offset, overflowIndexLength);
                }
            }
        }


        public void DeleteBuffer()
        {
            //if (count > DataCount) { throw new Exception("读取的数据长度大于缓冲区长度"); }
            int endIndex = Array.IndexOf(Buffer, (byte)13, DataStart, DataStart + DataCount);
            if (endIndex == -1) return;
            DataStart = endIndex - DataStart + 1;
            DataEnd = endIndex;
            DataCount = DataCount - (endIndex - DataStart + 1);
        }

        public void Clear(int count)
        {
            if (count > DataCount)
            {
                DataCount = 0;
                DataStart = 0;
                DataEnd = 0;
            }
            else
            {
                if (DataStart + count >= Buffer.Length)
                {
                    DataStart = (DataStart + count) - Buffer.Length;
                }
                else
                {
                    DataStart += count;
                }
                DataCount -= count;
            }
        }

        public void Clear()
        {
            DataCount = 0;
            DataStart = 0;
            DataEnd = 0;
            Buffer = new byte[Buffer.Length];
        }
        /// <summary>
        /// 获取当前缓冲区数据长度
        /// </summary>
        /// <returns></returns>
        public int GetDataCount() => DataCount;
        /// <summary>
        /// 获取当前缓冲区剩余空间
        /// </summary>
        /// <returns></returns>
        public int GetReserveCount() => Buffer.Length - DataCount;
        /// <summary>
        /// 获取指定位置的索引
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        /// <exception cref="IndexOutOfRangeException"></exception>
        public byte this[int index]
        {
            get
            {
                if (index >= DataCount)
                {
                    return 0;
                    throw new IndexOutOfRangeException("读取超过索引位置");
                }
                if (DataStart + index < Buffer.Length)
                {
                    return Buffer[index + DataStart];
                }
                else
                {
                    return Buffer[DataStart + index - Buffer.Length];
                }
            }
        }
        public void WriteBuffer(byte[] buffer)
        {
            WriteBuffer(buffer, 0, buffer.Length);
        }
    }
}
