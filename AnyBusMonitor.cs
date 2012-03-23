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
    using System.Xml.Linq;
    using System.Net;
    using System.IO;

    public class AnyBusMonitor
    {
        private MTCAdapter mAdapter;

        // These are the signals from the CNC
        public bool oMATADV { get; set; }
        public bool oMATCHG { get; set; }
        public bool oBFCDM { get; set; }
        public bool oALMAB_B { get; set; } // This should duplicate the machine tool condition...
        public bool oBFCHOP { get; set; }
        public bool oBFCHCL { get; set; }
        public bool ADVFail { get; set; }
        public bool CHGFail { get; set; }

        // Input registers from the Bar Feeder
        public bool iNMCY_B { get; set; }
        public bool iMATADV { get; set; }
        public bool iMATCHG { get; set; }
        public bool iIN24 { get; set; }
        public bool iSPOK { get; set; }
        public bool iBFANML_B { get; set; }
        public bool iTOPCUT { get; set; }
        public bool LoadFail { get; set; }
        public bool ChangeFail { get; set; }

        // Define the data items we're mirroring with these 
        // output variables
        private MTCDataItem mLoad = new MTCDataItem("load_mat");
        private MTCDataItem mChange = new MTCDataItem("change_mat");
        private MTCDataItem mChuck = new MTCDataItem("chuck_state");
        private MTCDataItem mSystem = new MTCDataItem("system");
        private MTCDataItem mLinkMode = new MTCDataItem("bf_link");
        private MTCDataItem mAvail = new MTCDataItem("avail");
        private MTCDataItem mTopCut = new MTCDataItem("top_cut");

        // MTConnect Data Item Values
        public string BFTopCut { get; set; }
        public string BFLoadMaterial { get; set; }
        public string BFChangeMaterial { get; set; }
        public string BFEndOfBar { get; set; }
        public string BFSpindleInterlock { get; set; }
        public string BFStock { get; set; }
        public string BFLength { get; set; }
        public string BFEmpty { get; set; }
        public string BFSystem { get; set; }

        public string Load { get { return mLoad.Value; } }
        public string TopCut { get { return mTopCut.Value; } }
        public string Change { get { return mChange.Value; } }
        public string Chuck { get { return mChuck.Value; } }
        public string System { get { return mSystem.Value; } }
        public string LinkMode { get { return mLinkMode.Value; } }

        public AnyBusMonitor(MTCAdapter adapter)
        {
            mAdapter = adapter;
            mAdapter.AddDataItem(mTopCut);
            mAdapter.AddDataItem(mLoad);
            mAdapter.AddDataItem(mChange);
            mAdapter.AddDataItem(mChuck);
            mAdapter.AddDataItem(mSystem);
            mAdapter.AddDataItem(mLinkMode);
            mAdapter.AddDataItem(mAvail);

            mAvail.Value = "AVAILABLE";

            BFConnectionError("Starting...");

            UpdateDevices();
        }

        public void UpdateDevices()
        {
            // Read Devices from AnyBus...

            // Get states from anybus...
            if (oBFCDM && oALMAB_B)
            {
                if (ADVFail)
                    mLoad.Value = "FAIL";
                else if (CHGFail)
                    mChange.Value = "FAIL";
                else
                {
                    if (iTOPCUT && oMATADV)
                        mTopCut.Value = "COMPLETE";
                    else if (iTOPCUT)
                        mTopCut.Value = "ACTIVE";
                    else
                        mTopCut.Value = "READY";

                    mLoad.Value = oMATADV ? "ACTIVE" : "READY";
                    mChange.Value = oMATCHG ? "ACTIVE" : "READY";
                }   
            }
            else
            {
                mLoad.Value = "NOT_READY";
                mChange.Value = "NOT_READY";
                mTopCut.Value = "NOT_READY";
            }


            if (oBFCHOP && !oBFCHCL)
                mChuck.Value = "OPEN";
            else if (oBFCHCL && !oBFCHOP)
                mChuck.Value = "CLOSED";
            else
                mChuck.Value = "UNLATCHED";

            mSystem.Value = oALMAB_B ? "normal||||" : "fault||||Alarm A or B";
            mLinkMode.Value = oBFCDM ? "ENABLED" : "DISABLED";

            // Send changed data...
            mAdapter.Send();
        }

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
                this.iIN24 = element.Value == "ACTIVE";
                this.BFEndOfBar = element.Value;
            }
            else if (element.Name.LocalName == "TopCut")
            {
                this.iTOPCUT = element.Value == "ACTIVE";;
                this.BFTopCut = element.Value;
            }
            else if (element.Name.LocalName == "SpindleInterlock")
            {
                this.iSPOK = element.Value == "UNLATCHED";
                this.BFSpindleInterlock = element.Value;
            }
            else if (element.Name.LocalName == "RemainingMaterial")
            {
                this.BFStock = element.Value;
            }
            else if (element.Name.LocalName == "BarLength")
            {
                this.BFLength = element.Value;
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
                this.iMATADV = completed;
                this.BFLoadMaterial = node.Value;
                this.LoadFail = failed;
                if (completed)
                {
                    this.oMATADV = false;
                }
            }
            else if (node.Name.LocalName == "ChangeMaterial")
            {
                this.iMATCHG = completed;
                this.BFChangeMaterial = node.Value;
                this.ChangeFail = failed;
                if (completed)
                {
                    this.oMATCHG = false;
                }
            }
            else
            {
                Console.WriteLine("Unknown node: " + node.Name);
            }
        }

        private void HandleCondition(XElement cond)
        {
            bool active = cond.Name.LocalName != "Fault" &&
                    cond.Name.LocalName != "Unavailable";
            if (cond.Attribute("type").Value == "FILL_LEVEL")
            {
                this.BFEmpty = cond.Name.LocalName + " - " + cond.Value;
                this.iNMCY_B = active;
            }
            else 
            {
                this.BFSystem = cond.Name.LocalName + " - " + cond.Value;
                this.iBFANML_B = active;
            }
        }

        public void ParseXML(string text)
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

            // Write out bits to AnyBus
            WriteToAnyBus();
        }

        public void BFConnectionError(string why)
        {
            Console.WriteLine("MTConnect stream just not connected: " + why);

            // Stream is now disconnected... set all DIO signals 
            // to low. Will automatically update on reconnect.

            this.iBFANML_B = false;
            this.iNMCY_B = false;
            this.iMATADV = false;
            this.iMATCHG = false;
            this.iIN24 = false;
            this.iSPOK = false;
            this.iTOPCUT = false;
            this.LoadFail = false;
            this.ChangeFail = false;

            this.BFEndOfBar = "";
            this.BFLoadMaterial = "";
            this.BFSpindleInterlock = "";
            this.BFStock = "";
            this.BFLength = "";
            this.BFChangeMaterial = "";
            this.BFEmpty = "";
            this.BFSystem = "";
            this.BFTopCut = "";

            WriteToAnyBus();
        }

        private UInt32 SetBit(int aIndex, bool aValue)
        {
            UInt32 val = aValue ? (UInt32)1 : (UInt32)0;
            return (UInt32)(val << aIndex);
        }

        private void WriteToAnyBus()
        {
            // Bit order
            // 0 - iIN23, 1 - iIN24, 2 - iBFANML_B, 3 - iMATCHG, 4 - iMATADV
            // 5 - iCUC_B, 6 - iSPOK, 7 - iNMCY, 8 - iNMCY_B, 9 - iMATRET
            UInt32 reg = SetBit(1, iIN24) | SetBit(2, iBFANML_B) |
                SetBit(3, iMATCHG) | SetBit(4, iMATADV) | SetBit(6, iSPOK) |
                SetBit(8, iNMCY_B);
            Console.WriteLine("Input register value: " + Convert.ToString((UInt16) reg, 16));
        }
    }
}
