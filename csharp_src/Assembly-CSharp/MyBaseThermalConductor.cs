using Protobuf;
using Sfs2X.Entities.Data;
using UnityEngine;

public class MyBaseThermalConductor : ThermalConductor
{
	private float? lastTemp;

	private int lastDir;

	private long lastTime;

	private ITimer timer;

	public bool Inited;

	public void Destroy()
	{
		GameEntry.Timer.CancelTimer(timer);
		timer = null;
	}

	public override void ParseData(ISFSObject msg)
	{
		base.ParseData(msg);
		Inited = true;
		AddTimer();
	}

	protected override void ParseData(Protobuf.ThermalConductor msg)
	{
		base.ParseData(msg);
		Inited = true;
		AddTimer();
	}

	private void AddTimer()
	{
		if (timer == null)
		{
			timer = GameEntry.Timer.RegisterTimerRepeat(1f, 1f, Update1000MS);
		}
	}

	private void Update1000MS()
	{
		int changeDirection = GetChangeDirection();
		if (changeDirection != lastDir)
		{
			GameEntry.Event.Fire(EventId.MyBaseTempDirectionChanged);
			lastDir = changeDirection;
		}
		float curTemperature = GetCurTemperature();
		if (!lastTemp.HasValue)
		{
			GameEntry.Event.Fire(EventId.MyBaseTemperatureChangeCrossZero, curTemperature < 0f);
			GameEntry.Event.Fire(EventId.MyBaseTemperatureConfigChange);
		}
		else
		{
			if (lastTemp >= 0f && curTemperature < 0f)
			{
				GameEntry.Event.Fire(EventId.MyBaseTemperatureChangeCrossZero, true);
			}
			else if (lastTemp < 0f && curTemperature >= 0f)
			{
				GameEntry.Event.Fire(EventId.MyBaseTemperatureChangeCrossZero, false);
			}
			if (Mathf.FloorToInt(lastTemp.Value) != Mathf.FloorToInt(curTemperature))
			{
				GameEntry.Event.Fire(EventId.MyBaseTemperatureConfigChange);
			}
		}
		lastTemp = curTemperature;
	}
}
