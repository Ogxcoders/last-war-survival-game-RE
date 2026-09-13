using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace SuperScrollView;

public class GridViewDemoScript : MonoBehaviour
{
	public LoopListView2 mLoopListView;

	private Button mScrollToButton;

	private Button mAddItemButton;

	private Button mSetCountButton;

	private InputField mScrollToInput;

	private InputField mAddItemInput;

	private InputField mSetCountInput;

	private Button mBackButton;

	private const int mItemCountPerRow = 3;

	private int mListItemTotalCount;

	private void Start()
	{
		mListItemTotalCount = DataSourceMgr.Get.TotalItemCount;
		int num = mListItemTotalCount / 3;
		if (mListItemTotalCount % 3 > 0)
		{
			num++;
		}
		mLoopListView.InitListView(num, OnGetItemByIndex);
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

	private void SetListItemTotalCount(int count)
	{
		mListItemTotalCount = count;
		if (mListItemTotalCount < 0)
		{
			mListItemTotalCount = 0;
		}
		if (mListItemTotalCount > DataSourceMgr.Get.TotalItemCount)
		{
			mListItemTotalCount = DataSourceMgr.Get.TotalItemCount;
		}
		int num = mListItemTotalCount / 3;
		if (mListItemTotalCount % 3 > 0)
		{
			num++;
		}
		mLoopListView.SetListItemCount(num, resetPos: false);
		mLoopListView.RefreshAllShownItem();
	}

	private LoopListViewItem2 OnGetItemByIndex(LoopListView2 listView, int index)
	{
		if (index < 0)
		{
			return null;
		}
		LoopListViewItem2 loopListViewItem = listView.NewListViewItem("ItemPrefab1");
		ListItem6 component = loopListViewItem.GetComponent<ListItem6>();
		if (!loopListViewItem.IsInitHandlerCalled)
		{
			loopListViewItem.IsInitHandlerCalled = true;
			component.Init();
		}
		for (int i = 0; i < 3; i++)
		{
			int num = index * 3 + i;
			if (num >= mListItemTotalCount)
			{
				component.mItemList[i].gameObject.SetActive(value: false);
				continue;
			}
			ItemData itemDataByIndex = DataSourceMgr.Get.GetItemDataByIndex(num);
			if (itemDataByIndex != null)
			{
				component.mItemList[i].gameObject.SetActive(value: true);
				component.mItemList[i].SetItemData(itemDataByIndex, num);
			}
			else
			{
				component.mItemList[i].gameObject.SetActive(value: false);
			}
		}
		return loopListViewItem;
	}

	private void OnJumpBtnClicked()
	{
		int result = 0;
		if (int.TryParse(mScrollToInput.text, out result))
		{
			if (result < 0)
			{
				result = 0;
			}
			result++;
			int num = result / 3;
			if (result % 3 > 0)
			{
				num++;
			}
			if (num > 0)
			{
				num--;
			}
			mLoopListView.MovePanelToItemIndex(num, 0f);
		}
	}

	private void OnAddItemBtnClicked()
	{
		int result = 0;
		if (int.TryParse(mAddItemInput.text, out result))
		{
			SetListItemTotalCount(mListItemTotalCount + result);
		}
	}

	private void OnSetItemCountBtnClicked()
	{
		int result = 0;
		if (int.TryParse(mSetCountInput.text, out result))
		{
			SetListItemTotalCount(result);
		}
	}
}
