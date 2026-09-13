using System;
using System.Collections.Generic;
using Protobuf;

public class ArmyInfo : IDisposable
{
	public int initHealth;

	public int health;

	public long uuid;

	public string uid;

	public string pic;

	public int picVer;

	public int headSkinId;

	public List<ArmySoldierInfo> Soldiers = new List<ArmySoldierInfo>();

	public List<HeroInfo> HeroInfos = new List<HeroInfo>();

	public List<ArmyUnitBuff> UnitBuffs = new List<ArmyUnitBuff>();

	public TacWeaponInfo WeaponInfo;

	public int weaponSkinId;

	private static ThreadSafeObjectPool<ArmySoldierInfo> _SoldiersPool = new ThreadSafeObjectPool<ArmySoldierInfo>(() => new ArmySoldierInfo());

	private static ThreadSafeObjectPool<HeroInfo> _HeroInfosPool = new ThreadSafeObjectPool<HeroInfo>(() => new HeroInfo());

	private static ThreadSafeObjectPool<ArmyUnitBuff> _UnitBuffsPool = new ThreadSafeObjectPool<ArmyUnitBuff>(() => new ArmyUnitBuff());

	private static ThreadSafeObjectPool<TacWeaponInfo> _WeaponInfoPool = new ThreadSafeObjectPool<TacWeaponInfo>(() => new TacWeaponInfo());

	public HeroInfo GetLeaderHero()
	{
		if (HeroInfos == null || HeroInfos.Count == 0)
		{
			return null;
		}
		HeroInfo heroInfo = null;
		foreach (HeroInfo heroInfo2 in HeroInfos)
		{
			if (heroInfo == null || heroInfo2.heroQuality > heroInfo.heroQuality)
			{
				heroInfo = heroInfo2;
			}
		}
		return heroInfo;
	}

	public static void ReleasePools(int remainCount)
	{
		_SoldiersPool.Release(remainCount);
		_HeroInfosPool.Release(remainCount);
		_UnitBuffsPool.Release(remainCount);
		_WeaponInfoPool.Release(remainCount);
		HeroInfo.ReleasePools(remainCount);
	}

	public void Dispose()
	{
		Soldiers.Clear();
		HeroInfos.Clear();
		UnitBuffs.Clear();
		WeaponInfo = null;
	}

	public void UpdateArmyList(ArmyUnitInfo proto)
	{
		HeroInfos.Clear();
		Soldiers.Clear();
		UnitBuffs.Clear();
		foreach (SoldierProto soldier in proto.Soldiers)
		{
			ArmySoldierInfo armySoldierInfo = new ArmySoldierInfo();
			armySoldierInfo.UpdateSoldier(soldier);
			Soldiers.Add(armySoldierInfo);
		}
		foreach (HeroInfoProto hero in proto.Heroes)
		{
			HeroInfo heroInfo = new HeroInfo();
			heroInfo.UpdateHeroInfo(hero);
			HeroInfos.Add(heroInfo);
		}
		foreach (Protobuf.ArmyUnitBuff unitBuff in proto.UnitBuffs)
		{
			ArmyUnitBuff armyUnitBuff = new ArmyUnitBuff();
			armyUnitBuff.UpdateUnitBuff(unitBuff);
			UnitBuffs.Add(armyUnitBuff);
		}
		WeaponProto weapon = proto.Weapon;
		if (weapon != null)
		{
			WeaponInfo = new TacWeaponInfo();
			WeaponInfo.UpdateWeapon(weapon);
		}
		weaponSkinId = proto.WeaponSkinId;
	}

	public bool IsMummy()
	{
		if (Soldiers != null && Soldiers.Count > 0)
		{
			for (int i = 0; i < Soldiers.Count; i++)
			{
				if (Soldiers[i] != null && Soldiers[i].IsMummy())
				{
					return true;
				}
			}
		}
		return false;
	}
}
