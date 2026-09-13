using Sfs2X.Entities.Data;
using Sfs2X.Requests;

public class UserCleanPostMessage : BaseMessage
{
	private static UserCleanPostMessage _instance;

	public static UserCleanPostMessage Instance => _instance ?? (_instance = MessageFactory.GetMessage<UserCleanPostMessage>());

	public override string GetMsgId()
	{
		return "user.clean.post";
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
		ApplicationLaunch.Instance.ReloadGame();
	}
}
