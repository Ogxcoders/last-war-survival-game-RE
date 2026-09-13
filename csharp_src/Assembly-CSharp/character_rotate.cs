using UnityEngine;

public class character_rotate : MonoBehaviour
{
	public float speed;

	private void Update()
	{
		base.transform.Rotate(Vector3.up * Time.deltaTime * speed);
	}
}
