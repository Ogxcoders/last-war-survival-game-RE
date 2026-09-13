using System;
using System.Reflection;

namespace Joker.Server;

public class TGateServer<T> : GateServer where T : GateActor
{
	protected readonly ConstructorInfo _factory;

	protected TGateServer()
	{
		_factory = typeof(T).GetConstructor(BindingFlags.Instance | BindingFlags.NonPublic, null, new Type[4]
		{
			typeof(GateServer),
			typeof(long),
			typeof(string),
			typeof(string)
		}, null);
	}

	protected override GateActor CreateGateActor(long channelId, string accountId, string roomId)
	{
		return (GateActor)_factory.Invoke(new object[4] { this, channelId, accountId, roomId });
	}
}
public class TGateServer : GateServer
{
	protected readonly Func<GateServer, long, string, string, GateActor> _factory;

	public TGateServer(Func<GateServer, long, string, string, GateActor> factory)
	{
		_factory = factory;
	}

	protected override GateActor CreateGateActor(long channelId, string accountId, string roomId)
	{
		return _factory(this, channelId, accountId, roomId);
	}
}
