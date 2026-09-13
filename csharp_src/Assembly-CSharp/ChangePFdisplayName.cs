using Sfs2X.Entities.Data;
using Sfs2X.Requests;

public class ChangePFdisplayName : BaseMessage
{
	private static ChangePFdisplayName _instance;

	public static ChangePFdisplayName Instance => _instance ?? (_instance = MessageFactory.GetMessage<ChangePFdisplayName>());

	public override string GetMsgId()
	{
		return "user.modify.nickName.google";
	}

	protected override IRequest CSSetData(params object[] args)
	{
		SFSObject sFSObject = new SFSObject();
		int futureId = GameEntry.Network.getFutureManager().getFutureId();
		sFSObject.PutInt("_id", futureId);
		GameEntry.Network.getFutureManager().onSendRequest(futureId, GetMsgId());
		sFSObject.PutUtfString("nickName", GameEntry.Sdk.pf_displayname);
		return new ExtensionRequest(GetMsgId(), sFSObject);
	}

	protected override void CSHandleResponse(ISFSObject message)
	{
		if (!message.ContainsKey("errorMessage"))
		{
			string text = message.GetText("newName");
			GameEntry.Lua.SetValue("LuaEntry.Player", "nickName", text);
		}
	}
}
