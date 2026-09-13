using Sfs2X.Entities.Data;

public class PushWorldMarchRetrunMessage : BaseMessage
{
	private static PushWorldMarchRetrunMessage _instance;

	public static PushWorldMarchRetrunMessage Instance => _instance ?? (_instance = MessageFactory.GetMessage<PushWorldMarchRetrunMessage>());

	public override string GetMsgId()
	{
		return "push.world.march.return.new";
	}

	protected override void CSHandleResponse(ISFSObject message)
	{
		WorldMarch worldMarch = new WorldMarch();
		worldMarch.UpdateWorldMarch(message);
		LuaBuildData buildingDataByBuildId = GameEntry.Data.Building.GetBuildingDataByBuildId(10100000);
		if (buildingDataByBuildId != null && SceneManager.World != null)
		{
			SceneManager.World.StartMarch(3, buildingDataByBuildId.pointId, 0L, -1, worldMarch.uuid, 0L);
		}
	}
}
