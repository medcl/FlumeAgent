using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Threading;
using FlumeAgent.Config;
using Thrift.Transport;

namespace FlumeAgent
{
	internal class ConnectionPool : IConnectionProvider
	{
		#region fields

//		private static readonly LogWrapper logger = new LogWrapper();

		private readonly Semaphore connectionLimiter;

		private readonly  Queue<IConnection> connections = new Queue<IConnection>();
		private readonly object padlock = new object();
		private readonly Server target;
		private readonly bool useLimiter;
	    private int ConnectionLifetimeMinutes ;

		#endregion

		#region ctor

        public ConnectionPool(string host, int port, int poolSize, int connectionLifetimeMinutes)
		{

			target = new Server(host, port);
            ConnectionLifetimeMinutes = connectionLifetimeMinutes;
            if (poolSize > 0)
			{
				useLimiter = true;
                connectionLimiter = new Semaphore(poolSize, poolSize);
			}
		}

		#endregion

		#region IConnectionProvider Members

		public IConnection Open()
		{
			if (EnterLimiter())
			{
				try
				{
					lock (padlock)
					{
						while (connections.Count > 0)
						{
							IConnection connection = connections.Dequeue();
							if (!IsAlive(connection))
							{
								ReleaseConnection(connection);
								continue;
							}
							return connection;
						}

						return NewConnection();
					}
				}
				catch
				{
					ExitLimiter();
					throw;
				}
			}

			throw new BrokenCollectorException("Too many connections to " + target);
		}

		public bool Close(IConnection connection)
		{
			lock (padlock)
			{
				if (IsAlive(connection))
					connections.Enqueue(connection);
				else
					ReleaseConnection(connection);

				ExitLimiter();
			}

			return true;
		}

		public IConnection CreateConnection()
		{
			throw new NotImplementedException();
		}

		#endregion

		private bool IsAlive(IConnection connection)
		{
			if (ConnectionLifetimeMinutes > 0
			    && connection.Created.AddMinutes(ConnectionLifetimeMinutes) < DateTime.Now)
				return false;

			return connection.IsOpen;
		}

		private bool EnterLimiter()
		{
			if (useLimiter)
			{
                if (connectionLimiter.WaitOne(FlumeConfig.Instance.ConnectTimeout, false))
					return true;
				return false;
			}
			return true;
		}

		private void ExitLimiter()
		{
			if (useLimiter)
			{
				try
				{
					connectionLimiter.Release();
				}
				catch (SemaphoreFullException)
				{
//					logger.ErrorFormat("Connection pool for {0} released a connection too many times", target);
				}
			}
		}

		private void ReleaseConnection(IConnection connection)
		{
			connection.Dispose();
		}

		public void ReleaseAll()
		{
			lock (padlock)
			{
				foreach (IConnection connection in connections)
				{
					try
					{
						connection.Dispose();
					}
					catch (SocketException)
					{
					}
					catch (ObjectDisposedException)
					{
					}
				}
			}
		}

		private IConnection NewConnection()
		{
			var connection = new Connection(target);
//			logger.InfoFormat("Server Opening ,{0}",target);
			connection.Open();
			return connection;
		}
	}
}