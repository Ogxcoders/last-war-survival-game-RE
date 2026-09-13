using Sfs2X.Entities.Data;
using Sfs2X.Requests;
using XLua;

[Hotfix(HotfixFlag.Stateless)]
public class FSTaskCommand : BaseMessage
{
	public class Request
	{
		public int value;
	}

	private static FSTaskCommand _instance;

	public static FSTaskCommand Instance => _instance ?? (_instance = MessageFactory.GetMessage<FSTaskCommand>());

	public override string GetMsgId()
	{
		return "praise.receive";
	}

	protected override IRequest CSSetData(params object[] args)
	{
		Request request = args[0] as Request;
		SFSObject sFSObject = new SFSObject();
		int futureId = GameEntry.Network.getFutureManager().getFutureId();
		sFSObject.PutInt("_id", futureId);
		GameEntry.Network.getFutureManager().onSendRequest(futureId, GetMsgId());
		sFSObject.PutInt("platforom", request.value);
		return new ExtensionRequest(GetMsgId(), sFSObject);
	}

	protected override void CSHandleResponse(ISFSObject message)
	{
	}
}
