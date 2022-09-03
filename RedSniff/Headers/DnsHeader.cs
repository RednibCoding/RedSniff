using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace RedSniff.Headers
{
    internal class DnsHeader
    {
        // DNS header fields
        ushort _identification;        // Sixteen bits for identification
        ushort _flags;                 // Sixteen bits for DNS flags
        ushort _totalQuestions;        // Sixteen bits indicating the number of entries 
                                       // in the questions list
        ushort _totalAnswerRRs;        // Sixteen bits indicating the number of entries
                                       // entries in the answer resource record list
        ushort _totalAuthorityRRs;     // Sixteen bits indicating the number of entries
                                       // entries in the authority resource record list
        ushort _totalAdditionalRRs;    // Sixteen bits indicating the number of entries
                                       // entries in the additional resource record list

        public DnsHeader(byte[] byBuffer, int nReceived)
        {
            MemoryStream memoryStream = new MemoryStream(byBuffer, 0, nReceived);
            BinaryReader binaryReader = new BinaryReader(memoryStream);

            // First sixteen bits are for identification
            _identification = (ushort)IPAddress.NetworkToHostOrder(binaryReader.ReadInt16());

            // Next sixteen contain the flags
            _flags = (ushort)IPAddress.NetworkToHostOrder(binaryReader.ReadInt16());

            // Read the total numbers of questions in the quesion list
            _totalQuestions = (ushort)IPAddress.NetworkToHostOrder(binaryReader.ReadInt16());

            // Read the total number of answers in the answer list
            _totalAnswerRRs = (ushort)IPAddress.NetworkToHostOrder(binaryReader.ReadInt16());

            // Read the total number of entries in the authority list
            _totalAuthorityRRs = (ushort)IPAddress.NetworkToHostOrder(binaryReader.ReadInt16());

            // Total number of entries in the additional resource record list
            _totalAdditionalRRs = (ushort)IPAddress.NetworkToHostOrder(binaryReader.ReadInt16());
        }

        public string Identification
        {
            get
            {
                return string.Format("0x{0:x2}", _identification);
            }
        }

        public string Flags
        {
            get
            {
                return string.Format("0x{0:x2}", _flags);
            }
        }

        public string TotalQuestions
        {
            get
            {
                return _totalQuestions.ToString();
            }
        }

        public string TotalAnswerRRs
        {
            get
            {
                return _totalAnswerRRs.ToString();
            }
        }

        public string TotalAuthorityRRs
        {
            get
            {
                return _totalAuthorityRRs.ToString();
            }
        }

        public string TotalAdditionalRRs
        {
            get
            {
                return _totalAdditionalRRs.ToString();
            }
        }
    }
}
