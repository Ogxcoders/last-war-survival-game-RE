using Sfs2X.Entities.Data;
using Sfs2X.Requests;

public class WorldMarchFormationChangeMessage : BaseMessage
{
	public class Request
	{
		public long uuid;

		public int target;

		public long targetUid;

		public string path;

		public int worldId;

		public bool autoBackHome = true;

		public int targetServerId;
	}

	private int m_type;

	private static WorldMarchFormationChangeMessage _instance;

	public static WorldMarchFormationChangeMessage Instance => _instance ?? (_instance = MessageFactory.GetMessage<WorldMarchFormationChangeMessage>());

	public override string GetMsgId()
	{
		return "world.march.change";
	}

	protected override IRequest CSSetData(params object[] args)
	{
		Request request = args[0] as Request;
		ISFSObject iSFSObject = new SFSObject();
		int futureId = GameEntry.Network.getFutureManager().getFutureId();
		iSFSObject.PutInt("_id", futureId);
		GameEntry.Network.getFutureManager().onSendRequest(futureId, GetMsgId());
		iSFSObject.PutLong("uuid", request.uuid);
		iSFSObject.PutInt("target", request.target);
		iSFSObject.PutLong("targetUid", request.targetUid);
		if (!request.path.IsNullOrEmpty())
		{
			iSFSObject.PutUtfString("path", request.path);
		}
		iSFSObject.PutInt("worldId", request.worldId);
		iSFSObject.PutBool("autoBackHome", request.autoBackHome);
		if (request.targetServerId > 0)
		{
			iSFSObject.PutInt("targetServer", request.targetServerId);
		}
		return new ExtensionRequest(GetMsgId(), iSFSObject);
	}

	protected override void CSHandleResponse(ISFSObject message)
	{
		if (!message.ContainsKey("errorCode"))
		{
			SceneManager.MarchDataMgr.HandleFormationMarchChange(message);
		}
	}

	protected override bool showErrorCode()
	{
		return true;
	}
}
