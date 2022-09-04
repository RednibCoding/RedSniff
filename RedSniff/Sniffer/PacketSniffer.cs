using RedSniff.Headers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;
using RedSniff.Filter;
using System.Globalization;

namespace RedSniff.Sniffer
{

    internal class PacketSniffer: DependencyObject
    {
        static List<Protocols>? _typesToSniff;
        static Socket? _mainSocket;                  // The socket which captures all incoming packets
        static byte[] _byteData = new byte[65535];
        static int _currentRowIndex;
        static bool _keepSniffing = false;
        static ResolvedFilter? _resolvedFilter;

        public void Stop()
        {
            _mainSocket?.Close();
            _currentRowIndex = 1;
            _keepSniffing = false;
            _byteData = new byte[65535];
        }

        public void Start(string interfaceToSniff, List<Protocols> typesToSniff, ResolvedFilter? resolvedFilter)
        {
            _resolvedFilter = resolvedFilter;
            _keepSniffing = true;
            _typesToSniff = typesToSniff;
            _currentRowIndex = 1;

            if (_keepSniffing)
            {
                // Start capturing the packets..

                // Thread safe clearing the datagrid
                Dispatcher.BeginInvoke((Action)(() =>
                {
                    ((MainWindow)Application.Current.MainWindow).DataGridItems.Clear();
                    ((MainWindow)Application.Current.MainWindow).PacketEntries.Clear();
                }));

                // For sniffing the socket to capture the packets has to be a raw socket, with the
                // address family being of type internetwork, and protocol being IP
                _mainSocket = new Socket(AddressFamily.InterNetwork, SocketType.Raw, ProtocolType.IP);

                // Bind the socket to the selected IP address
                _mainSocket.Bind(new IPEndPoint(IPAddress.Parse(interfaceToSniff), 0));

                // Set the socket  options
                _mainSocket.SetSocketOption(SocketOptionLevel.IP,            // Applies only to IP packets
                                            SocketOptionName.HeaderIncluded, // Set to include the header
                                            true);

                byte[] byteTrue = new byte[4] { 1, 0, 0, 0 };
                byte[] byteOut = new byte[4] { 1, 0, 0, 0 }; // Capture incoming and outgoing packets

                // Socket.IOControl is analogous to the WSAIoctl method of Winsock 2, Equivalent to SIO_RCVALL constantof Winsock 2
                _mainSocket.IOControl(IOControlCode.ReceiveAll, byteTrue, byteOut);

                // Start receiving the packets asynchronously
                _mainSocket.BeginReceive(_byteData, 0, _byteData.Length, SocketFlags.None, new AsyncCallback(onReceive), null);

                

            }
        }

        void onReceive(IAsyncResult ar)
        {
            var captureTime = DateTime.Now.ToString("HH:mm:ss.ffff", CultureInfo.InvariantCulture);
            try
            {
                int nReceived = _mainSocket!.EndReceive(ar);

                if (nReceived > 0)
                    parseData(_byteData, nReceived, captureTime);

                _byteData = new byte[65535];

                // Another call to BeginReceive so that we continue to receive the incoming packets
                _mainSocket.BeginReceive(_byteData, 0, _byteData.Length, SocketFlags.None, new AsyncCallback(onReceive), null);
            }
            catch (ObjectDisposedException)
            {
            }
            catch (Exception)
            {
                // Give a shit if a packet could not be read or parsed, just continue to receive the incoming packets
                _byteData = new byte[65535];
                _mainSocket!.BeginReceive(_byteData, 0, _byteData.Length, SocketFlags.None, new AsyncCallback(onReceive), null);
            }

        }

        void parseData(byte[] byteData, int nReceived, string captureTime)
        {

            // Since all protocol packets are encapsulated in the IP datagram
            // so we start by parsing the IP header and see what protocol data
            // is being carried by it
            IpHeader ipHeader = new IpHeader(byteData, nReceived);

            TcpHeader? tcpHeader = null;
            UdpHeader? udpHeader = null;
            DnsHeader? dnsHeader = null;


            // Now according to the protocol being carried by the IP datagram we parse 
            // the data field of the datagram
            switch (ipHeader.ProtocolType)
            {
                case Protocols.TCP:
                    if (!_typesToSniff!.Contains(Protocols.TCP)) break;

                    tcpHeader = new TcpHeader(ipHeader.Data, ipHeader.MessageLength);  // IPHeader.Data stores the data being carried by the IP datagram

                    // If the port is equal to 53 then the underlying protocol is DNS
                    // Note: DNS can use either TCP or UDP thats why the check is done twice
                    if (tcpHeader.DestinationPort == 53 || tcpHeader.SourcePort == 53)
                    {
                        dnsHeader = new DnsHeader(tcpHeader.Data, (int)tcpHeader.MessageLength);
                    }
                    break;

                case Protocols.UDP:
                    if (!_typesToSniff!.Contains(Protocols.UDP)) break;
                    udpHeader = new UdpHeader(ipHeader.Data, (int)ipHeader.MessageLength);  // IPHeader.Data stores the data being carried by the IP datagram
                                                                                                                                


                    // If the port is equal to 53 then the underlying protocol is DNS
                    // Note: DNS can use either TCP or UDP thats why the check is done twice
                    if (udpHeader.DestinationPort == 53 || udpHeader.SourcePort == 53)
                    {
                        // Length of UDP header is always eight bytes so we subtract that out of the total 
                        // length to find the length of the data
                        dnsHeader = new DnsHeader(udpHeader.Data, Convert.ToInt32(udpHeader.MessageLength) - 8);
                    }

                    break;

                case Protocols.Unknown:
                    break;
            }

            addToPacketEntries(ipHeader, tcpHeader!, udpHeader!, dnsHeader!, captureTime);
        }

        void addToPacketEntries(IpHeader? ipHeader, TcpHeader? tcpHeader, UdpHeader? udpHeader, DnsHeader? dnsHeader, string captureTime)
        {
            PacketEntry? packetEntry = null;
            if (ipHeader == null) return;

            if (_typesToSniff!.Contains(Protocols.TCP) && tcpHeader != null && ipHeader != null)
            {
                packetEntry = new PacketEntry();
                packetEntry.Id = _currentRowIndex;
                packetEntry.Protocol = "TCP";
                packetEntry.SrcIp = ipHeader.SourceAddress.ToString();
                packetEntry.DstIp = ipHeader.DestinationAddress.ToString();
                packetEntry.SrcPort = tcpHeader.SourcePort;
                packetEntry.DstPort = tcpHeader.DestinationPort;
                packetEntry.Flags = tcpHeader.Flags;
                packetEntry.TotalSize = ipHeader.TotalLength;
                packetEntry.MsgSize = tcpHeader.MessageLength;
                packetEntry.Captured = captureTime;
                packetEntry.Data = tcpHeader.Data;
                _currentRowIndex++;
            } else if (_typesToSniff!.Contains(Protocols.UDP) && udpHeader != null && ipHeader != null)
            {
                packetEntry = new PacketEntry();
                packetEntry.Id = _currentRowIndex;
                packetEntry.Protocol = "UDP";
                packetEntry.SrcIp = ipHeader.SourceAddress.ToString();
                packetEntry.DstIp = ipHeader.DestinationAddress.ToString();
                packetEntry.SrcPort = udpHeader.SourcePort;
                packetEntry.DstPort = udpHeader.DestinationPort;
                packetEntry.MsgSize = udpHeader.MessageLength;
                packetEntry.TotalSize = ipHeader.TotalLength;
                packetEntry.Captured = captureTime;
                packetEntry.Data = udpHeader.Data;
                _currentRowIndex++;
            }

            if (packetEntry == null) return;

            // Apply filter
            if (_resolvedFilter != null)
            {
                if (FilterInterpreter.IsAllowedPacket(packetEntry, _resolvedFilter))
                {
                    // Thread safe adding items
                    Dispatcher.BeginInvoke((Action)(() =>
                    {
                        if (packetEntry != null && ((MainWindow)Application.Current.MainWindow) != null)
                            ((MainWindow)Application.Current.MainWindow).DataGridItems.Add(packetEntry);
                    }));
                }
            }
            else
            {
                // Thread safe adding items
                Dispatcher.BeginInvoke((Action)(() =>
                {
                    if (packetEntry != null && ((MainWindow)Application.Current.MainWindow) != null)
                        ((MainWindow)Application.Current.MainWindow).DataGridItems.Add(packetEntry);
                }));
            }

            // Thread safe adding items
            Dispatcher.BeginInvoke((Action)(() =>
            {
                if (packetEntry != null && ((MainWindow)Application.Current.MainWindow) != null)
                    ((MainWindow)Application.Current.MainWindow).PacketEntries.Add(packetEntry);
            }));

        }
    }
}
