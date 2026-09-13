using UnityEngine;

namespace MoreMountains.NiceVibrations;

public class PresetDemoRotator : MonoBehaviour
{
	public Vector3 RotationSpeed = new Vector3(0f, 0f, 100f);

	protected void Update()
	{
		base.transform.Rotate(RotationSpeed * Time.deltaTime, Space.Self);
	}
}
