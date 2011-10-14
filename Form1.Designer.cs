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
            this.SuspendLayout();
            // 
            // startButton
            // 
            this.startButton.Location = new System.Drawing.Point(97, 50);
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
            this.label1.Location = new System.Drawing.Point(40, 16);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(29, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "URL";
            // 
            // url
            // 
            this.url.Location = new System.Drawing.Point(82, 13);
            this.url.Name = "url";
            this.url.Size = new System.Drawing.Size(391, 20);
            this.url.TabIndex = 3;
            this.url.Text = "http://10.211.55.2:5000/";
            // 
            // stopButton
            // 
            this.stopButton.Location = new System.Drawing.Point(224, 50);
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
            this.textBox1.Location = new System.Drawing.Point(12, 237);
            this.textBox1.Multiline = true;
            this.textBox1.Name = "textBox1";
            this.textBox1.ReadOnly = true;
            this.textBox1.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.textBox1.Size = new System.Drawing.Size(473, 172);
            this.textBox1.TabIndex = 5;
            // 
            // length
            // 
            this.length.Location = new System.Drawing.Point(100, 134);
            this.length.Name = "length";
            this.length.Size = new System.Drawing.Size(176, 20);
            this.length.TabIndex = 6;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(43, 137);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(40, 13);
            this.label2.TabIndex = 7;
            this.label2.Text = "Length";
            // 
            // toolId
            // 
            this.toolId.AutoSize = true;
            this.toolId.Location = new System.Drawing.Point(43, 103);
            this.toolId.Name = "toolId";
            this.toolId.Size = new System.Drawing.Size(40, 13);
            this.toolId.TabIndex = 8;
            this.toolId.Text = "Tool Id";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(43, 163);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(49, 13);
            this.label3.TabIndex = 10;
            this.label3.Text = "Diameter";
            // 
            // diameter
            // 
            this.diameter.Location = new System.Drawing.Point(100, 160);
            this.diameter.Name = "diameter";
            this.diameter.Size = new System.Drawing.Size(176, 20);
            this.diameter.TabIndex = 9;
            // 
            // update
            // 
            this.update.Location = new System.Drawing.Point(234, 198);
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
            this.lengthOffset.Location = new System.Drawing.Point(287, 134);
            this.lengthOffset.Name = "lengthOffset";
            this.lengthOffset.Size = new System.Drawing.Size(176, 20);
            this.lengthOffset.TabIndex = 12;
            // 
            // diameterOffset
            // 
            this.diameterOffset.Enabled = false;
            this.diameterOffset.Location = new System.Drawing.Point(287, 160);
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
            this.loadFile.Location = new System.Drawing.Point(348, 50);
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
            this.textBox2.Size = new System.Drawing.Size(473, 160);
            this.textBox2.TabIndex = 15;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(498, 587);
            this.Controls.Add(this.textBox2);
            this.Controls.Add(this.loadFile);
            this.Controls.Add(this.diameterOffset);
            this.Controls.Add(this.lengthOffset);
            this.Controls.Add(this.update);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.diameter);
            this.Controls.Add(this.toolId);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.length);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.stopButton);
            this.Controls.Add(this.url);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.startButton);
            this.Name = "Form1";
            this.Text = "MTConnect Push Stream";
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
    }
}

