using System;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;
using UnityEngine.Scripting;

namespace UnityEngine.Experimental.GlobalIllumination;

public static class Lightmapping
{
	public delegate void RequestLightsDelegate(Light[] requests, NativeArray<LightDataGI> lightsOutput);

	[RequiredByNativeCode]
	private static readonly RequestLightsDelegate s_DefaultDelegate = delegate(Light[] requests, NativeArray<LightDataGI> lightsOutput)
	{
		DirectionalLight dir = default(DirectionalLight);
		PointLight point = default(PointLight);
		SpotLight spot = default(SpotLight);
		RectangleLight rect = default(RectangleLight);
		DiscLight disc = default(DiscLight);
		LightDataGI value = default(LightDataGI);
		for (int i = 0; i < requests.Length; i++)
		{
			Light light = requests[i];
			switch (light.type)
			{
			case UnityEngine.LightType.Directional:
				LightmapperUtils.Extract(light, ref dir);
				value.Init(ref dir);
				break;
			case UnityEngine.LightType.Point:
				LightmapperUtils.Extract(light, ref point);
				value.Init(ref point);
				break;
			case UnityEngine.LightType.Spot:
				LightmapperUtils.Extract(light, ref spot);
				value.Init(ref spot);
				break;
			case UnityEngine.LightType.Area:
				LightmapperUtils.Extract(light, ref rect);
				value.Init(ref rect);
				break;
			case UnityEngine.LightType.Disc:
				LightmapperUtils.Extract(light, ref disc);
				value.Init(ref disc);
				break;
			default:
				value.InitNoBake(light.GetInstanceID());
				break;
			}
			lightsOutput[i] = value;
		}
	};

	[RequiredByNativeCode]
	private static RequestLightsDelegate s_RequestLightsDelegate = s_DefaultDelegate;

	[RequiredByNativeCode]
	public static void SetDelegate(RequestLightsDelegate del)
	{
		s_RequestLightsDelegate = ((del != null) ? del : s_DefaultDelegate);
	}

	[RequiredByNativeCode]
	public static RequestLightsDelegate GetDelegate()
	{
		return s_RequestLightsDelegate;
	}

	[RequiredByNativeCode]
	public static void ResetDelegate()
	{
		s_RequestLightsDelegate = s_DefaultDelegate;
	}

	[RequiredByNativeCode]
	internal unsafe static void RequestLights(Light[] lights, IntPtr outLightsPtr, int outLightsCount)
	{
		NativeArray<LightDataGI> lightsOutput = NativeArrayUnsafeUtility.ConvertExistingDataToNativeArray<LightDataGI>((void*)outLightsPtr, outLightsCount, Allocator.None);
		s_RequestLightsDelegate(lights, lightsOutput);
	}
}
