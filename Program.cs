using System;
using System.Text;
using System.Net;
using System.Net.Sockets;

namespace SocketClient
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Basic TCP Client!");
            StartClient();
        }

        public static void StartClient()
        {
            // 인코밍 데이터를 위한 데이터 버퍼
            byte[] bytes = new byte[1024];

            
            try
            {
                IPHostEntry ipHostInfo = Dns.GetHostEntry(Dns.GetHostName());
                IPAddress ipAddress = ipHostInfo.AddressList[0];
                IPEndPoint remoteEP = new IPEndPoint(ipAddress, 11000);

                Socket sender = new Socket(AddressFamily.InterNetwork,SocketType.Stream,ProtocolType.Tcp);

                try
                {
                    sender.Connect(remoteEP);

                    // null 가능 참조에 대한 역참조 경고
                    Console.WriteLine("Socket connected to {0}",sender.RemoteEndPoint.ToString());
                    
                    byte[] msg = Encoding.ASCII.GetBytes("This is a test<EOF>");

                    int bytesSent = sender.Send(msg);

                    int bytesRec = sender.Receive(bytes);
                    Console.WriteLine("Echoed test = {0}",Encoding.ASCII.GetString(bytes,0,bytesRec));

                    sender.Shutdown(SocketShutdown.Both);
                    sender.Close();
                }
                catch(ArgumentNullException ane)
                {
                    Console.WriteLine("ArgumentNullException : {0}",ane.ToString());
                }
                catch(SocketException se)
                {
                    Console.WriteLine("SocketException : {0}",se.ToString());
                }
                catch(Exception e)
                {
                    Console.WriteLine("Unexpected exception : {0}",e.ToString());
                }
            }
            catch(Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }
    }
}