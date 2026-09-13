using System.Collections.Generic;
using UnityEngine;

namespace SuperScrollView;

public class ChatItemProvider : LoopListViewItemProvider
{
	[SerializeField]
	private List<ItemPrefabConfData> mItemPrefabDataList = new List<ItemPrefabConfData>();

	public override bool AddItemPool(string itemPrefabName)
	{
		foreach (ItemPrefabConfData mItemPrefabData in mItemPrefabDataList)
		{
			if (!(mItemPrefabData.mItemPrefab == null))
			{
				string value = mItemPrefabData.mItemPrefab.name;
				if (itemPrefabName.StartsWith(value))
				{
					mHost.AddItemPoolByItemProvider(itemPrefabName, mItemPrefabData);
					return true;
				}
			}
		}
		return false;
	}
}
