using System.Xml.Serialization;

namespace FlumeAgent.Config
{
    public class Source
    {
        [XmlAttribute("Name")]
        public string Name;

        [XmlAttribute("Collector")]
        public string Collector;

    }
}