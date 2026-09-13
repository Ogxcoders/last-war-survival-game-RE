using Sfs2X.Entities.Data;
using Sfs2X.Requests;

public class LoginCrossServerMessage : BaseMessage
{
	private static LoginCrossServerMessage _instance;

	public string mUserName = "";

	public static LoginCrossServerMessage Instance => _instance ?? (_instance = MessageFactory.GetMessage<LoginCrossServerMessage>());

	public override string GetMsgId()
	{
		return "login";
	}

	public void SendRequest()
	{
		if (!GameEntry.NetworkCross.Logined)
		{
			GameEntry.NetworkCross.Send(this);
		}
	}

	public override void Send(params object[] args)
	{
		if (!GameEntry.NetworkCross.Logined)
		{
			IRequest request = CSSetData(args);
			GameEntry.NetworkCross.Send(request);
		}
	}

	protected override IRequest CSSetData(params object[] args)
	{
		ISFSObject iSFSObject = new SFSObject();
		int futureId = GameEntry.Network.getFutureManager().getFutureId();
		iSFSObject.PutInt("_id", futureId);
		GameEntry.Network.getFutureManager().onSendRequest(futureId, GetMsgId());
		int selfServerId = GameEntry.Data.Player.GetSelfServerId();
		mUserName = GameEntry.Data.Player.Uid;
		string text = selfServerId + "_" + mUserName;
		iSFSObject.PutUtfString("uid", text);
		int crossServerId = GameEntry.Data.Player.GetCrossServerId();
		iSFSObject.PutInt("sid", crossServerId);
		iSFSObject.PutInt("fsid", selfServerId);
		iSFSObject.PutInt("loginType", 1);
		bool flag = GameEntry.Data.Player.IsBattleFieldWatching;
		if (GameEntry.Data.Player.GetWorldType() == 0 || GameEntry.Data.Player.GetWorldId() == 0)
		{
			bool flag2 = (GameEntry.Data.Player.IsBattleFieldWatching = false);
			flag = flag2;
		}
		iSFSObject.PutInt("dragonWatchState", flag ? 1 : 0);
		iSFSObject.PutUtfString("appVersion", GameEntry.Sdk.Version);
		iSFSObject.PutUtfString("versionCode", GameEntry.Sdk.VersionCode);
		string zoneName = "APS" + crossServerId;
		return new LoginRequest(text, "", zoneName, iSFSObject);
	}

	protected override void CSHandleResponse(ISFSObject message)
	{
		if (message.ContainsKey("errorMessage"))
		{
			string utfString = message.GetUtfString("errorMessage");
			GameEntry.NetworkCross.RemoveConnect();
			switch (utfString)
			{
			case "E001":
				return;
			case "E002":
				return;
			case "E003":
				return;
			case "E004":
				return;
			case "120870":
				return;
			}
		}
		bool flag = false;
		if (message.ContainsKey("loginStatus") && message.TryGetString("loginStatus") == "0")
		{
			flag = true;
		}
		if (flag)
		{
			GameEntry.NetworkCross.Logined = true;
			return;
		}
		GameEntry.NetworkCross.RemoveConnect();
		GameEntry.NetworkCross.ClearSpecialCommand();
	}
}
