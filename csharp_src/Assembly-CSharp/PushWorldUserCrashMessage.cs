using Sfs2X.Entities.Data;

public class PushWorldUserCrashMessage : BaseMessage
{
	private static PushWorldUserCrashMessage _instance;

	public static PushWorldUserCrashMessage Instance => _instance ?? (_instance = MessageFactory.GetMessage<PushWorldUserCrashMessage>());

	public override string GetMsgId()
	{
		return "push.world.user.crash";
	}

	protected override void CSHandleResponse(ISFSObject message)
	{
		message?.ContainsKey("old");
	}
}
