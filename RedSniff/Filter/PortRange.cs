using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RedSniff.Filter
{
    public class PortRange
    {
        uint _lowerPort;
        uint _upperPort;

        public PortRange(uint lowerInclusive, uint upperInclusive)
        {
            _lowerPort = lowerInclusive;
            _upperPort = upperInclusive;
        }

        public bool IsInRange(uint port)
        {
            return _lowerPort >= port && _upperPort <= port;
        }
    }
}
