using Sfs2X.Entities.Data;
using Sfs2X.Requests;

public class UserTowerChangeListenMessage : BaseMessage
{
	public class Request
	{
		public long towerUuid;

		public long targetUuid;
	}

	private static UserTowerChangeListenMessage _instance;

	public static UserTowerChangeListenMessage Instance => _instance ?? (_instance = MessageFactory.GetMessage<UserTowerChangeListenMessage>());

	public override string GetMsgId()
	{
		return "user.tower.change.listen";
	}

	protected override IRequest CSSetData(params object[] args)
	{
		Request request = args[0] as Request;
		ISFSObject iSFSObject = new SFSObject();
		iSFSObject.PutLong("towerUuid", request.towerUuid);
		iSFSObject.PutLong("targetUuid", request.targetUuid);
		int futureId = GameEntry.Network.getFutureManager().getFutureId();
		iSFSObject.PutInt("_id", futureId);
		GameEntry.Network.getFutureManager().onSendRequest(futureId, GetMsgId());
		return new ExtensionRequest(GetMsgId(), iSFSObject);
	}

	protected override void CSHandleResponse(ISFSObject message)
	{
	}
}
