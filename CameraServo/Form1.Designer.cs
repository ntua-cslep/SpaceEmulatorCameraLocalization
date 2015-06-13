namespace CameraServo
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.buttonRemoveRbt = new System.Windows.Forms.Button();
            this.buttonAddRbt = new System.Windows.Forms.Button();
            this.textBoxPort = new System.Windows.Forms.TextBox();
            this.textBoxSign = new System.Windows.Forms.TextBox();
            this.textBoxName = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.RobotCheckList = new System.Windows.Forms.CheckedListBox();
            this.buttonStart = new System.Windows.Forms.Button();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.checkBoxLogging = new System.Windows.Forms.CheckBox();
            this.labelLogFile = new System.Windows.Forms.Label();
            this.buttonBrowseLogfile = new System.Windows.Forms.Button();
            this.textBoxLogFile = new System.Windows.Forms.TextBox();
            this.label15 = new System.Windows.Forms.Label();
            this.textBoxSettings = new System.Windows.Forms.TextBox();
            this.buttonBrowse = new System.Windows.Forms.Button();
            this.fit = new System.Windows.Forms.CheckBox();
            this.buttonServer = new System.Windows.Forms.Button();
            this.buttonHelp = new System.Windows.Forms.Button();
            this.deadzones = new System.Windows.Forms.GroupBox();
            this.label13 = new System.Windows.Forms.Label();
            this.leftDzone = new System.Windows.Forms.NumericUpDown();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.rightDzone = new System.Windows.Forms.NumericUpDown();
            this.upDzone = new System.Windows.Forms.NumericUpDown();
            this.lowDzone = new System.Windows.Forms.NumericUpDown();
            this.label7 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.buttonDebug = new System.Windows.Forms.Button();
            this.label12 = new System.Windows.Forms.Label();
            this.tolerance = new System.Windows.Forms.NumericUpDown();
            this.label11 = new System.Windows.Forms.Label();
            this.threshold = new System.Windows.Forms.NumericUpDown();
            this.label10 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.winWidth = new System.Windows.Forms.NumericUpDown();
            this.winHeight = new System.Windows.Forms.NumericUpDown();
            this.label4 = new System.Windows.Forms.Label();
            this.comboBoxDevice = new System.Windows.Forms.ComboBox();
            this.debug = new System.Windows.Forms.TextBox();
            this.imageProcWorker = new System.ComponentModel.BackgroundWorker();
            this.stat = new System.Windows.Forms.TextBox();
            this.label14 = new System.Windows.Forms.Label();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.openLogFile = new System.Windows.Forms.OpenFileDialog();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.deadzones.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.leftDzone)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.rightDzone)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.upDzone)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lowDzone)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.tolerance)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.threshold)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.winWidth)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.winHeight)).BeginInit();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.BackColor = System.Drawing.SystemColors.ControlLight;
            this.groupBox1.Controls.Add(this.buttonRemoveRbt);
            this.groupBox1.Controls.Add(this.buttonAddRbt);
            this.groupBox1.Controls.Add(this.textBoxPort);
            this.groupBox1.Controls.Add(this.textBoxSign);
            this.groupBox1.Controls.Add(this.textBoxName);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.RobotCheckList);
            this.groupBox1.Location = new System.Drawing.Point(1, 1);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(203, 194);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Robots";
            // 
            // buttonRemoveRbt
            // 
            this.buttonRemoveRbt.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonRemoveRbt.Location = new System.Drawing.Point(59, 92);
            this.buttonRemoveRbt.Name = "buttonRemoveRbt";
            this.buttonRemoveRbt.Size = new System.Drawing.Size(75, 23);
            this.buttonRemoveRbt.TabIndex = 20;
            this.buttonRemoveRbt.Text = "Remove";
            this.buttonRemoveRbt.UseVisualStyleBackColor = true;
            this.buttonRemoveRbt.Click += new System.EventHandler(this.buttonRemoveRbt_Click);
            // 
            // buttonAddRbt
            // 
            this.buttonAddRbt.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonAddRbt.Location = new System.Drawing.Point(140, 92);
            this.buttonAddRbt.Name = "buttonAddRbt";
            this.buttonAddRbt.Size = new System.Drawing.Size(57, 23);
            this.buttonAddRbt.TabIndex = 7;
            this.buttonAddRbt.Text = "Add";
            this.buttonAddRbt.UseVisualStyleBackColor = true;
            this.buttonAddRbt.Click += new System.EventHandler(this.buttonAddRbt_Click);
            // 
            // textBoxPort
            // 
            this.textBoxPort.Location = new System.Drawing.Point(71, 66);
            this.textBoxPort.Name = "textBoxPort";
            this.textBoxPort.Size = new System.Drawing.Size(126, 20);
            this.textBoxPort.TabIndex = 6;
            // 
            // textBoxSign
            // 
            this.textBoxSign.Location = new System.Drawing.Point(71, 39);
            this.textBoxSign.Name = "textBoxSign";
            this.textBoxSign.Size = new System.Drawing.Size(126, 20);
            this.textBoxSign.TabIndex = 5;
            // 
            // textBoxName
            // 
            this.textBoxName.Location = new System.Drawing.Point(71, 13);
            this.textBoxName.Name = "textBoxName";
            this.textBoxName.Size = new System.Drawing.Size(126, 20);
            this.textBoxName.TabIndex = 4;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(9, 69);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(53, 13);
            this.label3.TabIndex = 3;
            this.label3.Text = "Robot ID:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(9, 43);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(56, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "Sign (mm):";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(9, 16);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(38, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Name:";
            // 
            // RobotCheckList
            // 
            this.RobotCheckList.FormattingEnabled = true;
            this.RobotCheckList.Location = new System.Drawing.Point(6, 121);
            this.RobotCheckList.Name = "RobotCheckList";
            this.RobotCheckList.Size = new System.Drawing.Size(191, 64);
            this.RobotCheckList.TabIndex = 0;
            this.RobotCheckList.ThreeDCheckBoxes = true;
            // 
            // buttonStart
            // 
            this.buttonStart.BackColor = System.Drawing.Color.LimeGreen;
            this.buttonStart.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonStart.Location = new System.Drawing.Point(449, 21);
            this.buttonStart.Name = "buttonStart";
            this.buttonStart.Size = new System.Drawing.Size(98, 23);
            this.buttonStart.TabIndex = 1;
            this.buttonStart.Text = "Start tracking";
            this.buttonStart.UseVisualStyleBackColor = false;
            this.buttonStart.Click += new System.EventHandler(this.buttonStart_Click);
            // 
            // groupBox2
            // 
            this.groupBox2.BackColor = System.Drawing.SystemColors.ControlLight;
            this.groupBox2.Controls.Add(this.checkBoxLogging);
            this.groupBox2.Controls.Add(this.labelLogFile);
            this.groupBox2.Controls.Add(this.buttonBrowseLogfile);
            this.groupBox2.Controls.Add(this.textBoxLogFile);
            this.groupBox2.Controls.Add(this.label15);
            this.groupBox2.Controls.Add(this.textBoxSettings);
            this.groupBox2.Controls.Add(this.buttonBrowse);
            this.groupBox2.Controls.Add(this.fit);
            this.groupBox2.Controls.Add(this.buttonServer);
            this.groupBox2.Controls.Add(this.buttonHelp);
            this.groupBox2.Controls.Add(this.deadzones);
            this.groupBox2.Controls.Add(this.label12);
            this.groupBox2.Controls.Add(this.tolerance);
            this.groupBox2.Controls.Add(this.buttonStart);
            this.groupBox2.Controls.Add(this.label11);
            this.groupBox2.Controls.Add(this.threshold);
            this.groupBox2.Controls.Add(this.label10);
            this.groupBox2.Controls.Add(this.label9);
            this.groupBox2.Controls.Add(this.winWidth);
            this.groupBox2.Controls.Add(this.winHeight);
            this.groupBox2.Controls.Add(this.label4);
            this.groupBox2.Controls.Add(this.comboBoxDevice);
            this.groupBox2.Location = new System.Drawing.Point(204, 1);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(649, 194);
            this.groupBox2.TabIndex = 3;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Image proccessing";
            // 
            // checkBoxLogging
            // 
            this.checkBoxLogging.AutoSize = true;
            this.checkBoxLogging.Location = new System.Drawing.Point(449, 112);
            this.checkBoxLogging.Name = "checkBoxLogging";
            this.checkBoxLogging.Size = new System.Drawing.Size(99, 17);
            this.checkBoxLogging.TabIndex = 27;
            this.checkBoxLogging.Text = "Logging enable";
            this.checkBoxLogging.UseVisualStyleBackColor = true;
            this.checkBoxLogging.CheckedChanged += new System.EventHandler(this.checkBoxLogging_CheckedChanged);
            // 
            // labelLogFile
            // 
            this.labelLogFile.AutoSize = true;
            this.labelLogFile.Location = new System.Drawing.Point(14, 92);
            this.labelLogFile.Name = "labelLogFile";
            this.labelLogFile.Size = new System.Drawing.Size(41, 13);
            this.labelLogFile.TabIndex = 26;
            this.labelLogFile.Text = "Log file";
            // 
            // buttonBrowseLogfile
            // 
            this.buttonBrowseLogfile.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonBrowseLogfile.Location = new System.Drawing.Point(231, 112);
            this.buttonBrowseLogfile.Name = "buttonBrowseLogfile";
            this.buttonBrowseLogfile.Size = new System.Drawing.Size(62, 23);
            this.buttonBrowseLogfile.TabIndex = 25;
            this.buttonBrowseLogfile.Text = "Browse";
            this.buttonBrowseLogfile.UseVisualStyleBackColor = true;
            this.buttonBrowseLogfile.Click += new System.EventHandler(this.buttonBrowseLogfile_Click);
            // 
            // textBoxLogFile
            // 
            this.textBoxLogFile.Location = new System.Drawing.Point(6, 114);
            this.textBoxLogFile.Name = "textBoxLogFile";
            this.textBoxLogFile.Size = new System.Drawing.Size(219, 20);
            this.textBoxLogFile.TabIndex = 24;
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.Location = new System.Drawing.Point(11, 46);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(98, 13);
            this.label15.TabIndex = 23;
            this.label15.Text = "Camera settings file";
            // 
            // textBoxSettings
            // 
            this.textBoxSettings.Location = new System.Drawing.Point(6, 61);
            this.textBoxSettings.Name = "textBoxSettings";
            this.textBoxSettings.Size = new System.Drawing.Size(219, 20);
            this.textBoxSettings.TabIndex = 6;
            // 
            // buttonBrowse
            // 
            this.buttonBrowse.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonBrowse.Location = new System.Drawing.Point(231, 59);
            this.buttonBrowse.Name = "buttonBrowse";
            this.buttonBrowse.Size = new System.Drawing.Size(62, 23);
            this.buttonBrowse.TabIndex = 22;
            this.buttonBrowse.Text = "Browse";
            this.buttonBrowse.UseVisualStyleBackColor = true;
            this.buttonBrowse.Click += new System.EventHandler(this.buttonBrowse_Click);
            // 
            // fit
            // 
            this.fit.AutoSize = true;
            this.fit.Checked = true;
            this.fit.CheckState = System.Windows.Forms.CheckState.Checked;
            this.fit.Location = new System.Drawing.Point(449, 89);
            this.fit.Name = "fit";
            this.fit.Size = new System.Drawing.Size(117, 17);
            this.fit.TabIndex = 21;
            this.fit.Text = "Image fit to window";
            this.fit.UseVisualStyleBackColor = true;
            // 
            // buttonServer
            // 
            this.buttonServer.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonServer.Location = new System.Drawing.Point(551, 21);
            this.buttonServer.Name = "buttonServer";
            this.buttonServer.Size = new System.Drawing.Size(92, 23);
            this.buttonServer.TabIndex = 19;
            this.buttonServer.Text = "Start Server";
            this.buttonServer.UseVisualStyleBackColor = true;
            this.buttonServer.Click += new System.EventHandler(this.buttonServer_Click);
            // 
            // buttonHelp
            // 
            this.buttonHelp.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonHelp.Location = new System.Drawing.Point(449, 50);
            this.buttonHelp.Name = "buttonHelp";
            this.buttonHelp.Size = new System.Drawing.Size(98, 23);
            this.buttonHelp.TabIndex = 19;
            this.buttonHelp.Text = "Help Please!!!";
            this.buttonHelp.UseVisualStyleBackColor = true;
            this.buttonHelp.Click += new System.EventHandler(this.buttonHelp_Click);
            // 
            // deadzones
            // 
            this.deadzones.BackColor = System.Drawing.SystemColors.ControlLight;
            this.deadzones.Controls.Add(this.label13);
            this.deadzones.Controls.Add(this.leftDzone);
            this.deadzones.Controls.Add(this.label5);
            this.deadzones.Controls.Add(this.label6);
            this.deadzones.Controls.Add(this.rightDzone);
            this.deadzones.Controls.Add(this.upDzone);
            this.deadzones.Controls.Add(this.lowDzone);
            this.deadzones.Controls.Add(this.label7);
            this.deadzones.Controls.Add(this.label8);
            this.deadzones.Controls.Add(this.buttonDebug);
            this.deadzones.Location = new System.Drawing.Point(319, 15);
            this.deadzones.Name = "deadzones";
            this.deadzones.Size = new System.Drawing.Size(114, 171);
            this.deadzones.TabIndex = 5;
            this.deadzones.TabStop = false;
            this.deadzones.Text = "Image Deadzones";
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(6, 22);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(79, 13);
            this.label13.TabIndex = 11;
            this.label13.Text = "Values in pixels";
            // 
            // leftDzone
            // 
            this.leftDzone.Location = new System.Drawing.Point(48, 38);
            this.leftDzone.Maximum = new decimal(new int[] {
            800,
            0,
            0,
            0});
            this.leftDzone.Name = "leftDzone";
            this.leftDzone.Size = new System.Drawing.Size(60, 20);
            this.leftDzone.TabIndex = 3;
            this.leftDzone.Value = new decimal(new int[] {
            150,
            0,
            0,
            0});
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(5, 40);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(28, 13);
            this.label5.TabIndex = 4;
            this.label5.Text = "Left:";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(5, 66);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(35, 13);
            this.label6.TabIndex = 5;
            this.label6.Text = "Right:";
            // 
            // rightDzone
            // 
            this.rightDzone.Location = new System.Drawing.Point(48, 64);
            this.rightDzone.Maximum = new decimal(new int[] {
            800,
            0,
            0,
            0});
            this.rightDzone.Name = "rightDzone";
            this.rightDzone.Size = new System.Drawing.Size(60, 20);
            this.rightDzone.TabIndex = 6;
            this.rightDzone.Value = new decimal(new int[] {
            150,
            0,
            0,
            0});
            // 
            // upDzone
            // 
            this.upDzone.Location = new System.Drawing.Point(48, 90);
            this.upDzone.Maximum = new decimal(new int[] {
            600,
            0,
            0,
            0});
            this.upDzone.Name = "upDzone";
            this.upDzone.Size = new System.Drawing.Size(60, 20);
            this.upDzone.TabIndex = 7;
            this.upDzone.Value = new decimal(new int[] {
            15,
            0,
            0,
            0});
            // 
            // lowDzone
            // 
            this.lowDzone.Location = new System.Drawing.Point(48, 116);
            this.lowDzone.Maximum = new decimal(new int[] {
            600,
            0,
            0,
            0});
            this.lowDzone.Name = "lowDzone";
            this.lowDzone.Size = new System.Drawing.Size(60, 20);
            this.lowDzone.TabIndex = 8;
            this.lowDzone.Value = new decimal(new int[] {
            5,
            0,
            0,
            0});
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(3, 92);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(39, 13);
            this.label7.TabIndex = 9;
            this.label7.Text = "Upper:";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(3, 118);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(39, 13);
            this.label8.TabIndex = 10;
            this.label8.Text = "Lower:";
            // 
            // buttonDebug
            // 
            this.buttonDebug.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonDebug.Location = new System.Drawing.Point(6, 142);
            this.buttonDebug.Name = "buttonDebug";
            this.buttonDebug.Size = new System.Drawing.Size(102, 23);
            this.buttonDebug.TabIndex = 3;
            this.buttonDebug.Text = "Show";
            this.buttonDebug.UseVisualStyleBackColor = true;
            this.buttonDebug.Click += new System.EventHandler(this.buttonDebug_Click);
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(153, 142);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(80, 13);
            this.label12.TabIndex = 18;
            this.label12.Text = "Tolerance(mm):";
            // 
            // tolerance
            // 
            this.tolerance.DecimalPlaces = 1;
            this.tolerance.Increment = new decimal(new int[] {
            1,
            0,
            0,
            65536});
            this.tolerance.Location = new System.Drawing.Point(239, 140);
            this.tolerance.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            65536});
            this.tolerance.Name = "tolerance";
            this.tolerance.Size = new System.Drawing.Size(50, 20);
            this.tolerance.TabIndex = 17;
            this.tolerance.Value = new decimal(new int[] {
            1000,
            0,
            0,
            131072});
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(153, 168);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(57, 13);
            this.label11.TabIndex = 16;
            this.label11.Text = "Threshold:";
            // 
            // threshold
            // 
            this.threshold.Location = new System.Drawing.Point(239, 166);
            this.threshold.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
            this.threshold.Name = "threshold";
            this.threshold.Size = new System.Drawing.Size(50, 20);
            this.threshold.TabIndex = 15;
            this.threshold.Value = new decimal(new int[] {
            210,
            0,
            0,
            0});
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(11, 168);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(71, 13);
            this.label10.TabIndex = 14;
            this.label10.Text = "Height (pixel):";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(11, 142);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(68, 13);
            this.label9.TabIndex = 13;
            this.label9.Text = "Width (pixel):";
            // 
            // winWidth
            // 
            this.winWidth.Location = new System.Drawing.Point(97, 140);
            this.winWidth.Minimum = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.winWidth.Name = "winWidth";
            this.winWidth.Size = new System.Drawing.Size(50, 20);
            this.winWidth.TabIndex = 12;
            this.winWidth.Value = new decimal(new int[] {
            40,
            0,
            0,
            0});
            // 
            // winHeight
            // 
            this.winHeight.Location = new System.Drawing.Point(97, 166);
            this.winHeight.Minimum = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.winHeight.Name = "winHeight";
            this.winHeight.Size = new System.Drawing.Size(50, 20);
            this.winHeight.TabIndex = 11;
            this.winHeight.Value = new decimal(new int[] {
            40,
            0,
            0,
            0});
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(11, 21);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(44, 13);
            this.label4.TabIndex = 1;
            this.label4.Text = "Device:";
            // 
            // comboBoxDevice
            // 
            this.comboBoxDevice.FormattingEnabled = true;
            this.comboBoxDevice.Location = new System.Drawing.Point(59, 19);
            this.comboBoxDevice.Name = "comboBoxDevice";
            this.comboBoxDevice.Size = new System.Drawing.Size(100, 21);
            this.comboBoxDevice.TabIndex = 0;
            this.comboBoxDevice.SelectedIndexChanged += new System.EventHandler(this.comboBoxDevice_SelectedIndexChanged);
            // 
            // debug
            // 
            this.debug.AcceptsReturn = true;
            this.debug.BackColor = System.Drawing.SystemColors.Control;
            this.debug.Location = new System.Drawing.Point(1, 201);
            this.debug.Multiline = true;
            this.debug.Name = "debug";
            this.debug.ReadOnly = true;
            this.debug.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.debug.Size = new System.Drawing.Size(516, 137);
            this.debug.TabIndex = 2;
            // 
            // imageProcWorker
            // 
            this.imageProcWorker.WorkerReportsProgress = true;
            this.imageProcWorker.WorkerSupportsCancellation = true;
            this.imageProcWorker.DoWork += new System.ComponentModel.DoWorkEventHandler(this.imageProcWorker_DoWork);
            // 
            // stat
            // 
            this.stat.Location = new System.Drawing.Point(581, 318);
            this.stat.Name = "stat";
            this.stat.ReadOnly = true;
            this.stat.Size = new System.Drawing.Size(272, 20);
            this.stat.TabIndex = 4;
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Location = new System.Drawing.Point(523, 321);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(52, 13);
            this.label14.TabIndex = 5;
            this.label14.Text = "Statistics:";
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.DefaultExt = "xml";
            this.openFileDialog1.Filter = "xml files |*.xml";
            // 
            // openLogFile
            // 
            this.openLogFile.FileName = ".\\\\log.txt";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(853, 344);
            this.Controls.Add(this.label14);
            this.Controls.Add(this.stat);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.debug);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "Form1";
            this.Text = "Robot Tracker";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
            this.Load += new System.EventHandler(this.Form1_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.deadzones.ResumeLayout(false);
            this.deadzones.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.leftDzone)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.rightDzone)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.upDzone)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lowDzone)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.tolerance)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.threshold)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.winWidth)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.winHeight)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.TextBox textBoxPort;
        private System.Windows.Forms.TextBox textBoxSign;
        private System.Windows.Forms.TextBox textBoxName;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.CheckedListBox RobotCheckList;
        private System.Windows.Forms.Button buttonAddRbt;
        private System.Windows.Forms.Button buttonStart;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ComboBox comboBoxDevice;
        private System.ComponentModel.BackgroundWorker imageProcWorker;
        private System.Windows.Forms.Button buttonDebug;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.NumericUpDown lowDzone;
        private System.Windows.Forms.NumericUpDown upDzone;
        private System.Windows.Forms.NumericUpDown rightDzone;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.NumericUpDown leftDzone;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.NumericUpDown winWidth;
        private System.Windows.Forms.NumericUpDown winHeight;
        private System.Windows.Forms.TextBox stat;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.NumericUpDown threshold;
        private System.Windows.Forms.GroupBox deadzones;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.NumericUpDown tolerance;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.Button buttonHelp;
        public System.Windows.Forms.TextBox debug;
        private System.Windows.Forms.Button buttonRemoveRbt;
        private System.Windows.Forms.CheckBox fit;
        private System.Windows.Forms.Button buttonBrowse;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.TextBox textBoxSettings;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.Button buttonServer;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.Label labelLogFile;
        private System.Windows.Forms.Button buttonBrowseLogfile;
        private System.Windows.Forms.TextBox textBoxLogFile;
        private System.Windows.Forms.OpenFileDialog openLogFile;
        private System.Windows.Forms.CheckBox checkBoxLogging;
    }
}

