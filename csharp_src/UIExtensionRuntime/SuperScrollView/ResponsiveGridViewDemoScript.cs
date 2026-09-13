using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace SuperScrollView;

public class ResponsiveGridViewDemoScript : MonoBehaviour
{
	public LoopListView2 mLoopListView;

	private Button mScrollToButton;

	private Button mAddItemButton;

	private Button mSetCountButton;

	private InputField mScrollToInput;

	private InputField mAddItemInput;

	private InputField mSetCountInput;

	private Button mBackButton;

	private int mItemCountPerRow = 3;

	private int mListItemTotalCount;

	public DragChangSizeScript mDragChangSizeScript;

	private void Start()
	{
		mListItemTotalCount = DataSourceMgr.Get.TotalItemCount;
		int num = mListItemTotalCount / mItemCountPerRow;
		if (mListItemTotalCount % mItemCountPerRow > 0)
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
		mDragChangSizeScript.mOnDragEndAction = OnViewPortSizeChanged;
		OnViewPortSizeChanged();
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
		int num = mListItemTotalCount / mItemCountPerRow;
		if (mListItemTotalCount % mItemCountPerRow > 0)
		{
			num++;
		}
		mLoopListView.SetListItemCount(num, resetPos: false);
		mLoopListView.RefreshAllShownItem();
	}

	private void UpdateItemPrefab()
	{
		GameObject mItemPrefab = mLoopListView.GetItemPrefabConfData("ItemPrefab1").mItemPrefab;
		RectTransform component = mItemPrefab.GetComponent<RectTransform>();
		ListItem6 component2 = mItemPrefab.GetComponent<ListItem6>();
		float viewPortWidth = mLoopListView.ViewPortWidth;
		int count = component2.mItemList.Count;
		GameObject gameObject = component2.mItemList[0].gameObject;
		float width = gameObject.GetComponent<RectTransform>().rect.width;
		int num = Mathf.FloorToInt(viewPortWidth / width);
		if (num == 0)
		{
			num = 1;
		}
		mItemCountPerRow = num;
		float num2 = (viewPortWidth - width * (float)num) / (float)(num + 1);
		if (num2 < 0f)
		{
			num2 = 0f;
		}
		component.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, viewPortWidth);
		if (num > count)
		{
			int num3 = num - count;
			for (int i = 0; i < num3; i++)
			{
				GameObject obj = Object.Instantiate(gameObject, Vector3.zero, Quaternion.identity, component);
				RectTransform component3 = obj.GetComponent<RectTransform>();
				component3.localScale = Vector3.one;
				component3.localPosition = Vector3.zero;
				component3.rotation = Quaternion.identity;
				ListItem5 component4 = obj.GetComponent<ListItem5>();
				component2.mItemList.Add(component4);
			}
		}
		else if (num < count)
		{
			int num4 = count - num;
			for (int j = 0; j < num4; j++)
			{
				ListItem5 listItem = component2.mItemList[component2.mItemList.Count - 1];
				component2.mItemList.RemoveAt(component2.mItemList.Count - 1);
				Object.DestroyImmediate(listItem.gameObject);
			}
		}
		float num5 = num2;
		for (int k = 0; k < component2.mItemList.Count; k++)
		{
			component2.mItemList[k].gameObject.transform.localPosition = new Vector3(num5, 0f, 0f);
			num5 = num5 + width + num2;
		}
		mLoopListView.OnItemPrefabChanged("ItemPrefab1");
	}

	private void OnViewPortSizeChanged()
	{
		UpdateItemPrefab();
		SetListItemTotalCount(mListItemTotalCount);
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
		for (int i = 0; i < mItemCountPerRow; i++)
		{
			int num = index * mItemCountPerRow + i;
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
			int num = result / mItemCountPerRow;
			if (result % mItemCountPerRow > 0)
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
