using UnityEngine;

namespace MoreMountains.NiceVibrations;

public class BallDemoBall : MonoBehaviour
{
	public bool HapticsEnabled = true;

	public ParticleSystem HitParticles;

	public ParticleSystem HitPusherParticles;

	public LayerMask WallMask;

	public LayerMask PusherMask;

	public MMUIShaker LogoShaker;

	public AudioSource TransientAudioSource;

	protected Rigidbody2D _rigidBody;

	protected float _lastRaycastTimestamp;

	protected Animator _ballAnimator;

	protected int _hitAnimationParameter;

	protected virtual void Awake()
	{
		_rigidBody = base.gameObject.GetComponent<Rigidbody2D>();
		_ballAnimator = base.gameObject.GetComponent<Animator>();
		_hitAnimationParameter = Animator.StringToHash("Hit");
		MMNViOSCoreHaptics.CreateEngine();
	}

	protected virtual void OnCollisionEnter2D(Collision2D collision)
	{
		if ((int)WallMask == ((int)WallMask | (1 << collision.gameObject.layer)))
		{
			HitWall();
		}
	}

	protected virtual void Update()
	{
		float num = 5f;
		Debug.DrawLine(base.transform.position, Vector3.down * num, Color.red);
		if (Time.time - _lastRaycastTimestamp > 1f)
		{
			_lastRaycastTimestamp = Time.time;
			if (Physics2D.Raycast(base.transform.position, Vector2.down, num, WallMask).collider != null)
			{
				HitBottom();
			}
		}
	}

	protected virtual void HitBottom()
	{
		_rigidBody.AddForce(Vector2.up * 2500f);
		StartCoroutine(LogoShaker.Shake(0.2f));
	}

	protected virtual void HitWall()
	{
		float num = _rigidBody.velocity.magnitude / 100f;
		MMVibrationManager.TransientHaptic(num, 0.7f, alsoRumble: true, this);
		TransientAudioSource.volume = num;
		StartCoroutine(LogoShaker.Shake(0.2f));
		TransientAudioSource.Play();
		_ballAnimator.SetTrigger(_hitAnimationParameter);
	}

	public virtual void HitPusher()
	{
		HitPusherParticles.Play();
		MMVibrationManager.TransientHaptic(0.85f, 0.05f, alsoRumble: true, this);
		TransientAudioSource.volume = 0.1f;
		StartCoroutine(LogoShaker.Shake(0.2f));
		TransientAudioSource.Play();
		_ballAnimator.SetTrigger(_hitAnimationParameter);
	}
}
