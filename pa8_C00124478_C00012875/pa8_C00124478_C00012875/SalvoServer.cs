using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.IO;

//This Server will likely not be used, But I'm keeping it in case we need it later
namespace pa8_C00124478_C00012875
{
    class SalvoServer
    {
        private TcpListener server;
        private bool isRunning;

        public SalvoServer(int port)
        {
            server = new TcpListener(IPAddress.Any, port);
            server.Start();

            isRunning = true;

            FindClient();

        }

        public void FindClient()
        {
            while (isRunning)
            {
                TcpClient client = server.AcceptTcpClient();
                Thread t = new Thread(new ParameterizedThreadStart(HandleClient));
                t.Start(client);
            }
        }

        public void HandleClient(object obj)
        {
            TcpClient client = (TcpClient)obj;

            StreamWriter w = new StreamWriter(client.GetStream(), Encoding.ASCII);
            StreamReader r = new StreamReader(client.GetStream(), Encoding.ASCII);

            bool clientConnected = true;
            String msg = null;

            while (clientConnected)
            {
                msg = r.ReadLine();
                
                Console.WriteLine("Here's your message: " + msg);
            }
        }
    }
       
}
