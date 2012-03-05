namespace FlumeAgent
{
	internal interface IConnectionProvider
	{
		IConnection CreateConnection();

		IConnection Open();

		bool Close(IConnection connection);
	}
}