﻿using PacketDotNet;
using PacketDotNet.Ieee80211;
using SharpPcap;
using SharpPcap.LibPcap;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Redsniff
{
    internal class MainController
    {
        public void OnLoad()
        {

            var captureDevices = CaptureDeviceList.Instance;
            foreach (LibPcapLiveDevice device in captureDevices)
            {
                foreach (var address in device.Addresses)
                {
                    if (address.Addr.ipAddress.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
                    {
                        // Found the IPv4 address
                        IPAddress ipAddress = address.Addr.ipAddress;
                        Program.MainState.MainForm.ComboBox_Devices.Items.Add(ipAddress.ToString());
                        break;
                    }
                }
            }

            Program.MainState.MainForm.DataGridView_CapturedPackets.Columns.Add("Id", "Id");
            Program.MainState.MainForm.DataGridView_CapturedPackets.Columns.Add("Protocol", "Protocol");
            Program.MainState.MainForm.DataGridView_CapturedPackets.Columns.Add("SrcIp", "Source Ip");
            Program.MainState.MainForm.DataGridView_CapturedPackets.Columns.Add("DstIp", "Dest. Ip");
            Program.MainState.MainForm.DataGridView_CapturedPackets.Columns.Add("SrcPort", "Source Port");
            Program.MainState.MainForm.DataGridView_CapturedPackets.Columns.Add("DstPort", "Dest. Port");
            Program.MainState.MainForm.DataGridView_CapturedPackets.Columns.Add("MsgSize", "Message Size");
            Program.MainState.MainForm.DataGridView_CapturedPackets.Columns.Add("TotalSize", "Total Size");
            Program.MainState.MainForm.DataGridView_CapturedPackets.Columns.Add("Captured", "Captured");

            Program.MainState.MainForm.ComboBox_Protocols.SelectedIndex = 0;

            Program.MainState.MainForm.Button_ToggleEmptyPackets.Text = Program.MainState.ListEmptyPackets ? "Empty packets: Yes" : "Empty packets: No";

            updateInputStates();

        }

        public void StartCapturing()
        {
            if (Program.MainState.IsCapturing) return;

            if (!filtersValid()) return;

            var ip = Program.MainState.MainForm.ComboBox_Devices.SelectedItem as string;
            if (string.IsNullOrWhiteSpace(ip))
            {
                MessageBox.Show("Select a capture device first.", "No Capture Device", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            var device = getCaptureDeviceByIp(ip);
            if (device == null)
            {
                MessageBox.Show("No Capture Device found.", "No Capture Device", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            Program.MainState.IsCapturing = true;
            updateInputStates();
            Program.MainState.MainForm.DataGridView_CapturedPackets.Rows.Clear();

            Program.MainState.CurrentCaptureDevice = device;
            Program.MainState.CapturedPackets.Clear();
            Program.MainState.CapturedPacketsFiltered.Clear();


            device.OnPacketArrival += onPacketArrival;
            device.Open();
            device.Filter = Program.MainState.SelectedProtocol;
            device.StartCapture();
        }

        public void StopCapturing()
        {
            var device = Program.MainState.CurrentCaptureDevice;
            if (device == null) return;
            if (!Program.MainState.IsCapturing) return;
            Program.MainState.IsCapturing = false;

            device.StopCapture();
            device.Close();
            updateInputStates();
        }

        public void SaveCapture()
        {
            var sb = new StringBuilder();

            var emptyPacketsCount = 0;
            var packetsWrittenCount = 0;
            foreach (var packet in Program.MainState.CapturedPacketsFiltered)
            {
                if (packet != null && packet.MsgSize > 0)
                {
                    sb.AppendLine(packetToHexString(packet));
                    packetsWrittenCount++;
                }
                else
                {
                    emptyPacketsCount++;
                }
            }

            var finalOutput = new StringBuilder();
            finalOutput.Append($"##################################################################################\n");
            finalOutput.Append("Capture file generated by Redsniff <https://github.com/RednibCoding/RedSniff>\n");
            finalOutput.Append($"Date: {DateTime.Now}\n");
            finalOutput.Append($"Total packets written: {packetsWrittenCount}\n");
            finalOutput.Append($"Empty packets omitted: {emptyPacketsCount}\n");
            finalOutput.Append($"##################################################################################\n\n");
            finalOutput.Append(sb.ToString() + "\n");

            if (sb.Length > 0)
            {
                SaveFileDialog dialog = new SaveFileDialog()
                {
                    Filter = "Text Files(*.txt)|*.txt"
                };

                var result = dialog.ShowDialog();

                if (result == DialogResult.OK)
                {
                    File.WriteAllText(dialog.FileName, finalOutput.ToString());
                }
            }
        }

        public void ApplyFilter()
        {
            if (Program.MainState.IsCapturing) return;
            if (!filtersValid()) return;

            Program.MainState.CapturedPacketsFiltered.Clear();
            Program.MainState.MainForm.DataGridView_CapturedPackets.Rows.Clear();

            foreach(var packetEntry in Program.MainState.CapturedPackets)
            {
                if ((packetEntry.Data == null || packetEntry.Data.Length == 0) && !Program.MainState.ListEmptyPackets)
                    continue;

                if (doesPacketMatchFilter(packetEntry.SrcIp, packetEntry.DstIp, packetEntry.SrcPort, packetEntry.DstPort))
                {
                    Program.MainState.CapturedPacketsFiltered.Add(packetEntry);
                    addPacketEntryToDataGrid(packetEntry);
                }
            }

            Program.MainState.MainForm.Text = $"{Program.MainState.AppName}  |  Packets: {Program.MainState.CapturedPacketsFiltered.Count} / {Program.MainState.CapturedPackets.Count}";
        }

        internal void ClearFilter()
        {
            if (Program.MainState.IsCapturing) return;
            if (!filtersValid()) return;

            Program.MainState.CapturedPacketsFiltered.Clear();

            foreach (var packetEntry in Program.MainState.CapturedPackets)
            {
                if ((packetEntry.Data == null || packetEntry.Data.Length == 0) && !Program.MainState.ListEmptyPackets)
                    continue;

                Program.MainState.CapturedPacketsFiltered.Add(packetEntry);
                addPacketEntryToDataGrid(packetEntry);
            }

            Program.MainState.MainForm.Text = $"{Program.MainState.AppName}  |  Packets: {Program.MainState.CapturedPacketsFiltered.Count} / {Program.MainState.CapturedPackets.Count}";
        }

        internal void OnRowDoubleClick()
        {
            if (Program.MainState.MainForm.DataGridView_CapturedPackets.SelectedRows.Count <= 0) return;
            var selectedId = Program.MainState.MainForm.DataGridView_CapturedPackets.SelectedRows[0].Cells[0];

            var selectedPacket = Program.MainState.CapturedPackets.Where(p => p.Id == (int)selectedId.Value).FirstOrDefault();

            if (selectedPacket == null) return;

            Program.MainState.MainForm.TextBox_PacketData.Text = packetToHexString(selectedPacket);
        }

        void onPacketArrival(object sender, PacketCapture e)
        {
            // Handle the captured packet
            var packet = Packet.ParsePacket(e.GetPacket().LinkLayerType, e.GetPacket().Data);
            if (packet == null) return;

            IPPacket ipPacket = packet.Extract<IPPacket>();

            if (ipPacket != null && ipPacket.Protocol == ProtocolType.Tcp)
            {
                TcpPacket tcpPacket = ipPacket.Extract<TcpPacket>();
                if (tcpPacket != null)
                {
                    var packetEntry = new PacketEntry();
                    packetEntry.Id = Program.MainState.CapturedPackets.Count + 1;
                    packetEntry.Protocol = "tcp";
                    packetEntry.SrcIp = ipPacket.SourceAddress.ToString();
                    packetEntry.DstIp = ipPacket.DestinationAddress.ToString();
                    packetEntry.SrcPort = tcpPacket.SourcePort;
                    packetEntry.DstPort = tcpPacket.DestinationPort;
                    packetEntry.MsgSize = tcpPacket.PayloadData.Length;
                    packetEntry.TotalSize = ipPacket.TotalLength;
                    packetEntry.Captured = e.GetPacket().Timeval.ToString();
                    packetEntry.Data = tcpPacket.PayloadData;

                    Program.MainState.CapturedPackets.Add(packetEntry);

                    if ((packetEntry.Data == null || packetEntry.Data.Length == 0) && !Program.MainState.ListEmptyPackets)
                        return;

                    if (doesPacketMatchFilter(packetEntry.SrcIp, packetEntry.DstIp, packetEntry.SrcPort, packetEntry.DstPort))
                    {
                        Program.MainState.CapturedPacketsFiltered.Add(packetEntry);

                        // Update the DataGridView control from the main thread using the Invoke method
                        Program.MainState.MainForm.DataGridView_CapturedPackets.Invoke((MethodInvoker)delegate {
                            addPacketEntryToDataGrid(packetEntry);
                        });
                    }
                }
            }
            else if (ipPacket != null && ipPacket.Protocol == ProtocolType.Udp)
            {
                UdpPacket udpPacket = ipPacket.Extract<UdpPacket>();
                if (udpPacket != null)
                {
                    var packetEntry = new PacketEntry();

                    packetEntry.Id = Program.MainState.CapturedPackets.Count + 1;
                    packetEntry.Protocol = "udp";
                    packetEntry.SrcIp = ipPacket.SourceAddress.ToString();
                    packetEntry.DstIp = ipPacket.DestinationAddress.ToString();
                    packetEntry.SrcPort = udpPacket.SourcePort;
                    packetEntry.DstPort = udpPacket.DestinationPort;
                    packetEntry.MsgSize = udpPacket.PayloadData.Length;
                    packetEntry.TotalSize = ipPacket.TotalLength;
                    packetEntry.Captured = e.GetPacket().Timeval.ToString();
                    packetEntry.Data = udpPacket.PayloadData;

                    Program.MainState.CapturedPackets.Add(packetEntry);

                    if (doesPacketMatchFilter(packetEntry.SrcIp, packetEntry.DstIp, packetEntry.SrcPort, packetEntry.DstPort))
                    {
                        Program.MainState.CapturedPacketsFiltered.Add(packetEntry);

                        // Update the DataGridView control from the main thread using the Invoke method
                        Program.MainState.MainForm.DataGridView_CapturedPackets.Invoke((MethodInvoker)delegate {
                            addPacketEntryToDataGrid(packetEntry);
                        });
                    }
                }
            }

            Program.MainState.MainForm.Invoke((MethodInvoker)delegate {
                Program.MainState.MainForm.Text = $"{Program.MainState.AppName}  |  Packets: {Program.MainState.CapturedPacketsFiltered.Count} / {Program.MainState.CapturedPackets.Count}";
            });
            
        }

        bool doesPacketMatchFilter(string filterSourceIp, string filterDestIp, int filterSourcePort, int filterDestPort)
        {
            if (string.IsNullOrEmpty(Program.MainState.SrcIpFilter) &&
                        string.IsNullOrEmpty(Program.MainState.DstIpFilter) &&
                        Program.MainState.SrcPortFilter == -1 &&
                        Program.MainState.DstPortFilter == -1)
            {
                return true;
            }
            else
            {
                bool shouldAddPacket = false;

                if (!string.IsNullOrWhiteSpace(Program.MainState.SrcIpFilter) && Program.MainState.SrcIpFilter == filterSourceIp)
                {
                    shouldAddPacket = true;
                }
                else if (!string.IsNullOrWhiteSpace(Program.MainState.DstIpFilter) && Program.MainState.DstIpFilter == filterDestIp)
                {
                    shouldAddPacket = true;
                }
                else if (Program.MainState.SrcPortFilter != -1 && Program.MainState.SrcPortFilter == filterSourcePort)
                {
                    shouldAddPacket = true;
                }
                else if (Program.MainState.DstPortFilter != -1 && Program.MainState.DstPortFilter == filterDestPort)
                {
                    shouldAddPacket = true;
                }

                if (shouldAddPacket)
                {
                    return true;
                }
                return false;
            }
        }

        void addPacketEntryToDataGrid(PacketEntry packetEntry)
        {
            // Program.MainState.MainForm.DataGridView_CapturedPackets
            var row = new DataGridViewRow();
            row.CreateCells(Program.MainState.MainForm.DataGridView_CapturedPackets);
            row.Cells[0].Value = packetEntry.Id;
            row.Cells[1].Value = packetEntry.Protocol;
            row.Cells[2].Value = packetEntry.SrcIp;
            row.Cells[3].Value = packetEntry.DstIp;
            row.Cells[4].Value = packetEntry.SrcPort;
            row.Cells[5].Value = packetEntry.DstPort;
            row.Cells[6].Value = packetEntry.MsgSize;
            row.Cells[7].Value = packetEntry.TotalSize;
            row.Cells[8].Value = packetEntry.Captured;
            Program.MainState.MainForm.DataGridView_CapturedPackets.Rows.Add(row);
        }

        bool filtersValid()
        {
            var srcPortStr = Program.MainState.MainForm.TextBox_SrcPort.Text;
            var dstPortStr = Program.MainState.MainForm.TextBox_DstPort.Text;
            var srcIpStr = Program.MainState.MainForm.TextBox_SrcIp.Text;
            var dstIpStr = Program.MainState.MainForm.TextBox_DstIp.Text;

            if (string.IsNullOrWhiteSpace(srcPortStr))
            {
                Program.MainState.SrcPortFilter = -1;
            }
            else
            {
                var success = int.TryParse(srcPortStr, out var srcPortInt);
                if (!success)
                {
                    MessageBox.Show($"{srcPortStr} is not a valid port", "Invalid Port", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return false;
                }
                Program.MainState.SrcPortFilter = srcPortInt;
            }

            if (string.IsNullOrWhiteSpace(dstPortStr))
            {
                Program.MainState.DstPortFilter = -1;
            }
            else
            {
                var success = int.TryParse(dstPortStr, out var dstPortInt);
                if (!success)
                {
                    MessageBox.Show($"{dstPortStr} is not a valid port", "Invalid Port", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return false;
                }
                Program.MainState.DstPortFilter = dstPortInt;
            }

            if (!string.IsNullOrWhiteSpace(srcIpStr))
            {
                if (!isValidIpString(srcIpStr))
                {
                    MessageBox.Show($"{srcIpStr} is not a valid ip address", "Invalid Ip Address", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return false;
                }
                Program.MainState.SrcIpFilter = srcIpStr;
            }
            else
            {
                Program.MainState.SrcIpFilter = string.Empty;
            }

            if (!string.IsNullOrWhiteSpace(dstIpStr))
            {
                if (!isValidIpString(dstIpStr))
                {
                    MessageBox.Show($"{dstIpStr} is not a valid ip address", "Invalid Ip Address", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return false;
                }
                Program.MainState.DstIpFilter = dstIpStr;
            }
            else
            {
                Program.MainState.DstIpFilter = string.Empty;
            }

            return true;
        }

        bool isValidIpString(string ipString)
        {
            string[] ipAddressParts = ipString.Split('.');
            if (ipAddressParts.Length != 4) return false;
            if (IPAddress.TryParse(ipString, out var ipAddress))
            {
                return ipAddress.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork;
            }
            return false;
        }

        LibPcapLiveDevice? getCaptureDeviceByIp(string ip)
        {
            var captureDevices = CaptureDeviceList.Instance;
            foreach (LibPcapLiveDevice device in captureDevices)
            {
                foreach (var address in device.Addresses)
                {
                    if (address.Addr.ipAddress == null) continue;
                    if (address.Addr.ipAddress.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
                    {
                        // Found the IPv4 address
                        IPAddress ipAddress = address.Addr.ipAddress;
                        if (ipAddress.ToString() == ip)
                            return device;
                    }
                }
            }
            return null;
        }

        void updateInputStates()
        {
            if (Program.MainState.IsCapturing)
            {
                Program.MainState.MainForm.Button_Start.Enabled = false;
                Program.MainState.MainForm.Button_Stop.Enabled = true;
                Program.MainState.MainForm.Button_Save.Enabled = false;
                Program.MainState.MainForm.Button_Apply.Enabled = false;
                Program.MainState.MainForm.Button_Reset.Enabled = false;

                Program.MainState.MainForm.ComboBox_Devices.Enabled = false;
                Program.MainState.MainForm.ComboBox_Protocols.Enabled = false;
                Program.MainState.MainForm.TextBox_SrcPort.Enabled = false;
                Program.MainState.MainForm.TextBox_DstPort.Enabled = false;
                Program.MainState.MainForm.TextBox_SrcIp.Enabled = false;
                Program.MainState.MainForm.TextBox_DstIp.Enabled = false;

                Program.MainState.MainForm.Button_ToggleEmptyPackets.Enabled = false;
                Program.MainState.MainForm.Button_Tools.Enabled = false;
            }
            else
            {
                Program.MainState.MainForm.Button_Start.Enabled = true;
                Program.MainState.MainForm.Button_Stop.Enabled = false;
                Program.MainState.MainForm.Button_Apply.Enabled = true;
                Program.MainState.MainForm.Button_Reset.Enabled = true;

                if (Program.MainState.CapturedPacketsFiltered.Count > 0)
                {
                    Program.MainState.MainForm.Button_Save.Enabled = true;
                }
                else
                {
                    Program.MainState.MainForm.Button_Save.Enabled = false;
                }

                Program.MainState.MainForm.ComboBox_Devices.Enabled = true;
                Program.MainState.MainForm.ComboBox_Protocols.Enabled = true;
                Program.MainState.MainForm.TextBox_SrcPort.Enabled = true;
                Program.MainState.MainForm.TextBox_DstPort.Enabled = true;
                Program.MainState.MainForm.TextBox_SrcIp.Enabled = true;
                Program.MainState.MainForm.TextBox_DstIp.Enabled = true;

                Program.MainState.MainForm.Button_ToggleEmptyPackets.Enabled = true;
                Program.MainState.MainForm.Button_Tools.Enabled = true;
            }
        }

        string packetToHexString(PacketEntry packet)
        {
            var hexDump = new StringBuilder();

            hexDump.AppendLine($"Id:            {packet.Id}");
            hexDump.AppendLine($"Protocol:      {packet.Protocol}");
            hexDump.AppendLine($"From:          {packet.SrcIp}:{packet.SrcPort}");
            hexDump.AppendLine($"To:            {packet.DstIp}:{packet.DstPort}");
            hexDump.AppendLine($"Captured       {packet.Captured}");
            hexDump.AppendLine($"Message Size:  {packet.MsgSize} bytes");
            hexDump.AppendLine($"Total Size:    {packet.TotalSize} bytes");
            hexDump.AppendLine($"");

            hexDump.AppendLine("|------------------------------------------------|----------------|");
            hexDump.AppendLine("|00 01 02 03 04 05 06 07 08 09 0A 0B 0C 0D 0E 0F |0123456789ABCDEF|");
            hexDump.AppendLine("|------------------------------------------------|----------------|");
            try
            {
                int end = packet.Data.Length;
                for (int i = 0; i < end; i += 16)
                {
                    StringBuilder text = new StringBuilder();
                    StringBuilder hex = new StringBuilder();
                    for (int j = 0; j < 16; j++)
                    {
                        if (j + i < end)
                        {
                            byte val = packet.Data[j + i];

                            hex.Append(packet.Data[j + i].ToString("X2"));

                            hex.Append(" ");
                            if (val >= 32 && val < 127)
                            {

                                text.Append((char)val);
                            }
                            else
                            {
                                text.Append(".");
                            }
                        }
                    }
                    hexDump.AppendLine("|" + hex.ToString().PadRight(48) + "|" + text.ToString().PadRight(16) + "|");
                }
            }
            catch (Exception)
            {
                // Log.Error("HexDump", e.ToString());
            }

            hexDump.Append("-------------------------------------------------------------------\n\n");
            return hexDump.ToString();
        }
    }
}
