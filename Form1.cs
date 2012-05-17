using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Xml.Linq;
using System.Net;
using System.IO;

namespace Streamer
{
    public partial class Form1 : Form
    {
        MTConnect.MTConnectStream stream = new MTConnect.MTConnectStream();
        MTConnect.MTCAdapter adapter = new MTConnect.MTCAdapter();
        Uri mUri;
        AnyBusMonitor mAnyBus = null;

        delegate void ReceiveStreamCallback(object sender, MTConnect.RealTimeEventArgs args);
        delegate void ErrorCallback(object sender, MTConnect.ErrorArgs args);

        void ReceiveStream(object sender, MTConnect.RealTimeEventArgs args)
        {
            // InvokeRequired required compares the thread ID of the
            // calling thread to the thread ID of the creating thread.
            // If these threads are different, it returns true.
            if (this.endOfBar.InvokeRequired)
            {
                ReceiveStreamCallback d = new ReceiveStreamCallback(ReceiveStream);
                this.Invoke(d, new object[] { sender, args });
            }
            else
            {
                mAnyBus.ParseXML(args.document);
                UpdateOutput();
            }
        }

        void HandleConnectionError(object sender, MTConnect.ErrorArgs args)
        {
            Console.WriteLine("MTConnect stream just disconnected: " + args.why);
            if (this.endOfBar.InvokeRequired)
            {
                ErrorCallback d = new ErrorCallback(HandleConnectionError);
                this.Invoke(d, new object[] { sender, args });
            }
            else
            {
                mAnyBus.BFConnectionError(args.why);
                UpdateOutput();
            }
        }

        public Form1()
        {
            InitializeComponent();
        }

        private void stopButton_Click(object sender, EventArgs e)
        {
            adapter.Stop();
            stream.Stop();
        }

        private void startButton_Click(object sender, EventArgs e)
        {
            if (mAnyBus == null)
                mAnyBus = new AnyBusMonitor(adapter);

            String b = url.Text;
            if (!b.EndsWith("/")) b = b + "/";

            UpdateOutput();

            int heartbeat = 1000;

            mUri = new Uri(b);
            stream.Source = b + "sample?interval=0&heartbeat=" + heartbeat;
            stream.RequestTimeout = heartbeat * 2;
            stream.HeartbeatTimeout = heartbeat * 2;
            stream.DataEvent += new MTConnect.RealTimeData.RealTimeEventHandler(ReceiveStream);
            stream.ConnectionEvent += new MTConnect.ConnectionError.ConnectionErrorHandler(HandleConnectionError);
            stream.Start();

            adapter.Port = Convert.ToInt32(adapterPort.Text);
            adapter.Heartbeat = heartbeat;
            adapter.Start();
        }

        // Update output status
        private void UpdateOutput()
        {
            mAnyBus.UpdateDevices(); 
            
            // Update output text
            oLinkMode.Text = mAnyBus.LinkMode;
            oLoadMaterial.Text = mAnyBus.Feed;
            oChangeMaterial.Text = mAnyBus.Change;
            oChuckState.Text = mAnyBus.Chuck;
            oSystem.Text = mAnyBus.System;
            

            // Special update for interfaces auto-reset
            oMATADV.Checked = mAnyBus.oMATADV;
            oMATCHG.Checked = mAnyBus.oMATCHG;

            // Update input bits
            iBFANML_B.Checked = mAnyBus.iBFANML_B;
            iNMCY_B.Checked = mAnyBus.iNMCY_B;
            iMATADV.Checked = mAnyBus.iMATADV;
            iMATCHG.Checked = mAnyBus.iMATCHG;
            iIN24.Checked = mAnyBus.iIN24;
            iIN23.Checked = mAnyBus.iIN23;
            iSPOK.Checked = mAnyBus.iSPOK;
            iCUC_B.Checked = mAnyBus.iCUC_B;

            // Update input text
            endOfBar.Text = mAnyBus.BFEndOfBar;
            auxEndOfBar.Text = mAnyBus.BFAuxEndOfBar;
            condition.Text = mAnyBus.BFSystem;
            loadMaterial.Text = mAnyBus.BFMaterialFeed;
            changeMaterial.Text = mAnyBus.BFMaterialChange;
            empty.Text = mAnyBus.BFEmpty;
            spindleInterlock.Text = mAnyBus.BFSpindleInterlock;
            barLength.Text = mAnyBus.BFLength;
            bfMode.Text = mAnyBus.BFControllerMode;
            bfExec.Text = mAnyBus.BFExecution;

            mode.Text = mAnyBus.Mode;
            door.Text = mAnyBus.DoorState;

            iRegister.Text = "0x" + Convert.ToString(mAnyBus.iRegsiter, 16);
            oRegister.Text = "0x" + Convert.ToString(mAnyBus.oRegsiter, 16);
        }

        private void oBFCDM_CheckedChanged(object sender, EventArgs e)
        {
            mAnyBus.oBFCDM = oBFCDM.Checked;
            UpdateOutput();
        }

        private void oALMAB_B_CheckedChanged(object sender, EventArgs e)
        {
            mAnyBus.oALMAB_B = oALMAB_B.Checked;
            UpdateOutput();
        }

        private void oMATCHG_CheckedChanged(object sender, EventArgs e)
        {
            mAnyBus.oMATCHG = oMATCHG.Checked;
            UpdateOutput();
        }

        private void oMATADV_CheckedChanged(object sender, EventArgs e)
        {
            mAnyBus.oMATADV = oMATADV.Checked;
            UpdateOutput();
        }

        private void oBFCHOP_CheckedChanged(object sender, EventArgs e)
        {
            mAnyBus.oBFCHOP = oBFCHOP.Checked;
            UpdateOutput();
        }

        private void oBFCHCL_CheckedChanged(object sender, EventArgs e)
        {
            mAnyBus.oBFCHCL = oBFCHCL.Checked;
            UpdateOutput();
        }

        private void loadFail_CheckedChanged(object sender, EventArgs e)
        {
            this.mAnyBus.ADVFail = loadFail.Checked;
            UpdateOutput();
        }

        private void changeFail_CheckedChanged(object sender, EventArgs e)
        {
            this.mAnyBus.CHGFail = changeFail.Checked;
            UpdateOutput();
        }

        private void controllerMode_CheckedChanged(object sender, EventArgs e)
        {
            this.mAnyBus.ControllerMode = controllerMode.Checked;
            UpdateOutput();
        }

        private void doorOpen_CheckedChanged(object sender, EventArgs e)
        {
            this.mAnyBus.DoorOpen = doorOpen.Checked;
            UpdateOutput();
        }

        private void doorClosed_CheckedChanged(object sender, EventArgs e)
        {
            this.mAnyBus.DoorClosed = doorClosed.Checked;
            UpdateOutput();
        }
    }
}
