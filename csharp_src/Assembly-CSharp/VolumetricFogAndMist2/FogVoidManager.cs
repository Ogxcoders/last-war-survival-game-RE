using System;
using UnityEngine;

namespace VolumetricFogAndMist2;

[ExecuteInEditMode]
public class FogVoidManager : MonoBehaviour, IVolumetricFogManager
{
	public const int MAX_FOG_VOID = 8;

	[Header("Void Search Settings")]
	public Transform trackingCenter;

	public float newFogVoidCheckInterval = 3f;

	private FogVoid[] fogVoids;

	private Vector4[] fogVoidPositionAndSizes;

	private float checkNewFogVoidLastTime;

	private bool requireRefresh;

	public string managerName => "Fog Void Manager";

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
		if (fogVoidPositionAndSizes == null || fogVoidPositionAndSizes.Length != 8)
		{
			fogVoidPositionAndSizes = new Vector4[8];
		}
	}

	private void SubmitFogVoidData()
	{
		int num = 0;
		int num2 = 0;
		while (num < 8 && num2 < fogVoids.Length)
		{
			FogVoid fogVoid = fogVoids[num2];
			if (!(fogVoid == null) && fogVoid.isActiveAndEnabled)
			{
				Vector3 position = fogVoid.transform.position;
				fogVoidPositionAndSizes[num].x = position.x;
				fogVoidPositionAndSizes[num].y = 10f * (1f - fogVoid.falloff);
				fogVoidPositionAndSizes[num].z = position.z;
				fogVoidPositionAndSizes[num].w = 1f / (0.0001f + fogVoid.radius * fogVoid.radius);
				num++;
			}
			num2++;
		}
		Shader.SetGlobalVectorArray("_VF2_FogVoidPositionAndSizes", fogVoidPositionAndSizes);
		Shader.SetGlobalInt("_VF2_FogVoidCount", num);
	}

	public void TrackFogVoids(bool forceImmediateUpdate = false)
	{
		if (forceImmediateUpdate || fogVoids == null || !Application.isPlaying || (newFogVoidCheckInterval > 0f && Time.time - checkNewFogVoidLastTime > newFogVoidCheckInterval))
		{
			checkNewFogVoidLastTime = Time.time;
			fogVoids = UnityEngine.Object.FindObjectsOfType<FogVoid>();
			Array.Sort(fogVoids, fogVoidDistanceComparer);
		}
	}

	private int fogVoidDistanceComparer(FogVoid v1, FogVoid v2)
	{
		float sqrMagnitude = (v1.transform.position - trackingCenter.position).sqrMagnitude;
		float sqrMagnitude2 = (v2.transform.position - trackingCenter.position).sqrMagnitude;
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

	private void LateUpdate()
	{
		if (requireRefresh)
		{
			requireRefresh = false;
			TrackFogVoids(forceImmediateUpdate: true);
		}
		else
		{
			TrackFogVoids();
		}
		SubmitFogVoidData();
	}

	public void Refresh()
	{
		requireRefresh = true;
	}
}
