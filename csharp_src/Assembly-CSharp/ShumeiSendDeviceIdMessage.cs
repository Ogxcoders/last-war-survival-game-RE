using Sfs2X.Entities.Data;
using Sfs2X.Requests;

public class ShumeiSendDeviceIdMessage : BaseMessage
{
	public class Request
	{
		public string boxId;
	}

	private static ShumeiSendDeviceIdMessage _instance;

	public static ShumeiSendDeviceIdMessage Instance => _instance ?? (_instance = MessageFactory.GetMessage<ShumeiSendDeviceIdMessage>());

	public override string GetMsgId()
	{
		return "shumei.request";
	}

	protected override IRequest CSSetData(params object[] args)
	{
		Request request = args[0] as Request;
		ISFSObject iSFSObject = new SFSObject();
		iSFSObject.PutUtfString("boxId", request.boxId);
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
			ShumeiSdkManager.Instance.PrintInfoLog("send deviceId success");
		}
	}
}
