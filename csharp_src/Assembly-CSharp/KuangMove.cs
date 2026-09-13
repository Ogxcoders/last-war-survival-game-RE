using UnityEngine;

public class KuangMove : MonoBehaviour
{
	public float speed = 0.5f;

	public float addvalue;

	private Vector3 currentscale;

	private void Start()
	{
		currentscale = base.transform.localScale;
	}

	private void Update()
	{
		float num = Mathf.PingPong(Time.time * speed, addvalue);
		Vector3 vector = new Vector3(num, num, 0f);
		base.transform.localScale = currentscale + vector;
	}
}
