using System;
using System.Collections.Generic;
using System.Text;
using FlumeAgent;

namespace FlumeAgent
{
    public  class FlumeAgent
    {
        public static readonly FlumeAgent Instance=new FlumeAgent();

        public static string AgentHostName;

        public FlumeAgent()
        {
            AgentHostName = Environment.MachineName;
        }

        public void Send(string sourceName,string datastr)
        {
            if (datastr != null)
            {

                FlumeNode node = FlumeNodeManager.Instance.GetNode(sourceName);
                using (var esSession = new FlumeSession(node.ConnectionProvider))
                {
                    var client = esSession.GetClient();
                    var fields = new Dictionary<string, byte[]>();
                    fields["catalog"] = Encoding.Default.GetBytes(sourceName);
                    client.send_append(new ThriftFlumeEvent()
                    {
                        Timestamp = DateTime.Now.ToFileTimeUtc(),
//                        Priority = Priority.INFO,
                        Host = AgentHostName,
                        Body = Encoding.Default.GetBytes(datastr),
                        Fields = fields,
//                        Nanos = DateTime.Now.Ticks,
                    });

                }

            }
        }
        
    }

}