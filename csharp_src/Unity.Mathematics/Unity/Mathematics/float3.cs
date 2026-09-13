using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace Unity.Mathematics;

[Serializable]
[DebuggerTypeProxy(typeof(DebuggerProxy))]
public struct float3 : IEquatable<float3>, IFormattable
{
	internal sealed class DebuggerProxy
	{
		public float x;

		public float y;

		public float z;

		public DebuggerProxy(float3 v)
		{
			x = v.x;
			y = v.y;
			z = v.z;
		}
	}

	public float x;

	public float y;

	public float z;

	public static readonly float3 zero;

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

	public unsafe float this[int index]
	{
		get
		{
			fixed (float3* ptr = &this)
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
	public float3(float x, float y, float z)
	{
		this.x = x;
		this.y = y;
		this.z = z;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public float3(float x, float2 yz)
	{
		this.x = x;
		y = yz.x;
		z = yz.y;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public float3(float2 xy, float z)
	{
		x = xy.x;
		y = xy.y;
		this.z = z;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public float3(float3 xyz)
	{
		x = xyz.x;
		y = xyz.y;
		z = xyz.z;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public float3(float v)
	{
		x = v;
		y = v;
		z = v;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public float3(bool v)
	{
		x = (v ? 1f : 0f);
		y = (v ? 1f : 0f);
		z = (v ? 1f : 0f);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public float3(bool3 v)
	{
		x = (v.x ? 1f : 0f);
		y = (v.y ? 1f : 0f);
		z = (v.z ? 1f : 0f);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public float3(int v)
	{
		x = v;
		y = v;
		z = v;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public float3(int3 v)
	{
		x = v.x;
		y = v.y;
		z = v.z;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public float3(uint v)
	{
		x = v;
		y = v;
		z = v;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public float3(uint3 v)
	{
		x = v.x;
		y = v.y;
		z = v.z;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public float3(half v)
	{
		x = v;
		y = v;
		z = v;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public float3(half3 v)
	{
		x = v.x;
		y = v.y;
		z = v.z;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public float3(double v)
	{
		x = (float)v;
		y = (float)v;
		z = (float)v;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public float3(double3 v)
	{
		x = (float)v.x;
		y = (float)v.y;
		z = (float)v.z;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static implicit operator float3(float v)
	{
		return new float3(v);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static explicit operator float3(bool v)
	{
		return new float3(v);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static explicit operator float3(bool3 v)
	{
		return new float3(v);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static implicit operator float3(int v)
	{
		return new float3(v);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static implicit operator float3(int3 v)
	{
		return new float3(v);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static implicit operator float3(uint v)
	{
		return new float3(v);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static implicit operator float3(uint3 v)
	{
		return new float3(v);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static implicit operator float3(half v)
	{
		return new float3(v);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static implicit operator float3(half3 v)
	{
		return new float3(v);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static explicit operator float3(double v)
	{
		return new float3(v);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static explicit operator float3(double3 v)
	{
		return new float3(v);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static float3 operator *(float3 lhs, float3 rhs)
	{
		return new float3(lhs.x * rhs.x, lhs.y * rhs.y, lhs.z * rhs.z);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static float3 operator *(float3 lhs, float rhs)
	{
		return new float3(lhs.x * rhs, lhs.y * rhs, lhs.z * rhs);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static float3 operator *(float lhs, float3 rhs)
	{
		return new float3(lhs * rhs.x, lhs * rhs.y, lhs * rhs.z);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static float3 operator +(float3 lhs, float3 rhs)
	{
		return new float3(lhs.x + rhs.x, lhs.y + rhs.y, lhs.z + rhs.z);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static float3 operator +(float3 lhs, float rhs)
	{
		return new float3(lhs.x + rhs, lhs.y + rhs, lhs.z + rhs);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static float3 operator +(float lhs, float3 rhs)
	{
		return new float3(lhs + rhs.x, lhs + rhs.y, lhs + rhs.z);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static float3 operator -(float3 lhs, float3 rhs)
	{
		return new float3(lhs.x - rhs.x, lhs.y - rhs.y, lhs.z - rhs.z);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static float3 operator -(float3 lhs, float rhs)
	{
		return new float3(lhs.x - rhs, lhs.y - rhs, lhs.z - rhs);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static float3 operator -(float lhs, float3 rhs)
	{
		return new float3(lhs - rhs.x, lhs - rhs.y, lhs - rhs.z);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static float3 operator /(float3 lhs, float3 rhs)
	{
		return new float3(lhs.x / rhs.x, lhs.y / rhs.y, lhs.z / rhs.z);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static float3 operator /(float3 lhs, float rhs)
	{
		return new float3(lhs.x / rhs, lhs.y / rhs, lhs.z / rhs);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static float3 operator /(float lhs, float3 rhs)
	{
		return new float3(lhs / rhs.x, lhs / rhs.y, lhs / rhs.z);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static float3 operator %(float3 lhs, float3 rhs)
	{
		return new float3(lhs.x % rhs.x, lhs.y % rhs.y, lhs.z % rhs.z);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static float3 operator %(float3 lhs, float rhs)
	{
		return new float3(lhs.x % rhs, lhs.y % rhs, lhs.z % rhs);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static float3 operator %(float lhs, float3 rhs)
	{
		return new float3(lhs % rhs.x, lhs % rhs.y, lhs % rhs.z);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static float3 operator ++(float3 val)
	{
		return new float3(val.x += 1f, val.y += 1f, val.z += 1f);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static float3 operator --(float3 val)
	{
		return new float3(val.x -= 1f, val.y -= 1f, val.z -= 1f);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool3 operator <(float3 lhs, float3 rhs)
	{
		return new bool3(lhs.x < rhs.x, lhs.y < rhs.y, lhs.z < rhs.z);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool3 operator <(float3 lhs, float rhs)
	{
		return new bool3(lhs.x < rhs, lhs.y < rhs, lhs.z < rhs);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool3 operator <(float lhs, float3 rhs)
	{
		return new bool3(lhs < rhs.x, lhs < rhs.y, lhs < rhs.z);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool3 operator <=(float3 lhs, float3 rhs)
	{
		return new bool3(lhs.x <= rhs.x, lhs.y <= rhs.y, lhs.z <= rhs.z);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool3 operator <=(float3 lhs, float rhs)
	{
		return new bool3(lhs.x <= rhs, lhs.y <= rhs, lhs.z <= rhs);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool3 operator <=(float lhs, float3 rhs)
	{
		return new bool3(lhs <= rhs.x, lhs <= rhs.y, lhs <= rhs.z);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool3 operator >(float3 lhs, float3 rhs)
	{
		return new bool3(lhs.x > rhs.x, lhs.y > rhs.y, lhs.z > rhs.z);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool3 operator >(float3 lhs, float rhs)
	{
		return new bool3(lhs.x > rhs, lhs.y > rhs, lhs.z > rhs);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool3 operator >(float lhs, float3 rhs)
	{
		return new bool3(lhs > rhs.x, lhs > rhs.y, lhs > rhs.z);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool3 operator >=(float3 lhs, float3 rhs)
	{
		return new bool3(lhs.x >= rhs.x, lhs.y >= rhs.y, lhs.z >= rhs.z);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool3 operator >=(float3 lhs, float rhs)
	{
		return new bool3(lhs.x >= rhs, lhs.y >= rhs, lhs.z >= rhs);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool3 operator >=(float lhs, float3 rhs)
	{
		return new bool3(lhs >= rhs.x, lhs >= rhs.y, lhs >= rhs.z);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static float3 operator -(float3 val)
	{
		return new float3(0f - val.x, 0f - val.y, 0f - val.z);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static float3 operator +(float3 val)
	{
		return new float3(val.x, val.y, val.z);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool3 operator ==(float3 lhs, float3 rhs)
	{
		return new bool3(lhs.x == rhs.x, lhs.y == rhs.y, lhs.z == rhs.z);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool3 operator ==(float3 lhs, float rhs)
	{
		return new bool3(lhs.x == rhs, lhs.y == rhs, lhs.z == rhs);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool3 operator ==(float lhs, float3 rhs)
	{
		return new bool3(lhs == rhs.x, lhs == rhs.y, lhs == rhs.z);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool3 operator !=(float3 lhs, float3 rhs)
	{
		return new bool3(lhs.x != rhs.x, lhs.y != rhs.y, lhs.z != rhs.z);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool3 operator !=(float3 lhs, float rhs)
	{
		return new bool3(lhs.x != rhs, lhs.y != rhs, lhs.z != rhs);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool3 operator !=(float lhs, float3 rhs)
	{
		return new bool3(lhs != rhs.x, lhs != rhs.y, lhs != rhs.z);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public bool Equals(float3 rhs)
	{
		if (x == rhs.x && y == rhs.y)
		{
			return z == rhs.z;
		}
		return false;
	}

	public override bool Equals(object o)
	{
		return Equals((float3)o);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public override int GetHashCode()
	{
		return (int)math.hash(this);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public override string ToString()
	{
		return $"float3({x}f, {y}f, {z}f)";
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public string ToString(string format, IFormatProvider formatProvider)
	{
		return $"float3({x.ToString(format, formatProvider)}f, {y.ToString(format, formatProvider)}f, {z.ToString(format, formatProvider)}f)";
	}

	public static implicit operator Vector3(float3 v)
	{
		return new Vector3(v.x, v.y, v.z);
	}

	public static implicit operator float3(Vector3 v)
	{
		return new float3(v.x, v.y, v.z);
	}
}
