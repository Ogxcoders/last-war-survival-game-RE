using Sfs2X.Entities.Data;

public class PushUpdateWorldAssistanceInfoMessage : BaseMessage
{
	private static PushUpdateWorldAssistanceInfoMessage _instance;

	public static PushUpdateWorldAssistanceInfoMessage Instance => _instance ?? (_instance = MessageFactory.GetMessage<PushUpdateWorldAssistanceInfoMessage>());

	public override string GetMsgId()
	{
		return "push.update.world.assistance.Info";
	}

	protected override void CSHandleResponse(ISFSObject message)
	{
		if (message != null && !message.ContainsKey("errorCode"))
		{
			SceneManager.World?.HandleViewAssistanceInfoUpdateNotify(message);
		}
	}

	protected override bool showErrorCode()
	{
		return true;
	}
}
