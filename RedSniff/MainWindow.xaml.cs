using RedSniff.Headers;
using RedSniff.RiffLang;
using RedSniff.Windows;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Sockets;
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
        public ObservableCollection<PacketEntry> ListItems = new ObservableCollection<PacketEntry>();
        bool showLineNumbers = true;
        bool showTextRepresentation = true;

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

            btnShowLineNumbers.Click += btnShowLineNumbers_Click;
            btnShowText.Click += btnShowText_Click;

            dataGrid.ItemsSource = ListItems;

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
                DataEncoding encoding;
                Enum.TryParse(cmbEncoding.Text, out encoding);
                dataTextBox.Text = row.DumpData(encoding, showLineNumbers, showTextRepresentation);
            }
        }

        void cmbEncoding_Changed(object sender, RoutedEventArgs e)
        {
            var row = dataGrid.SelectedItem as PacketEntry;
            if (row != null && sender != null)
            {
                DataEncoding encoding;
                var selectedItemStr = (sender as ComboBox)!.SelectedItem as string;
                Enum.TryParse(selectedItemStr, out encoding);
                dataTextBox.Text = row.DumpData(encoding, showLineNumbers, showTextRepresentation);
            }
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
            var aboutWin = new AboutWindow();
            aboutWin.Show();
        }

        void btnShowLineNumbers_Click(object sender, RoutedEventArgs e)
        {
            showLineNumbers = showLineNumbers == true ? false : true;

            var row = dataGrid.SelectedItem as PacketEntry;
            if (row != null && sender != null)
            {
                DataEncoding encoding;
                Enum.TryParse(cmbEncoding.Text, out encoding);
                dataTextBox.Text = row.DumpData(encoding, showLineNumbers, showTextRepresentation);
            }
        }

        void btnShowText_Click(object sender, RoutedEventArgs e)
        {
            showTextRepresentation = showTextRepresentation == true ? false : true;

            var row = dataGrid.SelectedItem as PacketEntry;
            if (row != null && sender != null)
            {
                DataEncoding encoding;
                Enum.TryParse(cmbEncoding.Text, out encoding);
                dataTextBox.Text = row.DumpData(encoding, showLineNumbers, showTextRepresentation);
            }
        }

        void filterTextBox_OnKeyDown(object sender, KeyEventArgs e)
        {
            Trace.WriteLine(filterTextBox.CaretIndex);
            var lines = filterTextBox.Text.Split("\n");
            var numDigits = Helper.NumDigits(lines.Length);

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
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message, "RedSniff", MessageBoxButton.OK, MessageBoxImage.Error);
                ExecStop();
            }

        }

        

        

        public void ExecStop()
        {
            btnStart.IsEnabled = true;
            btnStart.Opacity = 1.0;
            btnStop.IsEnabled = false;
            btnStop.Opacity = 0.3;
            btnRestart.IsEnabled = false;
            btnRestart.Opacity = 0.3;

            _packetSniffer!.Stop();
        }

        void execRestart()
        {
            ExecStop();
            execStart();
        }
    }
}
