using Sfs2X.Entities.Data;

public class PushHotSpotEventDelMessage : BaseMessage
{
	private static PushHotSpotEventDelMessage _instance;

	public static PushHotSpotEventDelMessage Instance => _instance ?? (_instance = MessageFactory.GetMessage<PushHotSpotEventDelMessage>());

	public override string GetMsgId()
	{
		return "push.hot.spot.event.del";
	}

	protected override void CSHandleResponse(ISFSObject message)
	{
		if (!message.ContainsKey("errorCode"))
		{
			HeatSourceDataManager.GetInstance().HandlePushConstHotSpotDel(message);
		}
	}

	protected override bool showErrorCode()
	{
		return true;
	}
}
