using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Unity.Mathematics;

public static class math
{
	public enum RotationOrder : byte
	{
		XYZ = 0,
		XZY = 1,
		YXZ = 2,
		YZX = 3,
		ZXY = 4,
		ZYX = 5,
		Default = 4
	}

	public enum ShuffleComponent : byte
	{
		LeftX,
		LeftY,
		LeftZ,
		LeftW,
		RightX,
		RightY,
		RightZ,
		RightW
	}

	[StructLayout(LayoutKind.Explicit)]
	internal struct IntFloatUnion
	{
		[FieldOffset(0)]
		public int intValue;

		[FieldOffset(0)]
		public float floatValue;
	}

	[StructLayout(LayoutKind.Explicit)]
	internal struct LongDoubleUnion
	{
		[FieldOffset(0)]
		public long longValue;

		[FieldOffset(0)]
		public double doubleValue;
	}

	public const double E_DBL = Math.E;

	public const double LOG2E_DBL = 1.4426950408889634;

	public const double LOG10E_DBL = 0.4342944819032518;

	public const double LN2_DBL = 0.6931471805599453;

	public const double LN10_DBL = 2.302585092994046;

	public const double PI_DBL = Math.PI;

	public const double SQRT2_DBL = 1.4142135623730951;

	public const double EPSILON_DBL = 2.220446049250313E-16;

	public const double INFINITY_DBL = double.PositiveInfinity;

	public const double NAN_DBL = double.NaN;

	public const float FLT_MIN_NORMAL = 1.1754944E-38f;

	public const double DBL_MIN_NORMAL = 2.2250738585072014E-308;

	public const float E = MathF.E;

	public const float LOG2E = 1.442695f;

	public const float LOG10E = 0.4342945f;

	public const float LN2 = 0.6931472f;

	public const float LN10 = 2.3025851f;

	public const float PI = MathF.PI;

	public const float SQRT2 = 1.4142135f;

	public const float EPSILON = 1.1920929E-07f;

	public const float INFINITY = float.PositiveInfinity;

	public const float NAN = float.NaN;

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool2 bool2(bool x, bool y)
	{
		return new bool2(x, y);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool2 bool2(bool2 xy)
	{
		return new bool2(xy);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool2 bool2(bool v)
	{
		return new bool2(v);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static uint hash(bool2 v)
	{
		return csum(select(uint2(2426570171u, 1561977301u), uint2(4205774813u, 1650214333u), v));
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static uint2 hashwide(bool2 v)
	{
		return select(uint2(3388112843u, 1831150513u), uint2(1848374953u, 3430200247u), v);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool shuffle(bool2 a, bool2 b, ShuffleComponent x)
	{
		return select_shuffle_component(a, b, x);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool2 shuffle(bool2 a, bool2 b, ShuffleComponent x, ShuffleComponent y)
	{
		return bool2(select_shuffle_component(a, b, x), select_shuffle_component(a, b, y));
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool3 shuffle(bool2 a, bool2 b, ShuffleComponent x, ShuffleComponent y, ShuffleComponent z)
	{
		return bool3(select_shuffle_component(a, b, x), select_shuffle_component(a, b, y), select_shuffle_component(a, b, z));
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool4 shuffle(bool2 a, bool2 b, ShuffleComponent x, ShuffleComponent y, ShuffleComponent z, ShuffleComponent w)
	{
		return bool4(select_shuffle_component(a, b, x), select_shuffle_component(a, b, y), select_shuffle_component(a, b, z), select_shuffle_component(a, b, w));
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	internal static bool select_shuffle_component(bool2 a, bool2 b, ShuffleComponent component)
	{
		return component switch
		{
			ShuffleComponent.LeftX => a.x, 
			ShuffleComponent.LeftY => a.y, 
			ShuffleComponent.RightX => b.x, 
			ShuffleComponent.RightY => b.y, 
			_ => throw new ArgumentException("Invalid shuffle component: " + component), 
		};
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool2x2 bool2x2(bool2 c0, bool2 c1)
	{
		return new bool2x2(c0, c1);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool2x2 bool2x2(bool m00, bool m01, bool m10, bool m11)
	{
		return new bool2x2(m00, m01, m10, m11);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool2x2 bool2x2(bool v)
	{
		return new bool2x2(v);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool2x2 transpose(bool2x2 v)
	{
		return bool2x2(v.c0.x, v.c0.y, v.c1.x, v.c1.y);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static uint hash(bool2x2 v)
	{
		return csum(select(uint2(2062756937u, 2920485769u), uint2(1562056283u, 2265541847u), v.c0) + select(uint2(1283419601u, 1210229737u), uint2(2864955997u, 3525118277u), v.c1));
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static uint2 hashwide(bool2x2 v)
	{
		return select(uint2(2298260269u, 1632478733u), uint2(1537393931u, 2353355467u), v.c0) + select(uint2(3441847433u, 4052036147u), uint2(2011389559u, 2252224297u), v.c1);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool2x3 bool2x3(bool2 c0, bool2 c1, bool2 c2)
	{
		return new bool2x3(c0, c1, c2);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool2x3 bool2x3(bool m00, bool m01, bool m02, bool m10, bool m11, bool m12)
	{
		return new bool2x3(m00, m01, m02, m10, m11, m12);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool2x3 bool2x3(bool v)
	{
		return new bool2x3(v);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool3x2 transpose(bool2x3 v)
	{
		return bool3x2(v.c0.x, v.c0.y, v.c1.x, v.c1.y, v.c2.x, v.c2.y);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static uint hash(bool2x3 v)
	{
		return csum(select(uint2(2078515003u, 4206465343u), uint2(3025146473u, 3763046909u), v.c0) + select(uint2(3678265601u, 2070747979u), uint2(1480171127u, 1588341193u), v.c1) + select(uint2(4234155257u, 1811310911u), uint2(2635799963u, 4165137857u), v.c2));
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static uint2 hashwide(bool2x3 v)
	{
		return select(uint2(2759770933u, 2759319383u), uint2(3299952959u, 3121178323u), v.c0) + select(uint2(2948522579u, 1531026433u), uint2(1365086453u, 3969870067u), v.c1) + select(uint2(4192899797u, 3271228601u), uint2(1634639009u, 3318036811u), v.c2);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool2x4 bool2x4(bool2 c0, bool2 c1, bool2 c2, bool2 c3)
	{
		return new bool2x4(c0, c1, c2, c3);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool2x4 bool2x4(bool m00, bool m01, bool m02, bool m03, bool m10, bool m11, bool m12, bool m13)
	{
		return new bool2x4(m00, m01, m02, m03, m10, m11, m12, m13);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool2x4 bool2x4(bool v)
	{
		return new bool2x4(v);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool4x2 transpose(bool2x4 v)
	{
		return bool4x2(v.c0.x, v.c0.y, v.c1.x, v.c1.y, v.c2.x, v.c2.y, v.c3.x, v.c3.y);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static uint hash(bool2x4 v)
	{
		return csum(select(uint2(1168253063u, 4228926523u), uint2(1610574617u, 1584185147u), v.c0) + select(uint2(3041325733u, 3150930919u), uint2(3309258581u, 1770373673u), v.c1) + select(uint2(3778261171u, 3286279097u), uint2(4264629071u, 1898591447u), v.c2) + select(uint2(2641864091u, 1229113913u), uint2(3020867117u, 1449055807u), v.c3));
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static uint2 hashwide(bool2x4 v)
	{
		return select(uint2(2479033387u, 3702457169u), uint2(1845824257u, 1963973621u), v.c0) + select(uint2(2134758553u, 1391111867u), uint2(1167706003u, 2209736489u), v.c1) + select(uint2(3261535807u, 1740411209u), uint2(2910609089u, 2183822701u), v.c2) + select(uint2(3029516053u, 3547472099u), uint2(2057487037u, 3781937309u), v.c3);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool3 bool3(bool x, bool y, bool z)
	{
		return new bool3(x, y, z);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool3 bool3(bool x, bool2 yz)
	{
		return new bool3(x, yz);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool3 bool3(bool2 xy, bool z)
	{
		return new bool3(xy, z);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool3 bool3(bool3 xyz)
	{
		return new bool3(xyz);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool3 bool3(bool v)
	{
		return new bool3(v);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static uint hash(bool3 v)
	{
		return csum(select(uint3(2716413241u, 1166264321u, 2503385333u), uint3(2944493077u, 2599999021u, 3814721321u), v));
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static uint3 hashwide(bool3 v)
	{
		return select(uint3(1595355149u, 1728931849u, 2062756937u), uint3(2920485769u, 1562056283u, 2265541847u), v);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool shuffle(bool3 a, bool3 b, ShuffleComponent x)
	{
		return select_shuffle_component(a, b, x);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool2 shuffle(bool3 a, bool3 b, ShuffleComponent x, ShuffleComponent y)
	{
		return bool2(select_shuffle_component(a, b, x), select_shuffle_component(a, b, y));
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool3 shuffle(bool3 a, bool3 b, ShuffleComponent x, ShuffleComponent y, ShuffleComponent z)
	{
		return bool3(select_shuffle_component(a, b, x), select_shuffle_component(a, b, y), select_shuffle_component(a, b, z));
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool4 shuffle(bool3 a, bool3 b, ShuffleComponent x, ShuffleComponent y, ShuffleComponent z, ShuffleComponent w)
	{
		return bool4(select_shuffle_component(a, b, x), select_shuffle_component(a, b, y), select_shuffle_component(a, b, z), select_shuffle_component(a, b, w));
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	internal static bool select_shuffle_component(bool3 a, bool3 b, ShuffleComponent component)
	{
		return component switch
		{
			ShuffleComponent.LeftX => a.x, 
			ShuffleComponent.LeftY => a.y, 
			ShuffleComponent.LeftZ => a.z, 
			ShuffleComponent.RightX => b.x, 
			ShuffleComponent.RightY => b.y, 
			ShuffleComponent.RightZ => b.z, 
			_ => throw new ArgumentException("Invalid shuffle component: " + component), 
		};
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool3x2 bool3x2(bool3 c0, bool3 c1)
	{
		return new bool3x2(c0, c1);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool3x2 bool3x2(bool m00, bool m01, bool m10, bool m11, bool m20, bool m21)
	{
		return new bool3x2(m00, m01, m10, m11, m20, m21);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool3x2 bool3x2(bool v)
	{
		return new bool3x2(v);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool2x3 transpose(bool3x2 v)
	{
		return bool2x3(v.c0.x, v.c0.y, v.c0.z, v.c1.x, v.c1.y, v.c1.z);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static uint hash(bool3x2 v)
	{
		return csum(select(uint3(2627668003u, 1520214331u, 2949502447u), uint3(2827819133u, 3480140317u, 2642994593u), v.c0) + select(uint3(3940484981u, 1954192763u, 1091696537u), uint3(3052428017u, 4253034763u, 2338696631u), v.c1));
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static uint3 hashwide(bool3x2 v)
	{
		return select(uint3(3757372771u, 1885959949u, 3508684087u), uint3(3919501043u, 1209161033u, 4007793211u), v.c0) + select(uint3(3819806693u, 3458005183u, 2078515003u), uint3(4206465343u, 3025146473u, 3763046909u), v.c1);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool3x3 bool3x3(bool3 c0, bool3 c1, bool3 c2)
	{
		return new bool3x3(c0, c1, c2);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool3x3 bool3x3(bool m00, bool m01, bool m02, bool m10, bool m11, bool m12, bool m20, bool m21, bool m22)
	{
		return new bool3x3(m00, m01, m02, m10, m11, m12, m20, m21, m22);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool3x3 bool3x3(bool v)
	{
		return new bool3x3(v);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool3x3 transpose(bool3x3 v)
	{
		return bool3x3(v.c0.x, v.c0.y, v.c0.z, v.c1.x, v.c1.y, v.c1.z, v.c2.x, v.c2.y, v.c2.z);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static uint hash(bool3x3 v)
	{
		return csum(select(uint3(3881277847u, 4017968839u, 1727237899u), uint3(1648514723u, 1385344481u, 3538260197u), v.c0) + select(uint3(4066109527u, 2613148903u, 3367528529u), uint3(1678332449u, 2918459647u, 2744611081u), v.c1) + select(uint3(1952372791u, 2631698677u, 4200781601u), uint3(2119021007u, 1760485621u, 3157985881u), v.c2));
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static uint3 hashwide(bool3x3 v)
	{
		return select(uint3(2171534173u, 2723054263u, 1168253063u), uint3(4228926523u, 1610574617u, 1584185147u), v.c0) + select(uint3(3041325733u, 3150930919u, 3309258581u), uint3(1770373673u, 3778261171u, 3286279097u), v.c1) + select(uint3(4264629071u, 1898591447u, 2641864091u), uint3(1229113913u, 3020867117u, 1449055807u), v.c2);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool3x4 bool3x4(bool3 c0, bool3 c1, bool3 c2, bool3 c3)
	{
		return new bool3x4(c0, c1, c2, c3);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool3x4 bool3x4(bool m00, bool m01, bool m02, bool m03, bool m10, bool m11, bool m12, bool m13, bool m20, bool m21, bool m22, bool m23)
	{
		return new bool3x4(m00, m01, m02, m03, m10, m11, m12, m13, m20, m21, m22, m23);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool3x4 bool3x4(bool v)
	{
		return new bool3x4(v);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool4x3 transpose(bool3x4 v)
	{
		return bool4x3(v.c0.x, v.c0.y, v.c0.z, v.c1.x, v.c1.y, v.c1.z, v.c2.x, v.c2.y, v.c2.z, v.c3.x, v.c3.y, v.c3.z);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static uint hash(bool3x4 v)
	{
		return csum(select(uint3(2209710647u, 2201894441u, 2849577407u), uint3(3287031191u, 3098675399u, 1564399943u), v.c0) + select(uint3(1148435377u, 3416333663u, 1750611407u), uint3(3285396193u, 3110507567u, 4271396531u), v.c1) + select(uint3(4198118021u, 2908068253u, 3705492289u), uint3(2497566569u, 2716413241u, 1166264321u), v.c2) + select(uint3(2503385333u, 2944493077u, 2599999021u), uint3(3814721321u, 1595355149u, 1728931849u), v.c3));
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static uint3 hashwide(bool3x4 v)
	{
		return select(uint3(2062756937u, 2920485769u, 1562056283u), uint3(2265541847u, 1283419601u, 1210229737u), v.c0) + select(uint3(2864955997u, 3525118277u, 2298260269u), uint3(1632478733u, 1537393931u, 2353355467u), v.c1) + select(uint3(3441847433u, 4052036147u, 2011389559u), uint3(2252224297u, 3784421429u, 1750626223u), v.c2) + select(uint3(3571447507u, 3412283213u, 2601761069u), uint3(1254033427u, 2248573027u, 3612677113u), v.c3);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool4 bool4(bool x, bool y, bool z, bool w)
	{
		return new bool4(x, y, z, w);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool4 bool4(bool x, bool y, bool2 zw)
	{
		return new bool4(x, y, zw);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool4 bool4(bool x, bool2 yz, bool w)
	{
		return new bool4(x, yz, w);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool4 bool4(bool x, bool3 yzw)
	{
		return new bool4(x, yzw);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool4 bool4(bool2 xy, bool z, bool w)
	{
		return new bool4(xy, z, w);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool4 bool4(bool2 xy, bool2 zw)
	{
		return new bool4(xy, zw);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool4 bool4(bool3 xyz, bool w)
	{
		return new bool4(xyz, w);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool4 bool4(bool4 xyzw)
	{
		return new bool4(xyzw);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool4 bool4(bool v)
	{
		return new bool4(v);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static uint hash(bool4 v)
	{
		return csum(select(uint4(1610574617u, 1584185147u, 3041325733u, 3150930919u), uint4(3309258581u, 1770373673u, 3778261171u, 3286279097u), v));
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static uint4 hashwide(bool4 v)
	{
		return select(uint4(4264629071u, 1898591447u, 2641864091u, 1229113913u), uint4(3020867117u, 1449055807u, 2479033387u, 3702457169u), v);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool shuffle(bool4 a, bool4 b, ShuffleComponent x)
	{
		return select_shuffle_component(a, b, x);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool2 shuffle(bool4 a, bool4 b, ShuffleComponent x, ShuffleComponent y)
	{
		return bool2(select_shuffle_component(a, b, x), select_shuffle_component(a, b, y));
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool3 shuffle(bool4 a, bool4 b, ShuffleComponent x, ShuffleComponent y, ShuffleComponent z)
	{
		return bool3(select_shuffle_component(a, b, x), select_shuffle_component(a, b, y), select_shuffle_component(a, b, z));
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool4 shuffle(bool4 a, bool4 b, ShuffleComponent x, ShuffleComponent y, ShuffleComponent z, ShuffleComponent w)
	{
		return bool4(select_shuffle_component(a, b, x), select_shuffle_component(a, b, y), select_shuffle_component(a, b, z), select_shuffle_component(a, b, w));
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	internal static bool select_shuffle_component(bool4 a, bool4 b, ShuffleComponent component)
	{
		return component switch
		{
			ShuffleComponent.LeftX => a.x, 
			ShuffleComponent.LeftY => a.y, 
			ShuffleComponent.LeftZ => a.z, 
			ShuffleComponent.LeftW => a.w, 
			ShuffleComponent.RightX => b.x, 
			ShuffleComponent.RightY => b.y, 
			ShuffleComponent.RightZ => b.z, 
			ShuffleComponent.RightW => b.w, 
			_ => throw new ArgumentException("Invalid shuffle component: " + component), 
		};
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool4x2 bool4x2(bool4 c0, bool4 c1)
	{
		return new bool4x2(c0, c1);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool4x2 bool4x2(bool m00, bool m01, bool m10, bool m11, bool m20, bool m21, bool m30, bool m31)
	{
		return new bool4x2(m00, m01, m10, m11, m20, m21, m30, m31);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool4x2 bool4x2(bool v)
	{
		return new bool4x2(v);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool2x4 transpose(bool4x2 v)
	{
		return bool2x4(v.c0.x, v.c0.y, v.c0.z, v.c0.w, v.c1.x, v.c1.y, v.c1.z, v.c1.w);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static uint hash(bool4x2 v)
	{
		return csum(select(uint4(3516359879u, 3050356579u, 4178586719u, 2558655391u), uint4(1453413133u, 2152428077u, 1938706661u, 1338588197u), v.c0) + select(uint4(3439609253u, 3535343003u, 3546061613u, 2702024231u), uint4(1452124841u, 1966089551u, 2668168249u, 1587512777u), v.c1));
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static uint4 hashwide(bool4x2 v)
	{
		return select(uint4(2353831999u, 3101256173u, 2891822459u, 2837054189u), uint4(3016004371u, 4097481403u, 2229788699u, 2382715877u), v.c0) + select(uint4(1851936439u, 1938025801u, 3712598587u, 3956330501u), uint4(2437373431u, 1441286183u, 2426570171u, 1561977301u), v.c1);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool4x3 bool4x3(bool4 c0, bool4 c1, bool4 c2)
	{
		return new bool4x3(c0, c1, c2);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool4x3 bool4x3(bool m00, bool m01, bool m02, bool m10, bool m11, bool m12, bool m20, bool m21, bool m22, bool m30, bool m31, bool m32)
	{
		return new bool4x3(m00, m01, m02, m10, m11, m12, m20, m21, m22, m30, m31, m32);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool4x3 bool4x3(bool v)
	{
		return new bool4x3(v);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool3x4 transpose(bool4x3 v)
	{
		return bool3x4(v.c0.x, v.c0.y, v.c0.z, v.c0.w, v.c1.x, v.c1.y, v.c1.z, v.c1.w, v.c2.x, v.c2.y, v.c2.z, v.c2.w);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static uint hash(bool4x3 v)
	{
		return csum(select(uint4(3940484981u, 1954192763u, 1091696537u, 3052428017u), uint4(4253034763u, 2338696631u, 3757372771u, 1885959949u), v.c0) + select(uint4(3508684087u, 3919501043u, 1209161033u, 4007793211u), uint4(3819806693u, 3458005183u, 2078515003u, 4206465343u), v.c1) + select(uint4(3025146473u, 3763046909u, 3678265601u, 2070747979u), uint4(1480171127u, 1588341193u, 4234155257u, 1811310911u), v.c2));
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static uint4 hashwide(bool4x3 v)
	{
		return select(uint4(2635799963u, 4165137857u, 2759770933u, 2759319383u), uint4(3299952959u, 3121178323u, 2948522579u, 1531026433u), v.c0) + select(uint4(1365086453u, 3969870067u, 4192899797u, 3271228601u), uint4(1634639009u, 3318036811u, 3404170631u, 2048213449u), v.c1) + select(uint4(4164671783u, 1780759499u, 1352369353u, 2446407751u), uint4(1391928079u, 3475533443u, 3777095341u, 3385463369u), v.c2);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool4x4 bool4x4(bool4 c0, bool4 c1, bool4 c2, bool4 c3)
	{
		return new bool4x4(c0, c1, c2, c3);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool4x4 bool4x4(bool m00, bool m01, bool m02, bool m03, bool m10, bool m11, bool m12, bool m13, bool m20, bool m21, bool m22, bool m23, bool m30, bool m31, bool m32, bool m33)
	{
		return new bool4x4(m00, m01, m02, m03, m10, m11, m12, m13, m20, m21, m22, m23, m30, m31, m32, m33);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool4x4 bool4x4(bool v)
	{
		return new bool4x4(v);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool4x4 transpose(bool4x4 v)
	{
		return bool4x4(v.c0.x, v.c0.y, v.c0.z, v.c0.w, v.c1.x, v.c1.y, v.c1.z, v.c1.w, v.c2.x, v.c2.y, v.c2.z, v.c2.w, v.c3.x, v.c3.y, v.c3.z, v.c3.w);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static uint hash(bool4x4 v)
	{
		return csum(select(uint4(3516359879u, 3050356579u, 4178586719u, 2558655391u), uint4(1453413133u, 2152428077u, 1938706661u, 1338588197u), v.c0) + select(uint4(3439609253u, 3535343003u, 3546061613u, 2702024231u), uint4(1452124841u, 1966089551u, 2668168249u, 1587512777u), v.c1) + select(uint4(2353831999u, 3101256173u, 2891822459u, 2837054189u), uint4(3016004371u, 4097481403u, 2229788699u, 2382715877u), v.c2) + select(uint4(1851936439u, 1938025801u, 3712598587u, 3956330501u), uint4(2437373431u, 1441286183u, 2426570171u, 1561977301u), v.c3));
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static uint4 hashwide(bool4x4 v)
	{
		return select(uint4(4205774813u, 1650214333u, 3388112843u, 1831150513u), uint4(1848374953u, 3430200247u, 2209710647u, 2201894441u), v.c0) + select(uint4(2849577407u, 3287031191u, 3098675399u, 1564399943u), uint4(1148435377u, 3416333663u, 1750611407u, 3285396193u), v.c1) + select(uint4(3110507567u, 4271396531u, 4198118021u, 2908068253u), uint4(3705492289u, 2497566569u, 2716413241u, 1166264321u), v.c2) + select(uint4(2503385333u, 2944493077u, 2599999021u, 3814721321u), uint4(1595355149u, 1728931849u, 2062756937u, 2920485769u), v.c3);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static double2 double2(double x, double y)
	{
		return new double2(x, y);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static double2 double2(double2 xy)
	{
		return new double2(xy);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static double2 double2(double v)
	{
		return new double2(v);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static double2 double2(bool v)
	{
		return new double2(v);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static double2 double2(bool2 v)
	{
		return new double2(v);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static double2 double2(int v)
	{
		return new double2(v);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static double2 double2(int2 v)
	{
		return new double2(v);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static double2 double2(uint v)
	{
		return new double2(v);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static double2 double2(uint2 v)
	{
		return new double2(v);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static double2 double2(half v)
	{
		return new double2(v);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static double2 double2(half2 v)
	{
		return new double2(v);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static double2 double2(float v)
	{
		return new double2(v);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static double2 double2(float2 v)
	{
		return new double2(v);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static uint hash(double2 v)
	{
		return csum(fold_to_uint(v) * uint2(2503385333u, 2944493077u)) + 2599999021u;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static uint2 hashwide(double2 v)
	{
		return fold_to_uint(v) * uint2(3814721321u, 1595355149u) + 1728931849u;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static double shuffle(double2 a, double2 b, ShuffleComponent x)
	{
		return select_shuffle_component(a, b, x);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static double2 shuffle(double2 a, double2 b, ShuffleComponent x, ShuffleComponent y)
	{
		return double2(select_shuffle_component(a, b, x), select_shuffle_component(a, b, y));
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static double3 shuffle(double2 a, double2 b, ShuffleComponent x, ShuffleComponent y, ShuffleComponent z)
	{
		return double3(select_shuffle_component(a, b, x), select_shuffle_component(a, b, y), select_shuffle_component(a, b, z));
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static double4 shuffle(double2 a, double2 b, ShuffleComponent x, ShuffleComponent y, ShuffleComponent z, ShuffleComponent w)
	{
		return double4(select_shuffle_component(a, b, x), select_shuffle_component(a, b, y), select_shuffle_component(a, b, z), select_shuffle_component(a, b, w));
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	internal static double select_shuffle_component(double2 a, double2 b, ShuffleComponent component)
	{
		return component switch
		{
			ShuffleComponent.LeftX => a.x, 
			ShuffleComponent.LeftY => a.y, 
			ShuffleComponent.RightX => b.x, 
			ShuffleComponent.RightY => b.y, 
			_ => throw new ArgumentException("Invalid shuffle component: " + component), 
		};
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static double2x2 double2x2(double2 c0, double2 c1)
	{
		return new double2x2(c0, c1);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static double2x2 double2x2(double m00, double m01, double m10, double m11)
	{
		return new double2x2(m00, m01, m10, m11);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static double2x2 double2x2(double v)
	{
		return new double2x2(v);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static double2x2 double2x2(bool v)
	{
		return new double2x2(v);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static double2x2 double2x2(bool2x2 v)
	{
		return new double2x2(v);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static double2x2 double2x2(int v)
	{
		return new double2x2(v);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static double2x2 double2x2(int2x2 v)
	{
		return new double2x2(v);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static double2x2 double2x2(uint v)
	{
		return new double2x2(v);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static double2x2 double2x2(uint2x2 v)
	{
		return new double2x2(v);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static double2x2 double2x2(float v)
	{
		return new double2x2(v);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static double2x2 double2x2(float2x2 v)
	{
		return new double2x2(v);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static double2x2 transpose(double2x2 v)
	{
		return double2x2(v.c0.x, v.c0.y, v.c1.x, v.c1.y);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static double2x2 inverse(double2x2 m)
	{
		double x = m.c0.x;
		double x2 = m.c1.x;
		double y = m.c0.y;
		double y2 = m.c1.y;
		double num = x * y2 - x2 * y;
		return double2x2(y2, 0.0 - x2, 0.0 - y, x) * (1.0 / num);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static double determinant(double2x2 m)
	{
		double x = m.c0.x;
		double x2 = m.c1.x;
		double y = m.c0.y;
		double y2 = m.c1.y;
		return x * y2 - x2 * y;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static uint hash(double2x2 v)
	{
		return csum(fold_to_uint(v.c0) * uint2(4253034763u, 2338696631u) + fold_to_uint(v.c1) * uint2(3757372771u, 1885959949u)) + 3508684087u;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static uint2 hashwide(double2x2 v)
	{
		return fold_to_uint(v.c0) * uint2(3919501043u, 1209161033u) + fold_to_uint(v.c1) * uint2(4007793211u, 3819806693u) + 3458005183u;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static double2x3 double2x3(double2 c0, double2 c1, double2 c2)
	{
		return new double2x3(c0, c1, c2);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static double2x3 double2x3(double m00, double m01, double m02, double m10, double m11, double m12)
	{
		return new double2x3(m00, m01, m02, m10, m11, m12);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static double2x3 double2x3(double v)
	{
		return new double2x3(v);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static double2x3 double2x3(bool v)
	{
		return new double2x3(v);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static double2x3 double2x3(bool2x3 v)
	{
		return new double2x3(v);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static double2x3 double2x3(int v)
	{
		return new double2x3(v);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static double2x3 double2x3(int2x3 v)
	{
		return new double2x3(v);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static double2x3 double2x3(uint v)
	{
		return new double2x3(v);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static double2x3 double2x3(uint2x3 v)
	{
		return new double2x3(v);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static double2x3 double2x3(float v)
	{
		return new double2x3(v);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static double2x3 double2x3(float2x3 v)
	{
		return new double2x3(v);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static double3x2 transpose(double2x3 v)
	{
		return double3x2(v.c0.x, v.c0.y, v.c1.x, v.c1.y, v.c2.x, v.c2.y);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static uint hash(double2x3 v)
	{
		return csum(fold_to_uint(v.c0) * uint2(4066109527u, 2613148903u) + fold_to_uint(v.c1) * uint2(3367528529u, 1678332449u) + fold_to_uint(v.c2) * uint2(2918459647u, 2744611081u)) + 1952372791;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static uint2 hashwide(double2x3 v)
	{
		return fold_to_uint(v.c0) * uint2(2631698677u, 4200781601u) + fold_to_uint(v.c1) * uint2(2119021007u, 1760485621u) + fold_to_uint(v.c2) * uint2(3157985881u, 2171534173u) + 2723054263u;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static double2x4 double2x4(double2 c0, double2 c1, double2 c2, double2 c3)
	{
		return new double2x4(c0, c1, c2, c3);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static double2x4 double2x4(double m00, double m01, double m02, double m03, double m10, double m11, double m12, double m13)
	{
		return new double2x4(m00, m01, m02, m03, m10, m11, m12, m13);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static double2x4 double2x4(double v)
	{
		return new double2x4(v);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static double2x4 double2x4(bool v)
	{
		return new double2x4(v);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static double2x4 double2x4(bool2x4 v)
	{
		return new double2x4(v);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static double2x4 double2x4(int v)
	{
		return new double2x4(v);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static double2x4 double2x4(int2x4 v)
	{
		return new double2x4(v);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static double2x4 double2x4(uint v)
	{
		return new double2x4(v);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static double2x4 double2x4(uint2x4 v)
	{
		return new double2x4(v);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static double2x4 double2x4(float v)
	{
		return new double2x4(v);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static double2x4 double2x4(float2x4 v)
	{
		return new double2x4(v);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static double4x2 transpose(double2x4 v)
	{
		return double4x2(v.c0.x, v.c0.y, v.c1.x, v.c1.y, v.c2.x, v.c2.y, v.c3.x, v.c3.y);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static uint hash(double2x4 v)
	{
		return csum(fold_to_uint(v.c0) * uint2(2437373431u, 1441286183u) + fold_to_uint(v.c1) * uint2(2426570171u, 1561977301u) + fold_to_uint(v.c2) * uint2(4205774813u, 1650214333u) + fold_to_uint(v.c3) * uint2(3388112843u, 1831150513u)) + 1848374953;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static uint2 hashwide(double2x4 v)
	{
		return fold_to_uint(v.c0) * uint2(3430200247u, 2209710647u) + fold_to_uint(v.c1) * uint2(2201894441u, 2849577407u) + fold_to_uint(v.c2) * uint2(3287031191u, 3098675399u) + fold_to_uint(v.c3) * uint2(1564399943u, 1148435377u) + 3416333663u;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static double3 double3(double x, double y, double z)
	{
		return new double3(x, y, z);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static double3 double3(double x, double2 yz)
	{
		return new double3(x, yz);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static double3 double3(double2 xy, double z)
	{
		return new double3(xy, z);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static double3 double3(double3 xyz)
	{
		return new double3(xyz);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static double3 double3(double v)
	{
		return new double3(v);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static double3 double3(bool v)
	{
		return new double3(v);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static double3 double3(bool3 v)
	{
		return new double3(v);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static double3 double3(int v)
	{
		return new double3(v);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static double3 double3(int3 v)
	{
		return new double3(v);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static double3 double3(uint v)
	{
		return new double3(v);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static double3 double3(uint3 v)
	{
		return new double3(v);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static double3 double3(half v)
	{
		return new double3(v);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static double3 double3(half3 v)
	{
		return new double3(v);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static double3 double3(float v)
	{
		return new double3(v);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static double3 double3(float3 v)
	{
		return new double3(v);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static uint hash(double3 v)
	{
		return csum(fold_to_uint(v) * uint3(2937008387u, 3835713223u, 2216526373u)) + 3375971453u;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static uint3 hashwide(double3 v)
	{
		return fold_to_uint(v) * uint3(3559829411u, 3652178029u, 2544260129u) + 2013864031u;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static double shuffle(double3 a, double3 b, ShuffleComponent x)
	{
		return select_shuffle_component(a, b, x);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static double2 shuffle(double3 a, double3 b, ShuffleComponent x, ShuffleComponent y)
	{
		return double2(select_shuffle_component(a, b, x), select_shuffle_component(a, b, y));
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static double3 shuffle(double3 a, double3 b, ShuffleComponent x, ShuffleComponent y, ShuffleComponent z)
	{
		return double3(select_shuffle_component(a, b, x), select_shuffle_component(a, b, y), select_shuffle_component(a, b, z));
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static double4 shuffle(double3 a, double3 b, ShuffleComponent x, ShuffleComponent y, ShuffleComponent z, ShuffleComponent w)
	{
		return double4(select_shuffle_component(a, b, x), select_shuffle_component(a, b, y), select_shuffle_component(a, b, z), select_shuffle_component(a, b, w));
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	internal static double select_shuffle_component(double3 a, double3 b, ShuffleComponent component)
	{
		return component switch
		{
			ShuffleComponent.LeftX => a.x, 
			ShuffleComponent.LeftY => a.y, 
			ShuffleComponent.LeftZ => a.z, 
			ShuffleComponent.RightX => b.x, 
			ShuffleComponent.RightY => b.y, 
			ShuffleComponent.RightZ => b.z, 
			_ => throw new ArgumentException("Invalid shuffle component: " + component), 
		};
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static double3x2 double3x2(double3 c0, double3 c1)
	{
		return new double3x2(c0, c1);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static double3x2 double3x2(double m00, double m01, double m10, double m11, double m20, double m21)
	{
		return new double3x2(m00, m01, m10, m11, m20, m21);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static double3x2 double3x2(double v)
	{
		return new double3x2(v);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static double3x2 double3x2(bool v)
	{
		return new double3x2(v);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static double3x2 double3x2(bool3x2 v)
	{
		return new double3x2(v);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static double3x2 double3x2(int v)
	{
		return new double3x2(v);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static double3x2 double3x2(int3x2 v)
	{
		return new double3x2(v);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static double3x2 double3x2(uint v)
	{
		return new double3x2(v);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static double3x2 double3x2(uint3x2 v)
	{
		return new double3x2(v);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static double3x2 double3x2(float v)
	{
		return new double3x2(v);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static double3x2 double3x2(float3x2 v)
	{
		return new double3x2(v);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static double2x3 transpose(double3x2 v)
	{
		return double2x3(v.c0.x, v.c0.y, v.c0.z, v.c1.x, v.c1.y, v.c1.z);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static uint hash(double3x2 v)
	{
		return csum(fold_to_uint(v.c0) * uint3(3996716183u, 2626301701u, 1306289417u) + fold_to_uint(v.c1) * uint3(2096137163u, 1548578029u, 4178800919u)) + 3898072289u;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static uint3 hashwide(double3x2 v)
	{
		return fold_to_uint(v.c0) * uint3(4129428421u, 2631575897u, 2854656703u) + fold_to_uint(v.c1) * uint3(3578504047u, 4245178297u, 2173281923u) + 2973357649u;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static double3x3 double3x3(double3 c0, double3 c1, double3 c2)
	{
		return new double3x3(c0, c1, c2);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static double3x3 double3x3(double m00, double m01, double m02, double m10, double m11, double m12, double m20, double m21, double m22)
	{
		return new double3x3(m00, m01, m02, m10, m11, m12, m20, m21, m22);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static double3x3 double3x3(double v)
	{
		return new double3x3(v);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static double3x3 double3x3(bool v)
	{
		return new double3x3(v);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static double3x3 double3x3(bool3x3 v)
	{
		return new double3x3(v);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static double3x3 double3x3(int v)
	{
		return new double3x3(v);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static double3x3 double3x3(int3x3 v)
	{
		return new double3x3(v);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static double3x3 double3x3(uint v)
	{
		return new double3x3(v);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static double3x3 double3x3(uint3x3 v)
	{
		return new double3x3(v);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static double3x3 double3x3(float v)
	{
		return new double3x3(v);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static double3x3 double3x3(float3x3 v)
	{
		return new double3x3(v);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static double3x3 transpose(double3x3 v)
	{
		return double3x3(v.c0.x, v.c0.y, v.c0.z, v.c1.x, v.c1.y, v.c1.z, v.c2.x, v.c2.y, v.c2.z);
	}

	public static double3x3 inverse(double3x3 m)
	{
		double3 c = m.c0;
		double3 c2 = m.c1;
		double3 c3 = m.c2;
		double3 double5 = double3(c2.x, c3.x, c.x);
		double3 double6 = double3(c2.y, c3.y, c.y);
		double3 double7 = double3(c2.z, c3.z, c.z);
		double3 double8 = double6 * double7.yzx - double6.yzx * double7;
		double3 c4 = double5.yzx * double7 - double5 * double7.yzx;
		double3 c5 = double5 * double6.yzx - double5.yzx * double6;
		double num = 1.0 / csum(double5.zxy * double8);
		return double3x3(double8, c4, c5) * num;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static double determinant(double3x3 m)
	{
		double3 c = m.c0;
		double3 c2 = m.c1;
		double3 c3 = m.c2;
		double num = c2.y * c3.z - c2.z * c3.y;
		double num2 = c.y * c3.z - c.z * c3.y;
		double num3 = c.y * c2.z - c.z * c2.y;
		return c.x * num - c2.x * num2 + c3.x * num3;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static uint hash(double3x3 v)
	{
		return csum(fold_to_uint(v.c0) * uint3(2891822459u, 2837054189u, 3016004371u) + fold_to_uint(v.c1) * uint3(4097481403u, 2229788699u, 2382715877u) + fold_to_uint(v.c2) * uint3(1851936439u, 1938025801u, 3712598587u)) + 3956330501u;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static uint3 hashwide(double3x3 v)
	{
		return fold_to_uint(v.c0) * uint3(2437373431u, 1441286183u, 2426570171u) + fold_to_uint(v.c1) * uint3(1561977301u, 4205774813u, 1650214333u) + fold_to_uint(v.c2) * uint3(3388112843u, 1831150513u, 1848374953u) + 3430200247u;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static double3x4 double3x4(double3 c0, double3 c1, double3 c2, double3 c3)
	{
		return new double3x4(c0, c1, c2, c3);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static double3x4 double3x4(double m00, double m01, double m02, double m03, double m10, double m11, double m12, double m13, double m20, double m21, double m22, double m23)
	{
		return new double3x4(m00, m01, m02, m03, m10, m11, m12, m13, m20, m21, m22, m23);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static double3x4 double3x4(double v)
	{
		return new double3x4(v);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static double3x4 double3x4(bool v)
	{
		return new double3x4(v);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static double3x4 double3x4(bool3x4 v)
	{
		return new double3x4(v);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static double3x4 double3x4(int v)
	{
		return new double3x4(v);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static double3x4 double3x4(int3x4 v)
	{
		return new double3x4(v);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static double3x4 double3x4(uint v)
	{
		return new double3x4(v);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static double3x4 double3x4(uint3x4 v)
	{
		return new double3x4(v);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static double3x4 double3x4(float v)
	{
		return new double3x4(v);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static double3x4 double3x4(float3x4 v)
	{
		return new double3x4(v);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static double4x3 transpose(double3x4 v)
	{
		return double4x3(v.c0.x, v.c0.y, v.c0.z, v.c1.x, v.c1.y, v.c1.z, v.c2.x, v.c2.y, v.c2.z, v.c3.x, v.c3.y, v.c3.z);
	}

	public static double3x4 fastinverse(double3x4 m)
	{
		double3 c = m.c0;
		double3 c2 = m.c1;
		double3 c3 = m.c2;
		double3 c4 = m.c3;
		double3 double5 = double3(c.x, c2.x, c3.x);
		double3 double6 = double3(c.y, c2.y, c3.y);
		double3 double7 = double3(c.z, c2.z, c3.z);
		c4 = -(double5 * c4.x + double6 * c4.y + double7 * c4.z);
		return double3x4(double5, double6, double7, c4);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static uint hash(double3x4 v)
	{
		return csum(fold_to_uint(v.c0) * uint3(3996716183u, 2626301701u, 1306289417u) + fold_to_uint(v.c1) * uint3(2096137163u, 1548578029u, 4178800919u) + fold_to_uint(v.c2) * uint3(3898072289u, 4129428421u, 2631575897u) + fold_to_uint(v.c3) * uint3(2854656703u, 3578504047u, 4245178297u)) + 2173281923u;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static uint3 hashwide(double3x4 v)
	{
		return fold_to_uint(v.c0) * uint3(2973357649u, 3881277847u, 4017968839u) + fold_to_uint(v.c1) * uint3(1727237899u, 1648514723u, 1385344481u) + fold_to_uint(v.c2) * uint3(3538260197u, 4066109527u, 2613148903u) + fold_to_uint(v.c3) * uint3(3367528529u, 1678332449u, 2918459647u) + 2744611081u;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static double4 double4(double x, double y, double z, double w)
	{
		return new double4(x, y, z, w);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static double4 double4(double x, double y, double2 zw)
	{
		return new double4(x, y, zw);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static double4 double4(double x, double2 yz, double w)
	{
		return new double4(x, yz, w);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static double4 double4(double x, double3 yzw)
	{
		return new double4(x, yzw);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static double4 double4(double2 xy, double z, double w)
	{
		return new double4(xy, z, w);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static double4 double4(double2 xy, double2 zw)
	{
		return new double4(xy, zw);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static double4 double4(double3 xyz, double w)
	{
		return new double4(xyz, w);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static double4 double4(double4 xyzw)
	{
		return new double4(xyzw);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static double4 double4(double v)
	{
		return new double4(v);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static double4 double4(bool v)
	{
		return new double4(v);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static double4 double4(bool4 v)
	{
		return new double4(v);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static double4 double4(int v)
	{
		return new double4(v);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static double4 double4(int4 v)
	{
		return new double4(v);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static double4 double4(uint v)
	{
		return new double4(v);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static double4 double4(uint4 v)
	{
		return new double4(v);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static double4 double4(half v)
	{
		return new double4(v);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static double4 double4(half4 v)
	{
		return new double4(v);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static double4 double4(float v)
	{
		return new double4(v);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static double4 double4(float4 v)
	{
		return new double4(v);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static uint hash(double4 v)
	{
		return csum(fold_to_uint(v) * uint4(2669441947u, 1260114311u, 2650080659u, 4052675461u)) + 2652487619u;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static uint4 hashwide(double4 v)
	{
		return fold_to_uint(v) * uint4(2174136431u, 3528391193u, 2105559227u, 1899745391u) + 1966790317u;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static double shuffle(double4 a, double4 b, ShuffleComponent x)
	{
		return select_shuffle_component(a, b, x);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static double2 shuffle(double4 a, double4 b, ShuffleComponent x, ShuffleComponent y)
	{
		return double2(select_shuffle_component(a, b, x), select_shuffle_component(a, b, y));
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static double3 shuffle(double4 a, double4 b, ShuffleComponent x, ShuffleComponent y, ShuffleComponent z)
	{
		return double3(select_shuffle_component(a, b, x), select_shuffle_component(a, b, y), select_shuffle_component(a, b, z));
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static double4 shuffle(double4 a, double4 b, ShuffleComponent x, ShuffleComponent y, ShuffleComponent z, ShuffleComponent w)
	{
		return double4(select_shuffle_component(a, b, x), select_shuffle_component(a, b, y), select_shuffle_component(a, b, z), select_shuffle_component(a, b, w));
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	internal static double select_shuffle_component(double4 a, double4 b, ShuffleComponent component)
	{
		return component switch
		{
			ShuffleComponent.LeftX => a.x, 
			ShuffleComponent.LeftY => a.y, 
			ShuffleComponent.LeftZ => a.z, 
			ShuffleComponent.LeftW => a.w, 
			ShuffleComponent.RightX => b.x, 
			ShuffleComponent.RightY => b.y, 
			ShuffleComponent.RightZ => b.z, 
			ShuffleComponent.RightW => b.w, 
			_ => throw new ArgumentException("Invalid shuffle component: " + component), 
		};
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static double4x2 double4x2(double4 c0, double4 c1)
	{
		return new double4x2(c0, c1);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static double4x2 double4x2(double m00, double m01, double m10, double m11, double m20, double m21, double m30, double m31)
	{
		return new double4x2(m00, m01, m10, m11, m20, m21, m30, m31);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static double4x2 double4x2(double v)
	{
		return new double4x2(v);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static double4x2 double4x2(bool v)
	{
		return new double4x2(v);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static double4x2 double4x2(bool4x2 v)
	{
		return new double4x2(v);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static double4x2 double4x2(int v)
	{
		return new double4x2(v);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static double4x2 double4x2(int4x2 v)
	{
		return new double4x2(v);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static double4x2 double4x2(uint v)
	{
		return new double4x2(v);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static double4x2 double4x2(uint4x2 v)
	{
		return new double4x2(v);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static double4x2 double4x2(float v)
	{
		return new double4x2(v);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static double4x2 double4x2(float4x2 v)
	{
		return new double4x2(v);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static double2x4 transpose(double4x2 v)
	{
		return double2x4(v.c0.x, v.c0.y, v.c0.z, v.c0.w, v.c1.x, v.c1.y, v.c1.z, v.c1.w);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static uint hash(double4x2 v)
	{
		return csum(fold_to_uint(v.c0) * uint4(1521739981u, 1735296007u, 3010324327u, 1875523709u) + fold_to_uint(v.c1) * uint4(2937008387u, 3835713223u, 2216526373u, 3375971453u)) + 3559829411u;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static uint4 hashwide(double4x2 v)
	{
		return fold_to_uint(v.c0) * uint4(3652178029u, 2544260129u, 2013864031u, 2627668003u) + fold_to_uint(v.c1) * uint4(1520214331u, 2949502447u, 2827819133u, 3480140317u) + 2642994593u;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static double4x3 double4x3(double4 c0, double4 c1, double4 c2)
	{
		return new double4x3(c0, c1, c2);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static double4x3 double4x3(double m00, double m01, double m02, double m10, double m11, double m12, double m20, double m21, double m22, double m30, double m31, double m32)
	{
		return new double4x3(m00, m01, m02, m10, m11, m12, m20, m21, m22, m30, m31, m32);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static double4x3 double4x3(double v)
	{
		return new double4x3(v);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static double4x3 double4x3(bool v)
	{
		return new double4x3(v);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static double4x3 double4x3(bool4x3 v)
	{
		return new double4x3(v);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static double4x3 double4x3(int v)
	{
		return new double4x3(v);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static double4x3 double4x3(int4x3 v)
	{
		return new double4x3(v);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static double4x3 double4x3(uint v)
	{
		return new double4x3(v);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static double4x3 double4x3(uint4x3 v)
	{
		return new double4x3(v);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static double4x3 double4x3(float v)
	{
		return new double4x3(v);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static double4x3 double4x3(float4x3 v)
	{
		return new double4x3(v);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static double3x4 transpose(double4x3 v)
	{
		return double3x4(v.c0.x, v.c0.y, v.c0.z, v.c0.w, v.c1.x, v.c1.y, v.c1.z, v.c1.w, v.c2.x, v.c2.y, v.c2.z, v.c2.w);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static uint hash(double4x3 v)
	{
		return csum(fold_to_uint(v.c0) * uint4(2057338067u, 2942577577u, 2834440507u, 2671762487u) + fold_to_uint(v.c1) * uint4(2892026051u, 2455987759u, 3868600063u, 3170963179u) + fold_to_uint(v.c2) * uint4(2632835537u, 1136528209u, 2944626401u, 2972762423u)) + 1417889653;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static uint4 hashwide(double4x3 v)
	{
		return fold_to_uint(v.c0) * uint4(2080514593u, 2731544287u, 2828498809u, 2669441947u) + fold_to_uint(v.c1) * uint4(1260114311u, 2650080659u, 4052675461u, 2652487619u) + fold_to_uint(v.c2) * uint4(2174136431u, 3528391193u, 2105559227u, 1899745391u) + 1966790317u;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static double4x4 double4x4(double4 c0, double4 c1, double4 c2, double4 c3)
	{
		return new double4x4(c0, c1, c2, c3);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static double4x4 double4x4(double m00, double m01, double m02, double m03, double m10, double m11, double m12, double m13, double m20, double m21, double m22, double m23, double m30, double m31, double m32, double m33)
	{
		return new double4x4(m00, m01, m02, m03, m10, m11, m12, m13, m20, m21, m22, m23, m30, m31, m32, m33);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static double4x4 double4x4(double v)
	{
		return new double4x4(v);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static double4x4 double4x4(bool v)
	{
		return new double4x4(v);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static double4x4 double4x4(bool4x4 v)
	{
		return new double4x4(v);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static double4x4 double4x4(int v)
	{
		return new double4x4(v);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static double4x4 double4x4(int4x4 v)
	{
		return new double4x4(v);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static double4x4 double4x4(uint v)
	{
		return new double4x4(v);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static double4x4 double4x4(uint4x4 v)
	{
		return new double4x4(v);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static double4x4 double4x4(float v)
	{
		return new double4x4(v);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static double4x4 double4x4(float4x4 v)
	{
		return new double4x4(v);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static double3 rotate(double4x4 a, double3 b)
	{
		return (a.c0 * b.x + a.c1 * b.y + a.c2 * b.z).xyz;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static double3 transform(double4x4 a, double3 b)
	{
		return (a.c0 * b.x + a.c1 * b.y + a.c2 * b.z + a.c3).xyz;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static double4x4 transpose(double4x4 v)
	{
		return double4x4(v.c0.x, v.c0.y, v.c0.z, v.c0.w, v.c1.x, v.c1.y, v.c1.z, v.c1.w, v.c2.x, v.c2.y, v.c2.z, v.c2.w, v.c3.x, v.c3.y, v.c3.z, v.c3.w);
	}

	public static double4x4 inverse(double4x4 m)
	{
		double4 c = m.c0;
		double4 c2 = m.c1;
		double4 c3 = m.c2;
		double4 c4 = m.c3;
		double4 double5 = movelh(c2, c);
		double4 double6 = movelh(c3, c4);
		double4 double7 = movehl(c, c2);
		double4 double8 = movehl(c4, c3);
		double4 obj = shuffle(c2, c, ShuffleComponent.LeftY, ShuffleComponent.LeftZ, ShuffleComponent.RightY, ShuffleComponent.RightZ);
		double4 double9 = shuffle(c3, c4, ShuffleComponent.LeftY, ShuffleComponent.LeftZ, ShuffleComponent.RightY, ShuffleComponent.RightZ);
		double4 double10 = shuffle(c2, c, ShuffleComponent.LeftW, ShuffleComponent.LeftX, ShuffleComponent.RightW, ShuffleComponent.RightX);
		double4 double11 = shuffle(c3, c4, ShuffleComponent.LeftW, ShuffleComponent.LeftX, ShuffleComponent.RightW, ShuffleComponent.RightX);
		double4 double12 = shuffle(double6, double5, ShuffleComponent.LeftZ, ShuffleComponent.LeftX, ShuffleComponent.RightX, ShuffleComponent.RightZ);
		double4 double13 = shuffle(double6, double5, ShuffleComponent.LeftW, ShuffleComponent.LeftY, ShuffleComponent.RightY, ShuffleComponent.RightW);
		double4 double14 = shuffle(double8, double7, ShuffleComponent.LeftZ, ShuffleComponent.LeftX, ShuffleComponent.RightX, ShuffleComponent.RightZ);
		double4 double15 = shuffle(double8, double7, ShuffleComponent.LeftW, ShuffleComponent.LeftY, ShuffleComponent.RightY, ShuffleComponent.RightW);
		double4 double16 = shuffle(double5, double6, ShuffleComponent.LeftZ, ShuffleComponent.LeftX, ShuffleComponent.RightX, ShuffleComponent.RightZ);
		double4 obj2 = obj * double8 - double9 * double7;
		double4 double17 = double5 * double8 - double6 * double7;
		double4 double18 = double11 * double5 - double10 * double6;
		double4 double19 = shuffle(obj2, obj2, ShuffleComponent.LeftX, ShuffleComponent.LeftZ, ShuffleComponent.RightZ, ShuffleComponent.RightX);
		double4 double20 = shuffle(obj2, obj2, ShuffleComponent.LeftY, ShuffleComponent.LeftW, ShuffleComponent.RightW, ShuffleComponent.RightY);
		double4 double21 = shuffle(double17, double17, ShuffleComponent.LeftX, ShuffleComponent.LeftZ, ShuffleComponent.RightZ, ShuffleComponent.RightX);
		double4 double22 = shuffle(double17, double17, ShuffleComponent.LeftY, ShuffleComponent.LeftW, ShuffleComponent.RightW, ShuffleComponent.RightY);
		double4 double23 = double15 * double19 - double14 * double22 + double13 * double20;
		double4 double24 = double16 * double23;
		double24 += shuffle(double24, double24, ShuffleComponent.LeftY, ShuffleComponent.LeftX, ShuffleComponent.RightW, ShuffleComponent.RightZ);
		double24 -= shuffle(double24, double24, ShuffleComponent.LeftZ, ShuffleComponent.LeftZ, ShuffleComponent.RightX, ShuffleComponent.RightX);
		double4 double25 = double4(1.0) / double24;
		double4x4 result = default(double4x4);
		result.c0 = double23 * double25;
		double4 double26 = shuffle(double18, double18, ShuffleComponent.LeftX, ShuffleComponent.LeftZ, ShuffleComponent.RightZ, ShuffleComponent.RightX);
		double4 double27 = shuffle(double18, double18, ShuffleComponent.LeftY, ShuffleComponent.LeftW, ShuffleComponent.RightW, ShuffleComponent.RightY);
		double4 double28 = double14 * double26 - double12 * double20 - double15 * double21;
		result.c1 = double28 * double25;
		double4 double29 = double12 * double22 - double13 * double26 - double15 * double27;
		result.c2 = double29 * double25;
		double4 double30 = double13 * double21 - double12 * double19 + double14 * double27;
		result.c3 = double30 * double25;
		return result;
	}

	public static double4x4 fastinverse(double4x4 m)
	{
		double4 c = m.c0;
		double4 c2 = m.c1;
		double4 c3 = m.c2;
		double4 c4 = m.c3;
		double4 b = double4(0);
		double4 a = unpacklo(c, c3);
		double4 b2 = unpacklo(c2, b);
		double4 a2 = unpackhi(c, c3);
		double4 b3 = unpackhi(c2, b);
		double4 double5 = unpacklo(a, b2);
		double4 double6 = unpackhi(a, b2);
		double4 double7 = unpacklo(a2, b3);
		c4 = -(double5 * c4.x + double6 * c4.y + double7 * c4.z);
		c4.w = 1.0;
		return double4x4(double5, double6, double7, c4);
	}

	public static double determinant(double4x4 m)
	{
		double4 c = m.c0;
		double4 c2 = m.c1;
		double4 c3 = m.c2;
		double4 c4 = m.c3;
		double num = c2.y * (c3.z * c4.w - c3.w * c4.z) - c3.y * (c2.z * c4.w - c2.w * c4.z) + c4.y * (c2.z * c3.w - c2.w * c3.z);
		double num2 = c.y * (c3.z * c4.w - c3.w * c4.z) - c3.y * (c.z * c4.w - c.w * c4.z) + c4.y * (c.z * c3.w - c.w * c3.z);
		double num3 = c.y * (c2.z * c4.w - c2.w * c4.z) - c2.y * (c.z * c4.w - c.w * c4.z) + c4.y * (c.z * c2.w - c.w * c2.z);
		double num4 = c.y * (c2.z * c3.w - c2.w * c3.z) - c2.y * (c.z * c3.w - c.w * c3.z) + c3.y * (c.z * c2.w - c.w * c2.z);
		return c.x * num - c2.x * num2 + c3.x * num3 - c4.x * num4;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static uint hash(double4x4 v)
	{
		return csum(fold_to_uint(v.c0) * uint4(1306289417u, 2096137163u, 1548578029u, 4178800919u) + fold_to_uint(v.c1) * uint4(3898072289u, 4129428421u, 2631575897u, 2854656703u) + fold_to_uint(v.c2) * uint4(3578504047u, 4245178297u, 2173281923u, 2973357649u) + fold_to_uint(v.c3) * uint4(3881277847u, 4017968839u, 1727237899u, 1648514723u)) + 1385344481;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static uint4 hashwide(double4x4 v)
	{
		return fold_to_uint(v.c0) * uint4(3538260197u, 4066109527u, 2613148903u, 3367528529u) + fold_to_uint(v.c1) * uint4(1678332449u, 2918459647u, 2744611081u, 1952372791u) + fold_to_uint(v.c2) * uint4(2631698677u, 4200781601u, 2119021007u, 1760485621u) + fold_to_uint(v.c3) * uint4(3157985881u, 2171534173u, 2723054263u, 1168253063u) + 4228926523u;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static float2 float2(float x, float y)
	{
		return new float2(x, y);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static float2 float2(float2 xy)
	{
		return new float2(xy);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static float2 float2(float v)
	{
		return new float2(v);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static float2 float2(bool v)
	{
		return new float2(v);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static float2 float2(bool2 v)
	{
		return new float2(v);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static float2 float2(int v)
	{
		return new float2(v);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static float2 float2(int2 v)
	{
		return new float2(v);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static float2 float2(uint v)
	{
		return new float2(v);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static float2 float2(uint2 v)
	{
		return new float2(v);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static float2 float2(half v)
	{
		return new float2(v);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static float2 float2(half2 v)
	{
		return new float2(v);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static float2 float2(double v)
	{
		return new float2(v);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static float2 float2(double2 v)
	{
		return new float2(v);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static uint hash(float2 v)
	{
		return csum(asuint(v) * uint2(4198118021u, 2908068253u)) + 3705492289u;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static uint2 hashwide(float2 v)
	{
		return asuint(v) * uint2(2497566569u, 2716413241u) + 1166264321u;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static float shuffle(float2 a, float2 b, ShuffleComponent x)
	{
		return select_shuffle_component(a, b, x);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static float2 shuffle(float2 a, float2 b, ShuffleComponent x, ShuffleComponent y)
	{
		return float2(select_shuffle_component(a, b, x), select_shuffle_component(a, b, y));
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static float3 shuffle(float2 a, float2 b, ShuffleComponent x, ShuffleComponent y, ShuffleComponent z)
	{
		return float3(select_shuffle_component(a, b, x), select_shuffle_component(a, b, y), select_shuffle_component(a, b, z));
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static float4 shuffle(float2 a, float2 b, ShuffleComponent x, ShuffleComponent y, ShuffleComponent z, ShuffleComponent w)
	{
		return float4(select_shuffle_component(a, b, x), select_shuffle_component(a, b, y), select_shuffle_component(a, b, z), select_shuffle_component(a, b, w));
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	internal static float select_shuffle_component(float2 a, float2 b, ShuffleComponent component)
	{
		return component switch
		{
			ShuffleComponent.LeftX => a.x, 
			ShuffleComponent.LeftY => a.y, 
			ShuffleComponent.RightX => b.x, 
			ShuffleComponent.RightY => b.y, 
			_ => throw new ArgumentException("Invalid shuffle component: " + component), 
		};
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static float2x2 float2x2(float2 c0, float2 c1)
	{
		return new float2x2(c0, c1);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static float2x2 float2x2(float m00, float m01, float m10, float m11)
	{
		return new float2x2(m00, m01, m10, m11);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static float2x2 float2x2(float v)
	{
		return new float2x2(v);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static float2x2 float2x2(bool v)
	{
		return new float2x2(v);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static float2x2 float2x2(bool2x2 v)
	{
		return new float2x2(v);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static float2x2 float2x2(int v)
	{
		return new float2x2(v);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static float2x2 float2x2(int2x2 v)
	{
		return new float2x2(v);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static float2x2 float2x2(uint v)
	{
		return new float2x2(v);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static float2x2 float2x2(uint2x2 v)
	{
		return new float2x2(v);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static float2x2 float2x2(double v)
	{
		return new float2x2(v);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static float2x2 float2x2(double2x2 v)
	{
		return new float2x2(v);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static float2x2 transpose(float2x2 v)
	{
		return float2x2(v.c0.x, v.c0.y, v.c1.x, v.c1.y);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static float2x2 inverse(float2x2 m)
	{
		float x = m.c0.x;
		float x2 = m.c1.x;
		float y = m.c0.y;
		float y2 = m.c1.y;
		float num = x * y2 - x2 * y;
		return float2x2(y2, 0f - x2, 0f - y, x) * (1f / num);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static float determinant(float2x2 m)
	{
		float x = m.c0.x;
		float x2 = m.c1.x;
		float y = m.c0.y;
		float y2 = m.c1.y;
		return x * y2 - x2 * y;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static uint hash(float2x2 v)
	{
		return csum(asuint(v.c0) * uint2(2627668003u, 1520214331u) + asuint(v.c1) * uint2(2949502447u, 2827819133u)) + 3480140317u;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static uint2 hashwide(float2x2 v)
	{
		return asuint(v.c0) * uint2(2642994593u, 3940484981u) + asuint(v.c1) * uint2(1954192763u, 1091696537u) + 3052428017u;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static float2x3 float2x3(float2 c0, float2 c1, float2 c2)
	{
		return new float2x3(c0, c1, c2);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static float2x3 float2x3(float m00, float m01, float m02, float m10, float m11, float m12)
	{
		return new float2x3(m00, m01, m02, m10, m11, m12);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static float2x3 float2x3(float v)
	{
		return new float2x3(v);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static float2x3 float2x3(bool v)
	{
		return new float2x3(v);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static float2x3 float2x3(bool2x3 v)
	{
		return new float2x3(v);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static float2x3 float2x3(int v)
	{
		return new float2x3(v);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static float2x3 float2x3(int2x3 v)
	{
		return new float2x3(v);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static float2x3 float2x3(uint v)
	{
		return new float2x3(v);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static float2x3 float2x3(uint2x3 v)
	{
		return new float2x3(v);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static float2x3 float2x3(double v)
	{
		return new float2x3(v);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static float2x3 float2x3(double2x3 v)
	{
		return new float2x3(v);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static float3x2 transpose(float2x3 v)
	{
		return float3x2(v.c0.x, v.c0.y, v.c1.x, v.c1.y, v.c2.x, v.c2.y);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static uint hash(float2x3 v)
	{
		return csum(asuint(v.c0) * uint2(3898072289u, 4129428421u) + asuint(v.c1) * uint2(2631575897u, 2854656703u) + asuint(v.c2) * uint2(3578504047u, 4245178297u)) + 2173281923u;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static uint2 hashwide(float2x3 v)
	{
		return asuint(v.c0) * uint2(2973357649u, 3881277847u) + asuint(v.c1) * uint2(4017968839u, 1727237899u) + asuint(v.c2) * uint2(1648514723u, 1385344481u) + 3538260197u;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static float2x4 float2x4(float2 c0, float2 c1, float2 c2, float2 c3)
	{
		return new float2x4(c0, c1, c2, c3);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static float2x4 float2x4(float m00, float m01, float m02, float m03, float m10, float m11, float m12, float m13)
	{
		return new float2x4(m00, m01, m02, m03, m10, m11, m12, m13);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static float2x4 float2x4(float v)
	{
		return new float2x4(v);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static float2x4 float2x4(bool v)
	{
		return new float2x4(v);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static float2x4 float2x4(bool2x4 v)
	{
		return new float2x4(v);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static float2x4 float2x4(int v)
	{
		return new float2x4(v);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static float2x4 float2x4(int2x4 v)
	{
		return new float2x4(v);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static float2x4 float2x4(uint v)
	{
		return new float2x4(v);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static float2x4 float2x4(uint2x4 v)
	{
		return new float2x4(v);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static float2x4 float2x4(double v)
	{
		return new float2x4(v);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static float2x4 float2x4(double2x4 v)
	{
		return new float2x4(v);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static float4x2 transpose(float2x4 v)
	{
		return float4x2(v.c0.x, v.c0.y, v.c1.x, v.c1.y, v.c2.x, v.c2.y, v.c3.x, v.c3.y);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static uint hash(float2x4 v)
	{
		return csum(asuint(v.c0) * uint2(3546061613u, 2702024231u) + asuint(v.c1) * uint2(1452124841u, 1966089551u) + asuint(v.c2) * uint2(2668168249u, 1587512777u) + asuint(v.c3) * uint2(2353831999u, 3101256173u)) + 2891822459u;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static uint2 hashwide(float2x4 v)
	{
		return asuint(v.c0) * uint2(2837054189u, 3016004371u) + asuint(v.c1) * uint2(4097481403u, 2229788699u) + asuint(v.c2) * uint2(2382715877u, 1851936439u) + asuint(v.c3) * uint2(1938025801u, 3712598587u) + 3956330501u;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static float3 float3(float x, float y, float z)
	{
		return new float3(x, y, z);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static float3 float3(float x, float2 yz)
	{
		return new float3(x, yz);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static float3 float3(float2 xy, float z)
	{
		return new float3(xy, z);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static float3 float3(float3 xyz)
	{
		return new float3(xyz);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static float3 float3(float v)
	{
		return new float3(v);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static float3 float3(bool v)
	{
		return new float3(v);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static float3 float3(bool3 v)
	{
		return new float3(v);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static float3 float3(int v)
	{
		return new float3(v);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static float3 float3(int3 v)
	{
		return new float3(v);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static float3 float3(uint v)
	{
		return new float3(v);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static float3 float3(uint3 v)
	{
		return new float3(v);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static float3 float3(half v)
	{
		return new float3(v);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static float3 float3(half3 v)
	{
		return new float3(v);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static float3 float3(double v)
	{
		return new float3(v);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static float3 float3(double3 v)
	{
		return new float3(v);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static uint hash(float3 v)
	{
		return csum(asuint(v) * uint3(2601761069u, 1254033427u, 2248573027u)) + 3612677113u;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static uint3 hashwide(float3 v)
	{
		return asuint(v) * uint3(1521739981u, 1735296007u, 3010324327u) + 1875523709u;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static float shuffle(float3 a, float3 b, ShuffleComponent x)
	{
		return select_shuffle_component(a, b, x);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static float2 shuffle(float3 a, float3 b, ShuffleComponent x, ShuffleComponent y)
	{
		return float2(select_shuffle_component(a, b, x), select_shuffle_component(a, b, y));
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static float3 shuffle(float3 a, float3 b, ShuffleComponent x, ShuffleComponent y, ShuffleComponent z)
	{
		return float3(select_shuffle_component(a, b, x), select_shuffle_component(a, b, y), select_shuffle_component(a, b, z));
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static float4 shuffle(float3 a, float3 b, ShuffleComponent x, ShuffleComponent y, ShuffleComponent z, ShuffleComponent w)
	{
		return float4(select_shuffle_component(a, b, x), select_shuffle_component(a, b, y), select_shuffle_component(a, b, z), select_shuffle_component(a, b, w));
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	internal static float select_shuffle_component(float3 a, float3 b, ShuffleComponent component)
	{
		return component switch
		{
			ShuffleComponent.LeftX => a.x, 
			ShuffleComponent.LeftY => a.y, 
			ShuffleComponent.LeftZ => a.z, 
			ShuffleComponent.RightX => b.x, 
			ShuffleComponent.RightY => b.y, 
			ShuffleComponent.RightZ => b.z, 
			_ => throw new ArgumentException("Invalid shuffle component: " + component), 
		};
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static float3x2 float3x2(float3 c0, float3 c1)
	{
		return new float3x2(c0, c1);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static float3x2 float3x2(float m00, float m01, float m10, float m11, float m20, float m21)
	{
		return new float3x2(m00, m01, m10, m11, m20, m21);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static float3x2 float3x2(float v)
	{
		return new float3x2(v);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static float3x2 float3x2(bool v)
	{
		return new float3x2(v);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static float3x2 float3x2(bool3x2 v)
	{
		return new float3x2(v);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static float3x2 float3x2(int v)
	{
		return new float3x2(v);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static float3x2 float3x2(int3x2 v)
	{
		return new float3x2(v);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static float3x2 float3x2(uint v)
	{
		return new float3x2(v);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static float3x2 float3x2(uint3x2 v)
	{
		return new float3x2(v);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static float3x2 float3x2(double v)
	{
		return new float3x2(v);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static float3x2 float3x2(double3x2 v)
	{
		return new float3x2(v);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static float2x3 transpose(float3x2 v)
	{
		return float2x3(v.c0.x, v.c0.y, v.c0.z, v.c1.x, v.c1.y, v.c1.z);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static uint hash(float3x2 v)
	{
		return csum(asuint(v.c0) * uint3(3777095341u, 3385463369u, 1773538433u) + asuint(v.c1) * uint3(3773525029u, 4131962539u, 1809525511u)) + 4016293529u;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static uint3 hashwide(float3x2 v)
	{
		return asuint(v.c0) * uint3(2416021567u, 2828384717u, 2636362241u) + asuint(v.c1) * uint3(1258410977u, 1952565773u, 2037535609u) + 3592785499u;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static float3x3 float3x3(float3 c0, float3 c1, float3 c2)
	{
		return new float3x3(c0, c1, c2);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static float3x3 float3x3(float m00, float m01, float m02, float m10, float m11, float m12, float m20, float m21, float m22)
	{
		return new float3x3(m00, m01, m02, m10, m11, m12, m20, m21, m22);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static float3x3 float3x3(float v)
	{
		return new float3x3(v);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static float3x3 float3x3(bool v)
	{
		return new float3x3(v);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static float3x3 float3x3(bool3x3 v)
	{
		return new float3x3(v);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static float3x3 float3x3(int v)
	{
		return new float3x3(v);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static float3x3 float3x3(int3x3 v)
	{
		return new float3x3(v);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static float3x3 float3x3(uint v)
	{
		return new float3x3(v);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static float3x3 float3x3(uint3x3 v)
	{
		return new float3x3(v);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static float3x3 float3x3(double v)
	{
		return new float3x3(v);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static float3x3 float3x3(double3x3 v)
	{
		return new float3x3(v);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static float3x3 transpose(float3x3 v)
	{
		return float3x3(v.c0.x, v.c0.y, v.c0.z, v.c1.x, v.c1.y, v.c1.z, v.c2.x, v.c2.y, v.c2.z);
	}

	public static float3x3 inverse(float3x3 m)
	{
		float3 c = m.c0;
		float3 c2 = m.c1;
		float3 c3 = m.c2;
		float3 float5 = float3(c2.x, c3.x, c.x);
		float3 float6 = float3(c2.y, c3.y, c.y);
		float3 float7 = float3(c2.z, c3.z, c.z);
		float3 float8 = float6 * float7.yzx - float6.yzx * float7;
		float3 c4 = float5.yzx * float7 - float5 * float7.yzx;
		float3 c5 = float5 * float6.yzx - float5.yzx * float6;
		float num = 1f / csum(float5.zxy * float8);
		return float3x3(float8, c4, c5) * num;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static float determinant(float3x3 m)
	{
		float3 c = m.c0;
		float3 c2 = m.c1;
		float3 c3 = m.c2;
		float num = c2.y * c3.z - c2.z * c3.y;
		float num2 = c.y * c3.z - c.z * c3.y;
		float num3 = c.y * c2.z - c.z * c2.y;
		return c.x * num - c2.x * num2 + c3.x * num3;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static uint hash(float3x3 v)
	{
		return csum(asuint(v.c0) * uint3(1899745391u, 1966790317u, 3516359879u) + asuint(v.c1) * uint3(3050356579u, 4178586719u, 2558655391u) + asuint(v.c2) * uint3(1453413133u, 2152428077u, 1938706661u)) + 1338588197;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static uint3 hashwide(float3x3 v)
	{
		return asuint(v.c0) * uint3(3439609253u, 3535343003u, 3546061613u) + asuint(v.c1) * uint3(2702024231u, 1452124841u, 1966089551u) + asuint(v.c2) * uint3(2668168249u, 1587512777u, 2353831999u) + 3101256173u;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static float3x4 float3x4(float3 c0, float3 c1, float3 c2, float3 c3)
	{
		return new float3x4(c0, c1, c2, c3);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static float3x4 float3x4(float m00, float m01, float m02, float m03, float m10, float m11, float m12, float m13, float m20, float m21, float m22, float m23)
	{
		return new float3x4(m00, m01, m02, m03, m10, m11, m12, m13, m20, m21, m22, m23);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static float3x4 float3x4(float v)
	{
		return new float3x4(v);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static float3x4 float3x4(bool v)
	{
		return new float3x4(v);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static float3x4 float3x4(bool3x4 v)
	{
		return new float3x4(v);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static float3x4 float3x4(int v)
	{
		return new float3x4(v);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static float3x4 float3x4(int3x4 v)
	{
		return new float3x4(v);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static float3x4 float3x4(uint v)
	{
		return new float3x4(v);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static float3x4 float3x4(uint3x4 v)
	{
		return new float3x4(v);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static float3x4 float3x4(double v)
	{
		return new float3x4(v);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static float3x4 float3x4(double3x4 v)
	{
		return new float3x4(v);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static float4x3 transpose(float3x4 v)
	{
		return float4x3(v.c0.x, v.c0.y, v.c0.z, v.c1.x, v.c1.y, v.c1.z, v.c2.x, v.c2.y, v.c2.z, v.c3.x, v.c3.y, v.c3.z);
	}

	public static float3x4 fastinverse(float3x4 m)
	{
		float3 c = m.c0;
		float3 c2 = m.c1;
		float3 c3 = m.c2;
		float3 c4 = m.c3;
		float3 float5 = float3(c.x, c2.x, c3.x);
		float3 float6 = float3(c.y, c2.y, c3.y);
		float3 float7 = float3(c.z, c2.z, c3.z);
		c4 = -(float5 * c4.x + float6 * c4.y + float7 * c4.z);
		return float3x4(float5, float6, float7, c4);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static uint hash(float3x4 v)
	{
		return csum(asuint(v.c0) * uint3(4192899797u, 3271228601u, 1634639009u) + asuint(v.c1) * uint3(3318036811u, 3404170631u, 2048213449u) + asuint(v.c2) * uint3(4164671783u, 1780759499u, 1352369353u) + asuint(v.c3) * uint3(2446407751u, 1391928079u, 3475533443u)) + 3777095341u;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static uint3 hashwide(float3x4 v)
	{
		return asuint(v.c0) * uint3(3385463369u, 1773538433u, 3773525029u) + asuint(v.c1) * uint3(4131962539u, 1809525511u, 4016293529u) + asuint(v.c2) * uint3(2416021567u, 2828384717u, 2636362241u) + asuint(v.c3) * uint3(1258410977u, 1952565773u, 2037535609u) + 3592785499u;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static float4 float4(float x, float y, float z, float w)
	{
		return new float4(x, y, z, w);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static float4 float4(float x, float y, float2 zw)
	{
		return new float4(x, y, zw);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static float4 float4(float x, float2 yz, float w)
	{
		return new float4(x, yz, w);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static float4 float4(float x, float3 yzw)
	{
		return new float4(x, yzw);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static float4 float4(float2 xy, float z, float w)
	{
		return new float4(xy, z, w);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static float4 float4(float2 xy, float2 zw)
	{
		return new float4(xy, zw);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static float4 float4(float3 xyz, float w)
	{
		return new float4(xyz, w);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static float4 float4(float4 xyzw)
	{
		return new float4(xyzw);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static float4 float4(float v)
	{
		return new float4(v);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static float4 float4(bool v)
	{
		return new float4(v);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static float4 float4(bool4 v)
	{
		return new float4(v);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static float4 float4(int v)
	{
		return new float4(v);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static float4 float4(int4 v)
	{
		return new float4(v);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static float4 float4(uint v)
	{
		return new float4(v);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static float4 float4(uint4 v)
	{
		return new float4(v);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static float4 float4(half v)
	{
		return new float4(v);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static float4 float4(half4 v)
	{
		return new float4(v);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static float4 float4(double v)
	{
		return new float4(v);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static float4 float4(double4 v)
	{
		return new float4(v);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static uint hash(float4 v)
	{
		return csum(asuint(v) * uint4(3868600063u, 3170963179u, 2632835537u, 1136528209u)) + 2944626401u;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static uint4 hashwide(float4 v)
	{
		return asuint(v) * uint4(2972762423u, 1417889653u, 2080514593u, 2731544287u) + 2828498809u;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static float shuffle(float4 a, float4 b, ShuffleComponent x)
	{
		return select_shuffle_component(a, b, x);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static float2 shuffle(float4 a, float4 b, ShuffleComponent x, ShuffleComponent y)
	{
		return float2(select_shuffle_component(a, b, x), select_shuffle_component(a, b, y));
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static float3 shuffle(float4 a, float4 b, ShuffleComponent x, ShuffleComponent y, ShuffleComponent z)
	{
		return float3(select_shuffle_component(a, b, x), select_shuffle_component(a, b, y), select_shuffle_component(a, b, z));
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static float4 shuffle(float4 a, float4 b, ShuffleComponent x, ShuffleComponent y, ShuffleComponent z, ShuffleComponent w)
	{
		return float4(select_shuffle_component(a, b, x), select_shuffle_component(a, b, y), select_shuffle_component(a, b, z), select_shuffle_component(a, b, w));
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	internal static float select_shuffle_component(float4 a, float4 b, ShuffleComponent component)
	{
		return component switch
		{
			ShuffleComponent.LeftX => a.x, 
			ShuffleComponent.LeftY => a.y, 
			ShuffleComponent.LeftZ => a.z, 
			ShuffleComponent.LeftW => a.w, 
			ShuffleComponent.RightX => b.x, 
			ShuffleComponent.RightY => b.y, 
			ShuffleComponent.RightZ => b.z, 
			ShuffleComponent.RightW => b.w, 
			_ => throw new ArgumentException("Invalid shuffle component: " + component), 
		};
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static float4x2 float4x2(float4 c0, float4 c1)
	{
		return new float4x2(c0, c1);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static float4x2 float4x2(float m00, float m01, float m10, float m11, float m20, float m21, float m30, float m31)
	{
		return new float4x2(m00, m01, m10, m11, m20, m21, m30, m31);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static float4x2 float4x2(float v)
	{
		return new float4x2(v);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static float4x2 float4x2(bool v)
	{
		return new float4x2(v);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static float4x2 float4x2(bool4x2 v)
	{
		return new float4x2(v);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static float4x2 float4x2(int v)
	{
		return new float4x2(v);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static float4x2 float4x2(int4x2 v)
	{
		return new float4x2(v);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static float4x2 float4x2(uint v)
	{
		return new float4x2(v);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static float4x2 float4x2(uint4x2 v)
	{
		return new float4x2(v);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static float4x2 float4x2(double v)
	{
		return new float4x2(v);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static float4x2 float4x2(double4x2 v)
	{
		return new float4x2(v);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static float2x4 transpose(float4x2 v)
	{
		return float2x4(v.c0.x, v.c0.y, v.c0.z, v.c0.w, v.c1.x, v.c1.y, v.c1.z, v.c1.w);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static uint hash(float4x2 v)
	{
		return csum(asuint(v.c0) * uint4(2864955997u, 3525118277u, 2298260269u, 1632478733u) + asuint(v.c1) * uint4(1537393931u, 2353355467u, 3441847433u, 4052036147u)) + 2011389559;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static uint4 hashwide(float4x2 v)
	{
		return asuint(v.c0) * uint4(2252224297u, 3784421429u, 1750626223u, 3571447507u) + asuint(v.c1) * uint4(3412283213u, 2601761069u, 1254033427u, 2248573027u) + 3612677113u;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static float4x3 float4x3(float4 c0, float4 c1, float4 c2)
	{
		return new float4x3(c0, c1, c2);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static float4x3 float4x3(float m00, float m01, float m02, float m10, float m11, float m12, float m20, float m21, float m22, float m30, float m31, float m32)
	{
		return new float4x3(m00, m01, m02, m10, m11, m12, m20, m21, m22, m30, m31, m32);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static float4x3 float4x3(float v)
	{
		return new float4x3(v);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static float4x3 float4x3(bool v)
	{
		return new float4x3(v);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static float4x3 float4x3(bool4x3 v)
	{
		return new float4x3(v);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static float4x3 float4x3(int v)
	{
		return new float4x3(v);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static float4x3 float4x3(int4x3 v)
	{
		return new float4x3(v);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static float4x3 float4x3(uint v)
	{
		return new float4x3(v);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static float4x3 float4x3(uint4x3 v)
	{
		return new float4x3(v);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static float4x3 float4x3(double v)
	{
		return new float4x3(v);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static float4x3 float4x3(double4x3 v)
	{
		return new float4x3(v);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static float3x4 transpose(float4x3 v)
	{
		return float3x4(v.c0.x, v.c0.y, v.c0.z, v.c0.w, v.c1.x, v.c1.y, v.c1.z, v.c1.w, v.c2.x, v.c2.y, v.c2.z, v.c2.w);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static uint hash(float4x3 v)
	{
		return csum(asuint(v.c0) * uint4(3309258581u, 1770373673u, 3778261171u, 3286279097u) + asuint(v.c1) * uint4(4264629071u, 1898591447u, 2641864091u, 1229113913u) + asuint(v.c2) * uint4(3020867117u, 1449055807u, 2479033387u, 3702457169u)) + 1845824257;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static uint4 hashwide(float4x3 v)
	{
		return asuint(v.c0) * uint4(1963973621u, 2134758553u, 1391111867u, 1167706003u) + asuint(v.c1) * uint4(2209736489u, 3261535807u, 1740411209u, 2910609089u) + asuint(v.c2) * uint4(2183822701u, 3029516053u, 3547472099u, 2057487037u) + 3781937309u;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static float4x4 float4x4(float4 c0, float4 c1, float4 c2, float4 c3)
	{
		return new float4x4(c0, c1, c2, c3);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static float4x4 float4x4(float m00, float m01, float m02, float m03, float m10, float m11, float m12, float m13, float m20, float m21, float m22, float m23, float m30, float m31, float m32, float m33)
	{
		return new float4x4(m00, m01, m02, m03, m10, m11, m12, m13, m20, m21, m22, m23, m30, m31, m32, m33);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static float4x4 float4x4(float v)
	{
		return new float4x4(v);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static float4x4 float4x4(bool v)
	{
		return new float4x4(v);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static float4x4 float4x4(bool4x4 v)
	{
		return new float4x4(v);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static float4x4 float4x4(int v)
	{
		return new float4x4(v);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static float4x4 float4x4(int4x4 v)
	{
		return new float4x4(v);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static float4x4 float4x4(uint v)
	{
		return new float4x4(v);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static float4x4 float4x4(uint4x4 v)
	{
		return new float4x4(v);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static float4x4 float4x4(double v)
	{
		return new float4x4(v);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static float4x4 float4x4(double4x4 v)
	{
		return new float4x4(v);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static float3 rotate(float4x4 a, float3 b)
	{
		return (a.c0 * b.x + a.c1 * b.y + a.c2 * b.z).xyz;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static float3 transform(float4x4 a, float3 b)
	{
		return (a.c0 * b.x + a.c1 * b.y + a.c2 * b.z + a.c3).xyz;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static float4x4 transpose(float4x4 v)
	{
		return float4x4(v.c0.x, v.c0.y, v.c0.z, v.c0.w, v.c1.x, v.c1.y, v.c1.z, v.c1.w, v.c2.x, v.c2.y, v.c2.z, v.c2.w, v.c3.x, v.c3.y, v.c3.z, v.c3.w);
	}

	public static float4x4 inverse(float4x4 m)
	{
		float4 c = m.c0;
		float4 c2 = m.c1;
		float4 c3 = m.c2;
		float4 c4 = m.c3;
		float4 float5 = movelh(c2, c);
		float4 float6 = movelh(c3, c4);
		float4 float7 = movehl(c, c2);
		float4 float8 = movehl(c4, c3);
		float4 obj = shuffle(c2, c, ShuffleComponent.LeftY, ShuffleComponent.LeftZ, ShuffleComponent.RightY, ShuffleComponent.RightZ);
		float4 float9 = shuffle(c3, c4, ShuffleComponent.LeftY, ShuffleComponent.LeftZ, ShuffleComponent.RightY, ShuffleComponent.RightZ);
		float4 float10 = shuffle(c2, c, ShuffleComponent.LeftW, ShuffleComponent.LeftX, ShuffleComponent.RightW, ShuffleComponent.RightX);
		float4 float11 = shuffle(c3, c4, ShuffleComponent.LeftW, ShuffleComponent.LeftX, ShuffleComponent.RightW, ShuffleComponent.RightX);
		float4 float12 = shuffle(float6, float5, ShuffleComponent.LeftZ, ShuffleComponent.LeftX, ShuffleComponent.RightX, ShuffleComponent.RightZ);
		float4 float13 = shuffle(float6, float5, ShuffleComponent.LeftW, ShuffleComponent.LeftY, ShuffleComponent.RightY, ShuffleComponent.RightW);
		float4 float14 = shuffle(float8, float7, ShuffleComponent.LeftZ, ShuffleComponent.LeftX, ShuffleComponent.RightX, ShuffleComponent.RightZ);
		float4 float15 = shuffle(float8, float7, ShuffleComponent.LeftW, ShuffleComponent.LeftY, ShuffleComponent.RightY, ShuffleComponent.RightW);
		float4 float16 = shuffle(float5, float6, ShuffleComponent.LeftZ, ShuffleComponent.LeftX, ShuffleComponent.RightX, ShuffleComponent.RightZ);
		float4 obj2 = obj * float8 - float9 * float7;
		float4 float17 = float5 * float8 - float6 * float7;
		float4 float18 = float11 * float5 - float10 * float6;
		float4 float19 = shuffle(obj2, obj2, ShuffleComponent.LeftX, ShuffleComponent.LeftZ, ShuffleComponent.RightZ, ShuffleComponent.RightX);
		float4 float20 = shuffle(obj2, obj2, ShuffleComponent.LeftY, ShuffleComponent.LeftW, ShuffleComponent.RightW, ShuffleComponent.RightY);
		float4 float21 = shuffle(float17, float17, ShuffleComponent.LeftX, ShuffleComponent.LeftZ, ShuffleComponent.RightZ, ShuffleComponent.RightX);
		float4 float22 = shuffle(float17, float17, ShuffleComponent.LeftY, ShuffleComponent.LeftW, ShuffleComponent.RightW, ShuffleComponent.RightY);
		float4 float23 = float15 * float19 - float14 * float22 + float13 * float20;
		float4 float24 = float16 * float23;
		float24 += shuffle(float24, float24, ShuffleComponent.LeftY, ShuffleComponent.LeftX, ShuffleComponent.RightW, ShuffleComponent.RightZ);
		float24 -= shuffle(float24, float24, ShuffleComponent.LeftZ, ShuffleComponent.LeftZ, ShuffleComponent.RightX, ShuffleComponent.RightX);
		float4 float25 = float4(1f) / float24;
		float4x4 result = default(float4x4);
		result.c0 = float23 * float25;
		float4 float26 = shuffle(float18, float18, ShuffleComponent.LeftX, ShuffleComponent.LeftZ, ShuffleComponent.RightZ, ShuffleComponent.RightX);
		float4 float27 = shuffle(float18, float18, ShuffleComponent.LeftY, ShuffleComponent.LeftW, ShuffleComponent.RightW, ShuffleComponent.RightY);
		float4 float28 = float14 * float26 - float12 * float20 - float15 * float21;
		result.c1 = float28 * float25;
		float4 float29 = float12 * float22 - float13 * float26 - float15 * float27;
		result.c2 = float29 * float25;
		float4 float30 = float13 * float21 - float12 * float19 + float14 * float27;
		result.c3 = float30 * float25;
		return result;
	}

	public static float4x4 fastinverse(float4x4 m)
	{
		float4 c = m.c0;
		float4 c2 = m.c1;
		float4 c3 = m.c2;
		float4 c4 = m.c3;
		float4 b = float4(0);
		float4 a = unpacklo(c, c3);
		float4 b2 = unpacklo(c2, b);
		float4 a2 = unpackhi(c, c3);
		float4 b3 = unpackhi(c2, b);
		float4 float5 = unpacklo(a, b2);
		float4 float6 = unpackhi(a, b2);
		float4 float7 = unpacklo(a2, b3);
		c4 = -(float5 * c4.x + float6 * c4.y + float7 * c4.z);
		c4.w = 1f;
		return float4x4(float5, float6, float7, c4);
	}

	public static float determinant(float4x4 m)
	{
		float4 c = m.c0;
		float4 c2 = m.c1;
		float4 c3 = m.c2;
		float4 c4 = m.c3;
		float num = c2.y * (c3.z * c4.w - c3.w * c4.z) - c3.y * (c2.z * c4.w - c2.w * c4.z) + c4.y * (c2.z * c3.w - c2.w * c3.z);
		float num2 = c.y * (c3.z * c4.w - c3.w * c4.z) - c3.y * (c.z * c4.w - c.w * c4.z) + c4.y * (c.z * c3.w - c.w * c3.z);
		float num3 = c.y * (c2.z * c4.w - c2.w * c4.z) - c2.y * (c.z * c4.w - c.w * c4.z) + c4.y * (c.z * c2.w - c.w * c2.z);
		float num4 = c.y * (c2.z * c3.w - c2.w * c3.z) - c2.y * (c.z * c3.w - c.w * c3.z) + c3.y * (c.z * c2.w - c.w * c2.z);
		return c.x * num - c2.x * num2 + c3.x * num3 - c4.x * num4;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static uint hash(float4x4 v)
	{
		return csum(asuint(v.c0) * uint4(3299952959u, 3121178323u, 2948522579u, 1531026433u) + asuint(v.c1) * uint4(1365086453u, 3969870067u, 4192899797u, 3271228601u) + asuint(v.c2) * uint4(1634639009u, 3318036811u, 3404170631u, 2048213449u) + asuint(v.c3) * uint4(4164671783u, 1780759499u, 1352369353u, 2446407751u)) + 1391928079;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static uint4 hashwide(float4x4 v)
	{
		return asuint(v.c0) * uint4(3475533443u, 3777095341u, 3385463369u, 1773538433u) + asuint(v.c1) * uint4(3773525029u, 4131962539u, 1809525511u, 4016293529u) + asuint(v.c2) * uint4(2416021567u, 2828384717u, 2636362241u, 1258410977u) + asuint(v.c3) * uint4(1952565773u, 2037535609u, 3592785499u, 3996716183u) + 2626301701u;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static half half(half x)
	{
		return new half(x);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static half half(float v)
	{
		return new half(v);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static half half(double v)
	{
		return new half(v);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static uint hash(half v)
	{
		return (uint)(v.value * 1952372791 + -2123433123);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static half2 half2(half x, half y)
	{
		return new half2(x, y);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static half2 half2(half2 xy)
	{
		return new half2(xy);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static half2 half2(half v)
	{
		return new half2(v);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static half2 half2(float v)
	{
		return new half2(v);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static half2 half2(float2 v)
	{
		return new half2(v);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static half2 half2(double v)
	{
		return new half2(v);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static half2 half2(double2 v)
	{
		return new half2(v);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static uint hash(half2 v)
	{
		return csum(uint2(v.x.value, v.y.value) * uint2(1851936439u, 1938025801u)) + 3712598587u;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static uint2 hashwide(half2 v)
	{
		return uint2(v.x.value, v.y.value) * uint2(3956330501u, 2437373431u) + 1441286183u;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static half3 half3(half x, half y, half z)
	{
		return new half3(x, y, z);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static half3 half3(half x, half2 yz)
	{
		return new half3(x, yz);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static half3 half3(half2 xy, half z)
	{
		return new half3(xy, z);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static half3 half3(half3 xyz)
	{
		return new half3(xyz);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static half3 half3(half v)
	{
		return new half3(v);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static half3 half3(float v)
	{
		return new half3(v);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static half3 half3(float3 v)
	{
		return new half3(v);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static half3 half3(double v)
	{
		return new half3(v);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static half3 half3(double3 v)
	{
		return new half3(v);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static uint hash(half3 v)
	{
		return csum(uint3(v.x.value, v.y.value, v.z.value) * uint3(1750611407u, 3285396193u, 3110507567u)) + 4271396531u;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static uint3 hashwide(half3 v)
	{
		return uint3(v.x.value, v.y.value, v.z.value) * uint3(4198118021u, 2908068253u, 3705492289u) + 2497566569u;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static half4 half4(half x, half y, half z, half w)
	{
		return new half4(x, y, z, w);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static half4 half4(half x, half y, half2 zw)
	{
		return new half4(x, y, zw);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static half4 half4(half x, half2 yz, half w)
	{
		return new half4(x, yz, w);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static half4 half4(half x, half3 yzw)
	{
		return new half4(x, yzw);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static half4 half4(half2 xy, half z, half w)
	{
		return new half4(xy, z, w);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static half4 half4(half2 xy, half2 zw)
	{
		return new half4(xy, zw);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static half4 half4(half3 xyz, half w)
	{
		return new half4(xyz, w);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static half4 half4(half4 xyzw)
	{
		return new half4(xyzw);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static half4 half4(half v)
	{
		return new half4(v);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static half4 half4(float v)
	{
		return new half4(v);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static half4 half4(float4 v)
	{
		return new half4(v);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static half4 half4(double v)
	{
		return new half4(v);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static half4 half4(double4 v)
	{
		return new half4(v);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static uint hash(half4 v)
	{
		return csum(uint4(v.x.value, v.y.value, v.z.value, v.w.value) * uint4(1952372791u, 2631698677u, 4200781601u, 2119021007u)) + 1760485621;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static uint4 hashwide(half4 v)
	{
		return uint4(v.x.value, v.y.value, v.z.value, v.w.value) * uint4(3157985881u, 2171534173u, 2723054263u, 1168253063u) + 4228926523u;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static int2 int2(int x, int y)
	{
		return new int2(x, y);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static int2 int2(int2 xy)
	{
		return new int2(xy);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static int2 int2(int v)
	{
		return new int2(v);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static int2 int2(bool v)
	{
		return new int2(v);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static int2 int2(bool2 v)
	{
		return new int2(v);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static int2 int2(uint v)
	{
		return new int2(v);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static int2 int2(uint2 v)
	{
		return new int2(v);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static int2 int2(float v)
	{
		return new int2(v);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static int2 int2(float2 v)
	{
		return new int2(v);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static int2 int2(double v)
	{
		return new int2(v);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static int2 int2(double2 v)
	{
		return new int2(v);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static uint hash(int2 v)
	{
		return csum(asuint(v) * uint2(2209710647u, 2201894441u)) + 2849577407u;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static uint2 hashwide(int2 v)
	{
		return asuint(v) * uint2(3287031191u, 3098675399u) + 1564399943u;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static int shuffle(int2 a, int2 b, ShuffleComponent x)
	{
		return select_shuffle_component(a, b, x);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static int2 shuffle(int2 a, int2 b, ShuffleComponent x, ShuffleComponent y)
	{
		return int2(select_shuffle_component(a, b, x), select_shuffle_component(a, b, y));
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static int3 shuffle(int2 a, int2 b, ShuffleComponent x, ShuffleComponent y, ShuffleComponent z)
	{
		return int3(select_shuffle_component(a, b, x), select_shuffle_component(a, b, y), select_shuffle_component(a, b, z));
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static int4 shuffle(int2 a, int2 b, ShuffleComponent x, ShuffleComponent y, ShuffleComponent z, ShuffleComponent w)
	{
		return int4(select_shuffle_component(a, b, x), select_shuffle_component(a, b, y), select_shuffle_component(a, b, z), select_shuffle_component(a, b, w));
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	internal static int select_shuffle_component(int2 a, int2 b, ShuffleComponent component)
	{
		return component switch
		{
			ShuffleComponent.LeftX => a.x, 
			ShuffleComponent.LeftY => a.y, 
			ShuffleComponent.RightX => b.x, 
			ShuffleComponent.RightY => b.y, 
			_ => throw new ArgumentException("Invalid shuffle component: " + component), 
		};
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static int2x2 int2x2(int2 c0, int2 c1)
	{
		return new int2x2(c0, c1);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static int2x2 int2x2(int m00, int m01, int m10, int m11)
	{
		return new int2x2(m00, m01, m10, m11);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static int2x2 int2x2(int v)
	{
		return new int2x2(v);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static int2x2 int2x2(bool v)
	{
		return new int2x2(v);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static int2x2 int2x2(bool2x2 v)
	{
		return new int2x2(v);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static int2x2 int2x2(uint v)
	{
		return new int2x2(v);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static int2x2 int2x2(uint2x2 v)
	{
		return new int2x2(v);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static int2x2 int2x2(float v)
	{
		return new int2x2(v);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static int2x2 int2x2(float2x2 v)
	{
		return new int2x2(v);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static int2x2 int2x2(double v)
	{
		return new int2x2(v);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static int2x2 int2x2(double2x2 v)
	{
		return new int2x2(v);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static int2x2 transpose(int2x2 v)
	{
		return int2x2(v.c0.x, v.c0.y, v.c1.x, v.c1.y);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static int determinant(int2x2 m)
	{
		int x = m.c0.x;
		int x2 = m.c1.x;
		int y = m.c0.y;
		int y2 = m.c1.y;
		return x * y2 - x2 * y;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static uint hash(int2x2 v)
	{
		return csum(asuint(v.c0) * uint2(3784421429u, 1750626223u) + asuint(v.c1) * uint2(3571447507u, 3412283213u)) + 2601761069u;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static uint2 hashwide(int2x2 v)
	{
		return asuint(v.c0) * uint2(1254033427u, 2248573027u) + asuint(v.c1) * uint2(3612677113u, 1521739981u) + 1735296007u;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static int2x3 int2x3(int2 c0, int2 c1, int2 c2)
	{
		return new int2x3(c0, c1, c2);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static int2x3 int2x3(int m00, int m01, int m02, int m10, int m11, int m12)
	{
		return new int2x3(m00, m01, m02, m10, m11, m12);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static int2x3 int2x3(int v)
	{
		return new int2x3(v);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static int2x3 int2x3(bool v)
	{
		return new int2x3(v);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static int2x3 int2x3(bool2x3 v)
	{
		return new int2x3(v);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static int2x3 int2x3(uint v)
	{
		return new int2x3(v);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static int2x3 int2x3(uint2x3 v)
	{
		return new int2x3(v);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static int2x3 int2x3(float v)
	{
		return new int2x3(v);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static int2x3 int2x3(float2x3 v)
	{
		return new int2x3(v);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static int2x3 int2x3(double v)
	{
		return new int2x3(v);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static int2x3 int2x3(double2x3 v)
	{
		return new int2x3(v);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static int3x2 transpose(int2x3 v)
	{
		return int3x2(v.c0.x, v.c0.y, v.c1.x, v.c1.y, v.c2.x, v.c2.y);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static uint hash(int2x3 v)
	{
		return csum(asuint(v.c0) * uint2(3404170631u, 2048213449u) + asuint(v.c1) * uint2(4164671783u, 1780759499u) + asuint(v.c2) * uint2(1352369353u, 2446407751u)) + 1391928079;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static uint2 hashwide(int2x3 v)
	{
		return asuint(v.c0) * uint2(3475533443u, 3777095341u) + asuint(v.c1) * uint2(3385463369u, 1773538433u) + asuint(v.c2) * uint2(3773525029u, 4131962539u) + 1809525511u;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static int2x4 int2x4(int2 c0, int2 c1, int2 c2, int2 c3)
	{
		return new int2x4(c0, c1, c2, c3);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static int2x4 int2x4(int m00, int m01, int m02, int m03, int m10, int m11, int m12, int m13)
	{
		return new int2x4(m00, m01, m02, m03, m10, m11, m12, m13);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static int2x4 int2x4(int v)
	{
		return new int2x4(v);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static int2x4 int2x4(bool v)
	{
		return new int2x4(v);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static int2x4 int2x4(bool2x4 v)
	{
		return new int2x4(v);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static int2x4 int2x4(uint v)
	{
		return new int2x4(v);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static int2x4 int2x4(uint2x4 v)
	{
		return new int2x4(v);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static int2x4 int2x4(float v)
	{
		return new int2x4(v);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static int2x4 int2x4(float2x4 v)
	{
		return new int2x4(v);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static int2x4 int2x4(double v)
	{
		return new int2x4(v);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static int2x4 int2x4(double2x4 v)
	{
		return new int2x4(v);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static int4x2 transpose(int2x4 v)
	{
		return int4x2(v.c0.x, v.c0.y, v.c1.x, v.c1.y, v.c2.x, v.c2.y, v.c3.x, v.c3.y);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static uint hash(int2x4 v)
	{
		return csum(asuint(v.c0) * uint2(2057338067u, 2942577577u) + asuint(v.c1) * uint2(2834440507u, 2671762487u) + asuint(v.c2) * uint2(2892026051u, 2455987759u) + asuint(v.c3) * uint2(3868600063u, 3170963179u)) + 2632835537u;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static uint2 hashwide(int2x4 v)
	{
		return asuint(v.c0) * uint2(1136528209u, 2944626401u) + asuint(v.c1) * uint2(2972762423u, 1417889653u) + asuint(v.c2) * uint2(2080514593u, 2731544287u) + asuint(v.c3) * uint2(2828498809u, 2669441947u) + 1260114311u;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static int3 int3(int x, int y, int z)
	{
		return new int3(x, y, z);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static int3 int3(int x, int2 yz)
	{
		return new int3(x, yz);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static int3 int3(int2 xy, int z)
	{
		return new int3(xy, z);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static int3 int3(int3 xyz)
	{
		return new int3(xyz);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static int3 int3(int v)
	{
		return new int3(v);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static int3 int3(bool v)
	{
		return new int3(v);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static int3 int3(bool3 v)
	{
		return new int3(v);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static int3 int3(uint v)
	{
		return new int3(v);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static int3 int3(uint3 v)
	{
		return new int3(v);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static int3 int3(float v)
	{
		return new int3(v);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static int3 int3(float3 v)
	{
		return new int3(v);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static int3 int3(double v)
	{
		return new int3(v);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static int3 int3(double3 v)
	{
		return new int3(v);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static uint hash(int3 v)
	{
		return csum(asuint(v) * uint3(1283419601u, 1210229737u, 2864955997u)) + 3525118277u;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static uint3 hashwide(int3 v)
	{
		return asuint(v) * uint3(2298260269u, 1632478733u, 1537393931u) + 2353355467u;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static int shuffle(int3 a, int3 b, ShuffleComponent x)
	{
		return select_shuffle_component(a, b, x);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static int2 shuffle(int3 a, int3 b, ShuffleComponent x, ShuffleComponent y)
	{
		return int2(select_shuffle_component(a, b, x), select_shuffle_component(a, b, y));
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static int3 shuffle(int3 a, int3 b, ShuffleComponent x, ShuffleComponent y, ShuffleComponent z)
	{
		return int3(select_shuffle_component(a, b, x), select_shuffle_component(a, b, y), select_shuffle_component(a, b, z));
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static int4 shuffle(int3 a, int3 b, ShuffleComponent x, ShuffleComponent y, ShuffleComponent z, ShuffleComponent w)
	{
		return int4(select_shuffle_component(a, b, x), select_shuffle_component(a, b, y), select_shuffle_component(a, b, z), select_shuffle_component(a, b, w));
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	internal static int select_shuffle_component(int3 a, int3 b, ShuffleComponent component)
	{
		return component switch
		{
			ShuffleComponent.LeftX => a.x, 
			ShuffleComponent.LeftY => a.y, 
			ShuffleComponent.LeftZ => a.z, 
			ShuffleComponent.RightX => b.x, 
			ShuffleComponent.RightY => b.y, 
			ShuffleComponent.RightZ => b.z, 
			_ => throw new ArgumentException("Invalid shuffle component: " + component), 
		};
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static int3x2 int3x2(int3 c0, int3 c1)
	{
		return new int3x2(c0, c1);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static int3x2 int3x2(int m00, int m01, int m10, int m11, int m20, int m21)
	{
		return new int3x2(m00, m01, m10, m11, m20, m21);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static int3x2 int3x2(int v)
	{
		return new int3x2(v);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static int3x2 int3x2(bool v)
	{
		return new int3x2(v);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static int3x2 int3x2(bool3x2 v)
	{
		return new int3x2(v);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static int3x2 int3x2(uint v)
	{
		return new int3x2(v);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static int3x2 int3x2(uint3x2 v)
	{
		return new int3x2(v);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static int3x2 int3x2(float v)
	{
		return new int3x2(v);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static int3x2 int3x2(float3x2 v)
	{
		return new int3x2(v);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static int3x2 int3x2(double v)
	{
		return new int3x2(v);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static int3x2 int3x2(double3x2 v)
	{
		return new int3x2(v);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static int2x3 transpose(int3x2 v)
	{
		return int2x3(v.c0.x, v.c0.y, v.c0.z, v.c1.x, v.c1.y, v.c1.z);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static uint hash(int3x2 v)
	{
		return csum(asuint(v.c0) * uint3(3678265601u, 2070747979u, 1480171127u) + asuint(v.c1) * uint3(1588341193u, 4234155257u, 1811310911u)) + 2635799963u;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static uint3 hashwide(int3x2 v)
	{
		return asuint(v.c0) * uint3(4165137857u, 2759770933u, 2759319383u) + asuint(v.c1) * uint3(3299952959u, 3121178323u, 2948522579u) + 1531026433u;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static int3x3 int3x3(int3 c0, int3 c1, int3 c2)
	{
		return new int3x3(c0, c1, c2);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static int3x3 int3x3(int m00, int m01, int m02, int m10, int m11, int m12, int m20, int m21, int m22)
	{
		return new int3x3(m00, m01, m02, m10, m11, m12, m20, m21, m22);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static int3x3 int3x3(int v)
	{
		return new int3x3(v);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static int3x3 int3x3(bool v)
	{
		return new int3x3(v);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static int3x3 int3x3(bool3x3 v)
	{
		return new int3x3(v);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static int3x3 int3x3(uint v)
	{
		return new int3x3(v);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static int3x3 int3x3(uint3x3 v)
	{
		return new int3x3(v);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static int3x3 int3x3(float v)
	{
		return new int3x3(v);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static int3x3 int3x3(float3x3 v)
	{
		return new int3x3(v);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static int3x3 int3x3(double v)
	{
		return new int3x3(v);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static int3x3 int3x3(double3x3 v)
	{
		return new int3x3(v);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static int3x3 transpose(int3x3 v)
	{
		return int3x3(v.c0.x, v.c0.y, v.c0.z, v.c1.x, v.c1.y, v.c1.z, v.c2.x, v.c2.y, v.c2.z);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static int determinant(int3x3 m)
	{
		int3 c = m.c0;
		int3 c2 = m.c1;
		int3 c3 = m.c2;
		int num = c2.y * c3.z - c2.z * c3.y;
		int num2 = c.y * c3.z - c.z * c3.y;
		int num3 = c.y * c2.z - c.z * c2.y;
		return c.x * num - c2.x * num2 + c3.x * num3;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static uint hash(int3x3 v)
	{
		return csum(asuint(v.c0) * uint3(2479033387u, 3702457169u, 1845824257u) + asuint(v.c1) * uint3(1963973621u, 2134758553u, 1391111867u) + asuint(v.c2) * uint3(1167706003u, 2209736489u, 3261535807u)) + 1740411209;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static uint3 hashwide(int3x3 v)
	{
		return asuint(v.c0) * uint3(2910609089u, 2183822701u, 3029516053u) + asuint(v.c1) * uint3(3547472099u, 2057487037u, 3781937309u) + asuint(v.c2) * uint3(2057338067u, 2942577577u, 2834440507u) + 2671762487u;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static int3x4 int3x4(int3 c0, int3 c1, int3 c2, int3 c3)
	{
		return new int3x4(c0, c1, c2, c3);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static int3x4 int3x4(int m00, int m01, int m02, int m03, int m10, int m11, int m12, int m13, int m20, int m21, int m22, int m23)
	{
		return new int3x4(m00, m01, m02, m03, m10, m11, m12, m13, m20, m21, m22, m23);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static int3x4 int3x4(int v)
	{
		return new int3x4(v);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static int3x4 int3x4(bool v)
	{
		return new int3x4(v);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static int3x4 int3x4(bool3x4 v)
	{
		return new int3x4(v);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static int3x4 int3x4(uint v)
	{
		return new int3x4(v);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static int3x4 int3x4(uint3x4 v)
	{
		return new int3x4(v);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static int3x4 int3x4(float v)
	{
		return new int3x4(v);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static int3x4 int3x4(float3x4 v)
	{
		return new int3x4(v);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static int3x4 int3x4(double v)
	{
		return new int3x4(v);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static int3x4 int3x4(double3x4 v)
	{
		return new int3x4(v);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static int4x3 transpose(int3x4 v)
	{
		return int4x3(v.c0.x, v.c0.y, v.c0.z, v.c1.x, v.c1.y, v.c1.z, v.c2.x, v.c2.y, v.c2.z, v.c3.x, v.c3.y, v.c3.z);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static uint hash(int3x4 v)
	{
		return csum(asuint(v.c0) * uint3(1521739981u, 1735296007u, 3010324327u) + asuint(v.c1) * uint3(1875523709u, 2937008387u, 3835713223u) + asuint(v.c2) * uint3(2216526373u, 3375971453u, 3559829411u) + asuint(v.c3) * uint3(3652178029u, 2544260129u, 2013864031u)) + 2627668003u;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static uint3 hashwide(int3x4 v)
	{
		return asuint(v.c0) * uint3(1520214331u, 2949502447u, 2827819133u) + asuint(v.c1) * uint3(3480140317u, 2642994593u, 3940484981u) + asuint(v.c2) * uint3(1954192763u, 1091696537u, 3052428017u) + asuint(v.c3) * uint3(4253034763u, 2338696631u, 3757372771u) + 1885959949u;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static int4 int4(int x, int y, int z, int w)
	{
		return new int4(x, y, z, w);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static int4 int4(int x, int y, int2 zw)
	{
		return new int4(x, y, zw);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static int4 int4(int x, int2 yz, int w)
	{
		return new int4(x, yz, w);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static int4 int4(int x, int3 yzw)
	{
		return new int4(x, yzw);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static int4 int4(int2 xy, int z, int w)
	{
		return new int4(xy, z, w);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static int4 int4(int2 xy, int2 zw)
	{
		return new int4(xy, zw);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static int4 int4(int3 xyz, int w)
	{
		return new int4(xyz, w);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static int4 int4(int4 xyzw)
	{
		return new int4(xyzw);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static int4 int4(int v)
	{
		return new int4(v);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static int4 int4(bool v)
	{
		return new int4(v);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static int4 int4(bool4 v)
	{
		return new int4(v);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static int4 int4(uint v)
	{
		return new int4(v);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static int4 int4(uint4 v)
	{
		return new int4(v);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static int4 int4(float v)
	{
		return new int4(v);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static int4 int4(float4 v)
	{
		return new int4(v);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static int4 int4(double v)
	{
		return new int4(v);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static int4 int4(double4 v)
	{
		return new int4(v);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static uint hash(int4 v)
	{
		return csum(asuint(v) * uint4(1845824257u, 1963973621u, 2134758553u, 1391111867u)) + 1167706003;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static uint4 hashwide(int4 v)
	{
		return asuint(v) * uint4(2209736489u, 3261535807u, 1740411209u, 2910609089u) + 2183822701u;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static int shuffle(int4 a, int4 b, ShuffleComponent x)
	{
		return select_shuffle_component(a, b, x);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static int2 shuffle(int4 a, int4 b, ShuffleComponent x, ShuffleComponent y)
	{
		return int2(select_shuffle_component(a, b, x), select_shuffle_component(a, b, y));
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static int3 shuffle(int4 a, int4 b, ShuffleComponent x, ShuffleComponent y, ShuffleComponent z)
	{
		return int3(select_shuffle_component(a, b, x), select_shuffle_component(a, b, y), select_shuffle_component(a, b, z));
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static int4 shuffle(int4 a, int4 b, ShuffleComponent x, ShuffleComponent y, ShuffleComponent z, ShuffleComponent w)
	{
		return int4(select_shuffle_component(a, b, x), select_shuffle_component(a, b, y), select_shuffle_component(a, b, z), select_shuffle_component(a, b, w));
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	internal static int select_shuffle_component(int4 a, int4 b, ShuffleComponent component)
	{
		return component switch
		{
			ShuffleComponent.LeftX => a.x, 
			ShuffleComponent.LeftY => a.y, 
			ShuffleComponent.LeftZ => a.z, 
			ShuffleComponent.LeftW => a.w, 
			ShuffleComponent.RightX => b.x, 
			ShuffleComponent.RightY => b.y, 
			ShuffleComponent.RightZ => b.z, 
			ShuffleComponent.RightW => b.w, 
			_ => throw new ArgumentException("Invalid shuffle component: " + component), 
		};
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static int4x2 int4x2(int4 c0, int4 c1)
	{
		return new int4x2(c0, c1);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static int4x2 int4x2(int m00, int m01, int m10, int m11, int m20, int m21, int m30, int m31)
	{
		return new int4x2(m00, m01, m10, m11, m20, m21, m30, m31);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static int4x2 int4x2(int v)
	{
		return new int4x2(v);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static int4x2 int4x2(bool v)
	{
		return new int4x2(v);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static int4x2 int4x2(bool4x2 v)
	{
		return new int4x2(v);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static int4x2 int4x2(uint v)
	{
		return new int4x2(v);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static int4x2 int4x2(uint4x2 v)
	{
		return new int4x2(v);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static int4x2 int4x2(float v)
	{
		return new int4x2(v);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static int4x2 int4x2(float4x2 v)
	{
		return new int4x2(v);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static int4x2 int4x2(double v)
	{
		return new int4x2(v);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static int4x2 int4x2(double4x2 v)
	{
		return new int4x2(v);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static int2x4 transpose(int4x2 v)
	{
		return int2x4(v.c0.x, v.c0.y, v.c0.z, v.c0.w, v.c1.x, v.c1.y, v.c1.z, v.c1.w);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static uint hash(int4x2 v)
	{
		return csum(asuint(v.c0) * uint4(4205774813u, 1650214333u, 3388112843u, 1831150513u) + asuint(v.c1) * uint4(1848374953u, 3430200247u, 2209710647u, 2201894441u)) + 2849577407u;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static uint4 hashwide(int4x2 v)
	{
		return asuint(v.c0) * uint4(3287031191u, 3098675399u, 1564399943u, 1148435377u) + asuint(v.c1) * uint4(3416333663u, 1750611407u, 3285396193u, 3110507567u) + 4271396531u;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static int4x3 int4x3(int4 c0, int4 c1, int4 c2)
	{
		return new int4x3(c0, c1, c2);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static int4x3 int4x3(int m00, int m01, int m02, int m10, int m11, int m12, int m20, int m21, int m22, int m30, int m31, int m32)
	{
		return new int4x3(m00, m01, m02, m10, m11, m12, m20, m21, m22, m30, m31, m32);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static int4x3 int4x3(int v)
	{
		return new int4x3(v);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static int4x3 int4x3(bool v)
	{
		return new int4x3(v);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static int4x3 int4x3(bool4x3 v)
	{
		return new int4x3(v);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static int4x3 int4x3(uint v)
	{
		return new int4x3(v);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static int4x3 int4x3(uint4x3 v)
	{
		return new int4x3(v);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static int4x3 int4x3(float v)
	{
		return new int4x3(v);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static int4x3 int4x3(float4x3 v)
	{
		return new int4x3(v);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static int4x3 int4x3(double v)
	{
		return new int4x3(v);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static int4x3 int4x3(double4x3 v)
	{
		return new int4x3(v);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static int3x4 transpose(int4x3 v)
	{
		return int3x4(v.c0.x, v.c0.y, v.c0.z, v.c0.w, v.c1.x, v.c1.y, v.c1.z, v.c1.w, v.c2.x, v.c2.y, v.c2.z, v.c2.w);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static uint hash(int4x3 v)
	{
		return csum(asuint(v.c0) * uint4(1773538433u, 3773525029u, 4131962539u, 1809525511u) + asuint(v.c1) * uint4(4016293529u, 2416021567u, 2828384717u, 2636362241u) + asuint(v.c2) * uint4(1258410977u, 1952565773u, 2037535609u, 3592785499u)) + 3996716183u;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static uint4 hashwide(int4x3 v)
	{
		return asuint(v.c0) * uint4(2626301701u, 1306289417u, 2096137163u, 1548578029u) + asuint(v.c1) * uint4(4178800919u, 3898072289u, 4129428421u, 2631575897u) + asuint(v.c2) * uint4(2854656703u, 3578504047u, 4245178297u, 2173281923u) + 2973357649u;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static int4x4 int4x4(int4 c0, int4 c1, int4 c2, int4 c3)
	{
		return new int4x4(c0, c1, c2, c3);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static int4x4 int4x4(int m00, int m01, int m02, int m03, int m10, int m11, int m12, int m13, int m20, int m21, int m22, int m23, int m30, int m31, int m32, int m33)
	{
		return new int4x4(m00, m01, m02, m03, m10, m11, m12, m13, m20, m21, m22, m23, m30, m31, m32, m33);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static int4x4 int4x4(int v)
	{
		return new int4x4(v);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static int4x4 int4x4(bool v)
	{
		return new int4x4(v);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static int4x4 int4x4(bool4x4 v)
	{
		return new int4x4(v);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static int4x4 int4x4(uint v)
	{
		return new int4x4(v);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static int4x4 int4x4(uint4x4 v)
	{
		return new int4x4(v);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static int4x4 int4x4(float v)
	{
		return new int4x4(v);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static int4x4 int4x4(float4x4 v)
	{
		return new int4x4(v);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static int4x4 int4x4(double v)
	{
		return new int4x4(v);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static int4x4 int4x4(double4x4 v)
	{
		return new int4x4(v);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static int4x4 transpose(int4x4 v)
	{
		return int4x4(v.c0.x, v.c0.y, v.c0.z, v.c0.w, v.c1.x, v.c1.y, v.c1.z, v.c1.w, v.c2.x, v.c2.y, v.c2.z, v.c2.w, v.c3.x, v.c3.y, v.c3.z, v.c3.w);
	}

	public static int determinant(int4x4 m)
	{
		int4 c = m.c0;
		int4 c2 = m.c1;
		int4 c3 = m.c2;
		int4 c4 = m.c3;
		int num = c2.y * (c3.z * c4.w - c3.w * c4.z) - c3.y * (c2.z * c4.w - c2.w * c4.z) + c4.y * (c2.z * c3.w - c2.w * c3.z);
		int num2 = c.y * (c3.z * c4.w - c3.w * c4.z) - c3.y * (c.z * c4.w - c.w * c4.z) + c4.y * (c.z * c3.w - c.w * c3.z);
		int num3 = c.y * (c2.z * c4.w - c2.w * c4.z) - c2.y * (c.z * c4.w - c.w * c4.z) + c4.y * (c.z * c2.w - c.w * c2.z);
		int num4 = c.y * (c2.z * c3.w - c2.w * c3.z) - c2.y * (c.z * c3.w - c.w * c3.z) + c3.y * (c.z * c2.w - c.w * c2.z);
		return c.x * num - c2.x * num2 + c3.x * num3 - c4.x * num4;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static uint hash(int4x4 v)
	{
		return csum(asuint(v.c0) * uint4(1562056283u, 2265541847u, 1283419601u, 1210229737u) + asuint(v.c1) * uint4(2864955997u, 3525118277u, 2298260269u, 1632478733u) + asuint(v.c2) * uint4(1537393931u, 2353355467u, 3441847433u, 4052036147u) + asuint(v.c3) * uint4(2011389559u, 2252224297u, 3784421429u, 1750626223u)) + 3571447507u;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static uint4 hashwide(int4x4 v)
	{
		return asuint(v.c0) * uint4(3412283213u, 2601761069u, 1254033427u, 2248573027u) + asuint(v.c1) * uint4(3612677113u, 1521739981u, 1735296007u, 3010324327u) + asuint(v.c2) * uint4(1875523709u, 2937008387u, 3835713223u, 2216526373u) + asuint(v.c3) * uint4(3375971453u, 3559829411u, 3652178029u, 2544260129u) + 2013864031u;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static int asint(uint x)
	{
		return (int)x;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static int2 asint(uint2 x)
	{
		return int2((int)x.x, (int)x.y);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static int3 asint(uint3 x)
	{
		return int3((int)x.x, (int)x.y, (int)x.z);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static int4 asint(uint4 x)
	{
		return int4((int)x.x, (int)x.y, (int)x.z, (int)x.w);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static int asint(float x)
	{
		IntFloatUnion intFloatUnion = default(IntFloatUnion);
		intFloatUnion.intValue = 0;
		intFloatUnion.floatValue = x;
		return intFloatUnion.intValue;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static int2 asint(float2 x)
	{
		return int2(asint(x.x), asint(x.y));
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static int3 asint(float3 x)
	{
		return int3(asint(x.x), asint(x.y), asint(x.z));
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static int4 asint(float4 x)
	{
		return int4(asint(x.x), asint(x.y), asint(x.z), asint(x.w));
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static uint asuint(int x)
	{
		return (uint)x;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static uint2 asuint(int2 x)
	{
		return uint2((uint)x.x, (uint)x.y);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static uint3 asuint(int3 x)
	{
		return uint3((uint)x.x, (uint)x.y, (uint)x.z);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static uint4 asuint(int4 x)
	{
		return uint4((uint)x.x, (uint)x.y, (uint)x.z, (uint)x.w);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static uint asuint(float x)
	{
		return (uint)asint(x);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static uint2 asuint(float2 x)
	{
		return uint2(asuint(x.x), asuint(x.y));
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static uint3 asuint(float3 x)
	{
		return uint3(asuint(x.x), asuint(x.y), asuint(x.z));
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static uint4 asuint(float4 x)
	{
		return uint4(asuint(x.x), asuint(x.y), asuint(x.z), asuint(x.w));
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static long aslong(ulong x)
	{
		return (long)x;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static long aslong(double x)
	{
		LongDoubleUnion longDoubleUnion = default(LongDoubleUnion);
		longDoubleUnion.longValue = 0L;
		longDoubleUnion.doubleValue = x;
		return longDoubleUnion.longValue;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static ulong asulong(long x)
	{
		return (ulong)x;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static ulong asulong(double x)
	{
		return (ulong)aslong(x);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static float asfloat(int x)
	{
		IntFloatUnion intFloatUnion = default(IntFloatUnion);
		intFloatUnion.floatValue = 0f;
		intFloatUnion.intValue = x;
		return intFloatUnion.floatValue;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static float2 asfloat(int2 x)
	{
		return float2(asfloat(x.x), asfloat(x.y));
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static float3 asfloat(int3 x)
	{
		return float3(asfloat(x.x), asfloat(x.y), asfloat(x.z));
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static float4 asfloat(int4 x)
	{
		return float4(asfloat(x.x), asfloat(x.y), asfloat(x.z), asfloat(x.w));
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static float asfloat(uint x)
	{
		return asfloat((int)x);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static float2 asfloat(uint2 x)
	{
		return float2(asfloat(x.x), asfloat(x.y));
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static float3 asfloat(uint3 x)
	{
		return float3(asfloat(x.x), asfloat(x.y), asfloat(x.z));
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static float4 asfloat(uint4 x)
	{
		return float4(asfloat(x.x), asfloat(x.y), asfloat(x.z), asfloat(x.w));
	}

	public static int bitmask(bool4 value)
	{
		int num = 0;
		if (value.x)
		{
			num |= 1;
		}
		if (value.y)
		{
			num |= 2;
		}
		if (value.z)
		{
			num |= 4;
		}
		if (value.w)
		{
			num |= 8;
		}
		return num;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static double asdouble(long x)
	{
		LongDoubleUnion longDoubleUnion = default(LongDoubleUnion);
		longDoubleUnion.doubleValue = 0.0;
		longDoubleUnion.longValue = x;
		return longDoubleUnion.doubleValue;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static double asdouble(ulong x)
	{
		return asdouble((long)x);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool isfinite(float x)
	{
		return abs(x) < float.PositiveInfinity;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool2 isfinite(float2 x)
	{
		return abs(x) < float.PositiveInfinity;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool3 isfinite(float3 x)
	{
		return abs(x) < float.PositiveInfinity;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool4 isfinite(float4 x)
	{
		return abs(x) < float.PositiveInfinity;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool isfinite(double x)
	{
		return abs(x) < double.PositiveInfinity;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool2 isfinite(double2 x)
	{
		return abs(x) < double.PositiveInfinity;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool3 isfinite(double3 x)
	{
		return abs(x) < double.PositiveInfinity;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool4 isfinite(double4 x)
	{
		return abs(x) < double.PositiveInfinity;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool isinf(float x)
	{
		return abs(x) == float.PositiveInfinity;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool2 isinf(float2 x)
	{
		return abs(x) == float.PositiveInfinity;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool3 isinf(float3 x)
	{
		return abs(x) == float.PositiveInfinity;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool4 isinf(float4 x)
	{
		return abs(x) == float.PositiveInfinity;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool isinf(double x)
	{
		return abs(x) == double.PositiveInfinity;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool2 isinf(double2 x)
	{
		return abs(x) == double.PositiveInfinity;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool3 isinf(double3 x)
	{
		return abs(x) == double.PositiveInfinity;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool4 isinf(double4 x)
	{
		return abs(x) == double.PositiveInfinity;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool isnan(float x)
	{
		return (asuint(x) & 0x7FFFFFFF) > 2139095040;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool2 isnan(float2 x)
	{
		return (asuint(x) & 2147483647u) > 2139095040u;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool3 isnan(float3 x)
	{
		return (asuint(x) & 2147483647u) > 2139095040u;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool4 isnan(float4 x)
	{
		return (asuint(x) & 2147483647u) > 2139095040u;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool isnan(double x)
	{
		return (asulong(x) & 0x7FFFFFFFFFFFFFFFL) > 9218868437227405312L;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool2 isnan(double2 x)
	{
		return bool2((asulong(x.x) & 0x7FFFFFFFFFFFFFFFL) > 9218868437227405312L, (asulong(x.y) & 0x7FFFFFFFFFFFFFFFL) > 9218868437227405312L);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool3 isnan(double3 x)
	{
		return bool3((asulong(x.x) & 0x7FFFFFFFFFFFFFFFL) > 9218868437227405312L, (asulong(x.y) & 0x7FFFFFFFFFFFFFFFL) > 9218868437227405312L, (asulong(x.z) & 0x7FFFFFFFFFFFFFFFL) > 9218868437227405312L);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool4 isnan(double4 x)
	{
		return bool4((asulong(x.x) & 0x7FFFFFFFFFFFFFFFL) > 9218868437227405312L, (asulong(x.y) & 0x7FFFFFFFFFFFFFFFL) > 9218868437227405312L, (asulong(x.z) & 0x7FFFFFFFFFFFFFFFL) > 9218868437227405312L, (asulong(x.w) & 0x7FFFFFFFFFFFFFFFL) > 9218868437227405312L);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool ispow2(int x)
	{
		if (x > 0)
		{
			return (x & (x - 1)) == 0;
		}
		return false;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool2 ispow2(int2 x)
	{
		return new bool2(ispow2(x.x), ispow2(x.y));
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool3 ispow2(int3 x)
	{
		return new bool3(ispow2(x.x), ispow2(x.y), ispow2(x.z));
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool4 ispow2(int4 x)
	{
		return new bool4(ispow2(x.x), ispow2(x.y), ispow2(x.z), ispow2(x.w));
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool ispow2(uint x)
	{
		if (x != 0)
		{
			return (x & (x - 1)) == 0;
		}
		return false;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool2 ispow2(uint2 x)
	{
		return new bool2(ispow2(x.x), ispow2(x.y));
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool3 ispow2(uint3 x)
	{
		return new bool3(ispow2(x.x), ispow2(x.y), ispow2(x.z));
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool4 ispow2(uint4 x)
	{
		return new bool4(ispow2(x.x), ispow2(x.y), ispow2(x.z), ispow2(x.w));
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static int min(int x, int y)
	{
		if (x >= y)
		{
			return y;
		}
		return x;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static int2 min(int2 x, int2 y)
	{
		return new int2(min(x.x, y.x), min(x.y, y.y));
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static int3 min(int3 x, int3 y)
	{
		return new int3(min(x.x, y.x), min(x.y, y.y), min(x.z, y.z));
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static int4 min(int4 x, int4 y)
	{
		return new int4(min(x.x, y.x), min(x.y, y.y), min(x.z, y.z), min(x.w, y.w));
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static uint min(uint x, uint y)
	{
		if (x >= y)
		{
			return y;
		}
		return x;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static uint2 min(uint2 x, uint2 y)
	{
		return new uint2(min(x.x, y.x), min(x.y, y.y));
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static uint3 min(uint3 x, uint3 y)
	{
		return new uint3(min(x.x, y.x), min(x.y, y.y), min(x.z, y.z));
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static uint4 min(uint4 x, uint4 y)
	{
		return new uint4(min(x.x, y.x), min(x.y, y.y), min(x.z, y.z), min(x.w, y.w));
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static long min(long x, long y)
	{
		if (x >= y)
		{
			return y;
		}
		return x;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static ulong min(ulong x, ulong y)
	{
		if (x >= y)
		{
			return y;
		}
		return x;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static float min(float x, float y)
	{
		if (!float.IsNaN(y) && !(x < y))
		{
			return y;
		}
		return x;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static float2 min(float2 x, float2 y)
	{
		return new float2(min(x.x, y.x), min(x.y, y.y));
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static float3 min(float3 x, float3 y)
	{
		return new float3(min(x.x, y.x), min(x.y, y.y), min(x.z, y.z));
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static float4 min(float4 x, float4 y)
	{
		return new float4(min(x.x, y.x), min(x.y, y.y), min(x.z, y.z), min(x.w, y.w));
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static double min(double x, double y)
	{
		if (!double.IsNaN(y) && !(x < y))
		{
			return y;
		}
		return x;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static double2 min(double2 x, double2 y)
	{
		return new double2(min(x.x, y.x), min(x.y, y.y));
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static double3 min(double3 x, double3 y)
	{
		return new double3(min(x.x, y.x), min(x.y, y.y), min(x.z, y.z));
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static double4 min(double4 x, double4 y)
	{
		return new double4(min(x.x, y.x), min(x.y, y.y), min(x.z, y.z), min(x.w, y.w));
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static int max(int x, int y)
	{
		if (x <= y)
		{
			return y;
		}
		return x;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static int2 max(int2 x, int2 y)
	{
		return new int2(max(x.x, y.x), max(x.y, y.y));
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static int3 max(int3 x, int3 y)
	{
		return new int3(max(x.x, y.x), max(x.y, y.y), max(x.z, y.z));
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static int4 max(int4 x, int4 y)
	{
		return new int4(max(x.x, y.x), max(x.y, y.y), max(x.z, y.z), max(x.w, y.w));
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static uint max(uint x, uint y)
	{
		if (x <= y)
		{
			return y;
		}
		return x;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static uint2 max(uint2 x, uint2 y)
	{
		return new uint2(max(x.x, y.x), max(x.y, y.y));
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static uint3 max(uint3 x, uint3 y)
	{
		return new uint3(max(x.x, y.x), max(x.y, y.y), max(x.z, y.z));
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static uint4 max(uint4 x, uint4 y)
	{
		return new uint4(max(x.x, y.x), max(x.y, y.y), max(x.z, y.z), max(x.w, y.w));
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static long max(long x, long y)
	{
		if (x <= y)
		{
			return y;
		}
		return x;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static ulong max(ulong x, ulong y)
	{
		if (x <= y)
		{
			return y;
		}
		return x;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static float max(float x, float y)
	{
		if (!float.IsNaN(y) && !(x > y))
		{
			return y;
		}
		return x;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static float2 max(float2 x, float2 y)
	{
		return new float2(max(x.x, y.x), max(x.y, y.y));
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static float3 max(float3 x, float3 y)
	{
		return new float3(max(x.x, y.x), max(x.y, y.y), max(x.z, y.z));
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static float4 max(float4 x, float4 y)
	{
		return new float4(max(x.x, y.x), max(x.y, y.y), max(x.z, y.z), max(x.w, y.w));
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static double max(double x, double y)
	{
		if (!double.IsNaN(y) && !(x > y))
		{
			return y;
		}
		return x;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static double2 max(double2 x, double2 y)
	{
		return new double2(max(x.x, y.x), max(x.y, y.y));
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static double3 max(double3 x, double3 y)
	{
		return new double3(max(x.x, y.x), max(x.y, y.y), max(x.z, y.z));
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static double4 max(double4 x, double4 y)
	{
		return new double4(max(x.x, y.x), max(x.y, y.y), max(x.z, y.z), max(x.w, y.w));
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static float lerp(float x, float y, float s)
	{
		return x + s * (y - x);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static float2 lerp(float2 x, float2 y, float s)
	{
		return x + s * (y - x);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static float3 lerp(float3 x, float3 y, float s)
	{
		return x + s * (y - x);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static float4 lerp(float4 x, float4 y, float s)
	{
		return x + s * (y - x);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static float2 lerp(float2 x, float2 y, float2 s)
	{
		return x + s * (y - x);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static float3 lerp(float3 x, float3 y, float3 s)
	{
		return x + s * (y - x);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static float4 lerp(float4 x, float4 y, float4 s)
	{
		return x + s * (y - x);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static double lerp(double x, double y, double s)
	{
		return x + s * (y - x);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static double2 lerp(double2 x, double2 y, double s)
	{
		return x + s * (y - x);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static double3 lerp(double3 x, double3 y, double s)
	{
		return x + s * (y - x);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static double4 lerp(double4 x, double4 y, double s)
	{
		return x + s * (y - x);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static double2 lerp(double2 x, double2 y, double2 s)
	{
		return x + s * (y - x);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static double3 lerp(double3 x, double3 y, double3 s)
	{
		return x + s * (y - x);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static double4 lerp(double4 x, double4 y, double4 s)
	{
		return x + s * (y - x);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static float unlerp(float a, float b, float x)
	{
		return (x - a) / (b - a);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static float2 unlerp(float2 a, float2 b, float2 x)
	{
		return (x - a) / (b - a);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static float3 unlerp(float3 a, float3 b, float3 x)
	{
		return (x - a) / (b - a);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static float4 unlerp(float4 a, float4 b, float4 x)
	{
		return (x - a) / (b - a);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static double unlerp(double a, double b, double x)
	{
		return (x - a) / (b - a);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static double2 unlerp(double2 a, double2 b, double2 x)
	{
		return (x - a) / (b - a);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static double3 unlerp(double3 a, double3 b, double3 x)
	{
		return (x - a) / (b - a);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static double4 unlerp(double4 a, double4 b, double4 x)
	{
		return (x - a) / (b - a);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static float remap(float a, float b, float c, float d, float x)
	{
		return lerp(c, d, unlerp(a, b, x));
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static float2 remap(float2 a, float2 b, float2 c, float2 d, float2 x)
	{
		return lerp(c, d, unlerp(a, b, x));
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static float3 remap(float3 a, float3 b, float3 c, float3 d, float3 x)
	{
		return lerp(c, d, unlerp(a, b, x));
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static float4 remap(float4 a, float4 b, float4 c, float4 d, float4 x)
	{
		return lerp(c, d, unlerp(a, b, x));
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static double remap(double a, double b, double c, double d, double x)
	{
		return lerp(c, d, unlerp(a, b, x));
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static double2 remap(double2 a, double2 b, double2 c, double2 d, double2 x)
	{
		return lerp(c, d, unlerp(a, b, x));
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static double3 remap(double3 a, double3 b, double3 c, double3 d, double3 x)
	{
		return lerp(c, d, unlerp(a, b, x));
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static double4 remap(double4 a, double4 b, double4 c, double4 d, double4 x)
	{
		return lerp(c, d, unlerp(a, b, x));
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static int mad(int a, int b, int c)
	{
		return a * b + c;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static int2 mad(int2 a, int2 b, int2 c)
	{
		return a * b + c;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static int3 mad(int3 a, int3 b, int3 c)
	{
		return a * b + c;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static int4 mad(int4 a, int4 b, int4 c)
	{
		return a * b + c;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static uint mad(uint a, uint b, uint c)
	{
		return a * b + c;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static uint2 mad(uint2 a, uint2 b, uint2 c)
	{
		return a * b + c;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static uint3 mad(uint3 a, uint3 b, uint3 c)
	{
		return a * b + c;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static uint4 mad(uint4 a, uint4 b, uint4 c)
	{
		return a * b + c;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static long mad(long a, long b, long c)
	{
		return a * b + c;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static ulong mad(ulong a, ulong b, ulong c)
	{
		return a * b + c;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static float mad(float a, float b, float c)
	{
		return a * b + c;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static float2 mad(float2 a, float2 b, float2 c)
	{
		return a * b + c;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static float3 mad(float3 a, float3 b, float3 c)
	{
		return a * b + c;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static float4 mad(float4 a, float4 b, float4 c)
	{
		return a * b + c;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static double mad(double a, double b, double c)
	{
		return a * b + c;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static double2 mad(double2 a, double2 b, double2 c)
	{
		return a * b + c;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static double3 mad(double3 a, double3 b, double3 c)
	{
		return a * b + c;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static double4 mad(double4 a, double4 b, double4 c)
	{
		return a * b + c;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static int clamp(int x, int a, int b)
	{
		return max(a, min(b, x));
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static int2 clamp(int2 x, int2 a, int2 b)
	{
		return max(a, min(b, x));
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static int3 clamp(int3 x, int3 a, int3 b)
	{
		return max(a, min(b, x));
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static int4 clamp(int4 x, int4 a, int4 b)
	{
		return max(a, min(b, x));
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static uint clamp(uint x, uint a, uint b)
	{
		return max(a, min(b, x));
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static uint2 clamp(uint2 x, uint2 a, uint2 b)
	{
		return max(a, min(b, x));
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static uint3 clamp(uint3 x, uint3 a, uint3 b)
	{
		return max(a, min(b, x));
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static uint4 clamp(uint4 x, uint4 a, uint4 b)
	{
		return max(a, min(b, x));
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static long clamp(long x, long a, long b)
	{
		return max(a, min(b, x));
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static ulong clamp(ulong x, ulong a, ulong b)
	{
		return max(a, min(b, x));
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static float clamp(float x, float a, float b)
	{
		return max(a, min(b, x));
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static float2 clamp(float2 x, float2 a, float2 b)
	{
		return max(a, min(b, x));
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static float3 clamp(float3 x, float3 a, float3 b)
	{
		return max(a, min(b, x));
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static float4 clamp(float4 x, float4 a, float4 b)
	{
		return max(a, min(b, x));
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static double clamp(double x, double a, double b)
	{
		return max(a, min(b, x));
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static double2 clamp(double2 x, double2 a, double2 b)
	{
		return max(a, min(b, x));
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static double3 clamp(double3 x, double3 a, double3 b)
	{
		return max(a, min(b, x));
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static double4 clamp(double4 x, double4 a, double4 b)
	{
		return max(a, min(b, x));
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static float saturate(float x)
	{
		return clamp(x, 0f, 1f);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static float2 saturate(float2 x)
	{
		return clamp(x, new float2(0f), new float2(1f));
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static float3 saturate(float3 x)
	{
		return clamp(x, new float3(0f), new float3(1f));
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static float4 saturate(float4 x)
	{
		return clamp(x, new float4(0f), new float4(1f));
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static double saturate(double x)
	{
		return clamp(x, 0.0, 1.0);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static double2 saturate(double2 x)
	{
		return clamp(x, new double2(0.0), new double2(1.0));
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static double3 saturate(double3 x)
	{
		return clamp(x, new double3(0.0), new double3(1.0));
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static double4 saturate(double4 x)
	{
		return clamp(x, new double4(0.0), new double4(1.0));
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static int abs(int x)
	{
		return max(-x, x);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static int2 abs(int2 x)
	{
		return max(-x, x);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static int3 abs(int3 x)
	{
		return max(-x, x);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static int4 abs(int4 x)
	{
		return max(-x, x);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static long abs(long x)
	{
		return max(-x, x);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static float abs(float x)
	{
		return asfloat(asuint(x) & 0x7FFFFFFF);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static float2 abs(float2 x)
	{
		return asfloat(asuint(x) & 2147483647u);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static float3 abs(float3 x)
	{
		return asfloat(asuint(x) & 2147483647u);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static float4 abs(float4 x)
	{
		return asfloat(asuint(x) & 2147483647u);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static double abs(double x)
	{
		return asdouble(asulong(x) & 0x7FFFFFFFFFFFFFFFL);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static double2 abs(double2 x)
	{
		return double2(asdouble(asulong(x.x) & 0x7FFFFFFFFFFFFFFFL), asdouble(asulong(x.y) & 0x7FFFFFFFFFFFFFFFL));
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static double3 abs(double3 x)
	{
		return double3(asdouble(asulong(x.x) & 0x7FFFFFFFFFFFFFFFL), asdouble(asulong(x.y) & 0x7FFFFFFFFFFFFFFFL), asdouble(asulong(x.z) & 0x7FFFFFFFFFFFFFFFL));
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static double4 abs(double4 x)
	{
		return double4(asdouble(asulong(x.x) & 0x7FFFFFFFFFFFFFFFL), asdouble(asulong(x.y) & 0x7FFFFFFFFFFFFFFFL), asdouble(asulong(x.z) & 0x7FFFFFFFFFFFFFFFL), asdouble(asulong(x.w) & 0x7FFFFFFFFFFFFFFFL));
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static int dot(int x, int y)
	{
		return x * y;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static int dot(int2 x, int2 y)
	{
		return x.x * y.x + x.y * y.y;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static int dot(int3 x, int3 y)
	{
		return x.x * y.x + x.y * y.y + x.z * y.z;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static int dot(int4 x, int4 y)
	{
		return x.x * y.x + x.y * y.y + x.z * y.z + x.w * y.w;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static uint dot(uint x, uint y)
	{
		return x * y;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static uint dot(uint2 x, uint2 y)
	{
		return x.x * y.x + x.y * y.y;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static uint dot(uint3 x, uint3 y)
	{
		return x.x * y.x + x.y * y.y + x.z * y.z;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static uint dot(uint4 x, uint4 y)
	{
		return x.x * y.x + x.y * y.y + x.z * y.z + x.w * y.w;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static float dot(float x, float y)
	{
		return x * y;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static float dot(float2 x, float2 y)
	{
		return x.x * y.x + x.y * y.y;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static float dot(float3 x, float3 y)
	{
		return x.x * y.x + x.y * y.y + x.z * y.z;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static float dot(float4 x, float4 y)
	{
		return x.x * y.x + x.y * y.y + x.z * y.z + x.w * y.w;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static double dot(double x, double y)
	{
		return x * y;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static double dot(double2 x, double2 y)
	{
		return x.x * y.x + x.y * y.y;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static double dot(double3 x, double3 y)
	{
		return x.x * y.x + x.y * y.y + x.z * y.z;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static double dot(double4 x, double4 y)
	{
		return x.x * y.x + x.y * y.y + x.z * y.z + x.w * y.w;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static float tan(float x)
	{
		return (float)Math.Tan(x);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static float2 tan(float2 x)
	{
		return new float2(tan(x.x), tan(x.y));
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static float3 tan(float3 x)
	{
		return new float3(tan(x.x), tan(x.y), tan(x.z));
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static float4 tan(float4 x)
	{
		return new float4(tan(x.x), tan(x.y), tan(x.z), tan(x.w));
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static double tan(double x)
	{
		return Math.Tan(x);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static double2 tan(double2 x)
	{
		return new double2(tan(x.x), tan(x.y));
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static double3 tan(double3 x)
	{
		return new double3(tan(x.x), tan(x.y), tan(x.z));
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static double4 tan(double4 x)
	{
		return new double4(tan(x.x), tan(x.y), tan(x.z), tan(x.w));
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static float tanh(float x)
	{
		return (float)Math.Tanh(x);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static float2 tanh(float2 x)
	{
		return new float2(tanh(x.x), tanh(x.y));
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static float3 tanh(float3 x)
	{
		return new float3(tanh(x.x), tanh(x.y), tanh(x.z));
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static float4 tanh(float4 x)
	{
		return new float4(tanh(x.x), tanh(x.y), tanh(x.z), tanh(x.w));
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static double tanh(double x)
	{
		return Math.Tanh(x);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static double2 tanh(double2 x)
	{
		return new double2(tanh(x.x), tanh(x.y));
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static double3 tanh(double3 x)
	{
		return new double3(tanh(x.x), tanh(x.y), tanh(x.z));
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static double4 tanh(double4 x)
	{
		return new double4(tanh(x.x), tanh(x.y), tanh(x.z), tanh(x.w));
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static float atan(float x)
	{
		return (float)Math.Atan(x);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static float2 atan(float2 x)
	{
		return new float2(atan(x.x), atan(x.y));
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static float3 atan(float3 x)
	{
		return new float3(atan(x.x), atan(x.y), atan(x.z));
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static float4 atan(float4 x)
	{
		return new float4(atan(x.x), atan(x.y), atan(x.z), atan(x.w));
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static double atan(double x)
	{
		return Math.Atan(x);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static double2 atan(double2 x)
	{
		return new double2(atan(x.x), atan(x.y));
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static double3 atan(double3 x)
	{
		return new double3(atan(x.x), atan(x.y), atan(x.z));
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static double4 atan(double4 x)
	{
		return new double4(atan(x.x), atan(x.y), atan(x.z), atan(x.w));
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static float atan2(float y, float x)
	{
		return (float)Math.Atan2(y, x);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static float2 atan2(float2 y, float2 x)
	{
		return new float2(atan2(y.x, x.x), atan2(y.y, x.y));
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static float3 atan2(float3 y, float3 x)
	{
		return new float3(atan2(y.x, x.x), atan2(y.y, x.y), atan2(y.z, x.z));
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static float4 atan2(float4 y, float4 x)
	{
		return new float4(atan2(y.x, x.x), atan2(y.y, x.y), atan2(y.z, x.z), atan2(y.w, x.w));
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static double atan2(double y, double x)
	{
		return Math.Atan2(y, x);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static double2 atan2(double2 y, double2 x)
	{
		return new double2(atan2(y.x, x.x), atan2(y.y, x.y));
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static double3 atan2(double3 y, double3 x)
	{
		return new double3(atan2(y.x, x.x), atan2(y.y, x.y), atan2(y.z, x.z));
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static double4 atan2(double4 y, double4 x)
	{
		return new double4(atan2(y.x, x.x), atan2(y.y, x.y), atan2(y.z, x.z), atan2(y.w, x.w));
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static float cos(float x)
	{
		return (float)Math.Cos(x);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static float2 cos(float2 x)
	{
		return new float2(cos(x.x), cos(x.y));
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static float3 cos(float3 x)
	{
		return new float3(cos(x.x), cos(x.y), cos(x.z));
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static float4 cos(float4 x)
	{
		return new float4(cos(x.x), cos(x.y), cos(x.z), cos(x.w));
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static double cos(double x)
	{
		return Math.Cos(x);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static double2 cos(double2 x)
	{
		return new double2(cos(x.x), cos(x.y));
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static double3 cos(double3 x)
	{
		return new double3(cos(x.x), cos(x.y), cos(x.z));
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static double4 cos(double4 x)
	{
		return new double4(cos(x.x), cos(x.y), cos(x.z), cos(x.w));
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static float cosh(float x)
	{
		return (float)Math.Cosh(x);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static float2 cosh(float2 x)
	{
		return new float2(cosh(x.x), cosh(x.y));
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static float3 cosh(float3 x)
	{
		return new float3(cosh(x.x), cosh(x.y), cosh(x.z));
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static float4 cosh(float4 x)
	{
		return new float4(cosh(x.x), cosh(x.y), cosh(x.z), cosh(x.w));
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static double cosh(double x)
	{
		return Math.Cosh(x);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static double2 cosh(double2 x)
	{
		return new double2(cosh(x.x), cosh(x.y));
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static double3 cosh(double3 x)
	{
		return new double3(cosh(x.x), cosh(x.y), cosh(x.z));
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static double4 cosh(double4 x)
	{
		return new double4(cosh(x.x), cosh(x.y), cosh(x.z), cosh(x.w));
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static float acos(float x)
	{
		return (float)Math.Acos(x);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static float2 acos(float2 x)
	{
		return new float2(acos(x.x), acos(x.y));
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static float3 acos(float3 x)
	{
		return new float3(acos(x.x), acos(x.y), acos(x.z));
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static float4 acos(float4 x)
	{
		return new float4(acos(x.x), acos(x.y), acos(x.z), acos(x.w));
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static double acos(double x)
	{
		return Math.Acos(x);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static double2 acos(double2 x)
	{
		return new double2(acos(x.x), acos(x.y));
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static double3 acos(double3 x)
	{
		return new double3(acos(x.x), acos(x.y), acos(x.z));
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static double4 acos(double4 x)
	{
		return new double4(acos(x.x), acos(x.y), acos(x.z), acos(x.w));
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static float sin(float x)
	{
		return (float)Math.Sin(x);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static float2 sin(float2 x)
	{
		return new float2(sin(x.x), sin(x.y));
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static float3 sin(float3 x)
	{
		return new float3(sin(x.x), sin(x.y), sin(x.z));
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static float4 sin(float4 x)
	{
		return new float4(sin(x.x), sin(x.y), sin(x.z), sin(x.w));
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static double sin(double x)
	{
		return Math.Sin(x);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static double2 sin(double2 x)
	{
		return new double2(sin(x.x), sin(x.y));
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static double3 sin(double3 x)
	{
		return new double3(sin(x.x), sin(x.y), sin(x.z));
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static double4 sin(double4 x)
	{
		return new double4(sin(x.x), sin(x.y), sin(x.z), sin(x.w));
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static float sinh(float x)
	{
		return (float)Math.Sinh(x);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static float2 sinh(float2 x)
	{
		return new float2(sinh(x.x), sinh(x.y));
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static float3 sinh(float3 x)
	{
		return new float3(sinh(x.x), sinh(x.y), sinh(x.z));
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static float4 sinh(float4 x)
	{
		return new float4(sinh(x.x), sinh(x.y), sinh(x.z), sinh(x.w));
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static double sinh(double x)
	{
		return Math.Sinh(x);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static double2 sinh(double2 x)
	{
		return new double2(sinh(x.x), sinh(x.y));
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static double3 sinh(double3 x)
	{
		return new double3(sinh(x.x), sinh(x.y), sinh(x.z));
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static double4 sinh(double4 x)
	{
		return new double4(sinh(x.x), sinh(x.y), sinh(x.z), sinh(x.w));
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static float asin(float x)
	{
		return (float)Math.Asin(x);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static float2 asin(float2 x)
	{
		return new float2(asin(x.x), asin(x.y));
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static float3 asin(float3 x)
	{
		return new float3(asin(x.x), asin(x.y), asin(x.z));
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static float4 asin(float4 x)
	{
		return new float4(asin(x.x), asin(x.y), asin(x.z), asin(x.w));
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static double asin(double x)
	{
		return Math.Asin(x);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static double2 asin(double2 x)
	{
		return new double2(asin(x.x), asin(x.y));
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static double3 asin(double3 x)
	{
		return new double3(asin(x.x), asin(x.y), asin(x.z));
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static double4 asin(double4 x)
	{
		return new double4(asin(x.x), asin(x.y), asin(x.z), asin(x.w));
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static float floor(float x)
	{
		return (float)Math.Floor(x);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static float2 floor(float2 x)
	{
		return new float2(floor(x.x), floor(x.y));
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static float3 floor(float3 x)
	{
		return new float3(floor(x.x), floor(x.y), floor(x.z));
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static float4 floor(float4 x)
	{
		return new float4(floor(x.x), floor(x.y), floor(x.z), floor(x.w));
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static double floor(double x)
	{
		return Math.Floor(x);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static double2 floor(double2 x)
	{
		return new double2(floor(x.x), floor(x.y));
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static double3 floor(double3 x)
	{
		return new double3(floor(x.x), floor(x.y), floor(x.z));
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static double4 floor(double4 x)
	{
		return new double4(floor(x.x), floor(x.y), floor(x.z), floor(x.w));
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static float ceil(float x)
	{
		return (float)Math.Ceiling(x);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static float2 ceil(float2 x)
	{
		return new float2(ceil(x.x), ceil(x.y));
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static float3 ceil(float3 x)
	{
		return new float3(ceil(x.x), ceil(x.y), ceil(x.z));
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static float4 ceil(float4 x)
	{
		return new float4(ceil(x.x), ceil(x.y), ceil(x.z), ceil(x.w));
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static double ceil(double x)
	{
		return Math.Ceiling(x);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static double2 ceil(double2 x)
	{
		return new double2(ceil(x.x), ceil(x.y));
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static double3 ceil(double3 x)
	{
		return new double3(ceil(x.x), ceil(x.y), ceil(x.z));
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static double4 ceil(double4 x)
	{
		return new double4(ceil(x.x), ceil(x.y), ceil(x.z), ceil(x.w));
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static float round(float x)
	{
		return (float)Math.Round(x);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static float2 round(float2 x)
	{
		return new float2(round(x.x), round(x.y));
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static float3 round(float3 x)
	{
		return new float3(round(x.x), round(x.y), round(x.z));
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static float4 round(float4 x)
	{
		return new float4(round(x.x), round(x.y), round(x.z), round(x.w));
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static double round(double x)
	{
		return Math.Round(x);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static double2 round(double2 x)
	{
		return new double2(round(x.x), round(x.y));
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static double3 round(double3 x)
	{
		return new double3(round(x.x), round(x.y), round(x.z));
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static double4 round(double4 x)
	{
		return new double4(round(x.x), round(x.y), round(x.z), round(x.w));
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static float trunc(float x)
	{
		return (float)Math.Truncate(x);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static float2 trunc(float2 x)
	{
		return new float2(trunc(x.x), trunc(x.y));
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static float3 trunc(float3 x)
	{
		return new float3(trunc(x.x), trunc(x.y), trunc(x.z));
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static float4 trunc(float4 x)
	{
		return new float4(trunc(x.x), trunc(x.y), trunc(x.z), trunc(x.w));
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static double trunc(double x)
	{
		return Math.Truncate(x);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static double2 trunc(double2 x)
	{
		return new double2(trunc(x.x), trunc(x.y));
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static double3 trunc(double3 x)
	{
		return new double3(trunc(x.x), trunc(x.y), trunc(x.z));
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static double4 trunc(double4 x)
	{
		return new double4(trunc(x.x), trunc(x.y), trunc(x.z), trunc(x.w));
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static float frac(float x)
	{
		return x - floor(x);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static float2 frac(float2 x)
	{
		return x - floor(x);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static float3 frac(float3 x)
	{
		return x - floor(x);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static float4 frac(float4 x)
	{
		return x - floor(x);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static double frac(double x)
	{
		return x - floor(x);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static double2 frac(double2 x)
	{
		return x - floor(x);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static double3 frac(double3 x)
	{
		return x - floor(x);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static double4 frac(double4 x)
	{
		return x - floor(x);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static float rcp(float x)
	{
		return 1f / x;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static float2 rcp(float2 x)
	{
		return 1f / x;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static float3 rcp(float3 x)
	{
		return 1f / x;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static float4 rcp(float4 x)
	{
		return 1f / x;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static double rcp(double x)
	{
		return 1.0 / x;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static double2 rcp(double2 x)
	{
		return 1.0 / x;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static double3 rcp(double3 x)
	{
		return 1.0 / x;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static double4 rcp(double4 x)
	{
		return 1.0 / x;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static float sign(float x)
	{
		return ((x > 0f) ? 1f : 0f) - ((x < 0f) ? 1f : 0f);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static float2 sign(float2 x)
	{
		return new float2(sign(x.x), sign(x.y));
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static float3 sign(float3 x)
	{
		return new float3(sign(x.x), sign(x.y), sign(x.z));
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static float4 sign(float4 x)
	{
		return new float4(sign(x.x), sign(x.y), sign(x.z), sign(x.w));
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static double sign(double x)
	{
		if (x != 0.0)
		{
			return ((x > 0.0) ? 1.0 : 0.0) - ((x < 0.0) ? 1.0 : 0.0);
		}
		return 0.0;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static double2 sign(double2 x)
	{
		return new double2(sign(x.x), sign(x.y));
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static double3 sign(double3 x)
	{
		return new double3(sign(x.x), sign(x.y), sign(x.z));
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static double4 sign(double4 x)
	{
		return new double4(sign(x.x), sign(x.y), sign(x.z), sign(x.w));
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static float pow(float x, float y)
	{
		return (float)Math.Pow(x, y);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static float2 pow(float2 x, float2 y)
	{
		return new float2(pow(x.x, y.x), pow(x.y, y.y));
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static float3 pow(float3 x, float3 y)
	{
		return new float3(pow(x.x, y.x), pow(x.y, y.y), pow(x.z, y.z));
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static float4 pow(float4 x, float4 y)
	{
		return new float4(pow(x.x, y.x), pow(x.y, y.y), pow(x.z, y.z), pow(x.w, y.w));
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static double pow(double x, double y)
	{
		return Math.Pow(x, y);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static double2 pow(double2 x, double2 y)
	{
		return new double2(pow(x.x, y.x), pow(x.y, y.y));
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static double3 pow(double3 x, double3 y)
	{
		return new double3(pow(x.x, y.x), pow(x.y, y.y), pow(x.z, y.z));
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static double4 pow(double4 x, double4 y)
	{
		return new double4(pow(x.x, y.x), pow(x.y, y.y), pow(x.z, y.z), pow(x.w, y.w));
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static float exp(float x)
	{
		return (float)Math.Exp(x);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static float2 exp(float2 x)
	{
		return new float2(exp(x.x), exp(x.y));
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static float3 exp(float3 x)
	{
		return new float3(exp(x.x), exp(x.y), exp(x.z));
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static float4 exp(float4 x)
	{
		return new float4(exp(x.x), exp(x.y), exp(x.z), exp(x.w));
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static double exp(double x)
	{
		return Math.Exp(x);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static double2 exp(double2 x)
	{
		return new double2(exp(x.x), exp(x.y));
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static double3 exp(double3 x)
	{
		return new double3(exp(x.x), exp(x.y), exp(x.z));
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static double4 exp(double4 x)
	{
		return new double4(exp(x.x), exp(x.y), exp(x.z), exp(x.w));
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static float exp2(float x)
	{
		return (float)Math.Exp(x * 0.6931472f);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static float2 exp2(float2 x)
	{
		return new float2(exp2(x.x), exp2(x.y));
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static float3 exp2(float3 x)
	{
		return new float3(exp2(x.x), exp2(x.y), exp2(x.z));
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static float4 exp2(float4 x)
	{
		return new float4(exp2(x.x), exp2(x.y), exp2(x.z), exp2(x.w));
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static double exp2(double x)
	{
		return Math.Exp(x * 0.6931471805599453);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static double2 exp2(double2 x)
	{
		return new double2(exp2(x.x), exp2(x.y));
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static double3 exp2(double3 x)
	{
		return new double3(exp2(x.x), exp2(x.y), exp2(x.z));
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static double4 exp2(double4 x)
	{
		return new double4(exp2(x.x), exp2(x.y), exp2(x.z), exp2(x.w));
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static float exp10(float x)
	{
		return (float)Math.Exp(x * 2.3025851f);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static float2 exp10(float2 x)
	{
		return new float2(exp10(x.x), exp10(x.y));
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static float3 exp10(float3 x)
	{
		return new float3(exp10(x.x), exp10(x.y), exp10(x.z));
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static float4 exp10(float4 x)
	{
		return new float4(exp10(x.x), exp10(x.y), exp10(x.z), exp10(x.w));
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static double exp10(double x)
	{
		return Math.Exp(x * 2.302585092994046);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static double2 exp10(double2 x)
	{
		return new double2(exp10(x.x), exp10(x.y));
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static double3 exp10(double3 x)
	{
		return new double3(exp10(x.x), exp10(x.y), exp10(x.z));
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static double4 exp10(double4 x)
	{
		return new double4(exp10(x.x), exp10(x.y), exp10(x.z), exp10(x.w));
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static float log(float x)
	{
		return (float)Math.Log(x);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static float2 log(float2 x)
	{
		return new float2(log(x.x), log(x.y));
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static float3 log(float3 x)
	{
		return new float3(log(x.x), log(x.y), log(x.z));
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static float4 log(float4 x)
	{
		return new float4(log(x.x), log(x.y), log(x.z), log(x.w));
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static double log(double x)
	{
		return Math.Log(x);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static double2 log(double2 x)
	{
		return new double2(log(x.x), log(x.y));
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static double3 log(double3 x)
	{
		return new double3(log(x.x), log(x.y), log(x.z));
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static double4 log(double4 x)
	{
		return new double4(log(x.x), log(x.y), log(x.z), log(x.w));
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static float log2(float x)
	{
		return (float)Math.Log(x, 2.0);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static float2 log2(float2 x)
	{
		return new float2(log2(x.x), log2(x.y));
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static float3 log2(float3 x)
	{
		return new float3(log2(x.x), log2(x.y), log2(x.z));
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static float4 log2(float4 x)
	{
		return new float4(log2(x.x), log2(x.y), log2(x.z), log2(x.w));
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static double log2(double x)
	{
		return Math.Log(x, 2.0);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static double2 log2(double2 x)
	{
		return new double2(log2(x.x), log2(x.y));
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static double3 log2(double3 x)
	{
		return new double3(log2(x.x), log2(x.y), log2(x.z));
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static double4 log2(double4 x)
	{
		return new double4(log2(x.x), log2(x.y), log2(x.z), log2(x.w));
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static float log10(float x)
	{
		return (float)Math.Log10(x);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static float2 log10(float2 x)
	{
		return new float2(log10(x.x), log10(x.y));
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static float3 log10(float3 x)
	{
		return new float3(log10(x.x), log10(x.y), log10(x.z));
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static float4 log10(float4 x)
	{
		return new float4(log10(x.x), log10(x.y), log10(x.z), log10(x.w));
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static double log10(double x)
	{
		return Math.Log10(x);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static double2 log10(double2 x)
	{
		return new double2(log10(x.x), log10(x.y));
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static double3 log10(double3 x)
	{
		return new double3(log10(x.x), log10(x.y), log10(x.z));
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static double4 log10(double4 x)
	{
		return new double4(log10(x.x), log10(x.y), log10(x.z), log10(x.w));
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static float fmod(float x, float y)
	{
		return x % y;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static float2 fmod(float2 x, float2 y)
	{
		return new float2(x.x % y.x, x.y % y.y);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static float3 fmod(float3 x, float3 y)
	{
		return new float3(x.x % y.x, x.y % y.y, x.z % y.z);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static float4 fmod(float4 x, float4 y)
	{
		return new float4(x.x % y.x, x.y % y.y, x.z % y.z, x.w % y.w);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static double fmod(double x, double y)
	{
		return x % y;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static double2 fmod(double2 x, double2 y)
	{
		return new double2(x.x % y.x, x.y % y.y);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static double3 fmod(double3 x, double3 y)
	{
		return new double3(x.x % y.x, x.y % y.y, x.z % y.z);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static double4 fmod(double4 x, double4 y)
	{
		return new double4(x.x % y.x, x.y % y.y, x.z % y.z, x.w % y.w);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static float modf(float x, out float i)
	{
		i = trunc(x);
		return x - i;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static float2 modf(float2 x, out float2 i)
	{
		i = trunc(x);
		return x - i;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static float3 modf(float3 x, out float3 i)
	{
		i = trunc(x);
		return x - i;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static float4 modf(float4 x, out float4 i)
	{
		i = trunc(x);
		return x - i;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static double modf(double x, out double i)
	{
		i = trunc(x);
		return x - i;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static double2 modf(double2 x, out double2 i)
	{
		i = trunc(x);
		return x - i;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static double3 modf(double3 x, out double3 i)
	{
		i = trunc(x);
		return x - i;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static double4 modf(double4 x, out double4 i)
	{
		i = trunc(x);
		return x - i;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static float sqrt(float x)
	{
		return (float)Math.Sqrt(x);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static float2 sqrt(float2 x)
	{
		return new float2(sqrt(x.x), sqrt(x.y));
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static float3 sqrt(float3 x)
	{
		return new float3(sqrt(x.x), sqrt(x.y), sqrt(x.z));
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static float4 sqrt(float4 x)
	{
		return new float4(sqrt(x.x), sqrt(x.y), sqrt(x.z), sqrt(x.w));
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static double sqrt(double x)
	{
		return Math.Sqrt(x);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static double2 sqrt(double2 x)
	{
		return new double2(sqrt(x.x), sqrt(x.y));
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static double3 sqrt(double3 x)
	{
		return new double3(sqrt(x.x), sqrt(x.y), sqrt(x.z));
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static double4 sqrt(double4 x)
	{
		return new double4(sqrt(x.x), sqrt(x.y), sqrt(x.z), sqrt(x.w));
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static float rsqrt(float x)
	{
		return 1f / sqrt(x);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static float2 rsqrt(float2 x)
	{
		return 1f / sqrt(x);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static float3 rsqrt(float3 x)
	{
		return 1f / sqrt(x);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static float4 rsqrt(float4 x)
	{
		return 1f / sqrt(x);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static double rsqrt(double x)
	{
		return 1.0 / sqrt(x);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static double2 rsqrt(double2 x)
	{
		return 1.0 / sqrt(x);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static double3 rsqrt(double3 x)
	{
		return 1.0 / sqrt(x);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static double4 rsqrt(double4 x)
	{
		return 1.0 / sqrt(x);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static float2 normalize(float2 x)
	{
		return rsqrt(dot(x, x)) * x;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static float3 normalize(float3 x)
	{
		return rsqrt(dot(x, x)) * x;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static float4 normalize(float4 x)
	{
		return rsqrt(dot(x, x)) * x;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static double2 normalize(double2 x)
	{
		return rsqrt(dot(x, x)) * x;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static double3 normalize(double3 x)
	{
		return rsqrt(dot(x, x)) * x;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static double4 normalize(double4 x)
	{
		return rsqrt(dot(x, x)) * x;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static float2 normalizesafe(float2 x, float2 defaultvalue = default(float2))
	{
		float num = dot(x, x);
		return select(defaultvalue, x * rsqrt(num), num > 1.1754944E-38f);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static float3 normalizesafe(float3 x, float3 defaultvalue = default(float3))
	{
		float num = dot(x, x);
		return select(defaultvalue, x * rsqrt(num), num > 1.1754944E-38f);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static float4 normalizesafe(float4 x, float4 defaultvalue = default(float4))
	{
		float num = dot(x, x);
		return select(defaultvalue, x * rsqrt(num), num > 1.1754944E-38f);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static double2 normalizesafe(double2 x, double2 defaultvalue = default(double2))
	{
		double num = dot(x, x);
		return select(defaultvalue, x * rsqrt(num), num > 1.1754943508222875E-38);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static double3 normalizesafe(double3 x, double3 defaultvalue = default(double3))
	{
		double num = dot(x, x);
		return select(defaultvalue, x * rsqrt(num), num > 1.1754943508222875E-38);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static double4 normalizesafe(double4 x, double4 defaultvalue = default(double4))
	{
		double num = dot(x, x);
		return select(defaultvalue, x * rsqrt(num), num > 1.1754943508222875E-38);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static float length(float x)
	{
		return abs(x);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static float length(float2 x)
	{
		return sqrt(dot(x, x));
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static float length(float3 x)
	{
		return sqrt(dot(x, x));
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static float length(float4 x)
	{
		return sqrt(dot(x, x));
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static double length(double x)
	{
		return abs(x);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static double length(double2 x)
	{
		return sqrt(dot(x, x));
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static double length(double3 x)
	{
		return sqrt(dot(x, x));
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static double length(double4 x)
	{
		return sqrt(dot(x, x));
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static float lengthsq(float x)
	{
		return x * x;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static float lengthsq(float2 x)
	{
		return dot(x, x);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static float lengthsq(float3 x)
	{
		return dot(x, x);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static float lengthsq(float4 x)
	{
		return dot(x, x);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static double lengthsq(double x)
	{
		return x * x;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static double lengthsq(double2 x)
	{
		return dot(x, x);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static double lengthsq(double3 x)
	{
		return dot(x, x);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static double lengthsq(double4 x)
	{
		return dot(x, x);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static float distance(float x, float y)
	{
		return abs(y - x);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static float distance(float2 x, float2 y)
	{
		return length(y - x);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static float distance(float3 x, float3 y)
	{
		return length(y - x);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static float distance(float4 x, float4 y)
	{
		return length(y - x);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static double distance(double x, double y)
	{
		return abs(y - x);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static double distance(double2 x, double2 y)
	{
		return length(y - x);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static double distance(double3 x, double3 y)
	{
		return length(y - x);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static double distance(double4 x, double4 y)
	{
		return length(y - x);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static float distancesq(float x, float y)
	{
		return (y - x) * (y - x);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static float distancesq(float2 x, float2 y)
	{
		return lengthsq(y - x);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static float distancesq(float3 x, float3 y)
	{
		return lengthsq(y - x);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static float distancesq(float4 x, float4 y)
	{
		return lengthsq(y - x);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static double distancesq(double x, double y)
	{
		return (y - x) * (y - x);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static double distancesq(double2 x, double2 y)
	{
		return lengthsq(y - x);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static double distancesq(double3 x, double3 y)
	{
		return lengthsq(y - x);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static double distancesq(double4 x, double4 y)
	{
		return lengthsq(y - x);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static float3 cross(float3 x, float3 y)
	{
		return (x * y.yzx - x.yzx * y).yzx;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static double3 cross(double3 x, double3 y)
	{
		return (x * y.yzx - x.yzx * y).yzx;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static float smoothstep(float a, float b, float x)
	{
		float num = saturate((x - a) / (b - a));
		return num * num * (3f - 2f * num);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static float2 smoothstep(float2 a, float2 b, float2 x)
	{
		float2 float5 = saturate((x - a) / (b - a));
		return float5 * float5 * (3f - 2f * float5);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static float3 smoothstep(float3 a, float3 b, float3 x)
	{
		float3 float5 = saturate((x - a) / (b - a));
		return float5 * float5 * (3f - 2f * float5);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static float4 smoothstep(float4 a, float4 b, float4 x)
	{
		float4 float5 = saturate((x - a) / (b - a));
		return float5 * float5 * (3f - 2f * float5);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static double smoothstep(double a, double b, double x)
	{
		double num = saturate((x - a) / (b - a));
		return num * num * (3.0 - 2.0 * num);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static double2 smoothstep(double2 a, double2 b, double2 x)
	{
		double2 double5 = saturate((x - a) / (b - a));
		return double5 * double5 * (3.0 - 2.0 * double5);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static double3 smoothstep(double3 a, double3 b, double3 x)
	{
		double3 double5 = saturate((x - a) / (b - a));
		return double5 * double5 * (3.0 - 2.0 * double5);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static double4 smoothstep(double4 a, double4 b, double4 x)
	{
		double4 double5 = saturate((x - a) / (b - a));
		return double5 * double5 * (3.0 - 2.0 * double5);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool any(bool2 x)
	{
		if (!x.x)
		{
			return x.y;
		}
		return true;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool any(bool3 x)
	{
		if (!x.x && !x.y)
		{
			return x.z;
		}
		return true;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool any(bool4 x)
	{
		if (!x.x && !x.y && !x.z)
		{
			return x.w;
		}
		return true;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool any(int2 x)
	{
		if (x.x == 0)
		{
			return x.y != 0;
		}
		return true;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool any(int3 x)
	{
		if (x.x == 0 && x.y == 0)
		{
			return x.z != 0;
		}
		return true;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool any(int4 x)
	{
		if (x.x == 0 && x.y == 0 && x.z == 0)
		{
			return x.w != 0;
		}
		return true;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool any(uint2 x)
	{
		if (x.x == 0)
		{
			return x.y != 0;
		}
		return true;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool any(uint3 x)
	{
		if (x.x == 0 && x.y == 0)
		{
			return x.z != 0;
		}
		return true;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool any(uint4 x)
	{
		if (x.x == 0 && x.y == 0 && x.z == 0)
		{
			return x.w != 0;
		}
		return true;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool any(float2 x)
	{
		if (x.x == 0f)
		{
			return x.y != 0f;
		}
		return true;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool any(float3 x)
	{
		if (x.x == 0f && x.y == 0f)
		{
			return x.z != 0f;
		}
		return true;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool any(float4 x)
	{
		if (x.x == 0f && x.y == 0f && x.z == 0f)
		{
			return x.w != 0f;
		}
		return true;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool any(double2 x)
	{
		if (x.x == 0.0)
		{
			return x.y != 0.0;
		}
		return true;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool any(double3 x)
	{
		if (x.x == 0.0 && x.y == 0.0)
		{
			return x.z != 0.0;
		}
		return true;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool any(double4 x)
	{
		if (x.x == 0.0 && x.y == 0.0 && x.z == 0.0)
		{
			return x.w != 0.0;
		}
		return true;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool all(bool2 x)
	{
		if (x.x)
		{
			return x.y;
		}
		return false;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool all(bool3 x)
	{
		if (x.x && x.y)
		{
			return x.z;
		}
		return false;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool all(bool4 x)
	{
		if (x.x && x.y && x.z)
		{
			return x.w;
		}
		return false;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool all(int2 x)
	{
		if (x.x != 0)
		{
			return x.y != 0;
		}
		return false;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool all(int3 x)
	{
		if (x.x != 0 && x.y != 0)
		{
			return x.z != 0;
		}
		return false;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool all(int4 x)
	{
		if (x.x != 0 && x.y != 0 && x.z != 0)
		{
			return x.w != 0;
		}
		return false;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool all(uint2 x)
	{
		if (x.x != 0)
		{
			return x.y != 0;
		}
		return false;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool all(uint3 x)
	{
		if (x.x != 0 && x.y != 0)
		{
			return x.z != 0;
		}
		return false;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool all(uint4 x)
	{
		if (x.x != 0 && x.y != 0 && x.z != 0)
		{
			return x.w != 0;
		}
		return false;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool all(float2 x)
	{
		if (x.x != 0f)
		{
			return x.y != 0f;
		}
		return false;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool all(float3 x)
	{
		if (x.x != 0f && x.y != 0f)
		{
			return x.z != 0f;
		}
		return false;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool all(float4 x)
	{
		if (x.x != 0f && x.y != 0f && x.z != 0f)
		{
			return x.w != 0f;
		}
		return false;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool all(double2 x)
	{
		if (x.x != 0.0)
		{
			return x.y != 0.0;
		}
		return false;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool all(double3 x)
	{
		if (x.x != 0.0 && x.y != 0.0)
		{
			return x.z != 0.0;
		}
		return false;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool all(double4 x)
	{
		if (x.x != 0.0 && x.y != 0.0 && x.z != 0.0)
		{
			return x.w != 0.0;
		}
		return false;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static int select(int a, int b, bool c)
	{
		if (!c)
		{
			return a;
		}
		return b;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static int2 select(int2 a, int2 b, bool c)
	{
		if (!c)
		{
			return a;
		}
		return b;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static int3 select(int3 a, int3 b, bool c)
	{
		if (!c)
		{
			return a;
		}
		return b;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static int4 select(int4 a, int4 b, bool c)
	{
		if (!c)
		{
			return a;
		}
		return b;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static int2 select(int2 a, int2 b, bool2 c)
	{
		return new int2(c.x ? b.x : a.x, c.y ? b.y : a.y);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static int3 select(int3 a, int3 b, bool3 c)
	{
		return new int3(c.x ? b.x : a.x, c.y ? b.y : a.y, c.z ? b.z : a.z);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static int4 select(int4 a, int4 b, bool4 c)
	{
		return new int4(c.x ? b.x : a.x, c.y ? b.y : a.y, c.z ? b.z : a.z, c.w ? b.w : a.w);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static uint select(uint a, uint b, bool c)
	{
		if (!c)
		{
			return a;
		}
		return b;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static uint2 select(uint2 a, uint2 b, bool c)
	{
		if (!c)
		{
			return a;
		}
		return b;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static uint3 select(uint3 a, uint3 b, bool c)
	{
		if (!c)
		{
			return a;
		}
		return b;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static uint4 select(uint4 a, uint4 b, bool c)
	{
		if (!c)
		{
			return a;
		}
		return b;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static uint2 select(uint2 a, uint2 b, bool2 c)
	{
		return new uint2(c.x ? b.x : a.x, c.y ? b.y : a.y);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static uint3 select(uint3 a, uint3 b, bool3 c)
	{
		return new uint3(c.x ? b.x : a.x, c.y ? b.y : a.y, c.z ? b.z : a.z);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static uint4 select(uint4 a, uint4 b, bool4 c)
	{
		return new uint4(c.x ? b.x : a.x, c.y ? b.y : a.y, c.z ? b.z : a.z, c.w ? b.w : a.w);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static long select(long a, long b, bool c)
	{
		if (!c)
		{
			return a;
		}
		return b;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static ulong select(ulong a, ulong b, bool c)
	{
		if (!c)
		{
			return a;
		}
		return b;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static float select(float a, float b, bool c)
	{
		if (!c)
		{
			return a;
		}
		return b;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static float2 select(float2 a, float2 b, bool c)
	{
		if (!c)
		{
			return a;
		}
		return b;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static float3 select(float3 a, float3 b, bool c)
	{
		if (!c)
		{
			return a;
		}
		return b;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static float4 select(float4 a, float4 b, bool c)
	{
		if (!c)
		{
			return a;
		}
		return b;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static float2 select(float2 a, float2 b, bool2 c)
	{
		return new float2(c.x ? b.x : a.x, c.y ? b.y : a.y);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static float3 select(float3 a, float3 b, bool3 c)
	{
		return new float3(c.x ? b.x : a.x, c.y ? b.y : a.y, c.z ? b.z : a.z);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static float4 select(float4 a, float4 b, bool4 c)
	{
		return new float4(c.x ? b.x : a.x, c.y ? b.y : a.y, c.z ? b.z : a.z, c.w ? b.w : a.w);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static double select(double a, double b, bool c)
	{
		if (!c)
		{
			return a;
		}
		return b;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static double2 select(double2 a, double2 b, bool c)
	{
		if (!c)
		{
			return a;
		}
		return b;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static double3 select(double3 a, double3 b, bool c)
	{
		if (!c)
		{
			return a;
		}
		return b;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static double4 select(double4 a, double4 b, bool c)
	{
		if (!c)
		{
			return a;
		}
		return b;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static double2 select(double2 a, double2 b, bool2 c)
	{
		return new double2(c.x ? b.x : a.x, c.y ? b.y : a.y);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static double3 select(double3 a, double3 b, bool3 c)
	{
		return new double3(c.x ? b.x : a.x, c.y ? b.y : a.y, c.z ? b.z : a.z);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static double4 select(double4 a, double4 b, bool4 c)
	{
		return new double4(c.x ? b.x : a.x, c.y ? b.y : a.y, c.z ? b.z : a.z, c.w ? b.w : a.w);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static float step(float y, float x)
	{
		return select(0f, 1f, x >= y);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static float2 step(float2 y, float2 x)
	{
		return select(float2(0f), float2(1f), x >= y);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static float3 step(float3 y, float3 x)
	{
		return select(float3(0f), float3(1f), x >= y);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static float4 step(float4 y, float4 x)
	{
		return select(float4(0f), float4(1f), x >= y);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static double step(double y, double x)
	{
		return select(0.0, 1.0, x >= y);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static double2 step(double2 y, double2 x)
	{
		return select(double2(0.0), double2(1.0), x >= y);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static double3 step(double3 y, double3 x)
	{
		return select(double3(0.0), double3(1.0), x >= y);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static double4 step(double4 y, double4 x)
	{
		return select(double4(0.0), double4(1.0), x >= y);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static float2 reflect(float2 i, float2 n)
	{
		return i - 2f * n * dot(i, n);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static float3 reflect(float3 i, float3 n)
	{
		return i - 2f * n * dot(i, n);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static float4 reflect(float4 i, float4 n)
	{
		return i - 2f * n * dot(i, n);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static double2 reflect(double2 i, double2 n)
	{
		return i - 2.0 * n * dot(i, n);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static double3 reflect(double3 i, double3 n)
	{
		return i - 2.0 * n * dot(i, n);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static double4 reflect(double4 i, double4 n)
	{
		return i - 2.0 * n * dot(i, n);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static float2 refract(float2 i, float2 n, float eta)
	{
		float num = dot(n, i);
		float num2 = 1f - eta * eta * (1f - num * num);
		return select(0f, eta * i - (eta * num + sqrt(num2)) * n, num2 >= 0f);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static float3 refract(float3 i, float3 n, float eta)
	{
		float num = dot(n, i);
		float num2 = 1f - eta * eta * (1f - num * num);
		return select(0f, eta * i - (eta * num + sqrt(num2)) * n, num2 >= 0f);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static float4 refract(float4 i, float4 n, float eta)
	{
		float num = dot(n, i);
		float num2 = 1f - eta * eta * (1f - num * num);
		return select(0f, eta * i - (eta * num + sqrt(num2)) * n, num2 >= 0f);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static double2 refract(double2 i, double2 n, double eta)
	{
		double num = dot(n, i);
		double num2 = 1.0 - eta * eta * (1.0 - num * num);
		return select(0f, eta * i - (eta * num + sqrt(num2)) * n, num2 >= 0.0);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static double3 refract(double3 i, double3 n, double eta)
	{
		double num = dot(n, i);
		double num2 = 1.0 - eta * eta * (1.0 - num * num);
		return select(0f, eta * i - (eta * num + sqrt(num2)) * n, num2 >= 0.0);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static double4 refract(double4 i, double4 n, double eta)
	{
		double num = dot(n, i);
		double num2 = 1.0 - eta * eta * (1.0 - num * num);
		return select(0f, eta * i - (eta * num + sqrt(num2)) * n, num2 >= 0.0);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static float2 project(float2 a, float2 b)
	{
		return dot(a, b) / dot(b, b) * b;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static float3 project(float3 a, float3 b)
	{
		return dot(a, b) / dot(b, b) * b;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static float4 project(float4 a, float4 b)
	{
		return dot(a, b) / dot(b, b) * b;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static float2 projectsafe(float2 a, float2 b, float2 defaultValue = default(float2))
	{
		float2 float5 = project(a, b);
		return select(defaultValue, float5, all(isfinite(float5)));
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static float3 projectsafe(float3 a, float3 b, float3 defaultValue = default(float3))
	{
		float3 float5 = project(a, b);
		return select(defaultValue, float5, all(isfinite(float5)));
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static float4 projectsafe(float4 a, float4 b, float4 defaultValue = default(float4))
	{
		float4 float5 = project(a, b);
		return select(defaultValue, float5, all(isfinite(float5)));
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static double2 project(double2 a, double2 b)
	{
		return dot(a, b) / dot(b, b) * b;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static double3 project(double3 a, double3 b)
	{
		return dot(a, b) / dot(b, b) * b;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static double4 project(double4 a, double4 b)
	{
		return dot(a, b) / dot(b, b) * b;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static double2 projectsafe(double2 a, double2 b, double2 defaultValue = default(double2))
	{
		double2 double5 = project(a, b);
		return select(defaultValue, double5, all(isfinite(double5)));
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static double3 projectsafe(double3 a, double3 b, double3 defaultValue = default(double3))
	{
		double3 double5 = project(a, b);
		return select(defaultValue, double5, all(isfinite(double5)));
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static double4 projectsafe(double4 a, double4 b, double4 defaultValue = default(double4))
	{
		double4 double5 = project(a, b);
		return select(defaultValue, double5, all(isfinite(double5)));
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static float2 faceforward(float2 n, float2 i, float2 ng)
	{
		return select(n, -n, dot(ng, i) >= 0f);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static float3 faceforward(float3 n, float3 i, float3 ng)
	{
		return select(n, -n, dot(ng, i) >= 0f);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static float4 faceforward(float4 n, float4 i, float4 ng)
	{
		return select(n, -n, dot(ng, i) >= 0f);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static double2 faceforward(double2 n, double2 i, double2 ng)
	{
		return select(n, -n, dot(ng, i) >= 0.0);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static double3 faceforward(double3 n, double3 i, double3 ng)
	{
		return select(n, -n, dot(ng, i) >= 0.0);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static double4 faceforward(double4 n, double4 i, double4 ng)
	{
		return select(n, -n, dot(ng, i) >= 0.0);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static void sincos(float x, out float s, out float c)
	{
		s = sin(x);
		c = cos(x);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static void sincos(float2 x, out float2 s, out float2 c)
	{
		s = sin(x);
		c = cos(x);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static void sincos(float3 x, out float3 s, out float3 c)
	{
		s = sin(x);
		c = cos(x);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static void sincos(float4 x, out float4 s, out float4 c)
	{
		s = sin(x);
		c = cos(x);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static void sincos(double x, out double s, out double c)
	{
		s = sin(x);
		c = cos(x);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static void sincos(double2 x, out double2 s, out double2 c)
	{
		s = sin(x);
		c = cos(x);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static void sincos(double3 x, out double3 s, out double3 c)
	{
		s = sin(x);
		c = cos(x);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static void sincos(double4 x, out double4 s, out double4 c)
	{
		s = sin(x);
		c = cos(x);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static int countbits(int x)
	{
		return countbits((uint)x);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static int2 countbits(int2 x)
	{
		return countbits((uint2)x);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static int3 countbits(int3 x)
	{
		return countbits((uint3)x);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static int4 countbits(int4 x)
	{
		return countbits((uint4)x);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static int countbits(uint x)
	{
		x -= (x >> 1) & 0x55555555;
		x = (x & 0x33333333) + ((x >> 2) & 0x33333333);
		return (int)(((x + (x >> 4)) & 0xF0F0F0F) * 16843009 >> 24);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static int2 countbits(uint2 x)
	{
		x -= (x >> 1) & 1431655765u;
		x = (x & 858993459u) + ((x >> 2) & 858993459u);
		return int2(((x + (x >> 4)) & 252645135u) * 16843009u >> 24);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static int3 countbits(uint3 x)
	{
		x -= (x >> 1) & 1431655765u;
		x = (x & 858993459u) + ((x >> 2) & 858993459u);
		return int3(((x + (x >> 4)) & 252645135u) * 16843009u >> 24);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static int4 countbits(uint4 x)
	{
		x -= (x >> 1) & 1431655765u;
		x = (x & 858993459u) + ((x >> 2) & 858993459u);
		return int4(((x + (x >> 4)) & 252645135u) * 16843009u >> 24);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static int countbits(ulong x)
	{
		x -= (x >> 1) & 0x5555555555555555L;
		x = (x & 0x3333333333333333L) + ((x >> 2) & 0x3333333333333333L);
		return (int)(((x + (x >> 4)) & 0xF0F0F0F0F0F0F0FL) * 72340172838076673L >> 56);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static int countbits(long x)
	{
		return countbits((ulong)x);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static int lzcnt(int x)
	{
		return lzcnt((uint)x);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static int2 lzcnt(int2 x)
	{
		return int2(lzcnt(x.x), lzcnt(x.y));
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static int3 lzcnt(int3 x)
	{
		return int3(lzcnt(x.x), lzcnt(x.y), lzcnt(x.z));
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static int4 lzcnt(int4 x)
	{
		return int4(lzcnt(x.x), lzcnt(x.y), lzcnt(x.z), lzcnt(x.w));
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static int lzcnt(uint x)
	{
		if (x == 0)
		{
			return 32;
		}
		LongDoubleUnion longDoubleUnion = default(LongDoubleUnion);
		longDoubleUnion.doubleValue = 0.0;
		longDoubleUnion.longValue = 4841369599423283200L + (long)x;
		longDoubleUnion.doubleValue -= 4503599627370496.0;
		return 1054 - (int)(longDoubleUnion.longValue >> 52);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static int2 lzcnt(uint2 x)
	{
		return int2(lzcnt(x.x), lzcnt(x.y));
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static int3 lzcnt(uint3 x)
	{
		return int3(lzcnt(x.x), lzcnt(x.y), lzcnt(x.z));
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static int4 lzcnt(uint4 x)
	{
		return int4(lzcnt(x.x), lzcnt(x.y), lzcnt(x.z), lzcnt(x.w));
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static int lzcnt(long x)
	{
		return lzcnt((ulong)x);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static int lzcnt(ulong x)
	{
		if (x == 0L)
		{
			return 64;
		}
		uint num = (uint)(x >> 32);
		uint num2 = (uint)((num != 0) ? num : x);
		int num3 = ((num != 0) ? 1054 : 1086);
		LongDoubleUnion longDoubleUnion = default(LongDoubleUnion);
		longDoubleUnion.doubleValue = 0.0;
		longDoubleUnion.longValue = 4841369599423283200L + (long)num2;
		longDoubleUnion.doubleValue -= 4503599627370496.0;
		return num3 - (int)(longDoubleUnion.longValue >> 52);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static int tzcnt(int x)
	{
		return tzcnt((uint)x);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static int2 tzcnt(int2 x)
	{
		return int2(tzcnt(x.x), tzcnt(x.y));
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static int3 tzcnt(int3 v)
	{
		return int3(tzcnt(v.x), tzcnt(v.y), tzcnt(v.z));
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static int4 tzcnt(int4 v)
	{
		return int4(tzcnt(v.x), tzcnt(v.y), tzcnt(v.z), tzcnt(v.w));
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static int tzcnt(uint x)
	{
		if (x == 0)
		{
			return 32;
		}
		x &= (uint)(int)(0L - (long)x);
		LongDoubleUnion longDoubleUnion = default(LongDoubleUnion);
		longDoubleUnion.doubleValue = 0.0;
		longDoubleUnion.longValue = 4841369599423283200L + (long)x;
		longDoubleUnion.doubleValue -= 4503599627370496.0;
		return (int)(longDoubleUnion.longValue >> 52) - 1023;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static int2 tzcnt(uint2 x)
	{
		return int2(tzcnt(x.x), tzcnt(x.y));
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static int3 tzcnt(uint3 x)
	{
		return int3(tzcnt(x.x), tzcnt(x.y), tzcnt(x.z));
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static int4 tzcnt(uint4 x)
	{
		return int4(tzcnt(x.x), tzcnt(x.y), tzcnt(x.z), tzcnt(x.w));
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static int tzcnt(long x)
	{
		return tzcnt((ulong)x);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static int tzcnt(ulong x)
	{
		if (x == 0L)
		{
			return 64;
		}
		x &= 0L - x;
		uint num = (uint)x;
		uint num2 = (uint)((num != 0) ? num : (x >> 32));
		int num3 = ((num != 0) ? 1023 : 991);
		LongDoubleUnion longDoubleUnion = default(LongDoubleUnion);
		longDoubleUnion.doubleValue = 0.0;
		longDoubleUnion.longValue = 4841369599423283200L + (long)num2;
		longDoubleUnion.doubleValue -= 4503599627370496.0;
		return (int)(longDoubleUnion.longValue >> 52) - num3;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static int reversebits(int x)
	{
		return (int)reversebits((uint)x);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static int2 reversebits(int2 x)
	{
		return (int2)reversebits((uint2)x);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static int3 reversebits(int3 x)
	{
		return (int3)reversebits((uint3)x);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static int4 reversebits(int4 x)
	{
		return (int4)reversebits((uint4)x);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static uint reversebits(uint x)
	{
		x = ((x >> 1) & 0x55555555) | ((x & 0x55555555) << 1);
		x = ((x >> 2) & 0x33333333) | ((x & 0x33333333) << 2);
		x = ((x >> 4) & 0xF0F0F0F) | ((x & 0xF0F0F0F) << 4);
		x = ((x >> 8) & 0xFF00FF) | ((x & 0xFF00FF) << 8);
		return (x >> 16) | (x << 16);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static uint2 reversebits(uint2 x)
	{
		x = ((x >> 1) & 1431655765u) | ((x & 1431655765u) << 1);
		x = ((x >> 2) & 858993459u) | ((x & 858993459u) << 2);
		x = ((x >> 4) & 252645135u) | ((x & 252645135u) << 4);
		x = ((x >> 8) & 16711935u) | ((x & 16711935u) << 8);
		return (x >> 16) | (x << 16);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static uint3 reversebits(uint3 x)
	{
		x = ((x >> 1) & 1431655765u) | ((x & 1431655765u) << 1);
		x = ((x >> 2) & 858993459u) | ((x & 858993459u) << 2);
		x = ((x >> 4) & 252645135u) | ((x & 252645135u) << 4);
		x = ((x >> 8) & 16711935u) | ((x & 16711935u) << 8);
		return (x >> 16) | (x << 16);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static uint4 reversebits(uint4 x)
	{
		x = ((x >> 1) & 1431655765u) | ((x & 1431655765u) << 1);
		x = ((x >> 2) & 858993459u) | ((x & 858993459u) << 2);
		x = ((x >> 4) & 252645135u) | ((x & 252645135u) << 4);
		x = ((x >> 8) & 16711935u) | ((x & 16711935u) << 8);
		return (x >> 16) | (x << 16);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static long reversebits(long x)
	{
		return (long)reversebits((ulong)x);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static ulong reversebits(ulong x)
	{
		x = ((x >> 1) & 0x5555555555555555L) | ((x & 0x5555555555555555L) << 1);
		x = ((x >> 2) & 0x3333333333333333L) | ((x & 0x3333333333333333L) << 2);
		x = ((x >> 4) & 0xF0F0F0F0F0F0F0FL) | ((x & 0xF0F0F0F0F0F0F0FL) << 4);
		x = ((x >> 8) & 0xFF00FF00FF00FFL) | ((x & 0xFF00FF00FF00FFL) << 8);
		x = ((x >> 16) & 0xFFFF0000FFFFL) | ((x & 0xFFFF0000FFFFL) << 16);
		return (x >> 32) | (x << 32);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static int rol(int x, int n)
	{
		return (int)rol((uint)x, n);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static int2 rol(int2 x, int n)
	{
		return (int2)rol((uint2)x, n);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static int3 rol(int3 x, int n)
	{
		return (int3)rol((uint3)x, n);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static int4 rol(int4 x, int n)
	{
		return (int4)rol((uint4)x, n);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static uint rol(uint x, int n)
	{
		return (x << n) | (x >> 32 - n);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static uint2 rol(uint2 x, int n)
	{
		return (x << n) | (x >> 32 - n);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static uint3 rol(uint3 x, int n)
	{
		return (x << n) | (x >> 32 - n);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static uint4 rol(uint4 x, int n)
	{
		return (x << n) | (x >> 32 - n);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static long rol(long x, int n)
	{
		return (long)rol((ulong)x, n);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static ulong rol(ulong x, int n)
	{
		return (x << n) | (x >> 64 - n);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static int ror(int x, int n)
	{
		return (int)ror((uint)x, n);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static int2 ror(int2 x, int n)
	{
		return (int2)ror((uint2)x, n);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static int3 ror(int3 x, int n)
	{
		return (int3)ror((uint3)x, n);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static int4 ror(int4 x, int n)
	{
		return (int4)ror((uint4)x, n);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static uint ror(uint x, int n)
	{
		return (x >> n) | (x << 32 - n);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static uint2 ror(uint2 x, int n)
	{
		return (x >> n) | (x << 32 - n);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static uint3 ror(uint3 x, int n)
	{
		return (x >> n) | (x << 32 - n);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static uint4 ror(uint4 x, int n)
	{
		return (x >> n) | (x << 32 - n);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static long ror(long x, int n)
	{
		return (long)ror((ulong)x, n);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static ulong ror(ulong x, int n)
	{
		return (x >> n) | (x << 64 - n);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static int ceilpow2(int x)
	{
		x--;
		x |= x >> 1;
		x |= x >> 2;
		x |= x >> 4;
		x |= x >> 8;
		x |= x >> 16;
		return x + 1;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static int2 ceilpow2(int2 x)
	{
		x -= 1;
		x |= x >> 1;
		x |= x >> 2;
		x |= x >> 4;
		x |= x >> 8;
		x |= x >> 16;
		return x + 1;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static int3 ceilpow2(int3 x)
	{
		x -= 1;
		x |= x >> 1;
		x |= x >> 2;
		x |= x >> 4;
		x |= x >> 8;
		x |= x >> 16;
		return x + 1;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static int4 ceilpow2(int4 x)
	{
		x -= 1;
		x |= x >> 1;
		x |= x >> 2;
		x |= x >> 4;
		x |= x >> 8;
		x |= x >> 16;
		return x + 1;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static uint ceilpow2(uint x)
	{
		x--;
		x |= x >> 1;
		x |= x >> 2;
		x |= x >> 4;
		x |= x >> 8;
		x |= x >> 16;
		return x + 1;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static uint2 ceilpow2(uint2 x)
	{
		x -= 1u;
		x |= x >> 1;
		x |= x >> 2;
		x |= x >> 4;
		x |= x >> 8;
		x |= x >> 16;
		return x + 1u;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static uint3 ceilpow2(uint3 x)
	{
		x -= 1u;
		x |= x >> 1;
		x |= x >> 2;
		x |= x >> 4;
		x |= x >> 8;
		x |= x >> 16;
		return x + 1u;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static uint4 ceilpow2(uint4 x)
	{
		x -= 1u;
		x |= x >> 1;
		x |= x >> 2;
		x |= x >> 4;
		x |= x >> 8;
		x |= x >> 16;
		return x + 1u;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static long ceilpow2(long x)
	{
		x--;
		x |= x >> 1;
		x |= x >> 2;
		x |= x >> 4;
		x |= x >> 8;
		x |= x >> 16;
		x |= x >> 32;
		return x + 1;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static ulong ceilpow2(ulong x)
	{
		x--;
		x |= x >> 1;
		x |= x >> 2;
		x |= x >> 4;
		x |= x >> 8;
		x |= x >> 16;
		x |= x >> 32;
		return x + 1;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static int ceillog2(int x)
	{
		return 32 - lzcnt((uint)(x - 1));
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static int2 ceillog2(int2 x)
	{
		return new int2(ceillog2(x.x), ceillog2(x.y));
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static int3 ceillog2(int3 x)
	{
		return new int3(ceillog2(x.x), ceillog2(x.y), ceillog2(x.z));
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static int4 ceillog2(int4 x)
	{
		return new int4(ceillog2(x.x), ceillog2(x.y), ceillog2(x.z), ceillog2(x.w));
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static int ceillog2(uint x)
	{
		return 32 - lzcnt(x - 1);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static int2 ceillog2(uint2 x)
	{
		return new int2(ceillog2(x.x), ceillog2(x.y));
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static int3 ceillog2(uint3 x)
	{
		return new int3(ceillog2(x.x), ceillog2(x.y), ceillog2(x.z));
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static int4 ceillog2(uint4 x)
	{
		return new int4(ceillog2(x.x), ceillog2(x.y), ceillog2(x.z), ceillog2(x.w));
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static int floorlog2(int x)
	{
		return 31 - lzcnt((uint)x);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static int2 floorlog2(int2 x)
	{
		return new int2(floorlog2(x.x), floorlog2(x.y));
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static int3 floorlog2(int3 x)
	{
		return new int3(floorlog2(x.x), floorlog2(x.y), floorlog2(x.z));
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static int4 floorlog2(int4 x)
	{
		return new int4(floorlog2(x.x), floorlog2(x.y), floorlog2(x.z), floorlog2(x.w));
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static int floorlog2(uint x)
	{
		return 31 - lzcnt(x);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static int2 floorlog2(uint2 x)
	{
		return new int2(floorlog2(x.x), floorlog2(x.y));
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static int3 floorlog2(uint3 x)
	{
		return new int3(floorlog2(x.x), floorlog2(x.y), floorlog2(x.z));
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static int4 floorlog2(uint4 x)
	{
		return new int4(floorlog2(x.x), floorlog2(x.y), floorlog2(x.z), floorlog2(x.w));
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static float radians(float x)
	{
		return x * (MathF.PI / 180f);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static float2 radians(float2 x)
	{
		return x * (MathF.PI / 180f);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static float3 radians(float3 x)
	{
		return x * (MathF.PI / 180f);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static float4 radians(float4 x)
	{
		return x * (MathF.PI / 180f);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static double radians(double x)
	{
		return x * (Math.PI / 180.0);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static double2 radians(double2 x)
	{
		return x * (Math.PI / 180.0);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static double3 radians(double3 x)
	{
		return x * (Math.PI / 180.0);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static double4 radians(double4 x)
	{
		return x * (Math.PI / 180.0);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static float degrees(float x)
	{
		return x * 57.29578f;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static float2 degrees(float2 x)
	{
		return x * 57.29578f;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static float3 degrees(float3 x)
	{
		return x * 57.29578f;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static float4 degrees(float4 x)
	{
		return x * 57.29578f;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static double degrees(double x)
	{
		return x * (180.0 / Math.PI);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static double2 degrees(double2 x)
	{
		return x * (180.0 / Math.PI);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static double3 degrees(double3 x)
	{
		return x * (180.0 / Math.PI);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static double4 degrees(double4 x)
	{
		return x * (180.0 / Math.PI);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static int cmin(int2 x)
	{
		return min(x.x, x.y);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static int cmin(int3 x)
	{
		return min(min(x.x, x.y), x.z);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static int cmin(int4 x)
	{
		return min(min(x.x, x.y), min(x.z, x.w));
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static uint cmin(uint2 x)
	{
		return min(x.x, x.y);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static uint cmin(uint3 x)
	{
		return min(min(x.x, x.y), x.z);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static uint cmin(uint4 x)
	{
		return min(min(x.x, x.y), min(x.z, x.w));
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static float cmin(float2 x)
	{
		return min(x.x, x.y);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static float cmin(float3 x)
	{
		return min(min(x.x, x.y), x.z);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static float cmin(float4 x)
	{
		return min(min(x.x, x.y), min(x.z, x.w));
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static double cmin(double2 x)
	{
		return min(x.x, x.y);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static double cmin(double3 x)
	{
		return min(min(x.x, x.y), x.z);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static double cmin(double4 x)
	{
		return min(min(x.x, x.y), min(x.z, x.w));
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static int cmax(int2 x)
	{
		return max(x.x, x.y);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static int cmax(int3 x)
	{
		return max(max(x.x, x.y), x.z);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static int cmax(int4 x)
	{
		return max(max(x.x, x.y), max(x.z, x.w));
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static uint cmax(uint2 x)
	{
		return max(x.x, x.y);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static uint cmax(uint3 x)
	{
		return max(max(x.x, x.y), x.z);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static uint cmax(uint4 x)
	{
		return max(max(x.x, x.y), max(x.z, x.w));
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static float cmax(float2 x)
	{
		return max(x.x, x.y);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static float cmax(float3 x)
	{
		return max(max(x.x, x.y), x.z);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static float cmax(float4 x)
	{
		return max(max(x.x, x.y), max(x.z, x.w));
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static double cmax(double2 x)
	{
		return max(x.x, x.y);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static double cmax(double3 x)
	{
		return max(max(x.x, x.y), x.z);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static double cmax(double4 x)
	{
		return max(max(x.x, x.y), max(x.z, x.w));
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static int csum(int2 x)
	{
		return x.x + x.y;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static int csum(int3 x)
	{
		return x.x + x.y + x.z;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static int csum(int4 x)
	{
		return x.x + x.y + x.z + x.w;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static uint csum(uint2 x)
	{
		return x.x + x.y;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static uint csum(uint3 x)
	{
		return x.x + x.y + x.z;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static uint csum(uint4 x)
	{
		return x.x + x.y + x.z + x.w;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static float csum(float2 x)
	{
		return x.x + x.y;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static float csum(float3 x)
	{
		return x.x + x.y + x.z;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static float csum(float4 x)
	{
		return x.x + x.y + (x.z + x.w);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static double csum(double2 x)
	{
		return x.x + x.y;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static double csum(double3 x)
	{
		return x.x + x.y + x.z;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static double csum(double4 x)
	{
		return x.x + x.y + (x.z + x.w);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public unsafe static int compress(int* output, int index, int4 val, bool4 mask)
	{
		if (mask.x)
		{
			output[index++] = val.x;
		}
		if (mask.y)
		{
			output[index++] = val.y;
		}
		if (mask.z)
		{
			output[index++] = val.z;
		}
		if (mask.w)
		{
			output[index++] = val.w;
		}
		return index;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public unsafe static int compress(uint* output, int index, uint4 val, bool4 mask)
	{
		return compress((int*)output, index, *(int4*)(&val), mask);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public unsafe static int compress(float* output, int index, float4 val, bool4 mask)
	{
		return compress((int*)output, index, *(int4*)(&val), mask);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static float f16tof32(uint x)
	{
		uint num = (x & 0x7FFF) << 13;
		uint num2 = num & 0xF800000;
		uint num3 = num + 939524096 + select(0u, 939524096u, num2 == 260046848);
		return asfloat(select(num3, asuint(asfloat(num3 + 8388608) - 6.1035156E-05f), num2 == 0) | ((x & 0x8000) << 16));
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static float2 f16tof32(uint2 x)
	{
		uint2 obj = (x & 32767u) << 13;
		uint2 uint5 = obj & 260046848u;
		uint2 obj2 = obj + 939524096u + select(0u, 939524096u, uint5 == 260046848u);
		return asfloat(select(obj2, asuint(asfloat(obj2 + 8388608u) - 6.1035156E-05f), uint5 == 0u) | ((x & 32768u) << 16));
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static float3 f16tof32(uint3 x)
	{
		uint3 obj = (x & 32767u) << 13;
		uint3 uint5 = obj & 260046848u;
		uint3 obj2 = obj + 939524096u + select(0u, 939524096u, uint5 == 260046848u);
		return asfloat(select(obj2, asuint(asfloat(obj2 + 8388608u) - 6.1035156E-05f), uint5 == 0u) | ((x & 32768u) << 16));
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static float4 f16tof32(uint4 x)
	{
		uint4 obj = (x & 32767u) << 13;
		uint4 uint5 = obj & 260046848u;
		uint4 obj2 = obj + 939524096u + select(0u, 939524096u, uint5 == 260046848u);
		return asfloat(select(obj2, asuint(asfloat(obj2 + 8388608u) - 6.1035156E-05f), uint5 == 0u) | ((x & 32768u) << 16));
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static uint f32tof16(float x)
	{
		uint num = asuint(x);
		uint num2 = num & 0x7FFFF000;
		return select(asuint(min(asfloat(num2) * 1.92593E-34f, 260042750f)) + 4096 >> 13, select(31744u, 32256u, (int)num2 > 2139095040), (int)num2 >= 2139095040) | ((num & 0x80000FFFu) >> 16);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static uint2 f32tof16(float2 x)
	{
		uint2 uint5 = asuint(x);
		uint2 uint6 = uint5 & 2147479552u;
		return select((uint2)(asint(min(asfloat(uint6) * 1.92593E-34f, 260042750f)) + 4096) >> 13, select(31744u, 32256u, (int2)uint6 > 2139095040), (int2)uint6 >= 2139095040) | ((uint5 & 2147487743u) >> 16);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static uint3 f32tof16(float3 x)
	{
		uint3 uint5 = asuint(x);
		uint3 uint6 = uint5 & 2147479552u;
		return select((uint3)(asint(min(asfloat(uint6) * 1.92593E-34f, 260042750f)) + 4096) >> 13, select(31744u, 32256u, (int3)uint6 > 2139095040), (int3)uint6 >= 2139095040) | ((uint5 & 2147487743u) >> 16);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static uint4 f32tof16(float4 x)
	{
		uint4 uint5 = asuint(x);
		uint4 uint6 = uint5 & 2147479552u;
		return select((uint4)(asint(min(asfloat(uint6) * 1.92593E-34f, 260042750f)) + 4096) >> 13, select(31744u, 32256u, (int4)uint6 > 2139095040), (int4)uint6 >= 2139095040) | ((uint5 & 2147487743u) >> 16);
	}

	public unsafe static uint hash(void* pBuffer, int numBytes, uint seed = 0u)
	{
		uint4* ptr = (uint4*)pBuffer;
		uint num = seed + 374761393;
		if (numBytes >= 16)
		{
			uint4 uint5 = new uint4(606290984u, 2246822519u, 0u, 1640531535u) + seed;
			int num2 = numBytes >> 4;
			for (int i = 0; i < num2; i++)
			{
				uint5 += *(ptr++) * 2246822519u;
				uint5 = (uint5 << 13) | (uint5 >> 19);
				uint5 *= 2654435761u;
			}
			num = rol(uint5.x, 1) + rol(uint5.y, 7) + rol(uint5.z, 12) + rol(uint5.w, 18);
		}
		num += (uint)numBytes;
		uint* ptr2 = (uint*)ptr;
		for (int j = 0; j < ((numBytes >> 2) & 3); j++)
		{
			num += (uint)((int)(*(ptr2++)) * -1028477379);
			num = rol(num, 17) * 668265263;
		}
		byte* ptr3 = (byte*)ptr2;
		for (int k = 0; k < (numBytes & 3); k++)
		{
			num += (uint)(*(ptr3++) * 374761393);
			num = rol(num, 11) * 2654435761u;
		}
		num ^= num >> 15;
		num *= 2246822519u;
		num ^= num >> 13;
		num *= 3266489917u;
		return num ^ (num >> 16);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static float3 up()
	{
		return new float3(0f, 1f, 0f);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static float3 down()
	{
		return new float3(0f, -1f, 0f);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static float3 forward()
	{
		return new float3(0f, 0f, 1f);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static float3 back()
	{
		return new float3(0f, 0f, -1f);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static float3 left()
	{
		return new float3(-1f, 0f, 0f);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static float3 right()
	{
		return new float3(1f, 0f, 0f);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	internal static float4 unpacklo(float4 a, float4 b)
	{
		return shuffle(a, b, ShuffleComponent.LeftX, ShuffleComponent.RightX, ShuffleComponent.LeftY, ShuffleComponent.RightY);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	internal static double4 unpacklo(double4 a, double4 b)
	{
		return shuffle(a, b, ShuffleComponent.LeftX, ShuffleComponent.RightX, ShuffleComponent.LeftY, ShuffleComponent.RightY);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	internal static float4 unpackhi(float4 a, float4 b)
	{
		return shuffle(a, b, ShuffleComponent.LeftZ, ShuffleComponent.RightZ, ShuffleComponent.LeftW, ShuffleComponent.RightW);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	internal static double4 unpackhi(double4 a, double4 b)
	{
		return shuffle(a, b, ShuffleComponent.LeftZ, ShuffleComponent.RightZ, ShuffleComponent.LeftW, ShuffleComponent.RightW);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	internal static float4 movelh(float4 a, float4 b)
	{
		return shuffle(a, b, ShuffleComponent.LeftX, ShuffleComponent.LeftY, ShuffleComponent.RightX, ShuffleComponent.RightY);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	internal static double4 movelh(double4 a, double4 b)
	{
		return shuffle(a, b, ShuffleComponent.LeftX, ShuffleComponent.LeftY, ShuffleComponent.RightX, ShuffleComponent.RightY);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	internal static float4 movehl(float4 a, float4 b)
	{
		return shuffle(b, a, ShuffleComponent.LeftZ, ShuffleComponent.LeftW, ShuffleComponent.RightZ, ShuffleComponent.RightW);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	internal static double4 movehl(double4 a, double4 b)
	{
		return shuffle(b, a, ShuffleComponent.LeftZ, ShuffleComponent.LeftW, ShuffleComponent.RightZ, ShuffleComponent.RightW);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	internal static uint fold_to_uint(double x)
	{
		LongDoubleUnion longDoubleUnion = default(LongDoubleUnion);
		longDoubleUnion.longValue = 0L;
		longDoubleUnion.doubleValue = x;
		return (uint)((int)(longDoubleUnion.longValue >> 32) ^ (int)longDoubleUnion.longValue);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	internal static uint2 fold_to_uint(double2 x)
	{
		return uint2(fold_to_uint(x.x), fold_to_uint(x.y));
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	internal static uint3 fold_to_uint(double3 x)
	{
		return uint3(fold_to_uint(x.x), fold_to_uint(x.y), fold_to_uint(x.z));
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	internal static uint4 fold_to_uint(double4 x)
	{
		return uint4(fold_to_uint(x.x), fold_to_uint(x.y), fold_to_uint(x.z), fold_to_uint(x.w));
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static float3x3 float3x3(float4x4 f4x4)
	{
		return new float3x3(f4x4);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static float3x3 float3x3(quaternion rotation)
	{
		return new float3x3(rotation);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static float4x4 float4x4(float3x3 rotation, float3 translation)
	{
		return new float4x4(rotation, translation);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static float4x4 float4x4(quaternion rotation, float3 translation)
	{
		return new float4x4(rotation, translation);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static float4x4 float4x4(RigidTransform transform)
	{
		return new float4x4(transform);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static float3x3 orthonormalize(float3x3 i)
	{
		float3 c = i.c0;
		float3 float5 = i.c1 - i.c0 * dot(i.c1, i.c0);
		float num = length(c);
		float num2 = length(float5);
		bool c2 = num > 1E-30f && num2 > 1E-30f;
		float3x3 result = default(float3x3);
		result.c0 = select(float3(1f, 0f, 0f), c / num, c2);
		result.c1 = select(float3(0f, 1f, 0f), float5 / num2, c2);
		result.c2 = cross(result.c0, result.c1);
		return result;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static float mul(float a, float b)
	{
		return a * b;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static float mul(float2 a, float2 b)
	{
		return a.x * b.x + a.y * b.y;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static float2 mul(float2 a, float2x2 b)
	{
		return float2(a.x * b.c0.x + a.y * b.c0.y, a.x * b.c1.x + a.y * b.c1.y);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static float3 mul(float2 a, float2x3 b)
	{
		return float3(a.x * b.c0.x + a.y * b.c0.y, a.x * b.c1.x + a.y * b.c1.y, a.x * b.c2.x + a.y * b.c2.y);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static float4 mul(float2 a, float2x4 b)
	{
		return float4(a.x * b.c0.x + a.y * b.c0.y, a.x * b.c1.x + a.y * b.c1.y, a.x * b.c2.x + a.y * b.c2.y, a.x * b.c3.x + a.y * b.c3.y);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static float mul(float3 a, float3 b)
	{
		return a.x * b.x + a.y * b.y + a.z * b.z;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static float2 mul(float3 a, float3x2 b)
	{
		return float2(a.x * b.c0.x + a.y * b.c0.y + a.z * b.c0.z, a.x * b.c1.x + a.y * b.c1.y + a.z * b.c1.z);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static float3 mul(float3 a, float3x3 b)
	{
		return float3(a.x * b.c0.x + a.y * b.c0.y + a.z * b.c0.z, a.x * b.c1.x + a.y * b.c1.y + a.z * b.c1.z, a.x * b.c2.x + a.y * b.c2.y + a.z * b.c2.z);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static float4 mul(float3 a, float3x4 b)
	{
		return float4(a.x * b.c0.x + a.y * b.c0.y + a.z * b.c0.z, a.x * b.c1.x + a.y * b.c1.y + a.z * b.c1.z, a.x * b.c2.x + a.y * b.c2.y + a.z * b.c2.z, a.x * b.c3.x + a.y * b.c3.y + a.z * b.c3.z);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static float mul(float4 a, float4 b)
	{
		return a.x * b.x + a.y * b.y + a.z * b.z + a.w * b.w;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static float2 mul(float4 a, float4x2 b)
	{
		return float2(a.x * b.c0.x + a.y * b.c0.y + a.z * b.c0.z + a.w * b.c0.w, a.x * b.c1.x + a.y * b.c1.y + a.z * b.c1.z + a.w * b.c1.w);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static float3 mul(float4 a, float4x3 b)
	{
		return float3(a.x * b.c0.x + a.y * b.c0.y + a.z * b.c0.z + a.w * b.c0.w, a.x * b.c1.x + a.y * b.c1.y + a.z * b.c1.z + a.w * b.c1.w, a.x * b.c2.x + a.y * b.c2.y + a.z * b.c2.z + a.w * b.c2.w);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static float4 mul(float4 a, float4x4 b)
	{
		return float4(a.x * b.c0.x + a.y * b.c0.y + a.z * b.c0.z + a.w * b.c0.w, a.x * b.c1.x + a.y * b.c1.y + a.z * b.c1.z + a.w * b.c1.w, a.x * b.c2.x + a.y * b.c2.y + a.z * b.c2.z + a.w * b.c2.w, a.x * b.c3.x + a.y * b.c3.y + a.z * b.c3.z + a.w * b.c3.w);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static float2 mul(float2x2 a, float2 b)
	{
		return a.c0 * b.x + a.c1 * b.y;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static float2x2 mul(float2x2 a, float2x2 b)
	{
		return float2x2(a.c0 * b.c0.x + a.c1 * b.c0.y, a.c0 * b.c1.x + a.c1 * b.c1.y);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static float2x3 mul(float2x2 a, float2x3 b)
	{
		return float2x3(a.c0 * b.c0.x + a.c1 * b.c0.y, a.c0 * b.c1.x + a.c1 * b.c1.y, a.c0 * b.c2.x + a.c1 * b.c2.y);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static float2x4 mul(float2x2 a, float2x4 b)
	{
		return float2x4(a.c0 * b.c0.x + a.c1 * b.c0.y, a.c0 * b.c1.x + a.c1 * b.c1.y, a.c0 * b.c2.x + a.c1 * b.c2.y, a.c0 * b.c3.x + a.c1 * b.c3.y);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static float2 mul(float2x3 a, float3 b)
	{
		return a.c0 * b.x + a.c1 * b.y + a.c2 * b.z;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static float2x2 mul(float2x3 a, float3x2 b)
	{
		return float2x2(a.c0 * b.c0.x + a.c1 * b.c0.y + a.c2 * b.c0.z, a.c0 * b.c1.x + a.c1 * b.c1.y + a.c2 * b.c1.z);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static float2x3 mul(float2x3 a, float3x3 b)
	{
		return float2x3(a.c0 * b.c0.x + a.c1 * b.c0.y + a.c2 * b.c0.z, a.c0 * b.c1.x + a.c1 * b.c1.y + a.c2 * b.c1.z, a.c0 * b.c2.x + a.c1 * b.c2.y + a.c2 * b.c2.z);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static float2x4 mul(float2x3 a, float3x4 b)
	{
		return float2x4(a.c0 * b.c0.x + a.c1 * b.c0.y + a.c2 * b.c0.z, a.c0 * b.c1.x + a.c1 * b.c1.y + a.c2 * b.c1.z, a.c0 * b.c2.x + a.c1 * b.c2.y + a.c2 * b.c2.z, a.c0 * b.c3.x + a.c1 * b.c3.y + a.c2 * b.c3.z);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static float2 mul(float2x4 a, float4 b)
	{
		return a.c0 * b.x + a.c1 * b.y + a.c2 * b.z + a.c3 * b.w;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static float2x2 mul(float2x4 a, float4x2 b)
	{
		return float2x2(a.c0 * b.c0.x + a.c1 * b.c0.y + a.c2 * b.c0.z + a.c3 * b.c0.w, a.c0 * b.c1.x + a.c1 * b.c1.y + a.c2 * b.c1.z + a.c3 * b.c1.w);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static float2x3 mul(float2x4 a, float4x3 b)
	{
		return float2x3(a.c0 * b.c0.x + a.c1 * b.c0.y + a.c2 * b.c0.z + a.c3 * b.c0.w, a.c0 * b.c1.x + a.c1 * b.c1.y + a.c2 * b.c1.z + a.c3 * b.c1.w, a.c0 * b.c2.x + a.c1 * b.c2.y + a.c2 * b.c2.z + a.c3 * b.c2.w);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static float2x4 mul(float2x4 a, float4x4 b)
	{
		return float2x4(a.c0 * b.c0.x + a.c1 * b.c0.y + a.c2 * b.c0.z + a.c3 * b.c0.w, a.c0 * b.c1.x + a.c1 * b.c1.y + a.c2 * b.c1.z + a.c3 * b.c1.w, a.c0 * b.c2.x + a.c1 * b.c2.y + a.c2 * b.c2.z + a.c3 * b.c2.w, a.c0 * b.c3.x + a.c1 * b.c3.y + a.c2 * b.c3.z + a.c3 * b.c3.w);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static float3 mul(float3x2 a, float2 b)
	{
		return a.c0 * b.x + a.c1 * b.y;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static float3x2 mul(float3x2 a, float2x2 b)
	{
		return float3x2(a.c0 * b.c0.x + a.c1 * b.c0.y, a.c0 * b.c1.x + a.c1 * b.c1.y);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static float3x3 mul(float3x2 a, float2x3 b)
	{
		return float3x3(a.c0 * b.c0.x + a.c1 * b.c0.y, a.c0 * b.c1.x + a.c1 * b.c1.y, a.c0 * b.c2.x + a.c1 * b.c2.y);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static float3x4 mul(float3x2 a, float2x4 b)
	{
		return float3x4(a.c0 * b.c0.x + a.c1 * b.c0.y, a.c0 * b.c1.x + a.c1 * b.c1.y, a.c0 * b.c2.x + a.c1 * b.c2.y, a.c0 * b.c3.x + a.c1 * b.c3.y);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static float3 mul(float3x3 a, float3 b)
	{
		return a.c0 * b.x + a.c1 * b.y + a.c2 * b.z;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static float3x2 mul(float3x3 a, float3x2 b)
	{
		return float3x2(a.c0 * b.c0.x + a.c1 * b.c0.y + a.c2 * b.c0.z, a.c0 * b.c1.x + a.c1 * b.c1.y + a.c2 * b.c1.z);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static float3x3 mul(float3x3 a, float3x3 b)
	{
		return float3x3(a.c0 * b.c0.x + a.c1 * b.c0.y + a.c2 * b.c0.z, a.c0 * b.c1.x + a.c1 * b.c1.y + a.c2 * b.c1.z, a.c0 * b.c2.x + a.c1 * b.c2.y + a.c2 * b.c2.z);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static float3x4 mul(float3x3 a, float3x4 b)
	{
		return float3x4(a.c0 * b.c0.x + a.c1 * b.c0.y + a.c2 * b.c0.z, a.c0 * b.c1.x + a.c1 * b.c1.y + a.c2 * b.c1.z, a.c0 * b.c2.x + a.c1 * b.c2.y + a.c2 * b.c2.z, a.c0 * b.c3.x + a.c1 * b.c3.y + a.c2 * b.c3.z);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static float3 mul(float3x4 a, float4 b)
	{
		return a.c0 * b.x + a.c1 * b.y + a.c2 * b.z + a.c3 * b.w;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static float3x2 mul(float3x4 a, float4x2 b)
	{
		return float3x2(a.c0 * b.c0.x + a.c1 * b.c0.y + a.c2 * b.c0.z + a.c3 * b.c0.w, a.c0 * b.c1.x + a.c1 * b.c1.y + a.c2 * b.c1.z + a.c3 * b.c1.w);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static float3x3 mul(float3x4 a, float4x3 b)
	{
		return float3x3(a.c0 * b.c0.x + a.c1 * b.c0.y + a.c2 * b.c0.z + a.c3 * b.c0.w, a.c0 * b.c1.x + a.c1 * b.c1.y + a.c2 * b.c1.z + a.c3 * b.c1.w, a.c0 * b.c2.x + a.c1 * b.c2.y + a.c2 * b.c2.z + a.c3 * b.c2.w);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static float3x4 mul(float3x4 a, float4x4 b)
	{
		return float3x4(a.c0 * b.c0.x + a.c1 * b.c0.y + a.c2 * b.c0.z + a.c3 * b.c0.w, a.c0 * b.c1.x + a.c1 * b.c1.y + a.c2 * b.c1.z + a.c3 * b.c1.w, a.c0 * b.c2.x + a.c1 * b.c2.y + a.c2 * b.c2.z + a.c3 * b.c2.w, a.c0 * b.c3.x + a.c1 * b.c3.y + a.c2 * b.c3.z + a.c3 * b.c3.w);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static float4 mul(float4x2 a, float2 b)
	{
		return a.c0 * b.x + a.c1 * b.y;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static float4x2 mul(float4x2 a, float2x2 b)
	{
		return float4x2(a.c0 * b.c0.x + a.c1 * b.c0.y, a.c0 * b.c1.x + a.c1 * b.c1.y);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static float4x3 mul(float4x2 a, float2x3 b)
	{
		return float4x3(a.c0 * b.c0.x + a.c1 * b.c0.y, a.c0 * b.c1.x + a.c1 * b.c1.y, a.c0 * b.c2.x + a.c1 * b.c2.y);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static float4x4 mul(float4x2 a, float2x4 b)
	{
		return float4x4(a.c0 * b.c0.x + a.c1 * b.c0.y, a.c0 * b.c1.x + a.c1 * b.c1.y, a.c0 * b.c2.x + a.c1 * b.c2.y, a.c0 * b.c3.x + a.c1 * b.c3.y);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static float4 mul(float4x3 a, float3 b)
	{
		return a.c0 * b.x + a.c1 * b.y + a.c2 * b.z;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static float4x2 mul(float4x3 a, float3x2 b)
	{
		return float4x2(a.c0 * b.c0.x + a.c1 * b.c0.y + a.c2 * b.c0.z, a.c0 * b.c1.x + a.c1 * b.c1.y + a.c2 * b.c1.z);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static float4x3 mul(float4x3 a, float3x3 b)
	{
		return float4x3(a.c0 * b.c0.x + a.c1 * b.c0.y + a.c2 * b.c0.z, a.c0 * b.c1.x + a.c1 * b.c1.y + a.c2 * b.c1.z, a.c0 * b.c2.x + a.c1 * b.c2.y + a.c2 * b.c2.z);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static float4x4 mul(float4x3 a, float3x4 b)
	{
		return float4x4(a.c0 * b.c0.x + a.c1 * b.c0.y + a.c2 * b.c0.z, a.c0 * b.c1.x + a.c1 * b.c1.y + a.c2 * b.c1.z, a.c0 * b.c2.x + a.c1 * b.c2.y + a.c2 * b.c2.z, a.c0 * b.c3.x + a.c1 * b.c3.y + a.c2 * b.c3.z);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static float4 mul(float4x4 a, float4 b)
	{
		return a.c0 * b.x + a.c1 * b.y + a.c2 * b.z + a.c3 * b.w;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static float4x2 mul(float4x4 a, float4x2 b)
	{
		return float4x2(a.c0 * b.c0.x + a.c1 * b.c0.y + a.c2 * b.c0.z + a.c3 * b.c0.w, a.c0 * b.c1.x + a.c1 * b.c1.y + a.c2 * b.c1.z + a.c3 * b.c1.w);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static float4x3 mul(float4x4 a, float4x3 b)
	{
		return float4x3(a.c0 * b.c0.x + a.c1 * b.c0.y + a.c2 * b.c0.z + a.c3 * b.c0.w, a.c0 * b.c1.x + a.c1 * b.c1.y + a.c2 * b.c1.z + a.c3 * b.c1.w, a.c0 * b.c2.x + a.c1 * b.c2.y + a.c2 * b.c2.z + a.c3 * b.c2.w);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static float4x4 mul(float4x4 a, float4x4 b)
	{
		return float4x4(a.c0 * b.c0.x + a.c1 * b.c0.y + a.c2 * b.c0.z + a.c3 * b.c0.w, a.c0 * b.c1.x + a.c1 * b.c1.y + a.c2 * b.c1.z + a.c3 * b.c1.w, a.c0 * b.c2.x + a.c1 * b.c2.y + a.c2 * b.c2.z + a.c3 * b.c2.w, a.c0 * b.c3.x + a.c1 * b.c3.y + a.c2 * b.c3.z + a.c3 * b.c3.w);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static double mul(double a, double b)
	{
		return a * b;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static double mul(double2 a, double2 b)
	{
		return a.x * b.x + a.y * b.y;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static double2 mul(double2 a, double2x2 b)
	{
		return double2(a.x * b.c0.x + a.y * b.c0.y, a.x * b.c1.x + a.y * b.c1.y);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static double3 mul(double2 a, double2x3 b)
	{
		return double3(a.x * b.c0.x + a.y * b.c0.y, a.x * b.c1.x + a.y * b.c1.y, a.x * b.c2.x + a.y * b.c2.y);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static double4 mul(double2 a, double2x4 b)
	{
		return double4(a.x * b.c0.x + a.y * b.c0.y, a.x * b.c1.x + a.y * b.c1.y, a.x * b.c2.x + a.y * b.c2.y, a.x * b.c3.x + a.y * b.c3.y);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static double mul(double3 a, double3 b)
	{
		return a.x * b.x + a.y * b.y + a.z * b.z;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static double2 mul(double3 a, double3x2 b)
	{
		return double2(a.x * b.c0.x + a.y * b.c0.y + a.z * b.c0.z, a.x * b.c1.x + a.y * b.c1.y + a.z * b.c1.z);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static double3 mul(double3 a, double3x3 b)
	{
		return double3(a.x * b.c0.x + a.y * b.c0.y + a.z * b.c0.z, a.x * b.c1.x + a.y * b.c1.y + a.z * b.c1.z, a.x * b.c2.x + a.y * b.c2.y + a.z * b.c2.z);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static double4 mul(double3 a, double3x4 b)
	{
		return double4(a.x * b.c0.x + a.y * b.c0.y + a.z * b.c0.z, a.x * b.c1.x + a.y * b.c1.y + a.z * b.c1.z, a.x * b.c2.x + a.y * b.c2.y + a.z * b.c2.z, a.x * b.c3.x + a.y * b.c3.y + a.z * b.c3.z);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static double mul(double4 a, double4 b)
	{
		return a.x * b.x + a.y * b.y + a.z * b.z + a.w * b.w;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static double2 mul(double4 a, double4x2 b)
	{
		return double2(a.x * b.c0.x + a.y * b.c0.y + a.z * b.c0.z + a.w * b.c0.w, a.x * b.c1.x + a.y * b.c1.y + a.z * b.c1.z + a.w * b.c1.w);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static double3 mul(double4 a, double4x3 b)
	{
		return double3(a.x * b.c0.x + a.y * b.c0.y + a.z * b.c0.z + a.w * b.c0.w, a.x * b.c1.x + a.y * b.c1.y + a.z * b.c1.z + a.w * b.c1.w, a.x * b.c2.x + a.y * b.c2.y + a.z * b.c2.z + a.w * b.c2.w);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static double4 mul(double4 a, double4x4 b)
	{
		return double4(a.x * b.c0.x + a.y * b.c0.y + a.z * b.c0.z + a.w * b.c0.w, a.x * b.c1.x + a.y * b.c1.y + a.z * b.c1.z + a.w * b.c1.w, a.x * b.c2.x + a.y * b.c2.y + a.z * b.c2.z + a.w * b.c2.w, a.x * b.c3.x + a.y * b.c3.y + a.z * b.c3.z + a.w * b.c3.w);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static double2 mul(double2x2 a, double2 b)
	{
		return a.c0 * b.x + a.c1 * b.y;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static double2x2 mul(double2x2 a, double2x2 b)
	{
		return double2x2(a.c0 * b.c0.x + a.c1 * b.c0.y, a.c0 * b.c1.x + a.c1 * b.c1.y);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static double2x3 mul(double2x2 a, double2x3 b)
	{
		return double2x3(a.c0 * b.c0.x + a.c1 * b.c0.y, a.c0 * b.c1.x + a.c1 * b.c1.y, a.c0 * b.c2.x + a.c1 * b.c2.y);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static double2x4 mul(double2x2 a, double2x4 b)
	{
		return double2x4(a.c0 * b.c0.x + a.c1 * b.c0.y, a.c0 * b.c1.x + a.c1 * b.c1.y, a.c0 * b.c2.x + a.c1 * b.c2.y, a.c0 * b.c3.x + a.c1 * b.c3.y);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static double2 mul(double2x3 a, double3 b)
	{
		return a.c0 * b.x + a.c1 * b.y + a.c2 * b.z;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static double2x2 mul(double2x3 a, double3x2 b)
	{
		return double2x2(a.c0 * b.c0.x + a.c1 * b.c0.y + a.c2 * b.c0.z, a.c0 * b.c1.x + a.c1 * b.c1.y + a.c2 * b.c1.z);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static double2x3 mul(double2x3 a, double3x3 b)
	{
		return double2x3(a.c0 * b.c0.x + a.c1 * b.c0.y + a.c2 * b.c0.z, a.c0 * b.c1.x + a.c1 * b.c1.y + a.c2 * b.c1.z, a.c0 * b.c2.x + a.c1 * b.c2.y + a.c2 * b.c2.z);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static double2x4 mul(double2x3 a, double3x4 b)
	{
		return double2x4(a.c0 * b.c0.x + a.c1 * b.c0.y + a.c2 * b.c0.z, a.c0 * b.c1.x + a.c1 * b.c1.y + a.c2 * b.c1.z, a.c0 * b.c2.x + a.c1 * b.c2.y + a.c2 * b.c2.z, a.c0 * b.c3.x + a.c1 * b.c3.y + a.c2 * b.c3.z);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static double2 mul(double2x4 a, double4 b)
	{
		return a.c0 * b.x + a.c1 * b.y + a.c2 * b.z + a.c3 * b.w;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static double2x2 mul(double2x4 a, double4x2 b)
	{
		return double2x2(a.c0 * b.c0.x + a.c1 * b.c0.y + a.c2 * b.c0.z + a.c3 * b.c0.w, a.c0 * b.c1.x + a.c1 * b.c1.y + a.c2 * b.c1.z + a.c3 * b.c1.w);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static double2x3 mul(double2x4 a, double4x3 b)
	{
		return double2x3(a.c0 * b.c0.x + a.c1 * b.c0.y + a.c2 * b.c0.z + a.c3 * b.c0.w, a.c0 * b.c1.x + a.c1 * b.c1.y + a.c2 * b.c1.z + a.c3 * b.c1.w, a.c0 * b.c2.x + a.c1 * b.c2.y + a.c2 * b.c2.z + a.c3 * b.c2.w);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static double2x4 mul(double2x4 a, double4x4 b)
	{
		return double2x4(a.c0 * b.c0.x + a.c1 * b.c0.y + a.c2 * b.c0.z + a.c3 * b.c0.w, a.c0 * b.c1.x + a.c1 * b.c1.y + a.c2 * b.c1.z + a.c3 * b.c1.w, a.c0 * b.c2.x + a.c1 * b.c2.y + a.c2 * b.c2.z + a.c3 * b.c2.w, a.c0 * b.c3.x + a.c1 * b.c3.y + a.c2 * b.c3.z + a.c3 * b.c3.w);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static double3 mul(double3x2 a, double2 b)
	{
		return a.c0 * b.x + a.c1 * b.y;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static double3x2 mul(double3x2 a, double2x2 b)
	{
		return double3x2(a.c0 * b.c0.x + a.c1 * b.c0.y, a.c0 * b.c1.x + a.c1 * b.c1.y);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static double3x3 mul(double3x2 a, double2x3 b)
	{
		return double3x3(a.c0 * b.c0.x + a.c1 * b.c0.y, a.c0 * b.c1.x + a.c1 * b.c1.y, a.c0 * b.c2.x + a.c1 * b.c2.y);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static double3x4 mul(double3x2 a, double2x4 b)
	{
		return double3x4(a.c0 * b.c0.x + a.c1 * b.c0.y, a.c0 * b.c1.x + a.c1 * b.c1.y, a.c0 * b.c2.x + a.c1 * b.c2.y, a.c0 * b.c3.x + a.c1 * b.c3.y);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static double3 mul(double3x3 a, double3 b)
	{
		return a.c0 * b.x + a.c1 * b.y + a.c2 * b.z;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static double3x2 mul(double3x3 a, double3x2 b)
	{
		return double3x2(a.c0 * b.c0.x + a.c1 * b.c0.y + a.c2 * b.c0.z, a.c0 * b.c1.x + a.c1 * b.c1.y + a.c2 * b.c1.z);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static double3x3 mul(double3x3 a, double3x3 b)
	{
		return double3x3(a.c0 * b.c0.x + a.c1 * b.c0.y + a.c2 * b.c0.z, a.c0 * b.c1.x + a.c1 * b.c1.y + a.c2 * b.c1.z, a.c0 * b.c2.x + a.c1 * b.c2.y + a.c2 * b.c2.z);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static double3x4 mul(double3x3 a, double3x4 b)
	{
		return double3x4(a.c0 * b.c0.x + a.c1 * b.c0.y + a.c2 * b.c0.z, a.c0 * b.c1.x + a.c1 * b.c1.y + a.c2 * b.c1.z, a.c0 * b.c2.x + a.c1 * b.c2.y + a.c2 * b.c2.z, a.c0 * b.c3.x + a.c1 * b.c3.y + a.c2 * b.c3.z);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static double3 mul(double3x4 a, double4 b)
	{
		return a.c0 * b.x + a.c1 * b.y + a.c2 * b.z + a.c3 * b.w;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static double3x2 mul(double3x4 a, double4x2 b)
	{
		return double3x2(a.c0 * b.c0.x + a.c1 * b.c0.y + a.c2 * b.c0.z + a.c3 * b.c0.w, a.c0 * b.c1.x + a.c1 * b.c1.y + a.c2 * b.c1.z + a.c3 * b.c1.w);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static double3x3 mul(double3x4 a, double4x3 b)
	{
		return double3x3(a.c0 * b.c0.x + a.c1 * b.c0.y + a.c2 * b.c0.z + a.c3 * b.c0.w, a.c0 * b.c1.x + a.c1 * b.c1.y + a.c2 * b.c1.z + a.c3 * b.c1.w, a.c0 * b.c2.x + a.c1 * b.c2.y + a.c2 * b.c2.z + a.c3 * b.c2.w);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static double3x4 mul(double3x4 a, double4x4 b)
	{
		return double3x4(a.c0 * b.c0.x + a.c1 * b.c0.y + a.c2 * b.c0.z + a.c3 * b.c0.w, a.c0 * b.c1.x + a.c1 * b.c1.y + a.c2 * b.c1.z + a.c3 * b.c1.w, a.c0 * b.c2.x + a.c1 * b.c2.y + a.c2 * b.c2.z + a.c3 * b.c2.w, a.c0 * b.c3.x + a.c1 * b.c3.y + a.c2 * b.c3.z + a.c3 * b.c3.w);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static double4 mul(double4x2 a, double2 b)
	{
		return a.c0 * b.x + a.c1 * b.y;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static double4x2 mul(double4x2 a, double2x2 b)
	{
		return double4x2(a.c0 * b.c0.x + a.c1 * b.c0.y, a.c0 * b.c1.x + a.c1 * b.c1.y);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static double4x3 mul(double4x2 a, double2x3 b)
	{
		return double4x3(a.c0 * b.c0.x + a.c1 * b.c0.y, a.c0 * b.c1.x + a.c1 * b.c1.y, a.c0 * b.c2.x + a.c1 * b.c2.y);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static double4x4 mul(double4x2 a, double2x4 b)
	{
		return double4x4(a.c0 * b.c0.x + a.c1 * b.c0.y, a.c0 * b.c1.x + a.c1 * b.c1.y, a.c0 * b.c2.x + a.c1 * b.c2.y, a.c0 * b.c3.x + a.c1 * b.c3.y);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static double4 mul(double4x3 a, double3 b)
	{
		return a.c0 * b.x + a.c1 * b.y + a.c2 * b.z;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static double4x2 mul(double4x3 a, double3x2 b)
	{
		return double4x2(a.c0 * b.c0.x + a.c1 * b.c0.y + a.c2 * b.c0.z, a.c0 * b.c1.x + a.c1 * b.c1.y + a.c2 * b.c1.z);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static double4x3 mul(double4x3 a, double3x3 b)
	{
		return double4x3(a.c0 * b.c0.x + a.c1 * b.c0.y + a.c2 * b.c0.z, a.c0 * b.c1.x + a.c1 * b.c1.y + a.c2 * b.c1.z, a.c0 * b.c2.x + a.c1 * b.c2.y + a.c2 * b.c2.z);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static double4x4 mul(double4x3 a, double3x4 b)
	{
		return double4x4(a.c0 * b.c0.x + a.c1 * b.c0.y + a.c2 * b.c0.z, a.c0 * b.c1.x + a.c1 * b.c1.y + a.c2 * b.c1.z, a.c0 * b.c2.x + a.c1 * b.c2.y + a.c2 * b.c2.z, a.c0 * b.c3.x + a.c1 * b.c3.y + a.c2 * b.c3.z);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static double4 mul(double4x4 a, double4 b)
	{
		return a.c0 * b.x + a.c1 * b.y + a.c2 * b.z + a.c3 * b.w;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static double4x2 mul(double4x4 a, double4x2 b)
	{
		return double4x2(a.c0 * b.c0.x + a.c1 * b.c0.y + a.c2 * b.c0.z + a.c3 * b.c0.w, a.c0 * b.c1.x + a.c1 * b.c1.y + a.c2 * b.c1.z + a.c3 * b.c1.w);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static double4x3 mul(double4x4 a, double4x3 b)
	{
		return double4x3(a.c0 * b.c0.x + a.c1 * b.c0.y + a.c2 * b.c0.z + a.c3 * b.c0.w, a.c0 * b.c1.x + a.c1 * b.c1.y + a.c2 * b.c1.z + a.c3 * b.c1.w, a.c0 * b.c2.x + a.c1 * b.c2.y + a.c2 * b.c2.z + a.c3 * b.c2.w);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static double4x4 mul(double4x4 a, double4x4 b)
	{
		return double4x4(a.c0 * b.c0.x + a.c1 * b.c0.y + a.c2 * b.c0.z + a.c3 * b.c0.w, a.c0 * b.c1.x + a.c1 * b.c1.y + a.c2 * b.c1.z + a.c3 * b.c1.w, a.c0 * b.c2.x + a.c1 * b.c2.y + a.c2 * b.c2.z + a.c3 * b.c2.w, a.c0 * b.c3.x + a.c1 * b.c3.y + a.c2 * b.c3.z + a.c3 * b.c3.w);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static int mul(int a, int b)
	{
		return a * b;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static int mul(int2 a, int2 b)
	{
		return a.x * b.x + a.y * b.y;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static int2 mul(int2 a, int2x2 b)
	{
		return int2(a.x * b.c0.x + a.y * b.c0.y, a.x * b.c1.x + a.y * b.c1.y);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static int3 mul(int2 a, int2x3 b)
	{
		return int3(a.x * b.c0.x + a.y * b.c0.y, a.x * b.c1.x + a.y * b.c1.y, a.x * b.c2.x + a.y * b.c2.y);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static int4 mul(int2 a, int2x4 b)
	{
		return int4(a.x * b.c0.x + a.y * b.c0.y, a.x * b.c1.x + a.y * b.c1.y, a.x * b.c2.x + a.y * b.c2.y, a.x * b.c3.x + a.y * b.c3.y);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static int mul(int3 a, int3 b)
	{
		return a.x * b.x + a.y * b.y + a.z * b.z;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static int2 mul(int3 a, int3x2 b)
	{
		return int2(a.x * b.c0.x + a.y * b.c0.y + a.z * b.c0.z, a.x * b.c1.x + a.y * b.c1.y + a.z * b.c1.z);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static int3 mul(int3 a, int3x3 b)
	{
		return int3(a.x * b.c0.x + a.y * b.c0.y + a.z * b.c0.z, a.x * b.c1.x + a.y * b.c1.y + a.z * b.c1.z, a.x * b.c2.x + a.y * b.c2.y + a.z * b.c2.z);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static int4 mul(int3 a, int3x4 b)
	{
		return int4(a.x * b.c0.x + a.y * b.c0.y + a.z * b.c0.z, a.x * b.c1.x + a.y * b.c1.y + a.z * b.c1.z, a.x * b.c2.x + a.y * b.c2.y + a.z * b.c2.z, a.x * b.c3.x + a.y * b.c3.y + a.z * b.c3.z);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static int mul(int4 a, int4 b)
	{
		return a.x * b.x + a.y * b.y + a.z * b.z + a.w * b.w;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static int2 mul(int4 a, int4x2 b)
	{
		return int2(a.x * b.c0.x + a.y * b.c0.y + a.z * b.c0.z + a.w * b.c0.w, a.x * b.c1.x + a.y * b.c1.y + a.z * b.c1.z + a.w * b.c1.w);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static int3 mul(int4 a, int4x3 b)
	{
		return int3(a.x * b.c0.x + a.y * b.c0.y + a.z * b.c0.z + a.w * b.c0.w, a.x * b.c1.x + a.y * b.c1.y + a.z * b.c1.z + a.w * b.c1.w, a.x * b.c2.x + a.y * b.c2.y + a.z * b.c2.z + a.w * b.c2.w);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static int4 mul(int4 a, int4x4 b)
	{
		return int4(a.x * b.c0.x + a.y * b.c0.y + a.z * b.c0.z + a.w * b.c0.w, a.x * b.c1.x + a.y * b.c1.y + a.z * b.c1.z + a.w * b.c1.w, a.x * b.c2.x + a.y * b.c2.y + a.z * b.c2.z + a.w * b.c2.w, a.x * b.c3.x + a.y * b.c3.y + a.z * b.c3.z + a.w * b.c3.w);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static int2 mul(int2x2 a, int2 b)
	{
		return a.c0 * b.x + a.c1 * b.y;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static int2x2 mul(int2x2 a, int2x2 b)
	{
		return int2x2(a.c0 * b.c0.x + a.c1 * b.c0.y, a.c0 * b.c1.x + a.c1 * b.c1.y);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static int2x3 mul(int2x2 a, int2x3 b)
	{
		return int2x3(a.c0 * b.c0.x + a.c1 * b.c0.y, a.c0 * b.c1.x + a.c1 * b.c1.y, a.c0 * b.c2.x + a.c1 * b.c2.y);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static int2x4 mul(int2x2 a, int2x4 b)
	{
		return int2x4(a.c0 * b.c0.x + a.c1 * b.c0.y, a.c0 * b.c1.x + a.c1 * b.c1.y, a.c0 * b.c2.x + a.c1 * b.c2.y, a.c0 * b.c3.x + a.c1 * b.c3.y);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static int2 mul(int2x3 a, int3 b)
	{
		return a.c0 * b.x + a.c1 * b.y + a.c2 * b.z;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static int2x2 mul(int2x3 a, int3x2 b)
	{
		return int2x2(a.c0 * b.c0.x + a.c1 * b.c0.y + a.c2 * b.c0.z, a.c0 * b.c1.x + a.c1 * b.c1.y + a.c2 * b.c1.z);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static int2x3 mul(int2x3 a, int3x3 b)
	{
		return int2x3(a.c0 * b.c0.x + a.c1 * b.c0.y + a.c2 * b.c0.z, a.c0 * b.c1.x + a.c1 * b.c1.y + a.c2 * b.c1.z, a.c0 * b.c2.x + a.c1 * b.c2.y + a.c2 * b.c2.z);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static int2x4 mul(int2x3 a, int3x4 b)
	{
		return int2x4(a.c0 * b.c0.x + a.c1 * b.c0.y + a.c2 * b.c0.z, a.c0 * b.c1.x + a.c1 * b.c1.y + a.c2 * b.c1.z, a.c0 * b.c2.x + a.c1 * b.c2.y + a.c2 * b.c2.z, a.c0 * b.c3.x + a.c1 * b.c3.y + a.c2 * b.c3.z);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static int2 mul(int2x4 a, int4 b)
	{
		return a.c0 * b.x + a.c1 * b.y + a.c2 * b.z + a.c3 * b.w;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static int2x2 mul(int2x4 a, int4x2 b)
	{
		return int2x2(a.c0 * b.c0.x + a.c1 * b.c0.y + a.c2 * b.c0.z + a.c3 * b.c0.w, a.c0 * b.c1.x + a.c1 * b.c1.y + a.c2 * b.c1.z + a.c3 * b.c1.w);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static int2x3 mul(int2x4 a, int4x3 b)
	{
		return int2x3(a.c0 * b.c0.x + a.c1 * b.c0.y + a.c2 * b.c0.z + a.c3 * b.c0.w, a.c0 * b.c1.x + a.c1 * b.c1.y + a.c2 * b.c1.z + a.c3 * b.c1.w, a.c0 * b.c2.x + a.c1 * b.c2.y + a.c2 * b.c2.z + a.c3 * b.c2.w);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static int2x4 mul(int2x4 a, int4x4 b)
	{
		return int2x4(a.c0 * b.c0.x + a.c1 * b.c0.y + a.c2 * b.c0.z + a.c3 * b.c0.w, a.c0 * b.c1.x + a.c1 * b.c1.y + a.c2 * b.c1.z + a.c3 * b.c1.w, a.c0 * b.c2.x + a.c1 * b.c2.y + a.c2 * b.c2.z + a.c3 * b.c2.w, a.c0 * b.c3.x + a.c1 * b.c3.y + a.c2 * b.c3.z + a.c3 * b.c3.w);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static int3 mul(int3x2 a, int2 b)
	{
		return a.c0 * b.x + a.c1 * b.y;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static int3x2 mul(int3x2 a, int2x2 b)
	{
		return int3x2(a.c0 * b.c0.x + a.c1 * b.c0.y, a.c0 * b.c1.x + a.c1 * b.c1.y);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static int3x3 mul(int3x2 a, int2x3 b)
	{
		return int3x3(a.c0 * b.c0.x + a.c1 * b.c0.y, a.c0 * b.c1.x + a.c1 * b.c1.y, a.c0 * b.c2.x + a.c1 * b.c2.y);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static int3x4 mul(int3x2 a, int2x4 b)
	{
		return int3x4(a.c0 * b.c0.x + a.c1 * b.c0.y, a.c0 * b.c1.x + a.c1 * b.c1.y, a.c0 * b.c2.x + a.c1 * b.c2.y, a.c0 * b.c3.x + a.c1 * b.c3.y);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static int3 mul(int3x3 a, int3 b)
	{
		return a.c0 * b.x + a.c1 * b.y + a.c2 * b.z;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static int3x2 mul(int3x3 a, int3x2 b)
	{
		return int3x2(a.c0 * b.c0.x + a.c1 * b.c0.y + a.c2 * b.c0.z, a.c0 * b.c1.x + a.c1 * b.c1.y + a.c2 * b.c1.z);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static int3x3 mul(int3x3 a, int3x3 b)
	{
		return int3x3(a.c0 * b.c0.x + a.c1 * b.c0.y + a.c2 * b.c0.z, a.c0 * b.c1.x + a.c1 * b.c1.y + a.c2 * b.c1.z, a.c0 * b.c2.x + a.c1 * b.c2.y + a.c2 * b.c2.z);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static int3x4 mul(int3x3 a, int3x4 b)
	{
		return int3x4(a.c0 * b.c0.x + a.c1 * b.c0.y + a.c2 * b.c0.z, a.c0 * b.c1.x + a.c1 * b.c1.y + a.c2 * b.c1.z, a.c0 * b.c2.x + a.c1 * b.c2.y + a.c2 * b.c2.z, a.c0 * b.c3.x + a.c1 * b.c3.y + a.c2 * b.c3.z);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static int3 mul(int3x4 a, int4 b)
	{
		return a.c0 * b.x + a.c1 * b.y + a.c2 * b.z + a.c3 * b.w;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static int3x2 mul(int3x4 a, int4x2 b)
	{
		return int3x2(a.c0 * b.c0.x + a.c1 * b.c0.y + a.c2 * b.c0.z + a.c3 * b.c0.w, a.c0 * b.c1.x + a.c1 * b.c1.y + a.c2 * b.c1.z + a.c3 * b.c1.w);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static int3x3 mul(int3x4 a, int4x3 b)
	{
		return int3x3(a.c0 * b.c0.x + a.c1 * b.c0.y + a.c2 * b.c0.z + a.c3 * b.c0.w, a.c0 * b.c1.x + a.c1 * b.c1.y + a.c2 * b.c1.z + a.c3 * b.c1.w, a.c0 * b.c2.x + a.c1 * b.c2.y + a.c2 * b.c2.z + a.c3 * b.c2.w);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static int3x4 mul(int3x4 a, int4x4 b)
	{
		return int3x4(a.c0 * b.c0.x + a.c1 * b.c0.y + a.c2 * b.c0.z + a.c3 * b.c0.w, a.c0 * b.c1.x + a.c1 * b.c1.y + a.c2 * b.c1.z + a.c3 * b.c1.w, a.c0 * b.c2.x + a.c1 * b.c2.y + a.c2 * b.c2.z + a.c3 * b.c2.w, a.c0 * b.c3.x + a.c1 * b.c3.y + a.c2 * b.c3.z + a.c3 * b.c3.w);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static int4 mul(int4x2 a, int2 b)
	{
		return a.c0 * b.x + a.c1 * b.y;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static int4x2 mul(int4x2 a, int2x2 b)
	{
		return int4x2(a.c0 * b.c0.x + a.c1 * b.c0.y, a.c0 * b.c1.x + a.c1 * b.c1.y);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static int4x3 mul(int4x2 a, int2x3 b)
	{
		return int4x3(a.c0 * b.c0.x + a.c1 * b.c0.y, a.c0 * b.c1.x + a.c1 * b.c1.y, a.c0 * b.c2.x + a.c1 * b.c2.y);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static int4x4 mul(int4x2 a, int2x4 b)
	{
		return int4x4(a.c0 * b.c0.x + a.c1 * b.c0.y, a.c0 * b.c1.x + a.c1 * b.c1.y, a.c0 * b.c2.x + a.c1 * b.c2.y, a.c0 * b.c3.x + a.c1 * b.c3.y);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static int4 mul(int4x3 a, int3 b)
	{
		return a.c0 * b.x + a.c1 * b.y + a.c2 * b.z;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static int4x2 mul(int4x3 a, int3x2 b)
	{
		return int4x2(a.c0 * b.c0.x + a.c1 * b.c0.y + a.c2 * b.c0.z, a.c0 * b.c1.x + a.c1 * b.c1.y + a.c2 * b.c1.z);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static int4x3 mul(int4x3 a, int3x3 b)
	{
		return int4x3(a.c0 * b.c0.x + a.c1 * b.c0.y + a.c2 * b.c0.z, a.c0 * b.c1.x + a.c1 * b.c1.y + a.c2 * b.c1.z, a.c0 * b.c2.x + a.c1 * b.c2.y + a.c2 * b.c2.z);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static int4x4 mul(int4x3 a, int3x4 b)
	{
		return int4x4(a.c0 * b.c0.x + a.c1 * b.c0.y + a.c2 * b.c0.z, a.c0 * b.c1.x + a.c1 * b.c1.y + a.c2 * b.c1.z, a.c0 * b.c2.x + a.c1 * b.c2.y + a.c2 * b.c2.z, a.c0 * b.c3.x + a.c1 * b.c3.y + a.c2 * b.c3.z);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static int4 mul(int4x4 a, int4 b)
	{
		return a.c0 * b.x + a.c1 * b.y + a.c2 * b.z + a.c3 * b.w;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static int4x2 mul(int4x4 a, int4x2 b)
	{
		return int4x2(a.c0 * b.c0.x + a.c1 * b.c0.y + a.c2 * b.c0.z + a.c3 * b.c0.w, a.c0 * b.c1.x + a.c1 * b.c1.y + a.c2 * b.c1.z + a.c3 * b.c1.w);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static int4x3 mul(int4x4 a, int4x3 b)
	{
		return int4x3(a.c0 * b.c0.x + a.c1 * b.c0.y + a.c2 * b.c0.z + a.c3 * b.c0.w, a.c0 * b.c1.x + a.c1 * b.c1.y + a.c2 * b.c1.z + a.c3 * b.c1.w, a.c0 * b.c2.x + a.c1 * b.c2.y + a.c2 * b.c2.z + a.c3 * b.c2.w);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static int4x4 mul(int4x4 a, int4x4 b)
	{
		return int4x4(a.c0 * b.c0.x + a.c1 * b.c0.y + a.c2 * b.c0.z + a.c3 * b.c0.w, a.c0 * b.c1.x + a.c1 * b.c1.y + a.c2 * b.c1.z + a.c3 * b.c1.w, a.c0 * b.c2.x + a.c1 * b.c2.y + a.c2 * b.c2.z + a.c3 * b.c2.w, a.c0 * b.c3.x + a.c1 * b.c3.y + a.c2 * b.c3.z + a.c3 * b.c3.w);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static uint mul(uint a, uint b)
	{
		return a * b;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static uint mul(uint2 a, uint2 b)
	{
		return a.x * b.x + a.y * b.y;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static uint2 mul(uint2 a, uint2x2 b)
	{
		return uint2(a.x * b.c0.x + a.y * b.c0.y, a.x * b.c1.x + a.y * b.c1.y);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static uint3 mul(uint2 a, uint2x3 b)
	{
		return uint3(a.x * b.c0.x + a.y * b.c0.y, a.x * b.c1.x + a.y * b.c1.y, a.x * b.c2.x + a.y * b.c2.y);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static uint4 mul(uint2 a, uint2x4 b)
	{
		return uint4(a.x * b.c0.x + a.y * b.c0.y, a.x * b.c1.x + a.y * b.c1.y, a.x * b.c2.x + a.y * b.c2.y, a.x * b.c3.x + a.y * b.c3.y);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static uint mul(uint3 a, uint3 b)
	{
		return a.x * b.x + a.y * b.y + a.z * b.z;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static uint2 mul(uint3 a, uint3x2 b)
	{
		return uint2(a.x * b.c0.x + a.y * b.c0.y + a.z * b.c0.z, a.x * b.c1.x + a.y * b.c1.y + a.z * b.c1.z);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static uint3 mul(uint3 a, uint3x3 b)
	{
		return uint3(a.x * b.c0.x + a.y * b.c0.y + a.z * b.c0.z, a.x * b.c1.x + a.y * b.c1.y + a.z * b.c1.z, a.x * b.c2.x + a.y * b.c2.y + a.z * b.c2.z);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static uint4 mul(uint3 a, uint3x4 b)
	{
		return uint4(a.x * b.c0.x + a.y * b.c0.y + a.z * b.c0.z, a.x * b.c1.x + a.y * b.c1.y + a.z * b.c1.z, a.x * b.c2.x + a.y * b.c2.y + a.z * b.c2.z, a.x * b.c3.x + a.y * b.c3.y + a.z * b.c3.z);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static uint mul(uint4 a, uint4 b)
	{
		return a.x * b.x + a.y * b.y + a.z * b.z + a.w * b.w;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static uint2 mul(uint4 a, uint4x2 b)
	{
		return uint2(a.x * b.c0.x + a.y * b.c0.y + a.z * b.c0.z + a.w * b.c0.w, a.x * b.c1.x + a.y * b.c1.y + a.z * b.c1.z + a.w * b.c1.w);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static uint3 mul(uint4 a, uint4x3 b)
	{
		return uint3(a.x * b.c0.x + a.y * b.c0.y + a.z * b.c0.z + a.w * b.c0.w, a.x * b.c1.x + a.y * b.c1.y + a.z * b.c1.z + a.w * b.c1.w, a.x * b.c2.x + a.y * b.c2.y + a.z * b.c2.z + a.w * b.c2.w);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static uint4 mul(uint4 a, uint4x4 b)
	{
		return uint4(a.x * b.c0.x + a.y * b.c0.y + a.z * b.c0.z + a.w * b.c0.w, a.x * b.c1.x + a.y * b.c1.y + a.z * b.c1.z + a.w * b.c1.w, a.x * b.c2.x + a.y * b.c2.y + a.z * b.c2.z + a.w * b.c2.w, a.x * b.c3.x + a.y * b.c3.y + a.z * b.c3.z + a.w * b.c3.w);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static uint2 mul(uint2x2 a, uint2 b)
	{
		return a.c0 * b.x + a.c1 * b.y;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static uint2x2 mul(uint2x2 a, uint2x2 b)
	{
		return uint2x2(a.c0 * b.c0.x + a.c1 * b.c0.y, a.c0 * b.c1.x + a.c1 * b.c1.y);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static uint2x3 mul(uint2x2 a, uint2x3 b)
	{
		return uint2x3(a.c0 * b.c0.x + a.c1 * b.c0.y, a.c0 * b.c1.x + a.c1 * b.c1.y, a.c0 * b.c2.x + a.c1 * b.c2.y);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static uint2x4 mul(uint2x2 a, uint2x4 b)
	{
		return uint2x4(a.c0 * b.c0.x + a.c1 * b.c0.y, a.c0 * b.c1.x + a.c1 * b.c1.y, a.c0 * b.c2.x + a.c1 * b.c2.y, a.c0 * b.c3.x + a.c1 * b.c3.y);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static uint2 mul(uint2x3 a, uint3 b)
	{
		return a.c0 * b.x + a.c1 * b.y + a.c2 * b.z;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static uint2x2 mul(uint2x3 a, uint3x2 b)
	{
		return uint2x2(a.c0 * b.c0.x + a.c1 * b.c0.y + a.c2 * b.c0.z, a.c0 * b.c1.x + a.c1 * b.c1.y + a.c2 * b.c1.z);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static uint2x3 mul(uint2x3 a, uint3x3 b)
	{
		return uint2x3(a.c0 * b.c0.x + a.c1 * b.c0.y + a.c2 * b.c0.z, a.c0 * b.c1.x + a.c1 * b.c1.y + a.c2 * b.c1.z, a.c0 * b.c2.x + a.c1 * b.c2.y + a.c2 * b.c2.z);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static uint2x4 mul(uint2x3 a, uint3x4 b)
	{
		return uint2x4(a.c0 * b.c0.x + a.c1 * b.c0.y + a.c2 * b.c0.z, a.c0 * b.c1.x + a.c1 * b.c1.y + a.c2 * b.c1.z, a.c0 * b.c2.x + a.c1 * b.c2.y + a.c2 * b.c2.z, a.c0 * b.c3.x + a.c1 * b.c3.y + a.c2 * b.c3.z);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static uint2 mul(uint2x4 a, uint4 b)
	{
		return a.c0 * b.x + a.c1 * b.y + a.c2 * b.z + a.c3 * b.w;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static uint2x2 mul(uint2x4 a, uint4x2 b)
	{
		return uint2x2(a.c0 * b.c0.x + a.c1 * b.c0.y + a.c2 * b.c0.z + a.c3 * b.c0.w, a.c0 * b.c1.x + a.c1 * b.c1.y + a.c2 * b.c1.z + a.c3 * b.c1.w);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static uint2x3 mul(uint2x4 a, uint4x3 b)
	{
		return uint2x3(a.c0 * b.c0.x + a.c1 * b.c0.y + a.c2 * b.c0.z + a.c3 * b.c0.w, a.c0 * b.c1.x + a.c1 * b.c1.y + a.c2 * b.c1.z + a.c3 * b.c1.w, a.c0 * b.c2.x + a.c1 * b.c2.y + a.c2 * b.c2.z + a.c3 * b.c2.w);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static uint2x4 mul(uint2x4 a, uint4x4 b)
	{
		return uint2x4(a.c0 * b.c0.x + a.c1 * b.c0.y + a.c2 * b.c0.z + a.c3 * b.c0.w, a.c0 * b.c1.x + a.c1 * b.c1.y + a.c2 * b.c1.z + a.c3 * b.c1.w, a.c0 * b.c2.x + a.c1 * b.c2.y + a.c2 * b.c2.z + a.c3 * b.c2.w, a.c0 * b.c3.x + a.c1 * b.c3.y + a.c2 * b.c3.z + a.c3 * b.c3.w);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static uint3 mul(uint3x2 a, uint2 b)
	{
		return a.c0 * b.x + a.c1 * b.y;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static uint3x2 mul(uint3x2 a, uint2x2 b)
	{
		return uint3x2(a.c0 * b.c0.x + a.c1 * b.c0.y, a.c0 * b.c1.x + a.c1 * b.c1.y);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static uint3x3 mul(uint3x2 a, uint2x3 b)
	{
		return uint3x3(a.c0 * b.c0.x + a.c1 * b.c0.y, a.c0 * b.c1.x + a.c1 * b.c1.y, a.c0 * b.c2.x + a.c1 * b.c2.y);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static uint3x4 mul(uint3x2 a, uint2x4 b)
	{
		return uint3x4(a.c0 * b.c0.x + a.c1 * b.c0.y, a.c0 * b.c1.x + a.c1 * b.c1.y, a.c0 * b.c2.x + a.c1 * b.c2.y, a.c0 * b.c3.x + a.c1 * b.c3.y);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static uint3 mul(uint3x3 a, uint3 b)
	{
		return a.c0 * b.x + a.c1 * b.y + a.c2 * b.z;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static uint3x2 mul(uint3x3 a, uint3x2 b)
	{
		return uint3x2(a.c0 * b.c0.x + a.c1 * b.c0.y + a.c2 * b.c0.z, a.c0 * b.c1.x + a.c1 * b.c1.y + a.c2 * b.c1.z);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static uint3x3 mul(uint3x3 a, uint3x3 b)
	{
		return uint3x3(a.c0 * b.c0.x + a.c1 * b.c0.y + a.c2 * b.c0.z, a.c0 * b.c1.x + a.c1 * b.c1.y + a.c2 * b.c1.z, a.c0 * b.c2.x + a.c1 * b.c2.y + a.c2 * b.c2.z);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static uint3x4 mul(uint3x3 a, uint3x4 b)
	{
		return uint3x4(a.c0 * b.c0.x + a.c1 * b.c0.y + a.c2 * b.c0.z, a.c0 * b.c1.x + a.c1 * b.c1.y + a.c2 * b.c1.z, a.c0 * b.c2.x + a.c1 * b.c2.y + a.c2 * b.c2.z, a.c0 * b.c3.x + a.c1 * b.c3.y + a.c2 * b.c3.z);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static uint3 mul(uint3x4 a, uint4 b)
	{
		return a.c0 * b.x + a.c1 * b.y + a.c2 * b.z + a.c3 * b.w;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static uint3x2 mul(uint3x4 a, uint4x2 b)
	{
		return uint3x2(a.c0 * b.c0.x + a.c1 * b.c0.y + a.c2 * b.c0.z + a.c3 * b.c0.w, a.c0 * b.c1.x + a.c1 * b.c1.y + a.c2 * b.c1.z + a.c3 * b.c1.w);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static uint3x3 mul(uint3x4 a, uint4x3 b)
	{
		return uint3x3(a.c0 * b.c0.x + a.c1 * b.c0.y + a.c2 * b.c0.z + a.c3 * b.c0.w, a.c0 * b.c1.x + a.c1 * b.c1.y + a.c2 * b.c1.z + a.c3 * b.c1.w, a.c0 * b.c2.x + a.c1 * b.c2.y + a.c2 * b.c2.z + a.c3 * b.c2.w);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static uint3x4 mul(uint3x4 a, uint4x4 b)
	{
		return uint3x4(a.c0 * b.c0.x + a.c1 * b.c0.y + a.c2 * b.c0.z + a.c3 * b.c0.w, a.c0 * b.c1.x + a.c1 * b.c1.y + a.c2 * b.c1.z + a.c3 * b.c1.w, a.c0 * b.c2.x + a.c1 * b.c2.y + a.c2 * b.c2.z + a.c3 * b.c2.w, a.c0 * b.c3.x + a.c1 * b.c3.y + a.c2 * b.c3.z + a.c3 * b.c3.w);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static uint4 mul(uint4x2 a, uint2 b)
	{
		return a.c0 * b.x + a.c1 * b.y;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static uint4x2 mul(uint4x2 a, uint2x2 b)
	{
		return uint4x2(a.c0 * b.c0.x + a.c1 * b.c0.y, a.c0 * b.c1.x + a.c1 * b.c1.y);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static uint4x3 mul(uint4x2 a, uint2x3 b)
	{
		return uint4x3(a.c0 * b.c0.x + a.c1 * b.c0.y, a.c0 * b.c1.x + a.c1 * b.c1.y, a.c0 * b.c2.x + a.c1 * b.c2.y);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static uint4x4 mul(uint4x2 a, uint2x4 b)
	{
		return uint4x4(a.c0 * b.c0.x + a.c1 * b.c0.y, a.c0 * b.c1.x + a.c1 * b.c1.y, a.c0 * b.c2.x + a.c1 * b.c2.y, a.c0 * b.c3.x + a.c1 * b.c3.y);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static uint4 mul(uint4x3 a, uint3 b)
	{
		return a.c0 * b.x + a.c1 * b.y + a.c2 * b.z;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static uint4x2 mul(uint4x3 a, uint3x2 b)
	{
		return uint4x2(a.c0 * b.c0.x + a.c1 * b.c0.y + a.c2 * b.c0.z, a.c0 * b.c1.x + a.c1 * b.c1.y + a.c2 * b.c1.z);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static uint4x3 mul(uint4x3 a, uint3x3 b)
	{
		return uint4x3(a.c0 * b.c0.x + a.c1 * b.c0.y + a.c2 * b.c0.z, a.c0 * b.c1.x + a.c1 * b.c1.y + a.c2 * b.c1.z, a.c0 * b.c2.x + a.c1 * b.c2.y + a.c2 * b.c2.z);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static uint4x4 mul(uint4x3 a, uint3x4 b)
	{
		return uint4x4(a.c0 * b.c0.x + a.c1 * b.c0.y + a.c2 * b.c0.z, a.c0 * b.c1.x + a.c1 * b.c1.y + a.c2 * b.c1.z, a.c0 * b.c2.x + a.c1 * b.c2.y + a.c2 * b.c2.z, a.c0 * b.c3.x + a.c1 * b.c3.y + a.c2 * b.c3.z);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static uint4 mul(uint4x4 a, uint4 b)
	{
		return a.c0 * b.x + a.c1 * b.y + a.c2 * b.z + a.c3 * b.w;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static uint4x2 mul(uint4x4 a, uint4x2 b)
	{
		return uint4x2(a.c0 * b.c0.x + a.c1 * b.c0.y + a.c2 * b.c0.z + a.c3 * b.c0.w, a.c0 * b.c1.x + a.c1 * b.c1.y + a.c2 * b.c1.z + a.c3 * b.c1.w);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static uint4x3 mul(uint4x4 a, uint4x3 b)
	{
		return uint4x3(a.c0 * b.c0.x + a.c1 * b.c0.y + a.c2 * b.c0.z + a.c3 * b.c0.w, a.c0 * b.c1.x + a.c1 * b.c1.y + a.c2 * b.c1.z + a.c3 * b.c1.w, a.c0 * b.c2.x + a.c1 * b.c2.y + a.c2 * b.c2.z + a.c3 * b.c2.w);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static uint4x4 mul(uint4x4 a, uint4x4 b)
	{
		return uint4x4(a.c0 * b.c0.x + a.c1 * b.c0.y + a.c2 * b.c0.z + a.c3 * b.c0.w, a.c0 * b.c1.x + a.c1 * b.c1.y + a.c2 * b.c1.z + a.c3 * b.c1.w, a.c0 * b.c2.x + a.c1 * b.c2.y + a.c2 * b.c2.z + a.c3 * b.c2.w, a.c0 * b.c3.x + a.c1 * b.c3.y + a.c2 * b.c3.z + a.c3 * b.c3.w);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static quaternion quaternion(float x, float y, float z, float w)
	{
		return new quaternion(x, y, z, w);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static quaternion quaternion(float4 value)
	{
		return new quaternion(value);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static quaternion quaternion(float3x3 m)
	{
		return new quaternion(m);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static quaternion quaternion(float4x4 m)
	{
		return new quaternion(m);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static quaternion conjugate(quaternion q)
	{
		return quaternion(q.value * float4(-1f, -1f, -1f, 1f));
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static quaternion inverse(quaternion q)
	{
		float4 value = q.value;
		return quaternion(rcp(dot(value, value)) * value * float4(-1f, -1f, -1f, 1f));
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static float dot(quaternion a, quaternion b)
	{
		return dot(a.value, b.value);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static float length(quaternion q)
	{
		return sqrt(dot(q.value, q.value));
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static float lengthsq(quaternion q)
	{
		return dot(q.value, q.value);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static quaternion normalize(quaternion q)
	{
		float4 value = q.value;
		return quaternion(rsqrt(dot(value, value)) * value);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static quaternion normalizesafe(quaternion q)
	{
		float4 value = q.value;
		float num = dot(value, value);
		return quaternion(select(Unity.Mathematics.quaternion.identity.value, value * rsqrt(num), num > 1.1754944E-38f));
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static quaternion normalizesafe(quaternion q, quaternion defaultvalue)
	{
		float4 value = q.value;
		float num = dot(value, value);
		return quaternion(select(defaultvalue.value, value * rsqrt(num), num > 1.1754944E-38f));
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static quaternion unitexp(quaternion q)
	{
		float num = rsqrt(dot(q.value.xyz, q.value.xyz));
		sincos(rcp(num), out var s, out var c);
		return quaternion(float4(q.value.xyz * num * s, c));
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static quaternion exp(quaternion q)
	{
		float num = rsqrt(dot(q.value.xyz, q.value.xyz));
		sincos(rcp(num), out var s, out var c);
		return quaternion(float4(q.value.xyz * num * s, c) * exp(q.value.w));
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static quaternion unitlog(quaternion q)
	{
		float num = clamp(q.value.w, -1f, 1f);
		float num2 = acos(num) * rsqrt(1f - num * num);
		return quaternion(float4(q.value.xyz * num2, 0f));
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static quaternion log(quaternion q)
	{
		float num = dot(q.value.xyz, q.value.xyz);
		float x = num + q.value.w * q.value.w;
		float num2 = acos(clamp(q.value.w * rsqrt(x), -1f, 1f)) * rsqrt(num);
		return quaternion(float4(q.value.xyz * num2, 0.5f * log(x)));
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static quaternion mul(quaternion a, quaternion b)
	{
		return quaternion(a.value.wwww * b.value + (a.value.xyzx * b.value.wwwx + a.value.yzxy * b.value.zxyy) * float4(1f, 1f, 1f, -1f) - a.value.zxyz * b.value.yzxz);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static float3 mul(quaternion q, float3 v)
	{
		float3 float5 = 2f * cross(q.value.xyz, v);
		return v + q.value.w * float5 + cross(q.value.xyz, float5);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static float3 rotate(quaternion q, float3 v)
	{
		float3 float5 = 2f * cross(q.value.xyz, v);
		return v + q.value.w * float5 + cross(q.value.xyz, float5);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static quaternion nlerp(quaternion q1, quaternion q2, float t)
	{
		if (dot(q1, q2) < 0f)
		{
			q2.value = -q2.value;
		}
		return normalize(quaternion(lerp(q1.value, q2.value, t)));
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static quaternion slerp(quaternion q1, quaternion q2, float t)
	{
		float num = dot(q1, q2);
		if (num < 0f)
		{
			num = 0f - num;
			q2.value = -q2.value;
		}
		if (num < 0.9995f)
		{
			float num2 = acos(num);
			float num3 = rsqrt(1f - num * num);
			float num4 = sin(num2 * (1f - t)) * num3;
			float num5 = sin(num2 * t) * num3;
			return quaternion(q1.value * num4 + q2.value * num5);
		}
		return nlerp(q1, q2, t);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static uint hash(quaternion q)
	{
		return hash(q.value);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static uint4 hashwide(quaternion q)
	{
		return hashwide(q.value);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static float3 forward(quaternion q)
	{
		return mul(q, float3(0f, 0f, 1f));
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static RigidTransform RigidTransform(quaternion rot, float3 pos)
	{
		return new RigidTransform(rot, pos);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static RigidTransform RigidTransform(float3x3 rotation, float3 translation)
	{
		return new RigidTransform(rotation, translation);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static RigidTransform RigidTransform(float4x4 transform)
	{
		return new RigidTransform(transform);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static RigidTransform inverse(RigidTransform t)
	{
		quaternion obj = inverse(t.rot);
		float3 translation = mul(obj, -t.pos);
		return new RigidTransform(obj, translation);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static RigidTransform mul(RigidTransform a, RigidTransform b)
	{
		return new RigidTransform(mul(a.rot, b.rot), mul(a.rot, b.pos) + a.pos);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static float4 mul(RigidTransform a, float4 pos)
	{
		return float4(mul(a.rot, pos.xyz) + a.pos * pos.w, pos.w);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static float3 rotate(RigidTransform a, float3 dir)
	{
		return mul(a.rot, dir);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static float3 transform(RigidTransform a, float3 pos)
	{
		return mul(a.rot, pos) + a.pos;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static uint hash(RigidTransform t)
	{
		return hash(t.rot) + (uint)(-976930485 * (int)hash(t.pos));
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static uint4 hashwide(RigidTransform t)
	{
		return hashwide(t.rot) + 3318036811u * hashwide(t.pos).xyzz;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static uint2 uint2(uint x, uint y)
	{
		return new uint2(x, y);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static uint2 uint2(uint2 xy)
	{
		return new uint2(xy);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static uint2 uint2(uint v)
	{
		return new uint2(v);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static uint2 uint2(bool v)
	{
		return new uint2(v);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static uint2 uint2(bool2 v)
	{
		return new uint2(v);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static uint2 uint2(int v)
	{
		return new uint2(v);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static uint2 uint2(int2 v)
	{
		return new uint2(v);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static uint2 uint2(float v)
	{
		return new uint2(v);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static uint2 uint2(float2 v)
	{
		return new uint2(v);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static uint2 uint2(double v)
	{
		return new uint2(v);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static uint2 uint2(double2 v)
	{
		return new uint2(v);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static uint hash(uint2 v)
	{
		return csum(v * uint2(1148435377u, 3416333663u)) + 1750611407;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static uint2 hashwide(uint2 v)
	{
		return v * uint2(3285396193u, 3110507567u) + 4271396531u;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static uint shuffle(uint2 a, uint2 b, ShuffleComponent x)
	{
		return select_shuffle_component(a, b, x);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static uint2 shuffle(uint2 a, uint2 b, ShuffleComponent x, ShuffleComponent y)
	{
		return uint2(select_shuffle_component(a, b, x), select_shuffle_component(a, b, y));
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static uint3 shuffle(uint2 a, uint2 b, ShuffleComponent x, ShuffleComponent y, ShuffleComponent z)
	{
		return uint3(select_shuffle_component(a, b, x), select_shuffle_component(a, b, y), select_shuffle_component(a, b, z));
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static uint4 shuffle(uint2 a, uint2 b, ShuffleComponent x, ShuffleComponent y, ShuffleComponent z, ShuffleComponent w)
	{
		return uint4(select_shuffle_component(a, b, x), select_shuffle_component(a, b, y), select_shuffle_component(a, b, z), select_shuffle_component(a, b, w));
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	internal static uint select_shuffle_component(uint2 a, uint2 b, ShuffleComponent component)
	{
		return component switch
		{
			ShuffleComponent.LeftX => a.x, 
			ShuffleComponent.LeftY => a.y, 
			ShuffleComponent.RightX => b.x, 
			ShuffleComponent.RightY => b.y, 
			_ => throw new ArgumentException("Invalid shuffle component: " + component), 
		};
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static uint2x2 uint2x2(uint2 c0, uint2 c1)
	{
		return new uint2x2(c0, c1);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static uint2x2 uint2x2(uint m00, uint m01, uint m10, uint m11)
	{
		return new uint2x2(m00, m01, m10, m11);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static uint2x2 uint2x2(uint v)
	{
		return new uint2x2(v);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static uint2x2 uint2x2(bool v)
	{
		return new uint2x2(v);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static uint2x2 uint2x2(bool2x2 v)
	{
		return new uint2x2(v);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static uint2x2 uint2x2(int v)
	{
		return new uint2x2(v);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static uint2x2 uint2x2(int2x2 v)
	{
		return new uint2x2(v);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static uint2x2 uint2x2(float v)
	{
		return new uint2x2(v);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static uint2x2 uint2x2(float2x2 v)
	{
		return new uint2x2(v);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static uint2x2 uint2x2(double v)
	{
		return new uint2x2(v);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static uint2x2 uint2x2(double2x2 v)
	{
		return new uint2x2(v);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static uint2x2 transpose(uint2x2 v)
	{
		return uint2x2(v.c0.x, v.c0.y, v.c1.x, v.c1.y);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static uint hash(uint2x2 v)
	{
		return csum(v.c0 * uint2(3010324327u, 1875523709u) + v.c1 * uint2(2937008387u, 3835713223u)) + 2216526373u;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static uint2 hashwide(uint2x2 v)
	{
		return v.c0 * uint2(3375971453u, 3559829411u) + v.c1 * uint2(3652178029u, 2544260129u) + 2013864031u;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static uint2x3 uint2x3(uint2 c0, uint2 c1, uint2 c2)
	{
		return new uint2x3(c0, c1, c2);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static uint2x3 uint2x3(uint m00, uint m01, uint m02, uint m10, uint m11, uint m12)
	{
		return new uint2x3(m00, m01, m02, m10, m11, m12);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static uint2x3 uint2x3(uint v)
	{
		return new uint2x3(v);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static uint2x3 uint2x3(bool v)
	{
		return new uint2x3(v);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static uint2x3 uint2x3(bool2x3 v)
	{
		return new uint2x3(v);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static uint2x3 uint2x3(int v)
	{
		return new uint2x3(v);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static uint2x3 uint2x3(int2x3 v)
	{
		return new uint2x3(v);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static uint2x3 uint2x3(float v)
	{
		return new uint2x3(v);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static uint2x3 uint2x3(float2x3 v)
	{
		return new uint2x3(v);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static uint2x3 uint2x3(double v)
	{
		return new uint2x3(v);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static uint2x3 uint2x3(double2x3 v)
	{
		return new uint2x3(v);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static uint3x2 transpose(uint2x3 v)
	{
		return uint3x2(v.c0.x, v.c0.y, v.c1.x, v.c1.y, v.c2.x, v.c2.y);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static uint hash(uint2x3 v)
	{
		return csum(v.c0 * uint2(4016293529u, 2416021567u) + v.c1 * uint2(2828384717u, 2636362241u) + v.c2 * uint2(1258410977u, 1952565773u)) + 2037535609;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static uint2 hashwide(uint2x3 v)
	{
		return v.c0 * uint2(3592785499u, 3996716183u) + v.c1 * uint2(2626301701u, 1306289417u) + v.c2 * uint2(2096137163u, 1548578029u) + 4178800919u;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static uint2x4 uint2x4(uint2 c0, uint2 c1, uint2 c2, uint2 c3)
	{
		return new uint2x4(c0, c1, c2, c3);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static uint2x4 uint2x4(uint m00, uint m01, uint m02, uint m03, uint m10, uint m11, uint m12, uint m13)
	{
		return new uint2x4(m00, m01, m02, m03, m10, m11, m12, m13);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static uint2x4 uint2x4(uint v)
	{
		return new uint2x4(v);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static uint2x4 uint2x4(bool v)
	{
		return new uint2x4(v);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static uint2x4 uint2x4(bool2x4 v)
	{
		return new uint2x4(v);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static uint2x4 uint2x4(int v)
	{
		return new uint2x4(v);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static uint2x4 uint2x4(int2x4 v)
	{
		return new uint2x4(v);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static uint2x4 uint2x4(float v)
	{
		return new uint2x4(v);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static uint2x4 uint2x4(float2x4 v)
	{
		return new uint2x4(v);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static uint2x4 uint2x4(double v)
	{
		return new uint2x4(v);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static uint2x4 uint2x4(double2x4 v)
	{
		return new uint2x4(v);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static uint4x2 transpose(uint2x4 v)
	{
		return uint4x2(v.c0.x, v.c0.y, v.c1.x, v.c1.y, v.c2.x, v.c2.y, v.c3.x, v.c3.y);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static uint hash(uint2x4 v)
	{
		return csum(v.c0 * uint2(2650080659u, 4052675461u) + v.c1 * uint2(2652487619u, 2174136431u) + v.c2 * uint2(3528391193u, 2105559227u) + v.c3 * uint2(1899745391u, 1966790317u)) + 3516359879u;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static uint2 hashwide(uint2x4 v)
	{
		return v.c0 * uint2(3050356579u, 4178586719u) + v.c1 * uint2(2558655391u, 1453413133u) + v.c2 * uint2(2152428077u, 1938706661u) + v.c3 * uint2(1338588197u, 3439609253u) + 3535343003u;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static uint3 uint3(uint x, uint y, uint z)
	{
		return new uint3(x, y, z);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static uint3 uint3(uint x, uint2 yz)
	{
		return new uint3(x, yz);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static uint3 uint3(uint2 xy, uint z)
	{
		return new uint3(xy, z);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static uint3 uint3(uint3 xyz)
	{
		return new uint3(xyz);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static uint3 uint3(uint v)
	{
		return new uint3(v);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static uint3 uint3(bool v)
	{
		return new uint3(v);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static uint3 uint3(bool3 v)
	{
		return new uint3(v);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static uint3 uint3(int v)
	{
		return new uint3(v);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static uint3 uint3(int3 v)
	{
		return new uint3(v);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static uint3 uint3(float v)
	{
		return new uint3(v);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static uint3 uint3(float3 v)
	{
		return new uint3(v);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static uint3 uint3(double v)
	{
		return new uint3(v);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static uint3 uint3(double3 v)
	{
		return new uint3(v);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static uint hash(uint3 v)
	{
		return csum(v * uint3(3441847433u, 4052036147u, 2011389559u)) + 2252224297u;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static uint3 hashwide(uint3 v)
	{
		return v * uint3(3784421429u, 1750626223u, 3571447507u) + 3412283213u;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static uint shuffle(uint3 a, uint3 b, ShuffleComponent x)
	{
		return select_shuffle_component(a, b, x);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static uint2 shuffle(uint3 a, uint3 b, ShuffleComponent x, ShuffleComponent y)
	{
		return uint2(select_shuffle_component(a, b, x), select_shuffle_component(a, b, y));
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static uint3 shuffle(uint3 a, uint3 b, ShuffleComponent x, ShuffleComponent y, ShuffleComponent z)
	{
		return uint3(select_shuffle_component(a, b, x), select_shuffle_component(a, b, y), select_shuffle_component(a, b, z));
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static uint4 shuffle(uint3 a, uint3 b, ShuffleComponent x, ShuffleComponent y, ShuffleComponent z, ShuffleComponent w)
	{
		return uint4(select_shuffle_component(a, b, x), select_shuffle_component(a, b, y), select_shuffle_component(a, b, z), select_shuffle_component(a, b, w));
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	internal static uint select_shuffle_component(uint3 a, uint3 b, ShuffleComponent component)
	{
		return component switch
		{
			ShuffleComponent.LeftX => a.x, 
			ShuffleComponent.LeftY => a.y, 
			ShuffleComponent.LeftZ => a.z, 
			ShuffleComponent.RightX => b.x, 
			ShuffleComponent.RightY => b.y, 
			ShuffleComponent.RightZ => b.z, 
			_ => throw new ArgumentException("Invalid shuffle component: " + component), 
		};
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static uint3x2 uint3x2(uint3 c0, uint3 c1)
	{
		return new uint3x2(c0, c1);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static uint3x2 uint3x2(uint m00, uint m01, uint m10, uint m11, uint m20, uint m21)
	{
		return new uint3x2(m00, m01, m10, m11, m20, m21);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static uint3x2 uint3x2(uint v)
	{
		return new uint3x2(v);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static uint3x2 uint3x2(bool v)
	{
		return new uint3x2(v);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static uint3x2 uint3x2(bool3x2 v)
	{
		return new uint3x2(v);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static uint3x2 uint3x2(int v)
	{
		return new uint3x2(v);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static uint3x2 uint3x2(int3x2 v)
	{
		return new uint3x2(v);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static uint3x2 uint3x2(float v)
	{
		return new uint3x2(v);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static uint3x2 uint3x2(float3x2 v)
	{
		return new uint3x2(v);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static uint3x2 uint3x2(double v)
	{
		return new uint3x2(v);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static uint3x2 uint3x2(double3x2 v)
	{
		return new uint3x2(v);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static uint2x3 transpose(uint3x2 v)
	{
		return uint2x3(v.c0.x, v.c0.y, v.c0.z, v.c1.x, v.c1.y, v.c1.z);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static uint hash(uint3x2 v)
	{
		return csum(v.c0 * uint3(1365086453u, 3969870067u, 4192899797u) + v.c1 * uint3(3271228601u, 1634639009u, 3318036811u)) + 3404170631u;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static uint3 hashwide(uint3x2 v)
	{
		return v.c0 * uint3(2048213449u, 4164671783u, 1780759499u) + v.c1 * uint3(1352369353u, 2446407751u, 1391928079u) + 3475533443u;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static uint3x3 uint3x3(uint3 c0, uint3 c1, uint3 c2)
	{
		return new uint3x3(c0, c1, c2);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static uint3x3 uint3x3(uint m00, uint m01, uint m02, uint m10, uint m11, uint m12, uint m20, uint m21, uint m22)
	{
		return new uint3x3(m00, m01, m02, m10, m11, m12, m20, m21, m22);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static uint3x3 uint3x3(uint v)
	{
		return new uint3x3(v);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static uint3x3 uint3x3(bool v)
	{
		return new uint3x3(v);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static uint3x3 uint3x3(bool3x3 v)
	{
		return new uint3x3(v);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static uint3x3 uint3x3(int v)
	{
		return new uint3x3(v);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static uint3x3 uint3x3(int3x3 v)
	{
		return new uint3x3(v);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static uint3x3 uint3x3(float v)
	{
		return new uint3x3(v);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static uint3x3 uint3x3(float3x3 v)
	{
		return new uint3x3(v);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static uint3x3 uint3x3(double v)
	{
		return new uint3x3(v);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static uint3x3 uint3x3(double3x3 v)
	{
		return new uint3x3(v);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static uint3x3 transpose(uint3x3 v)
	{
		return uint3x3(v.c0.x, v.c0.y, v.c0.z, v.c1.x, v.c1.y, v.c1.z, v.c2.x, v.c2.y, v.c2.z);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static uint hash(uint3x3 v)
	{
		return csum(v.c0 * uint3(2892026051u, 2455987759u, 3868600063u) + v.c1 * uint3(3170963179u, 2632835537u, 1136528209u) + v.c2 * uint3(2944626401u, 2972762423u, 1417889653u)) + 2080514593;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static uint3 hashwide(uint3x3 v)
	{
		return v.c0 * uint3(2731544287u, 2828498809u, 2669441947u) + v.c1 * uint3(1260114311u, 2650080659u, 4052675461u) + v.c2 * uint3(2652487619u, 2174136431u, 3528391193u) + 2105559227u;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static uint3x4 uint3x4(uint3 c0, uint3 c1, uint3 c2, uint3 c3)
	{
		return new uint3x4(c0, c1, c2, c3);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static uint3x4 uint3x4(uint m00, uint m01, uint m02, uint m03, uint m10, uint m11, uint m12, uint m13, uint m20, uint m21, uint m22, uint m23)
	{
		return new uint3x4(m00, m01, m02, m03, m10, m11, m12, m13, m20, m21, m22, m23);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static uint3x4 uint3x4(uint v)
	{
		return new uint3x4(v);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static uint3x4 uint3x4(bool v)
	{
		return new uint3x4(v);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static uint3x4 uint3x4(bool3x4 v)
	{
		return new uint3x4(v);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static uint3x4 uint3x4(int v)
	{
		return new uint3x4(v);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static uint3x4 uint3x4(int3x4 v)
	{
		return new uint3x4(v);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static uint3x4 uint3x4(float v)
	{
		return new uint3x4(v);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static uint3x4 uint3x4(float3x4 v)
	{
		return new uint3x4(v);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static uint3x4 uint3x4(double v)
	{
		return new uint3x4(v);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static uint3x4 uint3x4(double3x4 v)
	{
		return new uint3x4(v);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static uint4x3 transpose(uint3x4 v)
	{
		return uint4x3(v.c0.x, v.c0.y, v.c0.z, v.c1.x, v.c1.y, v.c1.z, v.c2.x, v.c2.y, v.c2.z, v.c3.x, v.c3.y, v.c3.z);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static uint hash(uint3x4 v)
	{
		return csum(v.c0 * uint3(3508684087u, 3919501043u, 1209161033u) + v.c1 * uint3(4007793211u, 3819806693u, 3458005183u) + v.c2 * uint3(2078515003u, 4206465343u, 3025146473u) + v.c3 * uint3(3763046909u, 3678265601u, 2070747979u)) + 1480171127;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static uint3 hashwide(uint3x4 v)
	{
		return v.c0 * uint3(1588341193u, 4234155257u, 1811310911u) + v.c1 * uint3(2635799963u, 4165137857u, 2759770933u) + v.c2 * uint3(2759319383u, 3299952959u, 3121178323u) + v.c3 * uint3(2948522579u, 1531026433u, 1365086453u) + 3969870067u;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static uint4 uint4(uint x, uint y, uint z, uint w)
	{
		return new uint4(x, y, z, w);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static uint4 uint4(uint x, uint y, uint2 zw)
	{
		return new uint4(x, y, zw);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static uint4 uint4(uint x, uint2 yz, uint w)
	{
		return new uint4(x, yz, w);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static uint4 uint4(uint x, uint3 yzw)
	{
		return new uint4(x, yzw);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static uint4 uint4(uint2 xy, uint z, uint w)
	{
		return new uint4(xy, z, w);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static uint4 uint4(uint2 xy, uint2 zw)
	{
		return new uint4(xy, zw);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static uint4 uint4(uint3 xyz, uint w)
	{
		return new uint4(xyz, w);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static uint4 uint4(uint4 xyzw)
	{
		return new uint4(xyzw);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static uint4 uint4(uint v)
	{
		return new uint4(v);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static uint4 uint4(bool v)
	{
		return new uint4(v);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static uint4 uint4(bool4 v)
	{
		return new uint4(v);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static uint4 uint4(int v)
	{
		return new uint4(v);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static uint4 uint4(int4 v)
	{
		return new uint4(v);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static uint4 uint4(float v)
	{
		return new uint4(v);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static uint4 uint4(float4 v)
	{
		return new uint4(v);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static uint4 uint4(double v)
	{
		return new uint4(v);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static uint4 uint4(double4 v)
	{
		return new uint4(v);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static uint hash(uint4 v)
	{
		return csum(v * uint4(3029516053u, 3547472099u, 2057487037u, 3781937309u)) + 2057338067;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static uint4 hashwide(uint4 v)
	{
		return v * uint4(2942577577u, 2834440507u, 2671762487u, 2892026051u) + 2455987759u;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static uint shuffle(uint4 a, uint4 b, ShuffleComponent x)
	{
		return select_shuffle_component(a, b, x);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static uint2 shuffle(uint4 a, uint4 b, ShuffleComponent x, ShuffleComponent y)
	{
		return uint2(select_shuffle_component(a, b, x), select_shuffle_component(a, b, y));
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static uint3 shuffle(uint4 a, uint4 b, ShuffleComponent x, ShuffleComponent y, ShuffleComponent z)
	{
		return uint3(select_shuffle_component(a, b, x), select_shuffle_component(a, b, y), select_shuffle_component(a, b, z));
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static uint4 shuffle(uint4 a, uint4 b, ShuffleComponent x, ShuffleComponent y, ShuffleComponent z, ShuffleComponent w)
	{
		return uint4(select_shuffle_component(a, b, x), select_shuffle_component(a, b, y), select_shuffle_component(a, b, z), select_shuffle_component(a, b, w));
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	internal static uint select_shuffle_component(uint4 a, uint4 b, ShuffleComponent component)
	{
		return component switch
		{
			ShuffleComponent.LeftX => a.x, 
			ShuffleComponent.LeftY => a.y, 
			ShuffleComponent.LeftZ => a.z, 
			ShuffleComponent.LeftW => a.w, 
			ShuffleComponent.RightX => b.x, 
			ShuffleComponent.RightY => b.y, 
			ShuffleComponent.RightZ => b.z, 
			ShuffleComponent.RightW => b.w, 
			_ => throw new ArgumentException("Invalid shuffle component: " + component), 
		};
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static uint4x2 uint4x2(uint4 c0, uint4 c1)
	{
		return new uint4x2(c0, c1);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static uint4x2 uint4x2(uint m00, uint m01, uint m10, uint m11, uint m20, uint m21, uint m30, uint m31)
	{
		return new uint4x2(m00, m01, m10, m11, m20, m21, m30, m31);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static uint4x2 uint4x2(uint v)
	{
		return new uint4x2(v);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static uint4x2 uint4x2(bool v)
	{
		return new uint4x2(v);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static uint4x2 uint4x2(bool4x2 v)
	{
		return new uint4x2(v);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static uint4x2 uint4x2(int v)
	{
		return new uint4x2(v);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static uint4x2 uint4x2(int4x2 v)
	{
		return new uint4x2(v);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static uint4x2 uint4x2(float v)
	{
		return new uint4x2(v);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static uint4x2 uint4x2(float4x2 v)
	{
		return new uint4x2(v);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static uint4x2 uint4x2(double v)
	{
		return new uint4x2(v);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static uint4x2 uint4x2(double4x2 v)
	{
		return new uint4x2(v);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static uint2x4 transpose(uint4x2 v)
	{
		return uint2x4(v.c0.x, v.c0.y, v.c0.z, v.c0.w, v.c1.x, v.c1.y, v.c1.z, v.c1.w);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static uint hash(uint4x2 v)
	{
		return csum(v.c0 * uint4(4198118021u, 2908068253u, 3705492289u, 2497566569u) + v.c1 * uint4(2716413241u, 1166264321u, 2503385333u, 2944493077u)) + 2599999021u;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static uint4 hashwide(uint4x2 v)
	{
		return v.c0 * uint4(3814721321u, 1595355149u, 1728931849u, 2062756937u) + v.c1 * uint4(2920485769u, 1562056283u, 2265541847u, 1283419601u) + 1210229737u;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static uint4x3 uint4x3(uint4 c0, uint4 c1, uint4 c2)
	{
		return new uint4x3(c0, c1, c2);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static uint4x3 uint4x3(uint m00, uint m01, uint m02, uint m10, uint m11, uint m12, uint m20, uint m21, uint m22, uint m30, uint m31, uint m32)
	{
		return new uint4x3(m00, m01, m02, m10, m11, m12, m20, m21, m22, m30, m31, m32);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static uint4x3 uint4x3(uint v)
	{
		return new uint4x3(v);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static uint4x3 uint4x3(bool v)
	{
		return new uint4x3(v);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static uint4x3 uint4x3(bool4x3 v)
	{
		return new uint4x3(v);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static uint4x3 uint4x3(int v)
	{
		return new uint4x3(v);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static uint4x3 uint4x3(int4x3 v)
	{
		return new uint4x3(v);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static uint4x3 uint4x3(float v)
	{
		return new uint4x3(v);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static uint4x3 uint4x3(float4x3 v)
	{
		return new uint4x3(v);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static uint4x3 uint4x3(double v)
	{
		return new uint4x3(v);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static uint4x3 uint4x3(double4x3 v)
	{
		return new uint4x3(v);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static uint3x4 transpose(uint4x3 v)
	{
		return uint3x4(v.c0.x, v.c0.y, v.c0.z, v.c0.w, v.c1.x, v.c1.y, v.c1.z, v.c1.w, v.c2.x, v.c2.y, v.c2.z, v.c2.w);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static uint hash(uint4x3 v)
	{
		return csum(v.c0 * uint4(3881277847u, 4017968839u, 1727237899u, 1648514723u) + v.c1 * uint4(1385344481u, 3538260197u, 4066109527u, 2613148903u) + v.c2 * uint4(3367528529u, 1678332449u, 2918459647u, 2744611081u)) + 1952372791;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static uint4 hashwide(uint4x3 v)
	{
		return v.c0 * uint4(2631698677u, 4200781601u, 2119021007u, 1760485621u) + v.c1 * uint4(3157985881u, 2171534173u, 2723054263u, 1168253063u) + v.c2 * uint4(4228926523u, 1610574617u, 1584185147u, 3041325733u) + 3150930919u;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static uint4x4 uint4x4(uint4 c0, uint4 c1, uint4 c2, uint4 c3)
	{
		return new uint4x4(c0, c1, c2, c3);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static uint4x4 uint4x4(uint m00, uint m01, uint m02, uint m03, uint m10, uint m11, uint m12, uint m13, uint m20, uint m21, uint m22, uint m23, uint m30, uint m31, uint m32, uint m33)
	{
		return new uint4x4(m00, m01, m02, m03, m10, m11, m12, m13, m20, m21, m22, m23, m30, m31, m32, m33);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static uint4x4 uint4x4(uint v)
	{
		return new uint4x4(v);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static uint4x4 uint4x4(bool v)
	{
		return new uint4x4(v);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static uint4x4 uint4x4(bool4x4 v)
	{
		return new uint4x4(v);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static uint4x4 uint4x4(int v)
	{
		return new uint4x4(v);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static uint4x4 uint4x4(int4x4 v)
	{
		return new uint4x4(v);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static uint4x4 uint4x4(float v)
	{
		return new uint4x4(v);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static uint4x4 uint4x4(float4x4 v)
	{
		return new uint4x4(v);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static uint4x4 uint4x4(double v)
	{
		return new uint4x4(v);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static uint4x4 uint4x4(double4x4 v)
	{
		return new uint4x4(v);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static uint4x4 transpose(uint4x4 v)
	{
		return uint4x4(v.c0.x, v.c0.y, v.c0.z, v.c0.w, v.c1.x, v.c1.y, v.c1.z, v.c1.w, v.c2.x, v.c2.y, v.c2.z, v.c2.w, v.c3.x, v.c3.y, v.c3.z, v.c3.w);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static uint hash(uint4x4 v)
	{
		return csum(v.c0 * uint4(2627668003u, 1520214331u, 2949502447u, 2827819133u) + v.c1 * uint4(3480140317u, 2642994593u, 3940484981u, 1954192763u) + v.c2 * uint4(1091696537u, 3052428017u, 4253034763u, 2338696631u) + v.c3 * uint4(3757372771u, 1885959949u, 3508684087u, 3919501043u)) + 1209161033;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static uint4 hashwide(uint4x4 v)
	{
		return v.c0 * uint4(4007793211u, 3819806693u, 3458005183u, 2078515003u) + v.c1 * uint4(4206465343u, 3025146473u, 3763046909u, 3678265601u) + v.c2 * uint4(2070747979u, 1480171127u, 1588341193u, 4234155257u) + v.c3 * uint4(1811310911u, 2635799963u, 4165137857u, 2759770933u) + 2759319383u;
	}
}
