using Sfs2X.Entities.Data;

public class PushWorldTriggerUpdateMessage : BaseMessage
{
	private static PushWorldTriggerUpdateMessage _instance;

	public static PushWorldTriggerUpdateMessage Instance => _instance ?? (_instance = MessageFactory.GetMessage<PushWorldTriggerUpdateMessage>());

	public override string GetMsgId()
	{
		return "push.world.trigger.update";
	}

	protected override void CSHandleResponse(ISFSObject message)
	{
		if (SceneManager.World != null && SceneManager.IsInWorld())
		{
			int serverId = message.GetInt("serverId");
			int worldId = message.GetInt("worldId");
			SceneManager.World.HandlePushWorldTriggerUpdate(message, serverId, worldId);
		}
	}

	protected override bool showErrorCode()
	{
		return true;
	}
}
