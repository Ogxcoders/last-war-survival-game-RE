namespace SuperScrollView;

public class TreeViewItemCountData
{
	public int mTreeItemIndex;

	public int mChildCount;

	public bool mIsExpand = true;

	public int mBeginIndex;

	public int mEndIndex;

	public bool IsChild(int index)
	{
		return index != mBeginIndex;
	}

	public int GetChildIndex(int index)
	{
		if (!IsChild(index))
		{
			return -1;
		}
		return index - mBeginIndex - 1;
	}
}
