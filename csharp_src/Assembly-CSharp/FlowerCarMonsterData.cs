using System;
using Protobuf;

public class FlowerCarMonsterData : IDisposable
{
	public float armorRatio;

	public void SetData(FloatInfo msg)
	{
		armorRatio = ((msg.MaxArmor > 0) ? ((float)msg.CurrentArmor * 100f / (float)msg.MaxArmor) : 0f);
	}

	public void Dispose()
	{
	}
}
