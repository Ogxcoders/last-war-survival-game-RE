using UnityEngine;

public class AutoDestroy : MonoBehaviour
{
	public float time = 1f;

	public InstanceRequest handle;

	public bool realDestroy = true;

	public bool removeSelf;

	private float _timer;

	private void OnEnable()
	{
		_timer = 0f;
	}

	private void Update()
	{
		_timer += Time.deltaTime;
		if (!(_timer >= time))
		{
			return;
		}
		if (handle != null)
		{
			if (realDestroy)
			{
				handle.RealDestroy();
			}
			else
			{
				if (removeSelf)
				{
					Object.Destroy(this);
				}
				handle.Destroy();
			}
			handle = null;
		}
		else
		{
			Object.Destroy(base.gameObject);
		}
	}
}
