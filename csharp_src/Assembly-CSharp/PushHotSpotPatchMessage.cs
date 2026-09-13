using Sfs2X.Entities.Data;

public class PushHotSpotPatchMessage : BaseMessage
{
	private static PushHotSpotPatchMessage _instance;

	public static PushHotSpotPatchMessage Instance => _instance ?? (_instance = MessageFactory.GetMessage<PushHotSpotPatchMessage>());

	public override string GetMsgId()
	{
		return "push.hot.spot.patch";
	}

	protected override void CSHandleResponse(ISFSObject message)
	{
		if (!message.ContainsKey("errorCode"))
		{
			HeatSourceDataManager.GetInstance().HandlePushHotSpotPatch(message);
		}
	}

	protected override bool showErrorCode()
	{
		return true;
	}
}
