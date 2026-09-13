using System.Collections.Generic;

namespace SuperScrollView;

public class TreeViewItemData
{
	public string mName;

	public string mIcon;

	private List<ItemData> mChildItemDataList = new List<ItemData>();

	public int ChildCount => mChildItemDataList.Count;

	public void AddChild(ItemData data)
	{
		mChildItemDataList.Add(data);
	}

	public ItemData GetChild(int index)
	{
		if (index < 0 || index >= mChildItemDataList.Count)
		{
			return null;
		}
		return mChildItemDataList[index];
	}
}
