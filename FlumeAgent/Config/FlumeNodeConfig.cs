using System.Xml.Serialization;

namespace FlumeAgent.Config
{
    /// <summary>
    /// FlumeNode's config
    /// </summary>
    public class FlumeNodeConfig
    {
        public FlumeNodeConfig()
        {
            Host = "localhost";
            Enabled = true;
            Port = 2014;
            DangerZoneThreshold = 5;
            DangerZoneSeconds = 30;
            EnablePool = true;
            TimeOut = 120;
            PoolSize = 50;
            ConnectionLifetimeMinutes = 30;
        }

        [XmlAttribute("Enabled")]
        public bool Enabled { get; set; }
       
        [XmlAttribute("Host")]
        public string Host { get; set; }

        [XmlAttribute("Port")]
        public int Port { get; set; }

        [XmlAttribute("TimeOut")]
        public int TimeOut { get; set; }

        [XmlAttribute("DangerZoneThreshold")]
        public int DangerZoneThreshold { get; set; }

        [XmlAttribute("DangerZoneSeconds")]
        public int DangerZoneSeconds { get; set; }

        [XmlAttribute("EnablePool")]
        public bool EnablePool { get; set; }

        [XmlAttribute("PoolSize")]
        public int PoolSize { get; set; }

        [XmlAttribute("ConnectionLifetimeMinutes")]
        public int ConnectionLifetimeMinutes { get; set; }


        public override string ToString()
        {
            return Host + ":" + Port;
        }
        public override int GetHashCode()
        {
            return Host.GetHashCode() + Port.GetHashCode();
        }
    }

}