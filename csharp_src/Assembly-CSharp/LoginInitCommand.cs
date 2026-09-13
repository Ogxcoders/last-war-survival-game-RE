using Sfs2X.Entities.Data;
using Sfs2X.Requests;

public class LoginInitCommand : BaseMessage
{
	private static LoginInitCommand _instance;

	public static LoginInitCommand Instance => _instance ?? (_instance = MessageFactory.GetMessage<LoginInitCommand>());

	public override string GetMsgId()
	{
		return "login.init";
	}

	protected override IRequest CSSetData(params object[] args)
	{
		ISFSObject iSFSObject = new SFSObject();
		string text = GameEntry.Lua.CallWithReturn<string>("CSharpCallLuaInterface.GetConfigMd5");
		if (!text.IsNullOrEmpty())
		{
			iSFSObject.PutUtfString("dataConfigMd5", text);
		}
		int futureId = GameEntry.Network.getFutureManager().getFutureId();
		iSFSObject.PutInt("_id", futureId);
		GameEntry.Network.getFutureManager().onSendRequest(futureId, GetMsgId());
		return new ExtensionRequest(GetMsgId(), iSFSObject);
	}

	protected override void CSHandleResponse(ISFSObject message)
	{
		if (message != null && !message.ContainsKey("errorCode"))
		{
			InitMessage.Instance.InitData(message);
			if (SceneManager.World != null)
			{
				SceneManager.World.RefreshView();
			}
			GameEntry.Event.Fire(EventId.CloseDisconnectView);
			GameEntry.Event.Fire(EventId.Guide_video_Play);
		}
	}
}
