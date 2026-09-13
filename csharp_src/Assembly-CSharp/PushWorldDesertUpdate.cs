using Sfs2X.Entities.Data;

public class PushWorldDesertUpdate : BaseMessage
{
	private static PushWorldDesertUpdate _instance;

	public static PushWorldDesertUpdate Instance => _instance ?? (_instance = MessageFactory.GetMessage<PushWorldDesertUpdate>());

	public override string GetMsgId()
	{
		return "push.world.desert.update";
	}

	protected override void CSHandleResponse(ISFSObject message)
	{
		if (SceneManager.World != null)
		{
			SceneManager.World.HandleViewTileUpdateNotify(message);
		}
	}
}
