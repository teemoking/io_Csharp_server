/*****************************************
 * date: 2019-10-13
 * e-mail:AK1028430241@gmail.com
 * serverIp: 172.19.255.14
 * serverport:2111
 * compiler : visual studio 2019,win7 x64
 * procedure: alibaba cloud computing
 * description：接受客户端的指令，以16进制显示，
 * 多线程，向指定客户端发送消息记录ip,以16进制显示，
 * *****************************
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApplication2
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
            Application.Run(new MainForm());
        }
    }
}
