using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Admin.ModelsExtra.Json
{

    public class Rootobject
    {
        public string error { get; set; }
        public Device[] devices { get; set; }
    }

    public class Device
    {
        public string key { get; set; }
        public string name { get; set; }
        public string boxtype { get; set; }
        public string subtype { get; set; }
        public string onlineStatus { get; set; }
        public string healthStatus { get; set; }
        public int healthRank { get; set; }
        public string account { get; set; }
        public string ip { get; set; }
        public int cpeMaxCnt { get; set; }
        public Cmts cmts { get; set; }
        public Flap flap { get; set; }
        public Mta mta { get; set; }
        public string battStatus { get; set; }
        public int cpeCnt { get; set; }
        public string cpeMaxCntStr { get; set; }
        public string dhcpServer { get; set; }
        public string tftpServer { get; set; }
        public string timeServer { get; set; }
        public int tunerCnt { get; set; }
        public string uid { get; set; }
        public string grp { get; set; }
        public string subgrp { get; set; }
        public string healthListAsHtml { get; set; }
        public string type { get; set; }
        public string customerName { get; set; }
        public string customerPhone { get; set; }
        public string sourceIp { get; set; }
        public string sourceName { get; set; }
        public string sourceUpPort { get; set; }
        public Docsishealth docsisHealth { get; set; }
        public string sourcePort { get; set; }
        public string overallStatus { get; set; }
        public string address { get; set; }
        public string health { get; set; }
        public Nc nc { get; set; }
        public Rpd rpd { get; set; }
        public Radd radd { get; set; }
        public Om om { get; set; }
        public Pace pace { get; set; }
        public Dcthealth dctHealth { get; set; }
    }

    public class Cmts
    {
        public string name { get; set; }
        public string ip { get; set; }
        public string type { get; set; }
        public string vendor { get; set; }
        public string uptime { get; set; }
        public string description { get; set; }
        public string pingStatus { get; set; }
    }

    public class Flap
    {
        public string onlist { get; set; }
        public string crcs { get; set; }
        public string hits { get; set; }
        public string misses { get; set; }
        public string total { get; set; }
        public string registeredA { get; set; }
        public string rangeA { get; set; }
        public string status { get; set; }
        public string flapRate { get; set; }
        public string rawFlapNumber { get; set; }
        public string createFlapTime { get; set; }
        public string lastFlapTime { get; set; }
        public string insertionFails { get; set; }
        public string powerAdjustments { get; set; }
        public string provisioning { get; set; }
        public string prestatus { get; set; }
        public string lastFlapTimeArris { get; set; }
        public string insertionFailsArris { get; set; }
        public string provisioningArris { get; set; }
    }

    public class Mta
    {
        public string esafeService { get; set; }
        public string state { get; set; }
        public Battery battery { get; set; }
        public string[] lines { get; set; }
        public bool historyEnabled { get; set; }
        public string[] callHistory { get; set; }
    }

    public class Battery
    {
        public string state { get; set; }
        public string charge { get; set; }
        public string acfail { get; set; }
        public string low { get; set; }
        public string missing { get; set; }
        public string replace { get; set; }
        public string timeOnBattery { get; set; }
        public string batteryTimeAvail { get; set; }
    }

    public class Docsishealth
    {
        public string flap { get; set; }
        public Interface[] interfaces { get; set; }
        public Downstreamsnr downstreamSnr { get; set; }
        public Downstreampower downstreamPower { get; set; }
        public Downstreamcodewords downstreamCodewords { get; set; }
        public float? downstreamFec { get; set; }
        public Downstreammicroreflections downstreamMicroreflections { get; set; }
        public Upstreampower upstreamPower { get; set; }
        public Upstreamsnr upstreamSnr { get; set; }
        public Upstreamcodewords upstreamCodewords { get; set; }
        public float? upstreamFec { get; set; }
    }

    public class Downstreamsnr
    {
        public string name { get; set; }
        public string description { get; set; }
        public float[] values { get; set; }
        public string[] alarms { get; set; }
        public string pollDate { get; set; }
    }

    public class Downstreampower
    {
        public string name { get; set; }
        public string description { get; set; }
        public float[] values { get; set; }
        public string[] alarms { get; set; }
        public string pollDate { get; set; }
    }

    public class Downstreamcodewords
    {
        public string name { get; set; }
        public string description { get; set; }
        public float[] values { get; set; }
        public string[] alarms { get; set; }
        public string pollDate { get; set; }
    }

    public class Downstreammicroreflections
    {
        public string name { get; set; }
        public string description { get; set; }
        public float[] values { get; set; }
        public string[] alarms { get; set; }
        public string pollDate { get; set; }
    }

    public class Upstreampower
    {
        public string name { get; set; }
        public string description { get; set; }
        public float[] values { get; set; }
        public string[] alarms { get; set; }
        public string pollDate { get; set; }
    }

    public class Upstreamsnr
    {
        public string name { get; set; }
        public string description { get; set; }
        public float[] values { get; set; }
        public string[] alarms { get; set; }
        public string pollDate { get; set; }
    }

    public class Upstreamcodewords
    {
        public string name { get; set; }
        public string description { get; set; }
        public float[] values { get; set; }
        public string[] alarms { get; set; }
        public string pollDate { get; set; }
    }

    public class Interface
    {
        public string name { get; set; }
        public string description { get; set; }
        public float[] values { get; set; }
        public string[] alarms { get; set; }
        public string pollDate { get; set; }
    }

    public class Nc
    {
        public string name { get; set; }
        public string ip { get; set; }
        public string status { get; set; }
        public string hardwareVersion { get; set; }
        public string softwareVersion { get; set; }
        public string statusColor { get; set; }
    }

    public class Rpd
    {
        public string name { get; set; }
        public string ip { get; set; }
        public string status { get; set; }
        public string hardwareVersion { get; set; }
        public string softwareVersion { get; set; }
        public int port { get; set; }
        public string dmodFrequency { get; set; }
        public string dmodNoiseAvg { get; set; }
        public string dmodNoiseMax { get; set; }
        public string dmodNoiseMin { get; set; }
        public string dmodPktsCorrected { get; set; }
        public string dmodPktsErrored { get; set; }
        public string dmodPktsPerfect { get; set; }
        public int dmodPktsTotal { get; set; }
        public string dmodPowerLast { get; set; }
        public string dmodPowerMax { get; set; }
        public string dmodPowerMin { get; set; }
        public string dmodStatusAlarm { get; set; }
        public string dmodPktsCorrectedRatio { get; set; }
        public string dmodPktsErroredRatio { get; set; }
        public string dmodPktsPerfectRatio { get; set; }
        public string statusColor { get; set; }
    }

    public class Radd
    {
        public string name { get; set; }
        public string ip { get; set; }
        public string status { get; set; }
        public string hardwareVersion { get; set; }
        public string softwareVersion { get; set; }
        public string statusColor { get; set; }
    }

    public class Om
    {
        public string name { get; set; }
        public string ip { get; set; }
        public string status { get; set; }
        public string hardwareVersion { get; set; }
        public string softwareVersion { get; set; }
        public string statusColor { get; set; }
    }

    public class Pace
    {
        public string[] pages { get; set; }
    }

    public class Dcthealth
    {
        public string retries { get; set; }
        public string entries { get; set; }
        public string returnPower { get; set; }
    }

}