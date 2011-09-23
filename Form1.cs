using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Streamer
{
  public partial class Form1 : Form
    {
      MTConnect.MTConnectStream stream = new MTConnect.MTConnectStream();

      delegate void SetTextCallback(string text);

      private void SetText(string text)
      {
          text = text.Replace("\n", "\r\n");
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
            stream.Stop();   
        }

        private void startButton_Click(object sender, EventArgs e)
        {
            stream.Source = url.Text;
            stream.DataEvent += new MTConnect.RealTimeData.RealTimeEventHandler(PrintXML);
            stream.Start();
        }

        private void url_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
