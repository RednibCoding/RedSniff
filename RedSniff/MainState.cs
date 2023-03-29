﻿using SharpPcap.LibPcap;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Redsniff
{
    internal class MainState
    {
        public MainForm MainForm { get; set; } = new();
        public string AppName { get; } = "Redsniff";
        public List<PacketEntry> CapturedPackets { get; set; } = new();
        public List<PacketEntry> CapturedPacketsFiltered { get; set; } = new();
        public LibPcapLiveDevice? CurrentCaptureDevice { get; set; }
        public bool IsCapturing { get; set; } = false;
        public int SrcPortFilter { get; set; } = -1;
        public int DstPortFilter { get; set; } = -1;
        public string SrcIpFilter { get; set; } = string.Empty;
        public string DstIpFilter { get; set; } = string.Empty;
        public string SelectedProtocol { get; set; } = string.Empty;
    }
}
