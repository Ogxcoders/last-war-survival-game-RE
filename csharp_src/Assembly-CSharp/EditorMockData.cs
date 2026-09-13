using System.Collections.Generic;
using Google.Protobuf.Collections;
using Protobuf;
using Sfs2X.Entities.Data;

public static class EditorMockData
{
	private static long _uuid = 10000L;

	public static long GenUuid()
	{
		return ++_uuid;
	}

	public static ISFSObject MockDataInit()
	{
		SFSObject sFSObject = new SFSObject();
		sFSObject.PutUtfString("uid", "7211242831000100");
		sFSObject.PutInt("serverId", 100);
		sFSObject.PutInt("serverType", 0);
		sFSObject.PutInt("serverMax", 10000);
		SFSObject sFSObject2 = new SFSObject();
		sFSObject2.PutSFSObject("user", sFSObject);
		return sFSObject2;
	}

	public static ISFSObject MockWorldPointRemove(IEnumerable<int> points)
	{
		SFSObject sFSObject = new SFSObject();
		SFSArray sFSArray = new SFSArray();
		foreach (int point in points)
		{
			sFSArray.AddInt(point);
		}
		sFSObject.PutSFSArray("pointIds", sFSArray);
		return sFSObject;
	}

	public static void InitPlayerInfo()
	{
		ISFSObject obj = MockDataInit();
		GameEntry.Data.Player.CSInit(obj);
		GameEntry.Lua.SafeDoString("local PlayerInfo = require \"DataCenter.Global.PlayerInfo\"\nLuaEntry.Player = PlayerInfo.New()");
		GameEntry.Lua.SafeDoString($"LuaEntry.Player.serverMax = {GameEntry.GlobalData.serverMax}");
		GameEntry.Lua.SafeDoString($"LuaEntry.Player.serverId = {GameEntry.Data.Player.GetSelfServerId()}");
		GameEntry.Lua.SafeDoString($"LuaEntry.Player.serverType = {GameEntry.GlobalData.serverType}");
	}

	public static BuildPointInfo CreateBuildPointInfo(int index, int skinId = 0, int effectId = 0, int colourId = 0, int titleNameSkinId = 0)
	{
		BuildPointInfo buildPointInfo = new BuildPointInfo();
		buildPointInfo.uuid = GenUuid();
		buildPointInfo.pointIndex = index;
		buildPointInfo.mainIndex = index;
		buildPointInfo.pointType = WorldPointType.PlayerBuilding;
		buildPointInfo.tileSize = 1;
		buildPointInfo.serverId = 100;
		buildPointInfo.srcServerId = 100;
		buildPointInfo.worldId = 0;
		buildPointInfo.itemId = 10100000;
		buildPointInfo.level = 30;
		buildPointInfo.state = 0;
		buildPointInfo.allianceId = "";
		buildPointInfo.startTime = 1720434682;
		buildPointInfo.endTime = 1724480137;
		buildPointInfo.inside = 0;
		buildPointInfo.ownerUid = "7388286311000100";
		buildPointInfo.roadDir = GameDefines.BuildConnectRoadDirection.None;
		buildPointInfo.curHp = 9000;
		buildPointInfo.recoverSpeed = 0.378f;
		buildPointInfo.fireSpeed = 0.74f;
		buildPointInfo.lastHpTime = 1738757216;
		buildPointInfo.protectEndTime = 0;
		buildPointInfo.appearanceId = 1;
		buildPointInfo.playerName = "Mock" + buildPointInfo.uuid;
		buildPointInfo.alAbbr = "TT";
		buildPointInfo.lastCollectTime = 0L;
		buildPointInfo.unavailableTime = 1732879045000L;
		buildPointInfo.queueItemId = 0;
		buildPointInfo.queueStartTime = 0L;
		buildPointInfo.queueUpdateTime = 0L;
		buildPointInfo.destroyStartTime = 0L;
		buildPointInfo.destroyEndTime = 0L;
		buildPointInfo.RefreshRoadDir();
		buildPointInfo.skinId = skinId;
		buildPointInfo.colourId = colourId;
		buildPointInfo.colourTime = float.MaxValue;
		buildPointInfo.titleNameSkinId = titleNameSkinId;
		buildPointInfo.effectId = effectId;
		buildPointInfo.positionId = "";
		buildPointInfo.specialType = Protobuf.SpecialType.None;
		buildPointInfo.virusLayer = 0;
		buildPointInfo.virusEndTime = 0L;
		buildPointInfo.refuseTreadVirus = false;
		buildPointInfo.seasonRole = 0;
		buildPointInfo.status = new RepeatedField<Status>();
		buildPointInfo.countryFlag = "HK";
		buildPointInfo.aosType = AllianceOfficialSkillType.None;
		buildPointInfo.mummyConvertId = 0;
		buildPointInfo.mummyConvertCount = 0;
		buildPointInfo.crystal = 0;
		buildPointInfo.nucleus = 0;
		return buildPointInfo;
	}
}
