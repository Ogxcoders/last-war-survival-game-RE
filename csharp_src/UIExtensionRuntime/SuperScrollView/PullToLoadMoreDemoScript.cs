using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace SuperScrollView;

public class PullToLoadMoreDemoScript : MonoBehaviour
{
	public LoopListView2 mLoopListView;

	private LoadingTipStatus mLoadingTipStatus;

	private float mLoadingTipItemHeight = 100f;

	private int mLoadMoreCount = 20;

	private Button mScrollToButton;

	private InputField mScrollToInput;

	private Button mBackButton;

	private void Start()
	{
		mLoopListView.InitListView(DataSourceMgr.Get.TotalItemCount + 1, OnGetItemByIndex);
		mLoopListView.mOnBeginDragAction = OnBeginDrag;
		mLoopListView.mOnDragingAction = OnDraging;
		mLoopListView.mOnEndDragAction = OnEndDrag;
		mScrollToButton = GameObject.Find("ButtonPanel/buttonGroup2/ScrollToButton").GetComponent<Button>();
		mScrollToInput = GameObject.Find("ButtonPanel/buttonGroup2/ScrollToInputField").GetComponent<InputField>();
		mScrollToButton.onClick.AddListener(OnJumpBtnClicked);
		mBackButton = GameObject.Find("ButtonPanel/BackButton").GetComponent<Button>();
		mBackButton.onClick.AddListener(OnBackBtnClicked);
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
		LoopListViewItem2 loopListViewItem = null;
		if (index == DataSourceMgr.Get.TotalItemCount)
		{
			loopListViewItem = listView.NewListViewItem("ItemPrefab0");
			UpdateLoadingTip(loopListViewItem);
			return loopListViewItem;
		}
		ItemData itemDataByIndex = DataSourceMgr.Get.GetItemDataByIndex(index);
		if (itemDataByIndex == null)
		{
			return null;
		}
		loopListViewItem = listView.NewListViewItem("ItemPrefab1");
		ListItem2 component = loopListViewItem.GetComponent<ListItem2>();
		if (!loopListViewItem.IsInitHandlerCalled)
		{
			loopListViewItem.IsInitHandlerCalled = true;
			component.Init();
		}
		if (index == DataSourceMgr.Get.TotalItemCount - 1)
		{
			loopListViewItem.Padding = 0f;
		}
		component.SetItemData(itemDataByIndex, index);
		return loopListViewItem;
	}

	private void UpdateLoadingTip(LoopListViewItem2 item)
	{
		if (item == null)
		{
			return;
		}
		ListItem0 component = item.GetComponent<ListItem0>();
		if (!(component == null))
		{
			if (mLoadingTipStatus == LoadingTipStatus.None)
			{
				component.mRoot.SetActive(value: false);
				item.CachedRectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, 0f);
			}
			else if (mLoadingTipStatus == LoadingTipStatus.WaitRelease)
			{
				component.mRoot.SetActive(value: true);
				component.mText.text = "Release to Load More";
				component.mArrow.SetActive(value: true);
				component.mWaitingIcon.SetActive(value: false);
				item.CachedRectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, mLoadingTipItemHeight);
			}
			else if (mLoadingTipStatus == LoadingTipStatus.WaitLoad)
			{
				component.mRoot.SetActive(value: true);
				component.mArrow.SetActive(value: false);
				component.mWaitingIcon.SetActive(value: true);
				component.mText.text = "Loading ...";
				item.CachedRectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, mLoadingTipItemHeight);
			}
		}
	}

	private void OnBeginDrag(PointerEventData eventData)
	{
	}

	private void OnDraging(PointerEventData eventData)
	{
		if (mLoopListView.ShownItemCount == 0 || (mLoadingTipStatus != LoadingTipStatus.None && mLoadingTipStatus != LoadingTipStatus.WaitRelease))
		{
			return;
		}
		LoopListViewItem2 shownItemByItemIndex = mLoopListView.GetShownItemByItemIndex(DataSourceMgr.Get.TotalItemCount);
		if (shownItemByItemIndex == null)
		{
			return;
		}
		LoopListViewItem2 shownItemByItemIndex2 = mLoopListView.GetShownItemByItemIndex(DataSourceMgr.Get.TotalItemCount - 1);
		if (shownItemByItemIndex2 == null)
		{
			return;
		}
		if (mLoopListView.GetItemCornerPosInViewPort(shownItemByItemIndex2).y + mLoopListView.ViewPortSize >= mLoadingTipItemHeight)
		{
			if (mLoadingTipStatus == LoadingTipStatus.None)
			{
				mLoadingTipStatus = LoadingTipStatus.WaitRelease;
				UpdateLoadingTip(shownItemByItemIndex);
			}
		}
		else if (mLoadingTipStatus == LoadingTipStatus.WaitRelease)
		{
			mLoadingTipStatus = LoadingTipStatus.None;
			UpdateLoadingTip(shownItemByItemIndex);
		}
	}

	private void OnEndDrag(PointerEventData eventData)
	{
		if (mLoopListView.ShownItemCount == 0 || (mLoadingTipStatus != LoadingTipStatus.None && mLoadingTipStatus != LoadingTipStatus.WaitRelease))
		{
			return;
		}
		LoopListViewItem2 shownItemByItemIndex = mLoopListView.GetShownItemByItemIndex(DataSourceMgr.Get.TotalItemCount);
		if (!(shownItemByItemIndex == null))
		{
			mLoopListView.OnItemSizeChanged(shownItemByItemIndex.ItemIndex);
			if (mLoadingTipStatus == LoadingTipStatus.WaitRelease)
			{
				mLoadingTipStatus = LoadingTipStatus.WaitLoad;
				UpdateLoadingTip(shownItemByItemIndex);
				DataSourceMgr.Get.RequestLoadMoreDataList(mLoadMoreCount, OnDataSourceLoadMoreFinished);
			}
		}
	}

	private void OnDataSourceLoadMoreFinished()
	{
		if (mLoopListView.ShownItemCount != 0 && mLoadingTipStatus == LoadingTipStatus.WaitLoad)
		{
			mLoadingTipStatus = LoadingTipStatus.None;
			mLoopListView.SetListItemCount(DataSourceMgr.Get.TotalItemCount + 1, resetPos: false);
			mLoopListView.RefreshAllShownItem();
		}
	}

	private void OnJumpBtnClicked()
	{
		int result = 0;
		if (int.TryParse(mScrollToInput.text, out result) && result >= 0)
		{
			mLoopListView.MovePanelToItemIndex(result, 0f);
		}
	}
}
