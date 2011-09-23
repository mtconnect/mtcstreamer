

namespace MTConnect
{
	using System;
	using System.IO;
	using System.Text;
	using System.Threading;
	using System.Net;

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
        private const int bufSize = 512 * 1024;
        // size of portion to read at once
        private const int readSize = 1024;

		private Thread	thread = null;
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
				if ( thread != null )
					reloadEvent.Set( );
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
				if ( thread != null )
				{
                    // check thread status
					if ( thread.Join( 0 ) == false )
						return true;

					// the thread is not running, so free resources
					Free( );
				}
				return false;
			}
		}

        /// <summary>
        /// Initializes a new instance of the MTConnectStream class.
        /// </summary>
        /// 
        public MTConnectStream( ) { }

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
        public void Start( )
		{
			if ( thread == null )
			{
                // check source
                if ( ( source == null ) || ( source == string.Empty ) )
                    throw new ArgumentException( "MTConnect source is not specified" );
                
                framesReceived = 0;
				bytesReceived = 0;

				// create events
				stopEvent	= new ManualResetEvent( false );
				reloadEvent	= new ManualResetEvent( false );
				
				// create and start new thread
				thread = new Thread( new ThreadStart( WorkerThread ) );
				thread.Name = source;
				thread.Start( );
			}
		}

        /// <summary>
        /// Signal MTConnect source to stop its work.
        /// </summary>
        /// 
        /// <remarks>Signals MTConnect source to stop its background thread, stop to
        /// provide new frames and free resources.</remarks>
        /// 
        public void SignalToStop( )
		{
			// stop thread
			if ( thread != null )
			{
				// signal to stop
				stopEvent.Set( );
			}
		}

        /// <summary>
        /// Wait for MTConnect source has stopped.
        /// </summary>
        /// 
        /// <remarks>Waits for source stopping after it was signalled to stop using
        /// <see cref="SignalToStop"/> method.</remarks>
        /// 
        public void WaitForStop( )
		{
			if ( thread != null )
			{
				// wait for thread stop
				thread.Join( );

				Free( );
			}
		}

        /// <summary>
        /// Stop MTConnect source.
        /// </summary>
        /// 
        /// <remarks>Stops MTConnect source aborting its thread.</remarks>
        /// 
        public void Stop( )
		{
			if ( this.IsRunning )
			{
				thread.Abort( );
				WaitForStop( );
			}
		}

        /// <summary>
        /// Free resource.
        /// </summary>
        /// 
        private void Free( )
		{
			thread = null;

			// release events
			stopEvent.Close( );
			stopEvent = null;
			reloadEvent.Close( );
			reloadEvent = null;
		}

        /// <summary>
        /// Worker thread.
        /// </summary>
        /// 
        public void WorkerThread( )
		{
            // buffer to read stream
            byte[] buffer = new byte[bufSize];
            byte[] mimeBoundry = new byte[] {13, 10, 13, 10};

			while ( true )
			{
				// reset reload event
				reloadEvent.Reset( );

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
				int read, todo = 0, total = 0, pos = 0, align = 1;
				int start = 0, stop = 0;

				// align
				//  1 = searching for xml start
				//  2 = searching for xml end

				try
				{
					// create request
                    request = (HttpWebRequest) WebRequest.Create( source );
                    request.KeepAlive = false;
                    // set timeout value for the request
                    request.Timeout = requestTimeout;
                    request.Headers.Add("TE: chunked");
					// set connection group name
					if ( useSeparateConnectionGroup )
                        request.ConnectionGroupName = GetHashCode( ).ToString( );
					// get response
                    responce = request.GetResponse( );

					// check content type
                    string contentType = responce.ContentType;
                    if ( contentType.IndexOf( "multipart/x-mixed-replace" ) == -1 )
                    {
                        // Notify of error...
                        /// ErrorEventArgs handler...

                        request.Abort( );
                        request = null;
                        responce.Close( );
                        responce = null;

                        // need to stop ?
                        if ( stopEvent.WaitOne( 0, true ) )
                            break;
                        continue;
                    }

					// get boundary
					ASCIIEncoding encoding = new ASCIIEncoding( );
                    boundary = encoding.GetBytes( "--" + contentType.Substring( contentType.IndexOf( "boundary=", 0 ) + 9 ) );
					boundaryLen = boundary.Length;

					// get response stream
                    stream = responce.GetResponseStream( );

					// loop
					while ( ( !stopEvent.WaitOne( 0, true ) ) && ( !reloadEvent.WaitOne( 0, true ) ) )
					{
						// check total read
						if ( total > bufSize - readSize )
						{
							total = pos = todo = 0;
						}

						// read next portion from stream
						if ( ( read = stream.Read( buffer, total, readSize ) ) == 0 )
							throw new ApplicationException( );

						total += read;
						todo += read;

						// increment received bytes counter
						bytesReceived += read;

                        if (align == 1)
                        {
                            start = ByteArrayUtils.Find(buffer, mimeBoundry, pos, todo);
                            if (start != -1)
                            {
                                // found XML start and skip leading <cr><nl><cr><nl>
                                start += 4;
                                pos = start;
                                todo = total - pos;
                                align = 2;
                            }
                            else
                            {
                                // delimiter not found
                                todo = 3;
                                pos = total - todo;
                            }
                        }

				
						// search for document end
						while ( ( align == 2 ) && ( todo >= boundaryLen ) )
						{
							stop = ByteArrayUtils.Find( buffer, boundary, pos, todo );
							if ( stop != -1 )
							{
								pos		= stop;
								todo	= total - pos;

								// increment frames counter
								framesReceived ++;

								// notify
								if ( DataEvent != null )
								{
                                    string document = encoding.GetString(buffer, start, stop - start);
									// notify client
                                    DataEvent(this, new RealTimeEventArgs(document));
								}

								// shift array
								pos		= stop + boundaryLen;
								todo	= total - pos;
								Array.Copy( buffer, pos, buffer, 0, todo );

								total	= todo;
								pos		= 0;
								align	= 1;
							}
							else
							{
								// boundary not found
								todo	= boundaryLen - 1;
								pos		= total - todo;
							}
						}
					}
				}
				catch ( WebException exception )
				{
                    // provide information to clients
                   // wait for a while before the next try
					Thread.Sleep( 250 );
				}
				catch ( ApplicationException )
				{
					// wait for a while before the next try
					Thread.Sleep( 250 );
				}
				catch (Exception exception)
				{
                    Console.WriteLine("Execption occurred: {0}", exception.Message);
				}
				finally
				{
					// abort request
					if ( request != null)
					{
                        request.Abort( );
                        request = null;
					}
					// close response stream
					if ( stream != null )
					{
						stream.Close( );
						stream = null;
					}
					// close response
					if ( responce != null )
					{
                        responce.Close( );
                        responce = null;
					}
				}

				// need to stop ?
				if ( stopEvent.WaitOne( 0, true ) )
					break;
			}
		}
	}
}
