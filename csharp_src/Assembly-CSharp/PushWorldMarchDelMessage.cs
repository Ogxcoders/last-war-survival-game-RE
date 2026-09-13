using Sfs2X.Entities.Data;

public class PushWorldMarchDelMessage : BaseMessage
{
	private static PushWorldMarchDelMessage _instance;

	public static PushWorldMarchDelMessage Instance => _instance ?? (_instance = MessageFactory.GetMessage<PushWorldMarchDelMessage>());

	public override string GetMsgId()
	{
		return "push.world.march.del";
	}

	protected override void CSHandleResponse(ISFSObject message)
	{
		if (SceneManager.World != null)
		{
			SceneManager.MarchDataMgr.HandlePushWorldMarchDel(message);
		}
	}
}
