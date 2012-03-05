using System;

namespace FlumeAgent
{
	internal interface IConnection : IDisposable
	{
		DateTime Created { get; }
		bool IsOpen { get; }

		Server Server { get; }
        ThriftFlumeEventServer.Client Client { get; }

		void Open();
		void Close();
	}
}