using Sfs2X.Entities.Data;
using Sfs2X.Requests;
using XLua;

[Hotfix(HotfixFlag.Stateless)]
public class UserBindGaidMessage : BaseMessage
{
	public class Request
	{
		public string gaid;
	}

	private static UserBindGaidMessage _instance;

	public static UserBindGaidMessage Instance => _instance ?? (_instance = MessageFactory.GetMessage<UserBindGaidMessage>());

	public override string GetMsgId()
	{
		return "bind.gaid";
	}

	protected override IRequest CSSetData(params object[] args)
	{
		Request request = args[0] as Request;
		ISFSObject iSFSObject = new SFSObject();
		int futureId = GameEntry.Network.getFutureManager().getFutureId();
		iSFSObject.PutInt("_id", futureId);
		GameEntry.Network.getFutureManager().onSendRequest(futureId, GetMsgId());
		iSFSObject.PutUtfString("gaid", request.gaid);
		return new ExtensionRequest(GetMsgId(), iSFSObject);
	}

	protected override void CSHandleResponse(ISFSObject message)
	{
	}
}
