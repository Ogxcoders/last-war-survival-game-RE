using System;
using DG.Tweening;
using UnityEngine;

public class CityTroopUnit
{
	public enum UnitType
	{
		Junkman
	}

	private enum State
	{
		Birth,
		MovetoGarbage,
		PickGarbage,
		PickGarbageSuccess,
		PickGarbageFail,
		BackFromGarbage,
		ToCarrier,
		Destroy
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

	protected CityTroop owner;

	public CityTroopUnit(UnitType type, CityTroop owner)
	{
		this.type = type;
		this.owner = owner;
	}

	public void CreateInstance(Action onComplete, Transform parent = null)
	{
		string prefabPath = "";
		if (type == UnitType.Junkman)
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
			if (owner.IsTruckPickGarbageTroop())
			{
				LootAt(pickDest);
			}
			stateTime -= Time.deltaTime;
			if (stateTime < 0f && owner.IsTruckPickGarbageTroop())
			{
				MoveToGarbage();
			}
			break;
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
		case State.PickGarbageFail:
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
			LootAt(owner.GetTrunkPosition());
			if (stateTime < 0f)
			{
				gameObject.SetActive(value: false);
				state = State.Destroy;
			}
			break;
		}
	}

	public void BirthThenMoveToGarbage(Vector3 birthDest, Vector3 pickDest, Vector3 garbageObjectPos)
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

	private void MoveToGarbage()
	{
		state = State.MovetoGarbage;
		anim.Play("xiaoren_run");
		float num = Vector3.Distance(birthDest, pickDest);
		float num2 = 4f;
		stateTime = num / num2;
		transform.DOMove(pickDest, stateTime).SetEase(Ease.Linear);
	}

	private void GotoCarrier()
	{
		stateTime = 0.4f;
		state = State.ToCarrier;
		anim.Play("xiaoren_run");
		Vector3 vector = owner.GetTrunkPosition() + new Vector3(0f, 1.6f, 0f);
		Vector3 endValue = (vector + birthDest) / 2f;
		endValue.y = 1.95f;
		transform.DOMove(birthDest, stateTime).SetEase(Ease.InBack);
		Sequence sequence = DOTween.Sequence();
		sequence.Append(transform.DOMove(endValue, stateTime * 0.7f).SetEase(Ease.OutSine));
		sequence.Append(transform.DOMove(vector, stateTime * 0.3f).SetEase(Ease.InSine));
		sequence.PlayForward();
	}

	private void DoPickGarbage()
	{
		this.state = State.PickGarbage;
		anim.Play("xiaoren_work");
		SimpleAnimation.State state = anim.GetState("xiaoren_work");
		if (state != null)
		{
			state.speed = 2f;
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
		if (state != State.PickGarbageSuccess && state != State.BackFromGarbage && state != State.ToCarrier)
		{
			stateTime = 1f;
			state = State.PickGarbageSuccess;
			anim.Play("xiaoren_show");
		}
	}

	public void PickGarbageFail()
	{
		if (state != State.PickGarbageFail && state != State.BackFromGarbage && state != State.ToCarrier)
		{
			stateTime = 1f;
			state = State.PickGarbageFail;
			anim.Play("xiaoren_fail");
		}
	}

	public void PickMoveBack()
	{
		state = State.BackFromGarbage;
		float num = Vector3.Distance(birthDest, pickDest);
		float num2 = 4f;
		stateTime = Math.Min(num / num2, 1f);
		transform.DOMove(birthDest, stateTime).SetEase(Ease.Linear);
		anim.Play("xiaoren_run");
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
}
