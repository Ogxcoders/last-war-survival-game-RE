using System;
using UnityEngine;

namespace VolumetricFogAndMist2;

[ExecuteInEditMode]
public class PointLightManager : MonoBehaviour, IVolumetricFogManager
{
	public const int MAX_POINT_LIGHTS = 16;

	[Header("Point Light Search Settings")]
	[Tooltip("Point lights are sorted by distance to tracking center object")]
	public Transform trackingCenter;

	public float newLightsCheckInterval = 3f;

	[Header("Common Settings")]
	[Tooltip("Global inscattering multiplier for point lights")]
	public float inscattering = 1f;

	[Tooltip("Global intensity multiplier for point lights")]
	public float intensity = 1f;

	[Tooltip("Reduces light intensity near point lights")]
	public float insideAtten;

	private Light[] pointLights;

	private Vector4[] pointLightColorBuffer;

	private Vector4[] pointLightPositionBuffer;

	private float checkNewLightsLastTime;

	public string managerName => "Point Light Manager";

	private void OnEnable()
	{
		if (trackingCenter == null)
		{
			Camera cam = null;
			Tools.CheckCamera(ref cam);
			if (cam != null)
			{
				trackingCenter = cam.transform;
			}
		}
		if (pointLightColorBuffer == null || pointLightColorBuffer.Length != 16)
		{
			pointLightColorBuffer = new Vector4[16];
		}
		if (pointLightPositionBuffer == null || pointLightPositionBuffer.Length != 16)
		{
			pointLightPositionBuffer = new Vector4[16];
		}
	}

	private void LateUpdate()
	{
		TrackPointLights();
		SubmitPointLightData();
	}

	private void SubmitPointLightData()
	{
		int num = 0;
		int num2 = 0;
		while (num < 16 && num2 < pointLights.Length)
		{
			Light light = pointLights[num2];
			if (!(light == null) && light.isActiveAndEnabled && light.type == LightType.Point)
			{
				Vector3 position = light.transform.position;
				float num3 = light.range * inscattering / 25f;
				float num4 = light.intensity * intensity;
				if (num3 > 0f && num4 > 0f)
				{
					pointLightPositionBuffer[num].x = position.x;
					pointLightPositionBuffer[num].y = position.y;
					pointLightPositionBuffer[num].z = position.z;
					pointLightPositionBuffer[num].w = 0f;
					Color color = light.color;
					pointLightColorBuffer[num].x = color.r * num4;
					pointLightColorBuffer[num].y = color.g * num4;
					pointLightColorBuffer[num].z = color.b * num4;
					pointLightColorBuffer[num].w = num3;
					num++;
				}
			}
			num2++;
		}
		Shader.SetGlobalVectorArray("_VF2_PointLightColor", pointLightColorBuffer);
		Shader.SetGlobalVectorArray("_VF2_FogPointLightPosition", pointLightPositionBuffer);
		Shader.SetGlobalFloat("_VF2_PointLightInsideAtten", insideAtten);
		Shader.SetGlobalInt("_VF2_PointLightCount", num);
	}

	public void TrackPointLights(bool forceImmediateUpdate = false)
	{
		if (forceImmediateUpdate || pointLights == null || !Application.isPlaying || (newLightsCheckInterval > 0f && Time.time - checkNewLightsLastTime > newLightsCheckInterval))
		{
			checkNewLightsLastTime = Time.time;
			pointLights = UnityEngine.Object.FindObjectsOfType<Light>();
			Array.Sort(pointLights, pointLightsDistanceComparer);
		}
	}

	private int pointLightsDistanceComparer(Light l1, Light l2)
	{
		float sqrMagnitude = (l1.transform.position - trackingCenter.position).sqrMagnitude;
		float sqrMagnitude2 = (l2.transform.position - trackingCenter.position).sqrMagnitude;
		if (sqrMagnitude < sqrMagnitude2)
		{
			return -1;
		}
		if (sqrMagnitude > sqrMagnitude2)
		{
			return 1;
		}
		return 0;
	}
}
