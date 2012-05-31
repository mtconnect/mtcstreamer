

namespace MTConnect
{
    using System;
    using System.IO;
    using System.Text;
    using System.Threading;
    using System.Net;
    using System.Windows.Forms;
    using System.Xml.Linq;
    using System.Linq;

    internal class ByteArrayUtils
    {
        /// <summary>
        /// Check if the array contains needle on specified position.
        /// </summary>
        /// 
        /// <param name="array">Source array to check for needle.</param>
        /// <param name="needle">Needle we are searching for.</param>
        /// <param name="startIndex">Start index in source array.</param>
        /// 
        /// <returns>Returns <b>true</b> if the source array contains the needle at
        /// the specified index. Otherwise it returns <b>false</b>.</returns>
        /// 
        public static bool Compare(byte[] array, byte[] needle, int startIndex)
        {
            int needleLen = needle.Length;
            // compare
            for (int i = 0, p = startIndex; i < needleLen; i++, p++)
            {
                if (array[p] != needle[i])
                {
                    return false;
                }
            }
            return true;
        }

        /// <summary>
        /// Find subarray in the source array.
        /// </summary>
        /// 
        /// <param name="array">Source array to search for needle.</param>
        /// <param name="needle">Needle we are searching for.</param>
        /// <param name="startIndex">Start index in source array.</param>
        /// <param name="sourceLength">Number of bytes in source array, where the needle is searched for.</param>
        /// 
        /// <returns>Returns starting position of the needle if it was found or <b>-1</b> otherwise.</returns>
        /// 
        public static int Find(byte[] array, byte[] needle, int startIndex, int sourceLength)
        {
            int needleLen = needle.Length;
            int index;

            while (sourceLength >= needleLen)
            {
                // find needle's starting element
                index = Array.IndexOf(array, needle[0], startIndex, sourceLength - needleLen + 1);

                // if we did not find even the first element of the needles, then the search is failed
                if (index == -1)
                    return -1;

                int i, p;
                // check for needle
                for (i = 0, p = index; i < needleLen; i++, p++)
                {
                    if (array[p] != needle[i])
                    {
                        break;
                    }
                }

                if (i == needleLen)
                {
                    // needle was found
                    return index;
                }

                // continue to search for needle
                sourceLength -= (index - startIndex + 1);
                startIndex = index + 1;
            }
            return -1;
        }
    }
    public class RealTimeEventArgs : EventArgs
    {
        public RealTimeEventArgs(string document)
        {
            this.document = document;
        }

        public string document;
    }

    public class RealTimeData
    {
        public delegate void RealTimeEventHandler(object sender, RealTimeEventArgs args);

        public event RealTimeEventHandler RealTimeEvent;

        public void SendRealTimeEvent(string document)
        {
            RealTimeEventArgs args = new RealTimeEventArgs(document);
            RealTimeEvent(this, args);
        }
    }

    public class ErrorArgs : EventArgs
    {
        public ErrorArgs(string why)
        {
            this.why = why;
        }

        public string why;
    }

    public class ConnectionError
    {
        public delegate void ConnectionErrorHandler(object sender, ErrorArgs args);
        public event ConnectionErrorHandler ConnectionEvent;
        public void SendConnectionError(string why)
        {
            ErrorArgs args = new ErrorArgs(why);
            ConnectionEvent(this, args);
        }
    }

    public class MTConnectStream
    {
        // URL for MTConnect stream
        private string source;
        // received frames count
        private int framesReceived;
        // recieved byte count
        private int bytesReceived;
        // use separate HTTP connection group or use default
        private bool useSeparateConnectionGroup = true;
        // timeout value for web request
        private int requestTimeout = 2000;
        private int heartbeatTimeout = 2000;
        // user data associated with the source
        private object userData = null;


        // buffer size used to download MTConnect stream
        private const int bufSize = 2048 * 1024;
        // size of portion to read at once
        private const int readSize = 8 * 1024;

        private Thread thread = null;
        private ManualResetEvent stopEvent = null;
        private ManualResetEvent reloadEvent = null;

        /// <summary>
        /// New frame event.
        /// </summary>
        /// 
        /// <remarks>New MTConnect XML chunk has arrived.</remarks>
        /// 
        public event RealTimeData.RealTimeEventHandler DataEvent;

        // Connection error handler
        public event ConnectionError.ConnectionErrorHandler ConnectionEvent;

        /// <summary>
        /// Use or not separate connection group.
        /// </summary>
        /// 
        /// <remarks>The property indicates to open web request in separate connection group.</remarks>
        /// 
        public bool SeparateConnectionGroup
        {
            get { return useSeparateConnectionGroup; }
            set { useSeparateConnectionGroup = value; }
        }

        /// <summary>
        /// MTConnect source.
        /// </summary>
        /// 
        /// <remarks>URL, which provides MTConnect stream.</remarks>
        /// 
        public string Source
        {
            get { return source; }
            set
            {
                source = value;
                // signal to reload
                if (thread != null)
                    reloadEvent.Set();
            }
        }


        /// <summary>
        /// Received frames count.
        /// </summary>
        /// 
        /// <remarks>Number of frames the MTConnect XML provided from the moment of the last
        /// access to the property.
        /// </remarks>
        /// 
        public int FramesReceived
        {
            get
            {
                int frames = framesReceived;
                framesReceived = 0;
                return frames;
            }
        }

        /// <summary>
        /// Received bytes count.
        /// </summary>
        /// 
        /// <remarks>Number of bytes the MTConnect source provided from the moment of the last
        /// access to the property.
        /// </remarks>
        /// 
        public int BytesReceived
        {
            get
            {
                int bytes = bytesReceived;
                bytesReceived = 0;
                return bytes;
            }
        }

        /// <summary>
        /// User data.
        /// </summary>
        /// 
        /// <remarks>The property allows to associate user data with MTConnect source object.</remarks>
        /// 
        public object UserData
        {
            get { return userData; }
            set { userData = value; }
        }

        /// <summary>
        /// Request timeout value.
        /// </summary>
        /// 
        /// <remarks>The property sets timeout value in milliseconds for web requests.
        /// Default value is 10000 milliseconds.</remarks>
        /// 
        public int RequestTimeout
        {
            get { return requestTimeout; }
            set { requestTimeout = value; }
        }

        public int HeartbeatTimeout
        {
            get { return heartbeatTimeout; }
            set { heartbeatTimeout = value; }
        }

        /// <summary>
        /// State of the MTConnect source.
        /// </summary>
        /// 
        /// <remarks>Current state of MTConnect object.</remarks>
        /// 
        public bool IsRunning
        {
            get
            {
                if (thread != null)
                {
                    // check thread status
                    if (thread.Join(0) == false)
                        return true;

                    // the thread is not running, so free resources
                    Free();
                }
                return false;
            }
        }

        /// <summary>
        /// Initializes a new instance of the MTConnectStream class.
        /// </summary>
        /// 
        public MTConnectStream() { }

        /// <summary>
        /// Initializes a new instance of the MTConnectStream class.
        /// </summary>
        /// 
        /// <param name="source">URL, which provides MTConnect stream.</param>
        /// 
        public MTConnectStream(string source)
        {
            this.source = source;
        }

        /// <summary>
        /// Start an MTConnect source.
        /// </summary>
        /// 
        /// <remarks>Start an MTConnect stream.</remarks>
        /// 
        public void Start()
        {
            if (thread == null)
            {
                // check source
                if ((source == null) || (source == string.Empty))
                    throw new ArgumentException("MTConnect source is not specified");

                framesReceived = 0;
                bytesReceived = 0;

                // create events
                stopEvent = new ManualResetEvent(false);
                reloadEvent = new ManualResetEvent(false);

                // create and start new thread
                thread = new Thread(new ThreadStart(WorkerThread));
                thread.Name = source;
                thread.Start();
            }
        }

        /// <summary>
        /// Signal MTConnect source to stop its work.
        /// </summary>
        /// 
        /// <remarks>Signals MTConnect source to stop its background thread, stop to
        /// provide new frames and free resources.</remarks>
        /// 
        public void SignalToStop()
        {
            // stop thread
            if (thread != null)
            {
                // signal to stop
                stopEvent.Set();
            }
        }

        /// <summary>
        /// Wait for MTConnect source has stopped.
        /// </summary>
        /// 
        /// <remarks>Waits for source stopping after it was signalled to stop using
        /// <see cref="SignalToStop"/> method.</remarks>
        /// 
        public void WaitForStop()
        {
            if (thread != null)
            {
                // wait for thread stop
                thread.Join();

                Free();
            }
        }

        /// <summary>
        /// Stop MTConnect source.
        /// </summary>
        /// 
        /// <remarks>Stops MTConnect source aborting its thread.</remarks>
        /// 
        public void Stop()
        {
            if (this.IsRunning)
            {
                thread.Abort();
                WaitForStop();
            }
        }

        /// <summary>
        /// Free resource.
        /// </summary>
        /// 
        private void Free()
        {
            thread = null;

            // release events
            stopEvent.Close();
            stopEvent = null;
            reloadEvent.Close();
            reloadEvent = null;
        }

        /// <summary>
        /// Worker thread.
        /// </summary>
        /// 
        public void WorkerThread()
        {
            // buffer to read stream
            byte[] buffer = new byte[bufSize];
            byte[] mimeBoundry = new byte[] { 13, 10, 13, 10 };
            char[] crlf = new char[] { '\r', '\n' };

            try
            {
                while (!stopEvent.WaitOne(0, true))
                {
                    // reset reload event
                    reloadEvent.Reset();

                    // HTTP web request
                    HttpWebRequest request = null;
                    // web responce
                    WebResponse response = null;
                    // stream for MTConnect downloading
                    Stream stream = null;
                    // boundary betweeen xml documents
                    byte[] boundary = null;
                    // length of boundary
                    int boundaryLen;

                    // read amounts and positions
                    int read, count = 0;
                    int start = 0, partLength = 0;
                    bool body = false;

                    // align
                    //  1 = searching for xml start
                    //  2 = searching for xml end

                    try
                    {
                        // Perform a current and initialize the callback...
                        Uri b = new Uri(source);
                        string[] pathParts = b.AbsolutePath.Split('/');
                        string device = "";
                        if (pathParts.Length >= 2)
                            device = pathParts[1];
                        string current = "http://" + b.Host + ":" + b.Port + "/" + device
                             + "/current";
                        HttpWebRequest curentRequest = (HttpWebRequest)WebRequest.Create(current);
                        curentRequest.KeepAlive = false;
                        // set timeout value for the request
                        WebResponse curentResponse = curentRequest.GetResponse();
                        StreamReader currentReader = new StreamReader(curentResponse.GetResponseStream());
                        string doc = currentReader.ReadToEnd();
                        XElement currentDoc = XElement.Parse(doc);

                        // Let's first check if this was our asset
                        XNamespace currentNs = currentDoc.Name.Namespace;
                        XElement currentHeader = currentDoc.Descendants(currentNs + "Header").First();
                        String nextSequence = currentHeader.Attribute("nextSequence").Value;

                        // Call the callback with the current data...
                        DataEvent(this, new RealTimeEventArgs(doc));

                        // create request
                        request = (HttpWebRequest)WebRequest.Create(source + "&from=" + nextSequence);
                        request.KeepAlive = false;
                        // set timeout value for the request
                        request.Timeout = requestTimeout;

                        // set connection group name
                        if (useSeparateConnectionGroup)
                            request.ConnectionGroupName = GetHashCode().ToString();

                        // get response
                        response = request.GetResponse();

                        // check content type
                        string contentType = response.ContentType;
                        if (contentType.IndexOf("multipart/x-mixed-replace") == -1)
                        {
                            // Notify of error...
                            /// ErrorEventArgs handler...
                            request.Abort();
                            request = null;
                            response.Close();
                            response = null;

                            // need to stop ?
                            if (stopEvent.WaitOne(0, true))
                                break;
                            continue;
                        }

                        // get boundary
                        ASCIIEncoding encoding = new ASCIIEncoding();
                        boundary = encoding.GetBytes("--" + contentType.Substring(contentType.IndexOf("boundary=", 0) + 9));
                        boundaryLen = boundary.Length;

                        // This is just an example header to see if we should bother checking for a 
                        // new chunk after we finish with what we have processed.
                        int headerLen = "Content-type: text/xml\r\nContent-length: 12345\r\n\r\n".Length;

                        // get response stream
                        stream = response.GetResponseStream();
                        stream.ReadTimeout = heartbeatTimeout;

                        // loop
                        while ((!stopEvent.WaitOne(0, true)) && (!reloadEvent.WaitOne(0, true)))
                        {
                            // Read the remaining size of the buffer or our standard chunk size.
                            int readLength;
                            if (bufSize - count < readSize)
                                readLength = bufSize - count;
                            else
                                readLength = readSize;

                            // read next portion from stream
                            if ((read = stream.Read(buffer, count, readLength)) == 0)
                                throw new ApplicationException();

                            count += read;

                            // increment received bytes counter
                            bytesReceived += read;
                            bool needMoreData;

                            do
                            {
                                // Always assume we need more data...
                                needMoreData = true;
                                if (!body && count - start > headerLen)
                                {
                                    // Find the mime boundry indicating the mime head
                                    int head = ByteArrayUtils.Find(buffer, boundary, start, count - start);
                                    if (head == -1)
                                        Console.WriteLine("Framing error, boundary not found\n");
                                    else
                                    {
                                        // Locate the end of the mime header.
                                        start = head + boundaryLen + 2;
                                        int ind = ByteArrayUtils.Find(buffer, mimeBoundry, start, count - start);
                                        if (ind != -1)
                                        {
                                            // Parse the headers
                                            string header = encoding.GetString(buffer, start, ind - start);
                                            string[] headers = header.Split('\n');

                                            // Find the part length.
                                            partLength = 0;
                                            foreach (string s in headers)
                                            {
                                                string[] headerParts = s.Split(':');
                                                if (headerParts[0].ToLower() == "content-length")
                                                    partLength = Int32.Parse(headerParts[1]); // +boundaryLen;
                                            }

                                            // found XML start and skip leading <cr><nl><cr><nl>
                                            start = ind + 4;
                                            body = true;
                                        }
                                    }
                                }

                                if (body && partLength > 0 && (count - start) >= partLength)
                                {
                                    // increment frames counter
                                    framesReceived++;

                                    // notify
                                    if (DataEvent != null)
                                    {
                                        // Boundary should be right at the end, lets check there first...
                                        // Remember to skip back two characters for the final \r\n
                                        string document = encoding.GetString(buffer, start, partLength - 2);

                                        // notify client
                                        DataEvent(this, new RealTimeEventArgs(document));
                                    }

                                    // shift array
                                    int end = (start + partLength);
                                    if (count - end > 0)
                                        Array.Copy(buffer, end, buffer, 0, count - end);

                                    count = count - end;
                                    start = 0;
                                    body = false;
                                    partLength = 0;
                                    needMoreData = count < headerLen;
                                }
                            } while (!needMoreData);
                        }
                    }
                    catch (WebException exception)
                    {
                        // provide information to clients
                        // wait for a while before the next try
                        ConnectionEvent(this, new ErrorArgs(exception.ToString()));
                        Thread.Sleep(1000);
                    }
                    catch (ApplicationException exception)
                    {
                        // wait for a while before the next try
                        ConnectionEvent(this, new ErrorArgs(exception.ToString()));
                        Thread.Sleep(1000);
                    }
                    catch (Exception exception)
                    {
                        ConnectionEvent(this, new ErrorArgs(exception.ToString()));
                        Console.WriteLine("Execption occurred: {0}\n{1}", exception.Message, exception.StackTrace);
                        Thread.Sleep(1000);
                    }
                    finally
                    {
                        // abort request
                        if (request != null)
                        {
                            request.Abort();
                            request = null;
                        }
                        // close response stream
                        if (stream != null)
                        {
                            stream.Close();
                            stream = null;
                        }
                        // close response
                        if (response != null)
                        {
                            response.Close();
                            response = null;
                        }
                    }
                }
            }

            finally
            {
                ConnectionEvent(this, new ErrorArgs("Stopped"));
                thread = null;
                stopEvent.Close();
                reloadEvent.Close();
            }
        }
    }
}
