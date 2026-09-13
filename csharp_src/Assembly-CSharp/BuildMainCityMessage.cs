using Sfs2X.Entities.Data;
using Sfs2X.Requests;
using XLua;

[Hotfix(HotfixFlag.Stateless)]
public class BuildMainCityMessage : BaseMessage
{
	public class Request
	{
	}

	private static BuildMainCityMessage _instance;

	public static BuildMainCityMessage Instance => _instance ?? (_instance = MessageFactory.GetMessage<BuildMainCityMessage>());

	public override string GetMsgId()
	{
		return "build.main.city";
	}

	protected override IRequest CSSetData(params object[] args)
	{
		ISFSObject iSFSObject = new SFSObject();
		int futureId = GameEntry.Network.getFutureManager().getFutureId();
		iSFSObject.PutInt("_id", futureId);
		GameEntry.Network.getFutureManager().onSendRequest(futureId, GetMsgId());
		return new ExtensionRequest(GetMsgId(), iSFSObject);
	}

	protected override void CSHandleResponse(ISFSObject message)
	{
		if (message.ContainsKey("errorCode"))
		{
			UIUtils.ShowTips(message.TryGetString("errorCode"), 3f);
			return;
		}
		object param = ((SFSObject)message).ToLuaTable(GameEntry.Lua.Env);
		GameEntry.Lua.Call("CSharpCallLuaInterface.UpdateBuildings", param);
		GameEntry.Lua.Call("CSharpCallLuaInterface.InitRoadData", param);
		GameEntry.Lua.Call("CSharpCallLuaInterface.InitCityPointData", param);
		SceneManager.World.ClearReInitObject();
		SceneManager.World.ReInitObject();
		LuaBuildData buildingDataByBuildId = GameEntry.Data.Building.GetBuildingDataByBuildId(10100000);
		if (buildingDataByBuildId != null)
		{
			GameEntry.Event.Fire(EventId.BuildUpgradeFinish, buildingDataByBuildId.uuid);
			GameEntry.Sound.PlayEffect("effect_finished");
		}
		GameEntry.Event.Fire(EventId.BuildMainZeroUpgradeSuccess);
	}
}
