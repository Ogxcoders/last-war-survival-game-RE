using Sfs2X.Entities.Data;

public class PushWorldKillSkinMessage : BaseMessage
{
	private static PushWorldTriggerUpdateMessage _instance;

	public static PushWorldTriggerUpdateMessage Instance => _instance ?? (_instance = MessageFactory.GetMessage<PushWorldTriggerUpdateMessage>());

	public override string GetMsgId()
	{
		return "push.world.kill.skin";
	}

	protected override void CSHandleResponse(ISFSObject message)
	{
		if (SceneManager.World != null && SceneManager.IsInWorld())
		{
			SceneManager.World.HandlePushMultiKillUpdate(message);
		}
	}

	protected override bool showErrorCode()
	{
		return true;
	}
}
