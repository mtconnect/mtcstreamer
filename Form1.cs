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

        delegate void SetTextCallback(string text);
        delegate void ErrorCallback(object sender, MTConnect.ErrorArgs args);

        private void HandleEvent(XNode node)
        {
            if (node.GetType() != typeof(XElement))
                return;

            XElement element = (XElement) node;

            // Check each event, for the two interfaces, we'll handle
            // only the complete state, since that is what we're limited 
            // to...
            if (element.Name.LocalName == "LoadMaterial" || element.Name.LocalName == "ChangeMaterial")
            {
                HandleInterface(element);
            }
            else if (element.Name.LocalName == "EndOfBar")
            {
                this.iIN24.Checked = element.Value == "ACTIVE";
                this.endOfBar.Text = element.Value;
            }
            else if (element.Name.LocalName == "SpindleInterlock")
            {
                this.iSPOK.Checked = element.Value == "UNLATCHED";
                this.spindleInterlock.Text = element.Value;
            }
            else
            {
                Console.WriteLine("Unknown node: " + element.Name);
            }
        }

        private void HandleInterface(XElement node)
        {
            bool completed = node.Value == "COMPLETE";
            bool failed = node.Value == "FAIL";
            if (node.Name.LocalName == "LoadMaterial")
            {
                this.iMATADV.Checked = completed;
                this.loadMaterial.Text = node.Value;
                this.mAnyBus.ADVFail = failed;
            }
            else if (node.Name.LocalName == "ChangeMaterial")
            {
                this.iMATCHG.Checked = completed;
                this.changeMaterial.Text = node.Value;
                this.mAnyBus.CHGFail = failed;
            }
            else
            {
                Console.WriteLine("Unknown node: " + node.Name);
            }
            UpdateOutput();
        }

        private void HandleCondition(XElement cond)
        {
            bool active = cond.Name.LocalName != "Fault" &&
                    cond.Name.LocalName != "Unavailable";
            if (cond.Attribute("type").Value == "FILL_LEVEL")
            {
                this.empty.Text = cond.Value;
                this.iNMCY_B.Checked = active;
            }
            else 
            {
                this.condition.Text = cond.Value;
                this.iBFANML_B.Checked = active;
            }
        }

        private void ParseXML(string text)
        {
            // InvokeRequired required compares the thread ID of the
            // calling thread to the thread ID of the creating thread.
            // If these threads are different, it returns true.
            if (this.endOfBar.InvokeRequired)
            {
                SetTextCallback d = new SetTextCallback(ParseXML);
                this.Invoke(d, new object[] { text });
            }
            else
            {
                // Check for changes to the BarFeeder interface data items
                // and update the bits in AnyBus
                XElement barfeed = XElement.Parse(text);
                XNamespace ns = barfeed.Name.Namespace;

                // First check for any Faults...
                IEnumerable<XElement> conditionLists =
                    from node in barfeed.Descendants(ns + "Condition")
                    select node;
                foreach (XElement conditionList in conditionLists)
                {
                    foreach (XNode cond in conditionList.Nodes())
                        HandleCondition((XElement) cond);
                }

                // Now we handle the remaining states...
                IEnumerable<XElement> eventLists =
                    from node in barfeed.Descendants(ns + "Events")
                    select node;
                foreach (XElement eventList in eventLists)
                {
                    foreach (XNode evt in eventList.Nodes())
                        HandleEvent(evt);
                }
            }
        }

        void ReceiveStream(object sender, MTConnect.RealTimeEventArgs args)
        {
            ParseXML(args.document);
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
                // Stream is now disconnected... set all DIO signals 
                // to low. Will automatically update on reconnect.
                this.iBFANML_B.Checked = false;
                this.iNMCY_B.Checked = false;
                this.iMATADV.Checked = false;
                this.iMATCHG.Checked = false;
                this.iIN24.Checked = false;
                this.loadFail.Checked = false;
                this.changeFail.Checked = false;
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

            // Put all checkboxes in base states. need to check if this flashes
            // or if the UI waits to update.
            this.iNMCY_B.Checked = false;
            this.iMATADV.Checked = false;
            this.iMATCHG.Checked = false;
            this.iIN24.Checked = false;

            mUri = new Uri(b);
            stream.Source = b + "sample?interval=0&heartbeat=1000";
            stream.RequestTimeout = 2000;
            stream.DataEvent += new MTConnect.RealTimeData.RealTimeEventHandler(ReceiveStream);
            stream.ConnectionEvent += new MTConnect.ConnectionError.ConnectionErrorHandler(HandleConnectionError);
            stream.Start();

            adapter.Port = Convert.ToInt32(adapterPort.Text);
            adapter.Start();
        }

        // Update output status
        private void UpdateOutput()
        {
            mAnyBus.UpdateDevices(); 
            
            oLinkMode.Text = mAnyBus.LinkMode;
            oLoadMaterial.Text = mAnyBus.Load;
            oChangeMaterial.Text = mAnyBus.Change;
            oChuckState.Text = mAnyBus.Chuck;
            oSystem.Text = mAnyBus.System;
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
    }
}
