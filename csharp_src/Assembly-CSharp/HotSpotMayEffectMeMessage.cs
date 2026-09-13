using Sfs2X.Entities.Data;

public class HotSpotMayEffectMeMessage : BaseMessage
{
	private static HotSpotMayEffectMeMessage _instance;

	public static HotSpotMayEffectMeMessage Instance => _instance ?? (_instance = MessageFactory.GetMessage<HotSpotMayEffectMeMessage>());

	public override string GetMsgId()
	{
		return "hot.spot.may.effect.me";
	}

	protected override void CSHandleResponse(ISFSObject message)
	{
		if (!message.ContainsKey("errorCode"))
		{
			HeatSourceDataManager.GetInstance().HandleHotSpotMayEffectMe(message);
		}
	}

	protected override bool showErrorCode()
	{
		return true;
	}
}
