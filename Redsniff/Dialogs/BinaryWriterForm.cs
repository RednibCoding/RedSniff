using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Redsniff.Dialogs
{
    public partial class BinaryWriterForm : Form
    {
        public BinaryWriterForm()
        {
            InitializeComponent();
        }

        byte[] hexStringToByteArray(string hexString)
        {
            hexString = Regex.Replace(hexString, @"\s", "");
            byte[] byteArray = new byte[hexString.Length / 2];

            for (int i = 0; i < byteArray.Length; i++)
            {
                byteArray[i] = Convert.ToByte(hexString.Substring(i * 2, 2), 16);
            }

            return byteArray;
        }

        bool isValidHexString(string hexString)
        {
            if (hexString.Length % 2 != 0 || hexString.Length == 0)
            {
                return false;
            }

            foreach (char c in hexString)
            {
                if (!Uri.IsHexDigit(c) && !Char.IsWhiteSpace(c))
                {
                    return false;
                }
            }

            return true;
        }

        private void button_WriteBytes_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(textBox_HexValues.Text)) return;

            textBox_HexValues.Text = textBox_HexValues.Text.Trim();

            if (!isValidHexString(textBox_HexValues.Text))
            {
                MessageBox.Show("Invalid hex values", "Invalid hex values", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            SaveFileDialog dialog = new SaveFileDialog()
            {
                Filter = "|*.*"
            };

            var bytes = hexStringToByteArray(textBox_HexValues.Text);

            var result = dialog.ShowDialog();

            if (result == DialogResult.OK)
            {
                File.WriteAllBytes(dialog.FileName, bytes);
            }
        }
    }
}
