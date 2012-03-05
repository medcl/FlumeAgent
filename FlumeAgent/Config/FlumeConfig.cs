using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace FlumeAgent.Config
{
    //load config dynamic yourself , :)
    public class FlumeConfig
    {
        public int ConnectTimeout { get; set; }

        [XmlArray("Collectors")]
        [XmlArrayItem("Collector")]
        public List<Collector> Collectors;

        [XmlArray("Sources")]
        [XmlArrayItem("Source")]
        public List<Source> Sources;

        public FlumeConfig()
        {
            ConnectTimeout = 3000;
        }

        public static readonly FlumeConfig Instance=new FlumeConfig();
        public static event EventHandler<EventArgs> ConfigChanged;

        public FlumeConfig AddCollector(Collector collector)
        {
            if(Collectors==null)Collectors=new List<Collector>();
            Collectors.Add(collector);
            OnConfigChanged(null);
            return this;
        }

        public FlumeConfig AddSources(Source source)
        {
            if (Sources == null) Sources = new List<Source>();
            Sources.Add(source);
            OnConfigChanged(null);
            return this;
        }
        
        public static void OnConfigChanged(EventArgs e)
        {
            EventHandler<EventArgs> handler = ConfigChanged;
            if (handler != null) handler(null, e);
        }
    }
}