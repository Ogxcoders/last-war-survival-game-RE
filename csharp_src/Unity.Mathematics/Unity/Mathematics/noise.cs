using System;

namespace Unity.Mathematics;

public static class noise
{
	public static float2 cellular(float2 P)
	{
		float2 float5 = mod289(math.floor(P));
		float2 obj = math.frac(P);
		float3 float6 = math.float3(-1f, 0f, 1f);
		float3 float7 = math.float3(-0.5f, 0.5f, 1.5f);
		float3 float8 = permute(float5.x + float6);
		float3 obj2 = permute(float8.x + float5.y + float6);
		float3 float9 = math.frac(obj2 * (1f / 7f)) - 0.42857143f;
		float3 float10 = mod7(math.floor(obj2 * (1f / 7f))) * (1f / 7f) - 0.42857143f;
		float3 float11 = obj.x + 0.5f + 1f * float9;
		float3 float12 = obj.y - float7 + 1f * float10;
		float3 x = float11 * float11 + float12 * float12;
		float3 obj3 = permute(float8.y + float5.y + float6);
		float9 = math.frac(obj3 * (1f / 7f)) - 0.42857143f;
		float10 = mod7(math.floor(obj3 * (1f / 7f))) * (1f / 7f) - 0.42857143f;
		float11 = obj.x - 0.5f + 1f * float9;
		float12 = obj.y - float7 + 1f * float10;
		float3 y = float11 * float11 + float12 * float12;
		float3 obj4 = permute(float8.z + float5.y + float6);
		float9 = math.frac(obj4 * (1f / 7f)) - 0.42857143f;
		float10 = mod7(math.floor(obj4 * (1f / 7f))) * (1f / 7f) - 0.42857143f;
		float11 = obj.x - 1.5f + 1f * float9;
		float12 = obj.y - float7 + 1f * float10;
		float3 y2 = float11 * float11 + float12 * float12;
		float3 x2 = math.min(x, y);
		y = math.max(x, y);
		y = math.min(y, y2);
		x = math.min(x2, y);
		y = math.max(x2, y);
		x.xy = ((x.x < x.y) ? x.xy : x.yx);
		x.xz = ((x.x < x.z) ? x.xz : x.zx);
		x.yz = math.min(x.yz, y.yz);
		x.y = math.min(x.y, x.z);
		x.y = math.min(x.y, y.x);
		return math.sqrt(x.xy);
	}

	public static float2 cellular2x2(float2 P)
	{
		float2 float5 = mod289(math.floor(P));
		float2 obj = math.frac(P);
		float4 float6 = obj.x + math.float4(-0.5f, -1.5f, -0.5f, -1.5f);
		float4 float7 = obj.y + math.float4(-0.5f, -0.5f, -1.5f, -1.5f);
		float4 obj2 = permute(permute(float5.x + math.float4(0f, 1f, 0f, 1f)) + float5.y + math.float4(0f, 0f, 1f, 1f));
		float4 float8 = mod7(obj2) * (1f / 7f) + 1f / 14f;
		float4 float9 = mod7(math.floor(obj2 * (1f / 7f))) * (1f / 7f) + 1f / 14f;
		float4 obj3 = float6 + 0.8f * float8;
		float4 float10 = float7 + 0.8f * float9;
		float4 float11 = obj3 * obj3 + float10 * float10;
		float11.xy = ((float11.x < float11.y) ? float11.xy : float11.yx);
		float11.xz = ((float11.x < float11.z) ? float11.xz : float11.zx);
		float11.xw = ((float11.x < float11.w) ? float11.xw : float11.wx);
		float11.y = math.min(float11.y, float11.z);
		float11.y = math.min(float11.y, float11.w);
		return math.sqrt(float11.xy);
	}

	public static float2 cellular2x2x2(float3 P)
	{
		float3 float5 = mod289(math.floor(P));
		float3 float6 = math.frac(P);
		float4 float7 = float6.x + math.float4(0f, -1f, 0f, -1f);
		float4 float8 = float6.y + math.float4(0f, 0f, -1f, -1f);
		float4 obj = permute(permute(float5.x + math.float4(0f, 1f, 0f, 1f)) + float5.y + math.float4(0f, 0f, 1f, 1f));
		float4 float9 = permute(obj + float5.z);
		float4 obj2 = permute(obj + float5.z + math.float4(1f, 1f, 1f, 1f));
		float4 float10 = math.frac(float9 * (1f / 7f)) - 0.42857143f;
		float4 float11 = mod7(math.floor(float9 * (1f / 7f))) * (1f / 7f) - 0.42857143f;
		float4 float12 = math.floor(float9 * (1f / 49f)) * (1f / 6f) - 5f / 12f;
		float4 float13 = math.frac(obj2 * (1f / 7f)) - 0.42857143f;
		float4 float14 = mod7(math.floor(obj2 * (1f / 7f))) * (1f / 7f) - 0.42857143f;
		float4 float15 = math.floor(obj2 * (1f / 49f)) * (1f / 6f) - 5f / 12f;
		float4 float16 = float7 + 0.8f * float10;
		float4 float17 = float8 + 0.8f * float11;
		float4 float18 = float6.z + 0.8f * float12;
		float4 float19 = float7 + 0.8f * float13;
		float4 float20 = float8 + 0.8f * float14;
		float4 float21 = float6.z - 1f + 0.8f * float15;
		float4 x = float16 * float16 + float17 * float17 + float18 * float18;
		float4 y = float19 * float19 + float20 * float20 + float21 * float21;
		float4 float22 = math.min(x, y);
		y = math.max(x, y);
		float22.xy = ((float22.x < float22.y) ? float22.xy : float22.yx);
		float22.xz = ((float22.x < float22.z) ? float22.xz : float22.zx);
		float22.xw = ((float22.x < float22.w) ? float22.xw : float22.wx);
		float22.yzw = math.min(float22.yzw, y.yzw);
		float22.y = math.min(float22.y, float22.z);
		float22.y = math.min(float22.y, float22.w);
		float22.y = math.min(float22.y, y.x);
		return math.sqrt(float22.xy);
	}

	public static float2 cellular(float3 P)
	{
		float3 float5 = mod289(math.floor(P));
		float3 obj = math.frac(P) - 0.5f;
		float3 float6 = obj.x + math.float3(1f, 0f, -1f);
		float3 float7 = obj.y + math.float3(1f, 0f, -1f);
		float3 float8 = obj.z + math.float3(1f, 0f, -1f);
		float3 obj2 = permute(float5.x + math.float3(-1f, 0f, 1f));
		float3 float9 = permute(obj2 + float5.y - 1f);
		float3 float10 = permute(obj2 + float5.y);
		float3 obj3 = permute(obj2 + float5.y + 1f);
		float3 float11 = permute(float9 + float5.z - 1f);
		float3 float12 = permute(float9 + float5.z);
		float3 float13 = permute(float9 + float5.z + 1f);
		float3 float14 = permute(float10 + float5.z - 1f);
		float3 float15 = permute(float10 + float5.z);
		float3 float16 = permute(float10 + float5.z + 1f);
		float3 float17 = permute(obj3 + float5.z - 1f);
		float3 float18 = permute(obj3 + float5.z);
		float3 obj4 = permute(obj3 + float5.z + 1f);
		float3 float19 = math.frac(float11 * (1f / 7f)) - 0.42857143f;
		float3 float20 = mod7(math.floor(float11 * (1f / 7f))) * (1f / 7f) - 0.42857143f;
		float3 float21 = math.floor(float11 * (1f / 49f)) * (1f / 6f) - 5f / 12f;
		float3 float22 = math.frac(float12 * (1f / 7f)) - 0.42857143f;
		float3 float23 = mod7(math.floor(float12 * (1f / 7f))) * (1f / 7f) - 0.42857143f;
		float3 float24 = math.floor(float12 * (1f / 49f)) * (1f / 6f) - 5f / 12f;
		float3 float25 = math.frac(float13 * (1f / 7f)) - 0.42857143f;
		float3 float26 = mod7(math.floor(float13 * (1f / 7f))) * (1f / 7f) - 0.42857143f;
		float3 float27 = math.floor(float13 * (1f / 49f)) * (1f / 6f) - 5f / 12f;
		float3 float28 = math.frac(float14 * (1f / 7f)) - 0.42857143f;
		float3 float29 = mod7(math.floor(float14 * (1f / 7f))) * (1f / 7f) - 0.42857143f;
		float3 float30 = math.floor(float14 * (1f / 49f)) * (1f / 6f) - 5f / 12f;
		float3 float31 = math.frac(float15 * (1f / 7f)) - 0.42857143f;
		float3 float32 = mod7(math.floor(float15 * (1f / 7f))) * (1f / 7f) - 0.42857143f;
		float3 float33 = math.floor(float15 * (1f / 49f)) * (1f / 6f) - 5f / 12f;
		float3 float34 = math.frac(float16 * (1f / 7f)) - 0.42857143f;
		float3 float35 = mod7(math.floor(float16 * (1f / 7f))) * (1f / 7f) - 0.42857143f;
		float3 float36 = math.floor(float16 * (1f / 49f)) * (1f / 6f) - 5f / 12f;
		float3 float37 = math.frac(float17 * (1f / 7f)) - 0.42857143f;
		float3 float38 = mod7(math.floor(float17 * (1f / 7f))) * (1f / 7f) - 0.42857143f;
		float3 float39 = math.floor(float17 * (1f / 49f)) * (1f / 6f) - 5f / 12f;
		float3 float40 = math.frac(float18 * (1f / 7f)) - 0.42857143f;
		float3 float41 = mod7(math.floor(float18 * (1f / 7f))) * (1f / 7f) - 0.42857143f;
		float3 float42 = math.floor(float18 * (1f / 49f)) * (1f / 6f) - 5f / 12f;
		float3 float43 = math.frac(obj4 * (1f / 7f)) - 0.42857143f;
		float3 float44 = mod7(math.floor(obj4 * (1f / 7f))) * (1f / 7f) - 0.42857143f;
		float3 float45 = math.floor(obj4 * (1f / 49f)) * (1f / 6f) - 5f / 12f;
		float3 float46 = float6 + 1f * float19;
		float3 float47 = float7.x + 1f * float20;
		float3 float48 = float8.x + 1f * float21;
		float3 float49 = float6 + 1f * float22;
		float3 float50 = float7.x + 1f * float23;
		float3 float51 = float8.y + 1f * float24;
		float3 float52 = float6 + 1f * float25;
		float3 float53 = float7.x + 1f * float26;
		float3 float54 = float8.z + 1f * float27;
		float3 float55 = float6 + 1f * float28;
		float3 float56 = float7.y + 1f * float29;
		float3 float57 = float8.x + 1f * float30;
		float3 float58 = float6 + 1f * float31;
		float3 float59 = float7.y + 1f * float32;
		float3 float60 = float8.y + 1f * float33;
		float3 float61 = float6 + 1f * float34;
		float3 float62 = float7.y + 1f * float35;
		float3 float63 = float8.z + 1f * float36;
		float3 float64 = float6 + 1f * float37;
		float3 float65 = float7.z + 1f * float38;
		float3 float66 = float8.x + 1f * float39;
		float3 float67 = float6 + 1f * float40;
		float3 float68 = float7.z + 1f * float41;
		float3 float69 = float8.y + 1f * float42;
		float3 obj5 = float6 + 1f * float43;
		float3 float70 = float7.z + 1f * float44;
		float3 float71 = float8.z + 1f * float45;
		float3 x = float46 * float46 + float47 * float47 + float48 * float48;
		float3 y = float49 * float49 + float50 * float50 + float51 * float51;
		float3 y2 = float52 * float52 + float53 * float53 + float54 * float54;
		float3 x2 = float55 * float55 + float56 * float56 + float57 * float57;
		float3 y3 = float58 * float58 + float59 * float59 + float60 * float60;
		float3 y4 = float61 * float61 + float62 * float62 + float63 * float63;
		float3 x3 = float64 * float64 + float65 * float65 + float66 * float66;
		float3 y5 = float67 * float67 + float68 * float68 + float69 * float69;
		float3 y6 = obj5 * obj5 + float70 * float70 + float71 * float71;
		float3 x4 = math.min(x, y);
		y = math.max(x, y);
		x = math.min(x4, y2);
		y2 = math.max(x4, y2);
		y = math.min(y, y2);
		float3 x5 = math.min(x2, y3);
		y3 = math.max(x2, y3);
		x2 = math.min(x5, y4);
		y4 = math.max(x5, y4);
		y3 = math.min(y3, y4);
		float3 x6 = math.min(x3, y5);
		y5 = math.max(x3, y5);
		x3 = math.min(x6, y6);
		y6 = math.max(x6, y6);
		y5 = math.min(y5, y6);
		float3 x7 = math.min(x, x2);
		x2 = math.max(x, x2);
		x = math.min(x7, x3);
		x3 = math.max(x7, x3);
		x.xy = ((x.x < x.y) ? x.xy : x.yx);
		x.xz = ((x.x < x.z) ? x.xz : x.zx);
		y = math.min(y, x2);
		y = math.min(y, y3);
		y = math.min(y, x3);
		y = math.min(y, y5);
		x.yz = math.min(x.yz, y.xy);
		x.y = math.min(x.y, y.z);
		x.y = math.min(x.y, x.z);
		return math.sqrt(x.xy);
	}

	public static float cnoise(float2 P)
	{
		float4 x = math.floor(P.xyxy) + math.float4(0f, 0f, 1f, 1f);
		float4 float5 = math.frac(P.xyxy) - math.float4(0f, 0f, 1f, 1f);
		x = mod289(x);
		float4 xzxz = x.xzxz;
		float4 yyww = x.yyww;
		float4 xzxz2 = float5.xzxz;
		float4 yyww2 = float5.yyww;
		float4 obj = math.frac(permute(permute(xzxz) + yyww) * (1f / 41f)) * 2f - 1f;
		float4 float6 = math.abs(obj) - 0.5f;
		float4 float7 = math.floor(obj + 0.5f);
		float4 obj2 = obj - float7;
		float2 float8 = math.float2(obj2.x, float6.x);
		float2 float9 = math.float2(obj2.y, float6.y);
		float2 float10 = math.float2(obj2.z, float6.z);
		float2 float11 = math.float2(obj2.w, float6.w);
		float4 float12 = taylorInvSqrt(math.float4(math.dot(float8, float8), math.dot(float10, float10), math.dot(float9, float9), math.dot(float11, float11)));
		float8 *= float12.x;
		float10 *= float12.y;
		float9 *= float12.z;
		float11 *= float12.w;
		float x2 = math.dot(float8, math.float2(xzxz2.x, yyww2.x));
		float x3 = math.dot(float9, math.float2(xzxz2.y, yyww2.y));
		float y = math.dot(float10, math.float2(xzxz2.z, yyww2.z));
		float y2 = math.dot(float11, math.float2(xzxz2.w, yyww2.w));
		float2 float13 = fade(float5.xy);
		float2 float14 = math.lerp(math.float2(x2, y), math.float2(x3, y2), float13.x);
		float num = math.lerp(float14.x, float14.y, float13.y);
		return 2.3f * num;
	}

	public static float pnoise(float2 P, float2 rep)
	{
		float4 x = math.floor(P.xyxy) + math.float4(0f, 0f, 1f, 1f);
		float4 float5 = math.frac(P.xyxy) - math.float4(0f, 0f, 1f, 1f);
		x = math.fmod(x, rep.xyxy);
		x = mod289(x);
		float4 xzxz = x.xzxz;
		float4 yyww = x.yyww;
		float4 xzxz2 = float5.xzxz;
		float4 yyww2 = float5.yyww;
		float4 obj = math.frac(permute(permute(xzxz) + yyww) * (1f / 41f)) * 2f - 1f;
		float4 float6 = math.abs(obj) - 0.5f;
		float4 float7 = math.floor(obj + 0.5f);
		float4 obj2 = obj - float7;
		float2 float8 = math.float2(obj2.x, float6.x);
		float2 float9 = math.float2(obj2.y, float6.y);
		float2 float10 = math.float2(obj2.z, float6.z);
		float2 float11 = math.float2(obj2.w, float6.w);
		float4 float12 = taylorInvSqrt(math.float4(math.dot(float8, float8), math.dot(float10, float10), math.dot(float9, float9), math.dot(float11, float11)));
		float8 *= float12.x;
		float10 *= float12.y;
		float9 *= float12.z;
		float11 *= float12.w;
		float x2 = math.dot(float8, math.float2(xzxz2.x, yyww2.x));
		float x3 = math.dot(float9, math.float2(xzxz2.y, yyww2.y));
		float y = math.dot(float10, math.float2(xzxz2.z, yyww2.z));
		float y2 = math.dot(float11, math.float2(xzxz2.w, yyww2.w));
		float2 float13 = fade(float5.xy);
		float2 float14 = math.lerp(math.float2(x2, y), math.float2(x3, y2), float13.x);
		float num = math.lerp(float14.x, float14.y, float13.y);
		return 2.3f * num;
	}

	public static float cnoise(float3 P)
	{
		float3 float5 = math.floor(P);
		float3 x = float5 + math.float3(1f);
		float5 = mod289(float5);
		x = mod289(x);
		float3 float6 = math.frac(P);
		float3 y = float6 - math.float3(1f);
		float4 x2 = math.float4(float5.x, x.x, float5.x, x.x);
		float4 float7 = math.float4(float5.yy, x.yy);
		float4 zzzz = float5.zzzz;
		float4 zzzz2 = x.zzzz;
		float4 obj = permute(permute(x2) + float7);
		float4 float8 = permute(obj + zzzz);
		float4 obj2 = permute(obj + zzzz2);
		float4 x3 = float8 * (1f / 7f);
		float4 x4 = math.frac(math.floor(x3) * (1f / 7f)) - 0.5f;
		x3 = math.frac(x3);
		float4 y2 = math.float4(0.5f) - math.abs(x3) - math.abs(x4);
		float4 float9 = math.step(y2, math.float4(0f));
		x3 -= float9 * (math.step(0f, x3) - 0.5f);
		x4 -= float9 * (math.step(0f, x4) - 0.5f);
		float4 x5 = obj2 * (1f / 7f);
		float4 x6 = math.frac(math.floor(x5) * (1f / 7f)) - 0.5f;
		x5 = math.frac(x5);
		float4 y3 = math.float4(0.5f) - math.abs(x5) - math.abs(x6);
		float4 float10 = math.step(y3, math.float4(0f));
		x5 -= float10 * (math.step(0f, x5) - 0.5f);
		x6 -= float10 * (math.step(0f, x6) - 0.5f);
		float3 float11 = math.float3(x3.x, x4.x, y2.x);
		float3 float12 = math.float3(x3.y, x4.y, y2.y);
		float3 float13 = math.float3(x3.z, x4.z, y2.z);
		float3 float14 = math.float3(x3.w, x4.w, y2.w);
		float3 float15 = math.float3(x5.x, x6.x, y3.x);
		float3 float16 = math.float3(x5.y, x6.y, y3.y);
		float3 float17 = math.float3(x5.z, x6.z, y3.z);
		float3 float18 = math.float3(x5.w, x6.w, y3.w);
		float4 float19 = taylorInvSqrt(math.float4(math.dot(float11, float11), math.dot(float13, float13), math.dot(float12, float12), math.dot(float14, float14)));
		float11 *= float19.x;
		float13 *= float19.y;
		float12 *= float19.z;
		float14 *= float19.w;
		float4 float20 = taylorInvSqrt(math.float4(math.dot(float15, float15), math.dot(float17, float17), math.dot(float16, float16), math.dot(float18, float18)));
		float15 *= float20.x;
		float17 *= float20.y;
		float16 *= float20.z;
		float18 *= float20.w;
		float x7 = math.dot(float11, float6);
		float y4 = math.dot(float12, math.float3(y.x, float6.yz));
		float z = math.dot(float13, math.float3(float6.x, y.y, float6.z));
		float w = math.dot(float14, math.float3(y.xy, float6.z));
		float x8 = math.dot(float15, math.float3(float6.xy, y.z));
		float y5 = math.dot(float16, math.float3(y.x, float6.y, y.z));
		float z2 = math.dot(float17, math.float3(float6.x, y.yz));
		float w2 = math.dot(float18, y);
		float3 float21 = fade(float6);
		float4 float22 = math.lerp(math.float4(x7, y4, z, w), math.float4(x8, y5, z2, w2), float21.z);
		float2 float23 = math.lerp(float22.xy, float22.zw, float21.y);
		float num = math.lerp(float23.x, float23.y, float21.x);
		return 2.2f * num;
	}

	public static float pnoise(float3 P, float3 rep)
	{
		float3 float5 = math.fmod(math.floor(P), rep);
		float3 x = math.fmod(float5 + math.float3(1f), rep);
		float5 = mod289(float5);
		x = mod289(x);
		float3 float6 = math.frac(P);
		float3 y = float6 - math.float3(1f);
		float4 x2 = math.float4(float5.x, x.x, float5.x, x.x);
		float4 float7 = math.float4(float5.yy, x.yy);
		float4 zzzz = float5.zzzz;
		float4 zzzz2 = x.zzzz;
		float4 obj = permute(permute(x2) + float7);
		float4 float8 = permute(obj + zzzz);
		float4 obj2 = permute(obj + zzzz2);
		float4 x3 = float8 * (1f / 7f);
		float4 x4 = math.frac(math.floor(x3) * (1f / 7f)) - 0.5f;
		x3 = math.frac(x3);
		float4 y2 = math.float4(0.5f) - math.abs(x3) - math.abs(x4);
		float4 float9 = math.step(y2, math.float4(0f));
		x3 -= float9 * (math.step(0f, x3) - 0.5f);
		x4 -= float9 * (math.step(0f, x4) - 0.5f);
		float4 x5 = obj2 * (1f / 7f);
		float4 x6 = math.frac(math.floor(x5) * (1f / 7f)) - 0.5f;
		x5 = math.frac(x5);
		float4 y3 = math.float4(0.5f) - math.abs(x5) - math.abs(x6);
		float4 float10 = math.step(y3, math.float4(0f));
		x5 -= float10 * (math.step(0f, x5) - 0.5f);
		x6 -= float10 * (math.step(0f, x6) - 0.5f);
		float3 float11 = math.float3(x3.x, x4.x, y2.x);
		float3 float12 = math.float3(x3.y, x4.y, y2.y);
		float3 float13 = math.float3(x3.z, x4.z, y2.z);
		float3 float14 = math.float3(x3.w, x4.w, y2.w);
		float3 float15 = math.float3(x5.x, x6.x, y3.x);
		float3 float16 = math.float3(x5.y, x6.y, y3.y);
		float3 float17 = math.float3(x5.z, x6.z, y3.z);
		float3 float18 = math.float3(x5.w, x6.w, y3.w);
		float4 float19 = taylorInvSqrt(math.float4(math.dot(float11, float11), math.dot(float13, float13), math.dot(float12, float12), math.dot(float14, float14)));
		float11 *= float19.x;
		float13 *= float19.y;
		float12 *= float19.z;
		float14 *= float19.w;
		float4 float20 = taylorInvSqrt(math.float4(math.dot(float15, float15), math.dot(float17, float17), math.dot(float16, float16), math.dot(float18, float18)));
		float15 *= float20.x;
		float17 *= float20.y;
		float16 *= float20.z;
		float18 *= float20.w;
		float x7 = math.dot(float11, float6);
		float y4 = math.dot(float12, math.float3(y.x, float6.yz));
		float z = math.dot(float13, math.float3(float6.x, y.y, float6.z));
		float w = math.dot(float14, math.float3(y.xy, float6.z));
		float x8 = math.dot(float15, math.float3(float6.xy, y.z));
		float y5 = math.dot(float16, math.float3(y.x, float6.y, y.z));
		float z2 = math.dot(float17, math.float3(float6.x, y.yz));
		float w2 = math.dot(float18, y);
		float3 float21 = fade(float6);
		float4 float22 = math.lerp(math.float4(x7, y4, z, w), math.float4(x8, y5, z2, w2), float21.z);
		float2 float23 = math.lerp(float22.xy, float22.zw, float21.y);
		float num = math.lerp(float23.x, float23.y, float21.x);
		return 2.2f * num;
	}

	public static float cnoise(float4 P)
	{
		float4 float5 = math.floor(P);
		float4 x = float5 + 1f;
		float5 = mod289(float5);
		x = mod289(x);
		float4 float6 = math.frac(P);
		float4 y = float6 - 1f;
		float4 x2 = math.float4(float5.x, x.x, float5.x, x.x);
		float4 float7 = math.float4(float5.yy, x.yy);
		float4 float8 = math.float4(float5.zzzz);
		float4 float9 = math.float4(x.zzzz);
		float4 float10 = math.float4(float5.wwww);
		float4 float11 = math.float4(x.wwww);
		float4 obj = permute(permute(x2) + float7);
		float4 float12 = permute(obj + float8);
		float4 obj2 = permute(obj + float9);
		float4 float13 = permute(float12 + float10);
		float4 float14 = permute(float12 + float11);
		float4 float15 = permute(obj2 + float10);
		float4 obj3 = permute(obj2 + float11);
		float4 x3 = float13 * (1f / 7f);
		float4 x4 = math.floor(x3) * (1f / 7f);
		float4 x5 = math.floor(x4) * (1f / 6f);
		x3 = math.frac(x3) - 0.5f;
		x4 = math.frac(x4) - 0.5f;
		x5 = math.frac(x5) - 0.5f;
		float4 y2 = math.float4(0.75f) - math.abs(x3) - math.abs(x4) - math.abs(x5);
		float4 float16 = math.step(y2, math.float4(0f));
		x3 -= float16 * (math.step(0f, x3) - 0.5f);
		x4 -= float16 * (math.step(0f, x4) - 0.5f);
		float4 x6 = float14 * (1f / 7f);
		float4 x7 = math.floor(x6) * (1f / 7f);
		float4 x8 = math.floor(x7) * (1f / 6f);
		x6 = math.frac(x6) - 0.5f;
		x7 = math.frac(x7) - 0.5f;
		x8 = math.frac(x8) - 0.5f;
		float4 y3 = math.float4(0.75f) - math.abs(x6) - math.abs(x7) - math.abs(x8);
		float4 float17 = math.step(y3, math.float4(0f));
		x6 -= float17 * (math.step(0f, x6) - 0.5f);
		x7 -= float17 * (math.step(0f, x7) - 0.5f);
		float4 x9 = float15 * (1f / 7f);
		float4 x10 = math.floor(x9) * (1f / 7f);
		float4 x11 = math.floor(x10) * (1f / 6f);
		x9 = math.frac(x9) - 0.5f;
		x10 = math.frac(x10) - 0.5f;
		x11 = math.frac(x11) - 0.5f;
		float4 y4 = math.float4(0.75f) - math.abs(x9) - math.abs(x10) - math.abs(x11);
		float4 float18 = math.step(y4, math.float4(0f));
		x9 -= float18 * (math.step(0f, x9) - 0.5f);
		x10 -= float18 * (math.step(0f, x10) - 0.5f);
		float4 x12 = obj3 * (1f / 7f);
		float4 x13 = math.floor(x12) * (1f / 7f);
		float4 x14 = math.floor(x13) * (1f / 6f);
		x12 = math.frac(x12) - 0.5f;
		x13 = math.frac(x13) - 0.5f;
		x14 = math.frac(x14) - 0.5f;
		float4 y5 = math.float4(0.75f) - math.abs(x12) - math.abs(x13) - math.abs(x14);
		float4 float19 = math.step(y5, math.float4(0f));
		x12 -= float19 * (math.step(0f, x12) - 0.5f);
		x13 -= float19 * (math.step(0f, x13) - 0.5f);
		float4 float20 = math.float4(x3.x, x4.x, x5.x, y2.x);
		float4 float21 = math.float4(x3.y, x4.y, x5.y, y2.y);
		float4 float22 = math.float4(x3.z, x4.z, x5.z, y2.z);
		float4 float23 = math.float4(x3.w, x4.w, x5.w, y2.w);
		float4 float24 = math.float4(x9.x, x10.x, x11.x, y4.x);
		float4 float25 = math.float4(x9.y, x10.y, x11.y, y4.y);
		float4 float26 = math.float4(x9.z, x10.z, x11.z, y4.z);
		float4 float27 = math.float4(x9.w, x10.w, x11.w, y4.w);
		float4 obj4 = math.float4(x6.x, x7.x, x8.x, y3.x);
		float4 float28 = math.float4(x6.y, x7.y, x8.y, y3.y);
		float4 float29 = math.float4(x6.z, x7.z, x8.z, y3.z);
		float4 float30 = math.float4(x6.w, x7.w, x8.w, y3.w);
		float4 float31 = math.float4(x12.x, x13.x, x14.x, y5.x);
		float4 float32 = math.float4(x12.y, x13.y, x14.y, y5.y);
		float4 float33 = math.float4(x12.z, x13.z, x14.z, y5.z);
		float4 float34 = math.float4(x12.w, x13.w, x14.w, y5.w);
		float4 float35 = taylorInvSqrt(math.float4(math.dot(float20, float20), math.dot(float22, float22), math.dot(float21, float21), math.dot(float23, float23)));
		float20 *= float35.x;
		float22 *= float35.y;
		float21 *= float35.z;
		float23 *= float35.w;
		float4 float36 = taylorInvSqrt(math.float4(math.dot(obj4, obj4), math.dot(float29, float29), math.dot(float28, float28), math.dot(float30, float30)));
		float4 x15 = obj4 * float36.x;
		float29 *= float36.y;
		float28 *= float36.z;
		float30 *= float36.w;
		float4 float37 = taylorInvSqrt(math.float4(math.dot(float24, float24), math.dot(float26, float26), math.dot(float25, float25), math.dot(float27, float27)));
		float24 *= float37.x;
		float26 *= float37.y;
		float25 *= float37.z;
		float27 *= float37.w;
		float4 float38 = taylorInvSqrt(math.float4(math.dot(float31, float31), math.dot(float33, float33), math.dot(float32, float32), math.dot(float34, float34)));
		float31 *= float38.x;
		float33 *= float38.y;
		float32 *= float38.z;
		float34 *= float38.w;
		float x16 = math.dot(float20, float6);
		float y6 = math.dot(float21, math.float4(y.x, float6.yzw));
		float z = math.dot(float22, math.float4(float6.x, y.y, float6.zw));
		float w = math.dot(float23, math.float4(y.xy, float6.zw));
		float x17 = math.dot(float24, math.float4(float6.xy, y.z, float6.w));
		float y7 = math.dot(float25, math.float4(y.x, float6.y, y.z, float6.w));
		float z2 = math.dot(float26, math.float4(float6.x, y.yz, float6.w));
		float w2 = math.dot(float27, math.float4(y.xyz, float6.w));
		float x18 = math.dot(x15, math.float4(float6.xyz, y.w));
		float y8 = math.dot(float28, math.float4(y.x, float6.yz, y.w));
		float z3 = math.dot(float29, math.float4(float6.x, y.y, float6.z, y.w));
		float w3 = math.dot(float30, math.float4(y.xy, float6.z, y.w));
		float x19 = math.dot(float31, math.float4(float6.xy, y.zw));
		float y9 = math.dot(float32, math.float4(y.x, float6.y, y.zw));
		float z4 = math.dot(float33, math.float4(float6.x, y.yzw));
		float w4 = math.dot(float34, y);
		float4 float39 = fade(float6);
		float4 x20 = math.lerp(math.float4(x16, y6, z, w), math.float4(x18, y8, z3, w3), float39.w);
		float4 y10 = math.lerp(math.float4(x17, y7, z2, w2), math.float4(x19, y9, z4, w4), float39.w);
		float4 float40 = math.lerp(x20, y10, float39.z);
		float2 float41 = math.lerp(float40.xy, float40.zw, float39.y);
		float num = math.lerp(float41.x, float41.y, float39.x);
		return 2.2f * num;
	}

	public static float pnoise(float4 P, float4 rep)
	{
		float4 float5 = math.fmod(math.floor(P), rep);
		float4 x = math.fmod(float5 + 1f, rep);
		float5 = mod289(float5);
		x = mod289(x);
		float4 float6 = math.frac(P);
		float4 y = float6 - 1f;
		float4 x2 = math.float4(float5.x, x.x, float5.x, x.x);
		float4 float7 = math.float4(float5.yy, x.yy);
		float4 float8 = math.float4(float5.zzzz);
		float4 float9 = math.float4(x.zzzz);
		float4 float10 = math.float4(float5.wwww);
		float4 float11 = math.float4(x.wwww);
		float4 obj = permute(permute(x2) + float7);
		float4 float12 = permute(obj + float8);
		float4 obj2 = permute(obj + float9);
		float4 float13 = permute(float12 + float10);
		float4 float14 = permute(float12 + float11);
		float4 float15 = permute(obj2 + float10);
		float4 obj3 = permute(obj2 + float11);
		float4 x3 = float13 * (1f / 7f);
		float4 x4 = math.floor(x3) * (1f / 7f);
		float4 x5 = math.floor(x4) * (1f / 6f);
		x3 = math.frac(x3) - 0.5f;
		x4 = math.frac(x4) - 0.5f;
		x5 = math.frac(x5) - 0.5f;
		float4 y2 = math.float4(0.75f) - math.abs(x3) - math.abs(x4) - math.abs(x5);
		float4 float16 = math.step(y2, math.float4(0f));
		x3 -= float16 * (math.step(0f, x3) - 0.5f);
		x4 -= float16 * (math.step(0f, x4) - 0.5f);
		float4 x6 = float14 * (1f / 7f);
		float4 x7 = math.floor(x6) * (1f / 7f);
		float4 x8 = math.floor(x7) * (1f / 6f);
		x6 = math.frac(x6) - 0.5f;
		x7 = math.frac(x7) - 0.5f;
		x8 = math.frac(x8) - 0.5f;
		float4 y3 = math.float4(0.75f) - math.abs(x6) - math.abs(x7) - math.abs(x8);
		float4 float17 = math.step(y3, math.float4(0f));
		x6 -= float17 * (math.step(0f, x6) - 0.5f);
		x7 -= float17 * (math.step(0f, x7) - 0.5f);
		float4 x9 = float15 * (1f / 7f);
		float4 x10 = math.floor(x9) * (1f / 7f);
		float4 x11 = math.floor(x10) * (1f / 6f);
		x9 = math.frac(x9) - 0.5f;
		x10 = math.frac(x10) - 0.5f;
		x11 = math.frac(x11) - 0.5f;
		float4 y4 = math.float4(0.75f) - math.abs(x9) - math.abs(x10) - math.abs(x11);
		float4 float18 = math.step(y4, math.float4(0f));
		x9 -= float18 * (math.step(0f, x9) - 0.5f);
		x10 -= float18 * (math.step(0f, x10) - 0.5f);
		float4 x12 = obj3 * (1f / 7f);
		float4 x13 = math.floor(x12) * (1f / 7f);
		float4 x14 = math.floor(x13) * (1f / 6f);
		x12 = math.frac(x12) - 0.5f;
		x13 = math.frac(x13) - 0.5f;
		x14 = math.frac(x14) - 0.5f;
		float4 y5 = math.float4(0.75f) - math.abs(x12) - math.abs(x13) - math.abs(x14);
		float4 float19 = math.step(y5, math.float4(0f));
		x12 -= float19 * (math.step(0f, x12) - 0.5f);
		x13 -= float19 * (math.step(0f, x13) - 0.5f);
		float4 float20 = math.float4(x3.x, x4.x, x5.x, y2.x);
		float4 float21 = math.float4(x3.y, x4.y, x5.y, y2.y);
		float4 float22 = math.float4(x3.z, x4.z, x5.z, y2.z);
		float4 float23 = math.float4(x3.w, x4.w, x5.w, y2.w);
		float4 float24 = math.float4(x9.x, x10.x, x11.x, y4.x);
		float4 float25 = math.float4(x9.y, x10.y, x11.y, y4.y);
		float4 float26 = math.float4(x9.z, x10.z, x11.z, y4.z);
		float4 float27 = math.float4(x9.w, x10.w, x11.w, y4.w);
		float4 obj4 = math.float4(x6.x, x7.x, x8.x, y3.x);
		float4 float28 = math.float4(x6.y, x7.y, x8.y, y3.y);
		float4 float29 = math.float4(x6.z, x7.z, x8.z, y3.z);
		float4 float30 = math.float4(x6.w, x7.w, x8.w, y3.w);
		float4 float31 = math.float4(x12.x, x13.x, x14.x, y5.x);
		float4 float32 = math.float4(x12.y, x13.y, x14.y, y5.y);
		float4 float33 = math.float4(x12.z, x13.z, x14.z, y5.z);
		float4 float34 = math.float4(x12.w, x13.w, x14.w, y5.w);
		float4 float35 = taylorInvSqrt(math.float4(math.dot(float20, float20), math.dot(float22, float22), math.dot(float21, float21), math.dot(float23, float23)));
		float20 *= float35.x;
		float22 *= float35.y;
		float21 *= float35.z;
		float23 *= float35.w;
		float4 float36 = taylorInvSqrt(math.float4(math.dot(obj4, obj4), math.dot(float29, float29), math.dot(float28, float28), math.dot(float30, float30)));
		float4 x15 = obj4 * float36.x;
		float29 *= float36.y;
		float28 *= float36.z;
		float30 *= float36.w;
		float4 float37 = taylorInvSqrt(math.float4(math.dot(float24, float24), math.dot(float26, float26), math.dot(float25, float25), math.dot(float27, float27)));
		float24 *= float37.x;
		float26 *= float37.y;
		float25 *= float37.z;
		float27 *= float37.w;
		float4 float38 = taylorInvSqrt(math.float4(math.dot(float31, float31), math.dot(float33, float33), math.dot(float32, float32), math.dot(float34, float34)));
		float31 *= float38.x;
		float33 *= float38.y;
		float32 *= float38.z;
		float34 *= float38.w;
		float x16 = math.dot(float20, float6);
		float y6 = math.dot(float21, math.float4(y.x, float6.yzw));
		float z = math.dot(float22, math.float4(float6.x, y.y, float6.zw));
		float w = math.dot(float23, math.float4(y.xy, float6.zw));
		float x17 = math.dot(float24, math.float4(float6.xy, y.z, float6.w));
		float y7 = math.dot(float25, math.float4(y.x, float6.y, y.z, float6.w));
		float z2 = math.dot(float26, math.float4(float6.x, y.yz, float6.w));
		float w2 = math.dot(float27, math.float4(y.xyz, float6.w));
		float x18 = math.dot(x15, math.float4(float6.xyz, y.w));
		float y8 = math.dot(float28, math.float4(y.x, float6.yz, y.w));
		float z3 = math.dot(float29, math.float4(float6.x, y.y, float6.z, y.w));
		float w3 = math.dot(float30, math.float4(y.xy, float6.z, y.w));
		float x19 = math.dot(float31, math.float4(float6.xy, y.zw));
		float y9 = math.dot(float32, math.float4(y.x, float6.y, y.zw));
		float z4 = math.dot(float33, math.float4(float6.x, y.yzw));
		float w4 = math.dot(float34, y);
		float4 float39 = fade(float6);
		float4 x20 = math.lerp(math.float4(x16, y6, z, w), math.float4(x18, y8, z3, w3), float39.w);
		float4 y10 = math.lerp(math.float4(x17, y7, z2, w2), math.float4(x19, y9, z4, w4), float39.w);
		float4 float40 = math.lerp(x20, y10, float39.z);
		float2 float41 = math.lerp(float40.xy, float40.zw, float39.y);
		float num = math.lerp(float41.x, float41.y, float39.x);
		return 2.2f * num;
	}

	private static float mod289(float x)
	{
		return x - math.floor(x * 0.0034602077f) * 289f;
	}

	private static float2 mod289(float2 x)
	{
		return x - math.floor(x * 0.0034602077f) * 289f;
	}

	private static float3 mod289(float3 x)
	{
		return x - math.floor(x * 0.0034602077f) * 289f;
	}

	private static float4 mod289(float4 x)
	{
		return x - math.floor(x * 0.0034602077f) * 289f;
	}

	private static float3 mod7(float3 x)
	{
		return x - math.floor(x * (1f / 7f)) * 7f;
	}

	private static float4 mod7(float4 x)
	{
		return x - math.floor(x * (1f / 7f)) * 7f;
	}

	private static float permute(float x)
	{
		return mod289((34f * x + 1f) * x);
	}

	private static float3 permute(float3 x)
	{
		return mod289((34f * x + 1f) * x);
	}

	private static float4 permute(float4 x)
	{
		return mod289((34f * x + 1f) * x);
	}

	private static float taylorInvSqrt(float r)
	{
		return 1.7928429f - 0.85373473f * r;
	}

	private static float4 taylorInvSqrt(float4 r)
	{
		return 1.7928429f - 0.85373473f * r;
	}

	private static float2 fade(float2 t)
	{
		return t * t * t * (t * (t * 6f - 15f) + 10f);
	}

	private static float3 fade(float3 t)
	{
		return t * t * t * (t * (t * 6f - 15f) + 10f);
	}

	private static float4 fade(float4 t)
	{
		return t * t * t * (t * (t * 6f - 15f) + 10f);
	}

	private static float4 grad4(float j, float4 ip)
	{
		float4 float5 = math.float4(1f, 1f, 1f, -1f);
		float3 float6 = math.floor(math.frac(math.float3(j) * ip.xyz) * 7f) * ip.z - 1f;
		float w = 1.5f - math.dot(math.abs(float6), float5.xyz);
		float4 float7 = math.float4(float6, w);
		float4 float8 = math.float4(float7 < 0f);
		float7.xyz += (float8.xyz * 2f - 1f) * float8.www;
		return float7;
	}

	private static float2 rgrad2(float2 p, float rot)
	{
		float x = permute(permute(p.x) + p.y) * (1f / 41f) + rot;
		x = math.frac(x) * (MathF.PI * 2f);
		return math.float2(math.cos(x), math.sin(x));
	}

	public static float snoise(float2 v)
	{
		float4 float5 = math.float4(0.21132487f, 0.36602542f, -0.57735026f, 1f / 41f);
		float2 float6 = math.floor(v + math.dot(v, float5.yy));
		float2 float7 = v - float6 + math.dot(float6, float5.xx);
		float2 float8 = ((float7.x > float7.y) ? math.float2(1f, 0f) : math.float2(0f, 1f));
		float4 float9 = float7.xyxy + float5.xxzz;
		float9.xy -= float8;
		float6 = mod289(float6);
		float3 float10 = permute(permute(float6.y + math.float3(0f, float8.y, 1f)) + float6.x + math.float3(0f, float8.x, 1f));
		float3 float11 = math.max(0.5f - math.float3(math.dot(float7, float7), math.dot(float9.xy, float9.xy), math.dot(float9.zw, float9.zw)), 0f);
		float11 *= float11;
		float11 *= float11;
		float3 obj = 2f * math.frac(float10 * float5.www) - 1f;
		float3 float12 = math.abs(obj) - 0.5f;
		float3 float13 = math.floor(obj + 0.5f);
		float3 float14 = obj - float13;
		float11 *= 1.7928429f - 0.85373473f * (float14 * float14 + float12 * float12);
		float x = float14.x * float7.x + float12.x * float7.y;
		float2 yz = float14.yz * float9.xz + float12.yz * float9.yw;
		float3 y = math.float3(x, yz);
		return 130f * math.dot(float11, y);
	}

	public static float snoise(float3 v)
	{
		float2 float5 = math.float2(1f / 6f, 1f / 3f);
		float4 float6 = math.float4(0f, 0.5f, 1f, 2f);
		float3 float7 = math.floor(v + math.dot(v, float5.yyy));
		float3 float8 = v - float7 + math.dot(float7, float5.xxx);
		float3 float9 = math.step(float8.yzx, float8.xyz);
		float3 float10 = 1f - float9;
		float3 float11 = math.min(float9.xyz, float10.zxy);
		float3 float12 = math.max(float9.xyz, float10.zxy);
		float3 float13 = float8 - float11 + float5.xxx;
		float3 float14 = float8 - float12 + float5.yyy;
		float3 float15 = float8 - float6.yyy;
		float7 = mod289(float7);
		float4 float16 = permute(permute(permute(float7.z + math.float4(0f, float11.z, float12.z, 1f)) + float7.y + math.float4(0f, float11.y, float12.y, 1f)) + float7.x + math.float4(0f, float11.x, float12.x, 1f));
		float3 float17 = 1f / 7f * float6.wyz - float6.xzx;
		float4 obj = float16 - 49f * math.floor(float16 * float17.z * float17.z);
		float4 float18 = math.floor(obj * float17.z);
		float4 obj2 = math.floor(obj - 7f * float18);
		float4 x = float18 * float17.x + float17.yyyy;
		float4 x2 = obj2 * float17.x + float17.yyyy;
		float4 y = 1f - math.abs(x) - math.abs(x2);
		float4 x3 = math.float4(x.xy, x2.xy);
		float4 x4 = math.float4(x.zw, x2.zw);
		float4 float19 = math.floor(x3) * 2f + 1f;
		float4 float20 = math.floor(x4) * 2f + 1f;
		float4 float21 = -math.step(y, math.float4(0f));
		float4 float22 = x3.xzyw + float19.xzyw * float21.xxyy;
		float4 float23 = x4.xzyw + float20.xzyw * float21.zzww;
		float3 float24 = math.float3(float22.xy, y.x);
		float3 float25 = math.float3(float22.zw, y.y);
		float3 float26 = math.float3(float23.xy, y.z);
		float3 float27 = math.float3(float23.zw, y.w);
		float4 float28 = taylorInvSqrt(math.float4(math.dot(float24, float24), math.dot(float25, float25), math.dot(float26, float26), math.dot(float27, float27)));
		float24 *= float28.x;
		float25 *= float28.y;
		float26 *= float28.z;
		float27 *= float28.w;
		float4 float29 = math.max(0.6f - math.float4(math.dot(float8, float8), math.dot(float13, float13), math.dot(float14, float14), math.dot(float15, float15)), 0f);
		float29 *= float29;
		return 42f * math.dot(float29 * float29, math.float4(math.dot(float24, float8), math.dot(float25, float13), math.dot(float26, float14), math.dot(float27, float15)));
	}

	public static float snoise(float3 v, out float3 gradient)
	{
		float2 float5 = math.float2(1f / 6f, 1f / 3f);
		float4 float6 = math.float4(0f, 0.5f, 1f, 2f);
		float3 float7 = math.floor(v + math.dot(v, float5.yyy));
		float3 float8 = v - float7 + math.dot(float7, float5.xxx);
		float3 float9 = math.step(float8.yzx, float8.xyz);
		float3 float10 = 1f - float9;
		float3 float11 = math.min(float9.xyz, float10.zxy);
		float3 float12 = math.max(float9.xyz, float10.zxy);
		float3 float13 = float8 - float11 + float5.xxx;
		float3 float14 = float8 - float12 + float5.yyy;
		float3 float15 = float8 - float6.yyy;
		float7 = mod289(float7);
		float4 float16 = permute(permute(permute(float7.z + math.float4(0f, float11.z, float12.z, 1f)) + float7.y + math.float4(0f, float11.y, float12.y, 1f)) + float7.x + math.float4(0f, float11.x, float12.x, 1f));
		float3 float17 = 1f / 7f * float6.wyz - float6.xzx;
		float4 obj = float16 - 49f * math.floor(float16 * float17.z * float17.z);
		float4 float18 = math.floor(obj * float17.z);
		float4 obj2 = math.floor(obj - 7f * float18);
		float4 x = float18 * float17.x + float17.yyyy;
		float4 x2 = obj2 * float17.x + float17.yyyy;
		float4 y = 1f - math.abs(x) - math.abs(x2);
		float4 x3 = math.float4(x.xy, x2.xy);
		float4 x4 = math.float4(x.zw, x2.zw);
		float4 float19 = math.floor(x3) * 2f + 1f;
		float4 float20 = math.floor(x4) * 2f + 1f;
		float4 float21 = -math.step(y, math.float4(0f));
		float4 float22 = x3.xzyw + float19.xzyw * float21.xxyy;
		float4 float23 = x4.xzyw + float20.xzyw * float21.zzww;
		float3 float24 = math.float3(float22.xy, y.x);
		float3 float25 = math.float3(float22.zw, y.y);
		float3 float26 = math.float3(float23.xy, y.z);
		float3 float27 = math.float3(float23.zw, y.w);
		float4 float28 = taylorInvSqrt(math.float4(math.dot(float24, float24), math.dot(float25, float25), math.dot(float26, float26), math.dot(float27, float27)));
		float24 *= float28.x;
		float25 *= float28.y;
		float26 *= float28.z;
		float27 *= float28.w;
		float4 float29 = math.max(0.6f - math.float4(math.dot(float8, float8), math.dot(float13, float13), math.dot(float14, float14), math.dot(float15, float15)), 0f);
		float4 obj3 = float29 * float29;
		float4 x5 = obj3 * obj3;
		float4 float30 = math.float4(math.dot(float24, float8), math.dot(float25, float13), math.dot(float26, float14), math.dot(float27, float15));
		float4 float31 = obj3 * float29 * float30;
		gradient = -8f * (float31.x * float8 + float31.y * float13 + float31.z * float14 + float31.w * float15);
		gradient += x5.x * float24 + x5.y * float25 + x5.z * float26 + x5.w * float27;
		gradient *= 42f;
		return 42f * math.dot(x5, float30);
	}

	public static float snoise(float4 v)
	{
		float4 float5 = math.float4(0.1381966f, 0.2763932f, 0.4145898f, -0.4472136f);
		float4 float6 = math.floor(v + math.dot(v, math.float4(0.309017f)));
		float4 float7 = v - float6 + math.dot(float6, float5.xxxx);
		float4 float8 = math.float4(0f);
		float3 float9 = math.step(float7.yzw, float7.xxx);
		float3 float10 = math.step(float7.zww, float7.yyz);
		float8.x = float9.x + float9.y + float9.z;
		float8.yzw = 1f - float9;
		float8.y += float10.x + float10.y;
		float8.zw += 1f - float10.xy;
		float8.z += float10.z;
		float8.w += 1f - float10.z;
		float4 float11 = math.clamp(float8, 0f, 1f);
		float4 float12 = math.clamp(float8 - 1f, 0f, 1f);
		float4 float13 = math.clamp(float8 - 2f, 0f, 1f);
		float4 float14 = float7 - float13 + float5.xxxx;
		float4 float15 = float7 - float12 + float5.yyyy;
		float4 float16 = float7 - float11 + float5.zzzz;
		float4 float17 = float7 + float5.wwww;
		float6 = mod289(float6);
		float j = permute(permute(permute(permute(float6.w) + float6.z) + float6.y) + float6.x);
		float4 obj = permute(permute(permute(permute(float6.w + math.float4(float13.w, float12.w, float11.w, 1f)) + float6.z + math.float4(float13.z, float12.z, float11.z, 1f)) + float6.y + math.float4(float13.y, float12.y, float11.y, 1f)) + float6.x + math.float4(float13.x, float12.x, float11.x, 1f));
		float4 ip = math.float4(0.0034013605f, 1f / 49f, 1f / 7f, 0f);
		float4 float18 = grad4(j, ip);
		float4 float19 = grad4(obj.x, ip);
		float4 float20 = grad4(obj.y, ip);
		float4 float21 = grad4(obj.z, ip);
		float4 float22 = grad4(obj.w, ip);
		float4 float23 = taylorInvSqrt(math.float4(math.dot(float18, float18), math.dot(float19, float19), math.dot(float20, float20), math.dot(float21, float21)));
		float18 *= float23.x;
		float19 *= float23.y;
		float20 *= float23.z;
		float21 *= float23.w;
		float22 *= taylorInvSqrt(math.dot(float22, float22));
		float3 float24 = math.max(0.6f - math.float3(math.dot(float7, float7), math.dot(float14, float14), math.dot(float15, float15)), 0f);
		float2 float25 = math.max(0.6f - math.float2(math.dot(float16, float16), math.dot(float17, float17)), 0f);
		float24 *= float24;
		float25 *= float25;
		return 49f * (math.dot(float24 * float24, math.float3(math.dot(float18, float7), math.dot(float19, float14), math.dot(float20, float15))) + math.dot(float25 * float25, math.float2(math.dot(float21, float16), math.dot(float22, float17))));
	}

	public static float3 psrdnoise(float2 pos, float2 per, float rot)
	{
		pos.y += 0.01f;
		float2 x = math.float2(pos.x + pos.y * 0.5f, pos.y);
		float2 float5 = math.floor(x);
		float2 float6 = math.frac(x);
		float2 float7 = ((float6.x > float6.y) ? math.float2(1f, 0f) : math.float2(0f, 1f));
		float2 float8 = math.float2(float5.x - float5.y * 0.5f, float5.y);
		float2 float9 = math.float2(float8.x + float7.x - float7.y * 0.5f, float8.y + float7.y);
		float2 float10 = math.float2(float8.x + 0.5f, float8.y + 1f);
		float2 float11 = pos - float8;
		float2 float12 = pos - float9;
		float2 float13 = pos - float10;
		float3 obj = math.fmod(math.float3(float8.x, float9.x, float10.x), per.x);
		float3 float14 = math.fmod(math.float3(float8.y, float9.y, float10.y), per.y);
		float3 obj2 = obj + 0.5f * float14;
		float3 float15 = float14;
		float2 float16 = rgrad2(math.float2(obj2.x, float15.x), rot);
		float2 float17 = rgrad2(math.float2(obj2.y, float15.y), rot);
		float2 float18 = rgrad2(math.float2(obj2.z, float15.z), rot);
		float3 y = math.float3(math.dot(float16, float11), math.dot(float17, float12), math.dot(float18, float13));
		float3 float19 = 0.8f - math.float3(math.dot(float11, float11), math.dot(float12, float12), math.dot(float13, float13));
		float3 float20 = -2f * math.float3(float11.x, float12.x, float13.x);
		float3 float21 = -2f * math.float3(float11.y, float12.y, float13.y);
		if (float19.x < 0f)
		{
			float20.x = 0f;
			float21.x = 0f;
			float19.x = 0f;
		}
		if (float19.y < 0f)
		{
			float20.y = 0f;
			float21.y = 0f;
			float19.y = 0f;
		}
		if (float19.z < 0f)
		{
			float20.z = 0f;
			float21.z = 0f;
			float19.z = 0f;
		}
		float3 obj3 = float19 * float19;
		float3 x2 = obj3 * obj3;
		float3 float22 = obj3 * float19;
		float x3 = math.dot(x2, y);
		float2 float23 = math.float2(float20.x, float21.x) * 4f * float22.x;
		float2 float24 = x2.x * float16 + float23 * y.x;
		float2 float25 = math.float2(float20.y, float21.y) * 4f * float22.y;
		float2 float26 = x2.y * float17 + float25 * y.y;
		float2 float27 = math.float2(float20.z, float21.z) * 4f * float22.z;
		float2 float28 = x2.z * float18 + float27 * y.z;
		return 11f * math.float3(x3, float24 + float26 + float28);
	}

	public static float3 psrdnoise(float2 pos, float2 per)
	{
		return psrdnoise(pos, per, 0f);
	}

	public static float psrnoise(float2 pos, float2 per, float rot)
	{
		pos.y += 0.001f;
		float2 x = math.float2(pos.x + pos.y * 0.5f, pos.y);
		float2 float5 = math.floor(x);
		float2 float6 = math.frac(x);
		float2 float7 = ((float6.x > float6.y) ? math.float2(1f, 0f) : math.float2(0f, 1f));
		float2 float8 = math.float2(float5.x - float5.y * 0.5f, float5.y);
		float2 float9 = math.float2(float8.x + float7.x - float7.y * 0.5f, float8.y + float7.y);
		float2 float10 = math.float2(float8.x + 0.5f, float8.y + 1f);
		float2 float11 = pos - float8;
		float2 float12 = pos - float9;
		float2 float13 = pos - float10;
		float3 obj = math.fmod(math.float3(float8.x, float9.x, float10.x), per.x);
		float3 float14 = math.fmod(math.float3(float8.y, float9.y, float10.y), per.y);
		float3 obj2 = obj + 0.5f * float14;
		float3 float15 = float14;
		float2 x2 = rgrad2(math.float2(obj2.x, float15.x), rot);
		float2 x3 = rgrad2(math.float2(obj2.y, float15.y), rot);
		float2 x4 = rgrad2(math.float2(obj2.z, float15.z), rot);
		float3 y = math.float3(math.dot(x2, float11), math.dot(x3, float12), math.dot(x4, float13));
		float3 obj3 = math.max(0.8f - math.float3(math.dot(float11, float11), math.dot(float12, float12), math.dot(float13, float13)), 0f);
		float3 obj4 = obj3 * obj3;
		float num = math.dot(obj4 * obj4, y);
		return 11f * num;
	}

	public static float psrnoise(float2 pos, float2 per)
	{
		return psrnoise(pos, per, 0f);
	}

	public static float3 srdnoise(float2 pos, float rot)
	{
		pos.y += 0.001f;
		float2 x = math.float2(pos.x + pos.y * 0.5f, pos.y);
		float2 float5 = math.floor(x);
		float2 float6 = math.frac(x);
		float2 float7 = ((float6.x > float6.y) ? math.float2(1f, 0f) : math.float2(0f, 1f));
		float2 float8 = math.float2(float5.x - float5.y * 0.5f, float5.y);
		float2 float9 = math.float2(float8.x + float7.x - float7.y * 0.5f, float8.y + float7.y);
		float2 float10 = math.float2(float8.x + 0.5f, float8.y + 1f);
		float2 float11 = pos - float8;
		float2 float12 = pos - float9;
		float2 float13 = pos - float10;
		float3 obj = math.float3(float8.x, float9.x, float10.x);
		float3 float14 = math.float3(float8.y, float9.y, float10.y);
		float3 x2 = obj + 0.5f * float14;
		float3 x3 = float14;
		float3 obj2 = mod289(x2);
		x3 = mod289(x3);
		float2 float15 = rgrad2(math.float2(obj2.x, x3.x), rot);
		float2 float16 = rgrad2(math.float2(obj2.y, x3.y), rot);
		float2 float17 = rgrad2(math.float2(obj2.z, x3.z), rot);
		float3 y = math.float3(math.dot(float15, float11), math.dot(float16, float12), math.dot(float17, float13));
		float3 float18 = 0.8f - math.float3(math.dot(float11, float11), math.dot(float12, float12), math.dot(float13, float13));
		float3 float19 = -2f * math.float3(float11.x, float12.x, float13.x);
		float3 float20 = -2f * math.float3(float11.y, float12.y, float13.y);
		if (float18.x < 0f)
		{
			float19.x = 0f;
			float20.x = 0f;
			float18.x = 0f;
		}
		if (float18.y < 0f)
		{
			float19.y = 0f;
			float20.y = 0f;
			float18.y = 0f;
		}
		if (float18.z < 0f)
		{
			float19.z = 0f;
			float20.z = 0f;
			float18.z = 0f;
		}
		float3 obj3 = float18 * float18;
		float3 x4 = obj3 * obj3;
		float3 float21 = obj3 * float18;
		float x5 = math.dot(x4, y);
		float2 float22 = math.float2(float19.x, float20.x) * 4f * float21.x;
		float2 float23 = x4.x * float15 + float22 * y.x;
		float2 float24 = math.float2(float19.y, float20.y) * 4f * float21.y;
		float2 float25 = x4.y * float16 + float24 * y.y;
		float2 float26 = math.float2(float19.z, float20.z) * 4f * float21.z;
		float2 float27 = x4.z * float17 + float26 * y.z;
		return 11f * math.float3(x5, float23 + float25 + float27);
	}

	public static float3 srdnoise(float2 pos)
	{
		return srdnoise(pos, 0f);
	}

	public static float srnoise(float2 pos, float rot)
	{
		pos.y += 0.001f;
		float2 x = math.float2(pos.x + pos.y * 0.5f, pos.y);
		float2 float5 = math.floor(x);
		float2 float6 = math.frac(x);
		float2 float7 = ((float6.x > float6.y) ? math.float2(1f, 0f) : math.float2(0f, 1f));
		float2 float8 = math.float2(float5.x - float5.y * 0.5f, float5.y);
		float2 float9 = math.float2(float8.x + float7.x - float7.y * 0.5f, float8.y + float7.y);
		float2 float10 = math.float2(float8.x + 0.5f, float8.y + 1f);
		float2 float11 = pos - float8;
		float2 float12 = pos - float9;
		float2 float13 = pos - float10;
		float3 obj = math.float3(float8.x, float9.x, float10.x);
		float3 float14 = math.float3(float8.y, float9.y, float10.y);
		float3 x2 = obj + 0.5f * float14;
		float3 x3 = float14;
		float3 obj2 = mod289(x2);
		x3 = mod289(x3);
		float2 x4 = rgrad2(math.float2(obj2.x, x3.x), rot);
		float2 x5 = rgrad2(math.float2(obj2.y, x3.y), rot);
		float2 x6 = rgrad2(math.float2(obj2.z, x3.z), rot);
		float3 y = math.float3(math.dot(x4, float11), math.dot(x5, float12), math.dot(x6, float13));
		float3 obj3 = math.max(0.8f - math.float3(math.dot(float11, float11), math.dot(float12, float12), math.dot(float13, float13)), 0f);
		float3 obj4 = obj3 * obj3;
		float num = math.dot(obj4 * obj4, y);
		return 11f * num;
	}

	public static float srnoise(float2 pos)
	{
		return srnoise(pos, 0f);
	}
}
