using System;
using Sfs2X.Entities.Data;
using UnityEngine;

public class BloodyQueenMonster : IDisposable
{
	public int challengeId;

	public int round;

	public int cityId;

	public int state;

	public long attackStartTime;

	public long attackEndTime;

	public int standPoint;

	public Vector3 standWorldPos;

	public void Update(ISFSObject q)
	{
		challengeId = q.TryGetInt("challengeId");
		round = q.TryGetInt("round");
		cityId = q.TryGetInt("cityId");
		state = q.TryGetInt("state");
		attackStartTime = q.TryGetLong("attackStartTime");
		attackEndTime = q.TryGetLong("attackEndTime");
		standPoint = q.TryGetInt("standPoint");
		standWorldPos = TileCoord.TileIndexToWorld(standPoint, ForceChangeScene.World, 0);
	}

	public void Dispose()
	{
	}
}
