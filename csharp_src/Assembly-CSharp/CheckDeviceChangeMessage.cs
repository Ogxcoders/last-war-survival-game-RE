using Sfs2X.Entities.Data;
using Sfs2X.Requests;

public class CheckDeviceChangeMessage : BaseMessage
{
	private static CheckDeviceChangeMessage _instance;

	public static CheckDeviceChangeMessage Instance => _instance ?? (_instance = MessageFactory.GetMessage<CheckDeviceChangeMessage>());

	public override string GetMsgId()
	{
		return "check.device.change";
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
		message?.TryGetBool("r");
	}
}
