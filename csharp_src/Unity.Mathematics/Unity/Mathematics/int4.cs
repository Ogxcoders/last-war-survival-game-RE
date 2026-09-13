using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace Unity.Mathematics;

[Serializable]
[DebuggerTypeProxy(typeof(DebuggerProxy))]
public struct int4 : IEquatable<int4>, IFormattable
{
	internal sealed class DebuggerProxy
	{
		public int x;

		public int y;

		public int z;

		public int w;

		public DebuggerProxy(int4 v)
		{
			x = v.x;
			y = v.y;
			z = v.z;
			w = v.w;
		}
	}

	public int x;

	public int y;

	public int z;

	public int w;

	public static readonly int4 zero;

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
	public int4 xxxw
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new int4(x, x, x, w);
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
	public int4 xxyw
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new int4(x, x, y, w);
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
	public int4 xxzw
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new int4(x, x, z, w);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public int4 xxwx
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new int4(x, x, w, x);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public int4 xxwy
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new int4(x, x, w, y);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public int4 xxwz
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new int4(x, x, w, z);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public int4 xxww
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new int4(x, x, w, w);
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
	public int4 xyxw
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new int4(x, y, x, w);
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
	public int4 xyyw
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new int4(x, y, y, w);
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
	public int4 xyzw
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new int4(x, y, z, w);
		}
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		set
		{
			x = value.x;
			y = value.y;
			z = value.z;
			w = value.w;
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public int4 xywx
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new int4(x, y, w, x);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public int4 xywy
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new int4(x, y, w, y);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public int4 xywz
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new int4(x, y, w, z);
		}
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		set
		{
			x = value.x;
			y = value.y;
			w = value.z;
			z = value.w;
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public int4 xyww
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new int4(x, y, w, w);
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
	public int4 xzxw
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new int4(x, z, x, w);
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
	public int4 xzyw
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new int4(x, z, y, w);
		}
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		set
		{
			x = value.x;
			z = value.y;
			y = value.z;
			w = value.w;
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
	public int4 xzzw
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new int4(x, z, z, w);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public int4 xzwx
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new int4(x, z, w, x);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public int4 xzwy
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new int4(x, z, w, y);
		}
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		set
		{
			x = value.x;
			z = value.y;
			w = value.z;
			y = value.w;
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public int4 xzwz
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new int4(x, z, w, z);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public int4 xzww
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new int4(x, z, w, w);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public int4 xwxx
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new int4(x, w, x, x);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public int4 xwxy
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new int4(x, w, x, y);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public int4 xwxz
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new int4(x, w, x, z);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public int4 xwxw
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new int4(x, w, x, w);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public int4 xwyx
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new int4(x, w, y, x);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public int4 xwyy
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new int4(x, w, y, y);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public int4 xwyz
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new int4(x, w, y, z);
		}
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		set
		{
			x = value.x;
			w = value.y;
			y = value.z;
			z = value.w;
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public int4 xwyw
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new int4(x, w, y, w);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public int4 xwzx
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new int4(x, w, z, x);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public int4 xwzy
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new int4(x, w, z, y);
		}
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		set
		{
			x = value.x;
			w = value.y;
			z = value.z;
			y = value.w;
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public int4 xwzz
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new int4(x, w, z, z);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public int4 xwzw
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new int4(x, w, z, w);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public int4 xwwx
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new int4(x, w, w, x);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public int4 xwwy
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new int4(x, w, w, y);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public int4 xwwz
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new int4(x, w, w, z);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public int4 xwww
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new int4(x, w, w, w);
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
	public int4 yxxw
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new int4(y, x, x, w);
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
	public int4 yxyw
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new int4(y, x, y, w);
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
	public int4 yxzw
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new int4(y, x, z, w);
		}
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		set
		{
			y = value.x;
			x = value.y;
			z = value.z;
			w = value.w;
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public int4 yxwx
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new int4(y, x, w, x);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public int4 yxwy
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new int4(y, x, w, y);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public int4 yxwz
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new int4(y, x, w, z);
		}
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		set
		{
			y = value.x;
			x = value.y;
			w = value.z;
			z = value.w;
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public int4 yxww
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new int4(y, x, w, w);
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
	public int4 yyxw
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new int4(y, y, x, w);
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
	public int4 yyyw
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new int4(y, y, y, w);
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
	public int4 yyzw
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new int4(y, y, z, w);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public int4 yywx
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new int4(y, y, w, x);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public int4 yywy
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new int4(y, y, w, y);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public int4 yywz
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new int4(y, y, w, z);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public int4 yyww
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new int4(y, y, w, w);
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
	public int4 yzxw
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new int4(y, z, x, w);
		}
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		set
		{
			y = value.x;
			z = value.y;
			x = value.z;
			w = value.w;
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
	public int4 yzyw
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new int4(y, z, y, w);
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
	public int4 yzzw
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new int4(y, z, z, w);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public int4 yzwx
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new int4(y, z, w, x);
		}
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		set
		{
			y = value.x;
			z = value.y;
			w = value.z;
			x = value.w;
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public int4 yzwy
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new int4(y, z, w, y);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public int4 yzwz
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new int4(y, z, w, z);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public int4 yzww
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new int4(y, z, w, w);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public int4 ywxx
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new int4(y, w, x, x);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public int4 ywxy
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new int4(y, w, x, y);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public int4 ywxz
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new int4(y, w, x, z);
		}
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		set
		{
			y = value.x;
			w = value.y;
			x = value.z;
			z = value.w;
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public int4 ywxw
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new int4(y, w, x, w);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public int4 ywyx
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new int4(y, w, y, x);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public int4 ywyy
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new int4(y, w, y, y);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public int4 ywyz
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new int4(y, w, y, z);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public int4 ywyw
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new int4(y, w, y, w);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public int4 ywzx
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new int4(y, w, z, x);
		}
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		set
		{
			y = value.x;
			w = value.y;
			z = value.z;
			x = value.w;
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public int4 ywzy
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new int4(y, w, z, y);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public int4 ywzz
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new int4(y, w, z, z);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public int4 ywzw
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new int4(y, w, z, w);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public int4 ywwx
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new int4(y, w, w, x);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public int4 ywwy
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new int4(y, w, w, y);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public int4 ywwz
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new int4(y, w, w, z);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public int4 ywww
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new int4(y, w, w, w);
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
	public int4 zxxw
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new int4(z, x, x, w);
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
	public int4 zxyw
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new int4(z, x, y, w);
		}
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		set
		{
			z = value.x;
			x = value.y;
			y = value.z;
			w = value.w;
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
	public int4 zxzw
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new int4(z, x, z, w);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public int4 zxwx
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new int4(z, x, w, x);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public int4 zxwy
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new int4(z, x, w, y);
		}
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		set
		{
			z = value.x;
			x = value.y;
			w = value.z;
			y = value.w;
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public int4 zxwz
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new int4(z, x, w, z);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public int4 zxww
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new int4(z, x, w, w);
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
	public int4 zyxw
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new int4(z, y, x, w);
		}
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		set
		{
			z = value.x;
			y = value.y;
			x = value.z;
			w = value.w;
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
	public int4 zyyw
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new int4(z, y, y, w);
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
	public int4 zyzw
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new int4(z, y, z, w);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public int4 zywx
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new int4(z, y, w, x);
		}
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		set
		{
			z = value.x;
			y = value.y;
			w = value.z;
			x = value.w;
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public int4 zywy
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new int4(z, y, w, y);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public int4 zywz
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new int4(z, y, w, z);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public int4 zyww
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new int4(z, y, w, w);
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
	public int4 zzxw
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new int4(z, z, x, w);
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
	public int4 zzyw
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new int4(z, z, y, w);
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
	public int4 zzzw
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new int4(z, z, z, w);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public int4 zzwx
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new int4(z, z, w, x);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public int4 zzwy
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new int4(z, z, w, y);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public int4 zzwz
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new int4(z, z, w, z);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public int4 zzww
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new int4(z, z, w, w);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public int4 zwxx
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new int4(z, w, x, x);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public int4 zwxy
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new int4(z, w, x, y);
		}
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		set
		{
			z = value.x;
			w = value.y;
			x = value.z;
			y = value.w;
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public int4 zwxz
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new int4(z, w, x, z);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public int4 zwxw
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new int4(z, w, x, w);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public int4 zwyx
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new int4(z, w, y, x);
		}
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		set
		{
			z = value.x;
			w = value.y;
			y = value.z;
			x = value.w;
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public int4 zwyy
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new int4(z, w, y, y);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public int4 zwyz
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new int4(z, w, y, z);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public int4 zwyw
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new int4(z, w, y, w);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public int4 zwzx
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new int4(z, w, z, x);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public int4 zwzy
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new int4(z, w, z, y);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public int4 zwzz
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new int4(z, w, z, z);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public int4 zwzw
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new int4(z, w, z, w);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public int4 zwwx
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new int4(z, w, w, x);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public int4 zwwy
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new int4(z, w, w, y);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public int4 zwwz
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new int4(z, w, w, z);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public int4 zwww
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new int4(z, w, w, w);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public int4 wxxx
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new int4(w, x, x, x);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public int4 wxxy
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new int4(w, x, x, y);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public int4 wxxz
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new int4(w, x, x, z);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public int4 wxxw
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new int4(w, x, x, w);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public int4 wxyx
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new int4(w, x, y, x);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public int4 wxyy
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new int4(w, x, y, y);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public int4 wxyz
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new int4(w, x, y, z);
		}
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		set
		{
			w = value.x;
			x = value.y;
			y = value.z;
			z = value.w;
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public int4 wxyw
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new int4(w, x, y, w);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public int4 wxzx
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new int4(w, x, z, x);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public int4 wxzy
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new int4(w, x, z, y);
		}
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		set
		{
			w = value.x;
			x = value.y;
			z = value.z;
			y = value.w;
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public int4 wxzz
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new int4(w, x, z, z);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public int4 wxzw
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new int4(w, x, z, w);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public int4 wxwx
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new int4(w, x, w, x);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public int4 wxwy
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new int4(w, x, w, y);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public int4 wxwz
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new int4(w, x, w, z);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public int4 wxww
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new int4(w, x, w, w);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public int4 wyxx
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new int4(w, y, x, x);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public int4 wyxy
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new int4(w, y, x, y);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public int4 wyxz
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new int4(w, y, x, z);
		}
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		set
		{
			w = value.x;
			y = value.y;
			x = value.z;
			z = value.w;
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public int4 wyxw
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new int4(w, y, x, w);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public int4 wyyx
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new int4(w, y, y, x);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public int4 wyyy
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new int4(w, y, y, y);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public int4 wyyz
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new int4(w, y, y, z);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public int4 wyyw
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new int4(w, y, y, w);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public int4 wyzx
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new int4(w, y, z, x);
		}
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		set
		{
			w = value.x;
			y = value.y;
			z = value.z;
			x = value.w;
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public int4 wyzy
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new int4(w, y, z, y);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public int4 wyzz
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new int4(w, y, z, z);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public int4 wyzw
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new int4(w, y, z, w);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public int4 wywx
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new int4(w, y, w, x);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public int4 wywy
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new int4(w, y, w, y);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public int4 wywz
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new int4(w, y, w, z);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public int4 wyww
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new int4(w, y, w, w);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public int4 wzxx
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new int4(w, z, x, x);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public int4 wzxy
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new int4(w, z, x, y);
		}
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		set
		{
			w = value.x;
			z = value.y;
			x = value.z;
			y = value.w;
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public int4 wzxz
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new int4(w, z, x, z);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public int4 wzxw
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new int4(w, z, x, w);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public int4 wzyx
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new int4(w, z, y, x);
		}
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		set
		{
			w = value.x;
			z = value.y;
			y = value.z;
			x = value.w;
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public int4 wzyy
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new int4(w, z, y, y);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public int4 wzyz
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new int4(w, z, y, z);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public int4 wzyw
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new int4(w, z, y, w);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public int4 wzzx
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new int4(w, z, z, x);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public int4 wzzy
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new int4(w, z, z, y);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public int4 wzzz
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new int4(w, z, z, z);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public int4 wzzw
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new int4(w, z, z, w);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public int4 wzwx
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new int4(w, z, w, x);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public int4 wzwy
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new int4(w, z, w, y);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public int4 wzwz
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new int4(w, z, w, z);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public int4 wzww
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new int4(w, z, w, w);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public int4 wwxx
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new int4(w, w, x, x);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public int4 wwxy
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new int4(w, w, x, y);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public int4 wwxz
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new int4(w, w, x, z);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public int4 wwxw
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new int4(w, w, x, w);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public int4 wwyx
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new int4(w, w, y, x);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public int4 wwyy
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new int4(w, w, y, y);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public int4 wwyz
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new int4(w, w, y, z);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public int4 wwyw
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new int4(w, w, y, w);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public int4 wwzx
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new int4(w, w, z, x);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public int4 wwzy
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new int4(w, w, z, y);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public int4 wwzz
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new int4(w, w, z, z);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public int4 wwzw
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new int4(w, w, z, w);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public int4 wwwx
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new int4(w, w, w, x);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public int4 wwwy
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new int4(w, w, w, y);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public int4 wwwz
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new int4(w, w, w, z);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public int4 wwww
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new int4(w, w, w, w);
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
	public int3 xxw
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new int3(x, x, w);
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
	public int3 xyw
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new int3(x, y, w);
		}
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		set
		{
			x = value.x;
			y = value.y;
			w = value.z;
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
	public int3 xzw
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new int3(x, z, w);
		}
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		set
		{
			x = value.x;
			z = value.y;
			w = value.z;
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public int3 xwx
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new int3(x, w, x);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public int3 xwy
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new int3(x, w, y);
		}
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		set
		{
			x = value.x;
			w = value.y;
			y = value.z;
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public int3 xwz
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new int3(x, w, z);
		}
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		set
		{
			x = value.x;
			w = value.y;
			z = value.z;
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public int3 xww
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new int3(x, w, w);
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
	public int3 yxw
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new int3(y, x, w);
		}
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		set
		{
			y = value.x;
			x = value.y;
			w = value.z;
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
	public int3 yyw
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new int3(y, y, w);
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
	public int3 yzw
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new int3(y, z, w);
		}
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		set
		{
			y = value.x;
			z = value.y;
			w = value.z;
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public int3 ywx
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new int3(y, w, x);
		}
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		set
		{
			y = value.x;
			w = value.y;
			x = value.z;
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public int3 ywy
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new int3(y, w, y);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public int3 ywz
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new int3(y, w, z);
		}
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		set
		{
			y = value.x;
			w = value.y;
			z = value.z;
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public int3 yww
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new int3(y, w, w);
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
	public int3 zxw
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new int3(z, x, w);
		}
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		set
		{
			z = value.x;
			x = value.y;
			w = value.z;
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
	public int3 zyw
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new int3(z, y, w);
		}
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		set
		{
			z = value.x;
			y = value.y;
			w = value.z;
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
	public int3 zzw
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new int3(z, z, w);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public int3 zwx
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new int3(z, w, x);
		}
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		set
		{
			z = value.x;
			w = value.y;
			x = value.z;
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public int3 zwy
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new int3(z, w, y);
		}
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		set
		{
			z = value.x;
			w = value.y;
			y = value.z;
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public int3 zwz
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new int3(z, w, z);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public int3 zww
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new int3(z, w, w);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public int3 wxx
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new int3(w, x, x);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public int3 wxy
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new int3(w, x, y);
		}
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		set
		{
			w = value.x;
			x = value.y;
			y = value.z;
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public int3 wxz
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new int3(w, x, z);
		}
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		set
		{
			w = value.x;
			x = value.y;
			z = value.z;
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public int3 wxw
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new int3(w, x, w);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public int3 wyx
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new int3(w, y, x);
		}
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		set
		{
			w = value.x;
			y = value.y;
			x = value.z;
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public int3 wyy
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new int3(w, y, y);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public int3 wyz
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new int3(w, y, z);
		}
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		set
		{
			w = value.x;
			y = value.y;
			z = value.z;
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public int3 wyw
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new int3(w, y, w);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public int3 wzx
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new int3(w, z, x);
		}
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		set
		{
			w = value.x;
			z = value.y;
			x = value.z;
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public int3 wzy
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new int3(w, z, y);
		}
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		set
		{
			w = value.x;
			z = value.y;
			y = value.z;
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public int3 wzz
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new int3(w, z, z);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public int3 wzw
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new int3(w, z, w);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public int3 wwx
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new int3(w, w, x);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public int3 wwy
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new int3(w, w, y);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public int3 wwz
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new int3(w, w, z);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public int3 www
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new int3(w, w, w);
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
	public int2 xw
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new int2(x, w);
		}
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		set
		{
			x = value.x;
			w = value.y;
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
	public int2 yw
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new int2(y, w);
		}
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		set
		{
			y = value.x;
			w = value.y;
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

	[EditorBrowsable(EditorBrowsableState.Never)]
	public int2 zw
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new int2(z, w);
		}
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		set
		{
			z = value.x;
			w = value.y;
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public int2 wx
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new int2(w, x);
		}
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		set
		{
			w = value.x;
			x = value.y;
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public int2 wy
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new int2(w, y);
		}
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		set
		{
			w = value.x;
			y = value.y;
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public int2 wz
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new int2(w, z);
		}
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		set
		{
			w = value.x;
			z = value.y;
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public int2 ww
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new int2(w, w);
		}
	}

	public unsafe int this[int index]
	{
		get
		{
			fixed (int4* ptr = &this)
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
	public int4(int x, int y, int z, int w)
	{
		this.x = x;
		this.y = y;
		this.z = z;
		this.w = w;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public int4(int x, int y, int2 zw)
	{
		this.x = x;
		this.y = y;
		z = zw.x;
		w = zw.y;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public int4(int x, int2 yz, int w)
	{
		this.x = x;
		y = yz.x;
		z = yz.y;
		this.w = w;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public int4(int x, int3 yzw)
	{
		this.x = x;
		y = yzw.x;
		z = yzw.y;
		w = yzw.z;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public int4(int2 xy, int z, int w)
	{
		x = xy.x;
		y = xy.y;
		this.z = z;
		this.w = w;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public int4(int2 xy, int2 zw)
	{
		x = xy.x;
		y = xy.y;
		z = zw.x;
		w = zw.y;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public int4(int3 xyz, int w)
	{
		x = xyz.x;
		y = xyz.y;
		z = xyz.z;
		this.w = w;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public int4(int4 xyzw)
	{
		x = xyzw.x;
		y = xyzw.y;
		z = xyzw.z;
		w = xyzw.w;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public int4(int v)
	{
		x = v;
		y = v;
		z = v;
		w = v;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public int4(bool v)
	{
		x = (v ? 1 : 0);
		y = (v ? 1 : 0);
		z = (v ? 1 : 0);
		w = (v ? 1 : 0);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public int4(bool4 v)
	{
		x = (v.x ? 1 : 0);
		y = (v.y ? 1 : 0);
		z = (v.z ? 1 : 0);
		w = (v.w ? 1 : 0);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public int4(uint v)
	{
		x = (int)v;
		y = (int)v;
		z = (int)v;
		w = (int)v;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public int4(uint4 v)
	{
		x = (int)v.x;
		y = (int)v.y;
		z = (int)v.z;
		w = (int)v.w;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public int4(float v)
	{
		x = (int)v;
		y = (int)v;
		z = (int)v;
		w = (int)v;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public int4(float4 v)
	{
		x = (int)v.x;
		y = (int)v.y;
		z = (int)v.z;
		w = (int)v.w;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public int4(double v)
	{
		x = (int)v;
		y = (int)v;
		z = (int)v;
		w = (int)v;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public int4(double4 v)
	{
		x = (int)v.x;
		y = (int)v.y;
		z = (int)v.z;
		w = (int)v.w;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static implicit operator int4(int v)
	{
		return new int4(v);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static explicit operator int4(bool v)
	{
		return new int4(v);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static explicit operator int4(bool4 v)
	{
		return new int4(v);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static explicit operator int4(uint v)
	{
		return new int4(v);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static explicit operator int4(uint4 v)
	{
		return new int4(v);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static explicit operator int4(float v)
	{
		return new int4(v);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static explicit operator int4(float4 v)
	{
		return new int4(v);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static explicit operator int4(double v)
	{
		return new int4(v);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static explicit operator int4(double4 v)
	{
		return new int4(v);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static int4 operator *(int4 lhs, int4 rhs)
	{
		return new int4(lhs.x * rhs.x, lhs.y * rhs.y, lhs.z * rhs.z, lhs.w * rhs.w);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static int4 operator *(int4 lhs, int rhs)
	{
		return new int4(lhs.x * rhs, lhs.y * rhs, lhs.z * rhs, lhs.w * rhs);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static int4 operator *(int lhs, int4 rhs)
	{
		return new int4(lhs * rhs.x, lhs * rhs.y, lhs * rhs.z, lhs * rhs.w);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static int4 operator +(int4 lhs, int4 rhs)
	{
		return new int4(lhs.x + rhs.x, lhs.y + rhs.y, lhs.z + rhs.z, lhs.w + rhs.w);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static int4 operator +(int4 lhs, int rhs)
	{
		return new int4(lhs.x + rhs, lhs.y + rhs, lhs.z + rhs, lhs.w + rhs);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static int4 operator +(int lhs, int4 rhs)
	{
		return new int4(lhs + rhs.x, lhs + rhs.y, lhs + rhs.z, lhs + rhs.w);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static int4 operator -(int4 lhs, int4 rhs)
	{
		return new int4(lhs.x - rhs.x, lhs.y - rhs.y, lhs.z - rhs.z, lhs.w - rhs.w);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static int4 operator -(int4 lhs, int rhs)
	{
		return new int4(lhs.x - rhs, lhs.y - rhs, lhs.z - rhs, lhs.w - rhs);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static int4 operator -(int lhs, int4 rhs)
	{
		return new int4(lhs - rhs.x, lhs - rhs.y, lhs - rhs.z, lhs - rhs.w);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static int4 operator /(int4 lhs, int4 rhs)
	{
		return new int4(lhs.x / rhs.x, lhs.y / rhs.y, lhs.z / rhs.z, lhs.w / rhs.w);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static int4 operator /(int4 lhs, int rhs)
	{
		return new int4(lhs.x / rhs, lhs.y / rhs, lhs.z / rhs, lhs.w / rhs);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static int4 operator /(int lhs, int4 rhs)
	{
		return new int4(lhs / rhs.x, lhs / rhs.y, lhs / rhs.z, lhs / rhs.w);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static int4 operator %(int4 lhs, int4 rhs)
	{
		return new int4(lhs.x % rhs.x, lhs.y % rhs.y, lhs.z % rhs.z, lhs.w % rhs.w);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static int4 operator %(int4 lhs, int rhs)
	{
		return new int4(lhs.x % rhs, lhs.y % rhs, lhs.z % rhs, lhs.w % rhs);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static int4 operator %(int lhs, int4 rhs)
	{
		return new int4(lhs % rhs.x, lhs % rhs.y, lhs % rhs.z, lhs % rhs.w);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static int4 operator ++(int4 val)
	{
		return new int4(++val.x, ++val.y, ++val.z, ++val.w);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static int4 operator --(int4 val)
	{
		return new int4(--val.x, --val.y, --val.z, --val.w);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool4 operator <(int4 lhs, int4 rhs)
	{
		return new bool4(lhs.x < rhs.x, lhs.y < rhs.y, lhs.z < rhs.z, lhs.w < rhs.w);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool4 operator <(int4 lhs, int rhs)
	{
		return new bool4(lhs.x < rhs, lhs.y < rhs, lhs.z < rhs, lhs.w < rhs);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool4 operator <(int lhs, int4 rhs)
	{
		return new bool4(lhs < rhs.x, lhs < rhs.y, lhs < rhs.z, lhs < rhs.w);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool4 operator <=(int4 lhs, int4 rhs)
	{
		return new bool4(lhs.x <= rhs.x, lhs.y <= rhs.y, lhs.z <= rhs.z, lhs.w <= rhs.w);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool4 operator <=(int4 lhs, int rhs)
	{
		return new bool4(lhs.x <= rhs, lhs.y <= rhs, lhs.z <= rhs, lhs.w <= rhs);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool4 operator <=(int lhs, int4 rhs)
	{
		return new bool4(lhs <= rhs.x, lhs <= rhs.y, lhs <= rhs.z, lhs <= rhs.w);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool4 operator >(int4 lhs, int4 rhs)
	{
		return new bool4(lhs.x > rhs.x, lhs.y > rhs.y, lhs.z > rhs.z, lhs.w > rhs.w);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool4 operator >(int4 lhs, int rhs)
	{
		return new bool4(lhs.x > rhs, lhs.y > rhs, lhs.z > rhs, lhs.w > rhs);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool4 operator >(int lhs, int4 rhs)
	{
		return new bool4(lhs > rhs.x, lhs > rhs.y, lhs > rhs.z, lhs > rhs.w);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool4 operator >=(int4 lhs, int4 rhs)
	{
		return new bool4(lhs.x >= rhs.x, lhs.y >= rhs.y, lhs.z >= rhs.z, lhs.w >= rhs.w);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool4 operator >=(int4 lhs, int rhs)
	{
		return new bool4(lhs.x >= rhs, lhs.y >= rhs, lhs.z >= rhs, lhs.w >= rhs);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool4 operator >=(int lhs, int4 rhs)
	{
		return new bool4(lhs >= rhs.x, lhs >= rhs.y, lhs >= rhs.z, lhs >= rhs.w);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static int4 operator -(int4 val)
	{
		return new int4(-val.x, -val.y, -val.z, -val.w);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static int4 operator +(int4 val)
	{
		return new int4(val.x, val.y, val.z, val.w);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static int4 operator <<(int4 x, int n)
	{
		return new int4(x.x << n, x.y << n, x.z << n, x.w << n);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static int4 operator >>(int4 x, int n)
	{
		return new int4(x.x >> n, x.y >> n, x.z >> n, x.w >> n);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool4 operator ==(int4 lhs, int4 rhs)
	{
		return new bool4(lhs.x == rhs.x, lhs.y == rhs.y, lhs.z == rhs.z, lhs.w == rhs.w);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool4 operator ==(int4 lhs, int rhs)
	{
		return new bool4(lhs.x == rhs, lhs.y == rhs, lhs.z == rhs, lhs.w == rhs);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool4 operator ==(int lhs, int4 rhs)
	{
		return new bool4(lhs == rhs.x, lhs == rhs.y, lhs == rhs.z, lhs == rhs.w);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool4 operator !=(int4 lhs, int4 rhs)
	{
		return new bool4(lhs.x != rhs.x, lhs.y != rhs.y, lhs.z != rhs.z, lhs.w != rhs.w);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool4 operator !=(int4 lhs, int rhs)
	{
		return new bool4(lhs.x != rhs, lhs.y != rhs, lhs.z != rhs, lhs.w != rhs);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool4 operator !=(int lhs, int4 rhs)
	{
		return new bool4(lhs != rhs.x, lhs != rhs.y, lhs != rhs.z, lhs != rhs.w);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static int4 operator ~(int4 val)
	{
		return new int4(~val.x, ~val.y, ~val.z, ~val.w);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static int4 operator &(int4 lhs, int4 rhs)
	{
		return new int4(lhs.x & rhs.x, lhs.y & rhs.y, lhs.z & rhs.z, lhs.w & rhs.w);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static int4 operator &(int4 lhs, int rhs)
	{
		return new int4(lhs.x & rhs, lhs.y & rhs, lhs.z & rhs, lhs.w & rhs);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static int4 operator &(int lhs, int4 rhs)
	{
		return new int4(lhs & rhs.x, lhs & rhs.y, lhs & rhs.z, lhs & rhs.w);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static int4 operator |(int4 lhs, int4 rhs)
	{
		return new int4(lhs.x | rhs.x, lhs.y | rhs.y, lhs.z | rhs.z, lhs.w | rhs.w);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static int4 operator |(int4 lhs, int rhs)
	{
		return new int4(lhs.x | rhs, lhs.y | rhs, lhs.z | rhs, lhs.w | rhs);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static int4 operator |(int lhs, int4 rhs)
	{
		return new int4(lhs | rhs.x, lhs | rhs.y, lhs | rhs.z, lhs | rhs.w);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static int4 operator ^(int4 lhs, int4 rhs)
	{
		return new int4(lhs.x ^ rhs.x, lhs.y ^ rhs.y, lhs.z ^ rhs.z, lhs.w ^ rhs.w);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static int4 operator ^(int4 lhs, int rhs)
	{
		return new int4(lhs.x ^ rhs, lhs.y ^ rhs, lhs.z ^ rhs, lhs.w ^ rhs);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static int4 operator ^(int lhs, int4 rhs)
	{
		return new int4(lhs ^ rhs.x, lhs ^ rhs.y, lhs ^ rhs.z, lhs ^ rhs.w);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public bool Equals(int4 rhs)
	{
		if (x == rhs.x && y == rhs.y && z == rhs.z)
		{
			return w == rhs.w;
		}
		return false;
	}

	public override bool Equals(object o)
	{
		return Equals((int4)o);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public override int GetHashCode()
	{
		return (int)math.hash(this);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public override string ToString()
	{
		return $"int4({x}, {y}, {z}, {w})";
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public string ToString(string format, IFormatProvider formatProvider)
	{
		return $"int4({x.ToString(format, formatProvider)}, {y.ToString(format, formatProvider)}, {z.ToString(format, formatProvider)}, {w.ToString(format, formatProvider)})";
	}
}
