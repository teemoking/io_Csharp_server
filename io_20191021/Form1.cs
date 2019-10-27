using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Net;
using System.Net.Sockets;

namespace io_20191021
{
    public partial class ProForm : Form
    {
        private MainForm mainForm;
        private Socket socket;
        public ProForm()
        {
            InitializeComponent();
            ProForm_txtIP.Text = "172.19.255.14";
            ProForm_txtPort.Text = "2111";
            ProForm_txtIP.Show();
            ProForm_txtPort.Show();
        }

        private void ProForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            DialogResult key = MessageBox.Show("确认要关闭窗口吗？", "确定",
            MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            e.Cancel = (key == DialogResult.No);
        }

        private void btRun_Click(object sender, EventArgs e)
        {

            try
            {
                if (ProForm_txtIP.Text != "" && ProFrom_Port.Text != "")
                {
                    IPEndPoint serverIp = new IPEndPoint(IPAddress.Parse(ProForm_txtIP.Text),
                        int.Parse(ProForm_txtPort.Text));
                    socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                    socket.Bind(serverIp);
                }
                else 
                {
                    //dialog
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
                //dialog
            }
            mainForm = new MainForm(this, this.socket);
            mainForm.Show();
            this.Hide();
        }
    }
}
