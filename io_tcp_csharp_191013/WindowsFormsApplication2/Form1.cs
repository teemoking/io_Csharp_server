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
using System.Net.Sockets;
using System.Threading;
using System.Collections;
using System.IO;



namespace WindowsFormsApplication2
{
    public partial class MainForm : Form
    {
        private IPAddress serverIp;
        private IPEndPoint serverFullIP;
        private Socket sock=null;
        private Hashtable hatb;
        private Byte[] data = new Byte[1024];
        private IPEndPoint myselfIp;
        public MainForm()
        {
            InitializeComponent();
            this.Clear();
        }

        private void MainFormClosing(object sender, FormClosingEventArgs e)
        {
            DialogResult key = MessageBox.Show("确认要关闭窗口吗？", "确定",
            MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            e.Cancel = (key ==  DialogResult.No);
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }

        private void MainForm_Load(object sender, EventArgs e)
        {

        }

        private void LBox_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
        /***
         *委托 线程间操作对话框
         */
        private delegate void UpdateDelegate(String text, TextBoxBase window);
        private void UpdateMethod(String text,TextBoxBase window)
        {
            window.Text = text;
            window.Show();
        }
        

        public void Clear()
        {
            txtIp.Text = "172.19.255.14";
            txtPort.Text = "2111";
            txtIp.Show();
            txtPort.Show();
            RBox.Text = "";
            LBox.Text = "";
        }

        private void btClear_Click(object sender, EventArgs e)
        {
            this.Clear();
        }


        private void btRun_Click(object sender, EventArgs e)
        {
            try
            {
                if (txtIp.Text != "")
                    serverIp = IPAddress.Parse(txtIp.Text);
                else
                    serverIp = IPAddress.Parse("127.0.0.1");

                serverFullIP = new IPEndPoint(serverIp, int.Parse(txtPort.Text));

                sock = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                sock.Bind(serverFullIP);
               
                hatb = new Hashtable();
                sock.Listen(5);
                myselfIp = serverFullIP;
                sock.BeginAccept(new AsyncCallback(clientAccept),sock);

                LBox.Text = "启动成功" +DateTime.Now;
                LBox.Show();
                btRun.Enabled = false;
            }
            catch (Exception ex)
            {
                LBox.Text = "Exception" + ex.Message;
                LBox.Show();
                btRun.Enabled = true;
            }
            finally 
            {
               
            }
        }
        /*停止按钮，关闭所有链接套接字和监听套接字，释放hashtable的数据*/
        private void btStop_Click(object sender, EventArgs e)
        {
            /*异常跳转到client_accept*/
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
                
                Socket tempsocket = sock;
                sock = null;
                tempsocket.Close();
                tempsocket.Dispose();
               
             /*
              //关闭监听套接字，转到异步accept
                sock.Shutdown(SocketShutdown.Both);
                sock.Close();
                sock.Dispose();
                sock = null;
            */
                btRun.Enabled = true;//启动按钮可以使用   
                LBox.Text = "关闭成功" + DateTime.Now;
                LBox.Show(); 
            }
            catch(Exception ex)
            {
                LBox.Text = "Exception" + ex.Message;
                LBox.Show();
            }
        }
        /*发送按钮，数据不能为空，发送数据后的ip被记录，此后该ip和端口发送来的数据会以另外一种格式解析*/
        private void btSend_Click(object sender, EventArgs e)
        {
            if(RBox.Text != "")
            {
                try
                {
                    IPEndPoint sendIp = new IPEndPoint(IPAddress.Parse(txtIp2.Text),Convert.ToInt32(txtPort2.Text)); 
                    if (hatb.ContainsKey(sendIp))//发送数据的ip，找到对应的socket
                    {
                        myselfIp = sendIp; //记录需要解析协议的ip
                        /*处理输入框中的信息，转成byte[], "11 05 00 00"*/
                        int bytelength = (RBox.Text.Length + 1) / 3;
                        if (bytelength <= 0)
                            bytelength = 1;
                        byte[] sendMessage = new byte[bytelength];
                        stringtobyte(RBox.Text,sendMessage);

                        Socket socketClient =  (Socket)hatb[sendIp];
                        socketClient.Send(sendMessage);
                        RBox.Text = "发送成功" + RBox.Text;
                        RBox.Show();
                    }
                    else
                    {
                        RBox.Text = "发送失败，未连接";
                        RBox.Show();                        
                    }
                }
                catch (Exception ex)
                {
                    RBox.Text = "Exception" + ex.Message;
                    RBox.Show();
                }
            }
        }
        /*异步接受*/
        public void serverRecieve(IAsyncResult socket)
        {
            Socket socketClient = socket.AsyncState as Socket;
            try 
            {
                IPEndPoint clientIp = (IPEndPoint)socketClient.RemoteEndPoint;
                txtIp2.Invoke(new UpdateDelegate(UpdateMethod),new object[]{clientIp.Address.ToString(),txtIp2});
                txtPort2.Invoke(new UpdateDelegate(UpdateMethod), new object[] { clientIp.Port.ToString(),txtPort2 });

                int read = socketClient.EndReceive(socket);
                if (read > 0)
                {
                    string message = null;
                    try
                    {
                        this.Clear();//显示接受到的信息时，清空文本框信息
                    }
                    catch(Exception ex)
                    {
                        RBox.Invoke(new UpdateDelegate(UpdateMethod), new object[] { ex.Message, RBox });
                    }
                    /*接收到的byte[]信息，进行处理，转成“11 05 00 00” */
                    if (myselfIp.Equals(clientIp))//如果是被记录的ip，换种解析方式
                    {
                        string hexmessage = null;
                        foreach (byte bit in data)
                        {
                            char[] bithex = new char[3];
                            stringToHex(bit, bithex);
                            string tem = new string(bithex);
                            hexmessage += tem;
                        }
                        //writeToFile("c:\\io.txt", data);//记录收到的信息
                        //string showstr = null;
                        if (hexmessage.Length > 72)
                            message = hexmessage.Substring(0, 72);
                        else
                            message = hexmessage.Substring(0,hexmessage.Length);
                        writeToFile("c:\\io11.txt", data);
                        writeToFile("c:\\io11.txt", data);
                        writeToFile("c:\\io11.txt", data);
                        writeToFile("c:\\io11.txt", data);
                        writeToFile("c:\\io11.txt", data);
                        writeToFile("c:\\io11.txt", data);
                        //LBox.Invoke(new UpdateDelegate(UpdateMethod), new object[] { showstr, LBox });
                    }
                    else 
                    {
                        message = Encoding.Default.GetString(data, 0, read);
                        //data = new Byte[1024 * 10];//可能会引起内存泄lu
                    }
                    writeToFile("c:\\io2.txt", data);
                    /*显示接收的信息*/
                    string sendStr = message + "\n" + "请输入回复信息" + "-->" +
                        clientIp.Address.ToString() + ":" + clientIp.Port.ToString() + "\r\n";
                    LBox.Invoke(new UpdateDelegate(UpdateMethod), new object[] { sendStr, LBox });
                    
                    byteToClear(data, data.Length);//清空接收缓冲区
                    socketClient.BeginReceive(data, 0, data.Length, 0, new AsyncCallback(serverRecieve), socketClient);
                }
                else//如果接到到信息，read为0，默认连接断开
                {
                    socketClient.Close();
                    if (hatb.ContainsKey(clientIp)) hatb.Remove(clientIp);
                }
            }
            catch(Exception ex)
            {
                String exStr = "Exception" + ex.Message;
                LBox.Invoke(new UpdateDelegate(UpdateMethod),new object[]{exStr,LBox});

            }
 
        }

        public void Handle()
        {
           
        }
        /*异步连接*/
        public void clientAccept(IAsyncResult sock)
        {
            /*error  判断连接是否有效*/
            Socket socketListen = sock.AsyncState as Socket;
            Socket socketClient = null;

            try
            {
                socketClient = socketListen.EndAccept(sock);
                IPEndPoint clientIp = (IPEndPoint)socketClient.RemoteEndPoint;
                txtIp2.Invoke(new UpdateDelegate(UpdateMethod),new object[]{clientIp.Address.ToString(),txtIp2});
                txtPort2.Invoke(new UpdateDelegate(UpdateMethod),new object[]{clientIp.Port.ToString(),txtPort2});
                //添加连接套接字到hashtable，方便连接套接字的添加和删除
                if (!hatb.ContainsKey(clientIp)) hatb.Add(clientIp,socketClient);

                socketClient.BeginReceive(data,0,data.Length,SocketFlags.None,new AsyncCallback(serverRecieve),socketClient);
            }
            catch (Exception ex)
            {
                String exStr = "Exception" + ex.Message;
                LBox.Invoke(new UpdateDelegate(UpdateMethod), new object[] { exStr,LBox });
            }
            finally
            {
                socketListen.BeginAccept(new AsyncCallback(clientAccept), socketListen);
            }
        }
        /*发送消息，改变消息格式  "11 05 00 00 "转成byte[]*/
        public byte[] stringtobyte(string strSrc,byte[] arrDes)
        {
            string strDes = strSrc.ToUpper();
            byte[] arrSrc = Encoding.UTF8.GetBytes(strDes);

            //byte[] arrDes = new byte[(arrSrc.Length + 1) / 3];
            int k = 1;
            for (int i = 0, j = 0; i < arrSrc.Length; ++i)
            {
                if (i % 3 == 2) continue;
                if (i % 3 == 1) k = 1;
                if (i % 3 == 0) k = 16;
                if (arrSrc[i] > 47 && arrSrc[i] < 58)
                {
                    arrDes[j] += (byte)((byte)(arrSrc[i] - (byte)48) * k);
                }
                else if (strDes[i] > 64 && strDes[i] < 71)
                {
                    arrDes[j] += (byte)((byte)(arrSrc[i] - (byte)55) * k);
                }
                if (k == 1) ++j;
            }
            return arrDes;
        }

        /*记录消息到文件中，用ultraedit打开*/
        public void writeToFile(string path,byte[] d)
        {
            string filePath = path;
            try
            {  /*线程安全的读写方式*/
                using(FileStream fs = new FileStream(path,FileMode.OpenOrCreate,
                    FileAccess.ReadWrite,FileShare.ReadWrite))
                {
                    fs.Seek(0,SeekOrigin.End);
                    fs.Write(d,0,d.Length);
                }
            }
            catch(Exception ex)
            {
                LBox.Invoke(new UpdateDelegate(UpdateMethod), new object[] { ex.ToString(), LBox });
            }

        }

        /*清空接收缓冲取的函数*/
        public void byteToClear(byte[] d,int length)
        {
            for (int i = 0; i < length; ++i)
            { 
                d[i] = 0;
            }
        }

        /*将接受到得的信息转换格式，byte[]转成"11 05 00 00"*/
        public void stringToHex(byte bit,char[] bithex)
        {
            UInt32 da =  Convert.ToUInt32(bit);

            char[] str = {'0','1','2','3','4','5','6','7','8'
                             ,'9','A','B','C','D','E','F'};
            for (int i = 3; i >0; --i)
            {
                if (i == 3) 
                { 
                    bithex[i-1] = ' '; 
                    continue; 
                }
                UInt32 tem = da % 16;
                bithex[i-1] = str[tem];
                da /= 16;
            }
        }
    }
}

