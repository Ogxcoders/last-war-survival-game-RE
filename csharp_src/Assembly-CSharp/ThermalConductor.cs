using Protobuf;
using Sfs2X.Entities.Data;
using Unity.Mathematics;
using UnityEngine;

public class ThermalConductor
{
	public string uuid;

	public float cur;

	public long startTime;

	public float target;

	public long endTime;

	public int phase;

	public int nextPhase;

	public long nextPhaseEndTime;

	public long changePhaseDuration;

	public float speed;

	public int hp;

	public int maxHp;

	public ThermalConductor()
	{
	}

	public ThermalConductor(ISFSObject msg)
	{
		ParseData(msg);
	}

	public ThermalConductor(Protobuf.ThermalConductor msg)
	{
		ParseData(msg);
	}

	public virtual void ParseData(ISFSObject msg)
	{
		uuid = msg.TryGetString("uuid");
		startTime = msg.TryGetLong("startTime");
		cur = msg.TryGetFloat("cur");
		target = msg.TryGetFloat("target");
		endTime = msg.TryGetLong("endTime");
		phase = msg.TryGetInt("phase");
		nextPhase = msg.TryGetInt("nextPhase");
		nextPhaseEndTime = msg.TryGetLong("nextPhaseEndTime");
		changePhaseDuration = msg.TryGetLong("changePhaseDuration");
		speed = msg.TryGetFloat("speed");
		hp = msg.TryGetInt("hp");
		maxHp = msg.TryGetInt("maxHp");
		FixPhaseData();
	}

	protected virtual void ParseData(Protobuf.ThermalConductor msg)
	{
		uuid = msg.Uuid;
		startTime = msg.StartTime;
		cur = msg.Cur;
		target = msg.Target;
		endTime = msg.EndTime;
		phase = msg.Phase;
		nextPhase = msg.NextPhase;
		nextPhaseEndTime = msg.NextPhaseEndTime;
		changePhaseDuration = msg.ChangePhaseDuration;
		speed = msg.Speed;
		hp = msg.Hp;
		maxHp = msg.MaxHp;
		FixPhaseData();
	}

	public void FixPhaseData()
	{
		if (nextPhase != 0 && nextPhaseEndTime != 0L && nextPhaseEndTime < GameEntry.Timer.GetServerTime())
		{
			phase = nextPhase;
			nextPhase = 0;
			nextPhaseEndTime = 0L;
		}
	}

	public float GetCurTemperature()
	{
		long serverTime = GameEntry.Timer.GetServerTime();
		if (endTime <= serverTime)
		{
			return target;
		}
		float num = (float)(serverTime - startTime) * 0.001f * speed;
		if (target >= cur)
		{
			return math.min(cur + num, target);
		}
		return math.max(cur - num, target);
	}

	public string GetCurTemperatureString()
	{
		float curTemperature = GetCurTemperature();
		if (curTemperature % 1f == 0f)
		{
			return $"{curTemperature:0}°";
		}
		return $"{curTemperature:0.0}°";
	}

	public bool IsPhaseChanging()
	{
		FixPhaseData();
		return nextPhaseEndTime > 0;
	}

	public int GetPhaseChangeType()
	{
		if (IsPhaseChanging())
		{
			if (nextPhase == 2)
			{
				return 1;
			}
			if (nextPhase == 3)
			{
				return 2;
			}
			if (phase == 2)
			{
				return 3;
			}
			if (phase == 3)
			{
				return 4;
			}
		}
		return 0;
	}

	public int GetChangeDirection()
	{
		if (endTime < GameEntry.Timer.GetServerTime())
		{
			return 0;
		}
		if (Mathf.Abs(target - cur) < 0.01f)
		{
			return 0;
		}
		if (cur < target)
		{
			return 1;
		}
		return -1;
	}
}
