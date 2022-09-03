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
using RedSniff.RiffLang;
using System.Globalization;

namespace RedSniff
{

    internal class PacketSniffer: DependencyObject
    {
        static List<Protocols>? _typesToSniff;
        static Socket? _mainSocket;                  // The socket which captures all incoming packets
        static byte[] _byteData = new byte[65535];
        static int _currentRowIndex;
        static bool _keepSniffing = false;
        static ResolvedFilter? _filter;

        public void Stop()
        {
            _mainSocket?.Close();
            _currentRowIndex = 1;
            _keepSniffing = false;
            _byteData = new byte[65535];
        }

        public void Start(string interfaceToSniff, List<Protocols> typesToSniff, ResolvedFilter? filter)
        {
            _filter = filter;
            _keepSniffing = true;
            _typesToSniff = typesToSniff;
            _currentRowIndex = 1;

            if (_keepSniffing)
            {
                // Start capturing the packets..

                // Thread safe clearing the datagrid
                Dispatcher.BeginInvoke((Action)(() =>
                {
                    ((MainWindow)Application.Current.MainWindow).ListItems.Clear();
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
            var captureTime = DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss.ffff", CultureInfo.InvariantCulture);
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

            makeDataGridRow(ipHeader, tcpHeader!, udpHeader!, dnsHeader!, captureTime);



        }

        void makeDataGridRow(IpHeader? ipHeader, TcpHeader? tcpHeader, UdpHeader? udpHeader, DnsHeader? dnsHeader, string captureTime)
        {
            PacketEntry? item = null;
            if (ipHeader == null) return;

            if (_typesToSniff!.Contains(Protocols.TCP) && tcpHeader != null && ipHeader != null)
            {
                item = new PacketEntry();
                item.Id = _currentRowIndex;
                item.Protocol = "TCP";
                item.SrcIp = ipHeader.SourceAddress.ToString();
                item.DstIp = ipHeader.DestinationAddress.ToString();
                item.SrcPort = tcpHeader.SourcePort;
                item.DstPort = tcpHeader.DestinationPort;
                item.Flags = tcpHeader.Flags;
                item.TotalSize = ipHeader.TotalLength;
                item.MsgSize = tcpHeader.MessageLength;
                item.Captured = captureTime;
                item.Data = tcpHeader.Data;
                _currentRowIndex++;
            } else if (_typesToSniff!.Contains(Protocols.UDP) && udpHeader != null && ipHeader != null)
            {
                item = new PacketEntry();
                item.Id = _currentRowIndex;
                item.Protocol = "UDP";
                item.SrcIp = ipHeader.SourceAddress.ToString();
                item.DstIp = ipHeader.DestinationAddress.ToString();
                item.SrcPort = udpHeader.SourcePort;
                item.DstPort = udpHeader.DestinationPort;
                item.MsgSize = udpHeader.MessageLength;
                item.TotalSize = ipHeader.TotalLength;
                item.Captured = captureTime;
                item.Data = udpHeader.Data;
                _currentRowIndex++;
            }

            if (item == null) return;

            // Apply filterProgram
            if (_filter != null)
            {
                if (_filter.AllowedSrcPorts.Contains(item.SrcPort) ||
                    _filter.AllowedDstPorts.Contains(item.DstPort) ||
                    _filter.AllowedSrcIps.Contains(ipHeader!.SourceAddress) ||
                    _filter.AllowedDstIps.Contains(ipHeader!.DestinationAddress))
                {
                    // Thread safe adding items
                    Dispatcher.BeginInvoke((Action)(() =>
                    {
                        if (item != null && ((MainWindow)Application.Current.MainWindow) != null)
                            ((MainWindow)Application.Current.MainWindow).ListItems.Add(item);
                    }));
                }
            }
            else
            {
                // Thread safe adding items
                Dispatcher.BeginInvoke((Action)(() =>
                {
                    if (item != null && ((MainWindow)Application.Current.MainWindow) != null)
                        ((MainWindow)Application.Current.MainWindow).ListItems.Add(item);
                }));
            }
            
        }
    }
}
