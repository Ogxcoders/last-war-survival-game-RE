using UnityEngine;

namespace Coffee.UIParticleExtensions;

internal class BakingCamera : MonoBehaviour
{
	private static BakingCamera s_Instance;

	private static readonly Vector3 s_OrthoPosition = new Vector3(0f, 0f, -1000f);

	private static readonly Quaternion s_OrthoRotation = Quaternion.identity;

	private Camera _camera;

	private static BakingCamera Instance
	{
		get
		{
			if (!s_Instance)
			{
				BakingCamera result = Object.FindObjectOfType<BakingCamera>() ?? Create();
				s_Instance = result;
				return result;
			}
			return s_Instance;
		}
	}

	private static BakingCamera Create()
	{
		GameObject gameObject = new GameObject(typeof(BakingCamera).Name);
		gameObject.hideFlags = HideFlags.HideAndDontSave;
		BakingCamera bakingCamera = gameObject.AddComponent<BakingCamera>();
		bakingCamera._camera = gameObject.AddComponent<Camera>();
		bakingCamera._camera.orthographic = true;
		gameObject.SetActive(value: false);
		return bakingCamera;
	}

	private void Awake()
	{
		if (this == s_Instance)
		{
			Object.DontDestroyOnLoad(base.gameObject);
		}
	}

	public static Camera GetCamera(Canvas canvas)
	{
		if (!canvas)
		{
			return Camera.main;
		}
		canvas = canvas.rootCanvas;
		Vector2 size = ((RectTransform)canvas.transform).rect.size;
		Instance._camera.orthographicSize = Mathf.Max(size.x, size.y) * canvas.scaleFactor;
		Camera worldCamera = canvas.worldCamera;
		Instance.transform.SetPositionAndRotation(rotation: (canvas.renderMode != RenderMode.ScreenSpaceOverlay && (bool)worldCamera) ? worldCamera.transform.rotation : s_OrthoRotation, position: s_OrthoPosition);
		Instance._camera.orthographic = true;
		Instance._camera.farClipPlane = 2000f;
		return Instance._camera;
	}
}
