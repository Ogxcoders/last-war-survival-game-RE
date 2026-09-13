using System;
using System.Net;

namespace Joker.Server;

public class RouterServer : World, IStartupSync, IShutdownSync, IUpdate
{
	private NetworkSystem _network;

	private DoubleMap<int, long> _idToChannel = new DoubleMap<int, long>();

	public void Startup()
	{
		Log.Info("RouterServer Startup");
		_network = AddSystem<NetworkSystem, IPEndPoint>(new IPEndPoint(IPAddress.Any, 2525));
		NetworkSystem network = _network;
		network.OnAccept = (Action<long, IPEndPoint>)Delegate.Combine(network.OnAccept, new Action<long, IPEndPoint>(_OnAccept));
		NetworkSystem network2 = _network;
		network2.OnMessage = (Action<long, IMessage>)Delegate.Combine(network2.OnMessage, new Action<long, IMessage>(_OnMessage));
		NetworkSystem network3 = _network;
		network3.OnDisconnect = (Action<long>)Delegate.Combine(network3.OnDisconnect, new Action<long>(_OnDisconnect));
	}

	public void Shutdown()
	{
		Log.Info("RouterServer Shutdown");
	}

	public void Update()
	{
		(IMessage, object) message;
		while (TryDequeueMessage(out message))
		{
			if (message.Item1 is IRouteServerMessage routeServerMessage)
			{
				if (routeServerMessage.GetTo() == base.Id)
				{
					Log.Error($"can not route RouteMessage from:{routeServerMessage.GetFrom()} to self:{routeServerMessage.GetTo()} {message.Item1} in server: {base.Id}");
					continue;
				}
				World world = Singleton<WorldService>.Instance.GetWorld(routeServerMessage.GetTo());
				if (world != null)
				{
					world.EnqueueMessage(routeServerMessage, routeServerMessage.GetFrom());
					continue;
				}
				if (_idToChannel.TryGetValueByKey(routeServerMessage.GetTo(), out var value))
				{
					_network.Send(value, message.Item1);
					continue;
				}
				Log.Error($"can not route RouteMessage from:{routeServerMessage.GetFrom()} to:{routeServerMessage.GetTo()} {message.Item1} in server: {base.Id}");
			}
			else if (message.Item1 is IRouteActorMessage routeActorMessage)
			{
				string text = routeActorMessage.GetActorId();
				if (string.IsNullOrEmpty(text))
				{
					text = routeActorMessage.GetAccountId();
				}
				int targetId = Singleton<WorldService>.Instance.GetTargetId(text);
				World world2 = Singleton<WorldService>.Instance.GetWorld(targetId);
				long value2 = -1L;
				IRouteActorWithServerMessage routeActorWithServerMessage;
				if ((routeActorWithServerMessage = routeActorMessage as IRouteActorWithServerMessage) == null)
				{
					S2SRouteActorWithServerMessage s2SRouteActorWithServerMessage = new S2SRouteActorWithServerMessage();
					s2SRouteActorWithServerMessage.AccountId = routeActorMessage.GetAccountId();
					s2SRouteActorWithServerMessage.TargetId = routeActorMessage.GetActorId();
					if (_idToChannel.TryGetValueByKey(targetId, out value2))
					{
						s2SRouteActorWithServerMessage.To = targetId;
					}
					else
					{
						s2SRouteActorWithServerMessage.To = -1;
					}
					s2SRouteActorWithServerMessage.From = _idToChannel.GetKeyByValue((long)message.Item2);
					s2SRouteActorWithServerMessage.To = world2.Id;
					s2SRouteActorWithServerMessage.Message = message.Item1;
					routeActorWithServerMessage = s2SRouteActorWithServerMessage;
				}
				if (world2 != null)
				{
					world2.EnqueueMessage(routeActorWithServerMessage, routeActorWithServerMessage.GetFrom());
					continue;
				}
				if (routeActorWithServerMessage.GetTo() >= 0)
				{
					_network.Send(value2, routeActorWithServerMessage);
					continue;
				}
				Log.Error($"can not route RouteMessage from:{routeActorWithServerMessage.GetFrom()} to:{routeActorWithServerMessage.GetTo()} {message.Item1} in server: {base.Id}");
			}
			else if (message.Item1 is S2RRouterRegisterMessage s2RRouterRegisterMessage)
			{
				_idToChannel.Add(s2RRouterRegisterMessage.Node, s2RRouterRegisterMessage.ToChannel);
				R2SRouterRegisterMessage r2SRouterRegisterMessage = new R2SRouterRegisterMessage();
				r2SRouterRegisterMessage.Code = 0;
				_network.Send(s2RRouterRegisterMessage.ToChannel, r2SRouterRegisterMessage);
				Log.Info($"RouterServer register to {s2RRouterRegisterMessage.Node} {s2RRouterRegisterMessage.Name}");
			}
			else
			{
				Log.Error($"can not route message:{message.Item1}");
			}
		}
	}

	private void _OnAccept(long channel, IPEndPoint address)
	{
		Log.Info($"RouterServer accept to {address} {channel}");
	}

	private void _OnMessage(long channel, IMessage message)
	{
		Log.Debug($"RouterServer {base.Id} OnMessage {channel} {message}");
		if (message != null)
		{
			if (!(message is S2RRouterRegisterMessage s2RRouterRegisterMessage))
			{
				if (!(message is IRouteMessage))
				{
					goto IL_0043;
				}
			}
			else
			{
				s2RRouterRegisterMessage.ToChannel = channel;
			}
			EnqueueMessage(message, null);
			return;
		}
		goto IL_0043;
		IL_0043:
		Log.Error($"can not route message:{message}");
	}

	private void _OnDisconnect(long channel)
	{
		Log.Info($"RouterServer disconnect to {channel}");
		_idToChannel.RemoveByValue(channel);
	}
}
