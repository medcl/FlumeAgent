using System.Xml.Serialization;

namespace FlumeAgent.Config
{
    public class Collector
    {
        [XmlArray("ThriftNodes")]
        [XmlArrayItem("Node")]
        public FlumeNodeConfig[] ThriftNodes { get; set; }

        /// <summary>
        /// 日志收集器
        /// </summary>
        [XmlAttribute("Name")]
        public string Name;


    }
}