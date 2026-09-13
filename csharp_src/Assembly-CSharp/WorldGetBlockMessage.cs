using System.Text;
using Sfs2X.Entities.Data;
using Sfs2X.Requests;
using XLua;

public class WorldGetBlockMessage : BaseMessage
{
	public class Request
	{
		public int bigMap;

		public int x;

		public int y;

		public int serverId = -1;

		public int worldId;

		public int type;

		public int lod;

		public int[] index;

		public int blockSize;

		public bool firstTime;

		public int leftBottom;

		public int rightTop;

		public bool battleFieldFirst;
	}

	private static WorldGetBlockMessage _instance;

	public static WorldGetBlockMessage Instance => _instance ?? (_instance = MessageFactory.GetMessage<WorldGetBlockMessage>());

	public override string GetMsgId()
	{
		return "world.get.block";
	}

	protected override IRequest CSSetData(params object[] args)
	{
		Request request = args[0] as Request;
		SFSObject sFSObject = new SFSObject();
		int futureId = GameEntry.Network.getFutureManager().getFutureId();
		sFSObject.PutInt("_id", futureId);
		GameEntry.Network.getFutureManager().onSendRequest(futureId, GetMsgId());
		if (request.bigMap > 0)
		{
			sFSObject.PutInt("bigMap", request.bigMap);
		}
		else
		{
			sFSObject.PutInt("bigMap", 0);
		}
		sFSObject.PutInt("x", request.x);
		sFSObject.PutInt("y", request.y);
		sFSObject.PutInt("serverId", request.serverId);
		sFSObject.PutInt("worldId", request.worldId);
		sFSObject.PutInt("type", request.type);
		sFSObject.PutInt("viewLvl", request.lod);
		sFSObject.PutInt("timeStamp", (int)GameEntry.Timer.GetServerTime());
		sFSObject.PutIntArray("index", request.index);
		sFSObject.PutInt("blockSize", request.blockSize);
		if (request.firstTime)
		{
			sFSObject.PutInt("clearUuidSet", 1);
		}
		if (request.battleFieldFirst)
		{
			sFSObject.PutBool("force", val: true);
		}
		sFSObject.PutInt("leftBottom", request.leftBottom);
		sFSObject.PutInt("rightTop", request.rightTop);
		return new ExtensionRequest(GetMsgId(), sFSObject);
	}

	private string ArrayToStr(int[] arr)
	{
		StringBuilder stringBuilder = new StringBuilder();
		for (int i = 0; i < arr.Length; i++)
		{
			stringBuilder.Append(arr[i]);
			stringBuilder.Append(",");
		}
		return stringBuilder.ToString();
	}

	protected override void CSHandleResponse(ISFSObject message)
	{
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
