using Sfs2X.Entities.Data;
using Sfs2X.Requests;

public class WorldCrossServerMessage : BaseMessage
{
	public class RequestParam
	{
		public int x;

		public int y;

		public int type;

		public int forceServerId;

		public int worldId;

		public int leftBottom;

		public int rightTop;

		public int lod;
	}

	public int mServerId;

	private RequestParam mParam;

	private static WorldCrossServerMessage _instance;

	public static WorldCrossServerMessage Instance => _instance ?? (_instance = MessageFactory.GetMessage<WorldCrossServerMessage>());

	public override string GetMsgId()
	{
		return "world.get.cross";
	}

	protected override IRequest CSSetData(params object[] args)
	{
		RequestParam requestParam = mParam;
		ISFSObject iSFSObject = new SFSObject();
		int futureId = GameEntry.Network.getFutureManager().getFutureId();
		iSFSObject.PutInt("_id", futureId);
		GameEntry.Network.getFutureManager().onSendRequest(futureId, GetMsgId());
		iSFSObject.PutInt("x", requestParam.x);
		iSFSObject.PutInt("y", requestParam.y);
		iSFSObject.PutLong("timeStamp", GameEntry.Timer.GetServerTime());
		iSFSObject.PutInt("type", requestParam.type);
		iSFSObject.PutInt("worldId", requestParam.worldId);
		int val = GameEntry.Data.Player.GetCurServerId();
		if (requestParam.forceServerId != -1)
		{
			val = requestParam.forceServerId;
		}
		mServerId = val;
		iSFSObject.PutInt("serverId", val);
		iSFSObject.PutInt("leftBottom", requestParam.leftBottom);
		iSFSObject.PutInt("rightTop", requestParam.rightTop);
		iSFSObject.PutInt("viewLvl", requestParam.lod);
		return new ExtensionRequest(GetMsgId(), iSFSObject);
	}

	public void SendReqest(RequestParam param)
	{
		if (!string.IsNullOrEmpty(GameEntry.Data.Player.Uid) && GameEntry.Data.Player.GetSelfServerId() != GameEntry.Data.Player.GetCurServerId())
		{
			mParam = param;
			GameEntry.NetworkCross.Send(this);
			GameEntry.NetworkCross.ClearSpecialCommand();
			GameEntry.NetworkCross.AddSpecialCommand(this);
		}
	}

	public override void Send(params object[] args)
	{
		IRequest request = CSSetData(args);
		GameEntry.NetworkCross.Send(request);
	}

	protected override void CSHandleResponse(ISFSObject message)
	{
	}
}
