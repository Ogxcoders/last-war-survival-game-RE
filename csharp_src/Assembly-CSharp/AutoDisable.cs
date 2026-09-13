using UnityEngine;

public class AutoDisable : MonoBehaviour
{
	public float time = 1f;

	private float _timer;

	private void OnEnable()
	{
		_timer = 0f;
	}

	private void Update()
	{
		_timer += Time.deltaTime;
		if (_timer >= time)
		{
			base.gameObject.SetActive(value: false);
		}
	}
}
