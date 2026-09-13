using System.Collections.Generic;

namespace SuperScrollView;

public class TreeViewItemCountMgr
{
	private List<TreeViewItemCountData> mTreeItemDataList = new List<TreeViewItemCountData>();

	private TreeViewItemCountData mLastQueryResult;

	private bool mIsDirty = true;

	public int TreeViewItemCount => mTreeItemDataList.Count;

	public void AddTreeItem(int count, bool isExpand)
	{
		TreeViewItemCountData treeViewItemCountData = new TreeViewItemCountData();
		treeViewItemCountData.mTreeItemIndex = mTreeItemDataList.Count;
		treeViewItemCountData.mChildCount = count;
		treeViewItemCountData.mIsExpand = isExpand;
		mTreeItemDataList.Add(treeViewItemCountData);
		mIsDirty = true;
	}

	public void Clear()
	{
		mTreeItemDataList.Clear();
		mLastQueryResult = null;
		mIsDirty = true;
	}

	public TreeViewItemCountData GetTreeItem(int treeIndex)
	{
		if (treeIndex < 0 || treeIndex >= mTreeItemDataList.Count)
		{
			return null;
		}
		return mTreeItemDataList[treeIndex];
	}

	public void SetItemChildCount(int treeIndex, int count)
	{
		if (treeIndex >= 0 && treeIndex < mTreeItemDataList.Count)
		{
			mIsDirty = true;
			mTreeItemDataList[treeIndex].mChildCount = count;
		}
	}

	public void SetItemExpand(int treeIndex, bool isExpand)
	{
		if (treeIndex >= 0 && treeIndex < mTreeItemDataList.Count)
		{
			mIsDirty = true;
			mTreeItemDataList[treeIndex].mIsExpand = isExpand;
		}
	}

	public void ToggleItemExpand(int treeIndex)
	{
		if (treeIndex >= 0 && treeIndex < mTreeItemDataList.Count)
		{
			mIsDirty = true;
			TreeViewItemCountData treeViewItemCountData = mTreeItemDataList[treeIndex];
			treeViewItemCountData.mIsExpand = !treeViewItemCountData.mIsExpand;
		}
	}

	public bool IsTreeItemExpand(int treeIndex)
	{
		return GetTreeItem(treeIndex)?.mIsExpand ?? false;
	}

	private void UpdateAllTreeItemDataIndex()
	{
		if (!mIsDirty)
		{
			return;
		}
		mLastQueryResult = null;
		mIsDirty = false;
		int count = mTreeItemDataList.Count;
		if (count != 0)
		{
			TreeViewItemCountData treeViewItemCountData = mTreeItemDataList[0];
			treeViewItemCountData.mBeginIndex = 0;
			treeViewItemCountData.mEndIndex = (treeViewItemCountData.mIsExpand ? treeViewItemCountData.mChildCount : 0);
			int mEndIndex = treeViewItemCountData.mEndIndex;
			for (int i = 1; i < count; i++)
			{
				TreeViewItemCountData treeViewItemCountData2 = mTreeItemDataList[i];
				treeViewItemCountData2.mBeginIndex = mEndIndex + 1;
				treeViewItemCountData2.mEndIndex = treeViewItemCountData2.mBeginIndex + (treeViewItemCountData2.mIsExpand ? treeViewItemCountData2.mChildCount : 0);
				mEndIndex = treeViewItemCountData2.mEndIndex;
			}
		}
	}

	public int GetTotalItemAndChildCount()
	{
		int count = mTreeItemDataList.Count;
		if (count == 0)
		{
			return 0;
		}
		UpdateAllTreeItemDataIndex();
		return mTreeItemDataList[count - 1].mEndIndex + 1;
	}

	public TreeViewItemCountData QueryTreeItemByTotalIndex(int totalIndex)
	{
		if (totalIndex < 0)
		{
			return null;
		}
		int count = mTreeItemDataList.Count;
		if (count == 0)
		{
			return null;
		}
		UpdateAllTreeItemDataIndex();
		if (mLastQueryResult != null && mLastQueryResult.mBeginIndex <= totalIndex && mLastQueryResult.mEndIndex >= totalIndex)
		{
			return mLastQueryResult;
		}
		int num = 0;
		int num2 = count - 1;
		TreeViewItemCountData treeViewItemCountData = null;
		while (num <= num2)
		{
			int num3 = (num + num2) / 2;
			treeViewItemCountData = mTreeItemDataList[num3];
			if (treeViewItemCountData.mBeginIndex <= totalIndex && treeViewItemCountData.mEndIndex >= totalIndex)
			{
				mLastQueryResult = treeViewItemCountData;
				return treeViewItemCountData;
			}
			if (totalIndex > treeViewItemCountData.mEndIndex)
			{
				num = num3 + 1;
			}
			else
			{
				num2 = num3 - 1;
			}
		}
		return null;
	}
}
