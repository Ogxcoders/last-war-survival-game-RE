using Sfs2X.Entities.Data;

public class PushWorldLandUpdate : BaseMessage
{
	private static PushWorldLandUpdate _instance;

	public static PushWorldLandUpdate Instance => _instance ?? (_instance = MessageFactory.GetMessage<PushWorldLandUpdate>());

	public override string GetMsgId()
	{
		return "push.world.land.update";
	}

	protected override void CSHandleResponse(ISFSObject message)
	{
		if (SceneManager.World != null)
		{
			SceneManager.World.HandleLandUpdate(message);
		}
	}
}
