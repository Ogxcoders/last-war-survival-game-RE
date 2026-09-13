using Sfs2X.Entities.Data;
using Sfs2X.Requests;
using Zendesk;

public class ZendeskJWTTokenMessage : BaseMessage
{
	public class Request
	{
	}

	private static ZendeskJWTTokenMessage _instance;

	public static ZendeskJWTTokenMessage Instance => _instance ?? (_instance = MessageFactory.GetMessage<ZendeskJWTTokenMessage>());

	public override string GetMsgId()
	{
		return "create.zendesk.token";
	}

	protected override IRequest CSSetData(params object[] args)
	{
		ISFSObject iSFSObject = new SFSObject();
		int futureId = GameEntry.Network.getFutureManager().getFutureId();
		iSFSObject.PutInt("_id", futureId);
		GameEntry.Network.getFutureManager().onSendRequest(futureId, GetMsgId());
		return new ExtensionRequest(GetMsgId(), iSFSObject);
	}

	protected override void CSHandleResponse(ISFSObject message)
	{
		if (message.ContainsKey("errorCode"))
		{
			UIUtils.ShowTips(message.TryGetString("errorCode"), 3f);
		}
		else
		{
			ZendeskCore.ZendeskGetJWTToken(message.TryGetString("zendeskToken"));
		}
	}
}
