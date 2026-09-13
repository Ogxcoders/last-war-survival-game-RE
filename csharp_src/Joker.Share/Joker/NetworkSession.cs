using System;
using System.Net;

namespace Joker;

public class NetworkSession : IDisposable
{
	public bool IsValid { get; private set; }

	public NetworkSystem Owner { get; }

	public long ChannelID { get; }

	public IPEndPoint IPEndPoint { get; }

	public NetworkSession(NetworkSystem owner, long channelID, IPEndPoint ipEndPoint)
	{
		IsValid = true;
		Owner = owner;
		ChannelID = channelID;
		IPEndPoint = ipEndPoint;
	}

	public virtual void SendMessage(IMessage message)
	{
		Owner.Send(ChannelID, message);
	}

	public virtual void HandleMessage(IMessage message)
	{
		Owner.World.EnqueueMessage(message, this);
	}

	public virtual void Dispose()
	{
		if (IsValid)
		{
			Owner.Disconnect(ChannelID);
			IsValid = false;
		}
	}
}
