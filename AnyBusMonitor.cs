using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Streamer
{
    using System.IO;
    using System.Text;
    using System.Threading;
    using System.Net;
    using System.Net.Sockets;
    using System.Windows.Forms;
    using System.Collections;
    using MTConnect;

    public class AnyBusMonitor
    {
        private MTCAdapter mAdapter;
        private Thread mAnyBusThread;
        private bool mRunning = false;

        // These are the signals from the CNC
        public bool oMATADV { get; set; }
        public bool oMATCHG { get; set; }
        public bool oBFCDM { get; set; }
        public bool oALMAB_B { get; set; } // This should duplicate the machine tool condition...
        public bool oBFCHOP { get; set; }
        public bool oBFCHCL { get; set; }


        public AnyBusMonitor(MTCAdapter adapter)
        {
            mAdapter = adapter;
        }

        private void PollDevices()
        {
            while (mRunning)
            {
                // Get states from anybus...
                if (oMATADV)
                    mAdapter.Send("loadmat|ACTIVE");
                else
                    mAdapter.Send("loadmat|READY");

                Thread.Sleep(1000);
            }
        }

        public void Start()
        {
            mRunning = true;
            mAnyBusThread = new Thread(new ThreadStart(PollDevices));
            mAnyBusThread.Start();
        }

        public void Stop()
        {
            mRunning = false;
        }

    }
}
