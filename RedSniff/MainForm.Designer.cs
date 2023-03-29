namespace Redsniff
{
    partial class MainForm
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.Toolbar = new System.Windows.Forms.ToolStrip();
            this.Button_Start = new System.Windows.Forms.ToolStripButton();
            this.Button_Stop = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.Button_Save = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.label_Device = new System.Windows.Forms.ToolStripLabel();
            this.ComboBox_Devices = new System.Windows.Forms.ToolStripComboBox();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.label_Protocol = new System.Windows.Forms.ToolStripLabel();
            this.ComboBox_Protocols = new System.Windows.Forms.ToolStripComboBox();
            this.toolStripSeparator4 = new System.Windows.Forms.ToolStripSeparator();
            this.label_SrcPort = new System.Windows.Forms.ToolStripLabel();
            this.TextBox_SrcPort = new System.Windows.Forms.ToolStripTextBox();
            this.toolStripSeparator5 = new System.Windows.Forms.ToolStripSeparator();
            this.label_DstPort = new System.Windows.Forms.ToolStripLabel();
            this.TextBox_DstPort = new System.Windows.Forms.ToolStripTextBox();
            this.toolStripSeparator6 = new System.Windows.Forms.ToolStripSeparator();
            this.label_SrcIp = new System.Windows.Forms.ToolStripLabel();
            this.TextBox_SrcIp = new System.Windows.Forms.ToolStripTextBox();
            this.toolStripSeparator7 = new System.Windows.Forms.ToolStripSeparator();
            this.label_DstIp = new System.Windows.Forms.ToolStripLabel();
            this.TextBox_DstIp = new System.Windows.Forms.ToolStripTextBox();
            this.toolStripSeparator8 = new System.Windows.Forms.ToolStripSeparator();
            this.Button_Apply = new System.Windows.Forms.ToolStripButton();
            this.Button_Reset = new System.Windows.Forms.ToolStripButton();
            this.MainSplitContainer = new System.Windows.Forms.SplitContainer();
            this.DataGridView_CapturedPackets = new System.Windows.Forms.DataGridView();
            this.TextBox_PacketData = new System.Windows.Forms.TextBox();
            this.Button_About = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator9 = new System.Windows.Forms.ToolStripSeparator();
            this.Toolbar.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.MainSplitContainer)).BeginInit();
            this.MainSplitContainer.Panel1.SuspendLayout();
            this.MainSplitContainer.Panel2.SuspendLayout();
            this.MainSplitContainer.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.DataGridView_CapturedPackets)).BeginInit();
            this.SuspendLayout();
            // 
            // Toolbar
            // 
            this.Toolbar.ImageScalingSize = new System.Drawing.Size(24, 24);
            this.Toolbar.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.Button_Start,
            this.Button_Stop,
            this.toolStripSeparator1,
            this.Button_Save,
            this.toolStripSeparator3,
            this.label_Device,
            this.ComboBox_Devices,
            this.toolStripSeparator2,
            this.label_Protocol,
            this.ComboBox_Protocols,
            this.toolStripSeparator4,
            this.label_SrcPort,
            this.TextBox_SrcPort,
            this.toolStripSeparator5,
            this.label_DstPort,
            this.TextBox_DstPort,
            this.toolStripSeparator6,
            this.label_SrcIp,
            this.TextBox_SrcIp,
            this.toolStripSeparator7,
            this.label_DstIp,
            this.TextBox_DstIp,
            this.toolStripSeparator8,
            this.Button_Apply,
            this.Button_Reset,
            this.toolStripSeparator9,
            this.Button_About});
            this.Toolbar.Location = new System.Drawing.Point(0, 0);
            this.Toolbar.Name = "Toolbar";
            this.Toolbar.Size = new System.Drawing.Size(1623, 33);
            this.Toolbar.TabIndex = 0;
            this.Toolbar.Text = "toolStrip1";
            // 
            // Button_Start
            // 
            this.Button_Start.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.Button_Start.Image = global::Redsniff.Properties.Resources.play;
            this.Button_Start.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.Button_Start.Name = "Button_Start";
            this.Button_Start.Size = new System.Drawing.Size(34, 28);
            this.Button_Start.Text = "Start";
            this.Button_Start.Click += new System.EventHandler(this.Button_Start_Click);
            // 
            // Button_Stop
            // 
            this.Button_Stop.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.Button_Stop.Image = global::Redsniff.Properties.Resources.stop;
            this.Button_Stop.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.Button_Stop.Name = "Button_Stop";
            this.Button_Stop.Size = new System.Drawing.Size(34, 28);
            this.Button_Stop.Text = "Stop";
            this.Button_Stop.Click += new System.EventHandler(this.Button_Stop_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 33);
            // 
            // Button_Save
            // 
            this.Button_Save.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.Button_Save.Image = global::Redsniff.Properties.Resources.save;
            this.Button_Save.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.Button_Save.Name = "Button_Save";
            this.Button_Save.Size = new System.Drawing.Size(34, 28);
            this.Button_Save.Text = "Save";
            this.Button_Save.Click += new System.EventHandler(this.Button_Save_Click);
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            this.toolStripSeparator3.Size = new System.Drawing.Size(6, 33);
            // 
            // label_Device
            // 
            this.label_Device.Name = "label_Device";
            this.label_Device.Size = new System.Drawing.Size(68, 28);
            this.label_Device.Text = "Device:";
            // 
            // ComboBox_Devices
            // 
            this.ComboBox_Devices.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.ComboBox_Devices.DropDownWidth = 200;
            this.ComboBox_Devices.FlatStyle = System.Windows.Forms.FlatStyle.Standard;
            this.ComboBox_Devices.Name = "ComboBox_Devices";
            this.ComboBox_Devices.Size = new System.Drawing.Size(200, 33);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(6, 33);
            // 
            // label_Protocol
            // 
            this.label_Protocol.Name = "label_Protocol";
            this.label_Protocol.Size = new System.Drawing.Size(83, 28);
            this.label_Protocol.Text = "Protocol:";
            // 
            // ComboBox_Protocols
            // 
            this.ComboBox_Protocols.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.ComboBox_Protocols.FlatStyle = System.Windows.Forms.FlatStyle.Standard;
            this.ComboBox_Protocols.Items.AddRange(new object[] {
            "tcp",
            "udp"});
            this.ComboBox_Protocols.Name = "ComboBox_Protocols";
            this.ComboBox_Protocols.Size = new System.Drawing.Size(85, 33);
            // 
            // toolStripSeparator4
            // 
            this.toolStripSeparator4.Name = "toolStripSeparator4";
            this.toolStripSeparator4.Size = new System.Drawing.Size(6, 33);
            // 
            // label_SrcPort
            // 
            this.label_SrcPort.Name = "label_SrcPort";
            this.label_SrcPort.Size = new System.Drawing.Size(77, 28);
            this.label_SrcPort.Text = "Src Port:";
            // 
            // TextBox_SrcPort
            // 
            this.TextBox_SrcPort.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.TextBox_SrcPort.Name = "TextBox_SrcPort";
            this.TextBox_SrcPort.Size = new System.Drawing.Size(100, 33);
            // 
            // toolStripSeparator5
            // 
            this.toolStripSeparator5.Name = "toolStripSeparator5";
            this.toolStripSeparator5.Size = new System.Drawing.Size(6, 33);
            // 
            // label_DstPort
            // 
            this.label_DstPort.Name = "label_DstPort";
            this.label_DstPort.Size = new System.Drawing.Size(80, 28);
            this.label_DstPort.Text = "Dst Port:";
            // 
            // TextBox_DstPort
            // 
            this.TextBox_DstPort.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.TextBox_DstPort.Name = "TextBox_DstPort";
            this.TextBox_DstPort.Size = new System.Drawing.Size(100, 33);
            // 
            // toolStripSeparator6
            // 
            this.toolStripSeparator6.Name = "toolStripSeparator6";
            this.toolStripSeparator6.Size = new System.Drawing.Size(6, 33);
            // 
            // label_SrcIp
            // 
            this.label_SrcIp.Name = "label_SrcIp";
            this.label_SrcIp.Size = new System.Drawing.Size(61, 28);
            this.label_SrcIp.Text = "Src Ip:";
            // 
            // TextBox_SrcIp
            // 
            this.TextBox_SrcIp.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.TextBox_SrcIp.Name = "TextBox_SrcIp";
            this.TextBox_SrcIp.Size = new System.Drawing.Size(135, 33);
            // 
            // toolStripSeparator7
            // 
            this.toolStripSeparator7.Name = "toolStripSeparator7";
            this.toolStripSeparator7.Size = new System.Drawing.Size(6, 33);
            // 
            // label_DstIp
            // 
            this.label_DstIp.Name = "label_DstIp";
            this.label_DstIp.Size = new System.Drawing.Size(60, 28);
            this.label_DstIp.Text = "Dst Ip";
            // 
            // TextBox_DstIp
            // 
            this.TextBox_DstIp.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.TextBox_DstIp.Name = "TextBox_DstIp";
            this.TextBox_DstIp.Size = new System.Drawing.Size(135, 33);
            // 
            // toolStripSeparator8
            // 
            this.toolStripSeparator8.Name = "toolStripSeparator8";
            this.toolStripSeparator8.Size = new System.Drawing.Size(6, 33);
            // 
            // Button_Apply
            // 
            this.Button_Apply.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.Button_Apply.Image = global::Redsniff.Properties.Resources.accept;
            this.Button_Apply.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.Button_Apply.Name = "Button_Apply";
            this.Button_Apply.Size = new System.Drawing.Size(34, 28);
            this.Button_Apply.Text = "Apply";
            this.Button_Apply.ToolTipText = "Apply Filter";
            this.Button_Apply.Click += new System.EventHandler(this.Button_Apply_Click);
            // 
            // Button_Reset
            // 
            this.Button_Reset.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.Button_Reset.Image = global::Redsniff.Properties.Resources.refresh;
            this.Button_Reset.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.Button_Reset.Name = "Button_Reset";
            this.Button_Reset.Size = new System.Drawing.Size(34, 28);
            this.Button_Reset.Text = "Reset";
            this.Button_Reset.ToolTipText = "Reset Filter";
            this.Button_Reset.Click += new System.EventHandler(this.Button_Reset_Click);
            // 
            // MainSplitContainer
            // 
            this.MainSplitContainer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.MainSplitContainer.Location = new System.Drawing.Point(0, 33);
            this.MainSplitContainer.Name = "MainSplitContainer";
            this.MainSplitContainer.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // MainSplitContainer.Panel1
            // 
            this.MainSplitContainer.Panel1.Controls.Add(this.DataGridView_CapturedPackets);
            // 
            // MainSplitContainer.Panel2
            // 
            this.MainSplitContainer.Panel2.Controls.Add(this.TextBox_PacketData);
            this.MainSplitContainer.Size = new System.Drawing.Size(1623, 975);
            this.MainSplitContainer.SplitterDistance = 485;
            this.MainSplitContainer.TabIndex = 1;
            // 
            // DataGridView_CapturedPackets
            // 
            this.DataGridView_CapturedPackets.AllowUserToAddRows = false;
            this.DataGridView_CapturedPackets.AllowUserToDeleteRows = false;
            this.DataGridView_CapturedPackets.AllowUserToResizeRows = false;
            this.DataGridView_CapturedPackets.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.DataGridView_CapturedPackets.BackgroundColor = System.Drawing.Color.White;
            this.DataGridView_CapturedPackets.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.DataGridView_CapturedPackets.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.DataGridView_CapturedPackets.Dock = System.Windows.Forms.DockStyle.Fill;
            this.DataGridView_CapturedPackets.Location = new System.Drawing.Point(0, 0);
            this.DataGridView_CapturedPackets.MultiSelect = false;
            this.DataGridView_CapturedPackets.Name = "DataGridView_CapturedPackets";
            this.DataGridView_CapturedPackets.ReadOnly = true;
            this.DataGridView_CapturedPackets.RowHeadersWidth = 62;
            this.DataGridView_CapturedPackets.RowTemplate.Height = 33;
            this.DataGridView_CapturedPackets.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.DataGridView_CapturedPackets.Size = new System.Drawing.Size(1623, 485);
            this.DataGridView_CapturedPackets.TabIndex = 0;
            this.DataGridView_CapturedPackets.TabStop = false;
            // 
            // TextBox_PacketData
            // 
            this.TextBox_PacketData.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.TextBox_PacketData.Dock = System.Windows.Forms.DockStyle.Fill;
            this.TextBox_PacketData.Font = new System.Drawing.Font("Consolas", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.TextBox_PacketData.Location = new System.Drawing.Point(0, 0);
            this.TextBox_PacketData.Multiline = true;
            this.TextBox_PacketData.Name = "TextBox_PacketData";
            this.TextBox_PacketData.ReadOnly = true;
            this.TextBox_PacketData.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.TextBox_PacketData.Size = new System.Drawing.Size(1623, 486);
            this.TextBox_PacketData.TabIndex = 0;
            this.TextBox_PacketData.TabStop = false;
            this.TextBox_PacketData.WordWrap = false;
            // 
            // Button_About
            // 
            this.Button_About.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.Button_About.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.Button_About.Image = global::Redsniff.Properties.Resources.question_mark;
            this.Button_About.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.Button_About.Name = "Button_About";
            this.Button_About.Size = new System.Drawing.Size(34, 28);
            this.Button_About.Text = "About";
            this.Button_About.Click += new System.EventHandler(this.Button_About_Click);
            // 
            // toolStripSeparator9
            // 
            this.toolStripSeparator9.Name = "toolStripSeparator9";
            this.toolStripSeparator9.Size = new System.Drawing.Size(6, 33);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(10F, 25F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1623, 1008);
            this.Controls.Add(this.MainSplitContainer);
            this.Controls.Add(this.Toolbar);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MinimumSize = new System.Drawing.Size(1500, 800);
            this.Name = "MainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Redsniff";
            this.Toolbar.ResumeLayout(false);
            this.Toolbar.PerformLayout();
            this.MainSplitContainer.Panel1.ResumeLayout(false);
            this.MainSplitContainer.Panel2.ResumeLayout(false);
            this.MainSplitContainer.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.MainSplitContainer)).EndInit();
            this.MainSplitContainer.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.DataGridView_CapturedPackets)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        public ToolStrip Toolbar;
        public SplitContainer MainSplitContainer;
        public DataGridView DataGridView_CapturedPackets;
        private ToolStripSeparator toolStripSeparator1;
        private ToolStripSeparator toolStripSeparator2;
        private ToolStripLabel label_Protocol;
        public ToolStripComboBox ComboBox_Protocols;
        private ToolStripLabel label_Device;
        private ToolStripSeparator toolStripSeparator3;
        private ToolStripSeparator toolStripSeparator4;
        private ToolStripSeparator toolStripSeparator5;
        public ToolStripTextBox TextBox_SrcPort;
        private ToolStripLabel label_SrcPort;
        private ToolStripLabel label_DstPort;
        public ToolStripTextBox TextBox_DstPort;
        private ToolStripLabel label_SrcIp;
        public ToolStripTextBox TextBox_SrcIp;
        public ToolStripComboBox ComboBox_Devices;
        private ToolStripSeparator toolStripSeparator6;
        private ToolStripLabel label_DstIp;
        public ToolStripTextBox TextBox_DstIp;
        private ToolStripSeparator toolStripSeparator7;
        private ToolStripSeparator toolStripSeparator8;
        public ToolStripButton Button_Apply;
        public ToolStripButton Button_Reset;
        public ToolStripButton Button_Start;
        public ToolStripButton Button_Stop;
        public ToolStripButton Button_Save;
        public TextBox TextBox_PacketData;
        private ToolStripSeparator toolStripSeparator9;
        private ToolStripButton Button_About;
    }
}