using UnityEngine;

public class CameraUtil
{
	public static void CustomTransparencyAxis(Camera camera)
	{
		camera.transparencySortMode = TransparencySortMode.CustomAxis;
		camera.transparencySortAxis = new Vector3(0f, -1f, 0f);
	}

	public static void DefaultTransparencyAxis(Camera camera)
	{
		camera.transparencySortMode = TransparencySortMode.Default;
		camera.transparencySortAxis = new Vector3(0f, 0f, 0f);
	}
}
