namespace Redsniff.Dialogs
{
    partial class AboutForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AboutForm));
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.label_Red = new System.Windows.Forms.Label();
            this.label_Sniff = new System.Windows.Forms.Label();
            this.label_Version = new System.Windows.Forms.Label();
            this.label_Desc = new System.Windows.Forms.Label();
            this.linkLabel_Rednib = new System.Windows.Forms.LinkLabel();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // pictureBox1
            // 
            this.pictureBox1.BackgroundImage = global::Redsniff.Properties.Resources.shark_big;
            this.pictureBox1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.pictureBox1.Location = new System.Drawing.Point(12, 90);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(513, 287);
            this.pictureBox1.TabIndex = 0;
            this.pictureBox1.TabStop = false;
            // 
            // label_Red
            // 
            this.label_Red.AutoSize = true;
            this.label_Red.BackColor = System.Drawing.Color.Transparent;
            this.label_Red.Font = new System.Drawing.Font("Segoe UI", 26F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.label_Red.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(128)))));
            this.label_Red.Location = new System.Drawing.Point(148, -5);
            this.label_Red.Name = "label_Red";
            this.label_Red.Size = new System.Drawing.Size(123, 70);
            this.label_Red.TabIndex = 1;
            this.label_Red.Text = "Red";
            // 
            // label_Sniff
            // 
            this.label_Sniff.AutoSize = true;
            this.label_Sniff.BackColor = System.Drawing.Color.Transparent;
            this.label_Sniff.Font = new System.Drawing.Font("Segoe UI", 26F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.label_Sniff.ForeColor = System.Drawing.Color.Black;
            this.label_Sniff.Location = new System.Drawing.Point(248, -5);
            this.label_Sniff.Name = "label_Sniff";
            this.label_Sniff.Size = new System.Drawing.Size(139, 70);
            this.label_Sniff.TabIndex = 2;
            this.label_Sniff.Text = "sniff";
            // 
            // label_Version
            // 
            this.label_Version.AutoSize = true;
            this.label_Version.BackColor = System.Drawing.Color.Transparent;
            this.label_Version.Font = new System.Drawing.Font("Segoe UI Semibold", 11F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.label_Version.ForeColor = System.Drawing.Color.Black;
            this.label_Version.Location = new System.Drawing.Point(305, 52);
            this.label_Version.Name = "label_Version";
            this.label_Version.Size = new System.Drawing.Size(70, 30);
            this.label_Version.TabIndex = 3;
            this.label_Version.Text = "v0.0.0";
            // 
            // label_Desc
            // 
            this.label_Desc.AutoSize = true;
            this.label_Desc.BackColor = System.Drawing.Color.Transparent;
            this.label_Desc.Font = new System.Drawing.Font("Segoe UI Semibold", 11F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.label_Desc.ForeColor = System.Drawing.Color.Black;
            this.label_Desc.Location = new System.Drawing.Point(112, 399);
            this.label_Desc.Name = "label_Desc";
            this.label_Desc.Size = new System.Drawing.Size(317, 120);
            this.label_Desc.TabIndex = 4;
            this.label_Desc.Text = "The Network Packet Sniffer\r\n  - optimized for game packets\r\n  - easy to use\r\n  - " +
    "lightweight";
            // 
            // linkLabel_Rednib
            // 
            this.linkLabel_Rednib.AutoSize = true;
            this.linkLabel_Rednib.LinkBehavior = System.Windows.Forms.LinkBehavior.AlwaysUnderline;
            this.linkLabel_Rednib.LinkColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(128)))));
            this.linkLabel_Rednib.Location = new System.Drawing.Point(375, 537);
            this.linkLabel_Rednib.Name = "linkLabel_Rednib";
            this.linkLabel_Rednib.Size = new System.Drawing.Size(150, 25);
            this.linkLabel_Rednib.TabIndex = 5;
            this.linkLabel_Rednib.TabStop = true;
            this.linkLabel_Rednib.Text = "by RednibCoding";
            this.linkLabel_Rednib.VisitedLinkColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(128)))));
            this.linkLabel_Rednib.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkLabel_Rednib_LinkClicked);
            // 
            // AboutForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(10F, 25F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(537, 571);
            this.Controls.Add(this.linkLabel_Rednib);
            this.Controls.Add(this.label_Desc);
            this.Controls.Add(this.label_Version);
            this.Controls.Add(this.label_Sniff);
            this.Controls.Add(this.label_Red);
            this.Controls.Add(this.pictureBox1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "AboutForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "About";
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private PictureBox pictureBox1;
        private Label label_Red;
        private Label label_Sniff;
        private Label label_Version;
        private Label label_Desc;
        private LinkLabel linkLabel_Rednib;
    }
}