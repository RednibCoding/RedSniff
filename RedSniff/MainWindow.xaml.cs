using RedSniff.Headers;
using RedSniff.Filter;
using RedSniff.Dialogs;
using RedSniff.Sniffer;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Reflection.PortableExecutable;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace RedSniff
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        PacketSniffer? _packetSniffer;
        public ObservableCollection<PacketEntry> DataGridItems = new ObservableCollection<PacketEntry>();
        internal List<PacketEntry> PacketEntries = new List<PacketEntry>();
        bool _showLineNumbers = true;
        bool _showTextRepresentation = true;
        bool _showHeader = true;
        PacketEntry? _selectedPacketEntry;

        public MainWindow()
        {
            InitializeComponent();

            registerButtonEvents();
            fillCmbInterfaces();
            fillCmbDataEncoding();

        }

        void registerButtonEvents()
        {
            btnStart.Click += btnStart_Click;
            btnRestart.Click += btnRestart_Click;
            btnStop.Click += btnStop_Click;
            btnAbout.Click += btnAbout_Click;
            btnFilterHelp.Click += btnFilterHelp_Click;
            btnApplyFilter.Click += btnApplyFilter_Click;
            btnResetFilter.Click += btnResetFilter_Click;

            btnShowLineNumbers.Click += btnShowLineNumbers_Click;
            btnShowText.Click += btnShowText_Click;
            btnShowHeader.Click += btnShowHead_Click;

            dataGrid.ItemsSource = DataGridItems;

            // Set button visuals
            btnStart.IsEnabled = true;
            btnStart.Opacity = 1.0;
            btnStop.IsEnabled = false;
            btnStop.Opacity = 0.3;
            btnRestart.IsEnabled = false;
            btnRestart.Opacity = 0.3;
        }

        void dataGrid_Row_Click(object sender, RoutedEventArgs e)
        {
            var row = dataGrid.SelectedItem as PacketEntry;
            if (row != null)
            {
                _selectedPacketEntry = row;
                if (_selectedPacketEntry != null)
                    makeDataOutput(cmbEncoding.Text);
            }
        }

        void cmbEncoding_Changed(object sender, RoutedEventArgs e)
        {
            var selectedItemStr = (sender as ComboBox)!.SelectedItem as string;
            if (selectedItemStr != null && _selectedPacketEntry != null)
                makeDataOutput(selectedItemStr);
        }

        void btnStop_Click(object sender, RoutedEventArgs e)
        {
            ExecStop();
        }

        void btnRestart_Click(object sender, RoutedEventArgs e)
        {
            execRestart();
        }

        void btnStart_Click(object sender, RoutedEventArgs e)
        {
            execStart();
        }

        void btnAbout_Click(object sender, RoutedEventArgs e)
        {
            new AboutWindow().Show();
        }

        void btnFilterHelp_Click(object sender, RoutedEventArgs e)
        {
            new FilterHelpWindow().Show();
        }

        void btnApplyFilter_Click(object sender, RoutedEventArgs e)
        {
            FilterProgram? filterPrg = null;
            ResolvedFilter? resolvedFilter = null;
            if (filterTextBox.Text.Length > 0)
            {
                filterPrg = FilterParser.ParseInput(filterTextBox.Text);
                if (filterPrg == null) return;
                resolvedFilter = FilterResolver.ResolveFilter(filterPrg);
                if (resolvedFilter == null) return;
            }
            else
            {
                return;
            }

            DataGridItems.Clear();
            foreach (var packetEntry in PacketEntries)
            {
                if (FilterInterpreter.IsAllowedPacket(packetEntry, resolvedFilter))
                    if (packetEntry != null) DataGridItems.Add(packetEntry);
            }
        }

        void btnResetFilter_Click(object sender, RoutedEventArgs e)
        {
            DataGridItems.Clear();
            foreach (var packetEntry in PacketEntries)
                DataGridItems.Add(packetEntry);
        }

        void btnShowLineNumbers_Click(object sender, RoutedEventArgs e)
        {
            _showLineNumbers = _showLineNumbers == true ? false : true;
            if (_selectedPacketEntry != null)
                makeDataOutput(cmbEncoding.Text);
            
        }

        void btnShowText_Click(object sender, RoutedEventArgs e)
        {
            _showTextRepresentation = _showTextRepresentation == true ? false : true;
            if (_selectedPacketEntry != null)
                makeDataOutput(cmbEncoding.Text);
        }

        void btnShowHead_Click(object sender, RoutedEventArgs e)
        {
            _showHeader = _showHeader == true ? false : true;
            if (_selectedPacketEntry != null)
                makeDataOutput(cmbEncoding.Text);
        }

        void filterTextBox_OnKeyDown(object sender, KeyEventArgs e)
        {
            var lines = filterTextBox.Text.Split("\n");
            var numDigits = Helpers.NumDigits(lines.Length);

            var lineNumbers = new StringBuilder();
            for (var i = 0; i < lines.Length; i++)
            {
                if (e.Key == Key.Back && i == lines.Length - 1 && filterTextBox.CaretIndex <= 1) break;
                lineNumbers.Append($"{i+1}\n");
            }

            if (e.Key == Key.Return || e.Key == Key.Enter)
            {
                lineNumbers.Append($"{lines.Length+1}\n");
            }
        }


        void fillCmbInterfaces()
        {
            var hosyEntry = Dns.GetHostEntry((Dns.GetHostName()));
            if (hosyEntry.AddressList.Length > 0)
            {
                foreach (IPAddress ip in hosyEntry.AddressList)
                {
                    if (ip.AddressFamily == AddressFamily.InterNetwork)
                    {
                        var strIp = ip.ToString();
                        cmbInterfaces.Items.Add(strIp);
                    }
                }
                cmbInterfaces.Items.Add(IPAddress.Loopback);

                if (cmbInterfaces.Items.Count > 0)
                    cmbInterfaces.SelectedIndex = 0;
                else
                {
                    MessageBox.Show("No network adapters found", "RedSniff", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        void fillCmbDataEncoding()
        {
            string[] encodings = Enum.GetNames(typeof(DataEncoding));
            foreach (var encoding in encodings)
            {
                cmbEncoding.Items.Add(encoding);
            }
            cmbEncoding.SelectedIndex = 0;
        }

        void execStart()
        {
            if (string.IsNullOrWhiteSpace(cmbInterfaces.Text))
                MessageBox.Show("Select an interface to capture the packets", "RedSniff", MessageBoxButton.OK, MessageBoxImage.Error);

            List<Protocols> protocolsToSniff = new();
            if (tcpCheckBox.IsChecked ?? false)
                protocolsToSniff.Add(Protocols.TCP);
            if (udpCheckBox.IsChecked ?? false)
                protocolsToSniff.Add(Protocols.UDP);

            if (protocolsToSniff.Count <= 0)
            {
                MessageBox.Show("Select at least one protocol", "RedSniff", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            try
            {
                FilterProgram? filterPrg = null;
                ResolvedFilter? filter = null;
                if (filterTextBox.Text.Length > 0)
                {
                    filterPrg = FilterParser.ParseInput(filterTextBox.Text);
                    if (filterPrg == null) return;
                    filter = FilterResolver.ResolveFilter(filterPrg);
                    if (filter == null) return;
                }

                

                _packetSniffer = new PacketSniffer();
                _packetSniffer.Start(cmbInterfaces.Text, protocolsToSniff, filter);

                // Update button visuals
                btnStart.IsEnabled = false;
                btnStart.Opacity = 0.3;
                btnStop.IsEnabled = true;
                btnStop.Opacity = 1.0;
                btnRestart.IsEnabled = true;
                btnRestart.Opacity = 1.0;
                btnApplyFilter.IsEnabled = false;
                btnApplyFilter.Opacity = 0.3;
                btnResetFilter.IsEnabled = false;
                btnResetFilter.Opacity = 0.3;
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message, "RedSniff", MessageBoxButton.OK, MessageBoxImage.Error);
                ExecStop();
            }

        }

        void execRestart()
        {
            ExecStop();
            execStart();
        }

        public void ExecStop()
        {
            btnStart.IsEnabled = true;
            btnStart.Opacity = 1.0;
            btnStop.IsEnabled = false;
            btnStop.Opacity = 0.3;
            btnRestart.IsEnabled = false;
            btnRestart.Opacity = 0.3;
            btnApplyFilter.IsEnabled = true;
            btnApplyFilter.Opacity = 1.0;
            btnResetFilter.IsEnabled = true;
            btnResetFilter.Opacity = 1.0;

            _packetSniffer!.Stop();
        }

        void makeDataOutput(string encodingStr)
        {
            if (_selectedPacketEntry == null) return;

            DataEncoding encoding;
            var success = Enum.TryParse(encodingStr, out encoding);
            if (!success) return;

            var dataOutput = new StringBuilder();
            if (_showHeader)
                dataOutput.Append($"Packet size: {_selectedPacketEntry.TotalSize} bytes\nProtocol:    {_selectedPacketEntry.Protocol}\nFrom:        {_selectedPacketEntry.SrcIp}:{_selectedPacketEntry.SrcPort}\nTo:          {_selectedPacketEntry.DstIp}:{_selectedPacketEntry.DstPort}\nCaptured:    {DateTime.Now.ToString("yyyy-MM-dd ", CultureInfo.InvariantCulture)}{_selectedPacketEntry.Captured}\n\n\n");
            dataOutput.Append(_selectedPacketEntry.DumpData(encoding, _showLineNumbers, _showTextRepresentation));

            dataTextBox.Text = dataOutput.ToString();
        }
    }
}
