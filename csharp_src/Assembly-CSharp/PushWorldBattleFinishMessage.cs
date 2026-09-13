using Sfs2X.Entities.Data;

public class PushWorldBattleFinishMessage : BaseMessage
{
	private static PushWorldBattleFinishMessage _instance;

	public static PushWorldBattleFinishMessage Instance => _instance ?? (_instance = MessageFactory.GetMessage<PushWorldBattleFinishMessage>());

	public override string GetMsgId()
	{
		return "push.battle.finish";
	}

	protected override void CSHandleResponse(ISFSObject message)
	{
		if (SceneManager.World != null)
		{
			SceneManager.World.BattleFinish(message);
		}
	}
}
