using System;
using System.Threading;
using Thrift.Protocol;
using Thrift.Transport;

namespace FlumeAgent
{	
	internal class Connection : IConnection, IDisposable
	{
        private readonly ThriftFlumeEventServer.Client _client;
		private readonly TProtocol _protocol;
		private readonly TTransport _transport;
		private bool _disposed;

		/// <summary>
		/// 
		/// </summary>
		public Connection(Server server)
		{
			Created = DateTime.Now;
			Server = server;
			var tsocket = new TSocket(server.Host, server.Port);
			_transport = new TBufferedTransport(tsocket, 1024); 
			_protocol = new TBinaryProtocol(_transport);
            _client = new ThriftFlumeEventServer.Client(_protocol);
		}

		#region IConnection Members

		/// <summary>
		/// 
		/// </summary>
		public DateTime Created { get; private set; }

		/// <summary>
		/// 
		/// </summary>
		public Server Server { get; private set; }

		/// <summary>
		/// 
		/// </summary>
		public bool IsOpen
		{
			get { return _transport.IsOpen; }
		}

		/// <summary>
		/// 
		/// </summary>
		public void Open()
		{
			if (IsOpen)
				return;

			_transport.Open();
		}

		/// <summary>
		/// 
		/// </summary>
		public void Close()
		{
			if (!IsOpen)
				return;

			_transport.Close();
		}

		/// <summary>
		/// 
		/// </summary>
        public ThriftFlumeEventServer.Client Client
		{
			get { return _client; }
		}

		/// <summary>
		/// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
		/// </summary>
		public void Dispose()
		{
			Dispose(true);
		}

		#endregion

		/// <summary>
		/// Releases unmanaged and - optionally - managed resources
		/// </summary>
		/// <param name="disposing"><c>true</c> to release both managed and unmanaged resources; <c>false</c> to release only unmanaged resources.</param>
		protected virtual void Dispose(bool disposing)
		{
			if (_disposed)
			{
				return;
			}

			Close();
			_disposed = true;
		}

		/// <summary>
		/// Releases unmanaged resources and performs other cleanup operations before the
		/// <see cref="Connection"/> is reclaimed by garbage collection.
		/// </summary>
		~Connection()
		{
			Dispose(false);
		}
	}

    internal class AggregateCounter
    {
        private const int c_lsFree = 0;
        private const int c_lsOwned = 1;
        private readonly int[] data;
        private int _lock = c_lsFree;
        private int countThisMinute;
        private int countThisSecond;
        private int cursor;

        public AggregateCounter(int count)
        {
            data = new int[count];
        }

        public void IncrementCounter()
        {
            Interlocked.Increment(ref countThisSecond);
        }

        public void IncrementCounterBy(int value)
        {
            Interlocked.Add(ref countThisSecond, value);
        }

        private bool EnterLock()
        {
            Thread.BeginCriticalRegion();

            // If resource available, set it to in-use and return
            if (Interlocked.Exchange(ref _lock, c_lsOwned) == c_lsFree)
            {
                return true;
            }
            else
            {
                Thread.EndCriticalRegion();
                return false;
            }
        }

        private void ExitLock()
        {
            // Mark the resource as available
            Interlocked.Exchange(ref _lock, c_lsFree);
            Thread.EndCriticalRegion();
        }

        /// <summary>
        /// Grabs the accumulated amount in the counter and clears it.
        /// This needs to be called by a 1 second timer.
        /// </summary>
        /// <returns>The total aggregated over the last min</returns>
        public int Tick()
        {
            if (EnterLock())
            {
                try
                {
                    int totalThisSecond = Interlocked.Exchange(ref countThisSecond, 0);
                    int valueFrom1MinAgo = Interlocked.Exchange(ref data[cursor], totalThisSecond);

                    cursor++;
                    if (cursor >= data.Length) cursor = 0;

                    countThisMinute -= valueFrom1MinAgo;
                    countThisMinute += totalThisSecond;

                    return countThisMinute;
                }
                finally
                {
                    ExitLock();
                }
            }

            return -1;
        }
    }
}