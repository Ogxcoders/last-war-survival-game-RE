using Sfs2X.Entities.Data;
using Sfs2X.Requests;

public class LoginPushShumeiExceptionLevelMessage : BaseMessage
{
	private static LoginPushShumeiExceptionLevelMessage _instance;

	public static LoginPushShumeiExceptionLevelMessage Instance => _instance ?? (_instance = MessageFactory.GetMessage<LoginPushShumeiExceptionLevelMessage>());

	public override string GetMsgId()
	{
		return "login.push.shumei.exception.level";
	}

	protected override IRequest CSSetData(params object[] args)
	{
		ISFSObject parameters = new SFSObject();
		return new ExtensionRequest(GetMsgId(), parameters);
	}

	protected override void CSHandleResponse(ISFSObject message)
	{
		if (message.ContainsKey("errorCode"))
		{
			UIUtils.ShowTips(message.TryGetString("errorCode"), 3f);
		}
		else
		{
			GameEntry.Event.Fire(EventId.LoginPushShumeiExceptionLevel, message);
		}
	}
}
