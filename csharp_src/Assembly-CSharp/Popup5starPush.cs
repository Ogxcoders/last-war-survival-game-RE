using Sfs2X.Entities.Data;

public class Popup5starPush : BaseMessage
{
	private static Popup5starPush _instance;

	public static Popup5starPush Instance => _instance ?? (_instance = MessageFactory.GetMessage<Popup5starPush>());

	public override string GetMsgId()
	{
		return "push.popup.5star";
	}

	protected override void CSHandleResponse(ISFSObject message)
	{
	}
}
