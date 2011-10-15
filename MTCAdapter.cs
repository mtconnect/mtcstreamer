using System;

namespace MTConnect
{
    using System;
    using System.IO;
    using System.Text;
    using System.Threading;
    using System.Net;
    using System.Net.Sockets;
    using System.Windows.Forms;
    using System.Collections;

    public class MTCAdapter
    {
        private Thread mListenThread;
        private ArrayList mClients = new ArrayList();
        private bool mRunning = false;
        private TcpListener mListener;
        byte[] PONG;

        private int mPort;
        public int Port
        {
            get { return mPort; }
            set { mPort = value; }
        }


        public MTCAdapter(int aPort = 7878)
        {
            mPort = aPort;
            ASCIIEncoding encoder = new ASCIIEncoding();
            PONG = encoder.GetBytes("* PONG 10000\n");
        }

        public void Send(String aText)
        {
            DateTime now = DateTime.UtcNow;
            String timestamp = now.ToString("yyyy-MM-dd\\THH:mm:ss.fffffffK|");
            ASCIIEncoding encoder = new ASCIIEncoding();
            String line = timestamp + aText;
            byte[] message = encoder.GetBytes(line.ToCharArray());

            foreach (object obj in mClients) {
                NetworkStream client = (NetworkStream) obj;
                lock (client)
                {
                    client.Write(message, 0, message.Length);
                    client.Flush();
                }
            }
        }

        private void Receive(NetworkStream aClient, String aLine)
        {
            if (aLine.StartsWith("* PING"))
            {
                lock (aClient)
                {
                    Console.WriteLine("Received PING, sending PONG");
                    aClient.Write(PONG, 0, PONG.Length);
                    aClient.Flush();
                }
            }
        }

        private void HeartbeatClient(object client)
        {
            TcpClient tcpClient = (TcpClient)client;
            NetworkStream clientStream = tcpClient.GetStream();
            mClients.Add(clientStream);

            byte[] message = new byte[4096];
            ASCIIEncoding encoder = new ASCIIEncoding();
            int length = 0;

            while (mRunning && tcpClient.Connected)
            {
                int bytesRead = 0;

                try
                {
                    //blocks until a client sends a message
                    bytesRead = clientStream.Read(message, length, 4096 - length);
                }
                catch
                {
                    //a socket error has occured
                    break;
                }

                if (bytesRead == 0)
                {
                    //the client has disconnected from the server
                    break;
                }

                // See if we have a line
                int pos = length;
                length += bytesRead;
                int eol = 0;
                for (int i = pos; i < length; i++)
                {
                    if (message[i] == '\n')
                    {
                        String line = encoder.GetString(message, eol, i);
                        Receive(clientStream, line);
                        eol = i + 1;
                    }
                }

                // Remove the lines that have been processed.
                if (eol > 0)
                {
                    length = length - eol;
                    // Shift the message array to remove the lines.
                    if (length > 0)
                        Array.Copy(message, eol, message, 0, length);
                }
            }

            tcpClient.Close();
            mClients.Remove(clientStream);
        }


        private void ListenForClients()
        {
            mListener.Start();
            mRunning = true;

            while (mRunning)
            {
                //blocks until a client has connected to the server
                TcpClient client = mListener.AcceptTcpClient();

                //create a thread to handle communication 
                //with connected client
                Thread clientThread = new Thread(new ParameterizedThreadStart(HeartbeatClient));
                clientThread.Start(client);
            }
        }


        public void Start()
        {

            mListener = new TcpListener(IPAddress.Any, mPort);
            mListenThread = new Thread(new ThreadStart(ListenForClients));
            mListenThread.Start();
        }

        public void Stop()
        {
            mRunning = false;
            mListener.Stop();
        }
    }
}