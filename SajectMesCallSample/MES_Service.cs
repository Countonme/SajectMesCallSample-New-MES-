#region << 版 本 注 释 >>
/*----------------------------------------------------------------
 * 版权所有 (C) 2024 $Liteon$  保留所有权利。
 * CLR版本：4.0.30319.42000
 * 机器名称：REDEEMER
 * 公司名称：$Liteon$
 * 命名空间：SajectMesCallSample
 * 唯一标识：d3c5dbf7-fec2-4e2f-a6a1-11764649efff
 * 文件名：MES_Service
 * 当前用户域：REDEEMER
 *
 * 创建者：Administrator
 * 电子邮箱：yysvent@163.com
 * 创建时间：2024/1/20 17:02:06
 * 版本：V1.0.0
 * 描述：
 *
 * ----------------------------------------------------------------
 * 版本：V 1.0.1
 * 修改人：Shengbi
 * 时间：2024/1/20 17:02:06
 * 修改说明：新模组上线
 * 修改功能：
 * 
 *----------------------------------------------------------------*/
#endregion << 版 本 注 释 >>
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SajectMesCallSample
{
    class MesCommand
    {

        [DllImport("SajetConnect.dll", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall, EntryPoint = "SajetTransStart")]
        public static extern bool SajetTransStart();

        [DllImport("SajetConnect.dll", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall, EntryPoint = "SajetTransData")]
        public static extern Boolean SajetTransData(int f_iCommandNo, IntPtr f_pData, IntPtr f_pLen);

        [DllImport("SajetConnect.dll", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall, EntryPoint = "SajetTransClose")]
        public static extern bool SajetTransClose();


    }
    public class MES_Service
    {

        private static IntPtr AllocateMemory(int size)
        {
            return Marshal.AllocHGlobal(size);
        }

        private static void FreeMemory(IntPtr ptr)
        {
            Marshal.FreeHGlobal(ptr);
        }
        /// <summary>
        /// 打开MES 连接
        /// </summary>
        /// <returns></returns>
        public static bool MesConnect()
        {
            return MesCommand.SajetTransStart();
        }

        public static bool MesDisconnect()
        {

            return MesCommand.SajetTransClose();

        }
        private static IntPtr mallocIntptr(string strData)
        {
            Byte[] btData = System.Text.Encoding.Default.GetBytes(strData);
            IntPtr m_ptr = Marshal.AllocHGlobal(100);
            Byte[] btZero = new Byte[btData.Length + 1];
            Marshal.Copy(btData, 0, m_ptr, btData.Length);
            return m_ptr;
        }
        private static IntPtr mallocIntptr(int length)
        {
            IntPtr m_ptr = Marshal.AllocHGlobal(4);
            int[] btZero = new int[1];
            btZero[0] = length;
            Marshal.Copy(btZero, 0, m_ptr, 1);
            return m_ptr;
        }

        public static string MesSendSample(int command, string sp_data)
        {
            try
            {
        
                // 轉指針
                IntPtr initData = mallocIntptr(sp_data);
                IntPtr initLength = mallocIntptr(sp_data.Length);
                // transData
                MesCommand.SajetTransData(command, initData, initLength);
                // 獲取返回指針
                string ss = Marshal.PtrToStringAnsi(initData);
                int myi = Marshal.ReadInt32(initLength);
                if (myi > ss.Length)
                {
                    myi = ss.Length;
                }
                string receive = ss.Substring(0, myi);
            
                // 釋放指針
                Marshal.FreeHGlobal(initData);
                Marshal.FreeHGlobal(initLength);
                return receive;
            }
            catch
            {
              return "NG;Send to MES Fail";
            }
        }


    }
}
