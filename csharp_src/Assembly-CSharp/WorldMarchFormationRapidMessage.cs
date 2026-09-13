using Sfs2X.Entities.Data;

public class WorldMarchFormationRapidMessage : BaseMessage
{
	private static WorldMarchFormationRapidMessage _instance;

	public static WorldMarchFormationRapidMessage Instance => _instance ?? (_instance = MessageFactory.GetMessage<WorldMarchFormationRapidMessage>());

	public override string GetMsgId()
	{
		return "world.march.speed.up";
	}

	protected override void CSHandleResponse(ISFSObject message)
	{
		if (!message.ContainsKey("errorCode"))
		{
			SceneManager.MarchDataMgr.HandleFormationMarchChange(message);
		}
	}

	protected override bool showErrorCode()
	{
		return true;
	}
}
