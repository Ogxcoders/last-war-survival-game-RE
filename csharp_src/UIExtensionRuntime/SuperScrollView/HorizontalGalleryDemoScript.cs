using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace SuperScrollView;

public class HorizontalGalleryDemoScript : MonoBehaviour
{
	public LoopListView2 mLoopListView;

	private Button mScrollToButton;

	private Button mAddItemButton;

	private Button mSetCountButton;

	private InputField mScrollToInput;

	private InputField mAddItemInput;

	private InputField mSetCountInput;

	private Button mBackButton;

	private void Start()
	{
		mLoopListView.InitListView(DataSourceMgr.Get.TotalItemCount, OnGetItemByIndex);
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
		if (index < 0 || index >= DataSourceMgr.Get.TotalItemCount)
		{
			return null;
		}
		ItemData itemDataByIndex = DataSourceMgr.Get.GetItemDataByIndex(index);
		if (itemDataByIndex == null)
		{
			return null;
		}
		LoopListViewItem2 loopListViewItem = listView.NewListViewItem("ItemPrefab1");
		ListItem5 component = loopListViewItem.GetComponent<ListItem5>();
		if (!loopListViewItem.IsInitHandlerCalled)
		{
			loopListViewItem.IsInitHandlerCalled = true;
			component.Init();
		}
		component.SetItemData(itemDataByIndex, index);
		return loopListViewItem;
	}

	private void LateUpdate()
	{
		mLoopListView.UpdateAllShownItemSnapData();
		int shownItemCount = mLoopListView.ShownItemCount;
		for (int i = 0; i < shownItemCount; i++)
		{
			LoopListViewItem2 shownItemByIndex = mLoopListView.GetShownItemByIndex(i);
			ListItem5 component = shownItemByIndex.GetComponent<ListItem5>();
			float value = 1f - Mathf.Abs(shownItemByIndex.DistanceWithViewPortSnapCenter) / 700f;
			value = Mathf.Clamp(value, 0.4f, 1f);
			component.mContentRootObj.GetComponent<CanvasGroup>().alpha = value;
			component.mContentRootObj.transform.localScale = new Vector3(value, value, 1f);
		}
	}

	private void OnJumpBtnClicked()
	{
		int result = 0;
		if (int.TryParse(mScrollToInput.text, out result))
		{
			result -= 2;
			if (result < 0)
			{
				result = 0;
			}
			mLoopListView.MovePanelToItemIndex(result, 0f);
			mLoopListView.FinishSnapImmediately();
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
			if (result >= 0 && result <= DataSourceMgr.Get.TotalItemCount)
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
			mLoopListView.SetListItemCount(result, resetPos: false);
		}
	}
}
