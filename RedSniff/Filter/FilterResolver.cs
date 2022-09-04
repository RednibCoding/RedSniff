using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace RedSniff.Filter
{
    public static class FilterResolver
    {
        public static ResolvedFilter? ResolveFilter(FilterProgram program)
        {
            if (program == null) return null;

            List<uint> allowedSrcPorts = new();
            List<uint> allowedDstPorts = new();
            List<string> allowedSrcIps = new();
            List<string> allowedDstIps = new();

            foreach (var statement in program.Statements)
            {
                if (statement.GetType() == typeof(PortStatement))
                {
                    var portStatement = (PortStatement)statement;

                    if (portStatement.Ports != null && portStatement.AffectionType == AffectionType.src)
                        allowedSrcPorts.AddRange(portStatement.Ports);
                    else if (portStatement.Ports != null && portStatement.AffectionType == AffectionType.dst)
                        allowedDstPorts.AddRange(portStatement.Ports);
                    else if (portStatement.Ports != null && portStatement.AffectionType == AffectionType.both)
                    {
                        allowedSrcPorts.AddRange(portStatement.Ports);
                        allowedDstPorts.AddRange(portStatement.Ports);
                    }

                }
                else if (statement.GetType() == typeof(IpStatement))
                {
                    var ipStatement = (IpStatement)statement;

                    if (ipStatement.IpAdresses != null && ipStatement.AffectionType == AffectionType.src)
                        allowedSrcIps.AddRange(ipStatement.IpAdresses);
                    else if (ipStatement.IpAdresses != null && ipStatement.AffectionType == AffectionType.dst)
                        allowedDstIps.AddRange(ipStatement.IpAdresses);
                    else if (ipStatement.IpAdresses != null && ipStatement.AffectionType == AffectionType.both)
                    {
                        allowedSrcIps.AddRange(ipStatement.IpAdresses);
                        allowedDstIps.AddRange(ipStatement.IpAdresses);
                    }
                }
            }

            return new ResolvedFilter(allowedSrcPorts, allowedDstPorts, allowedSrcIps, allowedDstIps);
        }
    }
}
