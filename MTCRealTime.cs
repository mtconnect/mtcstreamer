

namespace MTConnect
{
    using System;
    using System.IO;
    using System.Text;
    using System.Threading;
    using System.Net;
    using System.Windows.Forms;

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
        private int requestTimeout = 10000;
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

            while (true)
            {
                // reset reload event
                reloadEvent.Reset();

                // HTTP web request
                HttpWebRequest request = null;
                // web responce
                WebResponse responce = null;
                // stream for MTConnect downloading
                Stream stream = null;
                // boundary betweeen xml documents
                byte[] boundary = null;
                // length of boundary
                int boundaryLen;

                // read amounts and positions
                int read, todo = 0, offset = 0, pos = 0;
                int start = 0, stop = 0;
                bool body = false;

                // align
                //  1 = searching for xml start
                //  2 = searching for xml end

                try
                {
                    // create request
                    request = (HttpWebRequest)WebRequest.Create(source);
                    request.KeepAlive = false;
                    // set timeout value for the request
                    request.Timeout = requestTimeout;

                    // set connection group name
                    if (useSeparateConnectionGroup)
                        request.ConnectionGroupName = GetHashCode().ToString();

                    // get response
                    responce = request.GetResponse();

                    // check content type
                    string contentType = responce.ContentType;
                    if (contentType.IndexOf("multipart/x-mixed-replace") == -1)
                    {
                        // Notify of error...
                        /// ErrorEventArgs handler...

                        request.Abort();
                        request = null;
                        responce.Close();
                        responce = null;

                        // need to stop ?
                        if (stopEvent.WaitOne(0, true))
                            break;
                        continue;
                    }

                    // get boundary
                    ASCIIEncoding encoding = new ASCIIEncoding();
                    boundary = encoding.GetBytes("\r\n--" + contentType.Substring(contentType.IndexOf("boundary=", 0) + 9));
                    boundaryLen = boundary.Length;

                    // This is just an example header to see if we should bother checking for a 
                    // new chunk after we finish with what we have processed.
                    int headerLen = "Content-type: text/xml\r\nContent-length: 123\r\n\r\n".Length;

                    // get response stream
                    stream = responce.GetResponseStream();
                    int partLength = 0;
                    pos = boundaryLen;

                    // loop
                    while ((!stopEvent.WaitOne(0, true)) && (!reloadEvent.WaitOne(0, true)))
                    {
                        // check total read
                        if (offset > bufSize - readSize)
                            offset = pos = todo = 0;

                        int readLength;
                        if (body && partLength > 0 && partLength - todo > readSize)
                            readLength = partLength - todo;
                        else
                            readLength = readSize;

                        // read next portion from stream
                        if ((read = stream.Read(buffer, offset, readLength)) == 0)
                            throw new ApplicationException();

                        offset += read;
                        todo += read;

                        // increment received bytes counter
                        bytesReceived += read;
                        bool needMoreData = false;

                        while (!needMoreData)
                        {
                            if (!body)
                            {
                                start = ByteArrayUtils.Find(buffer, mimeBoundry, pos, todo);
                                if (start != -1)
                                {
                                    // Parse the headers
                                    string header = encoding.GetString(buffer, pos, start - pos);
                                    string[] headers = header.Split('\n');

                                    // Find the part length.
                                    partLength = 0;
                                    foreach (string s in headers)
                                    {
                                        string[] headerParts = s.Split(':');
                                        if (headerParts[0].ToLower() == "content-length")
                                            partLength = Int32.Parse(headerParts[1]) + boundaryLen;
                                    }

                                    // found XML start and skip leading <cr><nl><cr><nl>
                                    start += 4;
                                    pos = start;
                                    todo = offset - pos;
                                    body = true;
                                }
                                else
                                {
                                    // Get some more data...
                                    needMoreData = true;
                                }
                            }

                            if (body && (partLength > 0 && todo >= partLength) ||
                                        (partLength <= 0 && todo >= boundaryLen))
                            {
                                // Boundary should be right at the end, lets check there first...
                                // Remember to skip back two characters for the final \r\n
                                int end = pos + (partLength - boundaryLen) - 2;
                                stop = ByteArrayUtils.Find(buffer, boundary, end, todo - (end - pos));

                                // OK, didn't find it, lets scan from the beginning. We have a framing
                                // error, but lets try to recover.
                                if (stop == -1)
                                    stop = ByteArrayUtils.Find(buffer, boundary, pos, todo);
                                if (stop != -1)
                                {
                                    // Add two for the \r\n at the end before the boundary.
                                    if (partLength > 0 && (stop - start + 2) != (partLength - boundaryLen))
                                    {
                                        // If we know the part length, then the boundry should be following.
                                        // This is not a major problem but does indicate a framing issue
                                        MessageBox.Show("Possible Framing Error", "Ignore for now",
                                                        MessageBoxButtons.OK, MessageBoxIcon.Exclamation);

                                    }

                                    pos = stop;
                                    todo = offset - pos;

                                    // increment frames counter
                                    framesReceived++;

                                    // notify
                                    if (DataEvent != null)
                                    {
                                        string document = encoding.GetString(buffer, start, stop - start);
                                        // notify client
                                        DataEvent(this, new RealTimeEventArgs(document));
                                    }

                                    // shift array
                                    pos = stop + boundaryLen;
                                    todo = offset - pos;
                                    Array.Copy(buffer, pos, buffer, 0, todo);

                                    offset = todo;
                                    pos = 0;
                                    body = false;
                                    partLength = 0;
                                    needMoreData = todo < headerLen;
                                }
                                else if (partLength > 0)
                                {
                                    MessageBox.Show("Possible Framing Error", "We should always find the boundary",
                                                    MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                                    needMoreData = true;
                                }
                            }
                            else
                            {
                                // Don't have enough data
                                needMoreData = true;
                            }
                        }
                    } 
                }
                catch (WebException exception)
                {
                    // provide information to clients
                    // wait for a while before the next try
                    Thread.Sleep(250);
                }
                catch (ApplicationException)
                {
                    // wait for a while before the next try
                    Thread.Sleep(250);
                }
                catch (Exception exception)
                {
                    Console.WriteLine("Execption occurred: {0}", exception.Message);
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
                    if (responce != null)
                    {
                        responce.Close();
                        responce = null;
                    }
                }

                // need to stop ?
                if (stopEvent.WaitOne(0, true))
                    break;
            }
        }
    }
}
