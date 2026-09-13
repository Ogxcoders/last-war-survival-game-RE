using Sfs2X.Entities.Data;

public class PushWorldGetBlock : BaseMessage
{
	private static PushWorldGetBlock _instance;

	public static PushWorldGetBlock Instance => _instance ?? (_instance = MessageFactory.GetMessage<PushWorldGetBlock>());

	public override string GetMsgId()
	{
		return "push.world.get.block";
	}

	protected override void CSHandleResponse(ISFSObject message)
	{
		if (SceneManager.World != null)
		{
			SceneManager.World.UpdateViewRequest(isForce: true);
		}
	}
}
