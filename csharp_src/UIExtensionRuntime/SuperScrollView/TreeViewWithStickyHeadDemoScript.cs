using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace SuperScrollView;

public class TreeViewWithStickyHeadDemoScript : MonoBehaviour
{
	public LoopListView2 mLoopListView;

	private Button mScrollToButton;

	private Button mExpandAllButton;

	private Button mCollapseAllButton;

	private InputField mScrollToInputItem;

	private InputField mScrollToInputChild;

	private Button mBackButton;

	private TreeViewItemCountMgr mTreeItemCountMgr = new TreeViewItemCountMgr();

	public ListItem12 mStickeyHeadItem;

	private float mStickeyHeadItemHeight = -1f;

	private void Start()
	{
		int treeViewItemCount = TreeViewDataSourceMgr.Get.TreeViewItemCount;
		for (int i = 0; i < treeViewItemCount; i++)
		{
			int childCount = TreeViewDataSourceMgr.Get.GetItemDataByIndex(i).ChildCount;
			mTreeItemCountMgr.AddTreeItem(childCount, isExpand: true);
		}
		mLoopListView.InitListView(mTreeItemCountMgr.GetTotalItemAndChildCount(), OnGetItemByIndex);
		mExpandAllButton = GameObject.Find("ButtonPanel/buttonGroup1/ExpandAllButton").GetComponent<Button>();
		mScrollToButton = GameObject.Find("ButtonPanel/buttonGroup2/ScrollToButton").GetComponent<Button>();
		mCollapseAllButton = GameObject.Find("ButtonPanel/buttonGroup3/CollapseAllButton").GetComponent<Button>();
		mScrollToInputItem = GameObject.Find("ButtonPanel/buttonGroup2/ScrollToInputFieldItem").GetComponent<InputField>();
		mScrollToInputChild = GameObject.Find("ButtonPanel/buttonGroup2/ScrollToInputFieldChild").GetComponent<InputField>();
		mScrollToButton.onClick.AddListener(OnJumpBtnClicked);
		mBackButton = GameObject.Find("ButtonPanel/BackButton").GetComponent<Button>();
		mBackButton.onClick.AddListener(OnBackBtnClicked);
		mExpandAllButton.onClick.AddListener(OnExpandAllBtnClicked);
		mCollapseAllButton.onClick.AddListener(OnCollapseAllBtnClicked);
		mStickeyHeadItemHeight = mStickeyHeadItem.GetComponent<RectTransform>().rect.height;
		mStickeyHeadItem.Init();
		mStickeyHeadItem.SetClickCallBack(OnExpandClicked);
		mLoopListView.ScrollRect.onValueChanged.AddListener(OnScrollContentPosChanged);
		UpdateStickeyHeadPos();
	}

	private void OnBackBtnClicked()
	{
		SceneManager.LoadScene("Menu");
	}

	private LoopListViewItem2 OnGetItemByIndex(LoopListView2 listView, int index)
	{
		if (index < 0)
		{
			return null;
		}
		TreeViewItemCountData treeViewItemCountData = mTreeItemCountMgr.QueryTreeItemByTotalIndex(index);
		if (treeViewItemCountData == null)
		{
			return null;
		}
		int mTreeItemIndex = treeViewItemCountData.mTreeItemIndex;
		TreeViewItemData itemDataByIndex = TreeViewDataSourceMgr.Get.GetItemDataByIndex(mTreeItemIndex);
		if (!treeViewItemCountData.IsChild(index))
		{
			LoopListViewItem2 loopListViewItem = listView.NewListViewItem("ItemPrefab1");
			ListItem12 component = loopListViewItem.GetComponent<ListItem12>();
			if (!loopListViewItem.IsInitHandlerCalled)
			{
				loopListViewItem.IsInitHandlerCalled = true;
				component.Init();
				component.SetClickCallBack(OnExpandClicked);
			}
			loopListViewItem.UserIntData1 = mTreeItemIndex;
			loopListViewItem.UserIntData2 = 0;
			component.mText.text = itemDataByIndex.mName;
			component.SetItemData(mTreeItemIndex, treeViewItemCountData.mIsExpand);
			return loopListViewItem;
		}
		int childIndex = treeViewItemCountData.GetChildIndex(index);
		ItemData child = itemDataByIndex.GetChild(childIndex);
		if (child == null)
		{
			return null;
		}
		LoopListViewItem2 loopListViewItem2 = listView.NewListViewItem("ItemPrefab2");
		ListItem13 component2 = loopListViewItem2.GetComponent<ListItem13>();
		if (!loopListViewItem2.IsInitHandlerCalled)
		{
			loopListViewItem2.IsInitHandlerCalled = true;
			component2.Init();
		}
		loopListViewItem2.UserIntData1 = mTreeItemIndex;
		loopListViewItem2.UserIntData2 = childIndex;
		component2.SetItemData(child, mTreeItemIndex, childIndex);
		return loopListViewItem2;
	}

	public void OnExpandClicked(int index)
	{
		mTreeItemCountMgr.ToggleItemExpand(index);
		mLoopListView.SetListItemCount(mTreeItemCountMgr.GetTotalItemAndChildCount(), resetPos: false);
		mLoopListView.RefreshAllShownItem();
	}

	private void OnJumpBtnClicked()
	{
		int result = 0;
		int result2 = 0;
		int num = 0;
		if (!int.TryParse(mScrollToInputItem.text, out result))
		{
			return;
		}
		if (!int.TryParse(mScrollToInputChild.text, out result2))
		{
			result2 = 0;
		}
		if (result2 < 0)
		{
			result2 = 0;
		}
		TreeViewItemCountData treeItem = mTreeItemCountMgr.GetTreeItem(result);
		if (treeItem == null)
		{
			return;
		}
		int mChildCount = treeItem.mChildCount;
		if (!treeItem.mIsExpand || mChildCount == 0 || result2 == 0)
		{
			num = treeItem.mBeginIndex;
		}
		else
		{
			if (result2 > mChildCount)
			{
				result2 = mChildCount;
			}
			if (result2 < 1)
			{
				result2 = 1;
			}
			num = treeItem.mBeginIndex + result2;
		}
		mLoopListView.MovePanelToItemIndex(num, mStickeyHeadItemHeight);
	}

	private void OnExpandAllBtnClicked()
	{
		int treeViewItemCount = mTreeItemCountMgr.TreeViewItemCount;
		for (int i = 0; i < treeViewItemCount; i++)
		{
			mTreeItemCountMgr.SetItemExpand(i, isExpand: true);
		}
		mLoopListView.SetListItemCount(mTreeItemCountMgr.GetTotalItemAndChildCount(), resetPos: false);
		mLoopListView.RefreshAllShownItem();
	}

	private void OnCollapseAllBtnClicked()
	{
		int treeViewItemCount = mTreeItemCountMgr.TreeViewItemCount;
		for (int i = 0; i < treeViewItemCount; i++)
		{
			mTreeItemCountMgr.SetItemExpand(i, isExpand: false);
		}
		mLoopListView.SetListItemCount(mTreeItemCountMgr.GetTotalItemAndChildCount(), resetPos: false);
		mLoopListView.RefreshAllShownItem();
	}

	private void UpdateStickeyHeadPos()
	{
		bool activeSelf = mStickeyHeadItem.gameObject.activeSelf;
		int shownItemCount = mLoopListView.ShownItemCount;
		if (shownItemCount == 0)
		{
			if (activeSelf)
			{
				mStickeyHeadItem.gameObject.SetActive(value: false);
			}
			return;
		}
		LoopListViewItem2 shownItemByIndex = mLoopListView.GetShownItemByIndex(0);
		Vector3 itemCornerPosInViewPort = mLoopListView.GetItemCornerPosInViewPort(shownItemByIndex, ItemCornerEnum.LeftTop);
		LoopListViewItem2 loopListViewItem = null;
		float y = itemCornerPosInViewPort.y;
		float num = y - shownItemByIndex.ItemSizeWithPadding;
		int num2 = -1;
		if (y <= 0f)
		{
			if (activeSelf)
			{
				mStickeyHeadItem.gameObject.SetActive(value: false);
			}
			return;
		}
		if (num < 0f)
		{
			loopListViewItem = shownItemByIndex;
			num2 = 0;
		}
		else
		{
			for (int i = 1; i < shownItemCount; i++)
			{
				LoopListViewItem2 shownItemByIndexWithoutCheck = mLoopListView.GetShownItemByIndexWithoutCheck(i);
				float num3 = num;
				num = num3 - shownItemByIndexWithoutCheck.ItemSizeWithPadding;
				if (num3 >= 0f && num <= 0f)
				{
					loopListViewItem = shownItemByIndexWithoutCheck;
					num2 = i;
					break;
				}
			}
		}
		if (loopListViewItem == null)
		{
			if (activeSelf)
			{
				mStickeyHeadItem.gameObject.SetActive(value: false);
			}
			return;
		}
		int userIntData = loopListViewItem.UserIntData1;
		_ = loopListViewItem.UserIntData2;
		TreeViewItemCountData treeItem = mTreeItemCountMgr.GetTreeItem(userIntData);
		if (treeItem == null)
		{
			if (activeSelf)
			{
				mStickeyHeadItem.gameObject.SetActive(value: false);
			}
			return;
		}
		if (!treeItem.mIsExpand || treeItem.mChildCount == 0)
		{
			if (activeSelf)
			{
				mStickeyHeadItem.gameObject.SetActive(value: false);
			}
			return;
		}
		if (!activeSelf)
		{
			mStickeyHeadItem.gameObject.SetActive(value: true);
		}
		if (mStickeyHeadItem.TreeItemIndex != userIntData)
		{
			TreeViewItemData itemDataByIndex = TreeViewDataSourceMgr.Get.GetItemDataByIndex(userIntData);
			mStickeyHeadItem.mText.text = itemDataByIndex.mName;
			mStickeyHeadItem.SetItemData(userIntData, treeItem.mIsExpand);
		}
		mStickeyHeadItem.gameObject.transform.localPosition = Vector3.zero;
		float num4 = 0f - num;
		float padding = loopListViewItem.Padding;
		if (num4 - padding >= mStickeyHeadItemHeight)
		{
			return;
		}
		for (int j = num2 + 1; j < shownItemCount; j++)
		{
			LoopListViewItem2 shownItemByIndexWithoutCheck2 = mLoopListView.GetShownItemByIndexWithoutCheck(j);
			if (shownItemByIndexWithoutCheck2.UserIntData1 != userIntData)
			{
				break;
			}
			num4 += shownItemByIndexWithoutCheck2.ItemSizeWithPadding;
			padding = shownItemByIndexWithoutCheck2.Padding;
			if (num4 - padding >= mStickeyHeadItemHeight)
			{
				return;
			}
		}
		float y2 = mStickeyHeadItemHeight - (num4 - padding);
		mStickeyHeadItem.gameObject.transform.localPosition = new Vector3(0f, y2, 0f);
	}

	private void OnScrollContentPosChanged(Vector2 pos)
	{
		UpdateStickeyHeadPos();
	}
}
