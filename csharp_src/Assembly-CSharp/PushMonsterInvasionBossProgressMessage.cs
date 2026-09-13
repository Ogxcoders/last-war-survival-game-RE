using Sfs2X.Entities.Data;

public class PushMonsterInvasionBossProgressMessage : BaseMessage
{
	private static PushMonsterInvasionBossProgressMessage _instance;

	public static PushMonsterInvasionBossProgressMessage Instance => _instance ?? (_instance = MessageFactory.GetMessage<PushMonsterInvasionBossProgressMessage>());

	public override string GetMsgId()
	{
		return "push.monster.invasion.boss.progress";
	}

	protected override void CSHandleResponse(ISFSObject message)
	{
		if (SceneManager.World != null)
		{
			long marchUuid = message.GetLong("uuid");
			SceneManager.World.GetTroop(marchUuid)?.MarkInvasionMonsterDialogFlag(3u);
			GameEntryProxy.Event.Fire(EventId.OnMonsterInvasionBossProgressChanged, message.GetInt("progress"));
		}
	}
}
