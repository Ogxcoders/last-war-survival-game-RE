using Sfs2X.Entities.Data;

public class PushWorldObjStateChange : BaseMessage
{
	private static PushWorldObjStateChange _instance;

	public static PushWorldObjStateChange Instance => _instance ?? (_instance = MessageFactory.GetMessage<PushWorldObjStateChange>());

	public override string GetMsgId()
	{
		return "push.world.obj.state.change";
	}

	protected override void CSHandleResponse(ISFSObject message)
	{
		if (SceneManager.World != null && SceneManager.World is WorldScene worldScene)
		{
			worldScene.HandlePushWorldObjStateChange(message);
		}
	}
}
