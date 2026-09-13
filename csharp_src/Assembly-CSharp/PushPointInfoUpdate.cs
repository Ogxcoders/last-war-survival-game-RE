using Sfs2X.Entities.Data;

public class PushPointInfoUpdate : BaseMessage
{
	private static PushPointInfoUpdate _instance;

	public static PushPointInfoUpdate Instance => _instance ?? (_instance = MessageFactory.GetMessage<PushPointInfoUpdate>());

	public override string GetMsgId()
	{
		return "push.world.point.update";
	}

	protected override void CSHandleResponse(ISFSObject message)
	{
		if (SceneManager.World != null)
		{
			SceneManager.World.HandleViewUpdateNotify(message);
		}
	}
}
