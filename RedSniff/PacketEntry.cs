using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Redsniff
{
    internal class PacketEntry
    {
        public int Id { get; set; }
        public string Protocol { get; set; } = string.Empty;
        public string SrcIp { get; set; } = string.Empty;
        public string DstIp { get; set; } = string.Empty; 
        public int SrcPort { get; set; }
        public int DstPort { get; set; }
        public string Flags { get; set; } = string.Empty;
        public int MsgSize { get; set; }
        public int TotalSize { get; set; }
        public string Captured { get; set; } = string.Empty;
        public byte[] Data { get; set; } = { };
    }
}
