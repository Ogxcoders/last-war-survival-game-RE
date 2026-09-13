namespace Joker.Server;

public class GateActor
{
	private GateServer _server;

	private NetworkSystem _network;

	public int GateId => _server.Id;

	public long ChannelID { get; private set; } = -1L;

	public string AccountId { get; private set; } = string.Empty;

	public string RoomID { get; private set; } = string.Empty;

	public string TargetId
	{
		get
		{
			if (!string.IsNullOrEmpty(RoomID))
			{
				return RoomID;
			}
			return AccountId;
		}
	}

	public GateActor(GateServer server, long channelID, string accountId, string roomId)
	{
		_server = server;
		AccountId = accountId;
		RoomID = roomId;
		ChannelID = channelID;
		_network = _server.GetSystem<NetworkSystem>();
	}

	public void SendToClient(IMessage message, object source = null)
	{
		_network.Send(ChannelID, message);
	}

	public void SendToServer(IMessage message, object source = null)
	{
		_server.Router.SendToServer(RoomID, message);
	}

	public void SendToActor(IMessage message, object source = null)
	{
		_server.Router.SendToActorWithServer(AccountId, RoomID, message);
	}

	public virtual bool TryProcessMessage(IMessage message, object sender)
	{
		_server.Router.SendToServer(RoomID, message);
		return true;
	}
}
