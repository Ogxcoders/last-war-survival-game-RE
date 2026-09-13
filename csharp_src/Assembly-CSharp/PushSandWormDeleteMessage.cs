using Sfs2X.Entities.Data;
using UnityEngine;

public class PushSandWormDeleteMessage : BaseMessage
{
	private static PushSandWormDeleteMessage _instance;

	public static PushSandWormDeleteMessage Instance => _instance ?? (_instance = MessageFactory.GetMessage<PushSandWormDeleteMessage>());

	public override string GetMsgId()
	{
		return "push.sandworm.delete";
	}

	protected override void CSHandleResponse(ISFSObject msg)
	{
		long num = msg.TryGetLong("uuid");
		if (GameEntry.Data.Player.GetMainUuid() == num)
		{
			GameEntry.Lua.Call("CSharpCallLuaInterface.SetMyBaseWormWrap", 0);
		}
		if (SceneManager.World == null)
		{
			return;
		}
		int serverId = GameEntry.Data.Player.GetCurServerId();
		PointInfo pointInfoByUuid = SceneManager.World.GetPointInfoByUuid(num);
		if (pointInfoByUuid != null && pointInfoByUuid is BuildPointInfo buildPointInfo)
		{
			serverId = pointInfoByUuid.serverId;
			if (buildPointInfo.sandWorm != null)
			{
				buildPointInfo.sandWorm.Dispose();
				buildPointInfo.sandWorm = null;
				if (SceneManager.World is WorldScene worldScene)
				{
					worldScene.HandleSandWormUpdate(buildPointInfo, SandWormAnim.Die);
				}
			}
		}
		if (!(SceneManager.World is WorldScene worldScene2))
		{
			return;
		}
		int num2 = msg.TryGetInt("monsterId");
		if (num2 > 0)
		{
			int index = msg.TryGetInt("pointId");
			string templateData = GameEntry.ConfigCache.GetTemplateData("lw_world_monster", num2, "model_name");
			Vector3 pos = TileCoord.TileIndexToWorld(index, ForceChangeScene.World, serverId);
			worldScene2.CreateVFX(templateData, pos, 1.7f, 0f, delegate(GameObject go)
			{
				go.GetComponent<SandWormMono>().PlayAnim(SandWormAnim.Die);
			});
		}
	}
}
