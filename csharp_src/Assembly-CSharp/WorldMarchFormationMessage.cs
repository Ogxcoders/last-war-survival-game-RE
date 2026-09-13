using Sfs2X.Entities.Data;
using Sfs2X.Requests;
using XLua;

[Hotfix(HotfixFlag.Stateless)]
public class WorldMarchFormationMessage : BaseMessage
{
	public class Request
	{
		public long formationUuid;

		public int target;

		public long targetUid;

		public string path;

		public int worldId;

		public int waitTimeIndex;

		public bool autoBackHome = true;

		public SFSObject formationParam;

		public int targetServerId;
	}

	private int m_type;

	private static WorldMarchFormationMessage _instance;

	public static WorldMarchFormationMessage Instance => _instance ?? (_instance = MessageFactory.GetMessage<WorldMarchFormationMessage>());

	public override string GetMsgId()
	{
		return "world.march.formation.new";
	}

	protected override IRequest CSSetData(params object[] args)
	{
		Request request = args[0] as Request;
		ISFSObject iSFSObject = new SFSObject();
		int futureId = GameEntry.Network.getFutureManager().getFutureId();
		iSFSObject.PutInt("_id", futureId);
		GameEntry.Network.getFutureManager().onSendRequest(futureId, GetMsgId());
		if (request.formationUuid != 0L)
		{
			iSFSObject.PutLong("formationUuid", request.formationUuid);
		}
		iSFSObject.PutInt("target", request.target);
		iSFSObject.PutLong("targetUid", request.targetUid);
		if (!request.path.IsNullOrEmpty())
		{
			iSFSObject.PutUtfString("path", request.path);
		}
		iSFSObject.PutInt("worldId", request.worldId);
		iSFSObject.PutInt("waitTimeIndex", request.waitTimeIndex);
		iSFSObject.PutBool("autoBackHome", request.autoBackHome);
		if (request.formationParam != null)
		{
			iSFSObject.PutSFSObject("formationParam", request.formationParam);
		}
		if (request.targetServerId > 0)
		{
			iSFSObject.PutInt("targetServer", request.targetServerId);
		}
		return new ExtensionRequest(GetMsgId(), iSFSObject);
	}

	protected override void CSHandleResponse(ISFSObject message)
	{
		if (message.ContainsKey("errorCode"))
		{
			GameEntry.Lua.Call("CSharpCallLuaInterface.MarchErrorLog", ((SFSObject)message).ToLuaTable(GameEntry.Lua.Env));
		}
		else
		{
			SceneManager.MarchDataMgr.HandleFormationMarch(message);
		}
	}

	protected override bool showErrorCode()
	{
		return false;
	}
}
