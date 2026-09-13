using UnityEngine;

public class CattleMature : WorldResourceItemBase
{
	public enum MatureState
	{
		None,
		Free,
		Feed,
		Product,
		Kill
	}

	private float gatherAnimatorTime;

	private float time;

	private MatureState curState;

	private long startTime;

	private long endTime;

	public override void Init(long bUuid, string modelName)
	{
		base.uuid = bUuid;
		string animName = modelName + "_big_tuzai";
		gatherAnimatorTime = GetClipLength(0, animName);
		curState = MatureState.None;
	}

	public void SetTime(long stTime, long edTime)
	{
		startTime = stTime;
		endTime = edTime;
	}

	public override void UnInit()
	{
		curState = MatureState.None;
		Object.Destroy(base.transform.gameObject);
	}

	public override void OnUpdateTime()
	{
		if (curState == MatureState.Kill)
		{
			time += Time.deltaTime;
			if (time >= gatherAnimatorTime)
			{
				UnInit();
			}
		}
		else if (curState == MatureState.Feed && startTime > 0 && endTime > 0)
		{
			long serverTime = GameEntry.Timer.GetServerTime();
			if (endTime < serverTime)
			{
				GameEntry.Event.Fire(EventId.FarmSecondProduct, base.uuid);
				ChangeState(MatureState.Product);
				startTime = 0L;
				endTime = 0L;
			}
		}
	}

	public MatureState GetCurState()
	{
		return curState;
	}

	public void ChangeState(MatureState state)
	{
		if (curState == state)
		{
			return;
		}
		switch (state)
		{
		case MatureState.Free:
			if (curState == MatureState.Product)
			{
				PlayAnim("get");
				PlayAnimQueued("idle");
			}
			else
			{
				PlayAnim("grow");
				PlayAnimQueued("grow");
			}
			break;
		case MatureState.Feed:
			PlayAnim("feed");
			break;
		case MatureState.Product:
			if (curState == MatureState.Feed)
			{
				PlayAnim("product");
				PlayAnimQueued("finish");
			}
			else
			{
				PlayAnim("finish");
			}
			break;
		case MatureState.Kill:
			PlayAnim("kill");
			time = 0f;
			break;
		}
		curState = state;
	}
}
