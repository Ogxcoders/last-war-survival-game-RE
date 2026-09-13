using GameFramework;
using Sfs2X.Entities.Data;

public class InitAfterMessage : BaseMessage
{
	private static InitAfterMessage _instance;

	public static InitAfterMessage Instance => _instance ?? (_instance = MessageFactory.GetMessage<InitAfterMessage>());

	public override string GetMsgId()
	{
		return "init.after";
	}

	protected override void CSHandleResponse(ISFSObject message)
	{
		Log.Info("[PushInit] handle PushInit after");
	}
}
