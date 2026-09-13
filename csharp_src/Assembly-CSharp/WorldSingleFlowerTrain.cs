using System.Collections.Generic;
using Sfs2X.Entities.Data;

public class WorldSingleFlowerTrain
{
	public string country;

	public int curLv;

	public int curExp;

	public long upLvTime;

	public int headPicVer;

	public long uuid;

	public long sendTime;

	public int allianceId;

	public string uid;

	public int chatBubbleId;

	public string name;

	public int connectNum;

	public int charBubbleET;

	public long endTime;

	public string abbr;

	public int cfgId;

	public long marchUuid;

	public long arriveTime;

	public int cheerCount;

	public int likeCount;

	public List<string> allCheerPlayerList;

	public int serverId;

	public WorldSingleFlowerTrain(long marchUuid)
	{
		this.marchUuid = marchUuid;
	}

	public void ParseData(ISFSObject msg)
	{
		country = msg.TryGetString("country");
		curLv = msg.TryGetInt("curLv");
		curExp = msg.TryGetInt("curExp");
		upLvTime = msg.TryGetLong("upLvTime");
		headPicVer = msg.TryGetInt("headPicVer");
		uuid = msg.TryGetLong("uuid");
		sendTime = msg.TryGetLong("sendTime");
		allianceId = msg.TryGetInt("allianceId");
		uid = msg.TryGetString("uid");
		chatBubbleId = msg.TryGetInt("chatBubbleId");
		name = msg.TryGetString("name");
		connectNum = msg.TryGetInt("connectNum");
		charBubbleET = msg.TryGetInt("charBubbleET");
		endTime = msg.TryGetLong("endTime");
		abbr = msg.TryGetString("abbr");
		cfgId = msg.TryGetInt("cfgId");
		arriveTime = msg.TryGetLong("arriveTime");
		cheerCount = msg.TryGetInt("cheer");
		likeCount = msg.TryGetInt("praise");
		serverId = msg.TryGetInt("serverId");
		ISFSObject iSFSObject = msg.TryGetObj("cheerPlayer");
		if (iSFSObject != null)
		{
			string[] keys = iSFSObject.GetKeys();
			if (allCheerPlayerList == null)
			{
				allCheerPlayerList = new List<string>();
			}
			string[] array = keys;
			foreach (string text in array)
			{
				string text2 = iSFSObject.TryGetString(text);
				allCheerPlayerList.Add(text + ";" + text2.ToString());
			}
		}
	}
}
