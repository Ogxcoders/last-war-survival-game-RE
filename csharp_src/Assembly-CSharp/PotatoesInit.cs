using UnityEngine;

public class PotatoesInit : WorldResourceItemBase
{
	private const string DaijiAnim = "_youmiaoqi_daiji";

	public override void Init(long bUuid, string modelName)
	{
		base.uuid = bUuid;
		PlayAnimQueued(modelName + "_youmiaoqi_daiji");
	}

	public override void UnInit()
	{
		Object.Destroy(base.transform.gameObject);
	}

	public override void OnUpdateTime()
	{
	}
}
