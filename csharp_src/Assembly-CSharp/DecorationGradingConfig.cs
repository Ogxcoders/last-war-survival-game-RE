using System.Collections.Generic;
using UnityEngine;

public class DecorationGradingConfig : MonoBehaviour
{
	[SerializeField]
	private List<DecorationGradingSetting> settings;

	public List<DecorationGradingSetting> Settings => settings;

	[ContextMenu("收集所有FBX节点")]
	private void CollectAllFBXNodes()
	{
	}

	private void GetAllChildren(Transform parent, List<GameObject> childrenList)
	{
		foreach (Transform item in parent)
		{
			if (item.gameObject.activeSelf)
			{
				childrenList.Add(item.gameObject);
				GetAllChildren(item, childrenList);
			}
		}
	}
}
