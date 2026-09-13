using Sfs2X.Entities.Data;

public class WorldMarchTrainSendMessage : BaseMessage
{
	private static WorldMarchTrainSendMessage _instance;

	public static WorldMarchTrainSendMessage Instance => _instance ?? (_instance = MessageFactory.GetMessage<WorldMarchTrainSendMessage>());

	public override string GetMsgId()
	{
		return "train.send";
	}

	protected override void CSHandleResponse(ISFSObject message)
	{
		if (!message.ContainsKey("errorCode"))
		{
			ISFSObject iSFSObject = message.TryGetObj("march");
			if (iSFSObject != null)
			{
				SceneManager.MarchDataMgr.HandleFormationMarch(iSFSObject);
			}
		}
		GameEntry.Lua.Call("CSharpCallLuaInterface.HandleDepartureTrain", ((SFSObject)message).ToLuaTable(GameEntry.Lua.Env));
	}

	protected override bool showErrorCode()
	{
		return false;
	}
}
