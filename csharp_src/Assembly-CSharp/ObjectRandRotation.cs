using UnityEngine;

public class ObjectRandRotation : MonoBehaviour
{
	public float axisX = 1f;

	public float axisY = 1f;

	public float axisZ = 1f;

	public float speed = 1f;

	private void Start()
	{
	}

	private void Update()
	{
		base.transform.Rotate(new Vector3(axisY, axisX, axisZ) * speed, Space.World);
	}
}
