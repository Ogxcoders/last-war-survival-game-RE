using Sfs2X.Entities.Data;
using Sfs2X.Requests;

public class WorldLeaveCrossServerMessage : BaseMessage
{
	private static WorldLeaveCrossServerMessage _instance;

	public static WorldLeaveCrossServerMessage Instance => _instance ?? (_instance = MessageFactory.GetMessage<WorldLeaveCrossServerMessage>());

	public override string GetMsgId()
	{
		return "user.leave.world";
	}

	public void SendRequest()
	{
		GameEntry.NetworkCross.Send(this);
	}

	protected override IRequest CSSetData(params object[] args)
	{
		ISFSObject iSFSObject = new SFSObject();
		int futureId = GameEntry.Network.getFutureManager().getFutureId();
		iSFSObject.PutInt("_id", futureId);
		GameEntry.Network.getFutureManager().onSendRequest(futureId, GetMsgId());
		iSFSObject.PutInt("worldId", GameEntry.Data.Player.GetWorldId());
		return new ExtensionRequest(GetMsgId(), iSFSObject);
	}

	public override void Send(params object[] args)
	{
		IRequest request = CSSetData(args);
		GameEntry.NetworkCross.Send(request);
	}

	protected override void CSHandleResponse(ISFSObject message)
	{
	}
}
