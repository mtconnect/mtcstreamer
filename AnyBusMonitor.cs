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
        public bool ADVFail { get; set; }
        public bool CHGFail { get; set; }

        // Define the data items we're mirroring with these 
        // output variables
        private MTCDataItem mLoad = new MTCDataItem("load_mat");
        private MTCDataItem mChange = new MTCDataItem("change_mat");
        private MTCDataItem mChuck = new MTCDataItem("chuck_state");
        private MTCDataItem mSystem = new MTCDataItem("system");
        private MTCDataItem mLinkMode = new MTCDataItem("bf_link");

        public string Load { get { return mLoad.Value; } }
        public string Change { get { return mChange.Value; } }
        public string Chuck { get { return mChuck.Value; } }
        public string System { get { return mSystem.Value; } }
        public string LinkMode { get { return mLinkMode.Value; } }

        public AnyBusMonitor(MTCAdapter adapter)
        {
            mAdapter = adapter;
            ADVFail = CHGFail = false;
        }

        public void UpdateDevices()
        {
            // Get states from anybus...
            if (oBFCDM)
            {
                if (ADVFail || CHGFail)
                {
                    if (ADVFail)
                        mLoad.Value = "FAIL";
                    else if (CHGFail)
                        mChange.Value = "FAIL";
                }
                else
                {
                    mLoad.Value = oMATADV ? "ACTIVE" : "READY";
                    mChange.Value = oMATCHG ? "ACTIVE" : "READY";
                }
                mLinkMode.Value = "ENABLED";
                mSystem.Value = oALMAB_B ? "normal||||" : "fault||||Alarm A or B";

                if (oBFCHOP)
                    mChuck.Value = "OPEN";
                else if (oBFCHCL)
                    mChuck.Value = "CLOSED";
                else
                    mChuck.Value = "UNLATCHED";
            }
            else
            {
                mLoad.Value = "NOT_READY";
                mChange.Value = "NOT_READY";
                mLinkMode.Value = "DISABLED";
            }

            // Send changed data...
            mAdapter.Send(mLoad);
            mAdapter.Send(mChange);
            mAdapter.Send(mChuck);
            mAdapter.Send(mSystem);
            mAdapter.Send(mLinkMode);
        }

        public void Start()
        {
            // ... when we have a real Ethernet IP
            mRunning = true;
        }

        public void Stop()
        {
            mRunning = false;
        }

    }
}
