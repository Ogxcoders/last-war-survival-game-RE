using System;

namespace Box2DSharp.Dynamics;

[Flags]
public enum BodyFlags
{
	Island = 1,
	IsAwake = 2,
	AutoSleep = 4,
	IsBullet = 8,
	FixedRotation = 0x10,
	IsEnabled = 0x20,
	Toi = 0x40
}
