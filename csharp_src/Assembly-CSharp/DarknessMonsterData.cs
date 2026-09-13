using System;
using Protobuf;

public class DarknessMonsterData : IDisposable
{
	public bool isBloodNight;

	public WhistleMonsterData whistleMonsterData;

	public FlowerCarMonsterData flowerCarMonsterData;

	public float hpRatio;

	public void SetData(MonsterInfo msg)
	{
		if (msg.Type == 22)
		{
			hpRatio = 100f;
		}
		else
		{
			hpRatio = ((msg.DarknessMonster.MaxHp > 0) ? ((float)msg.DarknessMonster.CurHp * 100f / (float)msg.DarknessMonster.MaxHp) : 0f);
		}
		isBloodNight = msg.DarknessMonster.IsBloodNight;
		if (msg.DarknessMonster.WhistleInfo != null)
		{
			if (whistleMonsterData == null)
			{
				whistleMonsterData = new WhistleMonsterData();
			}
			whistleMonsterData.SetData(msg.DarknessMonster.WhistleInfo);
		}
		if (msg.DarknessMonster.FloatInfo != null)
		{
			if (flowerCarMonsterData == null)
			{
				flowerCarMonsterData = new FlowerCarMonsterData();
			}
			flowerCarMonsterData.SetData(msg.DarknessMonster.FloatInfo);
		}
	}

	public void Dispose()
	{
	}
}
