using System;
using System.Collections.Generic;
using Protobuf;

public class ArmyUnitBuff : IDisposable
{
	public int buffId;

	public Dictionary<int, float> effectDict = new Dictionary<int, float>();

	public int expireTime;

	public void Dispose()
	{
		effectDict.Clear();
	}

	public void UpdateUnitBuff(Protobuf.ArmyUnitBuff proto)
	{
		buffId = proto.BuffId;
		effectDict = new Dictionary<int, float>();
		expireTime = proto.ExpireTime;
		for (int i = 0; i < proto.EffectId.Count; i++)
		{
			effectDict[proto.EffectId[i]] = proto.Value[i];
		}
	}
}
