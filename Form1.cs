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
      String mToolId;
      Uri mUri;

      delegate void SetTextCallback(string text);

      private double[] ComputeOffset(XElement asset, String name)
      {
          double[] values = new double[2];
          values[0] = values[1] = 0.0;
          try
          {
              XNamespace mta = asset.Name.Namespace;
              XElement node = asset.Descendants(mta + name).First();
              if (node != null)
              {
                  values[0] = Convert.ToDouble(node.Attribute("nominal").Value);
                  values[1] = Convert.ToDouble(node.Value);
              }
          }
          catch (Exception)
          {
              Console.WriteLine("Cannot get offset");
          }

          return values;
      }

      private void UpdateToolData(String assetId)
      {
          String uri = "http://" + mUri.Host + ":" + mUri.Port + "/assets/" + assetId;

          // Now we get the actual asset
          XElement asset = XElement.Load(uri);

          // Let's first check if this was our asset
          XNamespace mta = asset.Name.Namespace;
          XElement node = asset.Descendants(mta + "ToolLife").First();

          // Get the tool life
          if (node != null)
              this.toolLife.Text = node.Value;

          // Lets find a few pieces of data and display them specially
          double[] length = ComputeOffset(asset, "OverallToolLength");
          double[] diameter = ComputeOffset(asset, "CuttingDiameterMax");
          Console.WriteLine("Length offset = {0}", length[1] - length[0]);
          Console.WriteLine("Diameter offset = {0}", diameter[1] - diameter[0]);
          Console.WriteLine("Tool Life offset = {0}", this.toolLife.Text);

          this.length.Text = Convert.ToString(length[1]);
          this.lengthOffset.Text = Convert.ToString(length[1] - length[0]);

          this.diameter.Text = Convert.ToString(diameter[1]);
          this.diameterOffset.Text = Convert.ToString(diameter[1] - diameter[0]);
              
          this.textBox2.Text = asset.ToString();

          // Get cutting tool
          node = asset.Descendants(mta + "CuttingTool").First();

          // Don't resend if this is loopback
          if (node.Attribute("deviceUuid").Value != deviceUuid.Text)
          {
              String text = "@ASSET@|" + node.Attribute("assetId").Value + "|CuttingTool|--multiline--XXX\n" + 
                            asset + "\n--multiline--XXX\n";
              adapter.Send(text);
          }
      }

      private void SetText(string text)
      {
          // InvokeRequired required compares the thread ID of the
          // calling thread to the thread ID of the creating thread.
          // If these threads are different, it returns true.
          if (this.textBox1.InvokeRequired)
          {
              SetTextCallback d = new SetTextCallback(SetText);
              this.Invoke(d, new object[] { text });
          }
          else
          { 
              // First lets check for the AssetChanged event...
              XElement root = XElement.Parse(text);
              XNamespace mts = root.Name.Namespace;

              IEnumerable<XElement> changed = 
                  from node in root.Descendants(mts + "AssetChanged")
                  select node;

              // For each asset, get the tool data
              foreach (XElement assetChanged in changed)
              {
                  XAttribute kind = assetChanged.Attribute("assetType");
                  if (assetChanged.Value != "UNAVAILABLE" &&
                      kind != null && kind.Value == "CuttingTool")
                  {
                      mToolId = assetChanged.Value;
                      UpdateToolData(mToolId);

                      this.toolId.Text = mToolId;
                  }
              }

              text = text.Replace("\n", "\r\n");
              this.textBox1.Text = text;
          }
      }

        void PrintXML(object sender, MTConnect.RealTimeEventArgs args)
        {
            SetText(args.document);
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
            String b = url.Text;
            if (!b.EndsWith("/")) b = b + "/";

            mUri = new Uri(b);
            stream.Source = b + "sample?interval=10&path=//DataItem[@type=\"ASSET_CHANGED\"]";
            stream.DataEvent += new MTConnect.RealTimeData.RealTimeEventHandler(PrintXML);
            stream.Start();

            adapter.Port = Convert.ToInt32(adapterPort.Text);
            adapter.Start();
        }

        private void update_Click(object sender, EventArgs e)
        {
            string status;
            if (toolLife.Text != "0")
                status = "USED";
            else
                status = "NEW";
            status += ",MEASURED";

            String text = "@UPDATE_ASSET@|" + mToolId + 
                "|OverallToolLength|" +  length.Text + 
                "|CuttingDiameterMax|" + diameter.Text + 
                "|ToolLife@type=PART_COUNT|" + toolLife.Text + 
                "|CutterStatus|" + status + "\n";
            adapter.Send(text);
        }

        int mNumber = 0;
        private void openFileDialog1_FileOk(object sender, CancelEventArgs e)
        {
            FileStream file = null;
            try
            {
                Console.WriteLine("Got file {0}\n", openFileDialog1.FileName);
                file = new FileStream(openFileDialog1.FileName, FileMode.Open);
                byte[] tool = new byte[file.Length];
                file.Read(tool, 0, (int)file.Length);
                ASCIIEncoding encoder = new ASCIIEncoding();

                // Get the asset id...
                String xml = encoder.GetString(tool, 0, tool.Length);
                XElement root = XElement.Parse(xml);
                String id = root.Attribute("assetId").Value;
                id += Convert.ToSingle(mNumber);
                mNumber += 1;

                root.SetAttributeValue("assetId", id);

                // Create the line
                String text = "@ASSET@|" + id + "|CuttingTool|--multiline--XXX\n" + root.ToString() +
                     "\n--multiline--XXX\n";
                adapter.Send(text);
                UpdateToolData(id);
            }
            catch (Exception)
            {
                Console.WriteLine("Failed to send file {0}", 
                    openFileDialog1.FileName); 
            }

            if (file != null)
                file.Close();
        }

        private void loadFile_Click(object sender, EventArgs e)
        {
            openFileDialog1.ShowDialog();
        }
    }
}
