using System.Runtime.InteropServices;

namespace Box2DSharp.Collision.Collider;

[StructLayout(LayoutKind.Explicit, Size = 4)]
public struct ContactId
{
	[FieldOffset(0)]
	public ContactFeature ContactFeature;

	[FieldOffset(0)]
	public uint Key;
}
