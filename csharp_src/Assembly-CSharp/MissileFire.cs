using DG.Tweening;
using UnityEngine;

[ExecuteInEditMode]
[DisallowMultipleComponent]
public class MissileFire : MonoBehaviour
{
	public float moveTime = 0.5f;

	public Vector3 controlPointOffset = new Vector3(0f, 9f, 0f);

	public Vector3 targetPos;

	public LookAtTarget missile;

	public ParticleSystem hitEffect;

	public SimpleAnimation towerAnim;

	public float towerAnimTime;

	public Ease moveCurve;

	private Sequence _anim;

	private Vector3 lastTargetPos;

	private Vector3[] _pathList = new Vector3[SEGMENT_COUNT + 1];

	private static readonly int SEGMENT_COUNT = 30;

	public void Awake()
	{
		_anim = null;
		if (missile != null)
		{
			missile.gameObject.SetActive(value: false);
		}
	}

	public Tween DoFire()
	{
		if (_anim != null)
		{
			_anim.Kill();
		}
		if (hitEffect != null)
		{
			hitEffect.gameObject.SetActive(value: false);
		}
		if (missile != null)
		{
			Vector3 vector = new Vector3(0f, 0f, 4f);
			missile.gameObject.SetActive(value: false);
			if (lastTargetPos.Equals(targetPos))
			{
				missile.transform.localPosition = Vector3.zero;
				missile.transform.LookAt(vector);
			}
			else
			{
				lastTargetPos = targetPos;
				missile.targetPosition = targetPos;
				missile.transform.localPosition = Vector3.zero;
				missile.transform.LookAt(vector);
				Vector3 position = missile.transform.position;
				missile.transform.localPosition = vector;
				Vector3 position2 = missile.transform.position;
				Vector3 p = (position2 + targetPos) * 0.5f + controlPointOffset;
				for (int i = 1; i < SEGMENT_COUNT; i++)
				{
					_pathList[i] = CalculateCubicBezierPoint((float)i / (float)SEGMENT_COUNT, position2, p, targetPos);
				}
				_pathList[0] = position;
				_pathList[SEGMENT_COUNT] = targetPos;
			}
			Sequence sequence = DOTween.Sequence();
			if (towerAnim != null)
			{
				sequence.AppendCallback(delegate
				{
					towerAnim.Play("attack");
					towerAnim.PlayQueued("idle");
				});
				sequence.AppendInterval(towerAnimTime);
			}
			sequence.AppendCallback(delegate
			{
				missile.gameObject.SetActive(value: true);
				missile.transform.localPosition = Vector3.zero;
			});
			sequence.Append(missile.transform.DOPath(_pathList, moveTime)).SetEase(moveCurve);
			if (hitEffect != null)
			{
				sequence.AppendCallback(delegate
				{
					hitEffect.transform.position = missile.transform.position;
					hitEffect.gameObject.SetActive(value: true);
					hitEffect.Clear();
					hitEffect.Play();
					missile.gameObject.SetActive(value: false);
					missile.transform.localPosition = Vector3.zero;
				});
				sequence.AppendInterval(3f);
				sequence.AppendCallback(delegate
				{
					hitEffect.gameObject.SetActive(value: false);
					_anim = null;
				});
			}
			else
			{
				sequence.AppendCallback(delegate
				{
					missile.gameObject.SetActive(value: false);
					missile.transform.localPosition = Vector3.zero;
					_anim = null;
				});
			}
			sequence.OnKill(delegate
			{
				missile.transform.localPosition = Vector3.zero;
			});
			sequence.PlayForward();
			_anim = sequence;
			return sequence;
		}
		return null;
	}

	private Vector3 CalculateCubicBezierPoint(float t, Vector3 p0, Vector3 p1, Vector3 p2)
	{
		float num = 1f - t;
		float num2 = t * t;
		return num * num * p0 + 2f * num * t * p1 + num2 * p2;
	}
}
