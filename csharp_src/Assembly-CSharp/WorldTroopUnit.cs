using System;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class WorldTroopUnit
{
	public enum UnitType
	{
		Tank,
		Infantry,
		Plane,
		Junkman
	}

	private enum State
	{
		Birth,
		Attack,
		Back,
		Destroy,
		MovetoGarbage,
		PickGarbage,
		PickGarbageSuccess,
		PickGarbageFailed,
		BackFromGarbage,
		ToCarrier
	}

	private UnitType type;

	private State state;

	private InstanceRequest instance;

	private Transform transform;

	private Vector3 birthDest;

	private Vector3 pickDest;

	private Vector3 garbageObjectPos;

	private float stateTime;

	protected SimpleAnimation anim;

	protected GameObject gameObject;

	protected ParticleSystem[] particleSystems;

	protected WorldTroop owner;

	public WorldTroop target;

	protected List<InstanceRequest> effectList = new List<InstanceRequest>();

	public string Uid { get; set; }

	public WorldTroopUnit(UnitType type, WorldTroop owner)
	{
		this.type = type;
		this.owner = owner;
	}

	public void CreateInstance(Action onComplete, Transform parent = null)
	{
		string prefabPath = "";
		if (type == UnitType.Infantry)
		{
			prefabPath = "Assets/Main/Prefabs/March/WorldTroopSoldier.prefab";
		}
		else if (type == UnitType.Tank)
		{
			prefabPath = "Assets/Main/Prefabs/March/WorldTroopTank.prefab";
		}
		else if (type == UnitType.Plane)
		{
			prefabPath = "Assets/Main/Prefabs/March/WorldTroopPlane.prefab";
		}
		else if (type == UnitType.Junkman)
		{
			prefabPath = "Assets/Main/Prefabs/March/WorldTroopJunkman.prefab";
		}
		instance = GameEntry.Resource.InstantiateAsync(prefabPath);
		instance.completed += delegate
		{
			gameObject = instance.gameObject;
			transform = gameObject.transform;
			transform.SetParent(parent);
			transform.localRotation = Quaternion.identity;
			anim = gameObject.GetComponentInChildren<SimpleAnimation>();
			onComplete?.Invoke();
		};
	}

	public virtual void Destroy()
	{
		instance.Destroy();
		gameObject = null;
		transform = null;
		anim = null;
	}

	public void Update()
	{
		if (instance == null || !instance.isDone || gameObject == null)
		{
			return;
		}
		switch (state)
		{
		case State.Birth:
			if (owner.IsPickGarbageTroop())
			{
				LootAt(pickDest);
			}
			else
			{
				Vector3 position = Vector3.Lerp(transform.position, birthDest, Time.deltaTime * 2f);
				transform.position = position;
			}
			stateTime -= Time.deltaTime;
			if (stateTime < 0f)
			{
				if (owner.IsPickGarbageTroop())
				{
					MoveToGarbage();
				}
				else
				{
					state = State.Attack;
				}
			}
			break;
		case State.Back:
		{
			Vector3 position2 = owner.GetPosition();
			Vector3 position3 = Vector3.Lerp(transform.position, position2, Time.deltaTime);
			transform.position = position3;
			stateTime -= Time.deltaTime;
			if (stateTime < 0f)
			{
				state = State.Destroy;
			}
			break;
		}
		case State.MovetoGarbage:
			stateTime -= Time.deltaTime;
			ChangeLookAt(pickDest, garbageObjectPos, stateTime, 0.4f);
			if (stateTime < 0f)
			{
				DoPickGarbage();
			}
			break;
		case State.PickGarbage:
			transform.position = pickDest;
			LootAt(garbageObjectPos);
			break;
		case State.PickGarbageSuccess:
			transform.position = pickDest;
			stateTime -= Time.deltaTime;
			if (stateTime < 0f)
			{
				PickMoveBack();
			}
			break;
		case State.BackFromGarbage:
			stateTime -= Time.deltaTime;
			LootAt(birthDest);
			if (stateTime < 0f)
			{
				GotoCarrier();
			}
			break;
		case State.ToCarrier:
			stateTime -= Time.deltaTime;
			LootAt(owner.GetPosition());
			if (stateTime < 0f)
			{
				gameObject.SetActive(value: false);
				state = State.Destroy;
			}
			break;
		case State.Attack:
		case State.Destroy:
		case State.PickGarbageFailed:
			break;
		}
	}

	public void BirthThenAttack(Vector3 birthDest)
	{
		this.birthDest = birthDest;
		this.state = State.Birth;
		anim.Play("birth");
		anim.PlayQueued("attack");
		stateTime = 1f;
		SimpleAnimation.State state = anim.GetState("birth");
		if (state != null)
		{
			stateTime = state.length;
		}
	}

	public void BirthThenMoveToGarbage(Vector3 birthDest, Vector3 pickDest, Vector3 garbageObjectPos)
	{
		if (anim != null)
		{
			this.birthDest = birthDest;
			this.pickDest = pickDest;
			this.garbageObjectPos = garbageObjectPos;
			state = State.Birth;
			anim.Play("xiaoren_run");
			stateTime = 0.4f;
			Vector3 endValue = (transform.position + birthDest) / 2f;
			endValue.y = 1.95f;
			transform.DOMove(birthDest, stateTime).SetEase(Ease.InBack);
			Sequence sequence = DOTween.Sequence();
			sequence.Append(transform.DOMove(endValue, stateTime * 0.3f).SetEase(Ease.OutSine));
			sequence.Append(transform.DOMove(birthDest, stateTime * 0.7f).SetEase(Ease.InSine));
			sequence.PlayForward();
		}
	}

	private void MoveToGarbage()
	{
		if (anim != null)
		{
			state = State.MovetoGarbage;
			anim.Play("xiaoren_run");
			float num = Vector3.Distance(birthDest, pickDest);
			float num2 = 4f;
			stateTime = num / num2;
			transform.DOMove(pickDest, stateTime).SetEase(Ease.Linear);
		}
	}

	private void GotoCarrier()
	{
		if (anim != null)
		{
			stateTime = 0.4f;
			state = State.ToCarrier;
			anim.Play("xiaoren_run");
			Vector3 vector = owner.GetPosition() + new Vector3(0f, 1.6f, 0f);
			Vector3 endValue = (vector + birthDest) / 2f;
			endValue.y = 1.95f;
			transform.DOMove(birthDest, stateTime).SetEase(Ease.InBack);
			Sequence sequence = DOTween.Sequence();
			sequence.Append(transform.DOMove(endValue, stateTime * 0.7f).SetEase(Ease.OutSine));
			sequence.Append(transform.DOMove(vector, stateTime * 0.3f).SetEase(Ease.InSine));
			sequence.PlayForward();
		}
	}

	private void DoPickGarbage()
	{
		if (anim != null)
		{
			this.state = State.PickGarbage;
			anim.Play("xiaoren_work");
			SimpleAnimation.State state = anim.GetState("xiaoren_work");
			if (state != null)
			{
				_ = state.speed;
				state.speed = 2f;
			}
		}
	}

	public void BirthThenPickGarbage(Vector3 birthDest, Vector3 pickDest, Vector3 garbageObjectPos)
	{
		this.birthDest = birthDest;
		this.pickDest = pickDest;
		this.garbageObjectPos = garbageObjectPos;
		DoPickGarbage();
	}

	public void PickGarbageSuccess()
	{
		if (anim != null)
		{
			stateTime = 0.6f;
			state = State.PickGarbageSuccess;
			anim.Play("xiaoren_show");
		}
	}

	public void PickMoveBack()
	{
		if (anim != null)
		{
			state = State.BackFromGarbage;
			float num = Vector3.Distance(birthDest, pickDest);
			float num2 = 4f;
			stateTime = Math.Min(num / num2, 1f);
			transform.DOMove(birthDest, stateTime).SetEase(Ease.Linear);
			anim.Play("xiaoren_run");
		}
	}

	public void Back()
	{
		this.state = State.Back;
		if (anim != null)
		{
			anim.Play("back");
			stateTime = 1f;
			SimpleAnimation.State state = anim.GetState("back");
			if (state != null)
			{
				stateTime = state.length;
			}
		}
		YieldUtils.DelayActionWithOutContext(delegate
		{
			Destroy();
		}, 1f);
	}

	public void Attack()
	{
		state = State.Attack;
		anim.Play("attack");
	}

	public void SetPosition(Vector3 pos)
	{
		transform.position = pos;
	}

	public virtual void LootAt(Vector3 target)
	{
		transform.LookAt(target);
	}

	public virtual void ChangeLookAt(Vector3 start, Vector3 end, float currentTime, float totalTime)
	{
		if (currentTime >= totalTime)
		{
			transform.LookAt(start);
			return;
		}
		float val = (totalTime - currentTime) / totalTime;
		val = Math.Max(val, 0f);
		val = Math.Min(val, 1f);
		Vector3 worldPosition = Vector3.Lerp(start, end, val);
		transform.LookAt(worldPosition);
	}

	public bool IsBackFinish()
	{
		return state == State.Destroy;
	}

	public virtual void PlayAttackEffect()
	{
	}

	public virtual void StopAttackEffect()
	{
		if (target != null)
		{
			target.ClearHitEffect();
		}
		for (int i = 0; i < effectList.Count; i++)
		{
			effectList[i].Destroy();
		}
		effectList.Clear();
	}

	protected InstanceRequest CreateEffect(string path, string effectHangPoint, Action<GameObject> onComplete)
	{
		InstanceRequest req = GameEntry.Resource.InstantiateAsync(path);
		req.completed += delegate
		{
			GameObject gameObject = req.gameObject;
			if (gameObject == null)
			{
				Debug.Log($"init {path} error");
			}
			else if (!(this.gameObject == null))
			{
				Transform transform = this.gameObject.transform.Find(effectHangPoint);
				if (transform != null)
				{
					gameObject.transform.SetParent(transform);
					gameObject.transform.localPosition = Vector3.zero;
					gameObject.transform.localRotation = Quaternion.identity;
					gameObject.transform.localScale = Vector3.one;
					onComplete(gameObject);
				}
			}
		};
		return req;
	}

	protected void ReSetParticleSystems()
	{
		BattleUtil.ReSetParticleSystems(particleSystems);
	}

	protected virtual void PlayHitEffect(string effectPath, WorldTroop target)
	{
		target?.PlayHitEffect(effectPath);
	}
}
