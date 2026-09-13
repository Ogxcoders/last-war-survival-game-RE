using UnityEngine;

public class LookAtY : MonoBehaviour
{
	private Camera mainCamera;

	private void Start()
	{
		mainCamera = Camera.main;
	}

	private void Update()
	{
		mainCamera = Camera.main;
		if (mainCamera != null)
		{
			Vector3 normalized = (mainCamera.transform.position - base.transform.position).normalized;
			Quaternion.LookRotation(normalized, Vector3.up);
			Vector3 up = mainCamera.transform.up;
			Quaternion rotation = Quaternion.FromToRotation(Vector3.forward, up);
			base.transform.rotation = rotation;
		}
	}
}
