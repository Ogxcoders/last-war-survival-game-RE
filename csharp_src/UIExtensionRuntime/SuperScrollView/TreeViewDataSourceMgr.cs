using System.Collections.Generic;
using UnityEngine;

namespace SuperScrollView;

public class TreeViewDataSourceMgr : MonoBehaviour
{
	private List<TreeViewItemData> mItemDataList = new List<TreeViewItemData>();

	private static TreeViewDataSourceMgr instance;

	private int mTreeViewItemCount = 20;

	private int mTreeViewChildItemCount = 30;

	public static TreeViewDataSourceMgr Get
	{
		get
		{
			if (instance == null)
			{
				instance = Object.FindObjectOfType<TreeViewDataSourceMgr>();
			}
			return instance;
		}
	}

	public int TreeViewItemCount => mItemDataList.Count;

	public int TotalTreeViewItemAndChildCount
	{
		get
		{
			int count = mItemDataList.Count;
			int num = 0;
			for (int i = 0; i < count; i++)
			{
				num += mItemDataList[i].ChildCount;
			}
			return num;
		}
	}

	private void Awake()
	{
		Init();
	}

	public void Init()
	{
		DoRefreshDataSource();
	}

	public TreeViewItemData GetItemDataByIndex(int index)
	{
		if (index < 0 || index >= mItemDataList.Count)
		{
			return null;
		}
		return mItemDataList[index];
	}

	public ItemData GetItemChildDataByIndex(int itemIndex, int childIndex)
	{
		return GetItemDataByIndex(itemIndex)?.GetChild(childIndex);
	}

	private void DoRefreshDataSource()
	{
		mItemDataList.Clear();
		for (int i = 0; i < mTreeViewItemCount; i++)
		{
			TreeViewItemData treeViewItemData = new TreeViewItemData();
			treeViewItemData.mName = "Item" + i;
			treeViewItemData.mIcon = ResManager.Get.GetSpriteNameByIndex(Random.Range(0, 24));
			mItemDataList.Add(treeViewItemData);
			int num = mTreeViewChildItemCount;
			for (int j = 1; j <= num; j++)
			{
				ItemData itemData = new ItemData();
				itemData.mName = "Item" + i + ":Child" + j;
				itemData.mDesc = "Item Desc For " + itemData.mName;
				itemData.mIcon = ResManager.Get.GetSpriteNameByIndex(Random.Range(0, 24));
				itemData.mStarCount = Random.Range(0, 6);
				itemData.mFileSize = Random.Range(20, 999);
				treeViewItemData.AddChild(itemData);
			}
		}
	}
}
