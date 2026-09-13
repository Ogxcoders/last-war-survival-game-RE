using Sfs2X.Entities.Data;

public class PushWorldBeMoveMessage : BaseMessage
{
	private static PushWorldBeMoveMessage _instance;

	public static PushWorldBeMoveMessage Instance => _instance ?? (_instance = MessageFactory.GetMessage<PushWorldBeMoveMessage>());

	public override string GetMsgId()
	{
		return "push.city.be.move";
	}

	protected override void CSHandleResponse(ISFSObject message)
	{
	}
}
