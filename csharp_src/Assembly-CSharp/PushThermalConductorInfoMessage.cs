using Sfs2X.Entities.Data;

public class PushThermalConductorInfoMessage : BaseMessage
{
	private static PushThermalConductorInfoMessage _instance;

	public static PushThermalConductorInfoMessage Instance => _instance ?? (_instance = MessageFactory.GetMessage<PushThermalConductorInfoMessage>());

	public override string GetMsgId()
	{
		return "push.thermal.conductor.info";
	}

	protected override void CSHandleResponse(ISFSObject message)
	{
		HeatSourceDataManager.GetInstance().HandlePushThermalConductorInfo(message);
	}
}
