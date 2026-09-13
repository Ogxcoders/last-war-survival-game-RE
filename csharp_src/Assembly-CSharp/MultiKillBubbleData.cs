using System;
using Sfs2X.Entities.Data;

public class MultiKillBubbleData : IDisposable
{
	public int worldId;

	public int pointId;

	public int serverId;

	public long marchUuid;

	public string uid;

	public string name;

	public string pic;

	public int picVer;

	public int headSkinId;

	public long headSkinET;

	public int skinId;

	public int squadNo;

	public int configId;

	public long startTime;

	public int killNum;

	public string abbr;

	public string text;

	public bool isWolf;

	public string allianceId;

	public MultiKillConfig config;

	public void ParseData(ISFSObject msg)
	{
		marchUuid = msg.TryGetLong("marchUuid");
		worldId = msg.TryGetInt("worldId");
		pointId = msg.TryGetInt("pointId");
		serverId = msg.TryGetInt("serverId");
		uid = msg.TryGetString("uid");
		name = msg.TryGetString("name");
		pic = msg.TryGetString("pic");
		picVer = msg.TryGetInt("picVer");
		headSkinId = msg.TryGetInt("headSkinId");
		headSkinET = msg.TryGetLong("headSkinET");
		skinId = msg.TryGetInt("skinId");
		startTime = msg.TryGetLong("startTime");
		killNum = msg.TryGetInt("killNum");
		squadNo = msg.TryGetInt("squadNo");
		configId = msg.TryGetInt("configId");
		text = msg.TryGetString("text");
		isWolf = msg.TryGetBool("isWolf");
		ISFSObject iSFSObject = msg.TryGetObj("allianceInfo");
		if (iSFSObject != null)
		{
			abbr = iSFSObject.TryGetString("allianceAbbr");
			allianceId = iSFSObject.TryGetString("allianceId");
		}
	}

	public void CreateFakeData(int pointId, int playerIndex)
	{
	}

	public void SetConfig(MultiKillConfig config)
	{
		this.config = config;
	}

	public void Dispose()
	{
		abbr = null;
		allianceId = null;
		name = null;
		pic = null;
	}
}
