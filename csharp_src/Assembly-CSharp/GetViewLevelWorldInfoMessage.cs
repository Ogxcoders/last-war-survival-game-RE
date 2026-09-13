using Sfs2X.Entities.Data;
using Sfs2X.Requests;
using XLua;

public class GetViewLevelWorldInfoMessage : BaseMessage
{
	public class Request
	{
		public int x;

		public int y;

		public int serverId = -1;

		public int type;

		public int viewLvl;

		public int worldId;
	}

	private static GetViewLevelWorldInfoMessage _instance;

	public static GetViewLevelWorldInfoMessage Instance => _instance ?? (_instance = MessageFactory.GetMessage<GetViewLevelWorldInfoMessage>());

	public override string GetMsgId()
	{
		return "world.get.new";
	}

	protected override IRequest CSSetData(params object[] args)
	{
		Request request = args[0] as Request;
		SFSObject sFSObject = new SFSObject();
		int futureId = GameEntry.Network.getFutureManager().getFutureId();
		sFSObject.PutInt("_id", futureId);
		GameEntry.Network.getFutureManager().onSendRequest(futureId, GetMsgId());
		sFSObject.PutInt("x", request.x);
		sFSObject.PutInt("y", request.y);
		sFSObject.PutInt("serverId", request.serverId);
		sFSObject.PutInt("type", request.type);
		sFSObject.PutInt("viewLvl", request.viewLvl);
		sFSObject.PutInt("timeStamp", (int)GameEntry.Timer.GetServerTime());
		sFSObject.PutInt("worldId", request.worldId);
		return new ExtensionRequest(GetMsgId(), sFSObject);
	}

	protected override void CSHandleResponse(ISFSObject message)
	{
		if (message.ContainsKey("errorCode"))
		{
			return;
		}
		if (message.ContainsKey("timeStamp"))
		{
			GameEntry.Timer.UpdateServerMilliseconds(message.GetLong("timeStamp"));
			GameEntry.Lua.UpdateUITimeStamp(message.GetLong("timeStamp"));
		}
		if (SceneManager.IsInWorld())
		{
			if (GameEntry.Data.Player.IsInBattleField())
			{
				int worldType = GameEntry.Data.Player.GetWorldType();
				LuaTable luaTable = GameEntry.Lua.CallWithReturn<LuaTable, int>("CSharpCallLuaInterface.GetBattleFieldWorldSize", worldType);
				SceneManager.World.SetWorldSize(luaTable.Get<int>("x"), luaTable.Get<int>("y"));
			}
			else if (SeasonDataManager.Instance.InSeasonBigMapMode())
			{
				SceneManager.World.SetWorldSize(3000);
			}
			else
			{
				SceneManager.World.SetWorldSize(1000);
			}
			SceneManager.World.HandleViewPointsReply(message);
		}
	}
}
