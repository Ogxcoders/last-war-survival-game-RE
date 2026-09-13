using Sfs2X.Entities.Data;

public class PushSandWormUpdateMessage : BaseMessage
{
	private static PushSandWormUpdateMessage _instance;

	public static PushSandWormUpdateMessage Instance => _instance ?? (_instance = MessageFactory.GetMessage<PushSandWormUpdateMessage>());

	public override string GetMsgId()
	{
		return "push.sandworm.update";
	}

	protected override void CSHandleResponse(ISFSObject msg)
	{
		bool flag = msg.TryGetBool("isCreate");
		if (flag && msg.TryGetString("uid") == GameEntry.Data.Player.Uid)
		{
			ISFSObject obj = msg.TryGetObj("sandworm");
			int num = obj.TryGetInt("monsterId");
			GameEntry.Lua.Call("CSharpCallLuaInterface.ShowMyBaseWormAppearTip", num);
			long param = obj.TryGetLong("StateEndTime");
			GameEntry.Lua.Call("CSharpCallLuaInterface.SetMyBaseWormWrap", param, num);
		}
		long uuid = msg.TryGetLong("uuid");
		if (SceneManager.World == null)
		{
			return;
		}
		PointInfo pointInfoByUuid = SceneManager.World.GetPointInfoByUuid(uuid);
		if (pointInfoByUuid != null && pointInfoByUuid is BuildPointInfo buildPointInfo)
		{
			ISFSObject msg2 = msg.TryGetObj("sandworm");
			if (buildPointInfo.sandWorm == null)
			{
				buildPointInfo.sandWorm = new SandWormData();
			}
			buildPointInfo.sandWorm.SetData(msg2, buildPointInfo.ownerUid, buildPointInfo.playerName, buildPointInfo.alAbbr, buildPointInfo.srcServerId);
			if (SceneManager.World is WorldScene worldScene)
			{
				worldScene.HandleSandWormUpdate(buildPointInfo, flag ? SandWormAnim.Appear : SandWormAnim.Idle);
			}
		}
	}
}
