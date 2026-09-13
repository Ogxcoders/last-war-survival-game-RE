using Sfs2X.Entities.Data;
using Sfs2X.Requests;

public class PushRecordMessage : BaseMessage
{
	public class Request
	{
		public string record;

		public string click;
	}

	private static PushRecordMessage _instance;

	public static PushRecordMessage Instance => _instance ?? (_instance = MessageFactory.GetMessage<PushRecordMessage>());

	public override string GetMsgId()
	{
		return "push.record";
	}

	protected override IRequest CSSetData(params object[] args)
	{
		Request request = args[0] as Request;
		ISFSObject iSFSObject = new SFSObject();
		iSFSObject.PutUtfString("pushRecordData", request.record);
		iSFSObject.PutUtfString("pushClickData", request.click);
		int futureId = GameEntry.Network.getFutureManager().getFutureId();
		iSFSObject.PutInt("_id", futureId);
		GameEntry.Network.getFutureManager().onSendRequest(futureId, GetMsgId());
		return new ExtensionRequest(GetMsgId(), iSFSObject);
	}

	protected override void CSHandleResponse(ISFSObject message)
	{
		PushManager.Instance.clearPushCache();
	}
}
