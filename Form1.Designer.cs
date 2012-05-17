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
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.deviceUuid = new System.Windows.Forms.TextBox();
            this.adapterPort = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.stopOperation = new System.Windows.Forms.GroupBox();
            this.door = new System.Windows.Forms.TextBox();
            this.label17 = new System.Windows.Forms.Label();
            this.doorClosed = new System.Windows.Forms.CheckBox();
            this.doorOpen = new System.Windows.Forms.CheckBox();
            this.controllerMode = new System.Windows.Forms.CheckBox();
            this.label21 = new System.Windows.Forms.Label();
            this.oRegister = new System.Windows.Forms.TextBox();
            this.label20 = new System.Windows.Forms.Label();
            this.iRegister = new System.Windows.Forms.TextBox();
            this.mode = new System.Windows.Forms.TextBox();
            this.label19 = new System.Windows.Forms.Label();
            this.changeFail = new System.Windows.Forms.CheckBox();
            this.loadFail = new System.Windows.Forms.CheckBox();
            this.oSystem = new System.Windows.Forms.TextBox();
            this.label15 = new System.Windows.Forms.Label();
            this.oLinkMode = new System.Windows.Forms.TextBox();
            this.label14 = new System.Windows.Forms.Label();
            this.oChuckState = new System.Windows.Forms.TextBox();
            this.label13 = new System.Windows.Forms.Label();
            this.oChangeMaterial = new System.Windows.Forms.TextBox();
            this.label12 = new System.Windows.Forms.Label();
            this.oLoadMaterial = new System.Windows.Forms.TextBox();
            this.label11 = new System.Windows.Forms.Label();
            this.oBFCHCL = new System.Windows.Forms.CheckBox();
            this.oBFCHOP = new System.Windows.Forms.CheckBox();
            this.oMATADV = new System.Windows.Forms.CheckBox();
            this.oMATCHG = new System.Windows.Forms.CheckBox();
            this.oALMAB_B = new System.Windows.Forms.CheckBox();
            this.oBFCDM = new System.Windows.Forms.CheckBox();
            this.label10 = new System.Windows.Forms.Label();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.bfExec = new System.Windows.Forms.TextBox();
            this.label23 = new System.Windows.Forms.Label();
            this.bfMode = new System.Windows.Forms.TextBox();
            this.label22 = new System.Windows.Forms.Label();
            this.auxEndOfBar = new System.Windows.Forms.TextBox();
            this.label18 = new System.Windows.Forms.Label();
            this.iIN23 = new System.Windows.Forms.CheckBox();
            this.barLength = new System.Windows.Forms.TextBox();
            this.label16 = new System.Windows.Forms.Label();
            this.empty = new System.Windows.Forms.TextBox();
            this.spindleInterlock = new System.Windows.Forms.TextBox();
            this.loadMaterial = new System.Windows.Forms.TextBox();
            this.changeMaterial = new System.Windows.Forms.TextBox();
            this.condition = new System.Windows.Forms.TextBox();
            this.endOfBar = new System.Windows.Forms.TextBox();
            this.label9 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.iNMCY_B = new System.Windows.Forms.CheckBox();
            this.iSPOK = new System.Windows.Forms.CheckBox();
            this.iMATADV = new System.Windows.Forms.CheckBox();
            this.iMATCHG = new System.Windows.Forms.CheckBox();
            this.iBFANML_B = new System.Windows.Forms.CheckBox();
            this.iIN24 = new System.Windows.Forms.CheckBox();
            this.iCUCOK = new System.Windows.Forms.CheckBox();
            this.groupBox1.SuspendLayout();
            this.stopOperation.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.SuspendLayout();
            // 
            // startButton
            // 
            this.startButton.Location = new System.Drawing.Point(439, 19);
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
            this.url.Text = "http://10.211.55.2:5000/BarFeeder";
            // 
            // stopButton
            // 
            this.stopButton.Location = new System.Drawing.Point(439, 48);
            this.stopButton.Name = "stopButton";
            this.stopButton.Size = new System.Drawing.Size(75, 23);
            this.stopButton.TabIndex = 4;
            this.stopButton.Text = "Stop";
            this.stopButton.UseVisualStyleBackColor = true;
            this.stopButton.Click += new System.EventHandler(this.stopButton_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.deviceUuid);
            this.groupBox1.Controls.Add(this.adapterPort);
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.startButton);
            this.groupBox1.Controls.Add(this.stopButton);
            this.groupBox1.Controls.Add(this.url);
            this.groupBox1.Location = new System.Drawing.Point(12, 13);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(563, 106);
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
            // stopOperation
            // 
            this.stopOperation.Controls.Add(this.door);
            this.stopOperation.Controls.Add(this.label17);
            this.stopOperation.Controls.Add(this.doorClosed);
            this.stopOperation.Controls.Add(this.doorOpen);
            this.stopOperation.Controls.Add(this.controllerMode);
            this.stopOperation.Controls.Add(this.label21);
            this.stopOperation.Controls.Add(this.oRegister);
            this.stopOperation.Controls.Add(this.label20);
            this.stopOperation.Controls.Add(this.iRegister);
            this.stopOperation.Controls.Add(this.mode);
            this.stopOperation.Controls.Add(this.label19);
            this.stopOperation.Controls.Add(this.changeFail);
            this.stopOperation.Controls.Add(this.loadFail);
            this.stopOperation.Controls.Add(this.oSystem);
            this.stopOperation.Controls.Add(this.label15);
            this.stopOperation.Controls.Add(this.oLinkMode);
            this.stopOperation.Controls.Add(this.label14);
            this.stopOperation.Controls.Add(this.oChuckState);
            this.stopOperation.Controls.Add(this.label13);
            this.stopOperation.Controls.Add(this.oChangeMaterial);
            this.stopOperation.Controls.Add(this.label12);
            this.stopOperation.Controls.Add(this.oLoadMaterial);
            this.stopOperation.Controls.Add(this.label11);
            this.stopOperation.Controls.Add(this.oBFCHCL);
            this.stopOperation.Controls.Add(this.oBFCHOP);
            this.stopOperation.Controls.Add(this.oMATADV);
            this.stopOperation.Controls.Add(this.oMATCHG);
            this.stopOperation.Controls.Add(this.oALMAB_B);
            this.stopOperation.Controls.Add(this.oBFCDM);
            this.stopOperation.Controls.Add(this.label10);
            this.stopOperation.Location = new System.Drawing.Point(12, 125);
            this.stopOperation.Name = "stopOperation";
            this.stopOperation.Size = new System.Drawing.Size(563, 272);
            this.stopOperation.TabIndex = 17;
            this.stopOperation.TabStop = false;
            this.stopOperation.Text = "Machine Tool";
            // 
            // door
            // 
            this.door.Location = new System.Drawing.Point(317, 179);
            this.door.Name = "door";
            this.door.ReadOnly = true;
            this.door.Size = new System.Drawing.Size(226, 20);
            this.door.TabIndex = 39;
            // 
            // label17
            // 
            this.label17.AutoSize = true;
            this.label17.Location = new System.Drawing.Point(212, 181);
            this.label17.Name = "label17";
            this.label17.Size = new System.Drawing.Size(55, 13);
            this.label17.TabIndex = 38;
            this.label17.Text = "DoorState";
            // 
            // doorClosed
            // 
            this.doorClosed.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.doorClosed.Location = new System.Drawing.Point(10, 254);
            this.doorClosed.Name = "doorClosed";
            this.doorClosed.Size = new System.Drawing.Size(175, 17);
            this.doorClosed.TabIndex = 37;
            this.doorClosed.Text = "Door Closed";
            this.doorClosed.UseVisualStyleBackColor = true;
            this.doorClosed.CheckedChanged += new System.EventHandler(this.doorClosed_CheckedChanged);
            // 
            // doorOpen
            // 
            this.doorOpen.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.doorOpen.Location = new System.Drawing.Point(10, 232);
            this.doorOpen.Name = "doorOpen";
            this.doorOpen.Size = new System.Drawing.Size(175, 17);
            this.doorOpen.TabIndex = 36;
            this.doorOpen.Text = "Door Open";
            this.doorOpen.UseVisualStyleBackColor = true;
            this.doorOpen.CheckedChanged += new System.EventHandler(this.doorOpen_CheckedChanged);
            // 
            // controllerMode
            // 
            this.controllerMode.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.controllerMode.Location = new System.Drawing.Point(10, 163);
            this.controllerMode.Name = "controllerMode";
            this.controllerMode.Size = new System.Drawing.Size(175, 17);
            this.controllerMode.TabIndex = 35;
            this.controllerMode.Text = "Automatic Mode";
            this.controllerMode.UseVisualStyleBackColor = true;
            this.controllerMode.CheckedChanged += new System.EventHandler(this.controllerMode_CheckedChanged);
            // 
            // label21
            // 
            this.label21.AutoSize = true;
            this.label21.Location = new System.Drawing.Point(212, 246);
            this.label21.Name = "label21";
            this.label21.Size = new System.Drawing.Size(52, 13);
            this.label21.TabIndex = 34;
            this.label21.Text = "oRegister";
            // 
            // oRegister
            // 
            this.oRegister.Location = new System.Drawing.Point(317, 246);
            this.oRegister.Name = "oRegister";
            this.oRegister.ReadOnly = true;
            this.oRegister.Size = new System.Drawing.Size(226, 20);
            this.oRegister.TabIndex = 33;
            // 
            // label20
            // 
            this.label20.AutoSize = true;
            this.label20.Location = new System.Drawing.Point(212, 220);
            this.label20.Name = "label20";
            this.label20.Size = new System.Drawing.Size(48, 13);
            this.label20.TabIndex = 32;
            this.label20.Text = "iRegister";
            // 
            // iRegister
            // 
            this.iRegister.Location = new System.Drawing.Point(317, 220);
            this.iRegister.Name = "iRegister";
            this.iRegister.ReadOnly = true;
            this.iRegister.Size = new System.Drawing.Size(226, 20);
            this.iRegister.TabIndex = 31;
            // 
            // mode
            // 
            this.mode.Location = new System.Drawing.Point(317, 153);
            this.mode.Name = "mode";
            this.mode.ReadOnly = true;
            this.mode.Size = new System.Drawing.Size(226, 20);
            this.mode.TabIndex = 27;
            // 
            // label19
            // 
            this.label19.AutoSize = true;
            this.label19.Location = new System.Drawing.Point(212, 155);
            this.label19.Name = "label19";
            this.label19.Size = new System.Drawing.Size(78, 13);
            this.label19.TabIndex = 26;
            this.label19.Text = "ControllerMode";
            // 
            // changeFail
            // 
            this.changeFail.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.changeFail.Location = new System.Drawing.Point(10, 209);
            this.changeFail.Name = "changeFail";
            this.changeFail.Size = new System.Drawing.Size(175, 17);
            this.changeFail.TabIndex = 29;
            this.changeFail.Text = "Change Fail";
            this.changeFail.UseVisualStyleBackColor = true;
            this.changeFail.CheckedChanged += new System.EventHandler(this.changeFail_CheckedChanged);
            // 
            // loadFail
            // 
            this.loadFail.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.loadFail.Location = new System.Drawing.Point(10, 186);
            this.loadFail.Name = "loadFail";
            this.loadFail.Size = new System.Drawing.Size(175, 17);
            this.loadFail.TabIndex = 28;
            this.loadFail.Text = "Load Fail";
            this.loadFail.UseVisualStyleBackColor = true;
            this.loadFail.CheckedChanged += new System.EventHandler(this.loadFail_CheckedChanged);
            // 
            // oSystem
            // 
            this.oSystem.Location = new System.Drawing.Point(317, 125);
            this.oSystem.Name = "oSystem";
            this.oSystem.ReadOnly = true;
            this.oSystem.Size = new System.Drawing.Size(226, 20);
            this.oSystem.TabIndex = 27;
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.Location = new System.Drawing.Point(212, 128);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(75, 13);
            this.label15.TabIndex = 26;
            this.label15.Text = "System (Cond)";
            // 
            // oLinkMode
            // 
            this.oLinkMode.Location = new System.Drawing.Point(317, 99);
            this.oLinkMode.Name = "oLinkMode";
            this.oLinkMode.ReadOnly = true;
            this.oLinkMode.Size = new System.Drawing.Size(226, 20);
            this.oLinkMode.TabIndex = 25;
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Location = new System.Drawing.Point(212, 102);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(54, 13);
            this.label14.TabIndex = 24;
            this.label14.Text = "LinkMode";
            // 
            // oChuckState
            // 
            this.oChuckState.Location = new System.Drawing.Point(317, 73);
            this.oChuckState.Name = "oChuckState";
            this.oChuckState.ReadOnly = true;
            this.oChuckState.Size = new System.Drawing.Size(226, 20);
            this.oChuckState.TabIndex = 23;
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(212, 76);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(63, 13);
            this.label13.TabIndex = 22;
            this.label13.Text = "ChuckState";
            // 
            // oChangeMaterial
            // 
            this.oChangeMaterial.Location = new System.Drawing.Point(317, 47);
            this.oChangeMaterial.Name = "oChangeMaterial";
            this.oChangeMaterial.ReadOnly = true;
            this.oChangeMaterial.Size = new System.Drawing.Size(226, 20);
            this.oChangeMaterial.TabIndex = 21;
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(212, 50);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(81, 13);
            this.label12.TabIndex = 20;
            this.label12.Text = "MaterialChange";
            // 
            // oLoadMaterial
            // 
            this.oLoadMaterial.Location = new System.Drawing.Point(317, 21);
            this.oLoadMaterial.Name = "oLoadMaterial";
            this.oLoadMaterial.ReadOnly = true;
            this.oLoadMaterial.Size = new System.Drawing.Size(226, 20);
            this.oLoadMaterial.TabIndex = 19;
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(212, 24);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(68, 13);
            this.label11.TabIndex = 18;
            this.label11.Text = "MaterialFeed";
            // 
            // oBFCHCL
            // 
            this.oBFCHCL.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.oBFCHCL.Location = new System.Drawing.Point(10, 137);
            this.oBFCHCL.Name = "oBFCHCL";
            this.oBFCHCL.Size = new System.Drawing.Size(175, 17);
            this.oBFCHCL.TabIndex = 16;
            this.oBFCHCL.Text = "oBFCHCL - Chuck closed";
            this.oBFCHCL.UseVisualStyleBackColor = true;
            this.oBFCHCL.CheckedChanged += new System.EventHandler(this.oBFCHCL_CheckedChanged);
            // 
            // oBFCHOP
            // 
            this.oBFCHOP.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.oBFCHOP.Location = new System.Drawing.Point(10, 114);
            this.oBFCHOP.Name = "oBFCHOP";
            this.oBFCHOP.Size = new System.Drawing.Size(175, 17);
            this.oBFCHOP.TabIndex = 15;
            this.oBFCHOP.Text = "oBFCHOP - Chuck open";
            this.oBFCHOP.UseVisualStyleBackColor = true;
            this.oBFCHOP.CheckedChanged += new System.EventHandler(this.oBFCHOP_CheckedChanged);
            // 
            // oMATADV
            // 
            this.oMATADV.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.oMATADV.Location = new System.Drawing.Point(10, 91);
            this.oMATADV.Name = "oMATADV";
            this.oMATADV.Size = new System.Drawing.Size(175, 17);
            this.oMATADV.TabIndex = 14;
            this.oMATADV.Text = "oMATADV - Bar advance";
            this.oMATADV.UseVisualStyleBackColor = true;
            this.oMATADV.CheckedChanged += new System.EventHandler(this.oMATADV_CheckedChanged);
            // 
            // oMATCHG
            // 
            this.oMATCHG.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.oMATCHG.Location = new System.Drawing.Point(10, 68);
            this.oMATCHG.Name = "oMATCHG";
            this.oMATCHG.Size = new System.Drawing.Size(175, 17);
            this.oMATCHG.TabIndex = 13;
            this.oMATCHG.Text = "oMATCHG - Bar change";
            this.oMATCHG.UseVisualStyleBackColor = true;
            this.oMATCHG.CheckedChanged += new System.EventHandler(this.oMATCHG_CheckedChanged);
            // 
            // oALMAB_B
            // 
            this.oALMAB_B.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.oALMAB_B.Location = new System.Drawing.Point(10, 47);
            this.oALMAB_B.Name = "oALMAB_B";
            this.oALMAB_B.Size = new System.Drawing.Size(175, 17);
            this.oALMAB_B.TabIndex = 12;
            this.oALMAB_B.Text = "oALMAB_B - Alarm A or B ";
            this.oALMAB_B.UseVisualStyleBackColor = true;
            this.oALMAB_B.CheckedChanged += new System.EventHandler(this.oALMAB_B_CheckedChanged);
            // 
            // oBFCDM
            // 
            this.oBFCDM.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.oBFCDM.Location = new System.Drawing.Point(10, 24);
            this.oBFCDM.Name = "oBFCDM";
            this.oBFCDM.Size = new System.Drawing.Size(175, 17);
            this.oBFCDM.TabIndex = 11;
            this.oBFCDM.Text = "oBFCDM - Machine link mode";
            this.oBFCDM.UseVisualStyleBackColor = true;
            this.oBFCDM.CheckedChanged += new System.EventHandler(this.oBFCDM_CheckedChanged);
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(10, 29);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(0, 13);
            this.label10.TabIndex = 1;
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.iCUCOK);
            this.groupBox3.Controls.Add(this.bfExec);
            this.groupBox3.Controls.Add(this.label23);
            this.groupBox3.Controls.Add(this.bfMode);
            this.groupBox3.Controls.Add(this.label22);
            this.groupBox3.Controls.Add(this.auxEndOfBar);
            this.groupBox3.Controls.Add(this.label18);
            this.groupBox3.Controls.Add(this.iIN23);
            this.groupBox3.Controls.Add(this.barLength);
            this.groupBox3.Controls.Add(this.label16);
            this.groupBox3.Controls.Add(this.empty);
            this.groupBox3.Controls.Add(this.spindleInterlock);
            this.groupBox3.Controls.Add(this.loadMaterial);
            this.groupBox3.Controls.Add(this.changeMaterial);
            this.groupBox3.Controls.Add(this.condition);
            this.groupBox3.Controls.Add(this.endOfBar);
            this.groupBox3.Controls.Add(this.label9);
            this.groupBox3.Controls.Add(this.label8);
            this.groupBox3.Controls.Add(this.label7);
            this.groupBox3.Controls.Add(this.label6);
            this.groupBox3.Controls.Add(this.label3);
            this.groupBox3.Controls.Add(this.label2);
            this.groupBox3.Controls.Add(this.iNMCY_B);
            this.groupBox3.Controls.Add(this.iSPOK);
            this.groupBox3.Controls.Add(this.iMATADV);
            this.groupBox3.Controls.Add(this.iMATCHG);
            this.groupBox3.Controls.Add(this.iBFANML_B);
            this.groupBox3.Controls.Add(this.iIN24);
            this.groupBox3.Location = new System.Drawing.Point(12, 403);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(563, 273);
            this.groupBox3.TabIndex = 18;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Bar Feeder Status";
            // 
            // bfExec
            // 
            this.bfExec.Location = new System.Drawing.Point(317, 241);
            this.bfExec.Name = "bfExec";
            this.bfExec.ReadOnly = true;
            this.bfExec.Size = new System.Drawing.Size(226, 20);
            this.bfExec.TabIndex = 43;
            // 
            // label23
            // 
            this.label23.AutoSize = true;
            this.label23.Location = new System.Drawing.Point(207, 241);
            this.label23.Name = "label23";
            this.label23.Size = new System.Drawing.Size(54, 13);
            this.label23.TabIndex = 42;
            this.label23.Text = "Execution";
            // 
            // bfMode
            // 
            this.bfMode.Location = new System.Drawing.Point(317, 215);
            this.bfMode.Name = "bfMode";
            this.bfMode.ReadOnly = true;
            this.bfMode.Size = new System.Drawing.Size(226, 20);
            this.bfMode.TabIndex = 41;
            // 
            // label22
            // 
            this.label22.AutoSize = true;
            this.label22.Location = new System.Drawing.Point(207, 215);
            this.label22.Name = "label22";
            this.label22.Size = new System.Drawing.Size(78, 13);
            this.label22.TabIndex = 40;
            this.label22.Text = "ControllerMode";
            // 
            // auxEndOfBar
            // 
            this.auxEndOfBar.Location = new System.Drawing.Point(317, 189);
            this.auxEndOfBar.Name = "auxEndOfBar";
            this.auxEndOfBar.ReadOnly = true;
            this.auxEndOfBar.Size = new System.Drawing.Size(226, 20);
            this.auxEndOfBar.TabIndex = 29;
            // 
            // label18
            // 
            this.label18.AutoSize = true;
            this.label18.Location = new System.Drawing.Point(207, 191);
            this.label18.Name = "label18";
            this.label18.Size = new System.Drawing.Size(98, 13);
            this.label18.TabIndex = 28;
            this.label18.Text = "Auxillary End of Bar";
            // 
            // iIN23
            // 
            this.iIN23.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.iIN23.Location = new System.Drawing.Point(5, 157);
            this.iIN23.Name = "iIN23";
            this.iIN23.Size = new System.Drawing.Size(175, 17);
            this.iIN23.TabIndex = 27;
            this.iIN23.Text = "iIN23 - Aux EOB";
            this.iIN23.UseVisualStyleBackColor = true;
            // 
            // barLength
            // 
            this.barLength.Location = new System.Drawing.Point(317, 165);
            this.barLength.Name = "barLength";
            this.barLength.ReadOnly = true;
            this.barLength.Size = new System.Drawing.Size(226, 20);
            this.barLength.TabIndex = 24;
            // 
            // label16
            // 
            this.label16.AutoSize = true;
            this.label16.Location = new System.Drawing.Point(207, 163);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(40, 13);
            this.label16.TabIndex = 23;
            this.label16.Text = "Length";
            // 
            // empty
            // 
            this.empty.Location = new System.Drawing.Point(317, 139);
            this.empty.Name = "empty";
            this.empty.ReadOnly = true;
            this.empty.Size = new System.Drawing.Size(226, 20);
            this.empty.TabIndex = 22;
            // 
            // spindleInterlock
            // 
            this.spindleInterlock.Location = new System.Drawing.Point(317, 115);
            this.spindleInterlock.Name = "spindleInterlock";
            this.spindleInterlock.ReadOnly = true;
            this.spindleInterlock.Size = new System.Drawing.Size(226, 20);
            this.spindleInterlock.TabIndex = 21;
            // 
            // loadMaterial
            // 
            this.loadMaterial.Location = new System.Drawing.Point(317, 92);
            this.loadMaterial.Name = "loadMaterial";
            this.loadMaterial.ReadOnly = true;
            this.loadMaterial.Size = new System.Drawing.Size(226, 20);
            this.loadMaterial.TabIndex = 20;
            // 
            // changeMaterial
            // 
            this.changeMaterial.Location = new System.Drawing.Point(317, 68);
            this.changeMaterial.Name = "changeMaterial";
            this.changeMaterial.ReadOnly = true;
            this.changeMaterial.Size = new System.Drawing.Size(226, 20);
            this.changeMaterial.TabIndex = 19;
            // 
            // condition
            // 
            this.condition.Location = new System.Drawing.Point(317, 45);
            this.condition.Name = "condition";
            this.condition.ReadOnly = true;
            this.condition.Size = new System.Drawing.Size(226, 20);
            this.condition.TabIndex = 18;
            // 
            // endOfBar
            // 
            this.endOfBar.Location = new System.Drawing.Point(317, 22);
            this.endOfBar.Name = "endOfBar";
            this.endOfBar.ReadOnly = true;
            this.endOfBar.Size = new System.Drawing.Size(226, 20);
            this.endOfBar.TabIndex = 17;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(207, 137);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(36, 13);
            this.label9.TabIndex = 16;
            this.label9.Text = "Empty";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(207, 112);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(83, 13);
            this.label8.TabIndex = 15;
            this.label8.Text = "SpindleInterlock";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(207, 22);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(53, 13);
            this.label7.TabIndex = 14;
            this.label7.Text = "EndOfBar";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(207, 43);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(30, 13);
            this.label6.TabIndex = 13;
            this.label6.Text = "Fault";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(207, 66);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(81, 13);
            this.label3.TabIndex = 12;
            this.label3.Text = "MaterialChange";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(207, 89);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(68, 13);
            this.label2.TabIndex = 11;
            this.label2.Text = "MaterialFeed";
            // 
            // iNMCY_B
            // 
            this.iNMCY_B.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.iNMCY_B.Location = new System.Drawing.Point(5, 133);
            this.iNMCY_B.Name = "iNMCY_B";
            this.iNMCY_B.Size = new System.Drawing.Size(175, 17);
            this.iNMCY_B.TabIndex = 10;
            this.iNMCY_B.Text = "iNMCY_B - Bar feeder empty";
            this.iNMCY_B.UseVisualStyleBackColor = true;
            // 
            // iSPOK
            // 
            this.iSPOK.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.iSPOK.Location = new System.Drawing.Point(5, 110);
            this.iSPOK.Name = "iSPOK";
            this.iSPOK.Size = new System.Drawing.Size(175, 17);
            this.iSPOK.TabIndex = 9;
            this.iSPOK.Text = "iSPOK - Spindle interlock";
            this.iSPOK.UseVisualStyleBackColor = true;
            // 
            // iMATADV
            // 
            this.iMATADV.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.iMATADV.Location = new System.Drawing.Point(5, 87);
            this.iMATADV.Name = "iMATADV";
            this.iMATADV.Size = new System.Drawing.Size(175, 17);
            this.iMATADV.TabIndex = 8;
            this.iMATADV.Text = "iMATADV - Bar advance finish";
            this.iMATADV.UseVisualStyleBackColor = true;
            // 
            // iMATCHG
            // 
            this.iMATCHG.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.iMATCHG.Location = new System.Drawing.Point(5, 64);
            this.iMATCHG.Name = "iMATCHG";
            this.iMATCHG.Size = new System.Drawing.Size(175, 17);
            this.iMATCHG.TabIndex = 7;
            this.iMATCHG.Text = "iMATCHG - Bar change finish";
            this.iMATCHG.UseVisualStyleBackColor = true;
            // 
            // iBFANML_B
            // 
            this.iBFANML_B.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.iBFANML_B.Location = new System.Drawing.Point(5, 43);
            this.iBFANML_B.Name = "iBFANML_B";
            this.iBFANML_B.Size = new System.Drawing.Size(175, 17);
            this.iBFANML_B.TabIndex = 6;
            this.iBFANML_B.Text = "iBFANML_B - Bar feeder error";
            this.iBFANML_B.UseVisualStyleBackColor = true;
            // 
            // iIN24
            // 
            this.iIN24.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.iIN24.Location = new System.Drawing.Point(5, 20);
            this.iIN24.Name = "iIN24";
            this.iIN24.Size = new System.Drawing.Size(175, 17);
            this.iIN24.TabIndex = 5;
            this.iIN24.Text = "iIN24 - End of Bar";
            this.iIN24.UseVisualStyleBackColor = true;
            // 
            // iCUCOK
            // 
            this.iCUCOK.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.iCUCOK.Location = new System.Drawing.Point(5, 180);
            this.iCUCOK.Name = "iCUCOK";
            this.iCUCOK.Size = new System.Drawing.Size(175, 17);
            this.iCUCOK.TabIndex = 44;
            this.iCUCOK.Text = "iCUCOK - Manual Chuck Inlk";
            this.iCUCOK.UseVisualStyleBackColor = true;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(593, 688);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.stopOperation);
            this.Name = "Form1";
            this.Text = "MTConnect Push Stream";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.stopOperation.ResumeLayout(false);
            this.stopOperation.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button startButton;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox url;
        private System.Windows.Forms.Button stopButton;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox stopOperation;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox adapterPort;
        private System.Windows.Forms.TextBox deviceUuid;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.CheckBox iIN24;
        private System.Windows.Forms.CheckBox iBFANML_B;
        private System.Windows.Forms.CheckBox iMATCHG;
        private System.Windows.Forms.CheckBox iSPOK;
        private System.Windows.Forms.CheckBox iMATADV;
        private System.Windows.Forms.CheckBox iNMCY_B;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox empty;
        private System.Windows.Forms.TextBox spindleInterlock;
        private System.Windows.Forms.TextBox loadMaterial;
        private System.Windows.Forms.TextBox changeMaterial;
        private System.Windows.Forms.TextBox condition;
        private System.Windows.Forms.TextBox endOfBar;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.CheckBox oBFCHCL;
        private System.Windows.Forms.CheckBox oBFCHOP;
        private System.Windows.Forms.CheckBox oMATADV;
        private System.Windows.Forms.CheckBox oMATCHG;
        private System.Windows.Forms.CheckBox oALMAB_B;
        private System.Windows.Forms.CheckBox oBFCDM;
        private System.Windows.Forms.TextBox oLoadMaterial;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.TextBox oSystem;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.TextBox oLinkMode;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.TextBox oChuckState;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.TextBox oChangeMaterial;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.CheckBox loadFail;
        private System.Windows.Forms.CheckBox changeFail;
        private System.Windows.Forms.TextBox barLength;
        private System.Windows.Forms.Label label16;
        private System.Windows.Forms.CheckBox iIN23;
        private System.Windows.Forms.TextBox mode;
        private System.Windows.Forms.Label label19;
        private System.Windows.Forms.Label label20;
        private System.Windows.Forms.TextBox iRegister;
        private System.Windows.Forms.Label label21;
        private System.Windows.Forms.TextBox oRegister;
        private System.Windows.Forms.CheckBox controllerMode;
        private System.Windows.Forms.TextBox door;
        private System.Windows.Forms.Label label17;
        private System.Windows.Forms.CheckBox doorClosed;
        private System.Windows.Forms.CheckBox doorOpen;
        private System.Windows.Forms.TextBox auxEndOfBar;
        private System.Windows.Forms.Label label18;
        private System.Windows.Forms.TextBox bfExec;
        private System.Windows.Forms.Label label23;
        private System.Windows.Forms.TextBox bfMode;
        private System.Windows.Forms.Label label22;
        private System.Windows.Forms.CheckBox iCUCOK;
    }
}

