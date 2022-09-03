using System;
using System.IO;
using System.Net;

using System.Windows;

namespace RedSniff.Headers
{
    internal class IpHeader
    {
        // IP Header fields
        private byte _versionAndHeaderLength;     // Eight bits for version and header length
        private byte _differentiatedServices;     // Eight bits for differentiated services (TOS)
        private ushort _totalLength;              // Sixteen bits for total length of the datagram (header + message)
        private ushort _identification;           // Sixteen bits for identification
        private ushort _flagsAndOffset;           // Eight bits for flags and fragmentation offset
        private byte _ttl;                        // Eight bits for TTL (Time To Live)
        private byte _protocol;                   // Eight bits for the underlying protocol
        private short _checksum;                  // Sixteen bits containing the checksum of the header
                                                  // (checksum can be negative so taken as short)
        private uint _sourceIPAddress;            // Thirty two bit source IP Address
        private uint _destinationIPAddress;       // Thirty two bit destination IP Address
                                                  // End IP Header fields

        private byte _headerLength;               // Header length
        private byte[] _data = new byte[65535];  // Data carried by the datagram


        public IpHeader(byte[] bytesReceived, int nReceived)
        {
            BinaryReader binaryReader = new BinaryReader(new MemoryStream(bytesReceived, 0, nReceived));

            // The first eight bits of the IP header contain the version and
            // header length so we read them
            _versionAndHeaderLength = binaryReader.ReadByte();

            // The next eight bits contain the Differentiated services
            _differentiatedServices = binaryReader.ReadByte();

            // Next eight bits hold the total length of the datagram
            _totalLength = (ushort)IPAddress.NetworkToHostOrder(binaryReader.ReadInt16());

            // Next sixteen have the identification bytes
            _identification = (ushort)IPAddress.NetworkToHostOrder(binaryReader.ReadInt16());

            // Next sixteen bits contain the flags and fragmentation offset
            _flagsAndOffset = (ushort)IPAddress.NetworkToHostOrder(binaryReader.ReadInt16());

            // Next eight bits have the TTL value
            _ttl = binaryReader.ReadByte();

            // Next eight represnts the protocol encapsulated in the datagram
            _protocol = binaryReader.ReadByte();

            // Next sixteen bits contain the checksum of the header
            _checksum = IPAddress.NetworkToHostOrder(binaryReader.ReadInt16());

            // Next thirty two bits have the source IP address
            _sourceIPAddress = (uint)(binaryReader.ReadInt32());

            // Next thirty two hold the destination IP address
            _destinationIPAddress = (uint)(binaryReader.ReadInt32());

            // Now we calculate the header length

            _headerLength = _versionAndHeaderLength;
            // The last four bits of the version and header length field contain the
            // header length, we perform some simple binary airthmatic operations to
            // extract them
            _headerLength <<= 4;
            _headerLength >>= 4;
            // Multiply by four to get the exact header length
            _headerLength *= 4;

            // Copy the data carried by the data gram into another array so that
            // according to the protocol being carried in the IP datagram
            var count = _totalLength < _headerLength ? 0 : _totalLength - _headerLength;
            Array.Copy(bytesReceived, _headerLength, _data, 0, count);
        }

        public string Version
        {
            get
            {
                // Calculate the IP version

                // The four bits of the IP header contain the IP version
                if ((_versionAndHeaderLength >> 4) == 4)
                {
                    return "IP v4";
                }
                else if ((_versionAndHeaderLength >> 4) == 6)
                {
                    return "IP v6";
                }
                else
                {
                    return "Unknown";
                }
            }
        }

        public byte HeaderLength
        {
            get
            {
                return _headerLength;
            }
        }

        public ushort MessageLength
        {
            get
            {
                // MessageLength = Total length of the datagram - Header length
                return (ushort)(_totalLength - _headerLength);
            }
        }

        public string DifferentiatedServices
        {
            get
            {
                // Returns the differentiated services in hexadecimal format
                return string.Format("0x{0:x2} ({1})", _differentiatedServices,
                    _differentiatedServices);
            }
        }

        public string Flags
        {
            get
            {
                // The first three bits of the flags and fragmentation field 
                // represent the flags (which indicate whether the data is 
                // fragmented or not)
                int nFlags = _flagsAndOffset >> 13;
                if (nFlags == 2)
                {
                    return "Don't fragment";
                }
                else if (nFlags == 1)
                {
                    return "More fragments to come";
                }
                else
                {
                    return nFlags.ToString();
                }
            }
        }

        public string FragmentationOffset
        {
            get
            {
                // The last thirteen bits of the flags and fragmentation field 
                // contain the fragmentation offset
                int nOffset = _flagsAndOffset << 3;
                nOffset >>= 3;

                return nOffset.ToString();
            }
        }

        public string TTL
        {
            get
            {
                return _ttl.ToString();
            }
        }

        public Protocols ProtocolType
        {
            get
            {
                // The protocol field represents the protocol in the data portion
                // of the datagram
                if (_protocol == 6)        // A value of six represents the TCP protocol
                {
                    return Protocols.TCP;
                }
                else if (_protocol == 17)  // Seventeen for UDP
                {
                    return Protocols.UDP;
                }
                else
                {
                    return Protocols.Unknown;
                }
            }
        }

        public string Checksum
        {
            get
            {
                // Returns the checksum in hexadecimal format
                return string.Format("0x{0:x2}", _checksum);
            }
        }

        public IPAddress SourceAddress
        {
            get
            {
                return new IPAddress(_sourceIPAddress);
            }
        }

        public IPAddress DestinationAddress
        {
            get
            {
                return new IPAddress(_destinationIPAddress);
            }
        }

        public ushort TotalLength
        {
            get
            {
                return _totalLength;
            }
        }

        public string Identification
        {
            get
            {
                return _identification.ToString();
            }
        }

        public byte[] Data
        {
            get
            {
                return _data;
            }
        }
    }
}
