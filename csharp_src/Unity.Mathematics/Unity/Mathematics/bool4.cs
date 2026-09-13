using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Unity.Mathematics;

[Serializable]
[DebuggerTypeProxy(typeof(DebuggerProxy))]
public struct bool4 : IEquatable<bool4>
{
	internal sealed class DebuggerProxy
	{
		public bool x;

		public bool y;

		public bool z;

		public bool w;

		public DebuggerProxy(bool4 v)
		{
			x = v.x;
			y = v.y;
			z = v.z;
			w = v.w;
		}
	}

	[MarshalAs(UnmanagedType.U1)]
	public bool x;

	[MarshalAs(UnmanagedType.U1)]
	public bool y;

	[MarshalAs(UnmanagedType.U1)]
	public bool z;

	[MarshalAs(UnmanagedType.U1)]
	public bool w;

	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool4 xxxx
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new bool4(x, x, x, x);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool4 xxxy
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new bool4(x, x, x, y);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool4 xxxz
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new bool4(x, x, x, z);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool4 xxxw
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new bool4(x, x, x, w);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool4 xxyx
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new bool4(x, x, y, x);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool4 xxyy
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new bool4(x, x, y, y);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool4 xxyz
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new bool4(x, x, y, z);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool4 xxyw
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new bool4(x, x, y, w);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool4 xxzx
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new bool4(x, x, z, x);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool4 xxzy
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new bool4(x, x, z, y);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool4 xxzz
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new bool4(x, x, z, z);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool4 xxzw
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new bool4(x, x, z, w);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool4 xxwx
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new bool4(x, x, w, x);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool4 xxwy
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new bool4(x, x, w, y);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool4 xxwz
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new bool4(x, x, w, z);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool4 xxww
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new bool4(x, x, w, w);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool4 xyxx
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new bool4(x, y, x, x);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool4 xyxy
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new bool4(x, y, x, y);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool4 xyxz
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new bool4(x, y, x, z);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool4 xyxw
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new bool4(x, y, x, w);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool4 xyyx
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new bool4(x, y, y, x);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool4 xyyy
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new bool4(x, y, y, y);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool4 xyyz
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new bool4(x, y, y, z);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool4 xyyw
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new bool4(x, y, y, w);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool4 xyzx
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new bool4(x, y, z, x);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool4 xyzy
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new bool4(x, y, z, y);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool4 xyzz
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new bool4(x, y, z, z);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool4 xyzw
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new bool4(x, y, z, w);
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
	public bool4 xywx
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new bool4(x, y, w, x);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool4 xywy
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new bool4(x, y, w, y);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool4 xywz
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new bool4(x, y, w, z);
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
	public bool4 xyww
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new bool4(x, y, w, w);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool4 xzxx
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new bool4(x, z, x, x);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool4 xzxy
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new bool4(x, z, x, y);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool4 xzxz
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new bool4(x, z, x, z);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool4 xzxw
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new bool4(x, z, x, w);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool4 xzyx
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new bool4(x, z, y, x);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool4 xzyy
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new bool4(x, z, y, y);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool4 xzyz
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new bool4(x, z, y, z);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool4 xzyw
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new bool4(x, z, y, w);
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
	public bool4 xzzx
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new bool4(x, z, z, x);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool4 xzzy
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new bool4(x, z, z, y);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool4 xzzz
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new bool4(x, z, z, z);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool4 xzzw
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new bool4(x, z, z, w);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool4 xzwx
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new bool4(x, z, w, x);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool4 xzwy
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new bool4(x, z, w, y);
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
	public bool4 xzwz
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new bool4(x, z, w, z);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool4 xzww
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new bool4(x, z, w, w);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool4 xwxx
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new bool4(x, w, x, x);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool4 xwxy
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new bool4(x, w, x, y);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool4 xwxz
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new bool4(x, w, x, z);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool4 xwxw
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new bool4(x, w, x, w);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool4 xwyx
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new bool4(x, w, y, x);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool4 xwyy
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new bool4(x, w, y, y);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool4 xwyz
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new bool4(x, w, y, z);
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
	public bool4 xwyw
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new bool4(x, w, y, w);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool4 xwzx
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new bool4(x, w, z, x);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool4 xwzy
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new bool4(x, w, z, y);
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
	public bool4 xwzz
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new bool4(x, w, z, z);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool4 xwzw
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new bool4(x, w, z, w);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool4 xwwx
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new bool4(x, w, w, x);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool4 xwwy
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new bool4(x, w, w, y);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool4 xwwz
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new bool4(x, w, w, z);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool4 xwww
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new bool4(x, w, w, w);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool4 yxxx
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new bool4(y, x, x, x);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool4 yxxy
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new bool4(y, x, x, y);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool4 yxxz
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new bool4(y, x, x, z);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool4 yxxw
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new bool4(y, x, x, w);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool4 yxyx
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new bool4(y, x, y, x);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool4 yxyy
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new bool4(y, x, y, y);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool4 yxyz
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new bool4(y, x, y, z);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool4 yxyw
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new bool4(y, x, y, w);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool4 yxzx
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new bool4(y, x, z, x);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool4 yxzy
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new bool4(y, x, z, y);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool4 yxzz
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new bool4(y, x, z, z);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool4 yxzw
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new bool4(y, x, z, w);
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
	public bool4 yxwx
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new bool4(y, x, w, x);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool4 yxwy
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new bool4(y, x, w, y);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool4 yxwz
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new bool4(y, x, w, z);
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
	public bool4 yxww
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new bool4(y, x, w, w);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool4 yyxx
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new bool4(y, y, x, x);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool4 yyxy
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new bool4(y, y, x, y);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool4 yyxz
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new bool4(y, y, x, z);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool4 yyxw
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new bool4(y, y, x, w);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool4 yyyx
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new bool4(y, y, y, x);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool4 yyyy
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new bool4(y, y, y, y);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool4 yyyz
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new bool4(y, y, y, z);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool4 yyyw
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new bool4(y, y, y, w);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool4 yyzx
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new bool4(y, y, z, x);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool4 yyzy
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new bool4(y, y, z, y);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool4 yyzz
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new bool4(y, y, z, z);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool4 yyzw
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new bool4(y, y, z, w);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool4 yywx
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new bool4(y, y, w, x);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool4 yywy
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new bool4(y, y, w, y);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool4 yywz
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new bool4(y, y, w, z);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool4 yyww
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new bool4(y, y, w, w);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool4 yzxx
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new bool4(y, z, x, x);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool4 yzxy
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new bool4(y, z, x, y);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool4 yzxz
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new bool4(y, z, x, z);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool4 yzxw
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new bool4(y, z, x, w);
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
	public bool4 yzyx
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new bool4(y, z, y, x);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool4 yzyy
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new bool4(y, z, y, y);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool4 yzyz
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new bool4(y, z, y, z);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool4 yzyw
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new bool4(y, z, y, w);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool4 yzzx
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new bool4(y, z, z, x);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool4 yzzy
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new bool4(y, z, z, y);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool4 yzzz
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new bool4(y, z, z, z);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool4 yzzw
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new bool4(y, z, z, w);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool4 yzwx
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new bool4(y, z, w, x);
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
	public bool4 yzwy
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new bool4(y, z, w, y);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool4 yzwz
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new bool4(y, z, w, z);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool4 yzww
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new bool4(y, z, w, w);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool4 ywxx
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new bool4(y, w, x, x);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool4 ywxy
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new bool4(y, w, x, y);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool4 ywxz
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new bool4(y, w, x, z);
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
	public bool4 ywxw
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new bool4(y, w, x, w);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool4 ywyx
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new bool4(y, w, y, x);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool4 ywyy
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new bool4(y, w, y, y);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool4 ywyz
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new bool4(y, w, y, z);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool4 ywyw
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new bool4(y, w, y, w);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool4 ywzx
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new bool4(y, w, z, x);
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
	public bool4 ywzy
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new bool4(y, w, z, y);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool4 ywzz
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new bool4(y, w, z, z);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool4 ywzw
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new bool4(y, w, z, w);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool4 ywwx
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new bool4(y, w, w, x);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool4 ywwy
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new bool4(y, w, w, y);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool4 ywwz
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new bool4(y, w, w, z);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool4 ywww
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new bool4(y, w, w, w);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool4 zxxx
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new bool4(z, x, x, x);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool4 zxxy
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new bool4(z, x, x, y);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool4 zxxz
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new bool4(z, x, x, z);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool4 zxxw
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new bool4(z, x, x, w);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool4 zxyx
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new bool4(z, x, y, x);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool4 zxyy
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new bool4(z, x, y, y);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool4 zxyz
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new bool4(z, x, y, z);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool4 zxyw
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new bool4(z, x, y, w);
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
	public bool4 zxzx
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new bool4(z, x, z, x);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool4 zxzy
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new bool4(z, x, z, y);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool4 zxzz
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new bool4(z, x, z, z);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool4 zxzw
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new bool4(z, x, z, w);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool4 zxwx
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new bool4(z, x, w, x);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool4 zxwy
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new bool4(z, x, w, y);
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
	public bool4 zxwz
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new bool4(z, x, w, z);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool4 zxww
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new bool4(z, x, w, w);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool4 zyxx
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new bool4(z, y, x, x);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool4 zyxy
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new bool4(z, y, x, y);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool4 zyxz
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new bool4(z, y, x, z);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool4 zyxw
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new bool4(z, y, x, w);
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
	public bool4 zyyx
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new bool4(z, y, y, x);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool4 zyyy
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new bool4(z, y, y, y);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool4 zyyz
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new bool4(z, y, y, z);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool4 zyyw
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new bool4(z, y, y, w);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool4 zyzx
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new bool4(z, y, z, x);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool4 zyzy
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new bool4(z, y, z, y);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool4 zyzz
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new bool4(z, y, z, z);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool4 zyzw
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new bool4(z, y, z, w);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool4 zywx
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new bool4(z, y, w, x);
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
	public bool4 zywy
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new bool4(z, y, w, y);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool4 zywz
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new bool4(z, y, w, z);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool4 zyww
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new bool4(z, y, w, w);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool4 zzxx
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new bool4(z, z, x, x);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool4 zzxy
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new bool4(z, z, x, y);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool4 zzxz
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new bool4(z, z, x, z);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool4 zzxw
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new bool4(z, z, x, w);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool4 zzyx
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new bool4(z, z, y, x);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool4 zzyy
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new bool4(z, z, y, y);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool4 zzyz
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new bool4(z, z, y, z);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool4 zzyw
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new bool4(z, z, y, w);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool4 zzzx
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new bool4(z, z, z, x);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool4 zzzy
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new bool4(z, z, z, y);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool4 zzzz
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new bool4(z, z, z, z);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool4 zzzw
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new bool4(z, z, z, w);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool4 zzwx
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new bool4(z, z, w, x);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool4 zzwy
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new bool4(z, z, w, y);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool4 zzwz
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new bool4(z, z, w, z);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool4 zzww
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new bool4(z, z, w, w);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool4 zwxx
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new bool4(z, w, x, x);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool4 zwxy
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new bool4(z, w, x, y);
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
	public bool4 zwxz
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new bool4(z, w, x, z);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool4 zwxw
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new bool4(z, w, x, w);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool4 zwyx
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new bool4(z, w, y, x);
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
	public bool4 zwyy
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new bool4(z, w, y, y);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool4 zwyz
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new bool4(z, w, y, z);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool4 zwyw
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new bool4(z, w, y, w);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool4 zwzx
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new bool4(z, w, z, x);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool4 zwzy
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new bool4(z, w, z, y);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool4 zwzz
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new bool4(z, w, z, z);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool4 zwzw
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new bool4(z, w, z, w);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool4 zwwx
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new bool4(z, w, w, x);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool4 zwwy
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new bool4(z, w, w, y);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool4 zwwz
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new bool4(z, w, w, z);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool4 zwww
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new bool4(z, w, w, w);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool4 wxxx
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new bool4(w, x, x, x);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool4 wxxy
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new bool4(w, x, x, y);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool4 wxxz
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new bool4(w, x, x, z);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool4 wxxw
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new bool4(w, x, x, w);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool4 wxyx
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new bool4(w, x, y, x);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool4 wxyy
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new bool4(w, x, y, y);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool4 wxyz
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new bool4(w, x, y, z);
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
	public bool4 wxyw
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new bool4(w, x, y, w);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool4 wxzx
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new bool4(w, x, z, x);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool4 wxzy
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new bool4(w, x, z, y);
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
	public bool4 wxzz
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new bool4(w, x, z, z);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool4 wxzw
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new bool4(w, x, z, w);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool4 wxwx
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new bool4(w, x, w, x);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool4 wxwy
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new bool4(w, x, w, y);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool4 wxwz
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new bool4(w, x, w, z);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool4 wxww
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new bool4(w, x, w, w);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool4 wyxx
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new bool4(w, y, x, x);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool4 wyxy
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new bool4(w, y, x, y);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool4 wyxz
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new bool4(w, y, x, z);
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
	public bool4 wyxw
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new bool4(w, y, x, w);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool4 wyyx
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new bool4(w, y, y, x);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool4 wyyy
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new bool4(w, y, y, y);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool4 wyyz
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new bool4(w, y, y, z);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool4 wyyw
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new bool4(w, y, y, w);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool4 wyzx
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new bool4(w, y, z, x);
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
	public bool4 wyzy
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new bool4(w, y, z, y);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool4 wyzz
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new bool4(w, y, z, z);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool4 wyzw
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new bool4(w, y, z, w);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool4 wywx
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new bool4(w, y, w, x);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool4 wywy
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new bool4(w, y, w, y);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool4 wywz
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new bool4(w, y, w, z);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool4 wyww
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new bool4(w, y, w, w);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool4 wzxx
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new bool4(w, z, x, x);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool4 wzxy
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new bool4(w, z, x, y);
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
	public bool4 wzxz
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new bool4(w, z, x, z);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool4 wzxw
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new bool4(w, z, x, w);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool4 wzyx
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new bool4(w, z, y, x);
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
	public bool4 wzyy
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new bool4(w, z, y, y);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool4 wzyz
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new bool4(w, z, y, z);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool4 wzyw
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new bool4(w, z, y, w);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool4 wzzx
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new bool4(w, z, z, x);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool4 wzzy
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new bool4(w, z, z, y);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool4 wzzz
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new bool4(w, z, z, z);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool4 wzzw
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new bool4(w, z, z, w);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool4 wzwx
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new bool4(w, z, w, x);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool4 wzwy
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new bool4(w, z, w, y);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool4 wzwz
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new bool4(w, z, w, z);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool4 wzww
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new bool4(w, z, w, w);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool4 wwxx
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new bool4(w, w, x, x);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool4 wwxy
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new bool4(w, w, x, y);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool4 wwxz
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new bool4(w, w, x, z);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool4 wwxw
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new bool4(w, w, x, w);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool4 wwyx
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new bool4(w, w, y, x);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool4 wwyy
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new bool4(w, w, y, y);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool4 wwyz
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new bool4(w, w, y, z);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool4 wwyw
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new bool4(w, w, y, w);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool4 wwzx
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new bool4(w, w, z, x);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool4 wwzy
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new bool4(w, w, z, y);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool4 wwzz
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new bool4(w, w, z, z);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool4 wwzw
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new bool4(w, w, z, w);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool4 wwwx
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new bool4(w, w, w, x);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool4 wwwy
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new bool4(w, w, w, y);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool4 wwwz
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new bool4(w, w, w, z);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool4 wwww
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new bool4(w, w, w, w);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool3 xxx
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new bool3(x, x, x);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool3 xxy
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new bool3(x, x, y);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool3 xxz
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new bool3(x, x, z);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool3 xxw
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new bool3(x, x, w);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool3 xyx
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new bool3(x, y, x);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool3 xyy
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new bool3(x, y, y);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool3 xyz
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new bool3(x, y, z);
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
	public bool3 xyw
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new bool3(x, y, w);
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
	public bool3 xzx
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new bool3(x, z, x);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool3 xzy
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new bool3(x, z, y);
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
	public bool3 xzz
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new bool3(x, z, z);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool3 xzw
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new bool3(x, z, w);
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
	public bool3 xwx
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new bool3(x, w, x);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool3 xwy
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new bool3(x, w, y);
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
	public bool3 xwz
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new bool3(x, w, z);
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
	public bool3 xww
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new bool3(x, w, w);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool3 yxx
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new bool3(y, x, x);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool3 yxy
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new bool3(y, x, y);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool3 yxz
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new bool3(y, x, z);
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
	public bool3 yxw
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new bool3(y, x, w);
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
	public bool3 yyx
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new bool3(y, y, x);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool3 yyy
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new bool3(y, y, y);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool3 yyz
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new bool3(y, y, z);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool3 yyw
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new bool3(y, y, w);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool3 yzx
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new bool3(y, z, x);
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
	public bool3 yzy
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new bool3(y, z, y);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool3 yzz
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new bool3(y, z, z);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool3 yzw
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new bool3(y, z, w);
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
	public bool3 ywx
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new bool3(y, w, x);
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
	public bool3 ywy
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new bool3(y, w, y);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool3 ywz
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new bool3(y, w, z);
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
	public bool3 yww
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new bool3(y, w, w);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool3 zxx
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new bool3(z, x, x);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool3 zxy
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new bool3(z, x, y);
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
	public bool3 zxz
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new bool3(z, x, z);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool3 zxw
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new bool3(z, x, w);
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
	public bool3 zyx
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new bool3(z, y, x);
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
	public bool3 zyy
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new bool3(z, y, y);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool3 zyz
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new bool3(z, y, z);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool3 zyw
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new bool3(z, y, w);
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
	public bool3 zzx
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new bool3(z, z, x);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool3 zzy
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new bool3(z, z, y);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool3 zzz
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new bool3(z, z, z);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool3 zzw
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new bool3(z, z, w);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool3 zwx
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new bool3(z, w, x);
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
	public bool3 zwy
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new bool3(z, w, y);
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
	public bool3 zwz
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new bool3(z, w, z);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool3 zww
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new bool3(z, w, w);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool3 wxx
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new bool3(w, x, x);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool3 wxy
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new bool3(w, x, y);
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
	public bool3 wxz
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new bool3(w, x, z);
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
	public bool3 wxw
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new bool3(w, x, w);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool3 wyx
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new bool3(w, y, x);
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
	public bool3 wyy
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new bool3(w, y, y);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool3 wyz
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new bool3(w, y, z);
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
	public bool3 wyw
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new bool3(w, y, w);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool3 wzx
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new bool3(w, z, x);
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
	public bool3 wzy
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new bool3(w, z, y);
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
	public bool3 wzz
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new bool3(w, z, z);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool3 wzw
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new bool3(w, z, w);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool3 wwx
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new bool3(w, w, x);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool3 wwy
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new bool3(w, w, y);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool3 wwz
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new bool3(w, w, z);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool3 www
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new bool3(w, w, w);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool2 xx
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new bool2(x, x);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool2 xy
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new bool2(x, y);
		}
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		set
		{
			x = value.x;
			y = value.y;
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool2 xz
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new bool2(x, z);
		}
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		set
		{
			x = value.x;
			z = value.y;
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool2 xw
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new bool2(x, w);
		}
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		set
		{
			x = value.x;
			w = value.y;
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool2 yx
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new bool2(y, x);
		}
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		set
		{
			y = value.x;
			x = value.y;
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool2 yy
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new bool2(y, y);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool2 yz
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new bool2(y, z);
		}
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		set
		{
			y = value.x;
			z = value.y;
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool2 yw
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new bool2(y, w);
		}
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		set
		{
			y = value.x;
			w = value.y;
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool2 zx
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new bool2(z, x);
		}
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		set
		{
			z = value.x;
			x = value.y;
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool2 zy
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new bool2(z, y);
		}
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		set
		{
			z = value.x;
			y = value.y;
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool2 zz
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new bool2(z, z);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool2 zw
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new bool2(z, w);
		}
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		set
		{
			z = value.x;
			w = value.y;
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool2 wx
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new bool2(w, x);
		}
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		set
		{
			w = value.x;
			x = value.y;
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool2 wy
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new bool2(w, y);
		}
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		set
		{
			w = value.x;
			y = value.y;
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool2 wz
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new bool2(w, z);
		}
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		set
		{
			w = value.x;
			z = value.y;
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool2 ww
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new bool2(w, w);
		}
	}

	public unsafe bool this[int index]
	{
		get
		{
			fixed (bool4* ptr = &this)
			{
				return ((bool*)ptr)[index];
			}
		}
		set
		{
			fixed (bool* ptr = &x)
			{
				ptr[index] = value;
			}
		}
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public bool4(bool x, bool y, bool z, bool w)
	{
		this.x = x;
		this.y = y;
		this.z = z;
		this.w = w;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public bool4(bool x, bool y, bool2 zw)
	{
		this.x = x;
		this.y = y;
		z = zw.x;
		w = zw.y;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public bool4(bool x, bool2 yz, bool w)
	{
		this.x = x;
		y = yz.x;
		z = yz.y;
		this.w = w;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public bool4(bool x, bool3 yzw)
	{
		this.x = x;
		y = yzw.x;
		z = yzw.y;
		w = yzw.z;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public bool4(bool2 xy, bool z, bool w)
	{
		x = xy.x;
		y = xy.y;
		this.z = z;
		this.w = w;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public bool4(bool2 xy, bool2 zw)
	{
		x = xy.x;
		y = xy.y;
		z = zw.x;
		w = zw.y;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public bool4(bool3 xyz, bool w)
	{
		x = xyz.x;
		y = xyz.y;
		z = xyz.z;
		this.w = w;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public bool4(bool4 xyzw)
	{
		x = xyzw.x;
		y = xyzw.y;
		z = xyzw.z;
		w = xyzw.w;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public bool4(bool v)
	{
		x = v;
		y = v;
		z = v;
		w = v;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static implicit operator bool4(bool v)
	{
		return new bool4(v);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool4 operator ==(bool4 lhs, bool4 rhs)
	{
		return new bool4(lhs.x == rhs.x, lhs.y == rhs.y, lhs.z == rhs.z, lhs.w == rhs.w);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool4 operator ==(bool4 lhs, bool rhs)
	{
		return new bool4(lhs.x == rhs, lhs.y == rhs, lhs.z == rhs, lhs.w == rhs);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool4 operator ==(bool lhs, bool4 rhs)
	{
		return new bool4(lhs == rhs.x, lhs == rhs.y, lhs == rhs.z, lhs == rhs.w);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool4 operator !=(bool4 lhs, bool4 rhs)
	{
		return new bool4(lhs.x != rhs.x, lhs.y != rhs.y, lhs.z != rhs.z, lhs.w != rhs.w);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool4 operator !=(bool4 lhs, bool rhs)
	{
		return new bool4(lhs.x != rhs, lhs.y != rhs, lhs.z != rhs, lhs.w != rhs);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool4 operator !=(bool lhs, bool4 rhs)
	{
		return new bool4(lhs != rhs.x, lhs != rhs.y, lhs != rhs.z, lhs != rhs.w);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool4 operator !(bool4 val)
	{
		return new bool4(!val.x, !val.y, !val.z, !val.w);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool4 operator &(bool4 lhs, bool4 rhs)
	{
		return new bool4(lhs.x & rhs.x, lhs.y & rhs.y, lhs.z & rhs.z, lhs.w & rhs.w);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool4 operator &(bool4 lhs, bool rhs)
	{
		return new bool4(lhs.x && rhs, lhs.y && rhs, lhs.z && rhs, lhs.w && rhs);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool4 operator &(bool lhs, bool4 rhs)
	{
		return new bool4(lhs & rhs.x, lhs & rhs.y, lhs & rhs.z, lhs & rhs.w);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool4 operator |(bool4 lhs, bool4 rhs)
	{
		return new bool4(lhs.x | rhs.x, lhs.y | rhs.y, lhs.z | rhs.z, lhs.w | rhs.w);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool4 operator |(bool4 lhs, bool rhs)
	{
		return new bool4(lhs.x || rhs, lhs.y || rhs, lhs.z || rhs, lhs.w || rhs);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool4 operator |(bool lhs, bool4 rhs)
	{
		return new bool4(lhs | rhs.x, lhs | rhs.y, lhs | rhs.z, lhs | rhs.w);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool4 operator ^(bool4 lhs, bool4 rhs)
	{
		return new bool4(lhs.x ^ rhs.x, lhs.y ^ rhs.y, lhs.z ^ rhs.z, lhs.w ^ rhs.w);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool4 operator ^(bool4 lhs, bool rhs)
	{
		return new bool4(lhs.x ^ rhs, lhs.y ^ rhs, lhs.z ^ rhs, lhs.w ^ rhs);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool4 operator ^(bool lhs, bool4 rhs)
	{
		return new bool4(lhs ^ rhs.x, lhs ^ rhs.y, lhs ^ rhs.z, lhs ^ rhs.w);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public bool Equals(bool4 rhs)
	{
		if (x == rhs.x && y == rhs.y && z == rhs.z)
		{
			return w == rhs.w;
		}
		return false;
	}

	public override bool Equals(object o)
	{
		return Equals((bool4)o);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public override int GetHashCode()
	{
		return (int)math.hash(this);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public override string ToString()
	{
		return $"bool4({x}, {y}, {z}, {w})";
	}
}
