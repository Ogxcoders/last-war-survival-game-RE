using System.Collections.Generic;
using System.Net;

namespace Joker.Server;

public class GateServer : World, IStartupSync, IShutdownSync, IUpdate
{
	protected Dictionary<long, GateActor> _channelToPlayers = new Dictionary<long, GateActor>();

	protected Dictionary<string, GateActor> _accountToPlayers = new Dictionary<string, GateActor>();

	public NetworkSystem Network { get; private set; }

	public RouterSystem Router { get; private set; }

	public void Startup()
	{
		Log.Info("GateServer Startup");
		Network = AddSystem<NetworkSystem, IPEndPoint>(new IPEndPoint(IPAddress.Any, 7758));
		Router = AddSystem<RouterSystem>();
		Network.OnAccept = _OnAccept;
		Network.OnMessage = _OnMessage;
		Network.OnDisconnect = _OnDisconnect;
	}

	public void Shutdown()
	{
		Log.Info("GateServer Shutdown");
	}

	public virtual void Update()
	{
		(IMessage, object) message;
		while (TryDequeueMessage(out message))
		{
			if (!Router.ProcessMessage(message.Item1))
			{
				DoProcessMessage(message.Item1, message.Item2);
			}
		}
	}

	public bool TryGetActor(string accountId, long channelId, out GateActor actor)
	{
		if (!string.IsNullOrEmpty(accountId) && TryGetActorByAccount(accountId, channelId, out actor))
		{
			return true;
		}
		if (channelId >= 0 && TryGetActorByChannel(channelId, out actor))
		{
			return true;
		}
		actor = null;
		return false;
	}

	public bool TryGetActorByChannel(long channelId, out GateActor actor)
	{
		return _channelToPlayers.TryGetValue(channelId, out actor);
	}

	public bool TryGetActorByAccount(string accountId, long channel, out GateActor actor)
	{
		if (_accountToPlayers.TryGetValue(accountId, out actor))
		{
			if (channel == -1 || actor.ChannelID == channel)
			{
				return true;
			}
			Log.Error($"Channel {channel} is currently in channel {actor.ChannelID}");
			return false;
		}
		return false;
	}

	protected virtual void DoProcessMessage(IMessage message, object sender)
	{
		if (message is IRouteServerWithActorMessage routeServerWithActorMessage)
		{
			if (!(routeServerWithActorMessage.Message is S2CLogout))
			{
				if (routeServerWithActorMessage.GetTo() == base.Id && TryGetActor(routeServerWithActorMessage.GetAccountId(), -1L, out var actor))
				{
					actor.SendToClient(routeServerWithActorMessage.Message);
				}
				else
				{
					Log.Warning("can not find actor " + routeServerWithActorMessage.GetAccountId());
				}
			}
			return;
		}
		if (message is IDecoMessage decoMessage)
		{
			message = decoMessage.Message;
		}
		GateActor actor4;
		if (message is C2SLogin c2SLogin)
		{
			if (TryAddGateActor((long)sender, c2SLogin.AccountId, c2SLogin.ActorId, out var actor2))
			{
				actor2.SendToServer(c2SLogin);
			}
			else
			{
				Log.Error("can not add actor " + c2SLogin.AccountId + " " + c2SLogin.ActorId);
			}
		}
		else if (message is S2CLogout s2CLogout)
		{
			if (TryGetActorByAccount(s2CLogout.AccountId, -1L, out var _))
			{
				TryDelGateActorByAccount(s2CLogout.AccountId);
			}
		}
		else if (TryGetActorByChannel((long)sender, out actor4))
		{
			actor4.SendToActor(message);
		}
	}

	private void _OnAccept(long id, IPEndPoint address)
	{
	}

	private void _OnMessage(long id, IMessage message)
	{
		Log.Debug($"GateServer {base.Id} OnMessage {id} {message}");
		EnqueueMessage(message, id);
	}

	private void _OnDisconnect(long id)
	{
		if (TryGetActorByChannel(id, out var actor))
		{
			C2SLogout message = new C2SLogout
			{
				AccountId = actor.AccountId
			};
			actor.SendToServer(message);
			if (!TryDelGateActorByChannel(id))
			{
				Log.Error($"can not del actor by channel {id}");
			}
		}
	}

	protected virtual GateActor CreateGateActor(long channelId, string accountId, string roomId)
	{
		return new GateActor(this, channelId, accountId, roomId);
	}

	protected virtual bool TryAddGateActor(long channel, string accountId, string roomId, out GateActor actor)
	{
		if (_channelToPlayers.TryGetValue(channel, out actor))
		{
			return false;
		}
		actor = CreateGateActor(channel, accountId, roomId);
		_channelToPlayers.Add(channel, actor);
		_accountToPlayers.Add(accountId, actor);
		return true;
	}

	protected virtual bool TryDelGateActorByChannel(long channel)
	{
		if (_channelToPlayers.TryGetValue(channel, out var value))
		{
			return TryDelGateActor(value);
		}
		return false;
	}

	public virtual bool TryDelGateActorByAccount(string account)
	{
		if (_accountToPlayers.TryGetValue(account, out var value))
		{
			return TryDelGateActor(value);
		}
		return false;
	}

	public virtual bool TryDelGateActor(GateActor actor)
	{
		bool result = false;
		if (_accountToPlayers.ContainsKey(actor.AccountId))
		{
			_accountToPlayers.Remove(actor.AccountId);
			result = true;
		}
		if (_channelToPlayers.ContainsKey(actor.ChannelID))
		{
			_channelToPlayers.Remove(actor.ChannelID);
		}
		return result;
	}
}
