using Sfs2X.Entities.Data;

public class PushWorldBattleUpdateMessage : BaseMessage
{
	private static PushWorldBattleUpdateMessage _instance;

	public static PushWorldBattleUpdateMessage Instance => _instance ?? (_instance = MessageFactory.GetMessage<PushWorldBattleUpdateMessage>());

	public override string GetMsgId()
	{
		return "push.battle.round.info";
	}

	protected override void CSHandleResponse(ISFSObject message)
	{
		if (SceneManager.World != null)
		{
			SceneManager.World.UpdateBattleMessage(message);
		}
	}
}
