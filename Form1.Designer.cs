namespace Streamer
{
    partial class Form1
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.startButton = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.url = new System.Windows.Forms.TextBox();
            this.stopButton = new System.Windows.Forms.Button();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.length = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.toolId = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.diameter = new System.Windows.Forms.TextBox();
            this.update = new System.Windows.Forms.Button();
            this.lengthOffset = new System.Windows.Forms.TextBox();
            this.diameterOffset = new System.Windows.Forms.TextBox();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.loadFile = new System.Windows.Forms.Button();
            this.textBox2 = new System.Windows.Forms.TextBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.deviceUuid = new System.Windows.Forms.TextBox();
            this.adapterPort = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.toolLife = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // startButton
            // 
            this.startButton.Location = new System.Drawing.Point(577, 16);
            this.startButton.Name = "startButton";
            this.startButton.Size = new System.Drawing.Size(75, 23);
            this.startButton.TabIndex = 0;
            this.startButton.Text = "Start";
            this.startButton.UseVisualStyleBackColor = true;
            this.startButton.Click += new System.EventHandler(this.startButton_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(36, 29);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(81, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "Incomping URL";
            // 
            // url
            // 
            this.url.Location = new System.Drawing.Point(122, 13);
            this.url.Name = "url";
            this.url.Size = new System.Drawing.Size(264, 20);
            this.url.TabIndex = 3;
            this.url.Text = "http://localhost:5000/device";
            // 
            // stopButton
            // 
            this.stopButton.Location = new System.Drawing.Point(577, 74);
            this.stopButton.Name = "stopButton";
            this.stopButton.Size = new System.Drawing.Size(75, 23);
            this.stopButton.TabIndex = 4;
            this.stopButton.Text = "Stop";
            this.stopButton.UseVisualStyleBackColor = true;
            this.stopButton.Click += new System.EventHandler(this.stopButton_Click);
            // 
            // textBox1
            // 
            this.textBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.textBox1.Location = new System.Drawing.Point(12, 254);
            this.textBox1.Multiline = true;
            this.textBox1.Name = "textBox1";
            this.textBox1.ReadOnly = true;
            this.textBox1.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.textBox1.Size = new System.Drawing.Size(683, 155);
            this.textBox1.TabIndex = 5;
            // 
            // length
            // 
            this.length.Location = new System.Drawing.Point(93, 169);
            this.length.Name = "length";
            this.length.Size = new System.Drawing.Size(176, 20);
            this.length.TabIndex = 6;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(36, 172);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(40, 13);
            this.label2.TabIndex = 7;
            this.label2.Text = "Length";
            // 
            // toolId
            // 
            this.toolId.AutoSize = true;
            this.toolId.Location = new System.Drawing.Point(36, 146);
            this.toolId.Name = "toolId";
            this.toolId.Size = new System.Drawing.Size(40, 13);
            this.toolId.TabIndex = 8;
            this.toolId.Text = "Tool Id";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(20, 73);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(49, 13);
            this.label3.TabIndex = 10;
            this.label3.Text = "Diameter";
            // 
            // diameter
            // 
            this.diameter.Location = new System.Drawing.Point(93, 195);
            this.diameter.Name = "diameter";
            this.diameter.Size = new System.Drawing.Size(176, 20);
            this.diameter.TabIndex = 9;
            // 
            // update
            // 
            this.update.Location = new System.Drawing.Point(500, 50);
            this.update.Name = "update";
            this.update.Size = new System.Drawing.Size(75, 23);
            this.update.TabIndex = 11;
            this.update.Text = "Update";
            this.update.UseVisualStyleBackColor = true;
            this.update.Click += new System.EventHandler(this.update_Click);
            // 
            // lengthOffset
            // 
            this.lengthOffset.Enabled = false;
            this.lengthOffset.Location = new System.Drawing.Point(280, 169);
            this.lengthOffset.Name = "lengthOffset";
            this.lengthOffset.Size = new System.Drawing.Size(176, 20);
            this.lengthOffset.TabIndex = 12;
            // 
            // diameterOffset
            // 
            this.diameterOffset.Enabled = false;
            this.diameterOffset.Location = new System.Drawing.Point(280, 195);
            this.diameterOffset.Name = "diameterOffset";
            this.diameterOffset.Size = new System.Drawing.Size(176, 20);
            this.diameterOffset.TabIndex = 13;
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            this.openFileDialog1.FileOk += new System.ComponentModel.CancelEventHandler(this.openFileDialog1_FileOk);
            // 
            // loadFile
            // 
            this.loadFile.Location = new System.Drawing.Point(577, 45);
            this.loadFile.Name = "loadFile";
            this.loadFile.Size = new System.Drawing.Size(75, 23);
            this.loadFile.TabIndex = 14;
            this.loadFile.Text = "Load File...";
            this.loadFile.UseVisualStyleBackColor = true;
            this.loadFile.Click += new System.EventHandler(this.loadFile_Click);
            // 
            // textBox2
            // 
            this.textBox2.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.textBox2.Location = new System.Drawing.Point(13, 415);
            this.textBox2.Multiline = true;
            this.textBox2.Name = "textBox2";
            this.textBox2.ReadOnly = true;
            this.textBox2.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.textBox2.Size = new System.Drawing.Size(683, 160);
            this.textBox2.TabIndex = 15;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.deviceUuid);
            this.groupBox1.Controls.Add(this.adapterPort);
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.loadFile);
            this.groupBox1.Controls.Add(this.startButton);
            this.groupBox1.Controls.Add(this.stopButton);
            this.groupBox1.Controls.Add(this.url);
            this.groupBox1.Location = new System.Drawing.Point(12, 13);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(683, 106);
            this.groupBox1.TabIndex = 16;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Connection";
            // 
            // deviceUuid
            // 
            this.deviceUuid.Location = new System.Drawing.Point(122, 68);
            this.deviceUuid.Name = "deviceUuid";
            this.deviceUuid.Size = new System.Drawing.Size(264, 20);
            this.deviceUuid.TabIndex = 17;
            this.deviceUuid.Text = "cnc";
            // 
            // adapterPort
            // 
            this.adapterPort.Location = new System.Drawing.Point(122, 40);
            this.adapterPort.Name = "adapterPort";
            this.adapterPort.Size = new System.Drawing.Size(264, 20);
            this.adapterPort.TabIndex = 16;
            this.adapterPort.Text = "7878";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(21, 71);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(88, 13);
            this.label5.TabIndex = 15;
            this.label5.Text = "My Device UUID";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(21, 45);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(66, 13);
            this.label4.TabIndex = 0;
            this.label4.Text = "Adapter Port";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.label6);
            this.groupBox2.Controls.Add(this.toolLife);
            this.groupBox2.Controls.Add(this.update);
            this.groupBox2.Controls.Add(this.label3);
            this.groupBox2.Location = new System.Drawing.Point(12, 125);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(683, 123);
            this.groupBox2.TabIndex = 17;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Update";
            // 
            // toolLife
            // 
            this.toolLife.Location = new System.Drawing.Point(81, 96);
            this.toolLife.Name = "toolLife";
            this.toolLife.Size = new System.Drawing.Size(176, 20);
            this.toolLife.TabIndex = 18;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(20, 99);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(48, 13);
            this.label6.TabIndex = 18;
            this.label6.Text = "Tool Life";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(708, 587);
            this.Controls.Add(this.textBox2);
            this.Controls.Add(this.diameterOffset);
            this.Controls.Add(this.lengthOffset);
            this.Controls.Add(this.diameter);
            this.Controls.Add(this.toolId);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.length);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.groupBox2);
            this.Name = "Form1";
            this.Text = "MTConnect Push Stream";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button startButton;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox url;
        private System.Windows.Forms.Button stopButton;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.TextBox length;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label toolId;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox diameter;
        private System.Windows.Forms.Button update;
        private System.Windows.Forms.TextBox lengthOffset;
        private System.Windows.Forms.TextBox diameterOffset;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.Button loadFile;
        private System.Windows.Forms.TextBox textBox2;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox adapterPort;
        private System.Windows.Forms.TextBox deviceUuid;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox toolLife;
    }
}

