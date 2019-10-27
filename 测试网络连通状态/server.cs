using System;
using System.Net;
using System.IO;
using System.Text;
using System.Net.Sockets;

namespace m_server
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.Write("IP:");
            string m_ip = Console.ReadLine();//输入服务器的ip
            
            TcpListener m_listener = null;
            IPAddress m_host;
            try
            {
                m_host = IPAddress.Parse(m_ip);
                m_listener = new TcpListener(m_host,2111);//设置监听套接字

                m_listener.Start();//开始监听
                Console.WriteLine("server is listening");
          
                TcpClient m_client = m_listener.AcceptTcpClient();//连接套接字
                StreamReader rs = new StreamReader(m_client.GetStream());
                NetworkStream ws = m_client.GetStream();

                while(true)//读取客户端的数据
                {
                    string data = rs.ReadLine();
					if (data.Length == 0) break;
                    if(data != "")  Console.WriteLine(data);
                }
                
            }
            catch(Exception e)
            {
                m_listener.Stop();
                Console.WriteLine("exception:" + e.Message);
				Console.Read();
            }


        }
    }
}
