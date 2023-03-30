using Redsniff.Dialogs;
using System.Windows.Forms;

namespace Redsniff
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
            ComboBox_Protocols.SelectedIndexChanged += onProtocolChanged;
            DataGridView_CapturedPackets.RowPrePaint += onRowPrePaint;
            DataGridView_CapturedPackets.DoubleClick += onRowDoubleClick;
        }

        private void onRowDoubleClick(object? sender, EventArgs e)
        {
            Program.MainController.OnRowDoubleClick();
        }

        private void onRowPrePaint(object? sender, DataGridViewRowPrePaintEventArgs e)
        {
            if (e.RowIndex % 2 == 1)
                DataGridView_CapturedPackets.Rows[e.RowIndex].DefaultCellStyle.BackColor = Color.Gainsboro;
            else
                DataGridView_CapturedPackets.Rows[e.RowIndex].DefaultCellStyle.BackColor = Color.WhiteSmoke;
        }

        private void onProtocolChanged(object? sender, EventArgs e)
        {
            var protocolStr = ComboBox_Protocols.SelectedItem as string;
            if (protocolStr == null) return;
            Program.MainState.SelectedProtocol = protocolStr;
        }

        private void Button_Start_Click(object sender, EventArgs e)
        {
            Program.MainController.StartCapturing();
        }

        private void Button_Stop_Click(object sender, EventArgs e)
        {
            Program.MainController.StopCapturing();
        }

        private void Button_Save_Click(object sender, EventArgs e)
        {
            Program.MainController.SaveCapture();
        }

        private void Button_Apply_Click(object sender, EventArgs e)
        {
            Program.MainController.ApplyFilter();
        }

        private void Button_Reset_Click(object sender, EventArgs e)
        {
            Program.MainController.ClearFilter();
        }

        private void Button_ToggleEmptyPackets_Click(object sender, EventArgs e)
        {
            Program.MainState.ListEmptyPackets = !Program.MainState.ListEmptyPackets;
            if (Program.MainState.ListEmptyPackets)
            {
                Button_ToggleEmptyPackets.Text = "Empty packets: Yes";
            }
            else
            {
                Button_ToggleEmptyPackets.Text = "Empty packets: No";
            }
            Program.MainController.ApplyFilter();
        }

        private void ToolsMenuItem_About_Click(object sender, EventArgs e)
        {
            new AboutForm().Show();
        }

        private void ToolStripMenuItem_BinaryWriter_Click(object sender, EventArgs e)
        {
            new BinaryWriterForm().Show();
        }
    }
}