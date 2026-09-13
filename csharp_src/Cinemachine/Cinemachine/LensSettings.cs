using System;
using UnityEngine;

namespace Cinemachine;

[Serializable]
[DocumentationSorting(DocumentationSortingAttribute.Level.UserRef)]
public struct LensSettings
{
	public static LensSettings Default = new LensSettings(40f, 10f, 0.1f, 5000f, 0f);

	[Range(1f, 179f)]
	[Tooltip("This is the camera view in vertical degrees. For cinematic people, a 50mm lens on a super-35mm sensor would equal a 19.6 degree FOV")]
	public float FieldOfView;

	[Tooltip("When using an orthographic camera, this defines the half-height, in world coordinates, of the camera view.")]
	public float OrthographicSize;

	[Tooltip("This defines the near region in the renderable range of the camera frustum. Raising this value will stop the game from drawing things near the camera, which can sometimes come in handy.  Larger values will also increase your shadow resolution.")]
	public float NearClipPlane;

	[Tooltip("This defines the far region of the renderable range of the camera frustum. Typically you want to set this value as low as possible without cutting off desired distant objects")]
	public float FarClipPlane;

	[Range(-180f, 180f)]
	[Tooltip("Camera Z roll, or tilt, in degrees.")]
	public float Dutch;

	public Vector2 LensShift;

	public bool Orthographic { get; set; }

	public bool IsPhysicalCamera { get; set; }

	public Vector2 SensorSize { get; set; }

	public float Aspect
	{
		get
		{
			if (SensorSize.y != 0f)
			{
				return SensorSize.x / SensorSize.y;
			}
			return 1f;
		}
	}

	public static LensSettings FromCamera(Camera fromCamera)
	{
		LensSettings result = Default;
		if (fromCamera != null)
		{
			result.FieldOfView = fromCamera.fieldOfView;
			result.OrthographicSize = fromCamera.orthographicSize;
			result.NearClipPlane = fromCamera.nearClipPlane;
			result.FarClipPlane = fromCamera.farClipPlane;
			result.LensShift = fromCamera.lensShift;
			result.SnapshotCameraReadOnlyProperties(fromCamera);
		}
		return result;
	}

	public void SnapshotCameraReadOnlyProperties(Camera camera)
	{
		if (camera != null)
		{
			Orthographic = camera.orthographic;
			SensorSize = new Vector2(camera.aspect, 1f);
			IsPhysicalCamera = camera.usePhysicalProperties;
			if (IsPhysicalCamera)
			{
				SensorSize = camera.sensorSize;
			}
		}
	}

	public void SnapshotCameraReadOnlyProperties(ref LensSettings lens)
	{
		Orthographic = lens.Orthographic;
		SensorSize = lens.SensorSize;
		IsPhysicalCamera = lens.IsPhysicalCamera;
	}

	public LensSettings(float fov, float orthographicSize, float nearClip, float farClip, float dutch)
	{
		this = default(LensSettings);
		FieldOfView = fov;
		OrthographicSize = orthographicSize;
		NearClipPlane = nearClip;
		FarClipPlane = farClip;
		Dutch = dutch;
	}

	public static LensSettings Lerp(LensSettings lensA, LensSettings lensB, float t)
	{
		t = Mathf.Clamp01(t);
		return new LensSettings
		{
			FarClipPlane = Mathf.Lerp(lensA.FarClipPlane, lensB.FarClipPlane, t),
			NearClipPlane = Mathf.Lerp(lensA.NearClipPlane, lensB.NearClipPlane, t),
			FieldOfView = Mathf.Lerp(lensA.FieldOfView, lensB.FieldOfView, t),
			OrthographicSize = Mathf.Lerp(lensA.OrthographicSize, lensB.OrthographicSize, t),
			Dutch = Mathf.Lerp(lensA.Dutch, lensB.Dutch, t),
			Orthographic = (lensA.Orthographic && lensB.Orthographic),
			IsPhysicalCamera = (lensA.IsPhysicalCamera || lensB.IsPhysicalCamera),
			SensorSize = Vector2.Lerp(lensA.SensorSize, lensB.SensorSize, t),
			LensShift = Vector2.Lerp(lensA.LensShift, lensB.LensShift, t)
		};
	}

	public void Validate()
	{
		NearClipPlane = Mathf.Max(NearClipPlane, 0.01f);
		FarClipPlane = Mathf.Max(FarClipPlane, NearClipPlane + 0.01f);
		FieldOfView = Mathf.Clamp(FieldOfView, 0.1f, 179f);
	}
}
