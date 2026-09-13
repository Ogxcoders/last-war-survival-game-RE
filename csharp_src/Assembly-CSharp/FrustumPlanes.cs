using System.Runtime.InteropServices;
using Unity.Collections;
using Unity.Mathematics;
using UnityEngine;

[StructLayout(LayoutKind.Sequential, Size = 1)]
public struct FrustumPlanes
{
	public enum IntersectResult
	{
		Out,
		In,
		Partial
	}

	public struct PlanePacket4
	{
		public float4 Xs;

		public float4 Ys;

		public float4 Zs;

		public float4 Distances;
	}

	public static IntersectResult Intersect(NativeArray<float4> cullingPlanes, AABB a)
	{
		float3 center = a.Center;
		float3 extents = a.Extents;
		int num = 0;
		for (int i = 0; i < cullingPlanes.Length; i++)
		{
			float3 xyz = cullingPlanes[i].xyz;
			float num2 = math.dot(xyz, center) + cullingPlanes[i].w;
			float num3 = math.dot(extents, math.abs(xyz));
			if (num2 + num3 <= 0f)
			{
				return IntersectResult.Out;
			}
			if (num2 > num3)
			{
				num++;
			}
		}
		if (num != cullingPlanes.Length)
		{
			return IntersectResult.Partial;
		}
		return IntersectResult.In;
	}

	public static NativeArray<PlanePacket4> BuildSOAPlanePackets(NativeArray<Plane> cullingPlanes, Allocator allocator)
	{
		int length = cullingPlanes.Length;
		int num = length + 3 >> 2;
		NativeArray<PlanePacket4> result = new NativeArray<PlanePacket4>(num, allocator, NativeArrayOptions.UninitializedMemory);
		for (int i = 0; i < length; i++)
		{
			PlanePacket4 value = result[i >> 2];
			value.Xs[i & 3] = cullingPlanes[i].normal.x;
			value.Ys[i & 3] = cullingPlanes[i].normal.y;
			value.Zs[i & 3] = cullingPlanes[i].normal.z;
			value.Distances[i & 3] = cullingPlanes[i].distance;
			result[i >> 2] = value;
		}
		for (int j = length; j < 4 * num; j++)
		{
			PlanePacket4 value2 = result[j >> 2];
			value2.Xs[j & 3] = 1f;
			value2.Ys[j & 3] = 0f;
			value2.Zs[j & 3] = 0f;
			value2.Distances[j & 3] = 32786f;
			result[j >> 2] = value2;
		}
		return result;
	}

	public static IntersectResult Intersect2(NativeArray<PlanePacket4> cullingPlanePackets, AABB a)
	{
		float4 xxxx = a.Center.xxxx;
		float4 yyyy = a.Center.yyyy;
		float4 zzzz = a.Center.zzzz;
		float4 xxxx2 = a.Extents.xxxx;
		float4 yyyy2 = a.Extents.yyyy;
		float4 zzzz2 = a.Extents.zzzz;
		int4 x = 0;
		int4 x2 = 0;
		for (int i = 0; i < cullingPlanePackets.Length; i++)
		{
			PlanePacket4 planePacket = cullingPlanePackets[i];
			float4 @float = dot4(planePacket.Xs, planePacket.Ys, planePacket.Zs, xxxx, yyyy, zzzz) + planePacket.Distances;
			float4 float2 = dot4(xxxx2, yyyy2, zzzz2, math.abs(planePacket.Xs), math.abs(planePacket.Ys), math.abs(planePacket.Zs));
			x += (int4)(@float + float2 <= 0f);
			x2 += (int4)(@float > float2);
		}
		int num = math.csum(x2);
		if (math.csum(x) != 0)
		{
			return IntersectResult.Out;
		}
		if (num != 4 * cullingPlanePackets.Length)
		{
			return IntersectResult.Partial;
		}
		return IntersectResult.In;
	}

	public static IntersectResult Intersect2NoPartial(NativeArray<PlanePacket4> cullingPlanePackets, AABB a)
	{
		float4 xxxx = a.Center.xxxx;
		float4 yyyy = a.Center.yyyy;
		float4 zzzz = a.Center.zzzz;
		float4 xxxx2 = a.Extents.xxxx;
		float4 yyyy2 = a.Extents.yyyy;
		float4 zzzz2 = a.Extents.zzzz;
		int4 x = 0;
		for (int i = 0; i < cullingPlanePackets.Length; i++)
		{
			PlanePacket4 planePacket = cullingPlanePackets[i];
			float4 @float = dot4(planePacket.Xs, planePacket.Ys, planePacket.Zs, xxxx, yyyy, zzzz) + planePacket.Distances;
			float4 float2 = dot4(xxxx2, yyyy2, zzzz2, math.abs(planePacket.Xs), math.abs(planePacket.Ys), math.abs(planePacket.Zs));
			x += (int4)(@float + float2 <= 0f);
		}
		if (math.csum(x) <= 0)
		{
			return IntersectResult.In;
		}
		return IntersectResult.Out;
	}

	private static float4 dot4(float4 xs, float4 ys, float4 zs, float4 mx, float4 my, float4 mz)
	{
		return xs * mx + ys * my + zs * mz;
	}

	public static IntersectResult Intersect(NativeArray<float4> planes, float3 center, float radius)
	{
		int num = 0;
		for (int i = 0; i < planes.Length; i++)
		{
			float num2 = math.dot(planes[i].xyz, center) + planes[i].w;
			if (num2 < 0f - radius)
			{
				return IntersectResult.Out;
			}
			if (num2 > radius)
			{
				num++;
			}
		}
		if (num != planes.Length)
		{
			return IntersectResult.Partial;
		}
		return IntersectResult.In;
	}
}
