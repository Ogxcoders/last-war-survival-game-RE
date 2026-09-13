using System.Runtime.InteropServices;
using UnityEngine;

namespace Unity.Profiling.LowLevel.Unsafe;

[StructLayout(LayoutKind.Explicit, Size = 24)]
internal struct ProfilerCategoryDescription
{
	[FieldOffset(0)]
	public readonly ushort Id;

	[FieldOffset(4)]
	public readonly Color32 Color;

	[FieldOffset(8)]
	private readonly int reserved0;

	[FieldOffset(12)]
	public readonly int NameUtf8Len;

	[FieldOffset(16)]
	public unsafe readonly byte* NameUtf8;

	public unsafe string Name => ProfilerUnsafeUtility.Utf8ToString(NameUtf8, NameUtf8Len);
}
