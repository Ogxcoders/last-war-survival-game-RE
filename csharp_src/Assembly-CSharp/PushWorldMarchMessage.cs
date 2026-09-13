using Sfs2X.Entities.Data;
using XLua;

[Hotfix(HotfixFlag.Stateless)]
public class PushWorldMarchMessage : BaseMessage
{
	private static PushWorldMarchMessage _instance;

	public static PushWorldMarchMessage Instance => _instance ?? (_instance = MessageFactory.GetMessage<PushWorldMarchMessage>());

	public override string GetMsgId()
	{
		return "push.world.march.new";
	}

	protected override void CSHandleResponse(ISFSObject message)
	{
		if (SceneManager.World != null)
		{
			SceneManager.MarchDataMgr.HandlePushWorldMarchAdd(message);
		}
	}
}
