using System;
using Protobuf;

public class TacWeaponInfo : IDisposable
{
	public int weaponId;

	public int weaponLevel;

	public void UpdateWeapon(WeaponProto proto)
	{
		weaponId = proto.Id;
		weaponLevel = proto.Lv;
	}

	public void Dispose()
	{
	}
}
