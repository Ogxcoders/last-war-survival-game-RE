using GameFramework;
using Sfs2X.Entities.Data;

public class InitBeforeMessage : BaseMessage
{
	private static InitBeforeMessage _instance;

	public static InitBeforeMessage Instance => _instance ?? (_instance = MessageFactory.GetMessage<InitBeforeMessage>());

	public override string GetMsgId()
	{
		return "init.before";
	}

	protected override void CSHandleResponse(ISFSObject message)
	{
		Log.Info("[PushInit] handle PushInit before");
	}
}
