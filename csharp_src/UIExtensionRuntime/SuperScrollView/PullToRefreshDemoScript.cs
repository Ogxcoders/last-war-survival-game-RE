using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace SuperScrollView;

public class PullToRefreshDemoScript : MonoBehaviour
{
	public LoopListView2 mLoopListView;

	private LoadingTipStatus mLoadingTipStatus;

	private float mDataLoadedTipShowLeftTime;

	private float mLoadingTipItemHeight = 100f;

	private Button mScrollToButton;

	private Button mAddItemButton;

	private Button mSetCountButton;

	private InputField mScrollToInput;

	private InputField mAddItemInput;

	private InputField mSetCountInput;

	private Button mBackButton;

	private void Start()
	{
		mLoopListView.InitListView(DataSourceMgr.Get.TotalItemCount + 1, OnGetItemByIndex);
		mLoopListView.mOnBeginDragAction = OnBeginDrag;
		mLoopListView.mOnDragingAction = OnDraging;
		mLoopListView.mOnEndDragAction = OnEndDrag;
		mSetCountButton = GameObject.Find("ButtonPanel/buttonGroup1/SetCountButton").GetComponent<Button>();
		mScrollToButton = GameObject.Find("ButtonPanel/buttonGroup2/ScrollToButton").GetComponent<Button>();
		mAddItemButton = GameObject.Find("ButtonPanel/buttonGroup3/AddItemButton").GetComponent<Button>();
		mSetCountInput = GameObject.Find("ButtonPanel/buttonGroup1/SetCountInputField").GetComponent<InputField>();
		mScrollToInput = GameObject.Find("ButtonPanel/buttonGroup2/ScrollToInputField").GetComponent<InputField>();
		mAddItemInput = GameObject.Find("ButtonPanel/buttonGroup3/AddItemInputField").GetComponent<InputField>();
		mScrollToButton.onClick.AddListener(OnJumpBtnClicked);
		mAddItemButton.onClick.AddListener(OnAddItemBtnClicked);
		mSetCountButton.onClick.AddListener(OnSetItemCountBtnClicked);
		mBackButton = GameObject.Find("ButtonPanel/BackButton").GetComponent<Button>();
		mBackButton.onClick.AddListener(OnBackBtnClicked);
	}

	private void OnBackBtnClicked()
	{
		SceneManager.LoadScene("Menu");
	}

	private LoopListViewItem2 OnGetItemByIndex(LoopListView2 listView, int index)
	{
		if (index < 0 || index > DataSourceMgr.Get.TotalItemCount)
		{
			return null;
		}
		LoopListViewItem2 loopListViewItem = null;
		if (index == 0)
		{
			loopListViewItem = listView.NewListViewItem("ItemPrefab0");
			UpdateLoadingTip(loopListViewItem);
			return loopListViewItem;
		}
		int num = index - 1;
		ItemData itemDataByIndex = DataSourceMgr.Get.GetItemDataByIndex(num);
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
		component.SetItemData(itemDataByIndex, num);
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
				component.mText.text = "Release to Refresh";
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
			else if (mLoadingTipStatus == LoadingTipStatus.Loaded)
			{
				component.mRoot.SetActive(value: true);
				component.mArrow.SetActive(value: false);
				component.mWaitingIcon.SetActive(value: false);
				component.mText.text = "Refreshed Success";
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
		LoopListViewItem2 shownItemByItemIndex = mLoopListView.GetShownItemByItemIndex(0);
		if (shownItemByItemIndex == null)
		{
			return;
		}
		if (mLoopListView.ScrollRect.content.localPosition.y < 0f - mLoadingTipItemHeight)
		{
			if (mLoadingTipStatus == LoadingTipStatus.None)
			{
				mLoadingTipStatus = LoadingTipStatus.WaitRelease;
				UpdateLoadingTip(shownItemByItemIndex);
				shownItemByItemIndex.CachedRectTransform.localPosition = new Vector3(0f, mLoadingTipItemHeight, 0f);
			}
		}
		else if (mLoadingTipStatus == LoadingTipStatus.WaitRelease)
		{
			mLoadingTipStatus = LoadingTipStatus.None;
			UpdateLoadingTip(shownItemByItemIndex);
			shownItemByItemIndex.CachedRectTransform.localPosition = new Vector3(0f, 0f, 0f);
		}
	}

	private void OnEndDrag(PointerEventData eventData)
	{
		if (mLoopListView.ShownItemCount == 0 || (mLoadingTipStatus != LoadingTipStatus.None && mLoadingTipStatus != LoadingTipStatus.WaitRelease))
		{
			return;
		}
		LoopListViewItem2 shownItemByItemIndex = mLoopListView.GetShownItemByItemIndex(0);
		if (!(shownItemByItemIndex == null))
		{
			mLoopListView.OnItemSizeChanged(shownItemByItemIndex.ItemIndex);
			if (mLoadingTipStatus == LoadingTipStatus.WaitRelease)
			{
				mLoadingTipStatus = LoadingTipStatus.WaitLoad;
				UpdateLoadingTip(shownItemByItemIndex);
				DataSourceMgr.Get.RequestRefreshDataList(OnDataSourceRefreshFinished);
			}
		}
	}

	private void OnDataSourceRefreshFinished()
	{
		if (mLoopListView.ShownItemCount != 0 && mLoadingTipStatus == LoadingTipStatus.WaitLoad)
		{
			mLoadingTipStatus = LoadingTipStatus.Loaded;
			mDataLoadedTipShowLeftTime = 0.7f;
			LoopListViewItem2 shownItemByItemIndex = mLoopListView.GetShownItemByItemIndex(0);
			if (!(shownItemByItemIndex == null))
			{
				UpdateLoadingTip(shownItemByItemIndex);
				mLoopListView.RefreshAllShownItem();
			}
		}
	}

	private void Update()
	{
		if (mLoopListView.ShownItemCount == 0 || mLoadingTipStatus != LoadingTipStatus.Loaded)
		{
			return;
		}
		mDataLoadedTipShowLeftTime -= Time.deltaTime;
		if (mDataLoadedTipShowLeftTime <= 0f)
		{
			mLoadingTipStatus = LoadingTipStatus.None;
			LoopListViewItem2 shownItemByItemIndex = mLoopListView.GetShownItemByItemIndex(0);
			if (!(shownItemByItemIndex == null))
			{
				UpdateLoadingTip(shownItemByItemIndex);
				shownItemByItemIndex.CachedRectTransform.localPosition = new Vector3(0f, 0f - mLoadingTipItemHeight, 0f);
				mLoopListView.OnItemSizeChanged(0);
			}
		}
	}

	private void OnJumpBtnClicked()
	{
		int result = 0;
		if (int.TryParse(mScrollToInput.text, out result) && result >= 0)
		{
			result++;
			mLoopListView.MovePanelToItemIndex(result, 0f);
		}
	}

	private void OnAddItemBtnClicked()
	{
		if (mLoopListView.ItemTotalCount < 0)
		{
			return;
		}
		int result = 0;
		if (int.TryParse(mAddItemInput.text, out result))
		{
			result = mLoopListView.ItemTotalCount + result;
			if (result >= 0 && result <= DataSourceMgr.Get.TotalItemCount + 1)
			{
				mLoopListView.SetListItemCount(result, resetPos: false);
			}
		}
	}

	private void OnSetItemCountBtnClicked()
	{
		int result = 0;
		if (int.TryParse(mSetCountInput.text, out result) && result >= 0 && result <= DataSourceMgr.Get.TotalItemCount)
		{
			result++;
			mLoopListView.SetListItemCount(result, resetPos: false);
		}
	}
}
