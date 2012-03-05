using System;
using FlumeAgent;

namespace FlumeAgent
{
	/// <summary>
	/// ElasticSearch session
	/// </summary>
	internal class FlumeSession : IDisposable
	{
		[ThreadStatic] private static FlumeSession _current;

		private IConnection _connection;
		private bool _disposed;

		public FlumeSession(IConnectionProvider connectionProvider)
		{
			if (Current != null)
                throw new BrokenCollectorException("Cannot create a new session while there is one already active.");
			ConnectionProvider = connectionProvider;
			Current = this;
		}

		public Server CurrentServer
		{
			get { return _connection.Server; }
		}

		public static FlumeSession Current
		{
			get { return _current; }
			internal set { _current = value; }
		}

		/// <summary>
		/// Gets ConnectionProvider.
		/// </summary>
		internal IConnectionProvider ConnectionProvider { get; private set; }

		#region IDisposable Members

		/// <summary>
		/// 
		/// </summary>
		public void Dispose()
		{
			Dispose(true);
		}

		#endregion

		public ThriftFlumeEventServer.Client GetClient()
		{
			if (_connection == null || !_connection.IsOpen)
				_connection = ConnectionProvider.Open();

			return _connection.Client;
		}

		/// <summary>
		/// The dispose.
		/// </summary>
		/// <param name="disposing">
		/// The disposing.
		/// </param>
		protected virtual void Dispose(bool disposing)
		{
			if (!_disposed && disposing)
			{
				if (_connection != null)
					ConnectionProvider.Close(_connection);

				if (Current == this)
					Current = null;
			}

			_disposed = true;
		}

		/// <summary>
		/// Finalizes an instance of the ESSession
		/// </summary>
        ~FlumeSession()
		{
			Dispose(false);
		}
	}
}