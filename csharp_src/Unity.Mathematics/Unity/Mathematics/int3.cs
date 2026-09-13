using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace Unity.Mathematics;

[Serializable]
[DebuggerTypeProxy(typeof(DebuggerProxy))]
public struct int3 : IEquatable<int3>, IFormattable
{
	internal sealed class DebuggerProxy
	{
		public int x;

		public int y;

		public int z;

		public DebuggerProxy(int3 v)
		{
			x = v.x;
			y = v.y;
			z = v.z;
		}
	}

	public int x;

	public int y;

	public int z;

	public static readonly int3 zero;

	[EditorBrowsable(EditorBrowsableState.Never)]
	public int4 xxxx
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new int4(x, x, x, x);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public int4 xxxy
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new int4(x, x, x, y);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public int4 xxxz
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new int4(x, x, x, z);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public int4 xxyx
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new int4(x, x, y, x);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public int4 xxyy
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new int4(x, x, y, y);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public int4 xxyz
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new int4(x, x, y, z);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public int4 xxzx
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new int4(x, x, z, x);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public int4 xxzy
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new int4(x, x, z, y);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public int4 xxzz
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new int4(x, x, z, z);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public int4 xyxx
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new int4(x, y, x, x);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public int4 xyxy
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new int4(x, y, x, y);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public int4 xyxz
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new int4(x, y, x, z);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public int4 xyyx
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new int4(x, y, y, x);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public int4 xyyy
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new int4(x, y, y, y);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public int4 xyyz
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new int4(x, y, y, z);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public int4 xyzx
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new int4(x, y, z, x);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public int4 xyzy
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new int4(x, y, z, y);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public int4 xyzz
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new int4(x, y, z, z);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public int4 xzxx
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new int4(x, z, x, x);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public int4 xzxy
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new int4(x, z, x, y);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public int4 xzxz
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new int4(x, z, x, z);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public int4 xzyx
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new int4(x, z, y, x);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public int4 xzyy
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new int4(x, z, y, y);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public int4 xzyz
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new int4(x, z, y, z);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public int4 xzzx
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new int4(x, z, z, x);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public int4 xzzy
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new int4(x, z, z, y);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public int4 xzzz
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new int4(x, z, z, z);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public int4 yxxx
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new int4(y, x, x, x);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public int4 yxxy
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new int4(y, x, x, y);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public int4 yxxz
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new int4(y, x, x, z);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public int4 yxyx
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new int4(y, x, y, x);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public int4 yxyy
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new int4(y, x, y, y);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public int4 yxyz
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new int4(y, x, y, z);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public int4 yxzx
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new int4(y, x, z, x);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public int4 yxzy
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new int4(y, x, z, y);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public int4 yxzz
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new int4(y, x, z, z);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public int4 yyxx
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new int4(y, y, x, x);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public int4 yyxy
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new int4(y, y, x, y);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public int4 yyxz
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new int4(y, y, x, z);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public int4 yyyx
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new int4(y, y, y, x);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public int4 yyyy
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new int4(y, y, y, y);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public int4 yyyz
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new int4(y, y, y, z);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public int4 yyzx
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new int4(y, y, z, x);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public int4 yyzy
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new int4(y, y, z, y);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public int4 yyzz
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new int4(y, y, z, z);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public int4 yzxx
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new int4(y, z, x, x);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public int4 yzxy
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new int4(y, z, x, y);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public int4 yzxz
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new int4(y, z, x, z);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public int4 yzyx
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new int4(y, z, y, x);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public int4 yzyy
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new int4(y, z, y, y);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public int4 yzyz
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new int4(y, z, y, z);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public int4 yzzx
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new int4(y, z, z, x);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public int4 yzzy
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new int4(y, z, z, y);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public int4 yzzz
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new int4(y, z, z, z);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public int4 zxxx
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new int4(z, x, x, x);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public int4 zxxy
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new int4(z, x, x, y);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public int4 zxxz
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new int4(z, x, x, z);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public int4 zxyx
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new int4(z, x, y, x);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public int4 zxyy
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new int4(z, x, y, y);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public int4 zxyz
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new int4(z, x, y, z);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public int4 zxzx
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new int4(z, x, z, x);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public int4 zxzy
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new int4(z, x, z, y);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public int4 zxzz
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new int4(z, x, z, z);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public int4 zyxx
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new int4(z, y, x, x);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public int4 zyxy
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new int4(z, y, x, y);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public int4 zyxz
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new int4(z, y, x, z);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public int4 zyyx
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new int4(z, y, y, x);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public int4 zyyy
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new int4(z, y, y, y);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public int4 zyyz
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new int4(z, y, y, z);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public int4 zyzx
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new int4(z, y, z, x);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public int4 zyzy
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new int4(z, y, z, y);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public int4 zyzz
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new int4(z, y, z, z);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public int4 zzxx
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new int4(z, z, x, x);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public int4 zzxy
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new int4(z, z, x, y);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public int4 zzxz
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new int4(z, z, x, z);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public int4 zzyx
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new int4(z, z, y, x);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public int4 zzyy
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new int4(z, z, y, y);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public int4 zzyz
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new int4(z, z, y, z);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public int4 zzzx
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new int4(z, z, z, x);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public int4 zzzy
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new int4(z, z, z, y);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public int4 zzzz
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new int4(z, z, z, z);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public int3 xxx
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new int3(x, x, x);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public int3 xxy
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new int3(x, x, y);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public int3 xxz
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new int3(x, x, z);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public int3 xyx
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new int3(x, y, x);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public int3 xyy
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new int3(x, y, y);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public int3 xyz
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new int3(x, y, z);
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
	public int3 xzx
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new int3(x, z, x);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public int3 xzy
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new int3(x, z, y);
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
	public int3 xzz
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new int3(x, z, z);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public int3 yxx
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new int3(y, x, x);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public int3 yxy
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new int3(y, x, y);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public int3 yxz
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new int3(y, x, z);
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
	public int3 yyx
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new int3(y, y, x);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public int3 yyy
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new int3(y, y, y);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public int3 yyz
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new int3(y, y, z);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public int3 yzx
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new int3(y, z, x);
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
	public int3 yzy
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new int3(y, z, y);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public int3 yzz
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new int3(y, z, z);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public int3 zxx
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new int3(z, x, x);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public int3 zxy
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new int3(z, x, y);
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
	public int3 zxz
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new int3(z, x, z);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public int3 zyx
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new int3(z, y, x);
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
	public int3 zyy
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new int3(z, y, y);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public int3 zyz
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new int3(z, y, z);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public int3 zzx
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new int3(z, z, x);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public int3 zzy
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new int3(z, z, y);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public int3 zzz
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new int3(z, z, z);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public int2 xx
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new int2(x, x);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public int2 xy
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new int2(x, y);
		}
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		set
		{
			x = value.x;
			y = value.y;
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public int2 xz
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new int2(x, z);
		}
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		set
		{
			x = value.x;
			z = value.y;
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public int2 yx
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new int2(y, x);
		}
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		set
		{
			y = value.x;
			x = value.y;
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public int2 yy
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new int2(y, y);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public int2 yz
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new int2(y, z);
		}
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		set
		{
			y = value.x;
			z = value.y;
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public int2 zx
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new int2(z, x);
		}
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		set
		{
			z = value.x;
			x = value.y;
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public int2 zy
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new int2(z, y);
		}
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		set
		{
			z = value.x;
			y = value.y;
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public int2 zz
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new int2(z, z);
		}
	}

	public unsafe int this[int index]
	{
		get
		{
			fixed (int3* ptr = &this)
			{
				return ((int*)ptr)[index];
			}
		}
		set
		{
			fixed (int* ptr = &x)
			{
				ptr[index] = value;
			}
		}
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public int3(int x, int y, int z)
	{
		this.x = x;
		this.y = y;
		this.z = z;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public int3(int x, int2 yz)
	{
		this.x = x;
		y = yz.x;
		z = yz.y;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public int3(int2 xy, int z)
	{
		x = xy.x;
		y = xy.y;
		this.z = z;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public int3(int3 xyz)
	{
		x = xyz.x;
		y = xyz.y;
		z = xyz.z;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public int3(int v)
	{
		x = v;
		y = v;
		z = v;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public int3(bool v)
	{
		x = (v ? 1 : 0);
		y = (v ? 1 : 0);
		z = (v ? 1 : 0);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public int3(bool3 v)
	{
		x = (v.x ? 1 : 0);
		y = (v.y ? 1 : 0);
		z = (v.z ? 1 : 0);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public int3(uint v)
	{
		x = (int)v;
		y = (int)v;
		z = (int)v;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public int3(uint3 v)
	{
		x = (int)v.x;
		y = (int)v.y;
		z = (int)v.z;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public int3(float v)
	{
		x = (int)v;
		y = (int)v;
		z = (int)v;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public int3(float3 v)
	{
		x = (int)v.x;
		y = (int)v.y;
		z = (int)v.z;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public int3(double v)
	{
		x = (int)v;
		y = (int)v;
		z = (int)v;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public int3(double3 v)
	{
		x = (int)v.x;
		y = (int)v.y;
		z = (int)v.z;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static implicit operator int3(int v)
	{
		return new int3(v);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static explicit operator int3(bool v)
	{
		return new int3(v);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static explicit operator int3(bool3 v)
	{
		return new int3(v);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static explicit operator int3(uint v)
	{
		return new int3(v);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static explicit operator int3(uint3 v)
	{
		return new int3(v);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static explicit operator int3(float v)
	{
		return new int3(v);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static explicit operator int3(float3 v)
	{
		return new int3(v);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static explicit operator int3(double v)
	{
		return new int3(v);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static explicit operator int3(double3 v)
	{
		return new int3(v);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static int3 operator *(int3 lhs, int3 rhs)
	{
		return new int3(lhs.x * rhs.x, lhs.y * rhs.y, lhs.z * rhs.z);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static int3 operator *(int3 lhs, int rhs)
	{
		return new int3(lhs.x * rhs, lhs.y * rhs, lhs.z * rhs);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static int3 operator *(int lhs, int3 rhs)
	{
		return new int3(lhs * rhs.x, lhs * rhs.y, lhs * rhs.z);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static int3 operator +(int3 lhs, int3 rhs)
	{
		return new int3(lhs.x + rhs.x, lhs.y + rhs.y, lhs.z + rhs.z);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static int3 operator +(int3 lhs, int rhs)
	{
		return new int3(lhs.x + rhs, lhs.y + rhs, lhs.z + rhs);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static int3 operator +(int lhs, int3 rhs)
	{
		return new int3(lhs + rhs.x, lhs + rhs.y, lhs + rhs.z);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static int3 operator -(int3 lhs, int3 rhs)
	{
		return new int3(lhs.x - rhs.x, lhs.y - rhs.y, lhs.z - rhs.z);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static int3 operator -(int3 lhs, int rhs)
	{
		return new int3(lhs.x - rhs, lhs.y - rhs, lhs.z - rhs);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static int3 operator -(int lhs, int3 rhs)
	{
		return new int3(lhs - rhs.x, lhs - rhs.y, lhs - rhs.z);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static int3 operator /(int3 lhs, int3 rhs)
	{
		return new int3(lhs.x / rhs.x, lhs.y / rhs.y, lhs.z / rhs.z);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static int3 operator /(int3 lhs, int rhs)
	{
		return new int3(lhs.x / rhs, lhs.y / rhs, lhs.z / rhs);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static int3 operator /(int lhs, int3 rhs)
	{
		return new int3(lhs / rhs.x, lhs / rhs.y, lhs / rhs.z);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static int3 operator %(int3 lhs, int3 rhs)
	{
		return new int3(lhs.x % rhs.x, lhs.y % rhs.y, lhs.z % rhs.z);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static int3 operator %(int3 lhs, int rhs)
	{
		return new int3(lhs.x % rhs, lhs.y % rhs, lhs.z % rhs);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static int3 operator %(int lhs, int3 rhs)
	{
		return new int3(lhs % rhs.x, lhs % rhs.y, lhs % rhs.z);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static int3 operator ++(int3 val)
	{
		return new int3(++val.x, ++val.y, ++val.z);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static int3 operator --(int3 val)
	{
		return new int3(--val.x, --val.y, --val.z);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool3 operator <(int3 lhs, int3 rhs)
	{
		return new bool3(lhs.x < rhs.x, lhs.y < rhs.y, lhs.z < rhs.z);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool3 operator <(int3 lhs, int rhs)
	{
		return new bool3(lhs.x < rhs, lhs.y < rhs, lhs.z < rhs);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool3 operator <(int lhs, int3 rhs)
	{
		return new bool3(lhs < rhs.x, lhs < rhs.y, lhs < rhs.z);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool3 operator <=(int3 lhs, int3 rhs)
	{
		return new bool3(lhs.x <= rhs.x, lhs.y <= rhs.y, lhs.z <= rhs.z);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool3 operator <=(int3 lhs, int rhs)
	{
		return new bool3(lhs.x <= rhs, lhs.y <= rhs, lhs.z <= rhs);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool3 operator <=(int lhs, int3 rhs)
	{
		return new bool3(lhs <= rhs.x, lhs <= rhs.y, lhs <= rhs.z);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool3 operator >(int3 lhs, int3 rhs)
	{
		return new bool3(lhs.x > rhs.x, lhs.y > rhs.y, lhs.z > rhs.z);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool3 operator >(int3 lhs, int rhs)
	{
		return new bool3(lhs.x > rhs, lhs.y > rhs, lhs.z > rhs);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool3 operator >(int lhs, int3 rhs)
	{
		return new bool3(lhs > rhs.x, lhs > rhs.y, lhs > rhs.z);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool3 operator >=(int3 lhs, int3 rhs)
	{
		return new bool3(lhs.x >= rhs.x, lhs.y >= rhs.y, lhs.z >= rhs.z);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool3 operator >=(int3 lhs, int rhs)
	{
		return new bool3(lhs.x >= rhs, lhs.y >= rhs, lhs.z >= rhs);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool3 operator >=(int lhs, int3 rhs)
	{
		return new bool3(lhs >= rhs.x, lhs >= rhs.y, lhs >= rhs.z);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static int3 operator -(int3 val)
	{
		return new int3(-val.x, -val.y, -val.z);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static int3 operator +(int3 val)
	{
		return new int3(val.x, val.y, val.z);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static int3 operator <<(int3 x, int n)
	{
		return new int3(x.x << n, x.y << n, x.z << n);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static int3 operator >>(int3 x, int n)
	{
		return new int3(x.x >> n, x.y >> n, x.z >> n);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool3 operator ==(int3 lhs, int3 rhs)
	{
		return new bool3(lhs.x == rhs.x, lhs.y == rhs.y, lhs.z == rhs.z);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool3 operator ==(int3 lhs, int rhs)
	{
		return new bool3(lhs.x == rhs, lhs.y == rhs, lhs.z == rhs);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool3 operator ==(int lhs, int3 rhs)
	{
		return new bool3(lhs == rhs.x, lhs == rhs.y, lhs == rhs.z);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool3 operator !=(int3 lhs, int3 rhs)
	{
		return new bool3(lhs.x != rhs.x, lhs.y != rhs.y, lhs.z != rhs.z);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool3 operator !=(int3 lhs, int rhs)
	{
		return new bool3(lhs.x != rhs, lhs.y != rhs, lhs.z != rhs);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool3 operator !=(int lhs, int3 rhs)
	{
		return new bool3(lhs != rhs.x, lhs != rhs.y, lhs != rhs.z);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static int3 operator ~(int3 val)
	{
		return new int3(~val.x, ~val.y, ~val.z);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static int3 operator &(int3 lhs, int3 rhs)
	{
		return new int3(lhs.x & rhs.x, lhs.y & rhs.y, lhs.z & rhs.z);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static int3 operator &(int3 lhs, int rhs)
	{
		return new int3(lhs.x & rhs, lhs.y & rhs, lhs.z & rhs);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static int3 operator &(int lhs, int3 rhs)
	{
		return new int3(lhs & rhs.x, lhs & rhs.y, lhs & rhs.z);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static int3 operator |(int3 lhs, int3 rhs)
	{
		return new int3(lhs.x | rhs.x, lhs.y | rhs.y, lhs.z | rhs.z);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static int3 operator |(int3 lhs, int rhs)
	{
		return new int3(lhs.x | rhs, lhs.y | rhs, lhs.z | rhs);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static int3 operator |(int lhs, int3 rhs)
	{
		return new int3(lhs | rhs.x, lhs | rhs.y, lhs | rhs.z);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static int3 operator ^(int3 lhs, int3 rhs)
	{
		return new int3(lhs.x ^ rhs.x, lhs.y ^ rhs.y, lhs.z ^ rhs.z);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static int3 operator ^(int3 lhs, int rhs)
	{
		return new int3(lhs.x ^ rhs, lhs.y ^ rhs, lhs.z ^ rhs);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static int3 operator ^(int lhs, int3 rhs)
	{
		return new int3(lhs ^ rhs.x, lhs ^ rhs.y, lhs ^ rhs.z);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public bool Equals(int3 rhs)
	{
		if (x == rhs.x && y == rhs.y)
		{
			return z == rhs.z;
		}
		return false;
	}

	public override bool Equals(object o)
	{
		return Equals((int3)o);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public override int GetHashCode()
	{
		return (int)math.hash(this);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public override string ToString()
	{
		return $"int3({x}, {y}, {z})";
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public string ToString(string format, IFormatProvider formatProvider)
	{
		return $"int3({x.ToString(format, formatProvider)}, {y.ToString(format, formatProvider)}, {z.ToString(format, formatProvider)})";
	}
}
