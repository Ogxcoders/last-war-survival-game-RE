namespace UnityEngine.Rendering.Universal;

public static class CameraExtensions
{
	public static UniversalAdditionalCameraData GetUniversalAdditionalCameraData(this Camera camera)
	{
		GameObject gameObject = camera.gameObject;
		if (!gameObject.TryGetComponent<UniversalAdditionalCameraData>(out var component))
		{
			return gameObject.AddComponent<UniversalAdditionalCameraData>();
		}
		return component;
	}
}
