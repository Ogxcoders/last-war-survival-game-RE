using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace Unity.Mathematics;

[Serializable]
[DebuggerTypeProxy(typeof(DebuggerProxy))]
public struct uint3 : IEquatable<uint3>, IFormattable
{
	internal sealed class DebuggerProxy
	{
		public uint x;

		public uint y;

		public uint z;

		public DebuggerProxy(uint3 v)
		{
			x = v.x;
			y = v.y;
			z = v.z;
		}
	}

	public uint x;

	public uint y;

	public uint z;

	public static readonly uint3 zero;

	[EditorBrowsable(EditorBrowsableState.Never)]
	public uint4 xxxx
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new uint4(x, x, x, x);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public uint4 xxxy
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new uint4(x, x, x, y);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public uint4 xxxz
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new uint4(x, x, x, z);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public uint4 xxyx
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new uint4(x, x, y, x);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public uint4 xxyy
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new uint4(x, x, y, y);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public uint4 xxyz
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new uint4(x, x, y, z);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public uint4 xxzx
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new uint4(x, x, z, x);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public uint4 xxzy
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new uint4(x, x, z, y);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public uint4 xxzz
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new uint4(x, x, z, z);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public uint4 xyxx
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new uint4(x, y, x, x);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public uint4 xyxy
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new uint4(x, y, x, y);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public uint4 xyxz
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new uint4(x, y, x, z);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public uint4 xyyx
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new uint4(x, y, y, x);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public uint4 xyyy
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new uint4(x, y, y, y);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public uint4 xyyz
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new uint4(x, y, y, z);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public uint4 xyzx
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new uint4(x, y, z, x);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public uint4 xyzy
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new uint4(x, y, z, y);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public uint4 xyzz
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new uint4(x, y, z, z);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public uint4 xzxx
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new uint4(x, z, x, x);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public uint4 xzxy
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new uint4(x, z, x, y);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public uint4 xzxz
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new uint4(x, z, x, z);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public uint4 xzyx
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new uint4(x, z, y, x);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public uint4 xzyy
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new uint4(x, z, y, y);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public uint4 xzyz
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new uint4(x, z, y, z);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public uint4 xzzx
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new uint4(x, z, z, x);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public uint4 xzzy
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new uint4(x, z, z, y);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public uint4 xzzz
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new uint4(x, z, z, z);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public uint4 yxxx
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new uint4(y, x, x, x);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public uint4 yxxy
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new uint4(y, x, x, y);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public uint4 yxxz
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new uint4(y, x, x, z);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public uint4 yxyx
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new uint4(y, x, y, x);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public uint4 yxyy
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new uint4(y, x, y, y);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public uint4 yxyz
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new uint4(y, x, y, z);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public uint4 yxzx
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new uint4(y, x, z, x);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public uint4 yxzy
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new uint4(y, x, z, y);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public uint4 yxzz
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new uint4(y, x, z, z);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public uint4 yyxx
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new uint4(y, y, x, x);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public uint4 yyxy
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new uint4(y, y, x, y);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public uint4 yyxz
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new uint4(y, y, x, z);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public uint4 yyyx
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new uint4(y, y, y, x);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public uint4 yyyy
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new uint4(y, y, y, y);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public uint4 yyyz
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new uint4(y, y, y, z);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public uint4 yyzx
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new uint4(y, y, z, x);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public uint4 yyzy
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new uint4(y, y, z, y);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public uint4 yyzz
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new uint4(y, y, z, z);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public uint4 yzxx
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new uint4(y, z, x, x);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public uint4 yzxy
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new uint4(y, z, x, y);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public uint4 yzxz
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new uint4(y, z, x, z);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public uint4 yzyx
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new uint4(y, z, y, x);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public uint4 yzyy
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new uint4(y, z, y, y);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public uint4 yzyz
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new uint4(y, z, y, z);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public uint4 yzzx
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new uint4(y, z, z, x);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public uint4 yzzy
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new uint4(y, z, z, y);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public uint4 yzzz
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new uint4(y, z, z, z);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public uint4 zxxx
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new uint4(z, x, x, x);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public uint4 zxxy
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new uint4(z, x, x, y);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public uint4 zxxz
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new uint4(z, x, x, z);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public uint4 zxyx
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new uint4(z, x, y, x);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public uint4 zxyy
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new uint4(z, x, y, y);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public uint4 zxyz
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new uint4(z, x, y, z);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public uint4 zxzx
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new uint4(z, x, z, x);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public uint4 zxzy
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new uint4(z, x, z, y);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public uint4 zxzz
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new uint4(z, x, z, z);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public uint4 zyxx
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new uint4(z, y, x, x);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public uint4 zyxy
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new uint4(z, y, x, y);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public uint4 zyxz
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new uint4(z, y, x, z);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public uint4 zyyx
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new uint4(z, y, y, x);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public uint4 zyyy
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new uint4(z, y, y, y);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public uint4 zyyz
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new uint4(z, y, y, z);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public uint4 zyzx
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new uint4(z, y, z, x);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public uint4 zyzy
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new uint4(z, y, z, y);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public uint4 zyzz
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new uint4(z, y, z, z);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public uint4 zzxx
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new uint4(z, z, x, x);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public uint4 zzxy
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new uint4(z, z, x, y);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public uint4 zzxz
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new uint4(z, z, x, z);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public uint4 zzyx
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new uint4(z, z, y, x);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public uint4 zzyy
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new uint4(z, z, y, y);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public uint4 zzyz
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new uint4(z, z, y, z);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public uint4 zzzx
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new uint4(z, z, z, x);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public uint4 zzzy
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new uint4(z, z, z, y);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public uint4 zzzz
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new uint4(z, z, z, z);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public uint3 xxx
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new uint3(x, x, x);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public uint3 xxy
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new uint3(x, x, y);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public uint3 xxz
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new uint3(x, x, z);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public uint3 xyx
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new uint3(x, y, x);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public uint3 xyy
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new uint3(x, y, y);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public uint3 xyz
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new uint3(x, y, z);
		}
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		set
		{
			x = value.x;
			y = value.y;
			z = value.z;
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public uint3 xzx
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new uint3(x, z, x);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public uint3 xzy
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new uint3(x, z, y);
		}
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		set
		{
			x = value.x;
			z = value.y;
			y = value.z;
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public uint3 xzz
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new uint3(x, z, z);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public uint3 yxx
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new uint3(y, x, x);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public uint3 yxy
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new uint3(y, x, y);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public uint3 yxz
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new uint3(y, x, z);
		}
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		set
		{
			y = value.x;
			x = value.y;
			z = value.z;
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public uint3 yyx
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new uint3(y, y, x);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public uint3 yyy
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new uint3(y, y, y);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public uint3 yyz
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new uint3(y, y, z);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public uint3 yzx
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new uint3(y, z, x);
		}
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		set
		{
			y = value.x;
			z = value.y;
			x = value.z;
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public uint3 yzy
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new uint3(y, z, y);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public uint3 yzz
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new uint3(y, z, z);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public uint3 zxx
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new uint3(z, x, x);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public uint3 zxy
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new uint3(z, x, y);
		}
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		set
		{
			z = value.x;
			x = value.y;
			y = value.z;
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public uint3 zxz
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new uint3(z, x, z);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public uint3 zyx
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new uint3(z, y, x);
		}
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		set
		{
			z = value.x;
			y = value.y;
			x = value.z;
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public uint3 zyy
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new uint3(z, y, y);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public uint3 zyz
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new uint3(z, y, z);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public uint3 zzx
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new uint3(z, z, x);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public uint3 zzy
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new uint3(z, z, y);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public uint3 zzz
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new uint3(z, z, z);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public uint2 xx
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new uint2(x, x);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public uint2 xy
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new uint2(x, y);
		}
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		set
		{
			x = value.x;
			y = value.y;
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public uint2 xz
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new uint2(x, z);
		}
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		set
		{
			x = value.x;
			z = value.y;
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public uint2 yx
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new uint2(y, x);
		}
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		set
		{
			y = value.x;
			x = value.y;
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public uint2 yy
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new uint2(y, y);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public uint2 yz
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new uint2(y, z);
		}
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		set
		{
			y = value.x;
			z = value.y;
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public uint2 zx
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new uint2(z, x);
		}
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		set
		{
			z = value.x;
			x = value.y;
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public uint2 zy
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new uint2(z, y);
		}
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		set
		{
			z = value.x;
			y = value.y;
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public uint2 zz
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new uint2(z, z);
		}
	}

	public unsafe uint this[int index]
	{
		get
		{
			fixed (uint3* ptr = &this)
			{
				return ((uint*)ptr)[index];
			}
		}
		set
		{
			fixed (uint* ptr = &x)
			{
				ptr[index] = value;
			}
		}
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public uint3(uint x, uint y, uint z)
	{
		this.x = x;
		this.y = y;
		this.z = z;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public uint3(uint x, uint2 yz)
	{
		this.x = x;
		y = yz.x;
		z = yz.y;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public uint3(uint2 xy, uint z)
	{
		x = xy.x;
		y = xy.y;
		this.z = z;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public uint3(uint3 xyz)
	{
		x = xyz.x;
		y = xyz.y;
		z = xyz.z;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public uint3(uint v)
	{
		x = v;
		y = v;
		z = v;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public uint3(bool v)
	{
		x = (v ? 1u : 0u);
		y = (v ? 1u : 0u);
		z = (v ? 1u : 0u);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public uint3(bool3 v)
	{
		x = (v.x ? 1u : 0u);
		y = (v.y ? 1u : 0u);
		z = (v.z ? 1u : 0u);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public uint3(int v)
	{
		x = (uint)v;
		y = (uint)v;
		z = (uint)v;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public uint3(int3 v)
	{
		x = (uint)v.x;
		y = (uint)v.y;
		z = (uint)v.z;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public uint3(float v)
	{
		x = (uint)v;
		y = (uint)v;
		z = (uint)v;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public uint3(float3 v)
	{
		x = (uint)v.x;
		y = (uint)v.y;
		z = (uint)v.z;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public uint3(double v)
	{
		x = (uint)v;
		y = (uint)v;
		z = (uint)v;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public uint3(double3 v)
	{
		x = (uint)v.x;
		y = (uint)v.y;
		z = (uint)v.z;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static implicit operator uint3(uint v)
	{
		return new uint3(v);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static explicit operator uint3(bool v)
	{
		return new uint3(v);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static explicit operator uint3(bool3 v)
	{
		return new uint3(v);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static explicit operator uint3(int v)
	{
		return new uint3(v);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static explicit operator uint3(int3 v)
	{
		return new uint3(v);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static explicit operator uint3(float v)
	{
		return new uint3(v);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static explicit operator uint3(float3 v)
	{
		return new uint3(v);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static explicit operator uint3(double v)
	{
		return new uint3(v);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static explicit operator uint3(double3 v)
	{
		return new uint3(v);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static uint3 operator *(uint3 lhs, uint3 rhs)
	{
		return new uint3(lhs.x * rhs.x, lhs.y * rhs.y, lhs.z * rhs.z);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static uint3 operator *(uint3 lhs, uint rhs)
	{
		return new uint3(lhs.x * rhs, lhs.y * rhs, lhs.z * rhs);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static uint3 operator *(uint lhs, uint3 rhs)
	{
		return new uint3(lhs * rhs.x, lhs * rhs.y, lhs * rhs.z);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static uint3 operator +(uint3 lhs, uint3 rhs)
	{
		return new uint3(lhs.x + rhs.x, lhs.y + rhs.y, lhs.z + rhs.z);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static uint3 operator +(uint3 lhs, uint rhs)
	{
		return new uint3(lhs.x + rhs, lhs.y + rhs, lhs.z + rhs);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static uint3 operator +(uint lhs, uint3 rhs)
	{
		return new uint3(lhs + rhs.x, lhs + rhs.y, lhs + rhs.z);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static uint3 operator -(uint3 lhs, uint3 rhs)
	{
		return new uint3(lhs.x - rhs.x, lhs.y - rhs.y, lhs.z - rhs.z);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static uint3 operator -(uint3 lhs, uint rhs)
	{
		return new uint3(lhs.x - rhs, lhs.y - rhs, lhs.z - rhs);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static uint3 operator -(uint lhs, uint3 rhs)
	{
		return new uint3(lhs - rhs.x, lhs - rhs.y, lhs - rhs.z);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static uint3 operator /(uint3 lhs, uint3 rhs)
	{
		return new uint3(lhs.x / rhs.x, lhs.y / rhs.y, lhs.z / rhs.z);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static uint3 operator /(uint3 lhs, uint rhs)
	{
		return new uint3(lhs.x / rhs, lhs.y / rhs, lhs.z / rhs);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static uint3 operator /(uint lhs, uint3 rhs)
	{
		return new uint3(lhs / rhs.x, lhs / rhs.y, lhs / rhs.z);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static uint3 operator %(uint3 lhs, uint3 rhs)
	{
		return new uint3(lhs.x % rhs.x, lhs.y % rhs.y, lhs.z % rhs.z);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static uint3 operator %(uint3 lhs, uint rhs)
	{
		return new uint3(lhs.x % rhs, lhs.y % rhs, lhs.z % rhs);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static uint3 operator %(uint lhs, uint3 rhs)
	{
		return new uint3(lhs % rhs.x, lhs % rhs.y, lhs % rhs.z);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static uint3 operator ++(uint3 val)
	{
		return new uint3(++val.x, ++val.y, ++val.z);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static uint3 operator --(uint3 val)
	{
		return new uint3(--val.x, --val.y, --val.z);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool3 operator <(uint3 lhs, uint3 rhs)
	{
		return new bool3(lhs.x < rhs.x, lhs.y < rhs.y, lhs.z < rhs.z);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool3 operator <(uint3 lhs, uint rhs)
	{
		return new bool3(lhs.x < rhs, lhs.y < rhs, lhs.z < rhs);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool3 operator <(uint lhs, uint3 rhs)
	{
		return new bool3(lhs < rhs.x, lhs < rhs.y, lhs < rhs.z);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool3 operator <=(uint3 lhs, uint3 rhs)
	{
		return new bool3(lhs.x <= rhs.x, lhs.y <= rhs.y, lhs.z <= rhs.z);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool3 operator <=(uint3 lhs, uint rhs)
	{
		return new bool3(lhs.x <= rhs, lhs.y <= rhs, lhs.z <= rhs);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool3 operator <=(uint lhs, uint3 rhs)
	{
		return new bool3(lhs <= rhs.x, lhs <= rhs.y, lhs <= rhs.z);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool3 operator >(uint3 lhs, uint3 rhs)
	{
		return new bool3(lhs.x > rhs.x, lhs.y > rhs.y, lhs.z > rhs.z);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool3 operator >(uint3 lhs, uint rhs)
	{
		return new bool3(lhs.x > rhs, lhs.y > rhs, lhs.z > rhs);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool3 operator >(uint lhs, uint3 rhs)
	{
		return new bool3(lhs > rhs.x, lhs > rhs.y, lhs > rhs.z);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool3 operator >=(uint3 lhs, uint3 rhs)
	{
		return new bool3(lhs.x >= rhs.x, lhs.y >= rhs.y, lhs.z >= rhs.z);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool3 operator >=(uint3 lhs, uint rhs)
	{
		return new bool3(lhs.x >= rhs, lhs.y >= rhs, lhs.z >= rhs);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool3 operator >=(uint lhs, uint3 rhs)
	{
		return new bool3(lhs >= rhs.x, lhs >= rhs.y, lhs >= rhs.z);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static uint3 operator -(uint3 val)
	{
		return new uint3((uint)(0uL - (ulong)val.x), (uint)(0uL - (ulong)val.y), (uint)(0uL - (ulong)val.z));
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static uint3 operator +(uint3 val)
	{
		return new uint3(val.x, val.y, val.z);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static uint3 operator <<(uint3 x, int n)
	{
		return new uint3(x.x << n, x.y << n, x.z << n);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static uint3 operator >>(uint3 x, int n)
	{
		return new uint3(x.x >> n, x.y >> n, x.z >> n);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool3 operator ==(uint3 lhs, uint3 rhs)
	{
		return new bool3(lhs.x == rhs.x, lhs.y == rhs.y, lhs.z == rhs.z);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool3 operator ==(uint3 lhs, uint rhs)
	{
		return new bool3(lhs.x == rhs, lhs.y == rhs, lhs.z == rhs);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool3 operator ==(uint lhs, uint3 rhs)
	{
		return new bool3(lhs == rhs.x, lhs == rhs.y, lhs == rhs.z);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool3 operator !=(uint3 lhs, uint3 rhs)
	{
		return new bool3(lhs.x != rhs.x, lhs.y != rhs.y, lhs.z != rhs.z);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool3 operator !=(uint3 lhs, uint rhs)
	{
		return new bool3(lhs.x != rhs, lhs.y != rhs, lhs.z != rhs);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool3 operator !=(uint lhs, uint3 rhs)
	{
		return new bool3(lhs != rhs.x, lhs != rhs.y, lhs != rhs.z);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static uint3 operator ~(uint3 val)
	{
		return new uint3(~val.x, ~val.y, ~val.z);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static uint3 operator &(uint3 lhs, uint3 rhs)
	{
		return new uint3(lhs.x & rhs.x, lhs.y & rhs.y, lhs.z & rhs.z);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static uint3 operator &(uint3 lhs, uint rhs)
	{
		return new uint3(lhs.x & rhs, lhs.y & rhs, lhs.z & rhs);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static uint3 operator &(uint lhs, uint3 rhs)
	{
		return new uint3(lhs & rhs.x, lhs & rhs.y, lhs & rhs.z);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static uint3 operator |(uint3 lhs, uint3 rhs)
	{
		return new uint3(lhs.x | rhs.x, lhs.y | rhs.y, lhs.z | rhs.z);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static uint3 operator |(uint3 lhs, uint rhs)
	{
		return new uint3(lhs.x | rhs, lhs.y | rhs, lhs.z | rhs);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static uint3 operator |(uint lhs, uint3 rhs)
	{
		return new uint3(lhs | rhs.x, lhs | rhs.y, lhs | rhs.z);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static uint3 operator ^(uint3 lhs, uint3 rhs)
	{
		return new uint3(lhs.x ^ rhs.x, lhs.y ^ rhs.y, lhs.z ^ rhs.z);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static uint3 operator ^(uint3 lhs, uint rhs)
	{
		return new uint3(lhs.x ^ rhs, lhs.y ^ rhs, lhs.z ^ rhs);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static uint3 operator ^(uint lhs, uint3 rhs)
	{
		return new uint3(lhs ^ rhs.x, lhs ^ rhs.y, lhs ^ rhs.z);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public bool Equals(uint3 rhs)
	{
		if (x == rhs.x && y == rhs.y)
		{
			return z == rhs.z;
		}
		return false;
	}

	public override bool Equals(object o)
	{
		return Equals((uint3)o);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public override int GetHashCode()
	{
		return (int)math.hash(this);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public override string ToString()
	{
		return $"uint3({x}, {y}, {z})";
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public string ToString(string format, IFormatProvider formatProvider)
	{
		return $"uint3({x.ToString(format, formatProvider)}, {y.ToString(format, formatProvider)}, {z.ToString(format, formatProvider)})";
	}
}
