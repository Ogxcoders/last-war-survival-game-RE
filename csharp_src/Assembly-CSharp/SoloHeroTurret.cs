using System;
using UnityEngine;

public class SoloHeroTurret : MonoBehaviour
{
	public float rotateSpeed = 30f;

	public Transform target;

	public Action OnComplete;

	private void Update()
	{
		if (!(target == null))
		{
			Vector3 vector = target.position - base.transform.position;
			vector.y = 0f;
			vector = vector.normalized;
			if (Vector3.Dot(vector, base.transform.forward) > 0.999f)
			{
				OnComplete?.Invoke();
				target = null;
			}
			else
			{
				bool flag = Vector3.Cross(base.transform.forward, vector).y > 0f;
				Quaternion quaternion = Quaternion.AngleAxis(rotateSpeed * Time.deltaTime * (float)(flag ? 1 : (-1)), Vector3.up);
				base.transform.rotation = quaternion * base.transform.rotation;
			}
		}
	}
}
