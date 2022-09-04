using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace RedSniff.RiffLang
{
    public class ResolvedFilter
    {
        public List<uint> AllowedSrcPorts { get; }
        public List<uint> AllowedDstPorts { get; }
        public List<string> AllowedSrcIps { get; }
        public List<string> AllowedDstIps { get; }

        public ResolvedFilter(List<uint> allowedSrcPorts, List<uint> allowedDstPorts, List<string> allowedSrcIps, List<string> allowedDstIps)
        {
            AllowedSrcPorts = allowedSrcPorts;
            AllowedDstPorts = allowedDstPorts;
            AllowedSrcIps = allowedSrcIps;
            AllowedDstIps = allowedDstIps;
        }
    }
}
