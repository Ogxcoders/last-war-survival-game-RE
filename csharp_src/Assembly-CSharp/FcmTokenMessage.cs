using Sfs2X.Entities.Data;
using Sfs2X.Requests;

public class FcmTokenMessage : BaseMessage
{
	public class Request
	{
		public string token;

		public string fireabaseAppId;
	}

	private static FcmTokenMessage _instance;

	public static FcmTokenMessage Instance => _instance ?? (_instance = MessageFactory.GetMessage<FcmTokenMessage>());

	public override string GetMsgId()
	{
		return "change.user.parseid";
	}

	protected override IRequest CSSetData(params object[] args)
	{
		Request request = args[0] as Request;
		ISFSObject iSFSObject = new SFSObject();
		int futureId = GameEntry.Network.getFutureManager().getFutureId();
		iSFSObject.PutInt("_id", futureId);
		GameEntry.Network.getFutureManager().onSendRequest(futureId, GetMsgId());
		if (!request.token.IsNullOrEmpty() && !request.token.Equals("|") && !request.token.Equals("|fcm"))
		{
			iSFSObject.PutUtfString("parseRegisterId", request.token);
		}
		if (!request.fireabaseAppId.IsNullOrEmpty())
		{
			iSFSObject.PutUtfString("fireabaseAppId", request.fireabaseAppId);
		}
		return new ExtensionRequest(GetMsgId(), iSFSObject);
	}

	protected override void CSHandleResponse(ISFSObject message)
	{
	}
}
