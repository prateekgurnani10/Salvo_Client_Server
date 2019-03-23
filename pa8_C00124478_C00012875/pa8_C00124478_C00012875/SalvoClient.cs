using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Net;
using System.Net.Sockets;
using System.IO;
using System.Threading;

//This Client will likely not be used, But I'm keeping it in case we need it later
namespace pa8_C00124478_C00012875
{
    class SalvoClient
    {
        private TcpClient client;

        private StreamReader r;
        private StreamWriter w;

        private bool isConnected;
        public bool Connected { get { return isConnected; } }

        public SalvoClient(String IPAddress, int port)
        {
            client = new TcpClient();
            client.Connect(IPAddress, port);

            HandleCommunication();
        }

        public void HandleCommunication()
        {
            r = new StreamReader(client.GetStream(), Encoding.ASCII);
            w = new StreamWriter(client.GetStream(), Encoding.ASCII);

            isConnected = true;

            String msg = null;
            while (isConnected)
            {
                Console.Write("What is your message: ");
                msg = Console.ReadLine();

                w.WriteLine(msg);
                w.Flush();
            }
        }
    }
}
