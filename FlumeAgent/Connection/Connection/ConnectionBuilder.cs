using System;
using System.Collections.Generic;
using System.Text;
using Thrift.Transport;

namespace FlumeAgent
{
	internal class ConnectionBuilder
	{
	    /// <summary>
	    /// 
	    /// </summary>
	    /// <param name="host"></param>
	    /// <param name="port"></param>
	    /// <param name="timeout">Milliseconds</param>
	    public ConnectionBuilder(string host, int port = 9200, 
		                         int timeout = 0, bool pooled = false, int poolSize = 25, int lifetime = 0)
		{
			Servers = new List<Server> {new Server(host, port)};
			Timeout = timeout;
			Pooled = pooled;
			PoolSize = poolSize;
			Lifetime = lifetime;
		}

		/// <summary>
		/// 
		/// </summary>
		public int Timeout { get; private set; }

		/// <summary>
		/// 
		/// </summary>
		public int PoolSize { get; private set; }

		/// <summary>
		/// 
		/// </summary>
		public int Lifetime { get; private set; }

		/// <summary>
		/// 
		/// </summary>
		public bool Pooled { get; private set; }
		
        public IList<Server> Servers { get; private set; }

	}
}