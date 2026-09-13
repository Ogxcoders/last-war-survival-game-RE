using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace Unity.Mathematics;

[Serializable]
[DebuggerTypeProxy(typeof(DebuggerProxy))]
public struct float4 : IEquatable<float4>, IFormattable
{
	internal sealed class DebuggerProxy
	{
		public float x;

		public float y;

		public float z;

		public float w;

		public DebuggerProxy(float4 v)
		{
			x = v.x;
			y = v.y;
			z = v.z;
			w = v.w;
		}
	}

	public float x;

	public float y;

	public float z;

	public float w;

	public static readonly float4 zero;

	[EditorBrowsable(EditorBrowsableState.Never)]
	public float4 xxxx
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new float4(x, x, x, x);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public float4 xxxy
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new float4(x, x, x, y);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public float4 xxxz
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new float4(x, x, x, z);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public float4 xxxw
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new float4(x, x, x, w);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public float4 xxyx
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new float4(x, x, y, x);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public float4 xxyy
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new float4(x, x, y, y);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public float4 xxyz
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new float4(x, x, y, z);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public float4 xxyw
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new float4(x, x, y, w);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public float4 xxzx
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new float4(x, x, z, x);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public float4 xxzy
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new float4(x, x, z, y);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public float4 xxzz
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new float4(x, x, z, z);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public float4 xxzw
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new float4(x, x, z, w);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public float4 xxwx
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new float4(x, x, w, x);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public float4 xxwy
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new float4(x, x, w, y);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public float4 xxwz
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new float4(x, x, w, z);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public float4 xxww
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new float4(x, x, w, w);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public float4 xyxx
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new float4(x, y, x, x);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public float4 xyxy
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new float4(x, y, x, y);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public float4 xyxz
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new float4(x, y, x, z);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public float4 xyxw
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new float4(x, y, x, w);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public float4 xyyx
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new float4(x, y, y, x);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public float4 xyyy
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new float4(x, y, y, y);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public float4 xyyz
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new float4(x, y, y, z);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public float4 xyyw
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new float4(x, y, y, w);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public float4 xyzx
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new float4(x, y, z, x);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public float4 xyzy
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new float4(x, y, z, y);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public float4 xyzz
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new float4(x, y, z, z);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public float4 xyzw
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new float4(x, y, z, w);
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
	public float4 xywx
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new float4(x, y, w, x);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public float4 xywy
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new float4(x, y, w, y);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public float4 xywz
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new float4(x, y, w, z);
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
	public float4 xyww
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new float4(x, y, w, w);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public float4 xzxx
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new float4(x, z, x, x);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public float4 xzxy
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new float4(x, z, x, y);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public float4 xzxz
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new float4(x, z, x, z);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public float4 xzxw
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new float4(x, z, x, w);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public float4 xzyx
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new float4(x, z, y, x);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public float4 xzyy
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new float4(x, z, y, y);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public float4 xzyz
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new float4(x, z, y, z);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public float4 xzyw
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new float4(x, z, y, w);
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
	public float4 xzzx
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new float4(x, z, z, x);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public float4 xzzy
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new float4(x, z, z, y);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public float4 xzzz
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new float4(x, z, z, z);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public float4 xzzw
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new float4(x, z, z, w);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public float4 xzwx
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new float4(x, z, w, x);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public float4 xzwy
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new float4(x, z, w, y);
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
	public float4 xzwz
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new float4(x, z, w, z);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public float4 xzww
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new float4(x, z, w, w);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public float4 xwxx
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new float4(x, w, x, x);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public float4 xwxy
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new float4(x, w, x, y);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public float4 xwxz
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new float4(x, w, x, z);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public float4 xwxw
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new float4(x, w, x, w);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public float4 xwyx
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new float4(x, w, y, x);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public float4 xwyy
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new float4(x, w, y, y);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public float4 xwyz
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new float4(x, w, y, z);
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
	public float4 xwyw
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new float4(x, w, y, w);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public float4 xwzx
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new float4(x, w, z, x);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public float4 xwzy
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new float4(x, w, z, y);
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
	public float4 xwzz
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new float4(x, w, z, z);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public float4 xwzw
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new float4(x, w, z, w);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public float4 xwwx
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new float4(x, w, w, x);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public float4 xwwy
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new float4(x, w, w, y);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public float4 xwwz
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new float4(x, w, w, z);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public float4 xwww
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new float4(x, w, w, w);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public float4 yxxx
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new float4(y, x, x, x);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public float4 yxxy
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new float4(y, x, x, y);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public float4 yxxz
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new float4(y, x, x, z);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public float4 yxxw
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new float4(y, x, x, w);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public float4 yxyx
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new float4(y, x, y, x);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public float4 yxyy
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new float4(y, x, y, y);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public float4 yxyz
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new float4(y, x, y, z);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public float4 yxyw
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new float4(y, x, y, w);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public float4 yxzx
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new float4(y, x, z, x);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public float4 yxzy
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new float4(y, x, z, y);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public float4 yxzz
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new float4(y, x, z, z);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public float4 yxzw
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new float4(y, x, z, w);
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
	public float4 yxwx
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new float4(y, x, w, x);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public float4 yxwy
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new float4(y, x, w, y);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public float4 yxwz
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new float4(y, x, w, z);
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
	public float4 yxww
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new float4(y, x, w, w);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public float4 yyxx
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new float4(y, y, x, x);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public float4 yyxy
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new float4(y, y, x, y);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public float4 yyxz
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new float4(y, y, x, z);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public float4 yyxw
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new float4(y, y, x, w);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public float4 yyyx
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new float4(y, y, y, x);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public float4 yyyy
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new float4(y, y, y, y);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public float4 yyyz
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new float4(y, y, y, z);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public float4 yyyw
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new float4(y, y, y, w);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public float4 yyzx
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new float4(y, y, z, x);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public float4 yyzy
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new float4(y, y, z, y);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public float4 yyzz
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new float4(y, y, z, z);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public float4 yyzw
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new float4(y, y, z, w);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public float4 yywx
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new float4(y, y, w, x);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public float4 yywy
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new float4(y, y, w, y);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public float4 yywz
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new float4(y, y, w, z);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public float4 yyww
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new float4(y, y, w, w);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public float4 yzxx
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new float4(y, z, x, x);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public float4 yzxy
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new float4(y, z, x, y);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public float4 yzxz
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new float4(y, z, x, z);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public float4 yzxw
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new float4(y, z, x, w);
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
	public float4 yzyx
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new float4(y, z, y, x);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public float4 yzyy
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new float4(y, z, y, y);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public float4 yzyz
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new float4(y, z, y, z);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public float4 yzyw
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new float4(y, z, y, w);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public float4 yzzx
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new float4(y, z, z, x);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public float4 yzzy
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new float4(y, z, z, y);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public float4 yzzz
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new float4(y, z, z, z);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public float4 yzzw
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new float4(y, z, z, w);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public float4 yzwx
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new float4(y, z, w, x);
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
	public float4 yzwy
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new float4(y, z, w, y);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public float4 yzwz
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new float4(y, z, w, z);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public float4 yzww
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new float4(y, z, w, w);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public float4 ywxx
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new float4(y, w, x, x);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public float4 ywxy
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new float4(y, w, x, y);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public float4 ywxz
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new float4(y, w, x, z);
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
	public float4 ywxw
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new float4(y, w, x, w);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public float4 ywyx
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new float4(y, w, y, x);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public float4 ywyy
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new float4(y, w, y, y);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public float4 ywyz
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new float4(y, w, y, z);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public float4 ywyw
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new float4(y, w, y, w);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public float4 ywzx
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new float4(y, w, z, x);
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
	public float4 ywzy
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new float4(y, w, z, y);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public float4 ywzz
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new float4(y, w, z, z);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public float4 ywzw
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new float4(y, w, z, w);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public float4 ywwx
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new float4(y, w, w, x);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public float4 ywwy
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new float4(y, w, w, y);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public float4 ywwz
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new float4(y, w, w, z);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public float4 ywww
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new float4(y, w, w, w);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public float4 zxxx
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new float4(z, x, x, x);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public float4 zxxy
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new float4(z, x, x, y);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public float4 zxxz
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new float4(z, x, x, z);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public float4 zxxw
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new float4(z, x, x, w);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public float4 zxyx
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new float4(z, x, y, x);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public float4 zxyy
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new float4(z, x, y, y);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public float4 zxyz
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new float4(z, x, y, z);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public float4 zxyw
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new float4(z, x, y, w);
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
	public float4 zxzx
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new float4(z, x, z, x);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public float4 zxzy
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new float4(z, x, z, y);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public float4 zxzz
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new float4(z, x, z, z);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public float4 zxzw
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new float4(z, x, z, w);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public float4 zxwx
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new float4(z, x, w, x);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public float4 zxwy
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new float4(z, x, w, y);
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
	public float4 zxwz
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new float4(z, x, w, z);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public float4 zxww
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new float4(z, x, w, w);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public float4 zyxx
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new float4(z, y, x, x);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public float4 zyxy
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new float4(z, y, x, y);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public float4 zyxz
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new float4(z, y, x, z);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public float4 zyxw
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new float4(z, y, x, w);
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
	public float4 zyyx
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new float4(z, y, y, x);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public float4 zyyy
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new float4(z, y, y, y);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public float4 zyyz
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new float4(z, y, y, z);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public float4 zyyw
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new float4(z, y, y, w);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public float4 zyzx
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new float4(z, y, z, x);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public float4 zyzy
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new float4(z, y, z, y);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public float4 zyzz
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new float4(z, y, z, z);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public float4 zyzw
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new float4(z, y, z, w);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public float4 zywx
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new float4(z, y, w, x);
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
	public float4 zywy
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new float4(z, y, w, y);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public float4 zywz
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new float4(z, y, w, z);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public float4 zyww
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new float4(z, y, w, w);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public float4 zzxx
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new float4(z, z, x, x);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public float4 zzxy
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new float4(z, z, x, y);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public float4 zzxz
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new float4(z, z, x, z);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public float4 zzxw
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new float4(z, z, x, w);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public float4 zzyx
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new float4(z, z, y, x);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public float4 zzyy
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new float4(z, z, y, y);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public float4 zzyz
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new float4(z, z, y, z);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public float4 zzyw
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new float4(z, z, y, w);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public float4 zzzx
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new float4(z, z, z, x);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public float4 zzzy
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new float4(z, z, z, y);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public float4 zzzz
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new float4(z, z, z, z);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public float4 zzzw
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new float4(z, z, z, w);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public float4 zzwx
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new float4(z, z, w, x);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public float4 zzwy
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new float4(z, z, w, y);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public float4 zzwz
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new float4(z, z, w, z);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public float4 zzww
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new float4(z, z, w, w);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public float4 zwxx
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new float4(z, w, x, x);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public float4 zwxy
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new float4(z, w, x, y);
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
	public float4 zwxz
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new float4(z, w, x, z);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public float4 zwxw
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new float4(z, w, x, w);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public float4 zwyx
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new float4(z, w, y, x);
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
	public float4 zwyy
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new float4(z, w, y, y);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public float4 zwyz
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new float4(z, w, y, z);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public float4 zwyw
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new float4(z, w, y, w);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public float4 zwzx
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new float4(z, w, z, x);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public float4 zwzy
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new float4(z, w, z, y);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public float4 zwzz
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new float4(z, w, z, z);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public float4 zwzw
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new float4(z, w, z, w);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public float4 zwwx
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new float4(z, w, w, x);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public float4 zwwy
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new float4(z, w, w, y);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public float4 zwwz
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new float4(z, w, w, z);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public float4 zwww
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new float4(z, w, w, w);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public float4 wxxx
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new float4(w, x, x, x);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public float4 wxxy
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new float4(w, x, x, y);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public float4 wxxz
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new float4(w, x, x, z);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public float4 wxxw
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new float4(w, x, x, w);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public float4 wxyx
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new float4(w, x, y, x);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public float4 wxyy
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new float4(w, x, y, y);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public float4 wxyz
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new float4(w, x, y, z);
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
	public float4 wxyw
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new float4(w, x, y, w);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public float4 wxzx
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new float4(w, x, z, x);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public float4 wxzy
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new float4(w, x, z, y);
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
	public float4 wxzz
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new float4(w, x, z, z);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public float4 wxzw
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new float4(w, x, z, w);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public float4 wxwx
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new float4(w, x, w, x);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public float4 wxwy
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new float4(w, x, w, y);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public float4 wxwz
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new float4(w, x, w, z);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public float4 wxww
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new float4(w, x, w, w);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public float4 wyxx
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new float4(w, y, x, x);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public float4 wyxy
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new float4(w, y, x, y);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public float4 wyxz
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new float4(w, y, x, z);
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
	public float4 wyxw
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new float4(w, y, x, w);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public float4 wyyx
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new float4(w, y, y, x);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public float4 wyyy
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new float4(w, y, y, y);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public float4 wyyz
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new float4(w, y, y, z);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public float4 wyyw
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new float4(w, y, y, w);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public float4 wyzx
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new float4(w, y, z, x);
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
	public float4 wyzy
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new float4(w, y, z, y);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public float4 wyzz
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new float4(w, y, z, z);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public float4 wyzw
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new float4(w, y, z, w);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public float4 wywx
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new float4(w, y, w, x);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public float4 wywy
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new float4(w, y, w, y);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public float4 wywz
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new float4(w, y, w, z);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public float4 wyww
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new float4(w, y, w, w);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public float4 wzxx
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new float4(w, z, x, x);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public float4 wzxy
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new float4(w, z, x, y);
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
	public float4 wzxz
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new float4(w, z, x, z);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public float4 wzxw
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new float4(w, z, x, w);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public float4 wzyx
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new float4(w, z, y, x);
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
	public float4 wzyy
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new float4(w, z, y, y);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public float4 wzyz
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new float4(w, z, y, z);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public float4 wzyw
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new float4(w, z, y, w);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public float4 wzzx
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new float4(w, z, z, x);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public float4 wzzy
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new float4(w, z, z, y);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public float4 wzzz
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new float4(w, z, z, z);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public float4 wzzw
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new float4(w, z, z, w);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public float4 wzwx
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new float4(w, z, w, x);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public float4 wzwy
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new float4(w, z, w, y);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public float4 wzwz
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new float4(w, z, w, z);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public float4 wzww
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new float4(w, z, w, w);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public float4 wwxx
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new float4(w, w, x, x);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public float4 wwxy
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new float4(w, w, x, y);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public float4 wwxz
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new float4(w, w, x, z);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public float4 wwxw
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new float4(w, w, x, w);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public float4 wwyx
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new float4(w, w, y, x);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public float4 wwyy
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new float4(w, w, y, y);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public float4 wwyz
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new float4(w, w, y, z);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public float4 wwyw
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new float4(w, w, y, w);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public float4 wwzx
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new float4(w, w, z, x);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public float4 wwzy
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new float4(w, w, z, y);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public float4 wwzz
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new float4(w, w, z, z);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public float4 wwzw
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new float4(w, w, z, w);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public float4 wwwx
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new float4(w, w, w, x);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public float4 wwwy
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new float4(w, w, w, y);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public float4 wwwz
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new float4(w, w, w, z);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public float4 wwww
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new float4(w, w, w, w);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public float3 xxx
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new float3(x, x, x);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public float3 xxy
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new float3(x, x, y);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public float3 xxz
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new float3(x, x, z);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public float3 xxw
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new float3(x, x, w);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public float3 xyx
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new float3(x, y, x);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public float3 xyy
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new float3(x, y, y);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public float3 xyz
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new float3(x, y, z);
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
	public float3 xyw
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new float3(x, y, w);
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
	public float3 xzx
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new float3(x, z, x);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public float3 xzy
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new float3(x, z, y);
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
	public float3 xzz
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new float3(x, z, z);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public float3 xzw
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new float3(x, z, w);
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
	public float3 xwx
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new float3(x, w, x);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public float3 xwy
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new float3(x, w, y);
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
	public float3 xwz
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new float3(x, w, z);
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
	public float3 xww
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new float3(x, w, w);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public float3 yxx
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new float3(y, x, x);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public float3 yxy
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new float3(y, x, y);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public float3 yxz
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new float3(y, x, z);
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
	public float3 yxw
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new float3(y, x, w);
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
	public float3 yyx
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new float3(y, y, x);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public float3 yyy
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new float3(y, y, y);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public float3 yyz
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new float3(y, y, z);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public float3 yyw
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new float3(y, y, w);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public float3 yzx
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new float3(y, z, x);
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
	public float3 yzy
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new float3(y, z, y);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public float3 yzz
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new float3(y, z, z);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public float3 yzw
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new float3(y, z, w);
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
	public float3 ywx
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new float3(y, w, x);
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
	public float3 ywy
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new float3(y, w, y);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public float3 ywz
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new float3(y, w, z);
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
	public float3 yww
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new float3(y, w, w);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public float3 zxx
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new float3(z, x, x);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public float3 zxy
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new float3(z, x, y);
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
	public float3 zxz
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new float3(z, x, z);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public float3 zxw
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new float3(z, x, w);
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
	public float3 zyx
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new float3(z, y, x);
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
	public float3 zyy
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new float3(z, y, y);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public float3 zyz
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new float3(z, y, z);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public float3 zyw
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new float3(z, y, w);
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
	public float3 zzx
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new float3(z, z, x);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public float3 zzy
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new float3(z, z, y);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public float3 zzz
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new float3(z, z, z);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public float3 zzw
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new float3(z, z, w);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public float3 zwx
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new float3(z, w, x);
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
	public float3 zwy
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new float3(z, w, y);
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
	public float3 zwz
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new float3(z, w, z);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public float3 zww
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new float3(z, w, w);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public float3 wxx
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new float3(w, x, x);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public float3 wxy
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new float3(w, x, y);
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
	public float3 wxz
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new float3(w, x, z);
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
	public float3 wxw
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new float3(w, x, w);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public float3 wyx
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new float3(w, y, x);
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
	public float3 wyy
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new float3(w, y, y);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public float3 wyz
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new float3(w, y, z);
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
	public float3 wyw
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new float3(w, y, w);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public float3 wzx
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new float3(w, z, x);
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
	public float3 wzy
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new float3(w, z, y);
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
	public float3 wzz
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new float3(w, z, z);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public float3 wzw
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new float3(w, z, w);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public float3 wwx
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new float3(w, w, x);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public float3 wwy
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new float3(w, w, y);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public float3 wwz
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new float3(w, w, z);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public float3 www
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new float3(w, w, w);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public float2 xx
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new float2(x, x);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public float2 xy
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new float2(x, y);
		}
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		set
		{
			x = value.x;
			y = value.y;
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public float2 xz
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new float2(x, z);
		}
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		set
		{
			x = value.x;
			z = value.y;
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public float2 xw
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new float2(x, w);
		}
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		set
		{
			x = value.x;
			w = value.y;
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public float2 yx
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new float2(y, x);
		}
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		set
		{
			y = value.x;
			x = value.y;
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public float2 yy
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new float2(y, y);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public float2 yz
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new float2(y, z);
		}
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		set
		{
			y = value.x;
			z = value.y;
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public float2 yw
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new float2(y, w);
		}
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		set
		{
			y = value.x;
			w = value.y;
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public float2 zx
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new float2(z, x);
		}
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		set
		{
			z = value.x;
			x = value.y;
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public float2 zy
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new float2(z, y);
		}
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		set
		{
			z = value.x;
			y = value.y;
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public float2 zz
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new float2(z, z);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public float2 zw
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new float2(z, w);
		}
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		set
		{
			z = value.x;
			w = value.y;
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public float2 wx
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new float2(w, x);
		}
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		set
		{
			w = value.x;
			x = value.y;
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public float2 wy
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new float2(w, y);
		}
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		set
		{
			w = value.x;
			y = value.y;
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public float2 wz
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new float2(w, z);
		}
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		set
		{
			w = value.x;
			z = value.y;
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public float2 ww
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return new float2(w, w);
		}
	}

	public unsafe float this[int index]
	{
		get
		{
			fixed (float4* ptr = &this)
			{
				return ((float*)ptr)[index];
			}
		}
		set
		{
			fixed (float* ptr = &x)
			{
				ptr[index] = value;
			}
		}
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public float4(float x, float y, float z, float w)
	{
		this.x = x;
		this.y = y;
		this.z = z;
		this.w = w;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public float4(float x, float y, float2 zw)
	{
		this.x = x;
		this.y = y;
		z = zw.x;
		w = zw.y;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public float4(float x, float2 yz, float w)
	{
		this.x = x;
		y = yz.x;
		z = yz.y;
		this.w = w;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public float4(float x, float3 yzw)
	{
		this.x = x;
		y = yzw.x;
		z = yzw.y;
		w = yzw.z;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public float4(float2 xy, float z, float w)
	{
		x = xy.x;
		y = xy.y;
		this.z = z;
		this.w = w;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public float4(float2 xy, float2 zw)
	{
		x = xy.x;
		y = xy.y;
		z = zw.x;
		w = zw.y;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public float4(float3 xyz, float w)
	{
		x = xyz.x;
		y = xyz.y;
		z = xyz.z;
		this.w = w;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public float4(float4 xyzw)
	{
		x = xyzw.x;
		y = xyzw.y;
		z = xyzw.z;
		w = xyzw.w;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public float4(float v)
	{
		x = v;
		y = v;
		z = v;
		w = v;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public float4(bool v)
	{
		x = (v ? 1f : 0f);
		y = (v ? 1f : 0f);
		z = (v ? 1f : 0f);
		w = (v ? 1f : 0f);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public float4(bool4 v)
	{
		x = (v.x ? 1f : 0f);
		y = (v.y ? 1f : 0f);
		z = (v.z ? 1f : 0f);
		w = (v.w ? 1f : 0f);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public float4(int v)
	{
		x = v;
		y = v;
		z = v;
		w = v;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public float4(int4 v)
	{
		x = v.x;
		y = v.y;
		z = v.z;
		w = v.w;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public float4(uint v)
	{
		x = v;
		y = v;
		z = v;
		w = v;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public float4(uint4 v)
	{
		x = v.x;
		y = v.y;
		z = v.z;
		w = v.w;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public float4(half v)
	{
		x = v;
		y = v;
		z = v;
		w = v;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public float4(half4 v)
	{
		x = v.x;
		y = v.y;
		z = v.z;
		w = v.w;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public float4(double v)
	{
		x = (float)v;
		y = (float)v;
		z = (float)v;
		w = (float)v;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public float4(double4 v)
	{
		x = (float)v.x;
		y = (float)v.y;
		z = (float)v.z;
		w = (float)v.w;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static implicit operator float4(float v)
	{
		return new float4(v);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static explicit operator float4(bool v)
	{
		return new float4(v);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static explicit operator float4(bool4 v)
	{
		return new float4(v);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static implicit operator float4(int v)
	{
		return new float4(v);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static implicit operator float4(int4 v)
	{
		return new float4(v);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static implicit operator float4(uint v)
	{
		return new float4(v);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static implicit operator float4(uint4 v)
	{
		return new float4(v);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static implicit operator float4(half v)
	{
		return new float4(v);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static implicit operator float4(half4 v)
	{
		return new float4(v);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static explicit operator float4(double v)
	{
		return new float4(v);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static explicit operator float4(double4 v)
	{
		return new float4(v);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static float4 operator *(float4 lhs, float4 rhs)
	{
		return new float4(lhs.x * rhs.x, lhs.y * rhs.y, lhs.z * rhs.z, lhs.w * rhs.w);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static float4 operator *(float4 lhs, float rhs)
	{
		return new float4(lhs.x * rhs, lhs.y * rhs, lhs.z * rhs, lhs.w * rhs);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static float4 operator *(float lhs, float4 rhs)
	{
		return new float4(lhs * rhs.x, lhs * rhs.y, lhs * rhs.z, lhs * rhs.w);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static float4 operator +(float4 lhs, float4 rhs)
	{
		return new float4(lhs.x + rhs.x, lhs.y + rhs.y, lhs.z + rhs.z, lhs.w + rhs.w);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static float4 operator +(float4 lhs, float rhs)
	{
		return new float4(lhs.x + rhs, lhs.y + rhs, lhs.z + rhs, lhs.w + rhs);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static float4 operator +(float lhs, float4 rhs)
	{
		return new float4(lhs + rhs.x, lhs + rhs.y, lhs + rhs.z, lhs + rhs.w);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static float4 operator -(float4 lhs, float4 rhs)
	{
		return new float4(lhs.x - rhs.x, lhs.y - rhs.y, lhs.z - rhs.z, lhs.w - rhs.w);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static float4 operator -(float4 lhs, float rhs)
	{
		return new float4(lhs.x - rhs, lhs.y - rhs, lhs.z - rhs, lhs.w - rhs);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static float4 operator -(float lhs, float4 rhs)
	{
		return new float4(lhs - rhs.x, lhs - rhs.y, lhs - rhs.z, lhs - rhs.w);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static float4 operator /(float4 lhs, float4 rhs)
	{
		return new float4(lhs.x / rhs.x, lhs.y / rhs.y, lhs.z / rhs.z, lhs.w / rhs.w);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static float4 operator /(float4 lhs, float rhs)
	{
		return new float4(lhs.x / rhs, lhs.y / rhs, lhs.z / rhs, lhs.w / rhs);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static float4 operator /(float lhs, float4 rhs)
	{
		return new float4(lhs / rhs.x, lhs / rhs.y, lhs / rhs.z, lhs / rhs.w);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static float4 operator %(float4 lhs, float4 rhs)
	{
		return new float4(lhs.x % rhs.x, lhs.y % rhs.y, lhs.z % rhs.z, lhs.w % rhs.w);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static float4 operator %(float4 lhs, float rhs)
	{
		return new float4(lhs.x % rhs, lhs.y % rhs, lhs.z % rhs, lhs.w % rhs);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static float4 operator %(float lhs, float4 rhs)
	{
		return new float4(lhs % rhs.x, lhs % rhs.y, lhs % rhs.z, lhs % rhs.w);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static float4 operator ++(float4 val)
	{
		return new float4(val.x += 1f, val.y += 1f, val.z += 1f, val.w += 1f);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static float4 operator --(float4 val)
	{
		return new float4(val.x -= 1f, val.y -= 1f, val.z -= 1f, val.w -= 1f);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool4 operator <(float4 lhs, float4 rhs)
	{
		return new bool4(lhs.x < rhs.x, lhs.y < rhs.y, lhs.z < rhs.z, lhs.w < rhs.w);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool4 operator <(float4 lhs, float rhs)
	{
		return new bool4(lhs.x < rhs, lhs.y < rhs, lhs.z < rhs, lhs.w < rhs);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool4 operator <(float lhs, float4 rhs)
	{
		return new bool4(lhs < rhs.x, lhs < rhs.y, lhs < rhs.z, lhs < rhs.w);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool4 operator <=(float4 lhs, float4 rhs)
	{
		return new bool4(lhs.x <= rhs.x, lhs.y <= rhs.y, lhs.z <= rhs.z, lhs.w <= rhs.w);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool4 operator <=(float4 lhs, float rhs)
	{
		return new bool4(lhs.x <= rhs, lhs.y <= rhs, lhs.z <= rhs, lhs.w <= rhs);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool4 operator <=(float lhs, float4 rhs)
	{
		return new bool4(lhs <= rhs.x, lhs <= rhs.y, lhs <= rhs.z, lhs <= rhs.w);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool4 operator >(float4 lhs, float4 rhs)
	{
		return new bool4(lhs.x > rhs.x, lhs.y > rhs.y, lhs.z > rhs.z, lhs.w > rhs.w);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool4 operator >(float4 lhs, float rhs)
	{
		return new bool4(lhs.x > rhs, lhs.y > rhs, lhs.z > rhs, lhs.w > rhs);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool4 operator >(float lhs, float4 rhs)
	{
		return new bool4(lhs > rhs.x, lhs > rhs.y, lhs > rhs.z, lhs > rhs.w);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool4 operator >=(float4 lhs, float4 rhs)
	{
		return new bool4(lhs.x >= rhs.x, lhs.y >= rhs.y, lhs.z >= rhs.z, lhs.w >= rhs.w);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool4 operator >=(float4 lhs, float rhs)
	{
		return new bool4(lhs.x >= rhs, lhs.y >= rhs, lhs.z >= rhs, lhs.w >= rhs);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool4 operator >=(float lhs, float4 rhs)
	{
		return new bool4(lhs >= rhs.x, lhs >= rhs.y, lhs >= rhs.z, lhs >= rhs.w);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static float4 operator -(float4 val)
	{
		return new float4(0f - val.x, 0f - val.y, 0f - val.z, 0f - val.w);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static float4 operator +(float4 val)
	{
		return new float4(val.x, val.y, val.z, val.w);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool4 operator ==(float4 lhs, float4 rhs)
	{
		return new bool4(lhs.x == rhs.x, lhs.y == rhs.y, lhs.z == rhs.z, lhs.w == rhs.w);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool4 operator ==(float4 lhs, float rhs)
	{
		return new bool4(lhs.x == rhs, lhs.y == rhs, lhs.z == rhs, lhs.w == rhs);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool4 operator ==(float lhs, float4 rhs)
	{
		return new bool4(lhs == rhs.x, lhs == rhs.y, lhs == rhs.z, lhs == rhs.w);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool4 operator !=(float4 lhs, float4 rhs)
	{
		return new bool4(lhs.x != rhs.x, lhs.y != rhs.y, lhs.z != rhs.z, lhs.w != rhs.w);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool4 operator !=(float4 lhs, float rhs)
	{
		return new bool4(lhs.x != rhs, lhs.y != rhs, lhs.z != rhs, lhs.w != rhs);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool4 operator !=(float lhs, float4 rhs)
	{
		return new bool4(lhs != rhs.x, lhs != rhs.y, lhs != rhs.z, lhs != rhs.w);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public bool Equals(float4 rhs)
	{
		if (x == rhs.x && y == rhs.y && z == rhs.z)
		{
			return w == rhs.w;
		}
		return false;
	}

	public override bool Equals(object o)
	{
		return Equals((float4)o);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public override int GetHashCode()
	{
		return (int)math.hash(this);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public override string ToString()
	{
		return $"float4({x}f, {y}f, {z}f, {w}f)";
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public string ToString(string format, IFormatProvider formatProvider)
	{
		return $"float4({x.ToString(format, formatProvider)}f, {y.ToString(format, formatProvider)}f, {z.ToString(format, formatProvider)}f, {w.ToString(format, formatProvider)}f)";
	}

	public static implicit operator float4(Vector4 v)
	{
		return new float4(v.x, v.y, v.z, v.w);
	}

	public static implicit operator Vector4(float4 v)
	{
		return new Vector4(v.x, v.y, v.z, v.w);
	}
}
