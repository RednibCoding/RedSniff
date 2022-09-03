using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RedSniff
{
    internal class Helper
    {
        public static int NumDigits(long number)
        {
            if (number >= 0)
            {
                if (number < 10L) return 1;
                if (number < 100L) return 2;
                if (number < 1000L) return 3;
                if (number < 10000L) return 4;
                if (number < 100000L) return 5;
                if (number < 1000000L) return 6;
                if (number < 10000000L) return 7;
                if (number < 100000000L) return 8;
                if (number < 1000000000L) return 9;
                if (number < 10000000000L) return 10;
                if (number < 100000000000L) return 11;
                if (number < 1000000000000L) return 12;
                if (number < 10000000000000L) return 13;
                if (number < 100000000000000L) return 14;
                if (number < 1000000000000000L) return 15;
                if (number < 10000000000000000L) return 16;
                if (number < 100000000000000000L) return 17;
                if (number < 1000000000000000000L) return 18;
                return 19;
            }
            else
            {
                if (number > -10L) return 2;
                if (number > -100L) return 3;
                if (number > -1000L) return 4;
                if (number > -10000L) return 5;
                if (number > -100000L) return 6;
                if (number > -1000000L) return 7;
                if (number > -10000000L) return 8;
                if (number > -100000000L) return 9;
                if (number > -1000000000L) return 10;
                if (number > -10000000000L) return 11;
                if (number > -100000000000L) return 12;
                if (number > -1000000000000L) return 13;
                if (number > -10000000000000L) return 14;
                if (number > -100000000000000L) return 15;
                if (number > -1000000000000000L) return 16;
                if (number > -10000000000000000L) return 17;
                if (number > -100000000000000000L) return 18;
                if (number > -1000000000000000000L) return 19;
                return 20;
            }
        }
    }
}
