using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace RedSniff.Headers
{
    internal class TcpHeader
    {
        // TCP header fields
        private ushort _sourcePort;                     // Sixteen bits for the source port number
        private ushort _destinationPort;                // Sixteen bits for the destination port number
        private uint _sequenceNumber = 555;             // Thirty two bits for the sequence number
        private uint _acknowledgementNumber = 555;      // Thirty two bits for the acknowledgement number
        private ushort _dataOffsetAndFlags = 555;       // Sixteen bits for flags and data offset
        private ushort _window = 555;                   // Sixteen bits for the window size
        private short _checksum = 555;                  // Sixteen bits for the checksum
                                                        // (checksum can be negative so taken as short)
        private ushort _urgentPointer;                  // Sixteen bits for the urgent pointer


        private byte _headerLength;                 // Header length
        private ushort _messageLength;              // Length of the data being carried
        private byte[] _tcpData = new byte[65535];   // Data carried by the TCP packet

        public TcpHeader(byte[] buffer, int nReceived)
        {
           
            MemoryStream memoryStream = new MemoryStream(buffer, 0, nReceived);
            BinaryReader binaryReader = new BinaryReader(memoryStream);

            // The first sixteen bits contain the source port
            _sourcePort = (ushort)IPAddress.NetworkToHostOrder(binaryReader.ReadInt16());

            // The next sixteen contain the destiination port
            _destinationPort = (ushort)IPAddress.NetworkToHostOrder(binaryReader.ReadInt16());

            // Next thirty two have the sequence number
            _sequenceNumber = (uint)IPAddress.NetworkToHostOrder(binaryReader.ReadInt32());

            // Next thirty two have the acknowledgement number
            _acknowledgementNumber = (uint)IPAddress.NetworkToHostOrder(binaryReader.ReadInt32());

            // The next sixteen bits hold the flags and the data offset
            _dataOffsetAndFlags = (ushort)IPAddress.NetworkToHostOrder(binaryReader.ReadInt16());

            // The next sixteen contain the window size
            _window = (ushort)IPAddress.NetworkToHostOrder(binaryReader.ReadInt16());

            // In the next sixteen we have the checksum
            _checksum = (short)IPAddress.NetworkToHostOrder(binaryReader.ReadInt16());

            // The following sixteen contain the urgent pointer
            _urgentPointer = (ushort)IPAddress.NetworkToHostOrder(binaryReader.ReadInt16());

            // The data offset indicates where the data begins, so using it we
            // calculate the header length
            _headerLength = (byte)(_dataOffsetAndFlags >> 12);
            _headerLength *= 4;

            // Message length = Total length of the TCP packet - Header length
            _messageLength = (ushort)(nReceived - _headerLength);

            // Copy the TCP data into the data buffer
            Array.Copy(buffer, _headerLength, _tcpData, 0, nReceived - _headerLength);

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

        public string SequenceNumber
        {
            get
            {
                return _sequenceNumber.ToString();
            }
        }

        public string AcknowledgementNumber
        {
            get
            {
                // If the ACK flag is set then only we have a valid value in
                // the acknowlegement field, so check for it before returning 
                // anything
                if ((_dataOffsetAndFlags & 0x10) != 0)
                {
                    return _acknowledgementNumber.ToString();
                }
                else
                    return "";
            }
        }

        public string HeaderLength
        {
            get
            {
                return _headerLength.ToString();
            }
        }

        public string WindowSize
        {
            get
            {
                return _window.ToString();
            }
        }

        public string UrgentPointer
        {
            get
            {
                // If the URG flag is set then only we have a valid value in
                // the urgent pointer field, so check for it beore returning 
                // anything
                if ((_dataOffsetAndFlags & 0x20) != 0)
                {
                    return _urgentPointer.ToString();
                }
                else
                    return "";
            }
        }

        public string Flags
        {
            get
            {
                // The last six bits of the data offset and flags contain the
                // control bits

                // First we extract the flags
                int nFlags = _dataOffsetAndFlags & 0x3F;

                string strFlags = string.Format("0x{0:x2} (", nFlags);

                // Now we start looking whether individual bits are set or not
                if ((nFlags & 0x01) != 0)
                {
                    strFlags += "FIN, ";
                }
                if ((nFlags & 0x02) != 0)
                {
                    strFlags += "SYN, ";
                }
                if ((nFlags & 0x04) != 0)
                {
                    strFlags += "RST, ";
                }
                if ((nFlags & 0x08) != 0)
                {
                    strFlags += "PSH, ";
                }
                if ((nFlags & 0x10) != 0)
                {
                    strFlags += "ACK, ";
                }
                if ((nFlags & 0x20) != 0)
                {
                    strFlags += "URG";
                }
                strFlags += ")";

                if (strFlags.Contains("()"))
                {
                    strFlags = strFlags.Remove(strFlags.Length - 3);
                }
                else if (strFlags.Contains(", )"))
                {
                    strFlags = strFlags.Remove(strFlags.Length - 3, 2);
                }

                return strFlags;
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
                return _tcpData;
            }
        }

        public ushort MessageLength
        {
            get
            {
                return _messageLength;
            }
        }
    }
}
