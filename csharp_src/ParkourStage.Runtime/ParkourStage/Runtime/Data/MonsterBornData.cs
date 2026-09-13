using UnityEngine;

namespace ParkourStage.Runtime.Data;

public class MonsterBornData
{
	public string MonsterBornId = string.Empty;

	public Vector3 Coord;

	public MonsterBornType BornType = MonsterBornType.SingleMonster;

	public float GenZ;

	public int GenNumber = 1;

	public int GenDelta = 100;

	public float R1;

	public float R2;

	public string MonsterParams = string.Empty;

	public string BuffParams = string.Empty;

	public string Desc = string.Empty;

	public ViewRuleType ViewType;

	public float ViewOffset;

	public MoveRuleType MoveType;

	public Vector2 MoveCenterOffset = Vector2.zero;

	public float MoveEllipseLikeAScaleFactor = 1.5f;

	public string MoveExtraParam = string.Empty;

	public int MoveDirectionFactor = 1;

	public string ShowedBuffPath = string.Empty;

	public string ParaParams = string.Empty;

	public MonsterData ShowedMonsterData;
}
