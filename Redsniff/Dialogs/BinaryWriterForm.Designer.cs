namespace Redsniff.Dialogs
{
    partial class BinaryWriterForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(BinaryWriterForm));
            this.label_BinaryWriter = new System.Windows.Forms.Label();
            this.textBox_HexValues = new System.Windows.Forms.TextBox();
            this.button_WriteBytes = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label_BinaryWriter
            // 
            this.label_BinaryWriter.AutoSize = true;
            this.label_BinaryWriter.Location = new System.Drawing.Point(12, 9);
            this.label_BinaryWriter.Name = "label_BinaryWriter";
            this.label_BinaryWriter.Size = new System.Drawing.Size(586, 25);
            this.label_BinaryWriter.TabIndex = 0;
            this.label_BinaryWriter.Text = "Paste hex values into the textbox that you want to write as bytes to a file:";
            // 
            // textBox_HexValues
            // 
            this.textBox_HexValues.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBox_HexValues.Font = new System.Drawing.Font("Consolas", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.textBox_HexValues.Location = new System.Drawing.Point(13, 46);
            this.textBox_HexValues.Multiline = true;
            this.textBox_HexValues.Name = "textBox_HexValues";
            this.textBox_HexValues.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.textBox_HexValues.Size = new System.Drawing.Size(775, 342);
            this.textBox_HexValues.TabIndex = 1;
            this.textBox_HexValues.WordWrap = false;
            // 
            // button_WriteBytes
            // 
            this.button_WriteBytes.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.button_WriteBytes.Location = new System.Drawing.Point(13, 394);
            this.button_WriteBytes.Name = "button_WriteBytes";
            this.button_WriteBytes.Size = new System.Drawing.Size(775, 44);
            this.button_WriteBytes.TabIndex = 2;
            this.button_WriteBytes.Text = "Write bytes to file";
            this.button_WriteBytes.UseVisualStyleBackColor = true;
            this.button_WriteBytes.Click += new System.EventHandler(this.button_WriteBytes_Click);
            // 
            // BinaryWriterForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(10F, 25F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.button_WriteBytes);
            this.Controls.Add(this.textBox_HexValues);
            this.Controls.Add(this.label_BinaryWriter);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "BinaryWriterForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Binary Writer";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Label label_BinaryWriter;
        private TextBox textBox_HexValues;
        private Button button_WriteBytes;
    }
}