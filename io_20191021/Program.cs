/*****************************************
 * date: 2019-10-25
 * e-mail:AK1028430241@gmail.com
 * serverIp: 172.19.255.14
 * serverport:2111
 * compiler : visual studio 2019,win7 x64
 * procedure: alibaba cloud computing
 * description：接受客户端的指令，发送指令给io设备
 * 点击按钮，发送8位的指令数据，io设备回复所有设备状态11位
 * *****************************
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace io_20191021
{
    static class Program
    {
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new ProForm());
        }
    }
}
