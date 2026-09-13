using UnityEngine;

public class ManualUpdateAnimator : ManualUpdatorComponent
{
	public Animator animator;

	private void Awake()
	{
		if (animator == null)
		{
			animator = GetComponent<Animator>();
		}
	}

	protected new void OnEnable()
	{
		animator.enabled = true;
		base.OnEnable();
	}

	protected new void OnDisable()
	{
		animator.enabled = false;
		base.OnDisable();
	}

	public override void ManualUpdate(float delta)
	{
		animator.Update(delta);
	}
}
