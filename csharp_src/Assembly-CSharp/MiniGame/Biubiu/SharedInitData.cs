using System.Collections.Generic;
using Leopotam.EcsLite;

namespace MiniGame.Biubiu;

public struct SharedInitData
{
	public bool ReEnter;

	public int ServerId;

	public string LevelPath;

	public EPlayerID PlayerID;

	public List<InitPlayerInfo> PlayerInfos;

	public EcsPackedEntity UIAdaptEntity;
}
