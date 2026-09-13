using UnityEngine;

[ExecuteInEditMode]
[DisallowMultipleComponent]
public class LookAtTarget : MonoBehaviour
{
	public Vector3 targetPosition;

	private void Update()
	{
		base.transform.LookAt(targetPosition);
	}
}
