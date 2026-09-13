using Sfs2X.Entities.Data;
using Sfs2X.Requests;

public class SeasonHunterRefreshShadowInfoMessage : BaseMessage
{
	private static SeasonHunterRefreshShadowInfoMessage _instance;

	public static SeasonHunterRefreshShadowInfoMessage Instance => _instance ?? (_instance = MessageFactory.GetMessage<SeasonHunterRefreshShadowInfoMessage>());

	public override string GetMsgId()
	{
		return "season.hunter.refresh.shadow.info";
	}

	protected override IRequest CSSetData(params object[] args)
	{
		SFSObject sFSObject = new SFSObject();
		int futureId = GameEntry.Network.getFutureManager().getFutureId();
		sFSObject.PutInt("_id", futureId);
		if (args.Length > 1)
		{
			long val = (long)args[0];
			sFSObject.PutLong("treasureUuid", val);
			int val2 = (int)args[1];
			sFSObject.PutInt("tarServerId", val2);
		}
		GameEntry.Network.getFutureManager().onSendRequest(futureId, GetMsgId());
		return new ExtensionRequest(GetMsgId(), sFSObject);
	}

	protected override void CSHandleResponse(ISFSObject message)
	{
	}

	protected override bool showErrorCode()
	{
		return true;
	}
}
