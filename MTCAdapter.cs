using System;

namespace MTConnect
{
    using System.IO;
    using System.Text;
    using System.Threading;
    using System.Net;
    using System.Net.Sockets;
    using System.Windows.Forms;
    using System.Collections;

    public class MTCDataItem
    {
        private String mName;
        private String mValue = "UNAVAILABLE";
        bool mChanged = true;

        public MTCDataItem(String name)
        {
            mName = name;
        }

        public String Value {
            set { 
                if (mValue != value) {
                    mValue = value;
                    mChanged = true;
                }
            }
            get { return mValue; }
        }

        public void ResetChanged()
        {
            mChanged = false;
        }

        public bool Changed {
            get { return mChanged; }
        }

        public override string ToString()
        {
            return mName + "|" + mValue;
        }
    }

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
            PONG = encoder.GetBytes("* PONG 1000\n");
        }

        public void Send(MTCDataItem aDI)
        {
            if (aDI.Changed)
            {
                Send(aDI.ToString());
                aDI.ResetChanged();
            }
        }

        public void Send(String aText)
        {
            DateTime now = DateTime.UtcNow;
            String timestamp = now.ToString("yyyy-MM-dd\\THH:mm:ss.fffffffK|");
            ASCIIEncoding encoder = new ASCIIEncoding();
            String line = timestamp + aText + "\n";
            byte[] message = encoder.GetBytes(line.ToCharArray());
            Console.WriteLine("Sending: " + line);
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
                    // Console.WriteLine("Received PING, sending PONG");
                    aClient.Write(PONG, 0, PONG.Length);
                    aClient.Flush();
                }
            }
        }

        private void HeartbeatClient(object client)
        {
            TcpClient tcpClient = (TcpClient)client;
            NetworkStream clientStream = tcpClient.GetStream();
            clientStream.ReadTimeout = 2000;
            mClients.Add(clientStream);

            byte[] message = new byte[4096];
            ASCIIEncoding encoder = new ASCIIEncoding();
            int length = 0;

            try
            {
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
                        Console.WriteLine("Heartbeat read exception");
                        break;
                    }

                    if (bytesRead == 0)
                    {
                        //the client has disconnected from the server
                        Console.WriteLine("No bytes were read from heartbeat thread");
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
            }
            catch (Exception)
            {
                Console.WriteLine("Error during heartbeat");
            }

            tcpClient.Close();
            mClients.Remove(clientStream);
        }


        private void ListenForClients()
        {
            mListener.Start();
            mRunning = true;

            try
            {
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
            catch (Exception)
            {
                Console.WriteLine("Execption occurred waiting for connection");
                mListener.Stop();
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
            foreach (Object obj in mClients) {
                NetworkStream client = (NetworkStream)obj;
                client.Close();
            }
            mClients.Clear();
        }
    }
}