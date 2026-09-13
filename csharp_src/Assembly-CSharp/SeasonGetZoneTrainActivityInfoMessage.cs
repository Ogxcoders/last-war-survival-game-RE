using Sfs2X.Entities.Data;
using Sfs2X.Requests;

public class SeasonGetZoneTrainActivityInfoMessage : BaseMessage
{
	private static SeasonGetZoneTrainActivityInfoMessage _instance;

	public static SeasonGetZoneTrainActivityInfoMessage Instance => _instance ?? (_instance = MessageFactory.GetMessage<SeasonGetZoneTrainActivityInfoMessage>());

	public override string GetMsgId()
	{
		return "season.get.zone.train.activity.info";
	}

	protected override IRequest CSSetData(params object[] args)
	{
		ISFSObject iSFSObject = new SFSObject();
		int futureId = GameEntry.Network.getFutureManager().getFutureId();
		GameEntry.Network.getFutureManager().onSendRequest(futureId, GetMsgId());
		iSFSObject.PutInt("_id", futureId);
		return new ExtensionRequest(GetMsgId(), iSFSObject);
	}

	protected override void CSHandleResponse(ISFSObject message)
	{
		message.ContainsKey("errorCode");
		GameEntry.Lua.Call("CSharpCallLuaInterface.HandelSeasonGetZoneTrainActivityInfo", ((SFSObject)message).ToLuaTable(GameEntry.Lua.Env));
	}
}
