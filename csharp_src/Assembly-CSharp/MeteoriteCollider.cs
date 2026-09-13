using UnityEngine;

public class MeteoriteCollider : MonoBehaviour
{
	[SerializeField]
	private Transform _effect;

	[SerializeField]
	private Animator _anim;

	private ITimer hideTimer;

	private static float hideTime = 0.5f;

	private void OnEnable()
	{
		SceneManager.World.RegisterPhysics(this);
	}

	private void OnDisable()
	{
		SceneManager.World.UnregisterPhysics(this);
		if (hideTimer != null)
		{
			GameEntry.Timer.CancelTimer(hideTimer);
			hideTimer = null;
		}
	}

	private void OnTriggerEnter(Collider other)
	{
		if (hideTimer != null)
		{
			GameEntry.Timer.CancelTimer(hideTimer);
			hideTimer = null;
		}
		_effect.position = base.transform.position;
		_effect.gameObject.SetActive(value: true);
		_effect.transform.LookAt(other.transform);
		_anim.speed = 0f;
		hideTimer = GameEntry.Timer.RegisterTimer(hideTime, delegate
		{
			_anim.speed = 1f;
			base.gameObject.SetActive(value: false);
		});
	}
}
