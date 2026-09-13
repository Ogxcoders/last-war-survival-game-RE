using Sfs2X.Entities.Data;

public class PushWolfStatusChangeMessage : BaseMessage
{
	private static PushWolfStatusChangeMessage _instance;

	public static PushWolfStatusChangeMessage Instance => _instance ?? (_instance = MessageFactory.GetMessage<PushWolfStatusChangeMessage>());

	public override string GetMsgId()
	{
		return "push.wolf.status.change";
	}

	protected override void CSHandleResponse(ISFSObject message)
	{
		SceneManager.World.HandlePushWolfStatusChange(message);
	}
}
