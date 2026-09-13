using UnityEngine;

public class PotatoesMature : WorldResourceItemBase
{
	private const string DaijiAnim = "_chengshuqi_daiji";

	private const string ShougeAnim = "_chengshuqi_shouge";

	private float gatherAnimatorTime;

	private bool hadGather;

	private float time;

	private string modelName;

	public override void Init(long bUuid, string mdelName)
	{
		base.uuid = bUuid;
		modelName = mdelName;
		PlayAnimQueued(modelName + "_chengshuqi_daiji");
		string animName = modelName + "_chengshuqi_shouge";
		gatherAnimatorTime = GetClipLength(0, animName);
		hadGather = false;
	}

	public override void UnInit()
	{
		Object.Destroy(base.transform.gameObject);
	}

	public override void OnUpdateTime()
	{
		if (hadGather)
		{
			time += Time.deltaTime;
			if (time >= gatherAnimatorTime)
			{
				UnInit();
			}
		}
	}

	public void OnFinish()
	{
		hadGather = true;
		Transform child = base.transform.GetChild(0);
		if ((bool)child)
		{
			Transform child2 = child.GetChild(0);
			if ((bool)child2)
			{
				Transform child3 = child2.GetChild(1);
				if ((bool)child3)
				{
					child3.gameObject.SetActive(value: false);
				}
			}
		}
		PlayAnim(modelName + "_chengshuqi_shouge");
	}
}
