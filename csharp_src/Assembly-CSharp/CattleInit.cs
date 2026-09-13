using UnityEngine;

public class CattleInit : WorldResourceItemBase
{
	[SerializeField]
	private string[] initAnimQueue;

	public override void Init(long bUuid, string modelName)
	{
		base.uuid = bUuid;
		string[] array = initAnimQueue;
		foreach (string animName in array)
		{
			PlayAnimQueued(animName);
		}
	}

	public override void UnInit()
	{
		Object.Destroy(base.transform.gameObject);
	}

	public override void OnUpdateTime()
	{
	}
}
