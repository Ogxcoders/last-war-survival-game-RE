using Sfs2X.Entities.Data;
using Sfs2X.Requests;

public class ThermalConductorInfoMessage : BaseMessage
{
	private static ThermalConductorInfoMessage _instance;

	public static ThermalConductorInfoMessage Instance => _instance ?? (_instance = MessageFactory.GetMessage<ThermalConductorInfoMessage>());

	public override string GetMsgId()
	{
		return "thermal.conductor.info";
	}

	protected override IRequest CSSetData(params object[] args)
	{
		SFSObject sFSObject = new SFSObject();
		int futureId = GameEntry.Network.getFutureManager().getFutureId();
		sFSObject.PutInt("_id", futureId);
		if (args.Length != 0)
		{
			string val = (string)args[0];
			sFSObject.PutUtfString("uuid", val);
		}
		GameEntry.Network.getFutureManager().onSendRequest(futureId, GetMsgId());
		return new ExtensionRequest(GetMsgId(), sFSObject);
	}

	protected override void CSHandleResponse(ISFSObject message)
	{
		if (!message.ContainsKey("errorCode"))
		{
			HeatSourceDataManager.GetInstance().HandleThermalConductorInfo(message);
		}
	}

	protected override bool showErrorCode()
	{
		return true;
	}
}
