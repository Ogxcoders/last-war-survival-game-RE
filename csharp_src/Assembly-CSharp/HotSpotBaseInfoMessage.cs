using Sfs2X.Entities.Data;
using Sfs2X.Requests;

public class HotSpotBaseInfoMessage : BaseMessage
{
	private static HotSpotBaseInfoMessage _instance;

	public static HotSpotBaseInfoMessage Instance => _instance ?? (_instance = MessageFactory.GetMessage<HotSpotBaseInfoMessage>());

	public override string GetMsgId()
	{
		return "hot.spot.base.info";
	}

	protected override IRequest CSSetData(params object[] args)
	{
		int val = (int)args[0];
		SFSObject sFSObject = new SFSObject();
		int futureId = GameEntry.Network.getFutureManager().getFutureId();
		sFSObject.PutInt("_id", futureId);
		sFSObject.PutInt("serverId", val);
		GameEntry.Network.getFutureManager().onSendRequest(futureId, GetMsgId());
		return new ExtensionRequest(GetMsgId(), sFSObject);
	}

	protected override void CSHandleResponse(ISFSObject message)
	{
		if (!message.ContainsKey("errorCode"))
		{
			HeatSourceDataManager.GetInstance().HandleHotSpotBaseInfo(message);
		}
	}

	protected override bool showErrorCode()
	{
		return true;
	}
}
