using UnityEngine;

public class PotatoesGrowth : WorldResourceItemBase
{
	private const string DaijiAnim = "_chengzhangqi_daiji";

	public override void Init(long bUuid, string modelName)
	{
		base.uuid = bUuid;
		PlayAnimQueued(modelName + "_chengzhangqi_daiji");
	}

	public override void UnInit()
	{
		Object.Destroy(base.transform.gameObject);
	}

	public override void OnUpdateTime()
	{
	}
}
