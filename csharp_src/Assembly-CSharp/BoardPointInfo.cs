using Protobuf;

public class BoardPointInfo : PointInfo
{
	public new long uuid;

	public BoardState state;

	public string allianceId;

	public int curHp;

	public int inside;

	public BoardPointInfo()
	{
	}

	public BoardPointInfo(WorldPointInfo pi)
		: base(pi)
	{
		RoadInfo roadInfo = pi.RoadInfo;
		uuid = roadInfo.Uuid;
		ownerUid = roadInfo.OwnerUid;
		allianceId = roadInfo.AllianceId;
		curHp = roadInfo.CurrentHp;
		state = (BoardState)roadInfo.RoadState;
		inside = roadInfo.Inside;
	}

	public override PointInfo Clone()
	{
		BoardPointInfo boardPointInfo = new BoardPointInfo();
		BaseClone(boardPointInfo);
		boardPointInfo.uuid = uuid;
		boardPointInfo.state = state;
		boardPointInfo.allianceId = allianceId;
		boardPointInfo.ownerUid = ownerUid;
		boardPointInfo.curHp = curHp;
		boardPointInfo.inside = inside;
		return boardPointInfo;
	}

	public override PlayerType GetPlayerType()
	{
		if (GameEntry.Data.Player.GetUid() == ownerUid)
		{
			return PlayerType.PlayerSelf;
		}
		string text = GameEntry.Data.Player.GetAllianceId();
		if (string.IsNullOrEmpty(text))
		{
			return PlayerType.PlayerOther;
		}
		if (text == allianceId)
		{
			string text2 = GameEntry.Lua.CallWithReturn<string>("CSharpCallLuaInterface.GetAllianceLeaderUid");
			if (ownerUid != text2)
			{
				return PlayerType.PlayerAlliance;
			}
			return PlayerType.PlayerAllianceLeader;
		}
		return PlayerType.PlayerOther;
	}
}
