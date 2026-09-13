using GameFramework;
using Sfs2X.Entities.Data;
using Sfs2X.Requests;
using XLua;

[Hotfix(HotfixFlag.Stateless)]
public class CrossWorldMoveMessage : BaseMessage
{
	public class Request
	{
		public int Point;

		public string ItemUuid;

		public int ServerId;

		public int Type;

		public string TicketUuid;

		public int SubType = -1;

		public string MailId;

		public string Army;

		public string AssemblePoint;
	}

	private static CrossWorldMoveMessage _instance;

	private int _serverId;

	public static CrossWorldMoveMessage Instance => _instance ?? (_instance = MessageFactory.GetMessage<CrossWorldMoveMessage>());

	public override string GetMsgId()
	{
		return "cross.world.mv";
	}

	protected override IRequest CSSetData(params object[] args)
	{
		Request request = args[0] as Request;
		ISFSObject iSFSObject = new SFSObject();
		int futureId = GameEntry.Network.getFutureManager().getFutureId();
		iSFSObject.PutInt("_id", futureId);
		GameEntry.Network.getFutureManager().onSendRequest(futureId, GetMsgId());
		if (request != null)
		{
			_serverId = request.ServerId;
			iSFSObject.PutInt("point", request.Point);
			iSFSObject.PutUtfString("itemUUid", request.ItemUuid);
			iSFSObject.PutInt("serverId", request.ServerId);
			iSFSObject.PutInt("type", request.Type);
			iSFSObject.PutUtfString("ticketUUid", request.TicketUuid);
			if (request.SubType != -1)
			{
				iSFSObject.PutInt("subType", request.SubType);
			}
			if (!request.MailId.IsNullOrEmpty())
			{
				iSFSObject.PutUtfString("mailId", request.MailId);
			}
			if (!request.Army.IsNullOrEmpty())
			{
				iSFSObject.PutUtfString("army", request.Army);
			}
			if (!request.AssemblePoint.IsNullOrEmpty())
			{
				iSFSObject.PutUtfString("assemblePoint", request.AssemblePoint);
			}
		}
		return new ExtensionRequest(GetMsgId(), iSFSObject);
	}

	protected override void CSHandleResponse(ISFSObject message)
	{
		UIUtils.HideLoadingMask();
		if (message.ContainsKey("errorCode"))
		{
			UIUtils.ShowTips(message.TryGetString("errorCode"), 3f);
			return;
		}
		if (message.ContainsKey("assemblePoint"))
		{
			message.TryGetString("assemblePoint");
		}
		ISFSObject obj = message.TryGetObj("serverInfo");
		string ip = obj.TryGetString("ip");
		string text = obj.TryGetString("zone");
		text.Substring(3);
		int port = obj.TryGetInt("port");
		string text2 = message.TryGetString("uid");
		Log.Info("[AT]SetGUID_CrossWMMsg:" + text2);
		ApplicationLaunch.Instance.Loading.SaveGameServerSetting(ip, port, text, text2, "", 0);
		ApplicationLaunch.Instance.ReloadGame();
	}
}
