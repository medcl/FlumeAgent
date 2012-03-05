using System;
using System.Collections.Generic;
using System.Linq;
using FlumeAgent.Config;

namespace FlumeAgent
{
	internal class FlumeNodeManager
	{
		public static readonly FlumeNodeManager Instance = new FlumeNodeManager();
		private readonly Random rand;
        private Dictionary<string, List<FlumeNode>> nodes;
	    private Dictionary<string, string> sourceCollectorPair;
	    private FlumeConfig config;

        private FlumeNodeManager()
		{
			nodes = BuildNodes(FlumeConfig.Instance.Collectors);
            sourceCollectorPair = BuildSourceCollectorPair(FlumeConfig.Instance.Sources);
			rand = new Random((int) DateTime.Now.Ticks);
			FlumeConfig.ConfigChanged += FlumeConfig_Changed;
            config = FlumeConfig.Instance;
		}

	    private Dictionary<string, string> BuildSourceCollectorPair(List<Source> sources)
	    {
	        var result = new Dictionary<string, string>();
	        foreach (var source in sources)
	        {
	            result[source.Name.ToLower().Trim()] = source.Collector.ToLower().Trim();
	        }
	        return result;
	    }

	    private void FlumeConfig_Changed(object sender, EventArgs e)
		{
			var elasticSearchConfig = sender as FlumeConfig;
			if (elasticSearchConfig != null)
			{
//				logger.Info("Flume config reloading");
				config = elasticSearchConfig;
                sourceCollectorPair = BuildSourceCollectorPair(FlumeConfig.Instance.Sources);
				nodes = BuildNodes(config.Collectors);
//				logger.Info("Flume config reloaded");
			}
			else
			{
//				logger.Error("Attempt to reload with null flume config");
			}
		}


        private static Dictionary<string, List<FlumeNode>> BuildNodes(List<Collector> definitions)
        {
            var result =new Dictionary<string, List<FlumeNode>>();
			if (definitions != null && definitions.Count > 0)
			{
                foreach (var  catalogDefinition in definitions)
                {
                    if (catalogDefinition.ThriftNodes != null && catalogDefinition.ThriftNodes.Length>0)
                    {
                        foreach (var flumeNodeConfig in catalogDefinition.ThriftNodes)
                        {
                            string key = catalogDefinition.Name.Trim().ToLower();
                            if (result.ContainsKey(key))
                              {
                                  result[key].Add(new FlumeNode(flumeNodeConfig));
                                }else
                              {
                                  result[key] = new List<FlumeNode>();
                                  result[key].Add(new FlumeNode(flumeNodeConfig));
                                }
                        }
                    }
                }
			}
			return result;
		}

        public FlumeNode GetNode(string catalog)
        {
            //1.get collector name
            //2.get node

            catalog = catalog.Trim().ToLower();

            if (!sourceCollectorPair.ContainsKey(catalog))
            {
                throw new SourceNotDefinedException(catalog + " not exists in flume config");
            }

            var collectorName = sourceCollectorPair[catalog];

            if (!nodes.ContainsKey(collectorName))
            {
                throw new CollectorNotDefinedException(collectorName + " not exists in flume config");
            }

            var candidates = new List<FlumeNode>(from node in nodes[collectorName]
			                                  where node.Enabled && !node.InDangerZone
			                                  select node);
			if (candidates.Count > 0)
			{
				return candidates[rand.Next(candidates.Count)];
			}
			throw new BrokenCollectorException("no live node available");
		}


		internal void AggregateCounterTicker()
		{
            foreach (var node in nodes.Values)
                foreach (var flumeNode in node)
                {
                    flumeNode.AggregateCounterTicker();
                }
				
		}
	}
}