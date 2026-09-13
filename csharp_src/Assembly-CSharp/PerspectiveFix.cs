using UnityEngine;

public class PerspectiveFix : MonoBehaviour
{
	private Camera mainCamera;

	private Vector3 initialCameraPosition;

	private float initialUIPitch;

	private float initialHorizontalDistance;

	private void Start()
	{
		mainCamera = Camera.main;
		if (mainCamera != null)
		{
			initialCameraPosition = mainCamera.transform.position;
			initialUIPitch = base.transform.eulerAngles.x;
			Vector3 vector = mainCamera.transform.position - base.transform.position;
			vector.y = 0f;
			initialHorizontalDistance = vector.magnitude;
		}
	}

	private void LateUpdate()
	{
		if (!(mainCamera == null))
		{
			Vector3 vector = mainCamera.transform.position - base.transform.position;
			vector.y = 0f;
			float num = vector.magnitude / initialHorizontalDistance;
			float num2 = (1f - num) * 10f;
			float x = initialUIPitch + num2;
			Vector3 eulerAngles = base.transform.eulerAngles;
			eulerAngles.x = x;
			base.transform.rotation = Quaternion.Euler(eulerAngles);
		}
	}
}
