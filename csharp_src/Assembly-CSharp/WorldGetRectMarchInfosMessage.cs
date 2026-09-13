using Sfs2X.Entities.Data;
using Sfs2X.Requests;

public class WorldGetRectMarchInfosMessage : BaseMessage
{
	public class Request
	{
		public int x;

		public int y;
	}

	private int m_type;

	private static WorldGetRectMarchInfosMessage _instance;

	public static WorldGetRectMarchInfosMessage Instance => _instance ?? (_instance = MessageFactory.GetMessage<WorldGetRectMarchInfosMessage>());

	public override string GetMsgId()
	{
		return "world.get.march.infos";
	}

	protected override IRequest CSSetData(params object[] args)
	{
		ISFSObject iSFSObject = new SFSObject();
		Request request = args[0] as Request;
		iSFSObject.PutInt("x", request.x);
		iSFSObject.PutInt("y", request.y);
		iSFSObject.PutBool("needCross", val: true);
		int futureId = GameEntry.Network.getFutureManager().getFutureId();
		iSFSObject.PutInt("_id", futureId);
		GameEntry.Network.getFutureManager().onSendRequest(futureId, GetMsgId());
		return new ExtensionRequest(GetMsgId(), iSFSObject);
	}

	protected override void CSHandleResponse(ISFSObject message)
	{
		if (!message.ContainsKey("errorCode"))
		{
			SceneManager.MarchDataMgr.HandleWorldGetRectMarchInfos(message);
		}
	}

	protected override bool showErrorCode()
	{
		return true;
	}
}
