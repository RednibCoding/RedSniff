using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace RedSniff.RiffLang
{
    public static class FilterParser
    {
        /* 
         * Syntax BNF
         * 
         * FilterProgram:       ( Statement )+
         * Statement:           ( SRC | DEST| BOTH ) ( IpStatement | PortStatement )
         * IpStatement:         IP ip ( ip )*
         * PortStatement:       PORT port ( port )*
         *           
        */

        /* Keywords:
         * 
            "src",
            "dst",
            "both",
            "ip",
            "port",
        */

        public static FilterProgram? ParseInput(string input)
        {

            var statements = input.Split("\n");

            if (statements.Length <= 0)
                return null;

            FilterProgram program = new();
            string[] words;

            var line = 0;
            foreach (var statementStr in statements)
            {
                line++; // Just for error reporting

                if (string.IsNullOrWhiteSpace(statementStr)) continue;
                if (statementStr.Trim().StartsWith("#")) continue;

                words = statementStr.Split(" ", StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries);

                // A statement has at least 3 instructions: e.g   src  ip  args...
                if (words.Length < 2) //                           1   2    3...
                {
                    MessageBox.Show($"Error in line {line}: invalid filter statement '{statementStr}'", "RedSniff", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return null;
                }

                AffectionType affType;
                var success = Enum.TryParse(words[0], out affType);
                if (!success)
                {
                    MessageBox.Show($"Error in line {line}: unexpected affection type, expected 'both', 'src' or 'dst', got '{words[0]}'", "RedSniff", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return null;
                };

                Statement? statement = null;

                if (words[1] == "ip")
                {
                    statement = new IpStatement();
                    var ipStatement = parseIpStatementArgs(statement);
                    if (ipStatement == null) return null;
                    ipStatement.AffectionType = affType;
                    program.Statements.Add(ipStatement);
                }
                else if (words[1] == "port")
                {
                    statement = new PortStatement();
                    var portStatement = parsePortStatementArgs(statement);
                    if (portStatement == null) return null;
                    portStatement.AffectionType = affType;
                    program.Statements.Add(portStatement);
                }
                else
                {
                    MessageBox.Show($"Error in line {line}: unexpected statement, expected 'ip', 'iprange' 'port' or 'portrange', got '{words[1]}'", "RedSniff", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return null;
                }
            }

            if (program == null) return null;
            return program;

            IpStatement? parseIpStatementArgs(Statement statement)
            {
                var ipStatement = (IpStatement)statement;
                for (var i = 2; i < words.Length; i++)
                {
                    IPAddress? iPAddress;
                    var success = IPAddress.TryParse(words[i], out iPAddress);
                    if (!success)
                    {
                        MessageBox.Show($"Error in line {line}: unexpected ip address, expected ip format '0.0.0.0', got  '{words[i]}'", "RedSniff", MessageBoxButton.OK, MessageBoxImage.Warning);
                        return null;
                    }
                    ipStatement.IpAdresses = new();
                    ipStatement.IpAdresses.Add(iPAddress!);
                }
                return ipStatement;
            }

            PortStatement? parsePortStatementArgs(Statement statement)
            {
                var portStatement = (PortStatement)statement;
                for (var i = 2; i < words.Length; i++)
                {
                    uint port;
                    var success = uint.TryParse(words[i], out port);
                    if (!success)
                    {
                        MessageBox.Show($"Error in line {line}: unexpected port '{words[i]}'", "RedSniff", MessageBoxButton.OK, MessageBoxImage.Warning);
                        return null;
                    }

                    portStatement.Ports = new();
                    portStatement.Ports.Add(port);
                }
                return portStatement;
            }
        }
    }
}

