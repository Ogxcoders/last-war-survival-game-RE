using UnityEngine;

public static class CameraUtilities
{
	public static Camera GetOrFindMainCamera()
	{
		Camera camera = Camera.main;
		if (camera == null)
		{
			Camera[] allCameras = Camera.allCameras;
			foreach (Camera camera2 in allCameras)
			{
				if (camera2.gameObject.CompareTag("MainCamera"))
				{
					camera = camera2;
					break;
				}
			}
		}
		return camera;
	}
}
