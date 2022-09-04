using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace RedSniff.Filter
{    public enum AffectionType
    {
        none,
        src,
        dst,
        both,
    }

    public class FilterProgram
    {
        public List<Statement> Statements { get; set; } = new();
    }

    public abstract class Statement
    {
        public AffectionType AffectionType { get; set; } = AffectionType.none;
    }

    public class IpStatement : Statement
    {
        public List<string>? IpAdresses { get; set; }
    }

    public class PortStatement : Statement
    {
        public List<uint>? Ports { get; set; }
    }
}
