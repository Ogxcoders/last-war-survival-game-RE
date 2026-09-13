using System.Collections.Generic;
using Sfs2X.Entities.Data;
using UnityEngine;

public class HSRMarchManager
{
	private static HSRMarchManager instance;

	private long waitTime;

	private float speed;

	private Dictionary<int, Vector3> stationWorldPos = new Dictionary<int, Vector3>();

	private HSRMarch march;

	public static HSRMarchManager GetInstance()
	{
		if (instance == null)
		{
			instance = new HSRMarchManager();
			instance.Init();
		}
		return instance;
	}

	private void Init()
	{
	}

	public void Destroy()
	{
	}

	private void PullHSRMarchData()
	{
		SeasonGetZoneTrainActivityInfoMessage.Instance.Send();
	}

	public void HandleZoneTrainActivityInfoMessage(ISFSObject message)
	{
		if (message.TryGetLong("createTrainTime") <= 0)
		{
			march = null;
			return;
		}
		if (march == null)
		{
			march = new HSRMarch();
		}
		march.Init(message);
	}

	public HSRMarch GetMarch(long marchUuid)
	{
		return march;
	}

	public long GetWaitTime()
	{
		if (waitTime <= 0)
		{
			waitTime = GameEntry.Lua.CallWithReturn<int>("CSharpCallLuaInterface.GetHSRWaitTime");
		}
		return waitTime;
	}

	public float GetSpeed()
	{
		if (speed <= 0f)
		{
			speed = GameEntry.Lua.CallWithReturn<float, string, string>("CSharpCallLuaInterface.GetConfigNum", "server_train", "k11");
		}
		return speed;
	}

	public float GetCarriageLength()
	{
		return 5f;
	}

	public Vector3 GetStationPosition(int serverId)
	{
		if (stationWorldPos.TryGetValue(serverId, out var value))
		{
			return value;
		}
		Vector3 vector = TileCoord.TileIndexToWorld(GameEntry.Lua.CallWithReturn<int, int>("CSharpCallLuaInterface.GetKingCityPointId", serverId), ForceChangeScene.World, serverId);
		stationWorldPos[serverId] = vector;
		return vector;
	}

	public bool IsInView(Rect viewRect, long marchUuid)
	{
		if (march == null)
		{
			return false;
		}
		return march.IsInView(viewRect);
	}

	public Vector3 GetCurPosition()
	{
		if (march == null)
		{
			return Vector3.zero;
		}
		long serverTime = GameEntry.Timer.GetServerTime();
		march.GetPosition(serverTime, out var position, out var _, out var _);
		return position;
	}
}
