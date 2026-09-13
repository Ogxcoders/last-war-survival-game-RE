using Sfs2X.Entities.Data;

public class PushBloodQueenGunnerAttackMessage : BaseMessage
{
	private static PushBloodQueenGunnerAttackMessage _instance;

	public static PushBloodQueenGunnerAttackMessage Instance => _instance ?? (_instance = MessageFactory.GetMessage<PushBloodQueenGunnerAttackMessage>());

	public override string GetMsgId()
	{
		return "push.blood.queen.gunner.attack";
	}

	protected override void CSHandleResponse(ISFSObject message)
	{
		if (message.ContainsKey("errorCode"))
		{
			UIUtils.ShowTips(message.TryGetString("errorCode"), 3f);
			return;
		}
		int cityId = message.TryGetInt("cityId");
		long marchUuid = message.TryGetLong("uuid");
		long uuid = message.TryGetLong("cityUuid");
		SceneManager.World.GetTroop(marchUuid)?.OnPushBloodQueenGunnerAttack();
		WorldPointObject objectByUuid = SceneManager.World.GetObjectByUuid(uuid);
		if (objectByUuid != null && objectByUuid is WorldBasePointObject worldBasePointObject)
		{
			worldBasePointObject.ShowBloodQueenGunnerAttackEffect(cityId);
		}
	}
}
