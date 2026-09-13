using UnityEngine;

public class CitySpaceManAni : MonoBehaviour
{
	private Animator _animator;

	private void Awake()
	{
		_animator = GetComponent<Animator>();
	}

	public void SetTrigger(string triggerName)
	{
		_animator.SetTrigger(triggerName);
		_animator.SetLayerWeight(1, 1f);
	}

	public void SetSpeed(float speed)
	{
		_animator.speed = speed;
	}
}
