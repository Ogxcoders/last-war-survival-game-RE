using Sfs2X.Entities.Data;

public class WorldMarchTrainListSendMessage : BaseMessage
{
	private static WorldMarchTrainListSendMessage _instance;

	public static WorldMarchTrainListSendMessage Instance => _instance ?? (_instance = MessageFactory.GetMessage<WorldMarchTrainListSendMessage>());

	public override string GetMsgId()
	{
		return "train.batch.send";
	}

	protected override void CSHandleResponse(ISFSObject message)
	{
		ISFSArray iSFSArray = message.TryGetArray("trainInfoList");
		if (iSFSArray != null)
		{
			foreach (object item in iSFSArray)
			{
				if (item is ISFSObject obj)
				{
					ISFSObject iSFSObject = obj.TryGetObj("march");
					if (iSFSObject != null)
					{
						SceneManager.MarchDataMgr.HandleFormationMarch(iSFSObject);
					}
				}
			}
		}
		GameEntry.Lua.Call("CSharpCallLuaInterface.HandleDepartureTrainList", ((SFSObject)message).ToLuaTable(GameEntry.Lua.Env));
	}

	protected override bool showErrorCode()
	{
		return false;
	}
}
