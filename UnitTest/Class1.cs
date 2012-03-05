using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FlumeAgent.Config;
using NUnit.Framework;

namespace UnitTest
{
    [TestFixture]
    public class Class1
    {
        [Test]
        public void Test123()
        {
            //init config
            FlumeConfig.Instance.AddCollector(new Collector(){Name = "collector1", ThriftNodes = new FlumeNodeConfig[]{new FlumeNodeConfig(){Host = "10.129.8.125",Port = 2014,Enabled = true}, }});
            FlumeConfig.Instance.AddCollector(new Collector(){Name = "collector2", ThriftNodes = new FlumeNodeConfig[]{new FlumeNodeConfig(){Host = "10.129.8.125",Port = 2015,Enabled = true}, }});
            FlumeConfig.Instance.AddSources(new Source(){Name = "source1",Collector = "collector1"});
            FlumeConfig.Instance.AddSources(new Source(){Name = "source2",Collector = "collector2"});
          
            //collecting
            FlumeAgent. FlumeAgent agent=new FlumeAgent.FlumeAgent();
            agent.Send("source1", "hello,world,datata,blala.....");
            agent.Send("source2", "test from agent.....");
        }
    }
}
