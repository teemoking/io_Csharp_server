using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.IO;

namespace client
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.Write("Server IP:");
            string ip = Console.ReadLine();
            try
            {
                TcpClient m_client = new TcpClient();
                m_client.Connect(ip, 2111);
                StreamReader rs = new StreamReader(m_client.GetStream());
                NetworkStream ws = m_client.GetStream();
				
			
                while (true)
                {
                    string str = Console.ReadLine();
					byte[] data  = new byte[20];
					stringtobyte(str,data);
                    if(data.Length > 0)
                        ws.Write(data, 0, data.Length);
                }
            }
            catch(Exception e)
            {
                Console.WriteLine("Exception:" + e.Message);
				Console.Read();
            }
        }
		public static byte[] stringtobyte(string strSrc,byte[] arrDes)
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
    }
}
