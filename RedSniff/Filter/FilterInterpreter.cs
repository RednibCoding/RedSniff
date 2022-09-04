using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Threading;
using RedSniff.Sniffer;

namespace RedSniff.Filter
{
    public static class FilterInterpreter
    {
        public static bool IsAllowedPacket(PacketEntry packet, ResolvedFilter filter)
        {
            if (filter.AllowedSrcPorts.Contains(packet.SrcPort) ||
                    filter.AllowedDstPorts.Contains(packet.DstPort) ||
                    filter.AllowedSrcIps.Contains(packet.SrcIp) ||
                    filter.AllowedDstIps.Contains(packet.DstIp))
            {
                return true;
            }
            return false;
        }
    }
}
