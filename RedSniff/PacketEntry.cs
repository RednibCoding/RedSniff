using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Text.Unicode;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace RedSniff
{
    public class PacketEntry
    {
        public int Id { get; set; }
        public string Protocol { get; set; } = "";
        public string SrcIp { get; set; } = "";
        public string DstIp { get; set; } = "";
        public ushort SrcPort { get; set; }
        public ushort DstPort { get; set; }
        public string Flags { get; set; } = "";
        public ushort MsgSize { get; set; }
        public ushort TotalSize { get; set; }
        public string Captured { get; set; } = "";

        public byte[] Data = new byte[0];

        public string DumpData(DataEncoding encoding, bool showLineNumbers, bool showText)
        {
            var output = new StringBuilder();
            var tmp = new StringBuilder();
            for (var i = 0; i < TotalSize; i++)
            {
                if ((i+1) % 16 == 0)
                {
                    processLine(encoding, showLineNumbers, showText, ref tmp, ref output, i);
                }
                else
                {
                    if (i == TotalSize-1 && TotalSize % 16 != 0) // Last line but not filled to 16
                    {
                        processLine(encoding, showLineNumbers, showText, ref tmp, ref output, i);
                    }
                    else if ((i + 1) % 8 == 0)
                    {
                        tmp.Append(string.Format("{0:x2}   ", Data[i]));
                    }
                    else
                    {
                        if ((i + 1) % 4 == 0)
                            tmp.Append(string.Format("{0:x2}  ", Data[i]));
                        else
                            tmp.Append(string.Format("{0:x2} ", Data[i]));
                    }
                }
            }

            if (tmp.Length > 0)
            {
                output.Append(tmp.ToString());
                tmp.Clear();
            }

            return output.ToString();
        }

        void processLine(DataEncoding encoding, bool showLineNumbers, bool showText, ref StringBuilder line, ref StringBuilder lines, int index)
        {
            var rowWidth = 16;
            var xIndex = (index+1) - (index / rowWidth) * rowWidth;
            var isLastLine = index == TotalSize - 1 && TotalSize % rowWidth != 0 ? true : false;
            if (showLineNumbers)
            {
                // Line number
                var plus = isLastLine ? 1 : 0;
                line.Insert(0, "   ");
                line.Insert(0, string.Format("{0:X4}", (((index + 1) / rowWidth) + plus)));
            }

            if (showText)
            {
                var stepBackCount = xIndex == 0 ? rowWidth : xIndex;
                var lineOutputLen = rowWidth * 2 + 19; // 16 bytes to output * 2 digits + 19 whitespaces inbetween
                var idx = xIndex == 0 ? rowWidth : xIndex;
                var numMissingBytes = (rowWidth - idx);
                Trace.WriteLine($"Num missing bytes: {numMissingBytes}");
                var numWhiteSpacesNeeded = 0;
                if (numMissingBytes > 0)
                {
                    numWhiteSpacesNeeded = numMissingBytes * 3 + 4; // 2 digits for each byte + for each byte pair comes a whitespace after = numMissingPytes * 3
                    if (xIndex > 4) numWhiteSpacesNeeded -= 1;
                    if (xIndex > 8) numWhiteSpacesNeeded -= 2;
                    if (xIndex > 12) numWhiteSpacesNeeded -= 1;
                }

                var actualSpace = 4;
                numWhiteSpacesNeeded += actualSpace;

                line.Append(string.Format("{0:X2}", Data[index]));
                line.Append($"{new string(' ', numWhiteSpacesNeeded)}");

                var lineBytes = Data[((index + 1) - stepBackCount)..(index + 1)];
                var strEncoded = bytesToString(lineBytes, encoding);

                line.Append($"{strEncoded}\n");

                lines.Append(line.ToString());


                line.Clear();
            }
            else
            {
                line.Append(string.Format("{0:X2}\n", Data[index]));
                lines.Append(line.ToString());
                line.Clear();
            }
        }

        public string bytesToString(byte[] bytes, DataEncoding encoding)
        {
            StringBuilder tmpString = new StringBuilder();
            for (var i = 0; i < bytes.Length; i++)
            {
                char chr;
                switch (encoding)
                {
                    case DataEncoding.UTF8:
                        chr = Convert.ToChar(Encoding.UTF8.GetString(new byte[] { bytes[i] }));
                        break;
                    default:
                        chr = Convert.ToChar(Encoding.ASCII.GetString(new byte[] { bytes[i] }));
                        break;
                }
                if (char.IsControl(chr) || char.IsWhiteSpace(chr))
                    tmpString.Append(".");
                else
                    tmpString.Append(chr);

                if (i == 3 || i == 11)
                    tmpString.Append(" ");

                if (i == 7)
                    tmpString.Append("  ");
            }
            return tmpString.ToString();
        }
    }
}
