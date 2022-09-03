using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace RedSniff.Headers
{
    internal class UdpHeader
    {
        // UDP header fields
        private ushort _sourcePort;        // Sixteen bits for the source port number        
        private ushort _destinationPort;   // Sixteen bits for the destination port number
        private ushort _length;            // Length of the UDP header
        private short _checksum;           // Sixteen bits for the checksum
                                           // (checksum can be negative so taken as short)              

        private byte[] _udpData = new byte[65535];  // Data carried by the UDP packet

        public UdpHeader(byte[] byBuffer, int nReceived)
        {
            MemoryStream memoryStream = new MemoryStream(byBuffer, 0, nReceived);
            BinaryReader binaryReader = new BinaryReader(memoryStream);

            // The first sixteen bits contain the source port
            _sourcePort = (ushort)IPAddress.NetworkToHostOrder(binaryReader.ReadInt16());

            // The next sixteen bits contain the destination port
            _destinationPort = (ushort)IPAddress.NetworkToHostOrder(binaryReader.ReadInt16());

            // The next sixteen bits contain the length of the UDP packet
            _length = (ushort)IPAddress.NetworkToHostOrder(binaryReader.ReadInt16());

            // The next sixteen bits contain the checksum
            _checksum = IPAddress.NetworkToHostOrder(binaryReader.ReadInt16());

            // Copy the data carried by the UDP packet into the data buffer
            Array.Copy(byBuffer,
                       8,               // The UDP header is of 8 bytes so we start copying after it
                       _udpData,
                       0,
                       nReceived - 8);
        }

        public ushort SourcePort
        {
            get
            {
                return _sourcePort;
            }
        }

        public ushort DestinationPort
        {
            get
            {
                return _destinationPort;
            }
        }

        public ushort MessageLength
        {
            get
            {
                return _length;
            }
        }

        public string Checksum
        {
            get
            {
                // Return the checksum in hexadecimal format
                return string.Format("0x{0:x2}", _checksum);
            }
        }

        public byte[] Data
        {
            get
            {
                return _udpData;
            }
        }
    }
}
