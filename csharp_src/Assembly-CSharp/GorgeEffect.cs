using UnityEngine;

public class GorgeEffect : MonoBehaviour
{
	private SimpleAnimation _animator;

	private void Awake()
	{
		_animator = base.transform.GetComponent<SimpleAnimation>();
		float delaySec = (float)Random.Range(0, 30) / 10f;
		_animator.enabled = false;
		GameEntry.Timer.RegisterTimer(delaySec, delegate
		{
			_animator.enabled = true;
		});
	}
}
