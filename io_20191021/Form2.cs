using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net;
using System.IO;
using System.Net.Sockets;
using System.Collections;

namespace io_20191021
{
    public partial class MainForm : Form
    {
        private ProForm proForm;
        private Socket socketListen;
        private Hashtable hatb;
        private Byte[] data = new Byte[1024];
        private IPEndPoint myselfIp;
        private ArrayList btlist;
        private ArrayList lgtlist;

     
        //委托修改port和ip中的内容
        private delegate void UpdateDelegate(String text, TextBoxBase window);
        //委托改变picturebox控件的状态
        private delegate void lgtandbtDelegate(int reg, int btnum);
        private void UpdateMethod(String text, TextBoxBase window)
        {
            window.Text = text;
            window.Show();
        }
        

        public MainForm(ProForm pForm, Socket sock)
        {
            InitializeComponent();
            proForm = pForm;
            socketListen = sock;
            socketListen.Listen(5);
            hatb = new Hashtable();
            sock.BeginAccept(new AsyncCallback(clientAccept), socketListen);

            //按钮box
            PictureBox[] pbtbox = new PictureBox[] { btoff1, bton1, btoff2, bton2, btoff3, bton3, btoff4, bton4 };
            btlist = new ArrayList(pbtbox);
            //指示灯box
            PictureBox[] plgtbox = new PictureBox[] {lgtoff1,lgton1,lgtoff2,lgton2,lgtoff3,lgton3,lgtoff4,lgton4,
                                                    lgtoff5,lgton5,lgtoff6,lgton6,lgtoff7,lgton7,lgtoff8,lgton8};
            lgtlist = new ArrayList(plgtbox);
        }


        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (MessageBox.Show("确认要关闭窗口吗？", "确认", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                e.Cancel = false;
                System.Environment.Exit(0);
            }
            else {
                e.Cancel = true;
            }
        }
        /*
         * 关闭就抛出异常，跳转到异步接收函数中
         */
        private void btStop_Click(object sender, EventArgs e)
        {
            this.Hide();
            proForm.Show();
            try
            {
                foreach (DictionaryEntry de in hatb)
                {
                    Socket temsocket = (Socket)de.Value;
                    temsocket.Shutdown(SocketShutdown.Both);
                    temsocket.Close();
                    //hatb.Remove(de.Key);
                    /*error  删除后改变集合，无法循环*/
                }

                Socket tempsocket = socketListen;
                socketListen = null;
                tempsocket.Close();
                tempsocket.Dispose();
            }
            catch (Exception ex)
            {
                using (FileStream fs = new FileStream("./io.txt", FileMode.OpenOrCreate,
                                                        FileAccess.ReadWrite, FileShare.ReadWrite))
                {
                    fs.Seek(0, SeekOrigin.End);
                    byte[] data = Encoding.Default.GetBytes(ex.Message);
                    fs.Write(data, 0, data.Length);
                }
            }
        }
        //异步连接
        public void clientAccept(IAsyncResult sock)
        {
            /*error  判断连接是否有效*/
            Socket socketClient = null;

            try
            {
                socketClient = socketListen.EndAccept(sock);
                IPEndPoint clientIp = (IPEndPoint)socketClient.RemoteEndPoint;

                //添加连接套接字到hashtable，方便连接套接字的添加和删除
                if (!hatb.ContainsKey(clientIp)) hatb.Add(clientIp, socketClient);

                socketClient.BeginReceive(data, 0, data.Length, SocketFlags.None, new AsyncCallback(serverRecieve), socketClient);
            }
            catch (Exception ex)
            {
                using (FileStream fs = new FileStream("./io.txt", FileMode.OpenOrCreate,
                                                        FileAccess.ReadWrite, FileShare.ReadWrite))
                {
                    fs.Seek(0, SeekOrigin.End);
                    byte[] data = Encoding.Default.GetBytes(ex.Message);
                    fs.Write(data, 0, data.Length);
                }
            }
            finally
            {
                socketListen.BeginAccept(new AsyncCallback(clientAccept), socketListen);
            }
        }
        //异步接受
        public void serverRecieve(IAsyncResult socket)
        {
            Socket socketClient = socket.AsyncState as Socket;
            try
            {
                IPEndPoint clientIp = (IPEndPoint)socketClient.RemoteEndPoint;

                int read = socketClient.EndReceive(socket);
                if (read > 0)
                {
                    if (clientIp.Equals(myselfIp))//被记录的ip,按照特定格式处理数据
                    {
                        recDateAny(data);
                    }
                    else
                    {
                        if (data[0].Equals('#') && data[0].Equals(data[1]))
                        {
                            ;//忽略其他设备信息
                        }
                        else
                        {
                            //io设备会在连接后发送byte[],长度为50，内容不定
                            if (arrCapicity(data) == 50)
                            {
                                myselfIp = clientIp;
                                MainForm_txtIP.Invoke(new UpdateDelegate(UpdateMethod), new object[] { myselfIp.Address.ToString(), MainForm_txtIP });
                                MainForm_txtPort.Invoke(new UpdateDelegate(UpdateMethod), new object[] { myselfIp.Port.ToString(), MainForm_txtPort });
                            }
                        }
                    }
                    socketClient.BeginReceive(data, 0, data.Length, 0, new AsyncCallback(serverRecieve), socketClient);
                }
                else//如果接到到信息，read为0，默认连接断开
                {
                    socketClient.Close();
                    if (hatb.ContainsKey(clientIp)) hatb.Remove(clientIp);
                }
                byteToClear(data, data.Length);//清空接收缓冲区
            }
            catch (Exception ex)
            {
                using (FileStream fs = new FileStream("./io.txt", FileMode.OpenOrCreate,
                                                        FileAccess.ReadWrite, FileShare.ReadWrite))
                {
                    fs.Seek(0, SeekOrigin.End);
                    byte[] data = Encoding.Default.GetBytes(ex.Message);
                    fs.Write(data, 0, data.Length);
                }
            }

        }
        private void serverSend(byte[] sendMessage)
        {
            try
            {
                if (myselfIp != null && hatb.ContainsKey(myselfIp))//发送数据的ip，找到对应的socket,不能位null
                {
                    Socket socketClient = (Socket)hatb[myselfIp];
                    socketClient.Send(sendMessage);
                }
                else
                {
                    MessageBox.Show("io设备未连接，请连接后重试！");
                }
            }
            catch (Exception ex)
            {
                using (FileStream fs = new FileStream("./io.txt", FileMode.OpenOrCreate,
                                                    FileAccess.ReadWrite, FileShare.ReadWrite))
                {
                    fs.Seek(0, SeekOrigin.End);
                    byte[] data = Encoding.Default.GetBytes(ex.Message);
                    fs.Write(data, 0, data.Length);
                }
            }
        }
        //比较数组中的内容长度，若出现5个连续的0认为数据的终止位
        private int arrCapicity(byte[] capdata)
        {
            int i = 0;
            while (i < capdata.Length)
            {
                int k = i;
                while (k < capdata.Length && capdata[k] == 0 )
                {
                    if ((k - i) > 4) break;
                    k++;
                }
                if ((k - i) > 4) break;
                i++;
            }
            return i;
        }
        private void byteToClear(byte[] d, int length)
        {
            for (int i = 0; i < length; ++i)
            {
                d[i] = 0;
            }
        }
        private bool byteArrCompare(byte[] first,byte[] second) 
        {
            int i = 0;
            if (first.Length == second.Length)
            {
                while (i < first.Length && first[i] == second[i]) { i++; }
            }
            if (i == first.Length) return true;
            return false;
        }

        private void bton1_Click(object sender, EventArgs e)
        {
            bt_handle(1, true);
        }

        private void btoff1_Click(object sender, EventArgs e)
        {
            bt_handle(1, false);
        }

        private void bton2_Click(object sender, EventArgs e)
        {
            bt_handle(2, true);
        }

        private void btoff2_Click(object sender, EventArgs e)
        {
            bt_handle(2, false);
        }

        private void bton3_Click(object sender, EventArgs e)
        {
            bt_handle(3, true);
        }

        private void btoff3_Click(object sender, EventArgs e)
        {
            bt_handle(3, false);
        }

        private void bton4_Click(object sender, EventArgs e)
        {
            bt_handle(4, true);
        }

        private void btoff4_Click(object sender, EventArgs e)
        {
            bt_handle(4,false);
        }
        private void bt_handle(int btnum,bool btstatus) 
        {
            senDateAny(btnum,btstatus);
        }
        private void lgtoffandon(int reg,int btnum) 
        {
            if (reg == 0)
            {
                if (btnum < 5)
                {
                    (btlist[(btnum - 1) * 2] as PictureBox).Show();
                    (btlist[(btnum * 2) - 1] as PictureBox).Hide();
                }
                (lgtlist[(btnum - 1) * 2] as PictureBox).Show();
                (lgtlist[(btnum * 2) - 1] as PictureBox).Hide();
            }
            else if (reg == 1)
            {
                if (btnum < 5)
                {
                    (btlist[(btnum - 1) * 2] as PictureBox).Hide();
                    (btlist[(btnum * 2) - 1] as PictureBox).Show();
                }
                (lgtlist[(btnum - 1) * 2] as PictureBox).Hide();
                (lgtlist[(btnum * 2) - 1] as PictureBox).Show();
            }
        }
        private  void recDateAny(byte[] recdata)
        {
            int capcity = arrCapicity(recdata);
            if (capcity == 11)
            {
                int i = 0;
                byte[] checkdata = new byte[11];
                while (capcity > i) 
                {
                    checkdata[i] = recdata[i]; 
                    i++;    
                }
                //if crc校验 
                if (CRCchecked(checkdata) && checkdata[0] == 0x11) 
                {                    
                    int index = 2;
                    while (index <= 8)//校验第2 4 8 位是否为0 
                    {
                        if (checkdata[index] != 0) return;
                        index = index << 1;
                    }

                    int controlnum = 0;
                    byte[] statusdata = new byte[] { 1, 2, 4, 8 };
                    if (checkdata[1] == 0x42 && checkdata[3] == 0x20) { controlnum += 4; }
                        
                    /**
                     *灯 01 02 04 08
                     * 回复会将所有状态相加 写在第8位
                     * 03,08,00,0f
                     */
                    index = checkdata[7];
                    for (int k = 0; k < 4; ++k)
                    {
                        if( (statusdata[k] & index) == statusdata[k])
                        {
                            Invoke(new lgtandbtDelegate(lgtoffandon), new object[] { 1, controlnum + k+1 });
                        }
                        else 
                        {
                            Invoke(new lgtandbtDelegate(lgtoffandon), new object[] { 0, controlnum + k +1 });
                        }
                    }   
                }
            }
            return;
        }
        private  void senDateAny(int btnum, bool switchkey)
        {

            byte[] sendataout = new byte[] { 0x11, 0x05, 0x00, 0x00, 0xff, 0x00, 0x00,0x00};
          
            if (btnum > 0 && btnum < 5)
            {
                if (switchkey)//目前状态为开
                {
                    sendataout[4] = 0x00;//第5位改为关
                }
                sendataout[3] = (byte)((btnum-1) & 0xff);
                byte[] checkdata = new byte[2];
                CRCcaculate(sendataout,ref checkdata,sendataout.Length);

                sendataout[6] = checkdata[0];
                sendataout[7] = checkdata[1];
                serverSend(sendataout);
            }

        }
        private void CRCcaculate(byte[] predata, ref byte[] checkdata, int length)
        {
            UInt32 reg = 0xffff;
            int i = 0;
            while (i <= (length - 3))
            {
                reg ^= predata[i];
                ++i;
                for (int j = 0; j < 8; ++j)
                {
                    if ((reg & 0x01) == 0)
                    {
                        reg = (reg >> 1);
                    }
                    else
                    {
                        reg = (reg >> 1) ^ 0xa001;
                    }
                }
            }
            checkdata = BitConverter.GetBytes(reg);
        }
        private  bool CRCchecked(byte[] comdata)
        {
            byte[] crcdata = new byte[4];
            int length = comdata.Length;
            CRCcaculate(comdata, ref crcdata, length);

            return (crcdata[0] == comdata[length - 2] && crcdata[1] == comdata[length - 1]) ? true : false;
        }
    }
}
