using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace SuperScrollView;

public class PageViewDemoScript : MonoBehaviour
{
	public LoopListView2 mLoopListView;

	private Button mBackButton;

	private int mPageCount = 5;

	public Transform mDotsRootObj;

	private List<DotElem> mDotElemList = new List<DotElem>();

	private void Start()
	{
		InitDots();
		LoopListViewInitParam loopListViewInitParam = LoopListViewInitParam.CopyDefaultInitParam();
		loopListViewInitParam.mSnapVecThreshold = 99999f;
		mLoopListView.mOnBeginDragAction = OnBeginDrag;
		mLoopListView.mOnDragingAction = OnDraging;
		mLoopListView.mOnEndDragAction = OnEndDrag;
		mLoopListView.mOnSnapNearestChanged = OnSnapNearestChanged;
		mLoopListView.InitListView(mPageCount, OnGetItemByIndex, loopListViewInitParam);
		mBackButton = GameObject.Find("ButtonPanel/BackButton").GetComponent<Button>();
		mBackButton.onClick.AddListener(OnBackBtnClicked);
	}

	private void InitDots()
	{
		int childCount = mDotsRootObj.childCount;
		for (int i = 0; i < childCount; i++)
		{
			Transform child = mDotsRootObj.GetChild(i);
			DotElem dotElem = new DotElem();
			dotElem.mDotElemRoot = child.gameObject;
			dotElem.mDotSmall = child.Find("dotSmall").gameObject;
			dotElem.mDotBig = child.Find("dotBig").gameObject;
			ClickEventListener clickEventListener = ClickEventListener.Get(dotElem.mDotElemRoot);
			int index = i;
			clickEventListener.SetClickEventHandler(delegate
			{
				OnDotClicked(index);
			});
			mDotElemList.Add(dotElem);
		}
	}

	private void OnDotClicked(int index)
	{
		int curSnapNearestItemIndex = mLoopListView.CurSnapNearestItemIndex;
		if (curSnapNearestItemIndex >= 0 && curSnapNearestItemIndex < mPageCount && index != curSnapNearestItemIndex)
		{
			if (index > curSnapNearestItemIndex)
			{
				mLoopListView.SetSnapTargetItemIndex(curSnapNearestItemIndex + 1);
			}
			else
			{
				mLoopListView.SetSnapTargetItemIndex(curSnapNearestItemIndex - 1);
			}
		}
	}

	private void UpdateAllDots()
	{
		int curSnapNearestItemIndex = mLoopListView.CurSnapNearestItemIndex;
		if (curSnapNearestItemIndex < 0 || curSnapNearestItemIndex >= mPageCount)
		{
			return;
		}
		int count = mDotElemList.Count;
		if (curSnapNearestItemIndex >= count)
		{
			return;
		}
		for (int i = 0; i < count; i++)
		{
			DotElem dotElem = mDotElemList[i];
			if (i != curSnapNearestItemIndex)
			{
				dotElem.mDotSmall.SetActive(value: true);
				dotElem.mDotBig.SetActive(value: false);
			}
			else
			{
				dotElem.mDotSmall.SetActive(value: false);
				dotElem.mDotBig.SetActive(value: true);
			}
		}
	}

	private void OnSnapNearestChanged(LoopListView2 listView, LoopListViewItem2 item)
	{
		UpdateAllDots();
	}

	private void OnBackBtnClicked()
	{
		SceneManager.LoadScene("Menu");
	}

	private LoopListViewItem2 OnGetItemByIndex(LoopListView2 listView, int pageIndex)
	{
		if (pageIndex < 0 || pageIndex >= mPageCount)
		{
			return null;
		}
		LoopListViewItem2 loopListViewItem = listView.NewListViewItem("ItemPrefab1");
		ListItem14 component = loopListViewItem.GetComponent<ListItem14>();
		if (!loopListViewItem.IsInitHandlerCalled)
		{
			loopListViewItem.IsInitHandlerCalled = true;
			component.Init();
		}
		List<ListItem14Elem> mElemItemList = component.mElemItemList;
		int count = mElemItemList.Count;
		int num = pageIndex * count;
		int i;
		for (i = 0; i < count; i++)
		{
			ItemData itemDataByIndex = DataSourceMgr.Get.GetItemDataByIndex(num + i);
			if (itemDataByIndex == null)
			{
				break;
			}
			ListItem14Elem listItem14Elem = mElemItemList[i];
			listItem14Elem.mRootObj.SetActive(value: true);
			listItem14Elem.mIcon.sprite = ResManager.Get.GetSpriteByName(itemDataByIndex.mIcon);
			listItem14Elem.mName.text = itemDataByIndex.mName;
		}
		if (i < count)
		{
			for (; i < count; i++)
			{
				mElemItemList[i].mRootObj.SetActive(value: false);
			}
		}
		return loopListViewItem;
	}

	private void OnBeginDrag(PointerEventData eventData)
	{
	}

	private void OnDraging(PointerEventData eventData)
	{
	}

	private void OnEndDrag(PointerEventData eventData)
	{
		float x = mLoopListView.ScrollRect.velocity.x;
		int curSnapNearestItemIndex = mLoopListView.CurSnapNearestItemIndex;
		LoopListViewItem2 shownItemByItemIndex = mLoopListView.GetShownItemByItemIndex(curSnapNearestItemIndex);
		if (shownItemByItemIndex == null)
		{
			mLoopListView.ClearSnapData();
			return;
		}
		if (Mathf.Abs(x) < 50f)
		{
			mLoopListView.SetSnapTargetItemIndex(curSnapNearestItemIndex);
			return;
		}
		Vector3 itemCornerPosInViewPort = mLoopListView.GetItemCornerPosInViewPort(shownItemByItemIndex, ItemCornerEnum.LeftTop);
		if (itemCornerPosInViewPort.x > 0f)
		{
			if (x > 0f)
			{
				mLoopListView.SetSnapTargetItemIndex(curSnapNearestItemIndex - 1);
			}
			else
			{
				mLoopListView.SetSnapTargetItemIndex(curSnapNearestItemIndex);
			}
		}
		else if (itemCornerPosInViewPort.x < 0f)
		{
			if (x > 0f)
			{
				mLoopListView.SetSnapTargetItemIndex(curSnapNearestItemIndex);
			}
			else
			{
				mLoopListView.SetSnapTargetItemIndex(curSnapNearestItemIndex + 1);
			}
		}
		else if (x > 0f)
		{
			mLoopListView.SetSnapTargetItemIndex(curSnapNearestItemIndex - 1);
		}
		else
		{
			mLoopListView.SetSnapTargetItemIndex(curSnapNearestItemIndex + 1);
		}
	}
}
