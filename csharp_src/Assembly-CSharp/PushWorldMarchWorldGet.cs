using Sfs2X.Entities.Data;

public class PushWorldMarchWorldGet : BaseMessage
{
	private static PushWorldMarchWorldGet _instance;

	public static PushWorldMarchWorldGet Instance => _instance ?? (_instance = MessageFactory.GetMessage<PushWorldMarchWorldGet>());

	public override string GetMsgId()
	{
		return "push.world.march.world.get.new";
	}

	protected override void CSHandleResponse(ISFSObject message)
	{
		if (SceneManager.World != null)
		{
			SceneManager.MarchDataMgr.HandleWorldMarchGet(message);
		}
	}
}
